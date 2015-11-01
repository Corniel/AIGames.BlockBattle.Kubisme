using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class BattleBot
	{
		public BattleBot() { }
		public NodeDecisionMaker DecisionMaker { get; protected set; }

		public Field GetResponse(Field own, Block current, Block next, int round)
		{
			var move = DecisionMaker.GetMove(own, current, next, round);
			return DecisionMaker.BestField;
		}

		public static BattleBot Create(MT19937Generator rnd, EvaluatorParameters pars, int maxDepth)
		{
			return new BattleBot()
			{
				DecisionMaker = new NodeDecisionMaker(rnd)
				{
					MaximumDepth = maxDepth,
					Evaluator = new Evaluator()
					{
						Pars = pars,
					},
					Generator = new MoveGenerator(),
				}
			};
		}
	}
}
