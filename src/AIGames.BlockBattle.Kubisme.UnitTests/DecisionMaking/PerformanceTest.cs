using AIGames.BlockBattle.Kubisme.Genetics;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.UnitTests.DecisionMaking
{
	[TestFixture]
	public class PerformanceTest
	{
		public const int Runs = 10;

		[Test]
		public void Run_NodeDecisionMaker_92procent()
		{
			var dm = new NodeDecisionMaker()
			{
				Evaluator = new SimpleEvaluator()
				{
					Parameters = new SimpleParameters()
					// 4.565.153  0.18:26:11 Score: 31,56%, Win: 66,2, Lose: 58,9 Runs: 11.763, ID: 194242
					{
						FreeCellWeights = new int[] { -325, -258, -193, -122, -2, -1, -1, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 4 },
						ComboPotential = new int[] { 0, 56, 60, 64, 70, 95, 100, 80, 90, 70, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
						Points = 96,
						Combo = 16,
						Holes = -82,
						Blockades = -9,
						WallsLeft = 26,
						WallsRight = 27,
						Floor = -7,
						NeighborsHorizontal = -15,
						NeighborsVertical = 28,
					}
				},
				Points = new int[10],
				Generator = new MoveGenerator(),
				MaximumDuration = TimeSpan.FromMilliseconds(50),
				MaximumDepth = 4,
			};
			TestSimulation(dm, Runs);
		}

		[Test]
		public void Run_SimpleDecisionMaker_62procent()
		{
			var dm = new SimpleDecisionMaker()
			{
				Evaluator = new SimpleEvaluator()
				{
					Parameters = new SimpleParameters()
					// 4.565.153  0.18:26:11 Score: 31,56%, Win: 66,2, Lose: 58,9 Runs: 11.763, ID: 194242
					{
						FreeCellWeights = new int[] { -325, -258, -193, -122, -2, -1, -1, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 4 },
						ComboPotential = new int[] { 0, 56, 60, 64, 70, 95, 100, 80, 90, 70, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
						Points = 96,
						Combo = 16,
						Holes = -82,
						Blockades = -9,
						WallsLeft = 26,
						WallsRight = 27,
						Floor = -7,
						NeighborsHorizontal = -15,
						NeighborsVertical = 28,
					},
				},
				Generator = new MoveGenerator(),
			};
			TestSimulation(dm, Runs);
		}
		[Test]
		public void Run_SimpleDecisionMakerWithRowCountWeights_67procent()
		{
			var dm = new SimpleDecisionMaker()
			{
				Evaluator = new SimpleEvaluator()
				{
					Parameters = new SimpleParameters()
					{
						FreeCellWeights = new int[] { -449, -401, -130, -149, -68, -48, -20, 6, 10, 20, 7, 10, 13, 35, 9, 37, 10, 8, 28, -6, 45 },
						Points = 2450,
						Combo = 990,
						Holes = -1391,
						Blockades = -1207,
						WallsLeft = 946,
						WallsRight = 162,
						Floor = -544,
						NeighborsHorizontal = -757,
						NeighborsVertical = 1011,
					},
				},
				Generator = new MoveGenerator(),
			};
			TestSimulation(dm, Runs);
		}

		private SimScores TestSimulation(IDecisionMaker dm, int runs)
		{
			var rnd = new MT19937Generator(17);
			var result = new SimScores();
			var profile = new DefaultOpponentProfile();
			var sw = Stopwatch.StartNew();

			for (var run = 0; run < runs; run++)
			{
				var simulation = new GameSimulation()
				{
					Rnd = rnd,
					Profile = profile,
					DecisionMaker = dm,
				};
				result.Add(simulation.Run());
#if DEBUG
				var dir = new DirectoryInfo(Path.Combine(@"..\..\..\..\img", DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss_fff")));
				dir.Create();

				simulation.Draw(dir);
#endif
			}
			sw.Stop();

			Console.WriteLine("{0:0.0%}, {1:0.0} ms/game, Elapsed: {2}", result.Score, sw.Elapsed.TotalMilliseconds / runs, sw.Elapsed);
			result.Sort();
			Console.WriteLine(result[0]);
			return result;
		}
	}
}
