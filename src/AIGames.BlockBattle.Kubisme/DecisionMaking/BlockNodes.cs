using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme
{
	public class BlockNodes<T> : List<T> where T : IBlockNode
	{
		public BlockNodes(Block block) : base(block.ChildCount + 2) { }
	}
}
