using System;

namespace AIGames.BlockBattle.Kubisme
{
	public class Block1Node : BlockNode<BlockRndNode>
	{
		public Block1Node(Field field, BlockPath path, int score, int branchingfactor)
			: base(field, score) 
		{
			this.Path = path;
			this.BranchingFactor = branchingfactor;
		}

		public override byte Depth { get { return 1; } }

		public BlockPath Path { get; protected set; }

		protected override BlockRndNode Create(byte depth, Field field, ApplyParameters pars)
		{
			var score = pars.Evaluator.GetScore(field, Depth);
			return new BlockRndNode(field,  depth, score);
		}

		protected override Block GetBlock(ApplyParameters pars) { return pars.Next; }
	}
}
