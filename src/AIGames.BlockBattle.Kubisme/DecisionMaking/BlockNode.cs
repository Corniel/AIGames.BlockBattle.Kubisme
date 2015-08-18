using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public abstract class BlockNode<T> : IBlockNode where T : IBlockNode
	{
		protected BlockNode(Field field, int score)
		{
			this.Field = field;
			this.Score = score;
			this.ScoreField = score;
			BranchingFactor = 2;
		}

		public abstract byte Depth { get; }
		public int BranchingFactor { get; protected set; }

		public Field Field { get; protected set; }
		public int Score { get; protected set; }
		public int ScoreField { get; protected set; }

		public BlockNodes<T> Children { get; protected set; }

		public virtual void Apply(byte depth, ApplyParameters pars)
		{
			if (depth > Depth  && depth <= pars.MaximumDepth && pars.HasTimeLeft)
			{
				if (Children == null)
				{
					var block = GetBlock(pars);
					Children = new BlockNodes<T>(block);
					foreach (var field in pars.Generator.GetFields(Field, block, true))
					{
						if (!pars.HasTimeLeft) { return; }
						T child = Create(Depth, BlockNode.Apply(field, Depth, pars), pars);
						pars.Evaluations++;
						Children.Add(child);
					}
				}
				else
				{
					foreach (var child in Children.Take(BranchingFactor))
					{
						child.Apply(depth, pars);
					}
				}
				if (Children.Count == 0)
				{
					Score = pars.Evaluator.LostScore;
				}
				else
				{
					Children.Sort();
					Score = Children[0].Score;
				}
			}
		}

		protected abstract T Create(byte depth, Field field, ApplyParameters pars);
		protected abstract Block GetBlock(ApplyParameters pars);
		
		public int CompareTo(object obj) { return CompareTo((IBlockNode)obj); }
		public int CompareTo(IBlockNode other) { return other.Score.CompareTo(Score); }
		
		[DebuggerBrowsable(DebuggerBrowsableState.Never), ExcludeFromCodeCoverage]
		private string DebuggerDisplay
		{
			get
			{
				return String.Format("{0:#,##0}, Depth: {1}, Children: {2}", Score, Depth, Children == null ? 0 : Children.Count);
			}
		}
	}

	public static class BlockNode
	{
		public static Field Apply(Field field, int depth, ApplyParameters pars)
		{
			var garbage = PointsPredictor.GetGarbageCount(pars.Points, depth);
			if ((pars.Round + depth) % 20 == 0)
			{
				field = field.LockRow();
			}
			if (garbage > 0)
			{
				return field.Garbage(Row.GetGarbage(garbage, pars.Rnd));
			}
			return field;
		}
	}
}
