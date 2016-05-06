using NUnit.Framework;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Genetics
{
	[TestFixture]
	public class ParamCurveTest
	{
		[Test]
		public void Calculate_SingleValue_AllTheSame()
		{
			var curve = new ParamCurve(100);
			var act = curve.Calculate(3);
			var exp = new int[] { 100, 100, 100 };

			CollectionAssert.AreEqual(exp, act);
		}

		

		[Test]
		public void Calculate_0To5Factor1Power1_StraightLine()
		{
			var curve = new ParamCurve(1, 1, 0);
			var act = curve.Calculate(6);
			var exp = new int[] { 0, 1, 2, 3, 4, 5 };

			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void Calculate_Squared_Parabool()
		{
			var curve = new ParamCurve(1, 2, 0);
			var act = curve.Calculate(6);
			var exp = new int[] { 0, 1, 4, 9, 16, 25 };

			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void Calculate_HalfPower3Minus10_Parabool()
		{
			var curve = new ParamCurve(0.5, 3, -10);
			var act = curve.Calculate(4);
			var exp = new int[] { -10, -9, -5, 22 };

			CollectionAssert.AreEqual(exp, act);
		}
	}
}
