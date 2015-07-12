using AIGames.BlockBattle.Kubisme.Evaluation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class SimulationResult<T> : IComparable, IComparable<SimulationResult<T>>
	{
		public SimulationResult()
		{
			Scores = new List<SimScore>();
		}
		private static readonly PropertyInfo[] Props = typeof(SimpleEvaluator.Parameters).GetProperties(BindingFlags.Public | BindingFlags.Instance);
		public int Id { get; set; }

		public List<SimScore> Scores { get; protected set; }

		public double Score 
		{
			get
			{
				if (Scores.Count == 0) { return 0; }
				return (double)Scores.Sum((sc)=> sc.Result)/ (double)(Scores.Count << 1);
			}
		}
		public double WinningLength
		{
			get
			{
				if (Score == 0) { return 0; }
				return Scores.Where(sc => sc.Result == 2).Average(sc => sc.Turns);
			}
		}
		public double LosingLength
		{
			get
			{
				if (!Scores.Any(sc => sc.Result == 0)) { return 0; }
				return Scores.Where(sc => sc.Result == 0).Average(sc => sc.Turns);
			}
		}
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
					writer.AppendFormat("{0} = new int[] {{ {1} }},", prop.Name, String.Join("," , vals));
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
				return String.Format("Score: {0:0.00%}, Win: {1:0.0}, Lose: {2:0.0} Runs: {3:#,##0}", Score, WinningLength, LosingLength,  Scores.Count);
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
