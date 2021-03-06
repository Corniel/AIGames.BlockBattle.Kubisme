﻿using System;
using System.Configuration;
using System.Globalization;
using System.Reflection;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class AppConfig
	{
		public static AppConfig Data = new AppConfig();

		private AppConfig()
		{
			SearchDepth = 2;
			EloInitial = 1600d;
			EloBottom = 1550d;
			KInitial = 12;
			KMinimum = 1;
			KMultiplier = 0.98;
			BotCapacity = 64;
			BotStable = 1024;
			PairingsRandom = 64;
			LockFactor = 10;
			Rows = 20;
		}
				
		public double EloInitial { get; set; }
		public double EloBottom { get; set; }
		public double KInitial { get; set; }
		public double KMinimum { get; set; }
		public double KMultiplier { get; set; }
		public int Rows { get; set; }
		public int Garbage { get; set; }

		public int BotCapacity { get; set; }
		public int BotStable { get; set; }
		public int SearchDepth { get; set; }

		public int PairingsRandom { get; set; }

		public int LockFactor { get; set; }

		public void UpdateFromConfig()
		{
			foreach (var property in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
			{
				var str = ConfigurationManager.AppSettings.Get(property.Name);

				Console.Write("Read {0}: '{1}'", property.Name, str);
				
				if (!String.IsNullOrEmpty(str))
				{
					if (property.PropertyType == typeof(Double))
					{
						double val;
						if (Double.TryParse(str, NumberStyles.Number, CultureInfo.InvariantCulture, out val))
						{
							property.SetValue(this, val);
						}
						else { Console.Write(" ERROR");}
					}
					else if (property.PropertyType == typeof(Int32))
					{
						int val;
						if (Int32.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out val))
						{
							property.SetValue(this, val);
						}
						else { Console.Write(" ERROR"); }
					}
				}
				Console.WriteLine();
			}
		}
	}
}
