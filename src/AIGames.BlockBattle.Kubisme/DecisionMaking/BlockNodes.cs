using System;
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
			if (Count == 0)
			{
				return pars.Evaluator.LostScore;
			}

			var lastIndex = Math.Min(Count, branchingFactor);

			var max = this[0].Score;
			for (var i = 0; i < lastIndex; i++)
			{
				var child = this[i];

				// divide by 128.
				var delta = (max - child.Score) >> 7;
				var searchDepth = depth - delta;

				if (searchDepth >= depth)
				{
					child.Apply((byte)searchDepth, pars);
				}
				else
				{
					lastIndex = i - 1;
					break;
				}
			}
			Sort(lastIndex);
			return this[0].Score;
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

		/// <summary>Sorts the list.</summary>
		/// <remarks>
		/// This is an optimized form of bubble search.
		/// Because most of the list keeps it order, it is the optimal approach.
		/// </remarks>
		protected void Sort(int lastIndex)
		{
			lastIndex = Math.Min(Count - 1, lastIndex);
			for (var index = lastIndex; index >= 0; index--)
			{
				var val = this[index].Score;

				for (var swap = index + 1; swap < Count; swap++)
				{
					var other = this[swap];

					if (val >= other.Score) { break; }

					this[swap] = this[swap - 1];
					this[swap - 1] = other;
				}
			}
		}

		public bool Empty() { return Count == 0; }
	}
}
