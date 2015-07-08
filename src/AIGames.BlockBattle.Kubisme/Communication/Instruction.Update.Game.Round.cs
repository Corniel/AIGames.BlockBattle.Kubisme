using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGames.BlockBattle.Kubisme.Communication
{
	public struct RoundInstruction : IInstruction
	{
		public RoundInstruction(int round) { m_Value = round; }

		public int Value { get { return m_Value; } }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int m_Value;

		public override string ToString() { return String.Format("update game round {0}", Value); }

		internal static IInstruction Parse(string[] splited)
		{
			int round;
			if (Int32.TryParse(splited[3], out round) && round > 0)
			{
				return new RoundInstruction(round);
			}
			return null;
		}
	}
}
