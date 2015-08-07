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

		private Stopwatch sw = Stopwatch.StartNew();

		public BattleSimulator(MT19937Generator rnd)
		{
			File = new FileInfo("parameters.xml");
			Capacity = 97;
			Stable = 1000;

			Rnd = rnd;
			Randomizer = new ParameterRandomizer(rnd);

			Bots = new List<BotData>();
			Results = new ConcurrentQueue<BattlePairing>();
		}

		public bool InParallel { get; set; }
		public int LastId { get; protected set; }
		public int Capacity { get; set; }
		public int Simulations { get; set; }
		public int Stable { get; set; }

		public MT19937Generator Rnd { get; set; }
		public ParameterRandomizer Randomizer { get; set; }
		public List<BotData> Bots { get; protected set; }
		public BotData BestBot { get; protected set; }
		public FileInfo File { get; set; }

		public BotData GetHighestElo()
		{
			var bot = Bots.FirstOrDefault(b => b.Runs > Stable);
			return bot ?? Bots[0];
		}
		public BotData GetHighestAvg()
		{
			return Bots.Where(bot => bot.Runs > Stable).OrderByDescending(bot => bot.Average).FirstOrDefault();
		}

		public ConcurrentQueue<BattlePairing> Results { get; protected set; }

		public void Run()
		{
			lock (lockList)
			{
				if (File.Exists)
				{
					Bots.AddRange(SimulationBotCollection.Load(File));
					LastId = Bots.Max(bot => bot.Id);
				}
				if (Bots.Count < 2)
				{
					Bots.Add(new BotData(++LastId, SimpleParameters.GetDefault()));
					BestBot = GetHighestElo();
					Bots.Add(new BotData(++LastId, BestBot, Randomizer));
				}
			}

			GetRandomPairings(Capacity * 20);

			var queue = new ConcurrentQueue<MT19937Generator>();

			var keepRunning = true;

			while (true)
			{
				ProcessElos();
				LogRankings();
				LogStatus();

				if (!keepRunning) { break; }

				for (var i = queue.Count; i < 64; i++)
				{
					queue.Enqueue(new MT19937Generator());
				}

				var pairings = new List<BattlePairing>();

				lock (lockList)
				{
					if (Bots.Count > Capacity)
					{
						for (var i = Bots.Count - 1; i >= Capacity; i--)
						{
							var bot = Bots[i];
							Bots.Remove(bot);
						}
					}
				}
				foreach (var bot in Bots)
				{
					if(bot.Runs < Stable)
					{
						pairings.AddRange(PairOther(bot));
					}
					if (bot == BestBot)
					{
						pairings.AddRange(PairOther(bot));
						break; 
					}
				}
				
				if (Bots.Any(b => b.Runs < Capacity))
				{
					foreach (var bot in Bots.Where(b => b.Runs < Capacity))
					{
						pairings.AddRange(PairOther(bot));
					}
				}
				else
				{
					for (var i = 1; i <= 8; i++)
					{
						var newBot = new BotData(++LastId, BestBot, Randomizer);
						Bots.Insert(i, newBot);

						// the new created should challenge the others too.
						pairings.AddRange(PairOther(newBot));
					}
				}
				// The bot with the best avg deserves extra attention as well.
				var bestAvg = GetHighestAvg();
				if (bestAvg != null)
				{
					pairings.AddRange(PairOther(bestAvg));
				}
				
				// Run also random matches.
				pairings.AddRange(GetRandomPairings(Bots.Count * 4));

				var copy = pairings.OrderBy(p => Rnd.Next()).ToList();

				if (InParallel)
				{
					keepRunning = SimulateParallel(queue, copy);
				}
				else
				{
					keepRunning = Simulate(copy);
				}
			}
		}

		private void ProcessElos()
		{
			lock (lockElo)
			{
				BattlePairing pairing;
				while (Results.TryDequeue(out pairing))
				{
					var bot0 = pairing.Bot0;
					var bot1 = pairing.Bot1;
					var result = pairing.Result;
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
							elo0 = bot0.Elo+ (1.0 - z0) * bot0.K;
							elo1 = bot1.Elo+(0.0 - z1) * bot1.K;
							break;

						case BattleSimulation.Outcome.Draw:
							elo0 = bot0.Elo+(0.5 - z0) * bot0.K;
							elo1 = bot1.Elo+(0.5 - z1) * bot1.K;
							break;

						case BattleSimulation.Outcome.Loss:
							elo0 = bot0.Elo+(0.0 - z0) * bot0.K;
							elo1 = bot1.Elo+(1.0 - z1) * bot1.K;
							break;
					}

					var stable0 = bot0.Runs > Stable;
					var stable1 = bot1.Runs > Stable;

					if (stable1)
					{
						bot0.Elo = elo0;
						bot0.UpdateK();
					}
					if (stable0)
					{
						bot1.Elo = elo1;
						bot1.UpdateK();
					}
					if (!stable0 && !stable1)
					{
						bot0.Elo = elo0;
						bot1.Elo = elo1;
					}

				}
			}
		}

		private bool Simulate(IEnumerable<BattlePairing> pairings)
		{
			foreach (var p in pairings)
			{
				RunSimulation(p.Bot0, p.Bot1, Rnd);
			}
			return true;
		}

		private bool SimulateParallel(ConcurrentQueue<MT19937Generator> queue, IEnumerable<BattlePairing> pairings)
		{
			var result = true;
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
					else
					{
						result = false;
					}
				});
				return result;
			}
			catch { return false; }
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

		private IEnumerable<BattlePairing> PairOther(BotData bot0)
		{
			foreach (var bot1 in Bots)
			{
				if (bot1 != bot0)
				{
					yield return new BattlePairing(bot0, bot1);
				}
			}
		}

		private void RunSimulation(BotData bot0, BotData bot1, MT19937Generator rnd)
		{
			if (Simulations % 17 == 0)
			{
				Console.Write("\r{0:d\\.hh\\:mm\\:ss} {1:#,##0} ({2:0.00} /sec) Last ID: {3}  ",
					sw.Elapsed,
					Simulations,
					Simulations / sw.Elapsed.TotalSeconds,
					LastId);
			}

			var simulation = new BattleSimulation(bot0, bot1);
			var result = simulation.Run(rnd);

			Results.Enqueue(new BattlePairing(bot0, bot1) { Result = result });

			lock (lockElo)
			{
				Simulations++;
			}
		}

		private void LogRankings()
		{
			lock (lockList)
			{
				Bots.Sort();
			}

			BestBot = GetHighestElo();

			var bestAvg = GetHighestAvg();

			Console.Clear();
			var max = Math.Min(Console.WindowHeight - 2, Bots.Count);
			var bestId = BestBot.Id;
			var bestParent = BestBot.ParentId;

			for (var pos = 1; pos <= max; pos++)
			{
				var bot = Bots[pos - 1];
				if (bot == BestBot)
				{
					Console.ForegroundColor = ConsoleColor.Yellow;
				}
				else if (bot == bestAvg)
				{
					Console.ForegroundColor = ConsoleColor.Blue;
				}
				else if (bot.ParentId == bestId)
				{
					Console.ForegroundColor = ConsoleColor.Green;
				}
				else if (bot.Id == bestParent)
				{
					Console.ForegroundColor = ConsoleColor.Red;
				}
				else if (bot.ParentId == bestParent)
				{
					Console.ForegroundColor = ConsoleColor.Magenta;
				}
				else if (bot.Runs > Stable)
				{
					Console.ForegroundColor = ConsoleColor.White;
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Gray;
				}
				Console.WriteLine("{5,2} {0:0000.0} {4:0.0000}, Runs: {1,5}, ID: {2,5}, Parent: {3,5}", bot.Elo, bot.Runs, bot.Id, bot.ParentId, bot.Average, pos);
			}
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine();
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
