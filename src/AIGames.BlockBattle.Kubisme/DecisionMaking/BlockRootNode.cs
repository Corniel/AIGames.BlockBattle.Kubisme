using AIGames.BlockBattle.Kubisme.Models;

namespace AIGames.BlockBattle.Kubisme.DecisionMaking
{
	public class BlockRootNode : BlockNode<Block1Node>
	{
		public BlockRootNode(Field field) : base(field, 0) { }

		public override byte Depth { get { return 0; } }
		public override int BranchingFactor { get { return 12; } }
		
		public MovePath BestMove { get { return Children == null || Children.Count == 0 ? MovePath.None : Children[0].Path; } }

		protected override Block1Node Create(byte depth, Field field, MovePath path, ApplyParameters pars)
		{
			var score = pars.Evaluator.GetScore(field);
			return new Block1Node(field, path, score);
		}

		protected override Block GetBlock(ApplyParameters pars) { return pars.Current; }
	}
}
