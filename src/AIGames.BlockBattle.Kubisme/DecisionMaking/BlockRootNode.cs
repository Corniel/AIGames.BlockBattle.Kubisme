using System;
using System.Collections.Generic;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme
{
	public class BlockRootNode : BlockNode<Block1Node>
	{
		public BlockRootNode(Field field) : base(field, 0) { }

		public override byte Depth { get { return 0; } }
		public override int BranchingFactor { get { return 12; } }

		public BlockPath BestMove { get { return Children == null || Children.Count == 0 ? BlockPath.None : Children[0].Path; } }
		public Field BestField { get { return Children == null || Children.Count == 0 ? Field.Empty : Children[0].Field; } }

		public override void Apply(byte depth, ApplyParameters pars)
		{
			if (depth > Depth && depth <= pars.MaximumDepth && pars.HasTimeLeft)
			{
				if (Children == null)
				{
					Children = new List<Block1Node>();
					var block = GetBlock(pars);
					foreach (var candidate in pars.Generator.GetMoves(Field, block, Position.Start, true))
					{
						if (!pars.HasTimeLeft) { return; }
						var locks = (pars.Points[Depth + 1] >> 2) - 20 + candidate.Field.RowCount;
						Block1Node child = Create(Depth, locks > 0 ? candidate.Field.LockRows(locks) : candidate.Field, candidate.Path, pars);
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

		protected Block1Node Create(byte depth, Field field, BlockPath path, ApplyParameters pars)
		{
			var score = pars.Evaluator.GetScore(field);
			return new Block1Node(field, path, score);
		}
		protected override Block1Node Create(byte depth, Field field, ApplyParameters pars) { throw new NotImplementedException(); }

		protected override Block GetBlock(ApplyParameters pars) { return pars.Current; }
	}
}
