using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class SimplifiedDecisionMaker
	{
		public SimplifiedDecisionMaker(MT19937Generator rnd)
		{
			OpponentGenerator = new OpponentGenerator();
			MoveGenerator = new MoveGenerator();
			Evaluator = new ComplexEvaluator();
			Rnd = rnd;
		}
		public IOpponentGenerator OpponentGenerator { get; set; }
		public IMoveGenerator MoveGenerator { get; set; }
		public MT19937Generator Rnd { get; set; }

		public Field GetMove(Field own, Field other, Block current, Block next, int round)
		{
			var bestScore = int.MinValue;
			var bestField = Field.None;

			if (round > 20 && round % 20 == 1)
			{
				own = own.LockRow();
				other = other.LockRow();
			}

			var oppo = OpponentGenerator.Create(round, other, current, next, 2);

			Evaluator.Opponent = oppo;
			Evaluator.Initial = own;

			foreach (var depth1 in MoveGenerator.GetFields(own, current, true))
			{
				var applied1 = depth1;
				if (round > 21 && round % 20 == 2)
				{
					applied1 = applied1.LockRow();
				}
				if (oppo.States[1].Garbage > 0)
				{
					var garbage = Row.GetGarbage(oppo.States[1].Garbage, Rnd);
					applied1 = applied1.Garbage(garbage);
				}

				foreach (var depth2 in MoveGenerator.GetFields(applied1, next, true))
				{
					var applied2 = depth2;
					if (round > 22 && round % 20 == 3)
					{
						applied2 = applied2.LockRow();
					}
					if (oppo.States[2].Garbage > 0)
					{
						var garbage = Row.GetGarbage(oppo.States[2].Garbage, Rnd);
						applied2 = applied1.Garbage(garbage);
					}

					var score = Evaluator.GetScore(applied2, 2);
					if (score > bestScore)
					{
						bestScore = score;
						bestField = depth1;
					}
				}
			}
			return bestField;
		}

		public ComplexEvaluator Evaluator { get; set; }
	}
}
