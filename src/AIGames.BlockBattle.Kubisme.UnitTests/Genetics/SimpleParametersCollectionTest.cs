using AIGames.BlockBattle.Kubisme.Genetics.Serialization;
using NUnit.Framework;
using System.IO;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Genetics
{
	[TestFixture]
	public class SimpleParametersTest
	{
		[Test]
		public void Save_Collection_Successful()
		{
			var pars = SimpleParameters.GetDefault();
			var collection = new SimpleParametersCollection() { pars };
			var file = new FileInfo("Save_Collection_Successful.xml");
			collection.Save(file);
		}
	}
}
