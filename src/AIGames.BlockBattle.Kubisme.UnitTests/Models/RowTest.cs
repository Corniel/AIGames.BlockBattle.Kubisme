using NUnit.Framework;
using System.Linq;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Models
{
	[TestFixture]
	public class RowTest
	{
		[Test]
		public void Garbage_All_Matches()
		{
			var act = Row.Garbage.Select(r => Row.ToString((ushort)r)).ToArray();
			var exp = new string[] 
			{ 
				".XXXXXXXXX",
				"X.XXXXXXXX",
				"XX.XXXXXXX",
				"XXX.XXXXXX",
				"XXXX.XXXXX",
				"XXXXX.XXXX",
				"XXXXXX.XXX",
				"XXXXXXX.XX",
				"XXXXXXXX.X",
				"XXXXXXXXX.",
			};
			CollectionAssert.AreEqual(exp, act);
		}
		[Test]
		public void GetGarbage_5_Matches()
		{
			var rnd = new MT19937Generator(12);
			var act = Row.GetGarbage(5, rnd).Select(r => Row.ToString((ushort)r)).ToArray();
			var exp = new string[] 
			{ 
				"X.XXXXXXXX",
				"XXXX.XXXXX",
				"XXXXXX.XXX",
				"XXXXXXX.XX",
				"XX.XXXXXXX",
			};
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void GetGarbage_2WithSkip_Matches()
		{
			// With this randomizer the skip code should be triggered.
			var rnd = new MT19937Generator(0);
			var act = Row.GetGarbage(2, rnd).Select(r => Row.ToString((ushort)r)).ToArray();

			var exp = new string[] 
			{ 
				"XXXXXX.XXX",
				"XXXXXXXXX.",
				"XXXXXX.XXX",
				"XXXXXXX.XX",
				"XX.XXXXXXX",
			};
			CollectionAssert.AreEqual(exp, act);			
	}

		[Test]
		public void Flag_All_Matches()
		{
			var act = Row.Flag.Select(r => Bits.Count(r)).ToArray();
			var exp = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 };
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void Empty_None_Matches()
		{
			var act = Row.ToString(Row.Empty);
			var exp = "..........";
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Filled_None_Matches()
		{
			var act = Row.ToString(Row.Filled);
			var exp = "XXXXXXXXXX";
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Locked_None_11Bits()
		{
			var act = Bits.Count(Row.Locked);
			var exp = 11;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Row7BlockOneHole_All_Matches()
		{
			var act = Row.Row7BlockOneHole.Select(r => Row.ToString((ushort)r)).ToArray();
			var exp = new string[] 
			{ 
				"...XXXXXXX",
				"X...XXXXXX",
				"XX...XXXXX",
				"XXX...XXXX",
				"XXXX...XXX",
				"XXXXX...XX",
				"XXXXXX...X",
				"XXXXXXX...",
			};
			CollectionAssert.AreEqual(exp, act);
		}
		[Test]
		public void Row8BlockOneHole_All_Matches()
		{
			var act = Row.Row8BlockOneHole.Select(r => Row.ToString((ushort)r)).ToArray();
			var exp = new string[] 
			{ 
				"..XXXXXXXX",
				"X..XXXXXXX",
				"XX..XXXXXX",
				"XXX..XXXXX",
				"XXXX..XXXX",
				"XXXXX..XXX",
				"XXXXXX..XX",
				"XXXXXXX..X",
				"XXXXXXXX..",
			};
			CollectionAssert.AreEqual(exp, act);
		}
	}
}
