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
			MinimumDuration = TimeSpan.Zero;
			MaximumDuration = TimeSpan.MaxValue;
			Rnd = rnd;
		}

		public TimeSpan MinimumDuration { get; set; }
		public TimeSpan MaximumDuration { get; set; }
		public int MaximumDepth { get; set; }
		public MT19937Generator Rnd { get; set; }

		public IEvaluator Evaluator { get; set; }
		public IMoveGenerator Generator { get; set; }
		public BlockRootNode Root { get; protected set; }
		public ApplyParameters Pars { get; protected set; }
		public Field BestField { get; protected set; }

		public virtual BlockPath GetMove(Field field, Opponent opponent, Block current, Block next, int round)
		{
			Logs.Clear();
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

			while (Pars.Depth < Pars.MaximumDepth && Pars.Elapsed < MinimumDuration)
			{
				Root.Apply(++Pars.Depth, Pars);
				
				var parameters = (ComplexParameters)Evaluator.Parameters;
				var log = string.Format("{0:0.00} {1}: {2}", Root.Score / (double)parameters.Points, Pars, Root.BestMove);
				Logs.Add(log);
			}
			BestField = Root.BestField;
			return Root.BestMove;
		}

		[ExcludeFromCodeCoverage]
		public string GetLog()
		{
			var parameters = (ComplexParameters)Evaluator.Parameters;

			return string.Format(
				CultureInfo.InvariantCulture,
				"Round {0:00}, Points: {1:00}\r\n{2}",
				Pars.Round,
				BestField.Points,
				String.Join("\r\n", Logs));
		}
		private List<String> Logs = new List<string>();
	}
}
