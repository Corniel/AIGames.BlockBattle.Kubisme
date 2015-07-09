using AIGames.BlockBattle.Kubisme.Models;
using System;
using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme.Communication
{
	public class MoveInstruction : IInstruction
	{
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

		public static MoveInstruction Create(Position source, MovePath path)
		{
			var actions = new List<ActionType>();

			switch (path.Option)
			{
				case Block.RotationType.Right: actions.Add(ActionType.TurnRight); break;
				case Block.RotationType.Uturn: actions.Add(ActionType.TurnLeft); actions.Add(ActionType.TurnLeft); break;
				case Block.RotationType.Left: actions.Add(ActionType.TurnLeft); break;
			}
			var delta = source.Col - path.Target.Col;
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
			for (var i = 0; i < path.Target.Row - source.Row; i++)
			{
				actions.Add(ActionType.Drop);
			}
			return new MoveInstruction(actions.ToArray());
		}
	}

	
}
