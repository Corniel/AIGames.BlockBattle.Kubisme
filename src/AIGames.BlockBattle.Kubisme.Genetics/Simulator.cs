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
	public class Simulator
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
			GenerateCount = 3;
			RunsTest = 10;
			RunsRetry = 1000;
			Results = new List<SimulationResult<SimpleEvaluator.Parameters>>();
			LastId = 1;
#if DEBUG
			LogIndividualSimulations = true;
			LogDir = new DirectoryInfo(@"D:\Code\AIGames.BlockBattle.Kubisme\img");
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

		public int MaximumScore { get; protected set; }
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
			});

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
						Simulate(result, RunsRetry - RunsTest, Threshold);
						result.Id = ++LastId;
						Results.Insert(1, result);
						break;
					}
					LogStatus(false);
				}
			}
		}

		private void FitResults()
		{
			var best = BestResult.Id;
			Results.Sort();
			if (Results.Count > ResultCount)
			{
				Results.RemoveRange(ResultCount, Results.Count - ResultCount);
			}

			Simulate(Results[0], RunsTest, 0);
			Results.Sort();

			LogStatus(BestResult.Id != best);
		}

		private void LogStatus(bool newLine)
		{
			var line = String.Format("\r{0:#,#00}  {1:#,##0}:{2:00} {3}, ID: {4}, Max: {5}   ",
				Simulations,
				sw.Elapsed.TotalMinutes,
				sw.Elapsed.Seconds,
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
			var current = Block.All[Rnd.Next(7)];
			var next = Block.All[Rnd.Next(7)];

			((SimpleEvaluator)DecisionMaker.Evaluator).pars = result.Pars;

			for (var i = 0; i < simulations; i++)
			{
				var field = Field.Empty;
				var sx = Stopwatch.StartNew();
				var t = 0;
				var running = true;
				var blocks = 0;
				var points = 0;
				var combo = 0;
				while (running)
				{
					t++;

					var path = DecisionMaker.GetMove(field, Position.Start, current, next);
					if (LogIndividualSimulations)
					{
						Console.WriteLine(
							"{0,4}  {1,3} ({2})  {3:0.0}ms/t  {4,2}",
							t,
							field.Points,
							field.Combo,
							sx.Elapsed.TotalMilliseconds / t,
							field.FirstNoneEmptyRow);

						ImageLogger.Draw(field, LogDir, t - 1);
					}
					if (path.Equals(MovePath.None) || field.Points >= 80)
					{
						running = false;
					}
					else
					{
						var old = field;
						field = field.Apply(current[path.Option], path.Target);
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

						current = next;
						next = Block.All[Rnd.Next(7)];
						if (t % 10 == 0)
						{
							field = field.LockRows(1);
							running &= field.RowCount > 0;
						}
					}
				}
				var s = field.Points;

				if (s > MaximumScore)
				{
					MaximumScore = s;
				} 
				if (LogIndividualSimulations)
				{
					Console.ReadLine();
				}
				Simulations++;
				result.Scores.Add(SimScore.Create(t, s));
				if (result.Score < BestResult.Score * Threshold) { return; }
			}
		}

		private static readonly Field Small = Field.Create(0, 0, @"
..........
..........
..........
..........
..........");
	}
}
