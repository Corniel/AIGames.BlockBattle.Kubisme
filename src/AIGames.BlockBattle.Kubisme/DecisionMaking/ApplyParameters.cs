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
			Rnd = rnd;
			Blocks = new Block[size][];

			Blocks[2] = Block.All;
			Blocks[3] = Block.SubsetsOf3[Rnd.Next(Block.SubsetsOf3.Length)];
			for (var i = 4; i < size; i++)
			{
				Blocks[i] = Block.SubsetsOf2[Rnd.Next(Block.SubsetsOf2.Length)];
			}
		}

		public Block[][] Blocks { get; protected set; }

		public int Round { get; set; }

		public OpponentEvaluation Opponent { get; set; }

		public int MaximumDepth { get; set; }
		public bool HasTimeLeft { get { return Elapsed < MaximumDuration; } }
		public TimeSpan MaximumDuration { get; set; }
		public TimeSpan Elapsed { get { return sw.Elapsed; } }

		public IMoveGenerator Generator { get; set; }
		public Evaluator Evaluator { get; set; }
		public EvaluatorParameters Parameters { get; set; }
		public MT19937Generator Rnd { get; set; }

		public Block Current { get; set; }
		public Block Next { get; set; }
		public int Evaluations { get; set; }
		public byte Depth { get; set; }
		public int Garbage { get; set; }

		/// <summary>The initial value of first filled.</summary>
		/// <remarks>
		/// Used to generate garbage during calculations.
		/// </remarks>
		public int FirstFilled { get; set; }

		[ExcludeFromCodeCoverage]
		public override string ToString()
		{
			return String.Format("{0:0.000}s ({1} depth) {2:#,##0.0}kN ({3:#,##0.0}kN/s)", Elapsed.TotalSeconds, Depth, Evaluations / 1000d, Evaluations / Elapsed.TotalMilliseconds);
		}
	}
}
