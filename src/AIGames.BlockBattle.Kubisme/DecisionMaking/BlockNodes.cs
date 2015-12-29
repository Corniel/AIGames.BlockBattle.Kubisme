using System;
using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme
{
	public class BlockNodes<T> : List<T> where T : IBlockNode
	{
		/// <summary>Gets an empty collection of Block nodes.</summary>
		public static BlockNodes<T> GetEmpty()
		{
			return new BlockNodes<T>();
		}
		private BlockNodes() { }

		public BlockNodes(Block block) : base(block.ChildCount + 2) { }

		/// <summary>Gets the score of the best (first) node.</summary>
		public int GetScore(IBlockNode node)
		{
			if (Empty())
			{
				return  Scores.Loses(node.Depth);
			}
			return this[0].Score;
		}

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
		public void Apply(byte depth, ApplyParameters pars, int branchingFactor)
		{
			if (Empty() || depth == 0) { return; }

			var threshold = this[Math.Min(Count, branchingFactor) - 1].Score;
			var depthMin1 = (byte)(depth - 1);
			var dephtMin1Threshold = threshold + pars.Parameters.HolesUnreachable;

			foreach (var child in this)
			{
				// Only checks moves that are not worse than the threshold.
				if (child.Score < threshold) { break; }

				if (child.Score > dephtMin1Threshold)
				{
					child.Apply(depth, pars);
				}
				else
				{
					child.Apply(depthMin1, pars);
				}

				// we find a lower move, look for others that are potentially better.
				if (child.Score < threshold)
				{
					threshold = child.Score;
				}
			}
			base.Sort();
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
		/// 
		/// If the branching factor is higher, it's slower.
		/// </remarks>
		protected void Sort(int lastIndex)
		{
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
