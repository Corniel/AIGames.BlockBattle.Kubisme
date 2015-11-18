using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme
{
	public interface IDecisionMaker
	{
		Evaluator Evaluator { get; set; }
		IMoveGenerator Generator { get; set; }
		Field BestField { get; }

		BlockPath GetMove(Field field, Field opponent, Block current, Block next, int round);
	}
}
