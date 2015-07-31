using AIGames.BlockBattle.Kubisme.Genetics.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class BattleSimulator
	{
		private static readonly object lockElo = new object();
		private static readonly object lockList = new object();

		private Stopwatch sw;

		public BattleSimulator(MT19937Generator rnd)
		{
			File = new FileInfo("parameters.xml");
			Capacity = 97;
			Stable = 1000;

			Rnd = rnd;
			Randomizer = new ParameterRandomizer(rnd);

			Bots = new List<BotData>();
		}

		public bool InParallel { get; set; }
		public int LastId { get; protected set; }
		public int Capacity { get; set; }
		public int Simulations { get; set; }
		public int Stable { get; set; }

		public MT19937Generator Rnd { get; set; }
		public ParameterRandomizer Randomizer { get; set; }
		public List<BotData> Bots { get; protected set; }
		public BotData BestBot { get { return Bots[0]; } }
		public BotData ReferenceBot { get { return Bots.Single(bot => bot.Id == 1); } }
		public FileInfo File { get; set; }

		public void Run()
		{
			lock (lockList)
			{
				if (File.Exists)
				{
					Bots.AddRange(SimulationBotCollection.Load(File));
					LastId = Bots.Max(bot => bot.Id);
				}
				else
				{
					Bots.Add(new BotData(++LastId, SimpleParameters.GetDefault()));
					Bots.Add(new BotData(++LastId, BestBot, Randomizer));
				}
			}
			sw = Stopwatch.StartNew();

			GetRandomPairings(Capacity * 20);

			var queue = new ConcurrentQueue<MT19937Generator>();
			for (var i = 0; i < 64; i++)
			{
				queue.Enqueue(new MT19937Generator());
			}

			while (true)
			{
				LogStatus();

				var pairings = new List<BattlePairing>();

				if (BestBot.Runs < Stable)
				{
					pairings.AddRange(PairOther(BestBot, 1));
				}
				lock (lockList)
				{
					if (Bots.Count > Capacity)
					{
						var refBot = ReferenceBot;
						Bots.RemoveRange(Capacity, Bots.Count - Capacity);

						// Always keep the ref bot.
						if (!Bots.Contains(refBot))
						{
							Bots.Add(refBot);
						}
					}
				}
				// if the best bot stable: clone.
				if (BestBot.Runs > Capacity)
				{
					var newBot = new BotData(++LastId, BestBot, Randomizer);
					Bots.Add(newBot);

					// the new created should challenge the others too.
					pairings.AddRange(PairOther(newBot, 2));
				}

				// Run also random matches.
				pairings.AddRange(GetRandomPairings(Bots.Count * 4));

				var copy = pairings.OrderBy(p => Rnd.Next()).ToList();

				if (InParallel)
				{
					SimulateParallel(queue, copy);
				}
				else
				{
					Simulate(copy);
				}
			}
		}

		private void Simulate(IEnumerable<BattlePairing> pairings)
		{
			foreach(var p in pairings)
			{
				RunSimulation(p.Bot0, p.Bot1, Rnd);
			}
		}

		private void SimulateParallel(ConcurrentQueue<MT19937Generator> queue, IEnumerable<BattlePairing> pairings)
		{
			try
			{
				Parallel.ForEach(pairings, p =>
				{
					MT19937Generator rnd;
					if (queue.Count > 0 && queue.TryDequeue(out rnd))
					{
						RunSimulation(p.Bot0, p.Bot1, rnd);
						queue.Enqueue(rnd);
					}
				});
			}
			catch { }
		}

		private IEnumerable<BattlePairing> GetRandomPairings(int number)
		{
			var count = Bots.Count;

			// play random matches too.
			for (var match = 0; match < number; match++)
			{
				var i0 = Rnd.Next(count);
				var i1 = Rnd.Next(count);

				if (i0 != i1)
				{
					yield return new BattlePairing(Bots[i0], Bots[i1]);
				}
			}
		}

		private IEnumerable<BattlePairing> PairOther(BotData bot0, int skip)
		{
			foreach (var bot1 in Bots.Skip(skip))
			{
				yield return new BattlePairing(bot0, bot1);
			}
		}

		private void RunSimulation(BotData bot0, BotData bot1, MT19937Generator rnd)
		{
			if (Simulations % 7 == 0)
			{
				lock (lockList)
				{
					Bots.Sort();
				}
				Console.Write("\r{0:d\\.hh\\:mm\\:ss} {1} ({2:0.00} /sec), Elo: {3:0.0}, Avg: {7:0.0000}, Runs: {4}, ID: {5}, Parent: {6}     ",
					sw.Elapsed, Simulations, Simulations / sw.Elapsed.TotalSeconds,
					BestBot.Elo,
					BestBot.Runs,
					BestBot.Id,
					BestBot.ParentId,
					BestBot.Average);
			}

			var simulation = new BattleSimulation(bot0, bot1);
			var result = simulation.Run(rnd);
			lock (lockElo)
			{
				Simulations++;
				bot0.Runs++;
				bot1.Runs++;

				bot0.Turns += result.Turns;
				bot1.Turns += result.Turns;

				bot0.Points += result.Points0;
				bot1.Points += result.Points1;

				var elo0 = bot0.Elo;
				var elo1 = bot1.Elo;

				var z0 = Elo.GetZScore(elo0, elo1);
				var z1 = Elo.GetZScore(elo1, elo0);

				switch (result.Outcome)
				{
					case BattleSimulation.Outcome.Win:
						bot0.Elo += (1.0 - z0) * bot0.K;
						bot1.Elo += (0.0 - z1) * bot1.K;
						break;

					case BattleSimulation.Outcome.Draw:
						bot0.Elo += (0.5 - z0) * bot0.K;
						bot1.Elo += (0.5 - z1) * bot1.K;
						break;

					case BattleSimulation.Outcome.Loss:
						bot0.Elo += (0.0 - z0) * bot0.K;
						bot1.Elo += (1.0 - z1) * bot1.K;
						break;
				}
				if (bot0 == ReferenceBot)
				{
					var dif = elo0 - bot0.Elo;
					UpdateBotElos(dif);
				}
				else if (bot1 == ReferenceBot)
				{
					var dif = elo1 - bot1.Elo;
					UpdateBotElos(dif);
				}
				bot0.UpdateK();
				bot1.UpdateK();
			}
		}

		private void UpdateBotElos(Elo dif)
		{
			foreach (var bot in Bots)
			{
				bot.Elo += dif;
			}
		}

		private void LogStatus()
		{
			var collection = new SimulationBotCollection();
			collection.AddRange(Bots);
			collection.Save(File);

			using (var writer = new StreamWriter("parameters.cs", false))
			{
				foreach (var bot in Bots)
				{
					writer.WriteLine("// Elo: {0:0}, Avg: {4:0.000}, Runs: {1}, ID: {2}, Parent: {3}", bot.Elo, bot.Runs, bot.Id, bot.ParentId, bot.Average);
					writer.WriteLine(bot.ParametersToString());
					writer.WriteLine();
				}
			}
			using (var writer = new StreamWriter("ratings.txt", false))
			{
				int pos = 1;
				foreach (var bot in Bots)
				{
					writer.WriteLine("{5,2} {0:0000.0} {4:0.0000}, Runs: {1,5}, ID: {2,5}, Parent: {3,5}", bot.Elo, bot.Runs, bot.Id, bot.ParentId, bot.Average, pos++);
				}
			}
		}
	}
}
