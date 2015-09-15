
namespace AIGames.BlockBattle.Kubisme
{
	public static class BlockNode
	{
		/// <summary>Applies garbage and locks to the field.</summary>
		public static Field Apply(Field field, int depth, ApplyParameters pars)
		{
			var garbage = pars.Opponent.States[depth].Garbage;
			if ((pars.Round + depth) % 20 == 0)
			{
				field = field.LockRow();
			}
			if (garbage > 0)
			{
				return field.Garbage(Row.GetGarbage(garbage, pars.Rnd));
			}
			return field;
		}
	}
}
