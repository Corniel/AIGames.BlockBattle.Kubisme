using System;
using System.Diagnostics;

namespace AIGames.BlockBattle.Kubisme.Communication
{
	public struct RequestMoveInstruction : IInstruction
	{
		public RequestMoveInstruction(TimeSpan time) { m_Time = time; }

		public TimeSpan Time { get { return m_Time; } }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private TimeSpan m_Time;

		public override string ToString()
		{
			return String.Format("action moves {0:0}", Time.TotalMilliseconds);
		}

		internal static IInstruction Parse(string[] splited)
		{
			int ms;
			if (splited[1] == "moves" && splited.Length == 3 && Int32.TryParse(splited[2], out ms))
			{
				return new RequestMoveInstruction(TimeSpan.FromMilliseconds(ms));
			}
			return null;
		}
	}
}
