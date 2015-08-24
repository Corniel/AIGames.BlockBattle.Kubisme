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
		public int LastBlockades { get; set; }
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
				else if (prop.PropertyType == typeof(bool))
				{
					var val = (bool)prop.GetValue(this);
					writer.AppendFormat("{0}: {1}, ", prop.Name, val.ToString().ToLowerInvariant());
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
			// Elo: 1031, Avg: 0,858, Runs: 1533, ID: 7035, Parent: 6931
			{
				FreeCellWeights = new int[] { 41, 42, 25, 12, 9, 11, 0, -1, 6, 0, 12, -1, 2, 10, 0, 4, 4, -2, 0, -10, 14 },
				ComboPotential = new int[] { 15, 10, 5, -19, 11, -18, 11, -2, 22, -27, 32, 41, -46, -14, 14, -44, 7, -2, 29, -24, -22 },
				Points = 43,
				Combo = 17,
				Holes = -71,
				Blockades = -9,
				LastBlockades = -9,
				WallsLeft = 16,
				WallsRight = 18,
				Floor = 9,
				NeighborsHorizontal = -9,
				NeighborsVertical = 19,
			};
		}
	}
}
