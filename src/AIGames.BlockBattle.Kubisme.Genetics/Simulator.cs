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

			Threshold = 1024;
			ResultCount = 16;
			GenerateCount = 3;
			Runs = 23;
			MaximumTurns = 1000;
			Results = new List<SimulationResult<SimpleEvaluator.Parameters>>();
#if DEBUG
			LogIndividualSimulations = true;
#else
			//LogIndividualSimulations = true;
#endif
		}

		public bool LogIndividualSimulations { get; set; }
		public int Threshold { get; set; }
		public int GenerateCount { get; set; }
		public int ResultCount { get; set; }
		public int MaximumTurns { get; set; }

		public int MaximumScore { get; protected set; }
		public int Simulations { get; protected set; }
		public int Runs { get; protected set; }

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

					double scores = Simulate(pars, Runs, 0);
					var score = scores / Runs;
					if (score >= BestResult.Score)
					{
						var result = new SimulationResult<SimpleEvaluator.Parameters>()
						{
							Scores = (int)scores,
							Simulations = Runs,
							Pars = pars,
						};
						Results.Insert(1, result);
						break;
					}
					LogStatus(false);
				}
			}
		}

		private void FitResults()
		{
			var best = BestResult.Score;
			Results.Sort();
			if (Results.Count > ResultCount)
			{
				Results.RemoveRange(ResultCount, Results.Count - ResultCount);
			}

			var runs = Math.Min(Threshold, Results.Max(r => r.Simulations)) - BestResult.Simulations;
			BestResult.Scores += Simulate(Results[0].Pars, runs, -1);
			BestResult.Simulations += runs;
			Results.Sort();

			LogStatus(BestResult.Score != best);
		}

		private void LogStatus(bool newLine)
		{
			var line = String.Format("\r{0:#,#00}  {1} {2:#,##0.00}, Max: {3}   ",
				Simulations,
				sw.Elapsed,
				BestResult.Score,
				MaximumScore);

			Console.Write(line);
			if (newLine)
			{
				using( var writer = new StreamWriter("parameters.cs", false))
				{
					writer.Write("// ");
					writer.WriteLine(line.Trim());
					writer.WriteLine(BestResult.ToString());
				}

				Console.WriteLine();
			}
		}

		private int Simulate(SimpleEvaluator.Parameters pars, int simulations, int minscore)
		{
			var score = 0;
			var current = Block.All[Rnd.Next(7)];
			var next = Block.All[Rnd.Next(7)];

			((SimpleEvaluator)DecisionMaker.Evaluator).pars = pars;

			for (var i = 0; i < simulations; i++)
			{
				var field = Field.Empty;
				
				var sx = Stopwatch.StartNew();
				var t = 0;
				while (t++ < MaximumTurns)
				{

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
					}
					if (path.Target.Equals(Position.Start))
					{
						var s = field.Points;

						if (s < minscore) 
						{
							Simulations += i;
							return int.MinValue;
						}
						if (s > MaximumScore)
						{
							MaximumScore = s;
						}
						score += s;
						break;
					}
					field = field.Apply(current[path.Option], path.Target);
					current = next;
					next = Block.All[Rnd.Next(7)];
				}
			}
			Simulations += simulations;
			return score;
		}

		private static readonly Field Small = Field.Create(0, 0, @"
..........
..........
..........
..........
..........");
	}
}
