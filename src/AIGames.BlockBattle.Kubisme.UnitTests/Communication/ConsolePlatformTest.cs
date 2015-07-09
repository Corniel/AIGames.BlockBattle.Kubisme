using NUnit.Framework;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Communication
{
	[TestFixture]
	public class ConsolePlatformTest
	{
		[Test]
		public void DoRun_KubismeSimple_NoExceptions()
		{
			using(var platform = new ConsolePlatformTester("input.simple.txt"))
			{
				platform.DoRun(new KubismeBot());
			}
		}
	}
}
