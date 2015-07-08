using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGames.BlockBattle.Kubisme.Communication
{
	public static class SettingsInstruction
	{
		internal static IInstruction Parse(string[] splitted)
		{
			if (splitted.Length != 3) { return null; }

			switch (splitted[1])
			{
				case "your_bot": return YourBotInstruction.Parse(splitted);
			}
			return null;
		}
	}
	
}
