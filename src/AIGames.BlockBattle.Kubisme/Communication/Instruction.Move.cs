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
			var found = true;
			while (found)
			{
				found = false;
				for (var i = 1; i < Actions.Count; i++)
				{
					var prev = Actions[i - 1];
					var curr = Actions[i];

					// Just remove useless left-rights.
					if ((prev == ActionType.Right && curr == ActionType.Left) ||
						(curr == ActionType.Right && prev == ActionType.Left))
					{
						found = true;
						Actions.RemoveRange(i - 1, 2);
						break;
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
