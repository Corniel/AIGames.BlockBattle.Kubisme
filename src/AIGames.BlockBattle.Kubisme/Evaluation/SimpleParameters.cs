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
		}

		public int[] RowWeights { get; set; }
		public int[] RowCountWeights { get; set; }

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
			// 2.109.520  0.12:51:52 Score: 87,96%, Win: 110,8, Lose: 109,7 Runs: 2.300, ID: 128474
			{
				RowWeights = new int[] { -85, -50, -97, -76, -2, 0, -1, 0, 0, 0, 1, 1, 1, 1, 0, 2, 1, 2, 1, 5, -121 },
				RowCountWeights = new int[] { -20, 9, 26, 26, 35, 38, 48, 60, 57, 48, -1 },
				Points = 114,
				Combo = 8,
				Holes = -66,
				Blockades = -7,
				WallsLeft = 28,
				WallsRight = 27,
				Floor = -7,
				NeighborsHorizontal = -15,
				NeighborsVertical = 26,
			};
		}
	}
}
