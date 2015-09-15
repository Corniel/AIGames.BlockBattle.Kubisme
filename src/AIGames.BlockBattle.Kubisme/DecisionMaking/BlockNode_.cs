using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public abstract class BlockNode<T> : IBlockNode where T : IBlockNode
	{
		protected BlockNode(Field field, int score)
		{
			this.Field = field;
			this.Score = score;
			this.ScoreField = score;
			BranchingFactor = 2;
		}

		public abstract byte Depth { get; }
		public int BranchingFactor { get; protected set; }

		public Field Field { get; protected set; }
		public int Score { get; protected set; }
		public int ScoreField { get; protected set; }

		public BlockNodes<T> Children { get; protected set; }

		/// <summary>Applies the search on the current node.</summary>
		/// <param name="depth">
		/// The maximum depth to search.
		/// </param>
		/// <param name="pars">
		/// The parameters needed to apply the search.
		/// </param>
		public abstract void Apply(byte depth, ApplyParameters pars);

		protected abstract T Create(Field field, ApplyParameters pars);
		protected abstract Block GetBlock(ApplyParameters pars);

		public int CompareTo(object obj) { return CompareTo((IBlockNode)obj); }
		public int CompareTo(IBlockNode other) { return other.Score.CompareTo(Score); }

		[DebuggerBrowsable(DebuggerBrowsableState.Never), ExcludeFromCodeCoverage]
		private string DebuggerDisplay
		{
			get
			{
				return String.Format("{0:#,##0}, Depth: {1}, Children: {2}", Score, Depth, Children == null ? 0 : Children.Count);
			}
		}
	}
}