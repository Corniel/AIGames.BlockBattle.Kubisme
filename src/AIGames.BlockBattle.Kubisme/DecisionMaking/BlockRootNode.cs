using System;
using System.Collections.Generic;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme
{
	public class BlockRootNode : BlockNode<Block1Node>
	{
		public BlockRootNode(Field field) : base(field, 0) { }

		public override byte Depth { get { return 0; } }
		public override int BranchingFactor { get { return 16; } }

		public BlockPath BestMove { get { return Children == null || Children.Count == 0 ? BlockPath.None : Children[0].Path; } }
		public Field BestField { get { return Children == null || Children.Count == 0 ? Field.Empty : Children[0].Field; } }

		public override void Apply(byte depth, ApplyParameters pars)
		{
			if (depth > Depth && depth <= pars.MaximumDepth && pars.HasTimeLeft)
			{
				if (Children == null)
				{
					var block = GetBlock(pars);
					Children = new BlockNodes<Block1Node>(block);
					foreach (var candidate in pars.Generator.GetMoves(Field, block, true))
					{
						if (!pars.HasTimeLeft) { return; }
						Block1Node child = Create(Depth, BlockNode.Apply(candidate.Field, Depth, pars), candidate.Path, pars);
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
