using AIGames.BlockBattle.Kubisme.Models;
using System;

namespace AIGames.BlockBattle.Kubisme.Genetics.Models
{
	public class ComboAddictOpponentProfile : IOpponentProfile
	{
		public ComboAddictOpponentProfile()
		{
			MaxCombo = 4;
			MinRows = 4;
		}
		public int Turn { get; protected set; }
		public int PointsOwn { get; protected set; }
		public int PointsOppo { get; protected set; }
		
		public int Clears { get; protected set; }
		public int Combo { get; protected set; }
		public int MaxCombo { get; protected set; }
		public int MinRows { get; protected set; }
		

		public int Blocks
		{
			get
			{
				var blocks = 4 * Turn;
				blocks -= 10 * Clears;
				return blocks;
			}
		}
		public int Locks { get { return PointsOppo >> 2; } }
		public int Rows { get { return 20 - Locks; } }

		public Field Apply(Field field, int turn)
		{
			Turn = turn;
			PointsOppo = field.Points;

			var filledRows = 1 + (Blocks - 1) / 7;
			var freeRows = Rows - filledRows;

			var doclear = Blocks >= 10 && Combo < MaxCombo && (freeRows < 8 || Combo > 0);

			// With less then 6 rows, no combo's anymore.
			if (freeRows < 6)
			{
				doclear &= Combo == 0;
			}
			if (doclear)
			{
				var rest = Blocks % 3;
				var clears = rest > 2 && Blocks > 20 ? 2 : 1;
				PointsOwn += clears + Combo;
				Combo++;
				Clears += clears;
			}
			else
			{
				Combo = 0;
			}

			var lck = (PointsOwn >> 2) - (20 - field.RowCount);
			return lck > 0 ? field.LockRows(lck) : field;
		}

		public bool IsAlive(Field field, int turn)
		{
			return Rows > MinRows;
		}

		public override string ToString()
		{
			return String.Format("Turn: {6}, Pt: {0}-{1}, Rows: {2}, Clears: {3}, Combo: {4}, Blocks: {5}",
				PointsOwn,
				PointsOppo,
				Rows,
				Clears,
				Combo,
				Blocks,
				Turn);
		}
	}
}
