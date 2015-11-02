using System;

namespace AIGames.BlockBattle.Kubisme
{
	[Serializable]
	public class EvaluatorParameters
	{
		public EvaluatorParameters()
		{
			Unreachables = new int[22];
			EmptyCells = new int[22];
			Groups = new int[6];
		}

		public int[] EmptyRowsCalc { get { return m_FreeRows; } }
		private int[] m_FreeRows;

		public int[] UnreachableRowsCalc { get { return m_UnreachableRows; } }
		private int[] m_UnreachableRows;

		/// <summary>Get the score per hole.</summary>
		[ParameterType(ParameterType.Negative)]
		public int Holes { get; set; }
		[ParameterType(ParameterType.Positive)]
		public int Points { get; set; }
		[ParameterType(ParameterType.Positive)]
		public int Combo { get; set; }
		[ParameterType(ParameterType.Positive)]
		public int Skips { get; set; }

		/// <summary>The minimum distance to an unreachable hole.</summary>
		[ParameterType(ParameterType.Negative)]
		public int UnreachableDistance { get; set; }

		/// <summary>Points for a potential Tetris.</summary>
		[ParameterType(ParameterType.Positive)]
		public int TetrisPotential { get; set; }

		/// <summary>Points for the different number of groups per reachable hole.</summary>
		/// <remarks>
		/// Index 0 is never called.
		/// </remarks>
		[ParameterType(ParameterType.Descending)]
		public int[] Groups { get; set; }

		[ParameterType(ParameterType.Positive)]
		public int EmptyRowCount { get; set; }
		[ParameterType(ParameterType.Positive)]
		public int EmptyCellStaffle { get; set; }
		[ParameterType(ParameterType.Descending | ParameterType.Positive)]
		public int[] EmptyCells { get; set; }

		/// <summary>The less Unreachable.</summary>
		[ParameterType(ParameterType.Descending | ParameterType.Negative)]
		public int[] Unreachables { get; set; }
		[ParameterType(ParameterType.Negative)]
		public int UnreachableStaffle { get; set; }

		public EvaluatorParameters Calc()
		{
			m_FreeRows = new int[EmptyCells.Length];

			for (var i = 1; i < m_FreeRows.Length; i++)
			{
				m_FreeRows[i] = m_FreeRows[i - 1];
				m_FreeRows[i] += EmptyCells[i - 1] * EmptyRowCount;
				m_FreeRows[i] += EmptyCellStaffle * EmptyRowCount;
			}

			m_UnreachableRows = new int[Unreachables.Length];
			for (var i = 1; i < m_UnreachableRows.Length; i++)
			{
				m_UnreachableRows[i] = m_UnreachableRows[i - 1];
				m_UnreachableRows[i] += Unreachables[i - 1];
				m_UnreachableRows[i] += UnreachableStaffle;
			}
			return this;
		}

		public static EvaluatorParameters GetDefault()
		{
			var pars = new EvaluatorParameters()
			// Elo: 1655, Avg: 0.197, Runs: 17968, ID: 947, Parent: 907
			{
				//EmptyRowsCalc = new int[] { 0, 590, 930, 1010, 1070, 1120, 1160, 1200, 1240, 1280, 1320, 1360, 1400, 1430, 1460, 1490, 1520, 1550, 1580, 1610, 1630, 1650 },
				//UnreachableRowsCalc = new int[] { 0, -8, -18, -29, -51, -76, -101, -127, -158, -190, -224, -258, -293, -329, -378, -427, -476, -527, -587, -648, -717, -788 },
				Holes = -78,
				Points = 107,
				Combo = 40,
				Skips = 12,
				UnreachableDistance = -5,
				TetrisPotential = 34,
				Groups = new int[] { 30, 26, -10, -23, -33, -37 },
				EmptyRowCount = 10,
				EmptyCellStaffle = 1,
				EmptyCells = new int[] { 58, 33, 7, 5, 4, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1 },
				Unreachables = new int[] { -7, -9, -10, -21, -24, -24, -25, -30, -31, -33, -33, -34, -35, -48, -48, -48, -50, -59, -60, -68, -70, -75 },
				UnreachableStaffle = -1,
			};
			return pars.Calc();
		}

		public static EvaluatorParameters GetInitial()
		{
			return new EvaluatorParameters()
			{
				EmptyRowCount = 10,
				EmptyCellStaffle = 1,
				Holes = -100,
			}.Calc();
		}
	}
}
