using AIGames.BlockBattle.Kubisme.Models;
using System;
using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme.Communication
{
	public class MoveInstruction : IInstruction
	{
		public static readonly MoveInstruction NoMoves = new MoveInstruction(new ActionType[0]);

		public MoveInstruction(params ActionType[] actions)
		{
			Actions = actions;
		}

		private ActionType[] Actions;

		public override string ToString()
		{
			if (Actions.Length == 0) { return "no_moves"; }
			return String.Join(",", Actions).ToLowerInvariant();
		}

		public static MoveInstruction Create(Block block, Position source, Position target)
		{
			var actions = new List<ActionType>();

			switch (block.Rotation)
			{
				case Block.RotationType.Left: actions.Add(ActionType.TurnLeft); break;
				case Block.RotationType.Uturn: actions.Add(ActionType.TurnLeft); actions.Add(ActionType.TurnLeft); break;
				case Block.RotationType.Right: actions.Add(ActionType.TurnRight); break;
			}
			var delta = source.Col - target.Col;
			if (delta >= 0)
			{
				for (var i = 0; i < delta; i++)
				{
					actions.Add(ActionType.Left);
				}
			}
			else
			{
				for (var i = 0; i < -delta; i++)
				{
					actions.Add(ActionType.Right);
				}
			}
			var drop = target.Row - source.Row - block.Bottom;
			for (var i = 0; i < drop; i++)
			{
				actions.Add(ActionType.Drop);
			}
			return new MoveInstruction(actions.ToArray());
		}
	}

	
}
