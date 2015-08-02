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
			// Elo: 1447, Avg: 0,602, Runs: 4220, ID: 41462, Parent: 41010
			{
				FreeCellWeights = new int[] { 3, 6, 1, 0, 2, 1, -2, 2, 1, -2, 5, 1, -1, 0, 1, 0, -2, -1, -9, -30, -45 },
				ComboPotential = new int[] { -12, 4, 19, 36, 37, 23, 39, 69, -59, -14, 54, -67, 117, 26, 76, -91, -8, -4, 6, -35, -42 },
				Points = 56,
				Combo = 20,
				Holes = -26,
				Blockades = -2,
				WallsLeft = 8,
				WallsRight = 9,
				Floor = 11,
				NeighborsHorizontal = -2,
				NeighborsVertical = 8,
			};
		}
	}
}
