using AIGames.BlockBattle.Kubisme.Genetics;
using AIGames.BlockBattle.Kubisme.Genetics.Serialization;
using NUnit.Framework;
using System.IO;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Genetics
{
	[TestFixture]
	public class SimulationBotCollectionTest
	{
		[Test]
		public void Save_OneBot_ToFile()
		{
			var collection = new SimulationBotCollection()
			{
				new BotData()
				{
					Id = 17,
					Locked = true,
					DefPars = EvaluatorParameters.GetDefault(),
					EndPars = EvaluatorParameters.GetEndGame(),
				},
			};

			var file = new FileInfo("collection.xml");
			collection.Save(file);
		}
	}
}
