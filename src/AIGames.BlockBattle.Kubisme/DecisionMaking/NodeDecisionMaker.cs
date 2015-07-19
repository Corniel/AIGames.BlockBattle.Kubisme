using AIGames.BlockBattle.Kubisme.Evaluation;
using AIGames.BlockBattle.Kubisme.Models;
using System;

namespace AIGames.BlockBattle.Kubisme.DecisionMaking
{
	public class NodeDecisionMaker : IDecisionMaker
	{
		public NodeDecisionMaker()
		{
			MaximumDepth = int.MaxValue;
		}

		public TimeSpan MaximumDuration { get; set; }
		public int MaximumDepth { get; set; }
		public IEvaluator Evaluator { get; set; }
		public IMoveGenerator Generator { get; set; }
		public BlockRootNode Root { get; protected set; }
		public ApplyParameters Pars { get; protected set; }

		public MovePath GetMove(Field field, Position position, Block current, Block next)
		{
			Pars = new ApplyParameters()
			{
				MaximumDuration = MaximumDuration,
				MaximumDepth = MaximumDepth,
				Evaluator = Evaluator,
				Generator = Generator,
				Current = current,
				Next = next,
			};
			Root = new BlockRootNode(field);

			while (Pars.Depth++ < Pars.MaximumDepth && Pars.HasTimeLeft)
			{
				Root.Apply(Pars.Depth, Pars);
			}
			return Root.BestMove;
		}
	}
}
