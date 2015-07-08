using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AIGames.BlockBattle.Kubisme.Models
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public struct Position
	{
		public static readonly Position Start = new Position(4, -1);

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

		public override string ToString() { return String.Format("{0},{1}", Col, Row); }

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
