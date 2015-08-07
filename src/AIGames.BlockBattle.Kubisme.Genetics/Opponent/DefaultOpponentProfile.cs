namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class DefaultOpponentProfile : IOpponentProfile
	{
		public Field Apply(Field field, int turn)
		{
			if (turn % 20 == 0)
			{
				return field.LockRow();
			}
			return field;
		}

		public bool IsAlive(Field field, int turn)
		{
			return field.Points < 80;
		}
	}
}
