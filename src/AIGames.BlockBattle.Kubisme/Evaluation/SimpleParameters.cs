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
			// Elo: 1028, Avg: 0,852, Runs: 1603, ID: 4375, Parent: 3836
			{
				FreeCellWeights = new int[] { 17, 45, 27, 11, 30, 6, 13, -10, 13, -3, -2, 10, -6, 4, 1, -1, 1, -2, 13, -33, 1 },
				ComboPotential = new int[] { 19, 8, 8, 2, 0, 4, -1, 2, -7, -29, -3, -14, 14, -18, -6, 25, 23, -63, 22, -11, 22 },
				Points = 70,
				Combo = 7,
				Holes = -76,
				Blockades = -3,
				WallsLeft = 15,
				WallsRight = 16,
				Floor = 3,
				NeighborsHorizontal = -7,
				NeighborsVertical = 18,
			};
		}
	}
}
