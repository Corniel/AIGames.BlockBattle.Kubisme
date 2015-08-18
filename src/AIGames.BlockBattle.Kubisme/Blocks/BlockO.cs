using AIGames.BlockBattle.Kubisme.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGames.BlockBattle.Kubisme
{
	/// <summary>Gets the O block.</summary>
	/// <remarks>
	/// XX..
	/// XX..
	/// ....
	/// ....
	/// </remarks>
	public class BlockO : Block
	{
		public override Position Start { get { return new Position(4, -1); } }

		public override string Name { get { return "O"; } }
		public override int ChildCount { get { return 9; } }

		public override byte[] Lines { get { return lines; } }
		private static byte[] lines = new byte[] { 3, 3 };

		public override int Width { get { return 2; } }

		public override int BranchingFactor0 { get { return 5; } }
		public override int BranchingFactor1 { get { return 4; } }

		public override IEnumerable<int> GetColumns(Field field)
		{
			if (field.RowCount < Height) { }
			else if (field.FirstFilled > 0)
			{
				foreach (var column in Columns)
				{
					yield return column;
				}
			}
			else
			{
				var row = field[0];
				foreach (var column in Columns)
				{
					if((accessiblesO[column] & row) == 0)
					{
						yield return column;
					}
				}
			}
		}

		public static readonly ushort[] accessiblesO = new ushort[]
		{
			0x003F,
			0x003E,
			0x003C,
			0X0038,

			0x0030,

			0X0070,
			0x00F0,
			0x01F0,
			0x03F0,
		};

		public override BlockPath GetPath(Field field, int column)
		{
			return pathsO[column];
		}

		private static readonly BlockPath[] pathsO = new BlockPath[]
		{
			BlockPath.Create(ActionType.Left, ActionType.Left, ActionType.Left, ActionType.Left, ActionType.Drop),
			BlockPath.Create(ActionType.Left, ActionType.Left, ActionType.Left, ActionType.Drop),
			BlockPath.Create(ActionType.Left, ActionType.Left, ActionType.Drop),
			BlockPath.Create(ActionType.Left, ActionType.Drop),
			BlockPath.Create(ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.Right, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.Right, ActionType.Right, ActionType.Drop),
		};
	}
}
