using System;
using System.Diagnostics;

namespace AIGames.BlockBattle.Kubisme.Communication
{
	public struct YourBotInstruction : IInstruction
	{
		public YourBotInstruction(PlayerName name) { m_Name = name; }

		public PlayerName Name { get { return m_Name; } }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private PlayerName m_Name;

		public override string ToString() { return String.Format("settings your_bot {0}", Name).ToLowerInvariant(); }


		internal static IInstruction Parse(string[] splitted)
		{
			PlayerName name;
			if (Enum.TryParse<PlayerName>(splitted[2], true, out name))
			{
				return new YourBotInstruction(name);
			}
			return null;
		}

	}
}
