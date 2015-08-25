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

		public override int BranchingFactor0 { get { return 9; } }
		public override int BranchingFactor1 { get { return 7; } }

		public override Block TurnLeft() { return this[RotationType.Left]; }
		public override Block TurnRight() { return this[RotationType.Right]; }

		public override Position TurnLeft(Position position) { return new Position(position.Col + 1, position.Row); }
		public override Position TurnRight(Position position) { return new Position(position.Col, position.Row); }
	}
}
