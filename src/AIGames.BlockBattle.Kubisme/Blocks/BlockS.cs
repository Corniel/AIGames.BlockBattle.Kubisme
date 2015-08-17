using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme
{
	/// <summary>Gets the S block.</summary>
	/// <remarks>
	/// .XX.
	/// XX..
	/// ....
	/// ....
	/// </remarks>
	public class BlockS : Block
	{
		public override string Name { get { return "S"; } }
		public override int ChildCount { get { return 17; } }

		public override byte[] Lines { get { return lines; } }
		private static byte[] lines = new byte[] { 6, 3 };

		public override int Width { get { return 3; } }
	}

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
		private static byte[] lines = new byte[] {1, 3, 2 };

		public override int Width { get { return 2; } }

		public override IEnumerable<int> GetColumns(Field field)
		{
			return GetColumnsSZLeft(field);
		}

		public override BlockPath GetPath(Field field, int column)
		{
			return pathsSZLeft[column];
		}
	}
}
