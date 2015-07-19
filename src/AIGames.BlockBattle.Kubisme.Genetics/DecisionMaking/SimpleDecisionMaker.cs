using AIGames.BlockBattle.Kubisme.DecisionMaking;
using AIGames.BlockBattle.Kubisme.Evaluation;
using AIGames.BlockBattle.Kubisme.Models;
using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme.Genetics.DecisionMaking
{
	public class SimpleDecisionMaker : IDecisionMaker
	{
		public IEvaluator Evaluator { get; set; }
		public IMoveGenerator Generator { get; set; }

		protected MovePath GetBestMove(Position position, Block next, IEnumerable<MoveCandiate> candidates)
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

		public MovePath GetMove(Field field, Position position, Block current, Block next)
		{
			var candidates = Generator.GetMoves(field, current, position);
			return GetBestMove(position, next, candidates);
		}
	}
}
