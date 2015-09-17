using AIGames.BlockBattle.Kubisme.UnitTests.Evaluation;
using System.Collections.Generic;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.UnitTests.DecisionMaking
{
	public class NodeDecisionMakerTester : NodeDecisionMaker
	{
		public NodeDecisionMakerTester()
			: base(new MT19937Generator(17)) { }

		public List<string> Logs = new List<string>();

		public BlockPath GetMove(Field field, Block current, Block next, int round)
		{
			return GetMove(field, new OpponentStub(), current, next, round);
		}

		public override BlockPath GetMove(Field field, Opponent opponent, Block current, Block next, int round)
		{
			Pars = new ApplyParameters(Rnd)
			{
				Round = round,
				MaximumDuration = MaximumDuration,
				MaximumDepth = MaximumDepth,
				Evaluator = Evaluator,
				Generator = Generator,
				Current = current,
				Next = next,
				Opponent = opponent,
			};
			Root = new BlockRootNode(field);

			while (Pars.Depth < Pars.MaximumDepth && Pars.HasTimeLeft)
			{
				Root.Apply(++Pars.Depth, Pars);
				var log = string.Format("{0:0.00} {1}: {2}", Root.Children[0].Score/(double)100, Pars, Root.BestMove);
				Logs.Add(log);
			}
			BestField = Root.BestField;
			return Root.BestMove;
		}
	}
}
