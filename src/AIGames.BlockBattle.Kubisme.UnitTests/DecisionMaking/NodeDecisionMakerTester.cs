using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme.UnitTests.DecisionMaking
{
	public class NodeDecisionMakerTester : NodeDecisionMaker
	{
		public List<string> Logs = new List<string>();

		public BlockPath GetMove(Field field, Block current, Block next, int round)
		{
			return GetMove(field, null, current, next, round);
		}

		public override BlockPath GetMove(Field field, AIGames.BlockBattle.Kubisme.Opponent opponent, Block current, Block next, int round)
		{
			Pars = new ApplyParameters()
			{
				Round = round,
				MaximumDuration = MaximumDuration,
				MaximumDepth = MaximumDepth,
				Evaluator = Evaluator,
				Generator = Generator,
				Current = current,
				Next = next,
				Opponent = new AIGames.BlockBattle.Kubisme.Opponent(round, field, 8),
			};
			Root = new BlockRootNode(field);

			while (Pars.Depth < Pars.MaximumDepth && Pars.HasTimeLeft)
			{
				Root.Apply(++Pars.Depth, Pars);
				Logs.Add(Pars.ToString());
			}
			BestField = Root.BestField;
			return Root.BestMove;
		}
	}
}
