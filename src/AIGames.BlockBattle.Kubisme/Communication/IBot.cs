using System;

namespace AIGames.BlockBattle.Kubisme.Communication
{
	public interface IBot
	{
		void ApplySettings(Settings settings);
		void Update(GameState state);
		MoveInstruction GetMove(TimeSpan time);
	}
}
