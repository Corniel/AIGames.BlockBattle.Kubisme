using System;

namespace AIGames.BlockBattle.Kubisme
{
	public class NodeDecisionMaker : IDecisionMaker
	{
		public NodeDecisionMaker()
		{
			MaximumDepth = int.MaxValue;
			MaximumDuration = TimeSpan.MaxValue;
		}

		public int[] Points { get; set; }
		public TimeSpan MaximumDuration { get; set; }
		public int MaximumDepth { get; set; }
		public IEvaluator Evaluator { get; set; }
		public IMoveGenerator Generator { get; set; }
		public BlockRootNode Root { get; protected set; }
		public ApplyParameters Pars { get; protected set; }
		public Field BestField { get; protected set; }

		public BlockPath GetMove(Field field, Position position, Block current, Block next)
		{
			Pars = new ApplyParameters()
			{
				MaximumDuration = MaximumDuration,
				MaximumDepth = MaximumDepth,
				Evaluator = Evaluator,
				Generator = Generator,
				Current = current,
				Next = next,
				Points = Points,
			};
			Root = new BlockRootNode(field);

			while (Pars.Depth < Pars.MaximumDepth && Pars.HasTimeLeft)
			{
				Root.Apply(++Pars.Depth, Pars);
			}
			BestField = Root.BestField;
			return Root.BestMove;
		}
	}
}
