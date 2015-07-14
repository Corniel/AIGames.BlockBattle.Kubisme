using AIGames.BlockBattle.Kubisme.DecisionMaking;
using AIGames.BlockBattle.Kubisme.Evaluation;
using AIGames.BlockBattle.Kubisme.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class Simulator : IComparer<SimulationResult<SimpleEvaluator.Parameters>>
	{
		private Stopwatch sw = Stopwatch.StartNew();

		public Simulator(MT19937Generator rnd)
		{
			Rnd = rnd;
			Randomizer = new ParameterRandomizer(rnd);
			DecisionMaker = new DecisionMaker()
			{
				Evaluator = new SimpleEvaluator(),
				Generator = new MoveGenerator(),
			};

			Threshold = 0.975;
			ResultCount = 32;
			GenerateCount = 8;
			RunsTest = 10;
			RunsRetry = 500;
			Results = new List<SimulationResult<SimpleEvaluator.Parameters>>();
			LastId = 1;
#if DEBUG
			LogIndividualSimulations = true;
			LogDir = new DirectoryInfo(@"C:\Code\AIGames.BlockBattle.Kubisme\img");
			ImageLogger = new FieldVisualizer(16);
#else
			//LogIndividualSimulations = true;
#endif
		}

		public bool LogIndividualSimulations { get; set; }
		public DirectoryInfo LogDir { get; set; }
		public FieldVisualizer ImageLogger { get; set; }

		public double Threshold { get; set; }
		public int GenerateCount { get; set; }
		public int ResultCount { get; set; }

		public SimScore MaximumScore { get; protected set; }
		public int Simulations { get; protected set; }
		protected int LastId { get; set; }
		public int RunsTest { get; set; }
		public int RunsRetry { get; set; }

		public MT19937Generator Rnd { get; protected set; }
		public ParameterRandomizer Randomizer { get; protected set; }
		public DecisionMaker DecisionMaker { get; protected set; }
		public List<SimulationResult<SimpleEvaluator.Parameters>> Results { get; protected set; }
		public SimulationResult<SimpleEvaluator.Parameters> BestResult { get { return Results[0]; } }

		public void Run()
		{
			Results.Add(new SimulationResult<SimpleEvaluator.Parameters>()
			{
				Pars = SimpleEvaluator.Parameters.GetDefault(),
				Id = ++LastId,
			});
			Simulate(Results[0], RunsRetry, 1);

			var queue = new Queue<SimpleEvaluator.Parameters>();

			while (true)
			{
				FitResults();

				if (queue.Count == 0)
				{
					Randomizer.Generate<SimpleEvaluator.Parameters>(BestResult.Pars, queue, GenerateCount);
				}

				var count = queue.Count;

				// Lets empty the queue
				for (var c = 0; c < count; c++)
				{
					var pars = queue.Dequeue();

					var result = new SimulationResult<SimpleEvaluator.Parameters>()
					{
						Pars = pars,
					};
					Simulate(result, RunsTest, 0);
					if (result.Score >= BestResult.Score)
					{
						Simulate(result, RunsRetry, Threshold);
						if (result.Simulations >= RunsRetry * Threshold)
						{
							result.Id = ++LastId;
							Results.Add(result);
						}
						break;
					}
					LogStatus(false);
				}
			}
		}

		private void FitResults()
		{
			var best = BestResult.Id;
			Results.Sort(this);
			if (Results.Count > ResultCount)
			{
				Results.RemoveRange(ResultCount, Results.Count - ResultCount);
			}
			Simulate(Results[0], RunsTest, 0);
			Results.Sort(this);

			LogStatus(BestResult.Id != best);

#if DEBUG
			foreach (var result in Results)
			{
				result.Scores.Sort();
			}
#endif
		}
	
		private void LogStatus(bool newLine)
		{
			var line = String.Format("\r{0:#,#00}  {1:d\\.hh\\:mm\\:ss} {2}, ID: {3}, Max: {4}   ",
				Simulations,
				sw.Elapsed,
				BestResult.DebuggerDisplay,
				BestResult.Id,
				MaximumScore);

			Console.Write(line);
			if (newLine)
			{
				using (var writer = new StreamWriter("parameters.cs", false))
				{
					writer.Write("// ");
					writer.WriteLine(line.Trim());
					writer.WriteLine(BestResult);
					foreach (var result in Results.Skip(1).Take(7))
					{
						writer.WriteLine("// {0:#,##0.00}, ID {1}", result.DebuggerDisplay, result.Id);
						writer.WriteLine(result);
					}
				}

				Console.WriteLine();
			}
		}

		private void Simulate(SimulationResult<SimpleEvaluator.Parameters> result, int simulations, double threshold)
		{
			((SimpleEvaluator)DecisionMaker.Evaluator).pars = result.Pars;

			for (var i = 0; i < simulations; i++)
			{
				Simulate(result);

				if ((i & 15) == 0)
				{
					LogStatus(false);
				}
				if (result.Score <= BestResult.Score * threshold) { return; }
			}
		}

		private void Simulate(SimulationResult<SimpleEvaluator.Parameters> result)
		{
			var current = Block.All[Rnd.Next(7)];
			var next = Block.All[Rnd.Next(7)];

			var field = Field.Empty;
			var sx = Stopwatch.StartNew();
			var turn = 0;
			var running = true;
			var blocks = 0;
			var points = 0;
			var combo = 0;

			while (running)
			{
				turn++;

				var path = DecisionMaker.GetMove(field, Position.Start, current, next);

				if (LogIndividualSimulations)
				{
					Console.WriteLine("{0,4}  {1,3} ({2})  {3:0.0}ms/t  {4,2}", turn, field.Points, field.Combo, sx.Elapsed.TotalMilliseconds / turn, field.FirstNoneEmptyRow);
					ImageLogger.Draw(field, LogDir, turn - 1);
				}
				if (path.Equals(MovePath.None))
				{
					running = false;
				}
				else
				{
					var old = field;
					field = field.Apply(current[path.Option], path.Target);

					if (field.Points >= SimScore.WinningScore)
					{
						running = false;
					}
					if (LogIndividualSimulations)
					{
#if DEBUG
						var blocksTest = field.Count;
						var pointsTest = field.Points;
						var rows = pointsTest - points;
						if (rows > 0) { rows -= combo; }
						rows = Math.Min(4, rows);
						var blocksExp = blocks + 4 - 10 * rows;
						if (blocksExp != blocksTest)
						{
						}
						points = pointsTest;
						blocks = blocksTest;
						combo = field.Combo;
#endif
					}

					current = next;
					next = Block.All[Rnd.Next(7)];
					
					if (turn % 10 == 0)
					{
						field = field.LockRows(1);
						running &= field.RowCount > 0;
					}
				}
			}
			var score = new SimScore(turn, field.Points);

			if (score.CompareTo(MaximumScore) < 0)
			{
				MaximumScore = score;
			}
			if (LogIndividualSimulations)
			{
				Console.ReadLine();
			}
			Simulations++;
			result.Scores.Add(score);
		}

		public int Compare(SimulationResult<SimpleEvaluator.Parameters> l, SimulationResult<SimpleEvaluator.Parameters> r)
		{
			var compare = (r.Simulations >= RunsRetry).CompareTo(l.Simulations >= RunsRetry);
			if (compare != 0) { return compare; }
			compare = r.Score.CompareTo(l.Score);
			if (compare != 0) { return compare; }
			compare = l.WinningLength.CompareTo(r.WinningLength);
			if (compare != 0) { return compare; }
			compare = r.LosingLength.CompareTo(l.LosingLength);
			return compare;
		}
	}
}
