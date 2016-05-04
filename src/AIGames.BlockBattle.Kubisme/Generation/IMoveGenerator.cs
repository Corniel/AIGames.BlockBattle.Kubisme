using System;
using System.Collections.Generic;
namespace AIGames.BlockBattle.Kubisme
{
	public interface IMoveGenerator : IFieldGenerator
	{
		IEnumerable<MoveCandiate> GetMoves(Field field, Block current);
	}
}
