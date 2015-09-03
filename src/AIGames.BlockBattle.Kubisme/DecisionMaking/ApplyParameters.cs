using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme
{
	public class ApplyParameters
	{
		private readonly Stopwatch sw;

		public ApplyParameters()
		{
			sw = Stopwatch.StartNew();
			MaximumDepth = int.MaxValue;
			Points = new int[10];
			Rnd = new MT19937Generator();
		}

		public int[] Points { get; set; }
		public int Round { get; set; }

		public int MaximumDepth { get; set; }
		public bool HasTimeLeft { get { return Elapsed < MaximumDuration; } }
		public TimeSpan MaximumDuration { get; set; }
		public TimeSpan Elapsed { get { return sw.Elapsed; } }

		public IMoveGenerator Generator { get; set; }
		public IEvaluator Evaluator { get; set; }
		public MT19937Generator Rnd { get; set; }
		public Opponent Opponent { get; set; }

		public Block Current { get; set; }
		public Block Next { get; set; }
		public int Evaluations{ get; set; }
		public byte Depth { get; set; }

		[ExcludeFromCodeCoverage]
		public override string ToString()
		{
			return String.Format("{0:0.000}s ({1} depth) {2:#,##0.0}kN ({3:#,##0.0}kN/s)", Elapsed.TotalSeconds, Depth, Evaluations / 1000d, Evaluations / Elapsed.TotalMilliseconds);
		}
	}
}
