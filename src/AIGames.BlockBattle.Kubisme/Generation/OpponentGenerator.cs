namespace AIGames.BlockBattle.Kubisme
{
	public class OpponentGenerator : IOpponentGenerator
	{
		public OpponentGenerator ()
		{
			Generator = new MoveGenerator();
		}

		public MoveGenerator Generator { get; protected set; }

		public Opponent Create(int turn, Field field, Block current, Block next, int depth)
		{
			var opponent = new Opponent(turn, field, depth);
			opponent.States[0] = new OpponentState(field.Points, 0, field.FirstFilled, 0, field.RowCount);
			opponent.States[1] = new OpponentState(field.Points, 0, field.FirstFilled, 0, 20 - (turn + 1) / 20);

			foreach (var depth1 in Generator.GetFields(field, current, true))
			{
				if (depth1.Points > opponent.States[0].Points)
				{
					opponent.States[0] = opponent.States[0].SetPoints(depth1.Points);
				}
				if (depth1.FirstFilled > opponent.States[0].FirstFilled)
				{
					opponent.States[0] = opponent.States[0].SetFirstFilled(depth1.FirstFilled);
				}
				if (depth1.Combo > opponent.States[0].Combo)
				{
					opponent.States[0] = opponent.States[0].SetCombo(depth1.Combo);
				}

				foreach (var depth2 in Generator.GetFields(depth1, next, true))
				{
					if (depth2.Points > opponent.States[1].Points)
					{
						opponent.States[1] = opponent.States[1].SetPoints(depth2.Points);
					}
					if (depth2.FirstFilled > opponent.States[1].FirstFilled)
					{
						opponent.States[1] = opponent.States[1].SetFirstFilled(depth2.FirstFilled);
					}
					if (depth2.Combo > opponent.States[1].Combo)
					{
						opponent.States[1] = opponent.States[1].SetCombo(depth2.Combo);
					}
				}
			}

			opponent.States[0] = opponent.States[0].SetGarbage((opponent.States[0].Points >> 2) - (field.Points >> 2));
			opponent.States[1] = opponent.States[1].SetGarbage((opponent.States[1].Points >> 2) - (opponent.States[0].Points >> 2));

			var prev = opponent.States[1];

			var points = opponent.States[1].Points;
			var combo = opponent.States[1].Combo;

			for (var i = 2; i < opponent.States.Length; i++)
			{
				points += ++combo;
				var garbage = (points >> 2) - (prev.Points >> 2);
				var rows = 20 - (turn + 1 + i) / 20;
				var firstFilled = prev.FirstFilled + 1;

				if (prev.RowCount != rows)
				{
					firstFilled--;
				}
				if (firstFilled > rows + 1)
				{
					firstFilled = rows + 1;
				}

				opponent.States[i] = new OpponentState(points, garbage, firstFilled, combo, rows);
				prev = opponent.States[i];
			}
			
			return opponent;
		}
	}
}
