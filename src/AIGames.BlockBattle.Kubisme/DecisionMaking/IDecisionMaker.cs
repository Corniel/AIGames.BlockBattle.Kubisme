using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme
{
	public interface IDecisionMaker
	{
		IEvaluator Evaluator { get; set; }
		IMoveGenerator Generator { get; set; }
		Field BestField { get; }

		BlockPath GetMove(Field field, Opponent opponent, Block current, Block next, int round);
	}
}
