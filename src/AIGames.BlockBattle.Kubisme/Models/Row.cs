using AIGames.BlockBattle.Kubisme.Communication;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme
{
	public static class Row
	{
		/// <summary>A set of masks with 7 blocks and one hole.</summary>
		public static readonly HashSet<int> Row7BlockOneHole = new HashSet<int>() { 0X03F8, 0X03F1, 0X03E3, 0X03C7, 0X038F, 0X031F, 0X023F, 0X007F, };
		/// <summary>A set of masks with 8 blocks and one hole.</summary>
		public static readonly HashSet<int> Row8BlockOneHole = new HashSet<int>() { 0X03FC, 0X03F9, 0X03F3, 0X03E7, 0X03CF, 0X039F, 0X033F, 0X027F, 0X00FF, };

		public static readonly ushort[] Garbage = new ushort[]
		{
			0X03FE,
			0X03FD,
			0X03FA,
			0X03F7,

			0X03EF,
			0X03DF,
			0X03AF,
			0X037F,

			0X02FF,
			0X01FF,
		};

		[SuppressMessage("Microsoft.Usage", "CA2207:InitializeValueTypeStaticFieldsInline", Justification = "Too complex to generate otherwise.")]
		static Row()
		{
			Count = new byte[Row.Filled + 1];
			for (ushort r = Row.Empty; r <= Row.Filled; r++)
			{
				Count[r] = (byte)Bits.Count(r);
			}
		}
		public static readonly byte[] Count;
		
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
		public const ushort Empty = 0;
		public const ushort Filled = 0X03FF;
		public const ushort Locked = 0X07FF;

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
			if (line == "##########") { return Row.Locked; }
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

		public static ushort[] GetGarbage(int count, MT19937Generator rnd)
		{
			var garbage = new ushort[count];

			var prev = -1;
			for (var i = 0; i < count; i++)
			{
				var index = rnd.Next(i == 0 ? 10 : 9);
				// 9 is not assigned, so move the to that one.
				if (index == prev) { index = 9; }
				garbage[i] = Garbage[index];
				prev = index;
			}
			return garbage;
		}


		public static string ToString(ushort row)
		{
			if (row == Row.Locked) { return "##########"; }
			var chars = new char[10];

			for (var i = 0; i < 10; i++)
			{
				chars[i] = (Flag[i] & row) == 0 ? '.' : 'X';
			}

			return new String(chars);
		}

	}
}
