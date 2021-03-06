﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class BlockRndNode : IBlockNode
	{
		public const int BranchingFactor = 2;

		public BlockRndNode(Field field, byte depth, int score)
		{
			Field = field;
			Depth = ++depth;
			Score = score;
			ScoreField = score;
		}

		public List<BlockNodes<BlockRndNode>> Children { get; protected set; }

		/// <summary>Applies the search on the current node.</summary>
		/// <param name="depth">
		/// The maximum depth to search.
		/// </param>
		/// <param name="pars">
		/// The parameters needed to apply the search.
		/// </param>
		public void Apply(byte depth, ApplyParameters pars)
		{
			if (depth > Depth && depth <= pars.MaximumDepth && pars.HasTimeLeft)
			{
				if (Children == null)
				{
					Children = new List<BlockNodes<BlockRndNode>>();

					foreach(var block in pars.Blocks[Depth])
					{
						var nodes = new BlockNodes<BlockRndNode>(block);
						foreach (var field in pars.Generator.GetFields(Field, block))
						{
							if (!pars.HasTimeLeft) { return; }

							var applied = BlockNode.Apply(field, Depth, pars);
							if (!applied.IsNone)
							{
								var child = Create(applied, pars);
								nodes.InsertSorted(child);
							}
						}
						Children.Add(nodes);
					}
				}
				else
				{
					foreach (var nodes in Children)
					{
						nodes.Apply(depth, pars, BranchingFactor);
					}
				}
				Score = 0;
				foreach (var nodes in Children)
				{
					Score += nodes.GetScore(this);
				}
				if (Children.Count == 2)
				{
					// Divide by 2.
					Score >>= 1;
				}
				// We can not find valid responses. We will lose next turn.
				else if (Children.Count == 0)
				{
					Score = Scores.Loses(Depth + 1);
				}
				else
				{
					Score /= Children.Count;
				}
			}
		}

		protected BlockRndNode Create(Field field, ApplyParameters pars)
		{
			var score = pars.Evaluator.GetScore(field, Depth, pars.Parameters);
			pars.Evaluations++;
			return new BlockRndNode(field, Depth, score);
		}

		public void SetFinalScore(int score)
		{
			Score = score;
			Children = new List<BlockNodes<BlockRndNode>>();
		}

		public byte Depth { get; protected set; }
		public Field Field { get; protected set; }
		public int Score { get; protected set; }
		public int ScoreField { get; protected set; }

		public int CompareTo(object obj) { return CompareTo((IBlockNode)obj); }
		public int CompareTo(IBlockNode other) { return other.Score.CompareTo(Score); }

		[DebuggerBrowsable(DebuggerBrowsableState.Never), ExcludeFromCodeCoverage]
		private string DebuggerDisplay
		{
			get
			{
				return string.Format("{0}, Depth: {1}, Children: {2}",
					Scores.GetFormatted(Score),
					Depth,
					Children == null ? 0 :
					Children.SelectMany(ch => ch).Count());
			}
		}
	}
}
