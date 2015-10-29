using System;
using System.Diagnostics;

namespace AIGames.BlockBattle.Kubisme.Communication
{
	public struct SkipsInstruction : IInstruction
	{
		public SkipsInstruction(PlayerName name, int skips) 
		{
			m_Name = name;
			m_Skips = skips; 
		}

		public PlayerName Name { get { return m_Name; } }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private PlayerName m_Name;

		public int Skips { get { return m_Skips; } }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int m_Skips;

		public override string ToString() 
		{ 
			return String.Format("update {0} skips {1}", Name, Skips).ToLowerInvariant(); 
		}

		internal static IInstruction Parse(PlayerName name, string[] splited)
		{
			int skips;
			if (Int32.TryParse(splited[3], out skips) && skips >= 0)
			{
				return new SkipsInstruction(name, skips);
			}
			return null;
		}
	}
}
