namespace AIGames.BlockBattle.Kubisme
{
	public static class RowCount
	{
		static RowCount()
		{
			counts = new byte[1024];
			for (ushort r = 0; r < 1024; r++)
			{
				counts[r] = (byte)Bits.Count(r);
			}
		}
		public static int Get(int row)
		{
			return counts[row];
		}
		private static readonly byte[] counts;
	}
}
