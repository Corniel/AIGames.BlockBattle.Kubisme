using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AIGames.BlockBattle.Kubisme
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class Block1Node : BlockNode<BlockRndNode>
	{
		public Block1Node(Field field, BlockPath path, int score, int branchingfactor)
			: base(field, score) 
		{
			this.Path = path;
			this.BranchingFactor = branchingfactor;
		}

		public override byte Depth { get { return 1; } }

		public BlockPath Path { get; protected set; }

		/// <summary>Applies the search on the current node.</summary>
		/// <param name="depth">
		/// The maximum depth to search.
		/// </param>
		/// <param name="pars">
		/// The parameters needed to apply the search.
		/// </param>
		public override void Apply(byte depth, ApplyParameters pars)
		{
			if (depth > Depth && depth <= pars.MaximumDepth && pars.HasTimeLeft)
			{
				if (Children == null)
				{
					var block = GetBlock(pars);
					Children = new BlockNodes<BlockRndNode>(block);

					var applied = BlockNode.Apply(Field, Depth, pars);
					if (!applied.IsNone)
					{
						foreach (var field in pars.Generator.GetFields(applied, block, true))
						{
							if (!pars.HasTimeLeft) { return; }
							var child = Create(field, pars);
							Children.InsertSorted(child);
						}
					}
				}
				else
				{
					if (Children.Empty()) { return; }
					Children.Apply(depth, pars, BranchingFactor);
				}
				Score = Children.GetScore(this);
				
				// if we where close to dying, we don't want to forget that, if can
				// clean it up later.
				if (Field.FirstFilled < 4)
				{
					Score += pars.Evaluator.Pars.EmptyRows[Field.FirstFilled];
				}
			}
		}

		public void SetFinalScore(int score)
		{
			this.Score = score;
			this.Children = BlockNodes<BlockRndNode>.GetEmpty();
		}

		protected override BlockRndNode Create(Field field, ApplyParameters pars)
		{
			var score = pars.Evaluator.GetScore(field, Depth);
			pars.Evaluations++;
			return new BlockRndNode(field, Depth, score);
		}

		protected override Block GetBlock(ApplyParameters pars) { return pars.Next; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never), ExcludeFromCodeCoverage]
		private string DebuggerDisplay
		{
			get
			{
				return String.Format("{0:#,##0}, Depth: {1}, Children: {2}, Path: {3}", Score, Depth, Children == null ? 0 : Children.Count, Path);
			}
		}
	}
}
