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
		}

		public abstract byte Depth { get; }
		public abstract int BranchingFactor { get; }

		public Field Field { get; protected set; }
		public int Score { get; protected set; }
		public int ScoreField { get; protected set; }

		public List<T> Children { get; protected set; }

		public virtual void Apply(byte depth, ApplyParameters pars)
		{
			if (depth > Depth  && depth <= pars.MaximumDepth && pars.HasTimeLeft)
			{
				if (Children == null)
				{
					Children = new List<T>();
					var block = GetBlock(pars);
					foreach (var field in pars.Generator.GetFields(Field, block, true))
					{
						if (!pars.HasTimeLeft) { return; }

						var fld = (pars.Round + Depth) % 20 == 0 ? field.LockRow() : field;
						var garbage = (pars.Points[Depth + 1] >> 2) - 20 + field.RowCount;
						T child = Create(Depth, garbage > 0 ? fld.Garbage(garbage, pars.Rnd) : fld, pars);
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
}
