﻿namespace AIGames.BlockBattle.Kubisme
{
	public class BlockSUturn : BlockS
	{
		public override bool RotationOnly { get { return true; } }
		public override RotationType Rotation { get { return RotationType.Uturn; } }

		public override Block TurnLeft() { return this[RotationType.Right]; }
		public override Block TurnRight() { return this[RotationType.Left]; }

		public override Position TurnLeft(Position position) { return new Position(position.Col + 1, position.Row - 1); }
		public override Position TurnRight(Position position) { return new Position(position.Col, position.Row - 1); }
	}
}
