using AIGames.BlockBattle.Kubisme.Communication;
using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme
{
	/// <summary>Gets the I block rotated left.</summary>
	/// <remarks>
	/// X....
	/// X....
	/// X....
	/// X....
	/// </remarks>
	public class BlockILeft : BlockI
	{
		public override RotationType Rotation { get { return RotationType.Left; } }

		public override Position Start { get { return new Position(4, -1); } }

		public override byte[] Lines { get { return lines; } }
		private static byte[] lines = new byte[] { 1, 1, 1, 1 };

		public override int Width { get { return 1; } }

		public override IEnumerable<int> GetColumns(Field field)
		{
			if (field.RowCount < Height) { }
			else if (field.FirstFilled >= Height)
			{
				foreach (var column in Columns)
				{
					yield return column;
				}
			}
			else
			{
				var row0 = field[0];
				var row1 = field[1];
				var row2 = field[2];
				var row3 = field[3];

				foreach (var column in Columns)
				{
					if ((accessibles1[column] & row3) == 0 &&
						(accessibles1[column] & row2) == 0 &&
						(accessibles1[column] & row1) == 0 &&
						(accessibles0[column] & row0) == 0)
					{
						yield return column;
					}
				}
			}
		}

		public static readonly ushort[] accessibles0 = new ushort[]
		{
			0x007F,
			0x007F,
			0x007E,
			0x007C,

			0X0078,
			0X0078,
			
			0X00F8,
			0x01F8,
			0x03F8,
			0x03F8,
		};

		public static readonly ushort[] accessibles1 = new ushort[]
		{
			0x0003,
			0x0002,
			0x0004,
			0x0008,
			0x0010,

			0x0020,
			0x0040,
			0x0080,
			0x0100,
			0x0300,
		};

		public override BlockPath GetPath(Field field, int column)
		{
			return pathsILeft[column];
		}

		private static readonly BlockPath[] pathsILeft = new BlockPath[]
		{
			BlockPath.Create(ActionType.Left, ActionType.Left, ActionType.Left, ActionType.TurnLeft, ActionType.Left, ActionType.Drop),
			BlockPath.Create(ActionType.Left, ActionType.Left, ActionType.Left, ActionType.TurnLeft, ActionType.Drop),
			BlockPath.Create(ActionType.Left, ActionType.Left, ActionType.TurnLeft, ActionType.Drop),
			BlockPath.Create(ActionType.Left, ActionType.TurnLeft, ActionType.Drop),
			BlockPath.Create(ActionType.TurnLeft, ActionType.Drop),
			BlockPath.Create(ActionType.TurnRight, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.TurnRight, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.TurnRight, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.Right, ActionType.TurnRight, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.Right, ActionType.TurnRight, ActionType.Right, ActionType.Drop),
		};

		#region Rotation

		public override Block TurnLeft() { return this[RotationType.Uturn]; }
		public override Block TurnRight() { return this[RotationType.None]; }

		public override Position TurnLeft(Position position) { return new Position(position.Col - 1, position.Row + 2); }
		public override Position TurnRight(Position position) { return new Position(position.Col - 1, position.Row + 1); }

		#endregion
	}
}
