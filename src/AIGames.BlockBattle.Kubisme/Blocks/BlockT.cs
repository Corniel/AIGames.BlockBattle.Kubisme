using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme
{
	/// <summary>Gets the T block.</summary>
	/// <remarks>
	/// .X..
	/// XXX.
	/// ....
	/// ....
	/// </remarks>
	public class BlockT : Block
	{
		#region T-Spin

		public static readonly ushort[] TSpinTopMask = new ushort[]
		{
			0x005,
			0x00A,
			0x014,
			0x028,

			0x050,
			0x0A0,
			0x140,
			0x280,
		};

		public static readonly ushort[] TSpinRow1Mask = new ushort[]
		{
			0X007,
			0X00E,
			0X01C,
			0X038,
			
			0X070,
			0X0E0,
			0X1C0,
			0X380,
		};

		public static readonly ushort[] TSpinRow2Mask = new ushort[]
		{
			0X3FD,
			0X3FB,
			0X3F7,
			0X3EF,
			
			0X3DF,
			0X3BF,
			0X37F,
			0X2FF,
		};
		#endregion

		public override string Name { get { return "T"; } }

		public override byte[] Lines { get { return lines; } }
		private static byte[] lines = new byte[] { 2, 7 };

		public override int Width { get { return 3; } }

		#region Rotation

		public override Block TurnLeft() { return this[RotationType.Left]; }
		public override Block TurnRight() { return this[RotationType.Right]; }

		public override Position TurnLeft(Position position) { return new Position(position.Col, position.Row); }
		public override Position TurnRight(Position position) { return new Position(position.Col + 1, position.Row); }

		#endregion

		/// <summary>Returns true if a T-spin was applied.</summary>
		public override bool IsTSpin(Position pos, ushort[] rows)
		{
			return
				rows.Length > pos.Row -1 &&
				(TSpinTopMask[pos.Col] & rows[pos.Row]) != 0 &&
				// The tail of the T should be a perfect fit.
				(TSpinTopMask[pos.Col] & rows[pos.Row + 2]) == TSpinTopMask[pos.Col];
		}
	}
}
