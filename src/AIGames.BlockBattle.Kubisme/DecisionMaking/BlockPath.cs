using AIGames.BlockBattle.Kubisme.Communication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGames.BlockBattle.Kubisme.DecisionMaking
{
	public struct BlockPath
	{
		public static readonly BlockPath Empty = default(BlockPath);

		private ulong moves0;
		private uint moves1;

		public int Count { get 
		{
			int c = (int)(moves0 >> 60);
			c |= ((int)(moves1 >> 30)) << 4;
			return c; 
		} }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public IEnumerable<ActionType> Moves
		{
			get
			{
				var count = Count * 3;
				var c1 = count > 60 ? 60 : count;
				for (var i = 0; i < c1; i += 3)
				{
					var move = (ActionType)((moves0 >> i) & 7);
					yield return move;
				}
				count -= 60;
				for (var i = 0; i < count; i += 3)
				{
					var move = (ActionType)((moves1 >> i) & 7);
					yield return move;
				}
			}
		}
#if DEBUG
		[ExcludeFromCodeCoverage]
		private ActionType[] MovesDebug { get { return Moves.ToArray(); } }
#endif

		public override string ToString()
		{
			if (moves0 == 0) { return "no_moves"; }
			return String.Join(",", Moves).ToLowerInvariant();
		}

		public static BlockPath Create(params ActionType[] moves)
		{
			ulong m0 = 0;
			uint m1 = 0;
			int p = 0;

			foreach (var move in moves)
			{
				if (p < 60)
				{
					m0 |= ((ulong)move) << p;
				}
				else
				{
					m1 |= ((uint)move) << (p - 60);
				}
				p += 3;
			}
			m0 |= ((ulong)p / 3ul) << 60;
			m1 |= ((uint)p / 48u) << 30;
			return new BlockPath() { moves0 = m0, moves1 = m1 };
		}
	}
}
