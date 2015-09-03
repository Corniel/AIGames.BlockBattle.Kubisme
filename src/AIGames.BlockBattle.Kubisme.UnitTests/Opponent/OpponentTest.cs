using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Opponent
{
	[TestFixture]
	public class OpponentTest
	{
		[Test]
		public void Create_FieldWithTSpinPotential_()
		{
			var generator = new OpponentGenerator();

			var turn = 215;
			var field = Field.Create(0, 1, @"
..........
..........
..........
..........
..........
..........
XX..X..XX.
XXXX...XXX
XXXXX.XXXX
XXXXXXX.XX");

			var act = generator.Create(turn, field, Block.I, Block.T);

		}
	}
}
