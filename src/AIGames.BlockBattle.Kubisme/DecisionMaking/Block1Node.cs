﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AIGames.BlockBattle.Kubisme
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class Block1Node : BlockNode<BlockRndNode>
	{
		public Block1Node(Field field, BlockPath path, int score, int branchingfactor)
			: base(field, score) 
		{
			Path = path;
			BranchingFactor = branchingfactor;
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
						foreach (var field in pars.Generator.GetFields(applied, block))
						{
							if (!pars.HasTimeLeft) { return; }
							var child = Create(field, pars);

							// We can kill our opponent.
							if (child.Field.Points > Field.Points)
							{
								var garbageNew = child.Field.Points / 3;
								if (garbageNew - pars.Garbage > pars.Opponent.FirstFilled2)
								{
									child.SetFinalScore(Scores.Wins(2));
								}
							}
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
			}
		}

		public void SetFinalScore(int score)
		{
			Score = score;
			Children = BlockNodes<BlockRndNode>.GetEmpty();
		}

		protected override BlockRndNode Create(Field field, ApplyParameters pars)
		{
			var score = pars.Evaluator.GetScore(field, Depth, pars.Parameters);
			pars.Evaluations++;
			return new BlockRndNode(field, Depth, score);
		}

		protected override Block GetBlock(ApplyParameters pars) { return pars.Next; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never), ExcludeFromCodeCoverage]
		private string DebuggerDisplay
		{
			get
			{
				return string.Format("{0}, Depth: {1}, Children: {2}, Path: {3}",
					Scores.GetFormatted(Score), 
					Depth,
					Children == null ? 0 : Children.Count, 
					Path);
			}
		}
	}
}
