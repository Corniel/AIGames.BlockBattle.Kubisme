﻿using System.Collections.Generic;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class SimpleDecisionMaker : IDecisionMaker
	{
		public IEvaluator Evaluator { get; set; }
		public IMoveGenerator Generator { get; set; }
		public int[] Points { get; set; }
		public Field BestField { get; protected set; }

		protected BlockPath GetBestMove(Block next, IEnumerable<MoveCandiate> candidates)
		{
			var bestPath = BlockPath.None;
			var bestScore = int.MinValue;

			foreach (var candidate in candidates)
			{
				var fields = Generator.GetFields(candidate.Field, next, true);

				foreach (var field in fields)
				{
					var score = Evaluator.GetScore(field);

					if (score > bestScore)
					{
						bestPath = candidate.Path;
						bestScore = score;
						BestField = candidate.Field;
					}
				}
			}
			return bestPath;
		}

		public BlockPath GetMove(Field field, Block current, Block next, int round)
		{
			var bestMove = BlockPath.None;
			var bestScore = int.MinValue;

			foreach (var candidate in Generator.GetMoves(field, current, true))
			{
				foreach (var field2 in Generator.GetFields(candidate.Field, next, true))
				{
					var score = Evaluator.GetScore(field2);
					if (score > bestScore)
					{
						bestScore = score;
						BestField = candidate.Field;
						bestMove = candidate.Path;
					}
				}
			}
			return bestMove;
		}
	}
}
