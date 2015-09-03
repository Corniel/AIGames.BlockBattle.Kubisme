using Troschuetz.Random.Generators;
namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class BattleBot
	{
		public BattleBot() { }
		public SimplifiedDecisionMaker DecisionMaker { get; protected set; }

		public Field GetResponse(Field own, Field other, Block current, Block next, int round)
		{
			return DecisionMaker.GetMove(own, other, current, next, round);
		}

		public static BattleBot Create(MT19937Generator rnd, ComplexParameters pars)
		{
			return new BattleBot()
			{
				DecisionMaker = new SimplifiedDecisionMaker(rnd)
				{
					Evaluator = new ComplexEvaluator()
					{
						Parameters = pars,
					},
				}
			};
		}
	}
}
