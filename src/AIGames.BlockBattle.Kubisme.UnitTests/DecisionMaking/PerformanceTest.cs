﻿using AIGames.BlockBattle.Kubisme.DecisionMaking;
using AIGames.BlockBattle.Kubisme.Evaluation;
using AIGames.BlockBattle.Kubisme.Genetics;
using AIGames.BlockBattle.Kubisme.Genetics.DecisionMaking;
using AIGames.BlockBattle.Kubisme.Genetics.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.UnitTests.DecisionMaking
{
	[TestFixture]
	public class PerformanceTest
	{
		public const int Runs = 100;

		[Test]
		public void Run_DecisionMaker_92procent()
		{
			var dm = new DecisionMaker()
			{
				Evaluator = new SimpleEvaluator()
				{
					Parameters = new SimpleParameters()
					{
						RowWeights = new int[] { -48, 38, -124, -86, -64, -35, -13, 0, 7, 16, 6, 12, 28, 25, 29, 22, 46, -17, 54, 32, 7 },
						NineRowWeights = new int[] { 18, 89, -1, -75, 18, -23, -62, -18, 65, 60, 86, -30, -3, -63, 34, 5, -24, 48, -39, 36, -65 },
						Points = 250,
						Combo = 101,
						Holes = -136,
						Blockades = -121,
						WallsLeft = 98,
						WallsRight = 14,
						Floor = -55,
						NeighborsHorizontal = -80,
						NeighborsVertical = 102,
					},
				},
				Generator = new MoveGenerator(),
			};
			TestSimulation(dm, 100);
		}

		[Test]
		public void Run_SimpleDecisionMaker_62procent()
		{
			var dm = new SimpleDecisionMaker()
			{
				Evaluator = new SimpleEvaluator()
				{
					Parameters = new SimpleParameters()
					{
						RowWeights = new int[] { -48,38,-124,-86,-64,-35,-13,0,7,16,6,12,28,25,29,22,46,-17,54,32,7 },
						NineRowWeights = new int[] { 18,89,-1,-75,18,-23,-62,-18,65,60,86,-30,-3,-63,34,5,-24,48,-39,36,-65 },
						Points = 250,
						Combo = 101,
						Holes = -136,
						Blockades = -121,
						WallsLeft = 98,
						WallsRight = 14,
						Floor = -55,
						NeighborsHorizontal = -80,
						NeighborsVertical = 102,
					},
				},
				Generator = new MoveGenerator(),
			};
			TestSimulation(dm, Runs);
		}

		private SimScores TestSimulation(DecisionMaker dm, int runs)
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
			}
			sw.Stop();
			Console.WriteLine("{0:0.0%}, {1:0.0} ms/game, Elapsed: {2}", result.Score, sw.Elapsed.TotalMilliseconds / runs, sw.Elapsed);
			result.Sort();
			Console.WriteLine(result[0]);
			return result;
		}
	}
}