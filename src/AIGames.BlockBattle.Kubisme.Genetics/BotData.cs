using System;
using System.Reflection;
using System.Text;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	[Serializable]
	public class BotData : IComparable, IComparable<BotData>
	{
		public BotData()
		{
			Elo = 1000d;
			K = 12;
		}

		public BotData(int id, SimpleParameters pars): this()
		{
			Id = id;
			Pars = pars;
		}
		public BotData(int id, BotData parent, ParameterRandomizer rnd)
			: this(id, rnd.Randomize(parent.Pars))
		{
			Elo = parent.Elo - 20d;
			ParentId = parent.Id;
		}

		public SimpleParameters Pars { get; set; }

		public int Id { get; set; }
		public int ParentId { get; set; }
		public int Runs { get; set; }

		public long Points { get; set; }
		public long Turns { get; set; }

		public double Average { get { return Turns == 0 ? 0 : (double)Points / (double)Turns; } }

		public Elo Elo { get; set; }
		public double K { get; set; }

		public void UpdateK()
		{
			K = Math.Max(2, K * 0.99);
		}

		public int CompareTo(object obj)
		{
			return CompareTo((BotData)obj);
		}

		public int CompareTo(BotData other)
		{
			return other.Elo.CompareTo(Elo);
		}

		public override string ToString()
		{
			return String.Format("ID: {0}, Parent: {1}, Elo: {2:0} {3:#,###0} runs, Avg: {4:0.000}", Id, ParentId, Elo, Runs, Average);
		}

		private static readonly PropertyInfo[] Props = typeof(SimpleParameters).GetProperties(BindingFlags.Public | BindingFlags.Instance);
		
		public string ParametersToString()
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
	}
}
