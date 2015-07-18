using AIGames.BlockBattle.Kubisme.DecisionMaking;
using AIGames.BlockBattle.Kubisme.Evaluation;
using AIGames.BlockBattle.Kubisme.Genetics.DecisionMaking;
using AIGames.BlockBattle.Kubisme.Genetics.Models;
using AIGames.BlockBattle.Kubisme.Genetics.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class Simulator : IComparer<SimulationResult<SimpleParameters>>
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

			ResultCount = 32;
			GenerateCount = 8;
			RunsTest = 10;
			RunsRetry = 100;
			RunsMax = 10000;
			Results = new List<SimulationResult<SimpleParameters>>();
			File = new FileInfo("parameters.xml");
		}

		public FileInfo File { get; set; }
		public int GenerateCount { get; set; }
		public int ResultCount { get; set; }

		public SimScore MaximumScore { get; protected set; }
		public int Simulations { get; protected set; }
		protected int LastId { get; set; }
		public int RunsTest { get; set; }
		public int RunsRetry { get; set; }
		public int RunsMax { get; set; }

		public MT19937Generator Rnd { get; protected set; }
		public ParameterRandomizer Randomizer { get; protected set; }
		public IDecisionMaker DecisionMaker { get; protected set; }
		public List<SimulationResult<SimpleParameters>> Results { get; protected set; }
		public SimulationResult<SimpleParameters> BestResult { get { return Results[0]; } }

		public void Run()
		{
			var collection = new SimpleParametersCollection();

			if (File.Exists)
			{
				collection = SimpleParametersCollection.Load(File);
			}
			AddInitialResult(SimpleParameters.GetDefault());

			Run(collection);
		}

		public void Run(IEnumerable<SimpleParameters> collection)
		{
			foreach (var parameters in collection)
			{
				AddInitialResult(parameters);
			}

			var queue = new Queue<SimpleParameters>();

			while (true)
			{
				FitResults();

				if (queue.Count == 0)
				{
					for (var i = 0; i < GenerateCount; i++)
					{
						if (Results.Count <= i) { break; }
						Randomizer.Generate<SimpleParameters>(Results[i].Pars, queue, GenerateCount >> i);
					}
				}

				var count = queue.Count;

				// Lets empty the queue
				for (var c = 0; c < count; c++)
				{
					var pars = queue.Dequeue();

					var result = new SimulationResult<SimpleParameters>()
					{
						Pars = pars,
						Id = ++LastId,
					};
					var runs = RunsTest;
					Simulate(result, RunsTest);

					var compare = result.Scores.CompareTo(BestResult.Scores);

					if (compare <= 0)
					{
						while (result.Simulations < RunsRetry)
						{
							runs <<= 1;
							Simulate(result, runs);
							compare = result.Scores.CompareTo(BestResult.Scores);
							if (compare >= 0)
							{
								break;
							}
						}
						compare = result.Scores.CompareTo(BestResult.Scores);
						if (compare < 0)
						{
							if (result.Scores.Score > 0)
							{
							}
							queue.Clear();
						}
						Results.Add(result);
						break;
					}
					LogStatus(false);
				}
			}
		}

		private void AddInitialResult(SimpleParameters parameters)
		{
			var res = new SimulationResult<SimpleParameters>()
			{
				Pars = parameters,
				Id = ++LastId,
			};
			Results.Add(res);
			Simulate(res, RunsRetry >> 2);
		}

		private void FitResults()
		{
			var best = BestResult.Id;
			Results.Sort(this);
			if (Results.Count > ResultCount)
			{
				Results.RemoveRange(ResultCount, Results.Count - ResultCount);
			}
			if (Results[0].Scores.Count < RunsMax)
			{
				Simulate(Results[0], RunsTest);
				Results.Sort(this);
			}
			LogStatus(BestResult.Id != best);

			if (BestResult.Id != best)
			{
				var collection = new SimpleParametersCollection();
				collection.AddRange(Results.Select(res => res.Pars));
				collection.Save(File);
			}
#if DEBUG
			foreach (var result in Results)
			{
				result.Scores.Sort();
			}
#endif
		}

		private void LogStatus(bool newLine)
		{
			var line = String.Format("\r{0:#,#00}  {1:d\\.hh\\:mm\\:ss} {2}, ID: {3}  ",
				Simulations,
				sw.Elapsed,
				BestResult.DebuggerDisplay,
				BestResult.Id);

			Console.Write(line);
			if (newLine)
			{
				using (var writer = new StreamWriter("parameters.cs", false))
				{
					writer.Write("// ");
					writer.WriteLine(line.Trim());
					writer.WriteLine(BestResult);
					foreach (var result in Results.Skip(1))
					{
						writer.WriteLine("// {0:#,##0.00}, ID {1}", result.DebuggerDisplay, result.Id);
						writer.WriteLine(result);
					}
				}

				Console.WriteLine();
			}
		}

		private void Simulate(SimulationResult<SimpleParameters> result, int simulations)
		{
			DecisionMaker.Evaluator.Parameters = result.Pars;

			for (var i = 0; i < simulations; i++)
			{
				var game = new GameSimulation()
				{
					Rnd = Rnd,
					Profile = new DefaultOpponentProfile(),
					DecisionMaker = DecisionMaker,
				};
				var score = game.Run();
				result.Scores.Add(score);
				Simulations++;
				if (score.CompareTo(MaximumScore) < 0)
				{
					MaximumScore = score;
				}

				if (i % 5 == 0)
				{
					LogStatus(false);
				}
			}
		}

		public int Compare(SimulationResult<SimpleParameters> l, SimulationResult<SimpleParameters> r)
		{
			var ls = l.Scores;
			var rs = r.Scores;

			var compare = rs.Score.CompareTo(ls.Score);
			if (compare != 0) { return compare; }
			
			compare = ls.WinningLength.CompareTo(rs.WinningLength);
			if (compare != 0) { return compare; }

			compare = rs.LosingLength.CompareTo(ls.LosingLength);
			return compare;
		}
	}
}
