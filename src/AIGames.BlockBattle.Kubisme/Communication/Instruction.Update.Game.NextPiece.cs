using System;
using System.Diagnostics;

namespace AIGames.BlockBattle.Kubisme.Communication
{
	public struct NextPieceInstruction : IInstruction
	{
		public NextPieceInstruction(PieceType piece) { m_Piece = piece; }

		public PieceType Piece { get { return m_Piece; } }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private PieceType m_Piece;

		public override string ToString() { return String.Format("update game next_piece_type {0}", Piece); }

		internal static IInstruction Parse(string[] splited)
		{
			PieceType tp;
			if (Enum.TryParse<PieceType>(splited[3], out tp))
			{
				return new NextPieceInstruction(tp);
			}
			return null;
		}
	}
}
