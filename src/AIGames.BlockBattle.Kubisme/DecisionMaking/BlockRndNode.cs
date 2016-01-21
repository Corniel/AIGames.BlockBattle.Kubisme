using System;
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
			this.Field = field;
			this.Depth = ++depth;
			this.Score = score;
			this.ScoreField = score;
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
						foreach (var field in pars.Generator.GetFields(Field, block, Depth == 2))
						{
							if (!pars.HasTimeLeft) { return; }

							var applied = BlockNode.Apply(field, Depth, pars, block);
							if (!applied.IsNone)
							{
								var child = Create(applied, pars, block);

								// We can kill our opponent.
								if (Depth == 2 && child.Field.Points > Field.Points)
								{
									var garbageNew = child.Field.Points / 3;
									if (garbageNew - pars.Garbage > pars.Opponent.FirstFilled3[block])
									{
										child.SetFinalScore(Scores.Wins(3));
									}
								}
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

		protected BlockRndNode Create(Field field, ApplyParameters pars, Block block)
		{
			var score = pars.Evaluator.GetScore(field, pars.Parameters, Depth, pars.Opponent, block);
			pars.Evaluations++;
			return new BlockRndNode(field, Depth, score);
		}

		public void SetFinalScore(int score)
		{
			this.Score = score;
			this.Children = new List<BlockNodes<BlockRndNode>>();
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
				return String.Format("{0:#,##0}, Depth: {1}, Children: {2}", Score, Depth, Children == null ? 0 : Children.SelectMany(ch => ch).Count());
			}
		}
	}
}
