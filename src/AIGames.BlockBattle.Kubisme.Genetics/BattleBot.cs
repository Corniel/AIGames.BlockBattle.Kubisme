using Troschuetz.Random.Generators;
namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class BattleBot
	{
		public BattleBot() { }
		public NodeDecisionMaker DecisionMaker { get; protected set; }
		public OpponentGenerator OpponentGenerator { get; protected set; }

		public Field GetResponse(Field own, Field other, Block current, Block next, int round)
		{
			var opponent = OpponentGenerator.Create(round, other, current, next, 2);
			((ComplexEvaluator)DecisionMaker.Evaluator).Opponent = opponent;
			var move = DecisionMaker.GetMove(own, opponent, current, next, round);
			return DecisionMaker.BestField;
		}

		public static BattleBot Create(MT19937Generator rnd, ComplexParameters pars)
		{
			return new BattleBot()
			{
				OpponentGenerator = new OpponentGenerator(),
				DecisionMaker = new NodeDecisionMaker(rnd)
				{
					MaximumDepth = 2,
					Evaluator = new ComplexEvaluator()
					{
						Parameters = pars,
					},
					Generator = new MoveGenerator(),
				}
			};
		}
	}
}
