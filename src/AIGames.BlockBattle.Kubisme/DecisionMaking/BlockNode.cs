using AIGames.BlockBattle.Kubisme.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.DecisionMaking
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

		public void Apply(byte depth, ApplyParameters pars)
		{
			if (depth > Depth  && depth <= pars.MaximumDepth && pars.HasTimeLeft)
			{
				if (Children == null)
				{
					Children = new List<T>();
					var block = GetBlock(pars);
					foreach (var candidate in pars.Generator.GetMoves(Field, block, Position.Start))
					{
						if (!pars.HasTimeLeft) { return; }
						T child = Create(Depth, candidate.Field, candidate.Path, pars);
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

		protected abstract T Create(byte depth, Field field, MovePath path, ApplyParameters pars);
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
