using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AIGames.BlockBattle.Kubisme
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public struct OpponentState
	{
		public OpponentState(int points, int garbage, int firstFilled, int combo, int rowCount)
		{
			Points = (short)points;
			Garbage = (byte)garbage;
			FirstFilled = (sbyte)firstFilled;
			Combo = (byte)combo;
			RowCount = (byte)rowCount;
		}

		public readonly short Points;
		public readonly byte Garbage;
		public readonly sbyte FirstFilled;
		public readonly byte Combo;
		public readonly byte RowCount;

		public OpponentState SetPoints(int points)
		{
			return new OpponentState(points, Garbage, FirstFilled, Combo, RowCount);
		}
		public OpponentState SetGarbage(int garbage)
		{
			return new OpponentState(Points, garbage, FirstFilled, Combo, RowCount);
		}
		public OpponentState SetFirstFilled(int firstFilled)
		{
			return new OpponentState(Points, Garbage, firstFilled, Combo, RowCount);
		}
		public OpponentState SetCombo(int combo)
		{
			return new OpponentState(Points, Garbage, FirstFilled, combo, RowCount);
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never), ExcludeFromCodeCoverage]
		public string DebuggerDisplay
		{
			get
			{
				return String.Format("Pt: {0} (+{4}), FreeRows: {1}, Combo: {2}, Rows: {3}", Points, FirstFilled, Combo, RowCount, Garbage);
			}
		}
	}
}
