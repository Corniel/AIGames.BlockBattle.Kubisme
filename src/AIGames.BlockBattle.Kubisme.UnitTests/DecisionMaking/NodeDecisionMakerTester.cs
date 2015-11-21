using System;
using System.Linq;
using System.Collections.Generic;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.UnitTests.DecisionMaking
{
	public class NodeDecisionMakerTester : NodeDecisionMaker
	{
		public NodeDecisionMakerTester()
			: base(new MT19937Generator(17)) { }

		public List<PlyLog> Logs = new List<PlyLog>();

		public BlockPath GetMove(Field field, Block current, Block next, int round)
		{
			return GetMove(field, Field.Empty, current, next, round);
		}
		public override BlockPath GetMove(Field field, Field opponent, Block current, Block next, int round)
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
				FirstFilled = field.FirstFilled,
			};
			Root = new BlockRootNode(field);

			while (Pars.Depth < Pars.MaximumDepth && Pars.HasTimeLeft)
			{
				Root.Apply(++Pars.Depth, Pars);
				Logs.Add(new PlyLog(Pars.Round, Root.BestMove, Root.Score, Pars.Depth, Pars.Elapsed, Pars.Evaluations));
				Console.WriteLine(Logs.Last());
			}
			BestField = Root.BestField;
			return Root.BestMove;
		}
	}
}
