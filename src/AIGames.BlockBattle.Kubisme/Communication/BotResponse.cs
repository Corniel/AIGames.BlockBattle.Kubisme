using System;

namespace AIGames.BlockBattle.Kubisme.Communication
{
	public class BotResponse
	{
		public MoveInstruction Move { get; set; }
		public string Log { get; set; }

		public override string ToString()
		{
			return String.Format("Move: {0}, Log: {1}", Move, Log);
		}
	}
}
