namespace AIGames.BlockBattle.Kubisme
{
	/// <summary>Represents a class to manipulate and investigate bits.</summary>
	public static class Bits
	{
		/// <summary>Contains a flag bits for UInt16.</summary>
		public static readonly ushort[] FlagUInt16 = new ushort[]
		{
			0X0001,
			0X0002,
			0X0004,
			0x0008,
			0X0010,
			0X0020,
			0X0040,
			0x0080,
			0X0100,
			0X0200,
			0X0400,
			0x0800,
			0X1000,
			0X2000,
			0X4000,
			0x8000,
		};

		/// <summary>Counts the number of (1) bits.</summary>
		public static int Count(byte bits) { return BytesCount[bits]; }

		/// <summary>Counts the number of (1) bits.</summary>
		public static int Count(uint bits)
		{
			bits = bits - ((bits >> 1) & 0x55555555);
			bits = (bits & 0x33333333) + ((bits >> 2) & 0x33333333);
			return (int)(((bits + (bits >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
		}

		/// <summary>Counts the number of (1) bits.</summary>
		public static int Count(int bits)
		{
			bits = bits - ((bits >> 1) & 0x55555555);
			bits = (bits & 0x33333333) + ((bits >> 2) & 0x33333333);
			return (((bits + (bits >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
		}

		/// <summary>Counts the number of (1) bits.</summary>
		/// <remarks>
		/// See http://stackoverflow.com/questions/109023
		/// </remarks>
		public static int Count(long bits)
		{
			bits = bits - ((bits >> 1) & 0x5555555555555555);
			bits = (bits & 0x3333333333333333) + ((bits >> 2) & 0x3333333333333333);
			return (int)(unchecked(((bits + (bits >> 4)) & 0xF0F0F0F0F0F0F0F) * 0x101010101010101) >> 56);
		}

		/// <summary>Counts the number of (1) bits.</summary>
		public static int Count(ulong bits)
		{
			bits = bits - ((bits >> 1) & 0x5555555555555555UL);
			bits = (bits & 0x3333333333333333UL) + ((bits >> 2) & 0x3333333333333333UL);
			return (int)(unchecked(((bits + (bits >> 4)) & 0xF0F0F0F0F0F0F0FUL) * 0x101010101010101UL) >> 56);
		}

		/// <summary>lookup for Count(byte).</summary>
		private static readonly byte[] BytesCount = { 0, 1, 1, 2, 1, 2, 2, 3, 1, 2, 2, 3, 2, 3, 3, 4, 1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5, 1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7, 1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7, 3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7, 4, 5, 5, 6, 5, 6, 6, 7, 5, 6, 6, 7, 6, 7, 7, 8 };
	}
}
