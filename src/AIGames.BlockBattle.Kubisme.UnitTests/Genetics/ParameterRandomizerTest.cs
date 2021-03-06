﻿using AIGames.BlockBattle.Kubisme.Genetics;
using NUnit.Framework;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Genetics
{
	[TestFixture]
	public class ParameterRandomizerTest
	{
		[Test]
		public void Randomize_NegativeDescading_RandomValues()
		{
			var random = new MT19937Generator(17);
			var rnd = new ParameterRandomizer(random);

			var parameters = new ParameterCollectionClass()
			{
				NegativeDescading = new int[] { 100, 150 },
			};

			var act = rnd.Randomize(parameters).NegativeDescading;

			var exp = new int[] { -1, -68 };

			CollectionAssert.AreEqual(exp, act);
		}
	}

	public class ParameterCollectionClass
	{
		[ParameterType(ParameterType.Descending | ParameterType.Negative)]
		public int[] NegativeDescading { get; set; }
	}
}
