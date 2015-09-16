using System.Collections.Generic;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme
{
	public class BlockNodes<T> : List<T> where T : IBlockNode
	{
		public BlockNodes(Block block) : base(block.ChildCount + 2) { }

		/// <summary>Gets the score of the best (first) node.</summary>
		public int Score { get { return Count == 0 ? short.MinValue : this[0].Score; } }

		/// <summary>Applies the search on the child nodes.</summary>
		/// <param name="depth">
		/// The maximum depth to search.
		/// </param>
		/// <param name="pars">
		/// The parameters needed to apply the search.
		/// </param>
		/// <param name="branchingFactor">
		/// The maximum branching factor.
		/// </param>
		public int Apply(byte depth, ApplyParameters pars, int branchingFactor)
		{
			var score = pars.Evaluator.LostScore;

			foreach (var child in this.Take(branchingFactor))
			{
				var searchDepth = depth;

				// divide by 64.
				var delta = (this[0].Score - child.Score) >> 6;
				if (depth >= delta)
				{
					searchDepth -= (byte)delta;
				}
				child.Apply(searchDepth, pars);
			}

			if (Count > 0)
			{
				Sort();

				score = this[0].Score;
			}
			return score;
		}

		/// <summary>Inserts the children sorted.</summary>
		public void InsertSorted(T child)
		{
			for (var i = 0; i < Count; i++)
			{
				if (this[i].Score < child.Score)
				{
					Insert(i, child);
					return;
				}
			}
			Add(child);
		}
	}
}
