using AIGames.BlockBattle.Kubisme.Evaluation;
using AIGames.BlockBattle.Kubisme.Models;
using System;
using System.Diagnostics;

namespace AIGames.BlockBattle.Kubisme.DecisionMaking
{
	public class ApplyParameters
	{
		private readonly Stopwatch sw;

		public ApplyParameters()
		{
			sw = Stopwatch.StartNew();
			MaximumDepth = int.MaxValue;
		}

		public int MaximumDepth { get; set; }
		public bool HasTimeLeft { get { return Elapsed < MaximumDuration; } }
		public TimeSpan MaximumDuration { get; set; }
		public TimeSpan Elapsed { get { return sw.Elapsed; } }

		public IMoveGenerator Generator { get; set; }
		public IEvaluator Evaluator { get; set; }

		public Block Current { get; set; }
		public Block Next { get; set; }
		public int Evaluations{ get; set; }
		public byte Depth { get; set; }

		public override string ToString()
		{
			return String.Format("{0:0.000}s ({1} depth) {2:#,##0.0}kN ({3:#,##0.0}kN/s)", Elapsed.TotalSeconds, Depth, Evaluations / 1000d, Evaluations / Elapsed.TotalMilliseconds);
		}
	}
}
