using AIGames.BlockBattle.Kubisme.DecisionMaking;
using AIGames.BlockBattle.Kubisme.Evaluation;
using AIGames.BlockBattle.Kubisme.Genetics.DecisionMaking;
using AIGames.BlockBattle.Kubisme.Genetics.Models;
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
			DecisionMaker = new SimpleDecisionMaker()
			{
				Evaluator = new SimpleEvaluator(),
				Generator = new MoveGenerator(),
			};

			Threshold = 0.975;
			ResultCount = 32;
			GenerateCount = 8;
			RunsTest = 10;
			RunsRetry = 1000;
			Results = new List<SimulationResult<SimpleEvaluator.Parameters>>();
		}

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
		public SimpleDecisionMaker DecisionMaker { get; protected set; }
		public List<SimulationResult<SimpleEvaluator.Parameters>> Results { get; protected set; }
		public SimulationResult<SimpleEvaluator.Parameters> BestResult { get { return Results[0]; } }

		public void Run()
		{
			Results.Add(new SimulationResult<SimpleEvaluator.Parameters>()
			{
				Pars = SimpleEvaluator.Parameters.GetDefault(),
				Id = ++LastId,
			});
			Simulate(Results[0], RunsRetry >> 2, 0);

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
						Id = ++LastId,
					};
					Simulate(result, RunsTest, 0);
					if (result.Score >= BestResult.Score)
					{
						Simulate(result, RunsRetry, Threshold);
						if (result.Simulations >= RunsRetry * Threshold)
						{
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
				var game = new GameSimulation()
				{
					Rnd = Rnd,
					Profile = new OpponentProfile(),
					DecisionMaker = DecisionMaker,
				};
				var score = game.Run();
				result.Scores.Add(score);
				Simulations++;
				if (score.CompareTo(MaximumScore) < 0)
				{
					MaximumScore = score;
				}

				if ((i & 15) == 0)
				{
					LogStatus(false);
				}
				if (result.Score < BestResult.Score * threshold) { return; }
			}
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
