using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme
{
	/// <summary>Gets the S block rotated left.</summary>
	/// <remarks>
	/// X...
	/// XX..
	/// .X..
	/// ....
	/// </remarks>
	public class BlockSLeft : BlockS
	{
		public override RotationType Rotation { get { return RotationType.Left; } }

		public override byte[] Lines { get { return lines; } }
		private static byte[] lines = new byte[] { 1, 3, 2 };

		public override int Width { get { return 2; } }

		public override IEnumerable<int> GetColumns(Field field)
		{
			return GetColumnsSZLeft(field);
		}

		public override BlockPath GetPath(Field field, int column)
		{
			return pathsSZLeft[column];
		}

		public override Block TurnLeft() { return this[RotationType.Uturn]; }
		public override Block TurnRight() { return this[RotationType.None]; }

		public override Position TurnLeft(Position position) { return new Position(position.Col -1, position.Row + 1); }
		public override Position TurnRight(Position position) { return new Position(position.Col - 1, position.Row + 1); }
	}
}
