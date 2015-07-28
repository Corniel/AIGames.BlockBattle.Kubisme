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
			RowWeights = new int[21];
			RowCountWeights = new int[11];
			ComboPotential = new int[21];
		}

		public int[] RowWeights { get; set; }
		public int[] RowCountWeights { get; set; }
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
			// 4.565.153  0.18:26:11 Score: 31,56%, Win: 66,2, Lose: 58,9 Runs: 11.763, ID: 194242
			{
				RowWeights = new int[] { -325, -258, -193, -122, -2, -1, -1, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 4 },
				RowCountWeights = new int[] { 0, 8, 20, 22, 31, 38, 48, 64, 59, 45, 0 },
				ComboPotential = new int[] { 0, 56, 60, 64, 70, 95, 100, 80, 90, 70, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0,0 },
				Points = 96,
				Combo = 16,
				Holes = -82,
				Blockades = -9,
				WallsLeft = 26,
				WallsRight = 27,
				Floor = -7,
				NeighborsHorizontal = -15,
				NeighborsVertical = 28,
			};
		}
	}
}
