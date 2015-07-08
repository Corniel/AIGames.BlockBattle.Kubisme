using System;

namespace AIGames.BlockBattle.Kubisme.Communication
{
	public static class UpdatePlayerInstruction
	{
		internal static  IInstruction Parse(string[] splited)
		{
			PlayerName name;
			if (splited.Length == 4 && Enum.TryParse<PlayerName>(splited[1], true, out name) && name != PlayerName.None)
			{
				switch (splited[2])
				{
					case "row_points": return RowPointsInstruction.Parse(name, splited);
					case "combo": return ComboInstruction.Parse(name, splited);
					case "field": return FieldInstruction.Parse(name, splited);
				}
			}
			return null;
		}
	}
}
