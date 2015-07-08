namespace AIGames.BlockBattle.Kubisme.Communication
{
	public static class UpdateInstruction
	{
		internal static IInstruction Parse(string[] splited)
		{
			switch (splited[1])
			{
				case "game": return UpdateGameInstruction.Parse(splited);
				case "player1":
				case "player2": return UpdatePlayerInstruction.Parse(splited);
			}
			return null;
		}
	}
}
