using AIGames.BlockBattle.Kubisme.Communication;
using System;
using System.Collections.Generic;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme
{
	public static class Row
	{
		public const ushort Empty = 0;
		public const ushort Filled = 0X03FF;
		public const ushort Locked = 0X07FF;

		/// <summary>A set of masks with 2 connected block.</summary>
		public static readonly HashSet<int> Row2BlocksConnected = new HashSet<int>() { 0x0003, 0x0006, 0x000C, 0x0018, 0x0030, 0x0060, 0x00C0, 0x0180, 0x0300 };
	
		public static readonly ushort[] Flag = new ushort[]{
			0x0001,
			0x0002,
			0x0004,
			0x0008,
			0x0010,
			0x0020,
			0x0040,
			0x0080,
			0x0100,
			0x0200,

			0, 0, 0, 0, 0, 0
		};

		public static readonly ushort[] Garbage = new ushort[]
		{
			0X03FE,
			0X03FD,
			0X03FB,
			0X03F7,
			0X03EF,
			0X03DF,
			0X03BF,
			0X037F,
			0X02FF,
			0X01FF,
		};

		public static readonly ushort[] Garbage2 = GetGarbage2();

		public static readonly byte[] Count = GetCount();
		public static readonly byte[] Groups = GetGroups();
	
		public static ushort Create(GameState state, PlayerName name, int r)
		{
			ushort row = 0;

			for (var c = 0; c < 10; c++)
			{
				if (state[name].Field[r, c] == FieldInstruction.FixedBlock)
				{
					row |= Flag[c];
				}
				else if (state[name].Field[r, c] == FieldInstruction.LockedBlock)
				{
					return Row.Locked;
				}
			}
			return row;
		}

		public static ushort Create(string line)
		{
			ushort row = 0;
			for (var i = 0; i < 10; i++)
			{
				if (line[i] == 'X')
				{
					row |= Flag[i];
				}
			}
			return row;
		}

		public static ushort[] GetGarbage(int count, int pointsOld, MT19937Generator rnd)
		{
			var garbage = new ushort[count];

			var single = ((pointsOld / 3) & 1) == 0;

			for (var i = 0; i < count; i++)
			{
				garbage[i] = single ? Garbage[rnd.Next(10)] : Garbage2[rnd.Next(45)];
				single = !single;
			}
			return garbage;
		}

		public static bool HasRow2BlocksConnected(int row, int count)
		{
			// if more then 5 blocks there are at least two connected.
			if (count > 5) { return true; }
			// if there are less then 2 blocks there obviously are none.
			if (count < 2) { return false; }
			foreach (var mask in Row2BlocksConnected)
			{
				if ((row & mask) == mask) { return true; }
			}
			return false;
		}

		public static string ToString(ushort row)
		{
			var chars = new char[10];

			for (var i = 0; i < 10; i++)
			{
				chars[i] = (Flag[i] & row) == 0 ? '.' : 'X';
			}
			return new String(chars);
		}

		private static byte[] GetCount()
		{
			var bytes = new byte[Row.Filled + 1];
			for (ushort row = Row.Empty; row <= Row.Filled; row++)
			{
				bytes[row] = (byte)Bits.Count(row);
			}
			return bytes;
		}

		private static byte[] GetGroups()
		{
			var bytes = new byte[Row.Filled + 1];

			for (ushort row = 1; row < bytes.Length; row++)
			{
				int prev = row & 1;
				var count = (byte)prev;
				for (var c = 1; c < 10; c++)
				{
					var cur = row & Row.Flag[c];
					// if the previous cell was empty, and the current is not, 
					// a new group started.
					if (prev == 0 && cur > 0)
					{
						count++;
					}
					prev = cur;
				}
				bytes[row] = count;
			}
			return bytes;
		}

		private static ushort[] GetGarbage2()
		{
			var garbage = new ushort[9 + 8 + 7 + 6 + 5 + 4 + 3 + 2 + 1];
			var index = 0;
			for (var r = 0; r < 10; r++)
			{
				for (var i = r + 1; i < 10; i++)
				{
					var g = Garbage[i] & Garbage[r];
					garbage[index++] = (ushort)g;
				}
			}
			return garbage;
		}
	}
}
