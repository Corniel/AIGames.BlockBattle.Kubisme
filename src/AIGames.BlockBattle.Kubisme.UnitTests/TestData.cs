﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGames.BlockBattle.Kubisme.UnitTests
{
	public static class TestData
	{
		public static readonly Field Small = Field.Create(0, 0, @"
..........
..........
..........
..........
..........");

		public static readonly Field SmallFilled = Field.Create(0, 0, @"
..........
..........
..........
..........
....X.....
....X.....
...XX.....");
	}
}