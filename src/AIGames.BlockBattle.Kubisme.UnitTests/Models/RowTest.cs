using NUnit.Framework;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Models
{
	[TestFixture]
	public class RowTest
	{
		[Test]
		public void Row7BlockOneHole_All_Matches()
		{
			var act = Row.Row7BlockOneHole.Select(r => new Row((ushort)r).ToString()).ToArray();
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
			var act = Row.Row8BlockOneHole.Select(r => new Row((ushort)r).ToString()).ToArray();
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
