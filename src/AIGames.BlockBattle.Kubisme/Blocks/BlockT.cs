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
			0x0005,
			0x000A,
			0x0014,
			0x0028,

			0x0050,
			0x00A0,
			0x0140,
			0x0280,
		};

		public static readonly ushort[] TSpinTopBorderMask = new ushort[]
		{
			0x0008,
			0x0011,
			0x0022,
			0x0044,

			0x0088,
			0x0110,
			0x0220,
			0x0040,
		};

		public static readonly ushort[] TSpinRow1Mask = new ushort[]
		{
			0X03F8,
			0X03F1,
			0X03E3,
			0X03C7,
			
			0X038F,
			0X031F,
			0X023F,
			0X007F,
		};

		public static readonly ushort[] TSpinRow2Mask = new ushort[]
		{
			0X03FD,
			0X03FB,
			0X03F7,
			0X03EF,
			
			0X03DF,
			0X03BF,
			0X037F,
			0X02FF,
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
