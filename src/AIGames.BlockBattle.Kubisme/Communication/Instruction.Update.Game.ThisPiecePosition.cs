using System;
using System.Diagnostics;

namespace AIGames.BlockBattle.Kubisme.Communication
{

	public struct ThisPiecePositionInstruction : IInstruction
	{
		public ThisPiecePositionInstruction(Position pos) { m_Position = pos; }

		public Position Position { get { return m_Position; } }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Position m_Position;


		public override string ToString() { return String.Format("update game this_piece_position {0}", m_Position); }

		internal static IInstruction Parse(string[] splited)
		{
			var rc = splited[3].Split(',');
			if (rc.Length == 2)
			{
				int col;
				int row;

				if (Int32.TryParse(rc[0], out col) && Int32.TryParse(rc[1], out row))
				{
					return new ThisPiecePositionInstruction(new Position(col, row));
				}
			}
			return null;
		}
	}
}
