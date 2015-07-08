using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AIGames.BlockBattle.Kubisme.Communication
{
	public struct FieldInstruction : IInstruction
	{
		public FieldInstruction(PlayerName name, int[,] field) 
		{
			m_Name = name;
			m_Field = field; 
		}

		public PlayerName Name { get { return m_Name; } }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private PlayerName m_Name;

		public int[,] Field { get { return m_Field; } }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int[,] m_Field;

		public override string ToString() 
		{
			var sb = new StringBuilder();
			sb.AppendFormat("update {0} field ", Name.ToString().ToLowerInvariant());

			for (var row = 0; row < Field.GetLength(0); row++)
			{
				if (row > 0) { sb.Append(';'); }
				for (var col = 0; col < Field.GetLength(1); col++)
				{
					if (col > 0) { sb.Append(',');}
					sb.Append(Field[row, col]);
				}
			}
			return sb.ToString();
		}

		internal static IInstruction Parse(PlayerName name, string[] splited)
		{
			var rows = new List<int[]>();

			foreach(var row in splited[3].Split(';'))
			{
				var cells = row.Split(',');
				if (!cells.All(c => c == "0" || c == "1")) { return null; }

				rows.Add(cells.Select(c => Int32.Parse(c)).ToArray());
			}
			if (!rows.All(row => row.Length == rows[0].Length)) { return null; }

			var field = new int[rows.Count, rows[0].Length];

			for (var row = 0; row < rows.Count; row++)
			{
				for (var col = 0; col < rows[0].Length; col++)
				{
					field[row, col] = rows[row][col];
				}
			}
			return new FieldInstruction(name, field);
		}
	}
}
