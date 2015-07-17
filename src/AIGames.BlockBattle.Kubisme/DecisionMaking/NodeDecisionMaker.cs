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

		public MovePath GetMove(Field field, Position position, Block current, Block next)
		{
			var pars = new ApplyParameters()
			{
				MaximumDuration = MaximumDuration,
				MaximumDepth = MaximumDepth,
				Evaluator = Evaluator,
				Generator = Generator,
				Current = current,
				Next = next,
			};
			var root = new BlockRootNode(field);

			while (pars.Depth++ < pars.MaximumDepth && pars.HasTimeLeft)
			{
				root.Apply(pars.Depth, pars);
			}
			return root.BestMove;
		}
	}
}
