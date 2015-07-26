namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public interface IOpponentProfile
	{
		Field Apply(Field field, int turn);
		bool IsAlive(Field field, int turn);
	}
}
