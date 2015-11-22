
namespace AIGames.BlockBattle.Kubisme
{
	public static class BlockNode
	{
		/// <summary>Applies garbage and locks to the field.</summary>
		public static Field Apply(Field field, int depth, ApplyParameters pars)
		{
			// Lock a row every 15 turns. 
			if ((pars.Round + depth) % 15 == 0)
			{
				field = field.LockRow();
			}
			if (depth == 1)
			{
				if (pars.Opponent.Garbage1 > 0)
				{
					var garbage = Row.GetGarbage(pars.Opponent.Garbage1, pars.Opponent.Points0, pars.Rnd);
					return field.Garbage(garbage);
				}
			}
			else if (depth == 2 && pars.Opponent.Garbage2 > 0)
			{
				var garbage = Row.GetGarbage(pars.Opponent.Garbage2, pars.Opponent.Points1, pars.Rnd);
				return field.Garbage(garbage);
			}
			return field;
		}
	}
}
