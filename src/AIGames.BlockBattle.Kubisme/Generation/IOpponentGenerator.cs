namespace AIGames.BlockBattle.Kubisme
{
	public interface IOpponentGenerator
	{
		Opponent Create(int turn, Field field, Block current, Block next);
	}
}
