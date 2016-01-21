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
			return GetMove(field, current, next, round, EvaluatorParameters.GetDefault());
		}
		public BlockPath GetMove(Field field, Block current, Block next, int round, EvaluatorParameters pars)
		{
			return GetMove(field, Field.Empty, current, next, round, pars);
		}
		public BlockPath GetMove(Field field, Field opponent, Block current, Block next, int round, EvaluatorParameters pars)
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
				Parameters = pars,
			};
			Root = new BlockRootNode(field);

			var oppo = new OpponentEvaluator() { Generator = Pars.Generator };
			Pars.Opponent = oppo.Evaluate(opponent, current, next, MaximumDepth > 2);
			Logs.Add(new PlyLog(Pars.Round, BlockPath.None, 0, 0, Pars.Elapsed, Pars.Opponent.Count));

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
