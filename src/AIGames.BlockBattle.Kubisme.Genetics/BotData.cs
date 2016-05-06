using Qowaiv.Statistics;
using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	[Serializable]
	public class BotData : IComparable, IComparable<BotData>
	{
		public BotData()
		{
			Elo = AppConfig.Data.EloInitial;
			K = AppConfig.Data.KInitial;
			Stats = new BotStats();
		}

		public BotData(int id, EvaluatorParameters def)
			: this()
		{
			Id = id;
			DefPars = def.Calc();
		}
		public BotData(int id, BotData parent, ParameterRandomizer rnd)
			: this(id, rnd.Randomize(parent.DefPars))
		{
			ParentId = parent.Id;
			Generation = parent.Generation + 1;
		}

		public int Id { get; set; }
		public int ParentId { get; set; }
		[XmlElement("Gen")]
		public int Generation { get; set; }

		public BotStats Stats { get; set; }

		public bool IsStable { get { return Stats.Runs >= AppConfig.Data.BotStable; } }
		public bool ShouldBeLocked { get { return Stats.Runs >= AppConfig.Data.BotStable * AppConfig.Data.LockFactor; } }

		public bool Locked { get; set; }

		public Elo Elo { get; set; }
		public double K { get; set; }

		public double GetWeight(ParameterRandomizer rnd)
		{
			var score = rnd.Rnd.NextDouble();
			score *= (double)Elo * (double)Elo;
			return score;
		}

		public EvaluatorParameters DefPars { get; set; }

		public void UpdateK()
		{
			K = Math.Max(AppConfig.Data.KMinimum, K * AppConfig.Data.KMultiplier);
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
			return String.Format(CultureInfo.InvariantCulture,
				"ID: {0}, Parent: {1}, Elo: {2:0} {3:#,###0} (5:0.0) runs, Avg: {4:0.000}",
				Id, ParentId, Elo, Stats.Runs, Stats.PointsAvg, Stats.TurnsAvg);
		}

		private static readonly PropertyInfo[] Props = typeof(EvaluatorParameters).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);

		public static string ParametersToString(EvaluatorParameters pars)
		{
			var writer = new StringBuilder();
			writer.AppendLine("{");
			foreach (var prop in Props)
			{
				if (prop.PropertyType == typeof(int))
				{
					int val = (int)prop.GetValue(pars);
					writer.AppendFormat("{0} = {1},", prop.Name, val);
					writer.AppendLine();
				}
				else if (prop.PropertyType == typeof(bool))
				{
					bool val = (bool)prop.GetValue(pars);
					writer.AppendFormat("{0} = {1},", prop.Name, val.ToString().ToLowerInvariant());
					writer.AppendLine();
				}
				else if (prop.PropertyType == typeof(int[]))
				{
					int[] vals = (int[])prop.GetValue(pars);
					writer.AppendFormat("{0} = new [] {{ {1} }},", prop.Name, String.Join(",", vals));
					writer.AppendLine();
				}
				else if (prop.PropertyType == typeof(ParamCurve))
				{
					ParamCurve val = (ParamCurve)prop.GetValue(pars);
					writer.AppendFormat("{0} = {1},", prop.Name, val);
					writer.AppendLine();
				}
			}
			writer.AppendLine("};");

			return writer.ToString();
		}
	}
}
