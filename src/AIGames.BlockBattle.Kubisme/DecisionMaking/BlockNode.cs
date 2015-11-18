
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
			if (depth == 1 && pars.Opponent.MaximumGarbage > 0)
			{
				var garbage = Row.GetGarbage(pars.Opponent.MaximumGarbage, pars.Opponent.PointsOld, pars.Rnd);
				return field.Garbage(garbage);
			}
			return field;
		}
	}
}
