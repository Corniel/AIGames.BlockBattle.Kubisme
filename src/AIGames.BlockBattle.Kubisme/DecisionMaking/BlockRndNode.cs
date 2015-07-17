using AIGames.BlockBattle.Kubisme.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.DecisionMaking
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

		public List<BlockRndNode>[] Children { get; protected set; }

		public void Apply(byte depth, ApplyParameters pars)
		{
			if (depth > Depth && depth <= pars.MaximumDepth && pars.HasTimeLeft)
			{
				if (Children == null)
				{
					Children = new List<BlockRndNode>[7];

					for (var i = 0; i < 7 && pars.HasTimeLeft; i++)
					{
						var list = new List<BlockRndNode>();
						var block = Block.All[i];
						foreach (var candidate in pars.Generator.GetMoves(Field, block, Position.Start))
						{
							if (!pars.HasTimeLeft) { return; }
							var score = pars.Evaluator.GetScore(candidate.Field);
							var child = new BlockRndNode(candidate.Field, Depth, score);
							pars.Evaluations++;
							list.Add(child);
						}
						list.Sort();
						Children[i] = list;
					}
				}
				else
				{
					foreach (var list in Children)
					{
						foreach (var child in list.Take(BranchingFactor))
						{
							child.Apply(depth, pars);
						}
						list.Sort();
					}
				}
				Score = ScoreField;
				foreach (var list in Children)
				{
					Score += list == null || list.Count == 0 ? pars.Evaluator.LostScore : list[0].Score;
				}
				// Divide by 8.
				Score >>= 3;
			}
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
