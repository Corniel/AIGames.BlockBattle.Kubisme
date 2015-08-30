namespace AIGames.BlockBattle.Kubisme
{
	public class BlockZRight: BlockZLeft
	{
		public override bool RotationOnly { get { return true; } }
		public override RotationType Rotation { get { return RotationType.Right; } }

		public override Block TurnLeft() { return this[RotationType.None]; }
		public override Block TurnRight() { return this[RotationType.Uturn]; }

		public override Position TurnLeft(Position position) { return new Position(position.Col - 1, position.Row); }
		public override Position TurnRight(Position position) { return new Position(position.Col - 1, position.Row + 1); }
	}
}
