namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class BattleBot
	{
		public BattleBot() { }

		public NodeDecisionMaker DecisionMaker { get; protected set; }
		public IOpponentGenerator Predictor { get; protected set; }

		public Field GetResponse(Field own, Field other, Block current, Block next, int round)
		{
			var opponent = Predictor.Create(round, other, current, next);

			((ComplexEvaluator)DecisionMaker.Evaluator).Opponent = opponent;
			((ComplexEvaluator)DecisionMaker.Evaluator).Initial = own;

			var path = DecisionMaker.GetMove(own, opponent, current, next, round);
			if (path.Equals(BlockPath.None))
			{
				return Field.None;
			}
			return DecisionMaker.BestField;
		}

		public static BattleBot Create(ComplexParameters pars)
		{
			return new BattleBot()
			{
				Predictor = new OpponentGenerator(),
				DecisionMaker = new NodeDecisionMaker()
				{
					Evaluator = new ComplexEvaluator()
					{
						Parameters = pars,
					},
					Generator = new MoveGenerator(),
					MaximumDepth = AppConfig.Data.SearchDepth,
				}
			};
		}
	}
}
