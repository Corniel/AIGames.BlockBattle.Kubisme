namespace AIGames.BlockBattle.Kubisme
{
	public interface IEvaluator
	{
		int GetScore(Field field, int depth);
		IParameters Parameters { get; set; }
		int WinScore { get; }
		int DrawScore { get; }
		int LostScore { get; }
	}
}
