using System;
using System.Reflection;
using System.Text;

namespace AIGames.BlockBattle.Kubisme.Evaluation
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
			// 1.826.151  0.07:11:11 Score: 30,79%, Win: 66,4, Lose: 59,4 Runs: 3.690, ID: 109340
			{
				RowWeights = new int[] { -89, -58, -81, -130, -2, 0, -1, 0, 0, 0, 1, 1, 1, 1, 2, 2, 1, 2, 1, 5, -173 },
				RowCountWeights = new int[] { -24, 8, 20, 22, 27, 38, 48, 64, 59, 48, -7 },
				ComboPotential = new int[] { -36, -16, -2, -22, -4, 8, 32, 24, 40, 30, 8, 12, -110, 8, -46, -40, -4, -48, -18, 24, 4 },
				Points = 108,
				Combo = 12,
				Holes = -80,
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
