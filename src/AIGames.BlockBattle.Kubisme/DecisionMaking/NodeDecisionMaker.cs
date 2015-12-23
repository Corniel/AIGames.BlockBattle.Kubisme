using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme
{
	public class NodeDecisionMaker : IDecisionMaker
	{
		public NodeDecisionMaker(MT19937Generator rnd)
		{
			MaximumDepth = int.MaxValue;
			MinimumDuration = TimeSpan.MaxValue;
			MaximumDuration = TimeSpan.MaxValue;
			Rnd = rnd;
		}

		public TimeSpan MinimumDuration { get; set; }
		public TimeSpan MaximumDuration { get; set; }
		public int MaximumDepth { get; set; }
		public MT19937Generator Rnd { get; set; }

		public Evaluator Evaluator { get; set; }
		public IMoveGenerator Generator { get; set; }
		public BlockRootNode Root { get; protected set; }
		public EvaluatorParameters DefaultEvaluation { get; set; }
		public EvaluatorParameters EndGameEvaluation { get; set; }
		public ApplyParameters Pars { get; protected set; }
		public Field BestField { get; protected set; }

		public virtual BlockPath GetMove(Field field, Field opponent, Block current, Block next, int round)
		{
			Logs.Clear();
			Pars = new ApplyParameters(Rnd)
			{
				Garbage = field.Points / 3,
				Round = round,
				MaximumDuration = MaximumDuration,
				MaximumDepth = MaximumDepth,
				Evaluator = Evaluator,
				Generator = Generator,
				Current = current,
				Next = next,
				FirstFilled = field.FirstFilled,
				Parameters = (field.FirstFilled < 5) ? EndGameEvaluation : DefaultEvaluation,
			};
			var oppo = new OpponentEvaluator() { Generator = Pars.Generator };
			Pars.Opponent = oppo.Evaluate(opponent, current, next);

			var move = BlockPath.None;

			Root = new BlockRootNode(field);

			// search at least two ply deep.
			while (Pars.Depth < 2 || (Pars.Depth < Pars.MaximumDepth && Pars.Elapsed < MinimumDuration))
			{
				Root.Apply(++Pars.Depth, Pars);
				Logs.Add(new PlyLog(Pars.Round, Root.BestMove, Root.Score, Pars.Depth, Pars.Elapsed, Pars.Evaluations));
			}
			BestField = Root.BestField;
			return Root.BestMove;
		}

		[ExcludeFromCodeCoverage]
		public string GetLog()
		{
			return String.Join("\r\n", Logs);
		}
		private List<PlyLog> Logs = new List<PlyLog>();
	}
}
