namespace AIGames.BlockBattle.Kubisme
{
	public interface IEvaluator
	{
		int GetScore(Field field, int depth);
		IParameters Parameters { get; set; }
	}
}
