using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGames.BlockBattle.Kubisme.UnitTests.DecisionMaking
{
	public class NodeDecisionMakerTester : NodeDecisionMaker
	{
		public List<string> Logs = new List<string>();

		public override BlockPath GetMove(Field field, Block current, Block next, int round)
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
				Logs.Add(Pars.ToString());
			}
			BestField = Root.BestField;
			return Root.BestMove;
		}
	}
}
