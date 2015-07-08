using System;

namespace AIGames.BlockBattle.Kubisme.Models
{
	public struct MovePath
	{
		public readonly Block.RotationType Option;
		public readonly Position Target;

		public MovePath(Block.RotationType option, Position target)
		{
			Option = option;
			Target = target;
		}

		public override string ToString()
		{
			return String.Format("Option: {0}, Target: {1}", Option, Target);
		}
	}
}
