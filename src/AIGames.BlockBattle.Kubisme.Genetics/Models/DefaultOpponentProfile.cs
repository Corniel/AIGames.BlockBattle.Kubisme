using AIGames.BlockBattle.Kubisme.Models;

namespace AIGames.BlockBattle.Kubisme.Genetics.Models
{
	public class DefaultOpponentProfile : IOpponentProfile
	{
		public Field Apply(Field field, int turn)
		{
			if (turn % 10 == 0)
			{
				return field.LockRows(1);
			}
			return field;
		}

		public bool IsAlive(Field field, int turn)
		{
			return field.Points < 80;
		}
	}
}
