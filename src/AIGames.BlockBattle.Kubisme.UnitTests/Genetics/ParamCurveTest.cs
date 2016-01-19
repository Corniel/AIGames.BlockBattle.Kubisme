﻿using NUnit.Framework;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Genetics
{
	[TestFixture]
	public class ParamCurveTest
	{
		[Test]
		public void Calculate_0To5Factor1Power1_StraightLine()
		{
			var curve = new ParamCurve(0, 5, 1, 1);
			var act = curve.Calculate(6);
			var exp = new int[] { 0, 1, 2, 3, 4, 5 };

			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void Calculate_0To5Factor1Power2_StraightLine()
		{
			var curve = new ParamCurve(0, 5, 1, 2);
			var act = curve.Calculate(6);
			var exp = new int[] { 0, 1, 4, 9, 16, 25 };

			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void Calculate_1To9Factor2Power1_Parabool()
		{
			var curve = new ParamCurve(1, 9, 2, 1);
			var act = curve.Calculate(4);
			var exp = new int[] { 1, 2, 5, 9 };

			CollectionAssert.AreEqual(exp, act);
		}
	}
}
