using AIGames.BlockBattle.Kubisme.Communication;
using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme
{
	/// <summary>Gets the I block.</summary>
	/// <remarks>
	/// XXXX
	/// ....
	/// ....
	/// ....
	/// </remarks>
	public class BlockI: Block
	{
		public override Position Start { get { return new Position(3, 0); } }

		public override string Name { get { return "I"; } }
		public override int ChildCount { get { return 17; } }

		public override byte[] Lines { get { return lines; } }
		private static byte[] lines = new byte[] { 15 };

		public override int Width { get { return 4; } }

		public override IEnumerable<int> GetColumns(Field field)
		{
			if (field.FirstFilled > 0)
			{
				foreach (var column in Columns)
				{
					yield return column;
				}
			}
			else if(field.RowCount > 0)
			{
				var row = field[0];
				foreach (var column in Columns)
				{
					if ((accessiblesI[column] & row) == 0)
					{
						yield return column;
					}
				}
			}
		}

		public static readonly int[] accessiblesI = new int[]
		{
			0x007F,
			0x007E,
			0x007C,

			0X0078,
			
			0X00F8,
			0x0178,
			0x0378,
		};

		public override BlockPath GetPath(Field field, int column)
		{
			return pathsI[column];
		}

		private static readonly BlockPath[] pathsI = new BlockPath[]
		{
			BlockPath.Create(ActionType.Left, ActionType.Left, ActionType.Left, ActionType.Drop),
			BlockPath.Create(ActionType.Left, ActionType.Left, ActionType.Drop),
			BlockPath.Create(ActionType.Left, ActionType.Drop),
			BlockPath.Create(ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.Right, ActionType.Drop),
		};
	}
}
