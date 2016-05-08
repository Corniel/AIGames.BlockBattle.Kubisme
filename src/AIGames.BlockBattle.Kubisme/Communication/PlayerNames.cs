using System;

namespace AIGames.BlockBattle.Kubisme.Communication
{
	public static class PlayerNames
	{
		public static PlayerName Other(this PlayerName name)
		{
			switch (name)
			{
				case PlayerName.Player1: return PlayerName.Player2;
				case PlayerName.Player2: return PlayerName.Player1;
				default: throw new NotSupportedException();
			}
		}
	}
}
