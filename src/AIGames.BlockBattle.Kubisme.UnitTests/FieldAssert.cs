using NUnit.Framework;
using System.Diagnostics;

namespace AIGames.BlockBattle.Kubisme.UnitTests
{
	public static class FieldAssert
	{
		[DebuggerStepThrough]
		public static void AreEqual(string str, int points, int combo, int skips, Field act)
		{
			var exp = Field.Create(points, combo, skips, str);
			Assert.AreEqual(new TestField(exp), new TestField(act));
		}

		[DebuggerStepThrough]
		public static void IsNone(Field act)
		{
			Assert.AreEqual(new TestField(Field.None), new TestField(act));
		}
	}
}
