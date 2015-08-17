using System;
using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme
{
	public class BlockRndNodes : List<BlockRndNode> , IComparable, IComparable<BlockRndNodes>
	{
		public BlockRndNodes(Block block) : base(block.ChildCount) { }

		public int Score { get { return Count == 0 ? short.MinValue : this[0].Score; } }

		public int CompareTo(object obj)
		{
			return CompareTo((BlockRndNodes)obj);
		}

		public int CompareTo(BlockRndNodes other)
		{
			return other.Score.CompareTo(Score);
		}
	}
}
