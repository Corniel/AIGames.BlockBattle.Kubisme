
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
				return field.LockRow();
			}
			return field;
		}
	}
}
