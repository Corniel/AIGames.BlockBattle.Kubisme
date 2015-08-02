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

		[Test]
		public void DoRun_Round0007_NoExceptions()
		{
			using (var platform = new ConsolePlatformTester("input.version0007.txt"))
			{
				platform.DoRun(new KubismeBot());
			}
		}

		[Test]
		public void DoRun_Round0016_NoExceptions()
		{
			using (var platform = new ConsolePlatformTester("input.version0016.txt"))
			{
				platform.DoRun(new KubismeBot());
			}
		}
	}
}
