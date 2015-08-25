namespace AIGames.BlockBattle.Kubisme
{
	public class BlockIUturn : BlockI
	{
		public override bool RotationOnly { get { return true; } }

		public override Block TurnLeft() { return this[RotationType.Right]; }
		public override Block TurnRight() { return this[RotationType.Left]; }

		public override Position TurnLeft(Position position) { return new Position(position.Col + 2, position.Row - 2); }
		public override Position TurnRight(Position position) { return new Position(position.Col + 1, position.Row - 2); }
	}
}
