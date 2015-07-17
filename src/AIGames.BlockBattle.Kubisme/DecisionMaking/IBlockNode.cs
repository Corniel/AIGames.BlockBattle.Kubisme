using AIGames.BlockBattle.Kubisme.Models;
using System;

namespace AIGames.BlockBattle.Kubisme.DecisionMaking
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
