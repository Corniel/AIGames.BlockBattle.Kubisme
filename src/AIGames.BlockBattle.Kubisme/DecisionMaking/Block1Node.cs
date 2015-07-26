using System;

namespace AIGames.BlockBattle.Kubisme
{
	public class Block1Node : BlockNode<BlockRndNode>
	{
		public Block1Node(Field field, BlockPath path, int score)
			: base(field, score) 
		{
			this.Path = path;
		}

		public override byte Depth { get { return 1; } }
		public override int BranchingFactor { get { return 8; } }

		public BlockPath Path { get; protected set; }

		protected override BlockRndNode Create(byte depth, Field field, ApplyParameters pars)
		{
			var score = pars.Evaluator.GetScore(field);
			return new BlockRndNode(field,  depth, score);
		}

		protected override Block GetBlock(ApplyParameters pars) { return pars.Next; }
	}
}
