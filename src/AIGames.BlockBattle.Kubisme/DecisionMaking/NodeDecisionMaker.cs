using System;
using System.Globalization;

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

		public BlockPath GetMove(Field field, Block current, Block next, int round)
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

		public string GetLog()
		{
			var parameters = (SimpleParameters)Evaluator.Parameters;

			return string.Format(
				CultureInfo.InvariantCulture,
				"Round {3:00}, Points: {0:00}  {1:0.000}  {2}",
				BestField.Points,
				Root.Score / (double)parameters.Points,
				Pars,
				Pars.Round);
		}
	}
}
