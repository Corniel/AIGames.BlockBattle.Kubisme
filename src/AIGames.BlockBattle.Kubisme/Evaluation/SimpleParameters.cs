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
			// Elo: 3630, Avg: 0,903, Runs: 2360, ID: 7675, Parent: 7401
			{
				FreeCellWeights = new int[] { 137, 38, -7, -4, 41, -21, 17, 1, 0, -1, 7, 19, -4, -3, 5, -1, -2, -1, -5, -7, 28 },
				ComboPotential = new int[] { 21, 15, 2, -8, -4, 7, -19, -18, -33, 9, 7, -12, -38, 22, -7, 14, -3, -30, -26, -23, -46 },
				Points = 108,
				Combo = -5,
				Holes = -65,
				Blockades = 2,
				WallsLeft = 8,
				WallsRight = 9,
				Floor = 4,
				NeighborsHorizontal = -1,
				NeighborsVertical = 10,
			};
		}
	}
}
