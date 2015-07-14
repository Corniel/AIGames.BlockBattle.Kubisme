using AIGames.BlockBattle.Kubisme.Evaluation;
using AIGames.BlockBattle.Kubisme.Models;
using System.Collections.Generic;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.DecisionMaking
{
	public class DecisionMaker : IDecisionMaker
	{
		public IEvaluator Evaluator { get; set; }
		public IMoveGenerator Generator { get; set; }

		public MovePath GetMove(Field field, Position position, Block current, Block next)
		{
			var candidates = Generator.GetMoves(field, current, position);

			return GetBestMove(position, next, candidates);
		}

		private IEnumerable<MoveCandiate> GetBest(IEnumerable<MoveCandiate> candidates, int best)
		{
			return candidates
				.OrderByDescending(candidate => Evaluator.GetScore(candidate.Field))
				.Take(best);
		}

		protected virtual MovePath GetBestMove(Position position, Block next, IEnumerable<MoveCandiate> candidates)
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

		private int GetAverageScore(MoveCandiate test, Position position)
		{
			var score = 0;

			// Get the best result for all random options.
			foreach (var rnd in Block.All)
			{
				int rndScore = short.MinValue;
				var rndTests = Generator.GetMoves(test.Field, rnd, position);
				foreach (var rndTest in rndTests)
				{
					var s = Evaluator.GetScore(rndTest.Field);
					if (s > rndScore)
					{
						rndScore = s;
					}
				}
				score += rndScore;
			}
			return score;
		}
	}
}
