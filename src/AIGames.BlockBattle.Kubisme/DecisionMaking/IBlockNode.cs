using System;

namespace AIGames.BlockBattle.Kubisme
{
	public interface IBlockNode: IComparable, IComparable<IBlockNode>
	{
		void Apply(byte depth, ApplyParameters pars);
		byte Depth { get; }
		Field Field { get; }
		int Score { get; }
		int ScoreField { get; }
	}
}
