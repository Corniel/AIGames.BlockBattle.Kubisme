using AIGames.BlockBattle.Kubisme.DecisionMaking;
using AIGames.BlockBattle.Kubisme.Models;
using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme.Genetics.DecisionMaking
{
	public class SimpleDecisionMaker : DecisionMaker
	{
		private SimpleDecisionMaker() { }

		protected override MovePath GetBestMove(Position position, Block next, IEnumerable<MoveCandiate> candidates)
		{
			var bestPath = MovePath.None;
			var bestScore = int.MinValue;

			foreach (var candidate in candidates)
			{
				var tests = Generator.GetMoves(candidate.Field, next, position);

				foreach (var test in tests)
				{
					var score = Evaluator.GetScore(test.Field);

					if (score > bestScore)
					{
						bestPath = candidate.Path;
						bestScore = score;
					}
				}
			}
			return bestPath;
		}
	}
}
