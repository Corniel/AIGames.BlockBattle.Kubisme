using System;
#if DEBUG
using System.Reflection;
using System.Text;
#endif

namespace AIGames.BlockBattle.Kubisme
{
	[Serializable]
	public class SimpleParameters : IParameters
	{
#if DEBUG
		private static readonly PropertyInfo[] Props = typeof(SimpleParameters).GetProperties(BindingFlags.Public | BindingFlags.Instance);
#endif

		public SimpleParameters()
		{
			FreeCellWeights = new int[21];
			ComboPotential = new int[21];
		}

		public int[] FreeCellWeights { get; set; }
		public int[] ComboPotential { get; set; }

		public int Points { get; set; }
		public int Combo { get; set; }
		public int Holes { get; set; }
		public int Blockades { get; set; }
		public int WallsLeft { get; set; }
		public int WallsRight { get; set; }
		public int Floor { get; set; }
		public int NeighborsHorizontal { get; set; }
		public int NeighborsVertical { get; set; }

		/// <summary>Gets a string representation of the simple evaluator parameters.</summary>
		/// <remarks>
		/// Apparently, this code does not compile under Mono. As it is only for
		/// debug purposes, it is disabled in release mode.
		/// </remarks>
		public override string ToString()
		{
#if DEBUG
			var writer = new StringBuilder();
			foreach (var prop in Props)
			{
				if (prop.PropertyType == typeof(int))
				{
					var val = (int)prop.GetValue(this);
					writer.AppendFormat("{0}: {1}, ", prop.Name, val);
				}
			}
			foreach (var prop in Props)
			{
				if (prop.PropertyType == typeof(int[]))
				{
					var vals = (int[])prop.GetValue(this);
					writer.AppendFormat("{0}: {{{1}}}, ", prop.Name, String.Join(",", vals));
				}
			}
			writer.Remove(writer.Length - 2, 2);
			return writer.ToString();
#else
			return base.ToString();
#endif
		}

		public static SimpleParameters GetDefault()
		{
			return new SimpleParameters()
			// Elo: 1024, Avg: 0,752, Runs: 4403, ID: 1029, Parent: 915
			{
				FreeCellWeights = new int[] { 199, 95, 26, 20, 22, 23, 21, 16, 12, 11, 4, 6, 30, 5, 12, 29, -10, -4, -20, -31, 38 },
				ComboPotential = new int[] { 42, 23, 8, 41, 30, -6, -6, 11, -9, 7, 12, -30, -35, 27, -15, 25, 12, -47, 14, -6, -28 },
				Points = 127,
				Combo = 21,
				Holes = -88,
				Blockades = -10,
				WallsLeft = 53,
				WallsRight = 52,
				Floor = -8,
				NeighborsHorizontal = -17,
				NeighborsVertical = 46,
			};
		}
	}
}
