using System;
using System.Linq;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.WindowWidth = 100;

			var simulator = new BattleSimulator(new MT19937Generator());
			simulator.InParallel = args.Length > 0 && args.Any(arg => arg.Contains("par"));
			simulator.Run();
		}
	}
}
