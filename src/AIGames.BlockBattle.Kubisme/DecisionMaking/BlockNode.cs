
namespace AIGames.BlockBattle.Kubisme
{
	public static class BlockNode
	{
		/// <summary>Applies garbage and locks to the field.</summary>
		public static Field Apply(Field field, int depth, ApplyParameters pars)
		{
			// Lock a row every 15 turns. 
			// Alternatively Just to create some garbage. We use a lock because adding
			// a garbage line adds to much randomness to the outcome.
			if (depth == 1)
			{
				return field.SimulateGarbage(pars.FirstFilled);
			}
			else if ((pars.Round + depth) % 15 == 0 || (depth & 1) == 1)
			{
				return field.LockRow();
			}
			return field;
		}
	}
}
