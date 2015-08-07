using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class BattleBot
	{
		public BattleBot() { }

		public NodeDecisionMaker DecisionMaker { get; protected set; }
		public PointsPredictor Predictor { get; protected set; }

		public Field GetResponse(Field own, Field other, Block current, Block next, int round)
		{
			DecisionMaker.Points = Predictor.GetPoints(other, current, next);

			var path = DecisionMaker.GetMove(own, current, next, round);
			if (path.Equals(BlockPath.None))
			{
				return Field.None;
			}
			return DecisionMaker.BestField;
		}

		public static BattleBot Create(SimpleParameters pars)
		{
			return new BattleBot()
			{
				Predictor = new PointsPredictor(),
				DecisionMaker = new NodeDecisionMaker()
				{
					Evaluator = new SimpleEvaluator()
					{
						Parameters = pars,
					},
					Generator = new MoveGenerator(),
					MaximumDepth = 2,
				}
			};
		}
	}
}
