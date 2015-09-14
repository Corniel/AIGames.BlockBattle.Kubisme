using AIGames.BlockBattle.Kubisme.Communication;
using System.Collections.Generic;

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

		public override RotationType Rotation { get { return RotationType.Uturn; } }

		public override byte[] Lines { get { return lines; } }
		private static byte[] lines = new byte[] { 7, 2 };

		public override int Width { get { return 3; } }

		public override Block TurnLeft() { return this[RotationType.Right]; }
		public override Block TurnRight() { return this[RotationType.Left]; }

		public override Position TurnLeft(Position position) { return new Position(position.Col + 1, position.Row - 1); }
		public override Position TurnRight(Position position) { return new Position(position.Col, position.Row - 1); }

		public override IEnumerable<int> GetColumns(Field field)
		{
			if (field.FirstFilled > 1)
			{
				foreach (var col in Columns)
				{
					yield return col;
				}
			}
			else
			{
				var row = field[0];
				foreach (var column in Columns)
				{
					if ((accessibles[column] & row) == 0)
					{
						yield return column;
					}
				}
			}
		}

		public override BlockPath GetPath(Field field, int column)
		{
			return pathsTUTurn[column];
		}

		public static readonly BlockPath[] pathsTUTurn = new BlockPath[]
		{
			BlockPath.Create(ActionType.Left, ActionType.Left, ActionType.Left, ActionType.TurnLeft, ActionType.TurnLeft, ActionType.Drop),
			BlockPath.Create(ActionType.Left, ActionType.Left, ActionType.TurnLeft, ActionType.TurnLeft, ActionType.Drop),
			BlockPath.Create(ActionType.Left, ActionType.TurnLeft, ActionType.TurnLeft, ActionType.Drop),
			
			BlockPath.Create(ActionType.TurnLeft, ActionType.TurnLeft, ActionType.Drop),

			BlockPath.Create(ActionType.Right, ActionType.TurnRight, ActionType.TurnRight, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.TurnRight, ActionType.TurnRight, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.Right, ActionType.TurnRight, ActionType.TurnRight, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.Right, ActionType.Right, ActionType.TurnRight, ActionType.TurnRight, ActionType.Drop),
		};
	}
}
