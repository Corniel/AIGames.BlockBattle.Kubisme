using AIGames.BlockBattle.Kubisme.Evaluation;
using AIGames.BlockBattle.Kubisme.Models;

namespace AIGames.BlockBattle.Kubisme.DecisionMaking
{
	public interface IDecisionMaker
	{
		IEvaluator Evaluator { get; set; }
		IMoveGenerator Generator { get; set; }
		MovePath GetMove(Field field, Position position, Block current, Block next);
	}
}
