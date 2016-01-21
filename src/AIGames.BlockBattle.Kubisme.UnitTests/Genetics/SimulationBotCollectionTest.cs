using AIGames.BlockBattle.Kubisme.Genetics;
using AIGames.BlockBattle.Kubisme.Genetics.Serialization;
using NUnit.Framework;
using System.IO;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Genetics
{
	[TestFixture]
	public class SimulationBotCollectionTest
	{
		[Test, Category(Category.Deployment)]
		public void Save_OneBot_ToFile()
		{
			var collection = new SimulationBotCollection()
			{
				new BotData()
				{
					Id = 3,
					Locked = true,
					DefPars = EvaluatorParameters.GetDefault(),
				},
			};

			var file = new FileInfo("collection.xml");
			collection.Save(file);
		}
	}
}
