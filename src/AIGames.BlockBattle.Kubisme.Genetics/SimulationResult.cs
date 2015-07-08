using AIGames.BlockBattle.Kubisme.Evaluation;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class SimulationResult<T> : IComparable, IComparable<SimulationResult<T>>
	{
		private static readonly PropertyInfo[] Props = typeof(SimpleEvaluator.Parameters).GetProperties(BindingFlags.Public | BindingFlags.Instance);

		public double Score 
		{
			get
			{
				if (Simulations == 0) { return 0; }
				return (double)Scores / (double)Simulations; 
		
			}
		}

		public int Scores { get; set; }
		public int Simulations { get; set; }
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
					writer.AppendFormat("{0} = new int[] {{ {1} }},", prop.Name, String.Join("," , vals));
					writer.AppendLine();
				}
			}
			writer.AppendLine("};");

			return writer.ToString();
		}
		
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string DebuggerDisplay
		{
			get
			{
				return String.Format("Score: {0:0.00}, Runs: {1:#,##0}", Score, Simulations);
			}
		}

		public int CompareTo(object obj)
		{
			return CompareTo(obj as SimulationResult<T>);
			
		}

		public int CompareTo(SimulationResult<T> other)
		{
			if (other == null) { return -1; }
			return other.Score.CompareTo(Score);
		}
	}
}
