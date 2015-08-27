using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGames.BlockBattle.Kubisme
{
	public class BlockSRight: BlockSLeft
	{
		public override bool RotationOnly { get { return true; } }
		public override RotationType Rotation { get { return RotationType.Right; } }

		public override Block TurnLeft() { return this[RotationType.None]; }
		public override Block TurnRight() { return this[RotationType.Uturn]; }

		public override Position TurnLeft(Position position) { return new Position(position.Col, position.Row); }
		public override Position TurnRight(Position position) { return new Position(position.Col, position.Row + 1); }
	}
}
