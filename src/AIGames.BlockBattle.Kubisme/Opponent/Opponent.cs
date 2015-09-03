using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGames.BlockBattle.Kubisme
{
	public class Opponent
	{
		public Opponent(int turn, Field field ,int depth)
		{
			this.Turn = turn;
			this.Field = field;
			this.States = new OpponentState[depth + 1];
		}

		public int Turn { get; protected set; }
		public Field Field { get; protected set; }
		public OpponentState[] States { get; protected set; }
	}
}
