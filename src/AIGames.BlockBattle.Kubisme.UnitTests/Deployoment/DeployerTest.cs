using NUnit.Framework;
using System.IO;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Deployoment
{
	[TestFixture]
	public class DeployerTest
	{
		[Test]
		public void Deploy_Cubism()
		{
			var collectDir = new DirectoryInfo(@"..\..\..\AIGames.BlockBattle.Kubisme");
			var full = collectDir.FullName;
			Deployer.Run(collectDir, "Kubisme", "0002", false);
		}
	}
}
