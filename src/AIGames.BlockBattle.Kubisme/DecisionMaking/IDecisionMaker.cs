namespace AIGames.BlockBattle.Kubisme
{
	public interface IDecisionMaker
	{
		IEvaluator Evaluator { get; set; }
		IMoveGenerator Generator { get; set; }
		int[] Points { get; set; }
		Field BestField { get; }

		BlockPath GetMove(Field field, Block current, Block next, int round);
	}
}
