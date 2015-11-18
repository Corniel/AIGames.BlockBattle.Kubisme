using System;
using System.Collections.Generic;
using System.IO;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class BattleSimulation
	{
		public enum Outcome
		{
			Win,
			Draw,
			Loss,
		}

		public class Result
		{
			public Outcome Outcome { get; set; }
			public int Turns { get; set; }
			public int Points0 { get; set; }
			public int Points1 { get; set; }
		}

		public BattleSimulation(BotData bot0, BotData bot1, int depth)
		{
			Turns0 = new List<Field>();
			Turns1 = new List<Field>();

			Bot0 = bot0;
			Bot1 = bot1;
			SearchDepth = depth;
		}
		public List<Field> Turns0 { get; set; }
		public List<Field> Turns1 { get; set; }

		public BotData Bot0 { get; set; }
		public BotData Bot1 { get; set; }
		public int SearchDepth { get; set; }

		public static Field GetInitial()
		{
			return new Field(0, 0, 0, (byte)AppConfig.Data.Rows, new ushort[AppConfig.Data.Rows]);
		}

		public Result Run(MT19937Generator rnd, bool logGames)
		{
			var field0 = GetInitial();
			var field1 = GetInitial();
			var out0 = Field.None;
			var out1 = Field.None;

			var current = Block.All[rnd.Next(Block.All.Length)];
			var next = Block.All[rnd.Next(Block.All.Length)];

			var b0 = BattleBot.Create(rnd, Bot0.Pars, SearchDepth);
			var b1 = BattleBot.Create(rnd, Bot1.Pars, SearchDepth);

			var s0 = true;
			var s1 = true;

			var g0 = 0;
			var g1 = 0;

			int turns = 1;

			while(s0 && s1)
			{								  
				out0 = b0.GetResponse(field0, field1, current, next, turns);
				out1 = b1.GetResponse(field1, field0, current, next, turns);

				field0 = out0;
				field1 = out1;

				var t0 = field0.Points / 3;
				var t1 = field1.Points / 3;

				if (t0 > g0)
				{
					field1 = field1.Garbage(Row.GetGarbage(t0 - g0, g0 * 3, rnd));
					g0 = t0;
				}
				if (t1 > g1)
				{
					field0 = field0.Garbage(Row.GetGarbage(t1 - g1, g1 * 3, rnd));
					g1 = t1;
				}

				if (turns++ % 15 == 0)
				{
					field0 = field0.LockRow();
					field1 = field1.LockRow();
				}

				s0 = !field0.IsNone;
				s1 = !field1.IsNone;

				Turns0.Add(field0);
				Turns1.Add(field1);

				current = next;
				next = Block.All[rnd.Next(Block.All.Length)];
			}

			if (logGames)
			{
				var visualiser = new FieldVisualizer(16);
				var dir = new DirectoryInfo(Path.Combine("games", DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss_fff")));
				dir.Create();
				visualiser.Draw(Turns0, Turns1, dir);
			}

			var result = new Result()
			{
				Turns = Turns0.Count,
				Points0 = Turns0[Turns0.Count - 1].IsNone ? Turns0[Turns0.Count - 2].Points : Turns0[Turns0.Count - 1].Points,
				Points1 = Turns1[Turns1.Count - 1].IsNone ? Turns1[Turns1.Count - 2].Points : Turns1[Turns1.Count - 1].Points,
				Outcome = Outcome.Draw,
			};

			if (s0) { result.Outcome = Outcome.Win; }
			else if (s1) { result.Outcome = Outcome.Loss; }

			return result;
		}
	}
}
