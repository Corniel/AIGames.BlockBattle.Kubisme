using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGames.BlockBattle.Kubisme
{
	/// <summary>Gets the T block rotated twice.</summary>
	/// <remarks>
	/// XXX.
	/// .X..
	/// ....
	/// ....
	/// </remarks>
	public class BlockTUturn : BlockT
	{
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

		public override RotationType Rotation { get { return RotationType.Uturn; } }

		public override byte[] Lines { get { return lines; } }
		private static byte[] lines = new byte[] { 7, 2 };

		public override int Width { get { return 3; } }

		public override IEnumerable<int> GetColumns(Field field) { return GetColumnsJLTUTurn(field); }
		public override BlockPath GetPath(Field field, int column) { return pathsJLTUturn[column]; }

		public override Block TurnLeft() { return this[RotationType.Right]; }
		public override Block TurnRight() { return this[RotationType.Left]; }

		public override Position TurnLeft(Position position) { return new Position(position.Col + 1, position.Row - 1); }
		public override Position TurnRight(Position position) { return new Position(position.Col, position.Row - 1); }
	}
}
