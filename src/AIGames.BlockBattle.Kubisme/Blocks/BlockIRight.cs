namespace AIGames.BlockBattle.Kubisme
{
	public class BlockIRight : BlockILeft
	{
		public override bool RotationOnly { get { return true; } }

		public override Block TurnLeft() { return this[RotationType.None]; }
		public override Block TurnRight() { return this[RotationType.Uturn]; }

		public override Position TurnLeft(Position position) { return new Position(position.Col - 2, position.Row + 1); }
		public override Position TurnRight(Position position) { return new Position(position.Col - 2, position.Row + 2); }
	}
}
