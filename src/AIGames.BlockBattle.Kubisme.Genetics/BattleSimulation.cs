using System.Collections.Generic;
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

		public BattleSimulation(BotData bot0, BotData bot1)
		{
			Turns0 = new List<Field>();
			Turns1 = new List<Field>();

			Bot0 = bot0;
			Bot1 = bot1;
		}
		public List<Field> Turns0 { get; set; }
		public List<Field> Turns1 { get; set; }

		public BotData Bot0 { get; set; }
		public BotData Bot1 { get; set; }

		public Result Run(MT19937Generator rnd)
		{
			var field0 = Field.Empty;
			var field1 = Field.Empty;

			var current = Block.All[rnd.Next(Block.All.Length)];
			var next = Block.All[rnd.Next(Block.All.Length)];

			var b0 = BattleBot.Create(Bot0.Pars);
			var b1 = BattleBot.Create(Bot1.Pars);

			var s0 = true;
			var s1 = true;

			while(s0 && s1)
			{
				field0 = b0.GetResponse(field0, field1, current, next, Turns0.Count + 1);
				field1 = b1.GetResponse(field1, field0, current, next, Turns1.Count + 1);

				//var rows = 20 - (Turns0.Count / 20);

				//while (field0.RowCount > rows)
				//{
				//	field0 = field0.LockRow();
				//}
				//while (field1.RowCount > rows)
				//{
				//	field1 = field1.LockRow();
				//}

				Turns0.Add(field0);
				Turns1.Add(field1);

				s0 = field0.Points != Field.None.Points;
				s1 = field1.Points != Field.None.Points;

				current = next;
				next = Block.All[rnd.Next(Block.All.Length)];
			}

			var result = new Result()
			{
				Turns = Turns0.Count,
				Points0 = Turns0[Turns0.Count - 1].Points == Field.None.Points ? Turns0[Turns0.Count - 2].Points : Turns0[Turns0.Count - 1].Points,
				Points1 = Turns1[Turns1.Count - 1].Points == Field.None.Points ? Turns1[Turns1.Count - 2].Points : Turns1[Turns1.Count - 1].Points,
				Outcome = Outcome.Draw,
			};

			if (s0) { result.Outcome = Outcome.Win; }
			else if (s1) { result.Outcome = Outcome.Loss; }

			return result;
		}
	}
}
