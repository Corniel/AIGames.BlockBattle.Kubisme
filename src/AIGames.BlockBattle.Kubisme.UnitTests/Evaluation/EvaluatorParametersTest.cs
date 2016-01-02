using AIGames.BlockBattle.Kubisme.Genetics;
using NUnit.Framework;
using System;
using System.IO;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Evaluation
{
	[TestFixture]
	public class EvaluatorParametersTest
	{
		[Test]
		public void GetDefault_None_ParametersToString()
		{
			var pars = EvaluatorParameters.GetDefault();
			var act = BotData.ParametersToString(pars);
			Console.Write(act);

			var bots = new Bots();
			bots.Add(pars);
			bots.Save(new FileInfo("default.xml"));
		}
	}
}
