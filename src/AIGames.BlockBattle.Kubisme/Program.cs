using System.Diagnostics.CodeAnalysis;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme
{
	public class Program
	{
		[ExcludeFromCodeCoverage]
		public static void Main(string[] args)
		{
			Communication.ConsolePlatform.Run(new KubismeBot());
		}
	}
}
