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
		public static readonly int[] TestNodes = new int[] { 1, 4 };
		public const int BranchingFactor = 2;

		public BlockRndNode(Field field, byte depth, int score)
		{
			this.Field = field;
			this.Depth = ++depth;
			this.Score = score;
			this.ScoreField = score;
		}

		public List<BlockRndNodes> Children { get; protected set; }

		public void Apply(byte depth, ApplyParameters pars)
		{
			if (depth > Depth && depth <= pars.MaximumDepth && pars.HasTimeLeft)
			{
				if (Children == null)
				{
					Children = new List<BlockRndNodes>();

					foreach(var block in Block.All)
					{
						var nodes = new BlockRndNodes();
						foreach (var field in pars.Generator.GetFields(Field, block, false))
						{
							if (!pars.HasTimeLeft) { return; }
							var score = pars.Evaluator.GetScore(field);
							var locks = (pars.Points[Depth + 1] >> 2) - 20 + field.RowCount;
							var child = new BlockRndNode(locks > 0 ? field.LockRows(locks) : field, Depth, score);
							pars.Evaluations++;
							nodes.Add(child);
						}
						nodes.Sort();
						Children.Add(nodes);
					}
				}
				else
				{
					if (Children.Count == 7)
					{
						foreach (var test in TestNodes)
						{
							var nodes = Children[test];
							foreach (var child in nodes.Take(BranchingFactor))
							{
								child.Apply(depth, pars);
							}
							nodes.Sort();
						}
					}
					else
					{
						foreach (var nodes in Children)
						{
							foreach (var child in nodes.Take(BranchingFactor))
							{
								child.Apply(depth, pars);
							}
							nodes.Sort();
						}
					}
					Children.Sort();
				}
				Score = ScoreField;
				foreach (var nodes in Children)
				{
					Score += nodes.Score;
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
