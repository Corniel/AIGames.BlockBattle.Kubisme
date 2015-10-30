using System;
using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme.Communication
{
	public class MoveInstruction : IInstruction
	{
		public static readonly MoveInstruction NoMoves = new MoveInstruction(new ActionType[0]);

		public MoveInstruction(params ActionType[] actions)
		{
			Actions = new List<ActionType>();

			if (actions.Length == 1 && actions[0] == ActionType.Skip)
			{
				// skip is a move on its own.
				Actions.Add(ActionType.Skip);
			}
			else if (actions.Length != 0)
			{
				var skip = true;

				Actions.Add(ActionType.Drop);
				for (var i = actions.Length - 1; i >= 0; i--)
				{
					var action = actions[i];

					skip &= action == ActionType.Down || action == ActionType.Drop;
					
					// Skip and none should not be sent.
					if (!skip && action != ActionType.Skip && action != ActionType.None)
					{
						Actions.Insert(0, action);
					}
				}
			}
		}

		private readonly List<ActionType> Actions;

		public override string ToString()
		{
			if (Actions.Count == 0) { return "no_moves"; }
			return String.Join(",", Actions).ToLowerInvariant();
		}
	}
}
