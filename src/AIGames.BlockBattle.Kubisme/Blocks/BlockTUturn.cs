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
