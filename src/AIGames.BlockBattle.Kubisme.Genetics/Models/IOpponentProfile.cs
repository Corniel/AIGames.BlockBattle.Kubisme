using AIGames.BlockBattle.Kubisme.Models;

namespace AIGames.BlockBattle.Kubisme.Genetics.Models
{
	public interface IOpponentProfile
	{
		Field Apply(Field field, int turn);
		bool IsAlive(Field field, int turn);
	}
}
