using System;
#if DEBUG
using System.Reflection;
using System.Text;
#endif

namespace AIGames.BlockBattle.Kubisme
{
	[Serializable]
	public class ComplexParameters : IParameters
	{
#if DEBUG
		private static readonly PropertyInfo[] Props = typeof(ComplexParameters).GetProperties(BindingFlags.Public | BindingFlags.Instance);
#endif

		public ComplexParameters()
		{
			GarbagePotential = new int[3];
			FreeRows = new int[22];
		}

		/// <summary>A value that corrects for the time it takes for creating new garbage.</summary>
		[ParameterType(ParameterType.Ascending | ParameterType.Positive)]
		public int[] GarbagePotential { get; set; }

		/// <summary>The more free rows the we have, the better it is.</summary>
		[ParameterType(ParameterType.Ascending | ParameterType.Negative)]
		public int[] FreeRows { get; set; }

		[ParameterType(ParameterType.Positive)]
		public int FirstFilled { get; set; }

		[ParameterType(ParameterType.Positive)]
		public int Points { get; set; }
		
		public int Combo { get; set; }
		public int Skips { get; set; }

		[ParameterType(ParameterType.Negative)]
		public int Holes { get; set; }

		public int WallsLeft { get; set; }
		public int WallsRight { get; set; }

		public int NeighborsHorizontal { get; set; }
		public int NeighborsVertical { get; set; }
		public int Floor { get; set; }
		
		public int TSpinPotential { get; set; }

		[ParameterType(ParameterType.Negative)]
		public int Blockades { get; set; }
		[ParameterType(ParameterType.Negative)]
		public int LastBlockades { get; set; }

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

		public static ComplexParameters GetDefault()
		{
			var pars = new ComplexParameters()
			// Elo: 1579, Avg: 0,184, Runs: 578, ID: 1078, Parent: 1051
			{
				GarbagePotential = new int[] { 13, 16, 74 },
				FreeRows = new int[] { -99, -92, -90, -88, -84, -84, -82, -80, -77, -73, -73, -70, -67, -67, -64, -60, -55, -55, -54, -52, -38, -10 },
				FirstFilled = 21,
				Points = 20,
				Combo = 24,
				Skips = 29,
				Holes = -23,
				WallsLeft = 3,
				WallsRight = 3,
				NeighborsHorizontal = 1,
				NeighborsVertical = 2,
				Floor = 0,
				TSpinPotential = 47,
				Blockades = -1,
				LastBlockades = -1,
			};
			return pars;
		}
	}
}
