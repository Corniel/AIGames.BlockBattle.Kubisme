using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			AppConfig.Data.UpdateFromConfig();
			Console.ReadLine();
			var simulator = new BattleSimulator(new MT19937Generator());
			simulator.InParallel = args.Length > 0 && args.Any(arg => arg.Contains("par"));
			simulator.LogGames = args.Length > 0 && args.Any(arg => arg.Contains("log"));
			while (true)
			{
				simulator.Run();
			}
		}
	}
}
