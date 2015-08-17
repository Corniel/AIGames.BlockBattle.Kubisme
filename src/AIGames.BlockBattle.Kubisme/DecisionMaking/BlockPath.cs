using AIGames.BlockBattle.Kubisme.Communication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme
{
	public struct BlockPath : IInstruction
	{
		private const UInt64 Mask = 0x0FFFFFFFFFFFFFFF;

		public static readonly BlockPath None = default(BlockPath);
		public static readonly BlockPath Left = BlockPath.Create(ActionType.TurnLeft);
		public static readonly BlockPath Right = BlockPath.Create(ActionType.TurnRight);
		public static readonly BlockPath Uturn = BlockPath.Create(ActionType.TurnLeft, ActionType.TurnLeft);

		private BlockPath(ulong m0, ulong m1, int count)
		{
			m0 |= ((ulong)count) << 60;
			m1 |= ((ulong)count >> 4) << 60;
			moves0 = m0;
			moves1 = m1;
		}

		private readonly ulong moves0;
		private readonly ulong moves1;

		public int Count
		{
			get
			{
				int c = (int)(moves0 >> 60);
				c |= ((int)(moves1 >> 60)) << 4;
				return c;
			}
		}


		/// <summary>Gets the last added action.</summary>
		/// <remarks>
		/// Useful to prevent infinite left-right switches.
		/// </remarks>
		public ActionType Last
		{
			get
			{
				var shft = Count * 3;
				if (shft < 60)
				{
					return (ActionType)((moves0 >> shft) & 7);
				}
				else
				{
					return (ActionType)((moves1 >> (shft - 60)) & 7);
				}

			}
		}

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

		public BlockPath Add(ActionType move)
		{
			var count = Count;
			var m0 = moves0 & Mask;
			var m1 = moves1 & Mask;

			if (count < 20)
			{
				m0 |= ((ulong)move) << (3 * count);
			}
			else
			{
				m1 |= ((uint)move) << (3 * (count - 20));
			}
			return new BlockPath(m0, m1, count + 1);
		}

		public BlockPath AddTurnLeft() { return Add(ActionType.TurnLeft); }
		public BlockPath AddTurnRight() { return Add(ActionType.TurnRight); }

		public BlockPath AddLeft() { return Add(ActionType.Left); }
		public BlockPath AddRight() { return Add(ActionType.Right); }
		public BlockPath AddDown() { return Add(ActionType.Down); }
		public BlockPath AddDrop() { return Add(ActionType.Drop); }

		public BlockPath AddShift(int count)
		{
			var path = this;
			for (var i = 1; i <= count; i++)
			{
				path = path.AddRight();
			}
			for (var i = -1; i >= count; i--)
			{
				path = path.AddLeft();
			}
			return path;
		}

		public override int GetHashCode()
		{
			return moves0.GetHashCode() ^ moves1.GetHashCode();
		}
		public override bool Equals(object obj)
		{
			var path = (BlockPath)obj;
			return path.moves0 == moves0 && path.moves1 == moves1;
		}

		public override string ToString()
		{
			if (moves0 == 0) { return "no_moves"; }
			return String.Join(",", Moves).ToLowerInvariant();
		}

		public static BlockPath Create(params ActionType[] moves)
		{
			ulong m0 = 0;
			ulong m1 = 0;
			int p = 0;

			foreach (var move in moves)
			{
				if (p < 60)
				{
					m0 |= ((ulong)move) << p;
				}
				else
				{
					m1 |= ((ulong)move) << (p - 60);
				}
				p += 3;
			}
			return new BlockPath(m0, m1, p / 3);
		}

		internal static BlockPath Create(Block block, Position source, Position target)
		{
			var actions = new List<ActionType>();

			switch (block.Rotation)
			{
				case Block.RotationType.Left: actions.Add(ActionType.TurnLeft); break;
				case Block.RotationType.Uturn: actions.Add(ActionType.TurnLeft); actions.Add(ActionType.TurnLeft); break;
				case Block.RotationType.Right: actions.Add(ActionType.TurnRight); break;
			}
			var delta = source.Col - target.Col;
			if (delta >= 0)
			{
				for (var i = 0; i < delta; i++)
				{
					actions.Add(ActionType.Left);
				}
			}
			else
			{
				for (var i = 0; i < -delta; i++)
				{
					actions.Add(ActionType.Right);
				}
			}
			var down = target.Row - source.Row - block.Bottom;
			for (var i = 0; i < down; i++)
			{
				actions.Add(ActionType.Down);
			}
			return BlockPath.Create(actions.ToArray());
		}
	}
}
