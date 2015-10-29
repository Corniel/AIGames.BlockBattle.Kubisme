using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme
{
	public class ApplyParameters
	{
		private readonly Stopwatch sw;

		public ApplyParameters(MT19937Generator rnd)
		{
			const int size = 16;
			sw = Stopwatch.StartNew();
			MaximumDepth = int.MaxValue;
			Points = new int[size];
			Rnd = rnd;
			Blocks = new Block[size][];

			for (var i = 0; i < 4; i++)
			{
				Blocks[i] = Block.SubsetsOf3[Rnd.Next(Block.SubsetsOf3.Length)];
			}
			for (var i = 4; i < size; i++)
			{
				Blocks[i] = Block.SubsetsOf2[Rnd.Next(Block.SubsetsOf2.Length)];
			}
		}

		public Block[][] Blocks { get; protected set; }

		public int[] Points { get; set; }
		public int Round { get; set; }

		public int MaximumDepth { get; set; }
		public bool HasTimeLeft { get { return Elapsed < MaximumDuration; } }
		public TimeSpan MaximumDuration { get; set; }
		public TimeSpan Elapsed { get { return sw.Elapsed; } }

		public IMoveGenerator Generator { get; set; }
		public IEvaluator Evaluator { get; set; }
		public MT19937Generator Rnd { get; set; }

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
