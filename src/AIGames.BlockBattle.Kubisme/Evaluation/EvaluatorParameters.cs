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

		/// <summary>Points for each cell that is connected to a wall.</summary>
		public int Walls { get; set; }

		/// <summary>Points for a potential Tetris.</summary>
		[ParameterType(ParameterType.Positive)]
		public int TetrisPotential { get; set; }

		/// <summary>Points for the different number of groups per reachable hole.</summary>
		/// <remarks>
		/// Index 0 is never called.
		/// </remarks>
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
			{
				EmptyRowCount = 10,
				EmptyCellStaffle = 1,
				Holes = -100,
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
			};
		}
	}
}
