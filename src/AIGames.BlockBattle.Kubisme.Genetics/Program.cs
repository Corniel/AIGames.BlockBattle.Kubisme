using System;
using System.Linq;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class Program
	{
		public static void Main(string[] args)
		{
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
