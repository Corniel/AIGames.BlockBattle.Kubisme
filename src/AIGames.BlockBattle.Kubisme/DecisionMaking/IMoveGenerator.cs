using AIGames.BlockBattle.Kubisme.Models;
using System;
using System.Collections.Generic;
namespace AIGames.BlockBattle.Kubisme.DecisionMaking
{
	public interface IMoveGenerator
	{
		IEnumerable<MoveCandiate> GetMoves(Field field, Block current, Position pos);
	}
}
