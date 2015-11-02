
namespace AIGames.BlockBattle.Kubisme
{
	public static class BlockNode
	{
		/// <summary>Applies garbage and locks to the field.</summary>
		public static Field Apply(Field field, int depth, ApplyParameters pars)
		{
			if (((pars.Round + depth) & 15) == 15)
			{
				field = field.LockRow();
			}
			// Just to create some garbage, add it 'randomly'.
			if ((depth & 1) == 1)
			{
				return field.Garbage(Row.GetGarbage(1, field.Points, pars.Rnd));
			}
			return field;
		}
	}
}
