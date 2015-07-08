using AIGames.BlockBattle.Kubisme.Models;

namespace AIGames.BlockBattle.Kubisme.Evaluation
{
	public interface IEvaluator
	{
		double GetScore(Field field);
	}
}
