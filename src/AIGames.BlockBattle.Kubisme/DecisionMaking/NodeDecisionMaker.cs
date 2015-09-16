﻿using System;
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
			MaximumDuration = TimeSpan.MaxValue;
			Rnd = rnd;
		}

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
				"Round {3:00}, Points: {0:00}  {1:0.000}  {2}",
				BestField.Points,
				Root.Score / (double)parameters.Points,
				Pars,
				Pars.Round);
		}
	}
}
