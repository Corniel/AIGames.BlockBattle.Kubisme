using System;
using System.Diagnostics;

namespace AIGames.BlockBattle.Kubisme.Communication
{
	public struct RowPointsInstruction : IInstruction
	{
		public RowPointsInstruction(PlayerName name, int points) 
		{
			m_Name = name;
			m_Points = points; 
		}

		public PlayerName Name { get { return m_Name; } }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private PlayerName m_Name;

		public int Points { get { return m_Points; } }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int m_Points;

		public override string ToString() 
		{ 
			return String.Format("update {0} row_points {1}", Name, Points).ToLowerInvariant(); 
		}

		internal static IInstruction Parse(PlayerName name, string[] splited)
		{
			int points;
			if (Int32.TryParse(splited[3], out points) && points >= 0)
			{
				return new RowPointsInstruction(name, points);
			}
			return null;
		}
	}
}
