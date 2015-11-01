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
			CollectionAssert.AllItemsAreUnique(act);
		}
		[Test]
		public void Garbage2_All_Matches()
		{
			var act = Row.Garbage2.Select(r => Row.ToString((ushort)r)).ToArray();
			var exp = new string[] 
			{ 
				"..XXXXXXXX",
				".X.XXXXXXX",
				".XX.XXXXXX",
				".XXX.XXXXX",
				".XXXX.XXXX",
				".XXXXX.XXX",
				".XXXXXX.XX",
				".XXXXXXX.X",
				".XXXXXXXX.",
				"X..XXXXXXX",
				"X.X.XXXXXX",
				"X.XX.XXXXX",
				"X.XXX.XXXX",
				"X.XXXX.XXX",
				"X.XXXXX.XX",
				"X.XXXXXX.X",
				"X.XXXXXXX.",
				"XX..XXXXXX",
				"XX.X.XXXXX",
				"XX.XX.XXXX",
				"XX.XXX.XXX",
				"XX.XXXX.XX",
				"XX.XXXXX.X",
				"XX.XXXXXX.",
				"XXX..XXXXX",
				"XXX.X.XXXX",
				"XXX.XX.XXX",
				"XXX.XXX.XX",
				"XXX.XXXX.X",
				"XXX.XXXXX.",
				"XXXX..XXXX",
				"XXXX.X.XXX",
				"XXXX.XX.XX",
				"XXXX.XXX.X",
				"XXXX.XXXX.",
				"XXXXX..XXX",
				"XXXXX.X.XX",
				"XXXXX.XX.X",
				"XXXXX.XXX.",
				"XXXXXX..XX",
				"XXXXXX.X.X",
				"XXXXXX.XX.",
				"XXXXXXX..X",
				"XXXXXXX.X.",
				"XXXXXXXX..",
			};

			foreach (var line in act)
			{
				System.Console.WriteLine('"' + line + "\",");
			}
			CollectionAssert.AreEqual(exp, act);
			CollectionAssert.AllItemsAreUnique(act);
		}
		[Test]
		public void GetGarbage_5_Matches()
		{
			var rnd = new MT19937Generator(12);
			var act = Row.GetGarbage(5,0, rnd).Select(r => Row.ToString((ushort)r)).ToArray();
			var exp = new string[] 
			{ 
				"X.XXXXXXXX",
				"XX.XXX.XXX",
				"XXXXXXX.XX",
				"XXXXXX..XX",
				"XX.XXXXXXX",
			};
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void GetGarbage_2WithSkip_Matches()
		{
			// With this randomizer the skip code should be triggered.
			var rnd = new MT19937Generator(0);
			var act = Row.GetGarbage(2, 3, rnd).Select(r => Row.ToString((ushort)r)).ToArray();

			var exp = new string[] 
			{ 
				"XXX..XXXXX",
				"XXXXX.XXXX",
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
		public void Groups_ooXXoXXooX_3()
		{
			var row = Row.Create("..XX.XX..X");
			var exp = 3;
			var act = Row.Groups[row];
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Groups_xoXXXXXooo_2()
		{
			var row = Row.Create("X.XXXXX...");
			var exp = 2;
			var act = Row.Groups[row];
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Groups_Max_5()
		{
			var exp = (byte)5;
			var act = Row.Groups.Max();
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Row2BlocksConnected_All_Matches()
		{
			var act = Row.Row2BlocksConnected.Select(r => Row.ToString((ushort)r)).ToArray();
			var exp = new string[] 
			{ 
				"XX........",
				".XX.......",
				"..XX......",
				"...XX.....",
				"....XX....",
				".....XX...",
				"......XX..",
				".......XX.",
				"........XX",
			};
			CollectionAssert.AreEqual(exp, act);
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
