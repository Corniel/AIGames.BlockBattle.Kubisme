using AIGames.BlockBattle.Kubisme.Genetics;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Genetics
{
	[TestFixture]
	public class ParameterRandomizerTest
	{
		[Test]
		public void Randomize_NegativeDescading_Min1Min19()
		{
			var random = new MT19937Generator(17);
			var rnd = new ParameterRandomizer(random);

			var parameters = new ParameterCollectionClass()
			{
				NegativeDescading = new int[]{100, 150},
			};

			var act = rnd.Randomize(parameters).NegativeDescading;

			var exp = new int[]{-1, -19};

			CollectionAssert.AreEqual(exp, act);
		}
	}

	public class ParameterCollectionClass
	{
		[ParameterType(ParameterType.Descending | ParameterType.Negative)]
		public int[] NegativeDescading { get; set; }
	}
}
