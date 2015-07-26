using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme
{
	public interface IFieldGenerator
	{

		IEnumerable<Field> GetFields(Field field, Block current, Position pos, bool searchNoDrops);
	}
}
