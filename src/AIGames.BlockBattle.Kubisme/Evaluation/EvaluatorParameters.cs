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
			// Elo: 1662, Avg: 0.189, Runs: 2085, ID: 4308, Parent: 4295
			{
				//EmptyRowsCalc = new int[] { 0, 330, 510, 610, 680, 750, 820, 890, 950, 1010, 1070, 1130, 1190, 1250, 1310, 1370, 1420, 1470, 1520, 1570, 1620, 1670 },
				//UnreachableRowsCalc = new int[] { 0, -8, -16, -26, -36, -51, -69, -88, -113, -144, -176, -208, -242, -278, -320, -369, -418, -467, -525, -586, -654, -723 },
				Holes = -78,
				Points = 75,
				Combo = 27,
				Skips = 10,
				TetrisPotential = 41,
				Groups = new int[] { 30, 29, -5, -23, -24, -37 },
				EmptyRowCount = 10,
				EmptyCellStaffle = 3,
				EmptyCells = new int[] { 30, 15, 7, 4, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 1 },
				Unreachables = new int[] { -7, -7, -9, -9, -14, -17, -18, -24, -30, -31, -31, -33, -35, -41, -48, -48, -48, -57, -60, -67, -68, -74 },
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
