using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AIGames.BlockBattle.Kubisme
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public struct Position : IEquatable<Position>
	{
		//public static readonly Position Start = new Position(4, -1);
		public static readonly Position None = new Position(255, 255);

		[DebuggerStepThrough]
		public Position(int c, int r) : this((sbyte)c, (sbyte)r) { }
		[DebuggerStepThrough]
		public Position(sbyte c, sbyte r)
		{
			col = c;
			row = r;
		}

		public int Col { get { return col; } }
		public int Row { get { return row; } }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private sbyte row;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private sbyte col;

		public Position Left { get { return new Position(col - 1, row); } }
		public Position Right { get { return new Position(col + 1, row); } }
		public Position Down { get { return new Position(col, row + 1); } }
		public Position	Up { get { return new Position(col, row - 1); } }

		public override string ToString() { return String.Format("{0},{1}", Col, Row); }

		public override int GetHashCode()
		{
			int hash = row;
			hash |= (int)col << 8;
			return hash;
		}

		public override bool Equals(object obj)
		{
			return base.Equals((Position)obj);
		}
		public bool Equals(Position pos) { return row == pos.row && col == pos.col; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never), ExcludeFromCodeCoverage]
		private string DebuggerDisplay
		{
			get
			{
				return String.Format("Col: {0}, Row: {1}", Col, Row);
			}
		}
		
	}
}
