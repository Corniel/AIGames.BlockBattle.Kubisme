using System.Collections.Generic;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class SimpleDecisionMaker : IDecisionMaker
	{
		public IEvaluator Evaluator { get; set; }
		public IMoveGenerator Generator { get; set; }
		public int[] Points { get; set; }
		public Field BestField { get; protected set; }

		protected BlockPath GetBestMove(Position position, Block next, IEnumerable<MoveCandiate> candidates)
		{
			var bestPath = BlockPath.None;
			var bestScore = int.MinValue;

			foreach (var candidate in candidates)
			{
				var fields = Generator.GetFields(candidate.Field, next, position, true);

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

		public BlockPath GetMove(Field field, Position position, Block current, Block next)
		{
			var candidates = Generator.GetMoves(field, current, position, true);
		
			return GetBestMove(position, next, candidates);
		}
	}
}
