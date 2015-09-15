using System;

namespace AIGames.BlockBattle.Kubisme
{
	public interface IBlockNode: IComparable, IComparable<IBlockNode>
	{
		/// <summary>Applies the search on the current node.</summary>
		/// <param name="depth">
		/// The maximum depth to search.
		/// </param>
		/// <param name="pars">
		/// The parameters needed to apply the search.
		/// </param>
		void Apply(byte depth, ApplyParameters pars);
		byte Depth { get; }
		Field Field { get; }
		int Score { get; }
		int ScoreField { get; }
	}
}
