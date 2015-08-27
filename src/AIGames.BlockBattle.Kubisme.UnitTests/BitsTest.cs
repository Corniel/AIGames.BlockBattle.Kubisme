using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGames.BlockBattle.Kubisme.UnitTests
{
	[TestFixture]
	public class BitsTest
	{
		[Test]
		public void Count_ByteNum128_1()
		{
			var exp = 1;
			var act = Bits.Count((Byte)128);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Count_Int16Num128_1()
		{
			var exp = 1;
			var act = Bits.Count((Int16)128);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Count_UInt16Num128_1()
		{
			var exp = 1;
			var act = Bits.Count((UInt16)128);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Count_Int32Num128_1()
		{
			var exp = 1;
			var act = Bits.Count((Int32)128);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Count_UInt32Num128_1()
		{
			var exp = 1;
			var act = Bits.Count((UInt32)128);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Count_Int64Num128_1()
		{
			var exp = 1;
			var act = Bits.Count((Int64)128);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Count_UInt64Num128_1()
		{
			var exp = 1;
			var act = Bits.Count((UInt64)128);
			Assert.AreEqual(exp, act);
		}


		public static string[] Select(IEnumerable<ushort> masks)
		{
			return masks.Select(mask => Row.ToString(mask)).ToArray();
		}
	}
}
