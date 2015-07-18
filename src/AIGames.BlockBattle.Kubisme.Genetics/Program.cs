using System;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.WindowWidth = 100;
			var simulator = new Simulator(new MT19937Generator());
			simulator.Run();
		}
	}
}
