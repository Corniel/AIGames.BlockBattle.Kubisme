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
			FreeRowWeights = new int[21];
			ComboPotential = new int[21];
		}

		[ParameterType(ParameterType.Descending | ParameterType.Positive)]
		public int[] FreeRowWeights { get; set; }

		[ParameterType(ParameterType.Descending)]
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
			// Elo: 1049, Avg: 0,844, Runs: 1442, ID: 6140, Parent: 5272
			{
				FreeRowWeights = new int[] { 59, 40, 24, 14, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1 },
				ComboPotential = new int[] { 5, 2, 1, -27, -37, -59, -59, -60, -60, -60, -63, -63, -74, -74, -74, -78, -89, -93, -105, -117, -139 },
				Points = 13,
				Combo = 4,
				Holes = -13,
				Blockades = 0,
				LastBlockades = -2,
				WallsLeft = 3,
				WallsRight = 3,
				Floor = 5,
				NeighborsHorizontal = 0,
				NeighborsVertical = 3,
			};
		}
	}
}
