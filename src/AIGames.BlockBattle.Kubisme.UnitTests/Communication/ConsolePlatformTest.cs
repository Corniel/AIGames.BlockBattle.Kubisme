﻿using NUnit.Framework;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Communication;

[TestFixture, Category(Category.IntegrationTest), Ignore(reason: "Takes a while")]
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

	[Test]
	public void DoRun_Round0017_NoExceptions()
	{
		using (var platform = new ConsolePlatformTester("input.version0017.txt"))
		{
			platform.DoRun(new KubismeBot());
		}
	}

	[Test]
	public void DoRun_Round0018_NoExceptions()
	{
		using (var platform = new ConsolePlatformTester("input.version0018.txt"))
		{
			platform.DoRun(new KubismeBot());
		}
	}

	[Test]
	public void DoRun_Round0020_NoExceptions()
	{
		using (var platform = new ConsolePlatformTester("input.version0020.txt"))
		{
			platform.DoRun(new KubismeBot());
		}
	}

	[Test]
	public void DoRun_Round0021_NoExceptions()
	{
		using (var platform = new ConsolePlatformTester("input.version0021.txt"))
		{
			platform.DoRun(new KubismeBot());
		}
	}

	[Test]
	public void DoRun_Round0039_NoExceptions()
	{
		using (var platform = new ConsolePlatformTester("input.version0039.txt"))
		{
			platform.DoRun(new KubismeBot());
		}
	}

	[Test]
	public void DoRun_Round0040_NoExceptions()
	{
		using (var platform = new ConsolePlatformTester("input.version0040.txt"))
		{
			platform.DoRun(new KubismeBot());
		}
	}

	[Test]
	public void DoRun_Round0041_NoExceptions()
	{
		using (var platform = new ConsolePlatformTester("input.version0041.txt"))
		{
			platform.DoRun(new KubismeBot());
		}
	}
}
