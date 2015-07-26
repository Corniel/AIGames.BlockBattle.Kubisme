using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class SimulationResult<T>: IComparable, IComparable<SimulationResult<T>>
	{
		public SimulationResult() { Scores = new SimScores(); }

		private static readonly PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
		public int Id { get; set; }

		public SimScores Scores { get; protected set; }

		public int Simulations { get { return Scores.Count; } }

		public T Pars { get; set; }

		public override string ToString()
		{
			var writer = new StringBuilder();
			writer.AppendLine("{");
			foreach (var prop in Props)
			{
				if (prop.PropertyType == typeof(int))
				{
					int val = (int)prop.GetValue(Pars);
					writer.AppendFormat("{0} = {1},", prop.Name, val);
					writer.AppendLine();
				}
				else if (prop.PropertyType == typeof(int[]))
				{
					int[] vals = (int[])prop.GetValue(Pars);
					writer.AppendFormat("{0} = new int[] {{ {1} }},", prop.Name, String.Join(",", vals));
					writer.AppendLine();
				}
			}
			writer.AppendLine("};");

			return writer.ToString();
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal string DebuggerDisplay
		{
			get
			{
				return String.Format("Score: {0:0.00%}, Win: {1:0.0}, Lose: {2:0.0} Runs: {3:#,##0}, ID: {4}", Scores.Score, Scores.WinningLength, Scores.LosingLength, Simulations, Id);
			}
		}

		public int CompareTo(object obj) { return CompareTo((SimulationResult<T>)obj); }

		public int CompareTo(SimulationResult<T> other)
		{
			return Scores.CompareTo(other.Scores);
		}
	}
}
