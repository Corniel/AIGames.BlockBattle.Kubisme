using AIGames.BlockBattle.Kubisme.Evaluation;
using AIGames.BlockBattle.Kubisme.Models;
using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme.DecisionMaking
{
	public class DecisionMaker
	{
		public IEvaluator Evaluator { get; set; }
		public MoveGenerator Generator { get; set; }

		public MovePath GetMove(Field field, Position position, Block current, Block next)
		{
			var candidates = Generator.GetMoves(field, current, position);

			return GetBestMove(position, next, candidates);
		}

		private MovePath GetBestMove(Position position, Block next, IEnumerable<MoveCandiate> candidates)
		{
			var bestPath = new MovePath(0, position);
			var bestScore = double.MinValue;

			foreach (var candidate in candidates)
			{
				var tests = Generator.GetMoves(candidate.Field, next, position);

				foreach (var test in tests)
				{
					var score = Evaluator.GetScore(test.Field);
					//var score = GetAverageScore(test, position);

					if (score > bestScore)
					{
						bestPath = candidate.Path;
						bestScore = score;
					}
				}
			}
			return bestPath;
		}

		private double GetAverageScore(MoveCandiate test, Position position)
		{
			var score = 0.0;

			// Get the best result for all random options.
			foreach (var rnd in Block.All)
			{
				double rndScore = short.MinValue;
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
