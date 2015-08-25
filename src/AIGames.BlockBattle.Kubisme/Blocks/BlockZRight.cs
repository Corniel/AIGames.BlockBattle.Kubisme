namespace AIGames.BlockBattle.Kubisme
{
	public class BlockZRight: BlockZLeft
	{
		public override bool RotationOnly { get { return true; } }

		public override Block TurnLeft() { return this[RotationType.None]; }
		public override Block TurnRight() { return this[RotationType.Uturn]; }

		public override Position TurnLeft(Position position) { return new Position(position.Col, position.Row); }
		public override Position TurnRight(Position position) { return new Position(position.Col, position.Row + 1); }
	}
}
