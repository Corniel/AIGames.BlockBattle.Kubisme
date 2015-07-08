namespace AIGames.BlockBattle.Kubisme.Communication
{
	public static class UpdateGameInstruction
	{
		internal static IInstruction Parse(string[] splited)
		{
			switch (splited[2])
			{
				case "round": return RoundInstruction.Parse(splited);
				case "this_piece_type": return ThisPieceInstruction.Parse(splited);
				case "next_piece_type": return NextPieceInstruction.Parse(splited);
				case "this_piece_position": return ThisPiecePositionInstruction.Parse(splited);
			}
			return null;
		}
	}
}
