using AIGames.BlockBattle.Kubisme.Genetics.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

		public static EvaluatorParameters GetDef()
		{
			return new EvaluatorParameters()
			{
				Holes = -200,
				Points = 75,
				TriplePotentialI = 100,
				TetrisPotential = 300,
				TSpinPontential = 600,
				Skips = 300,
			}
			.Calc();
		}
		public static EvaluatorParameters GetEnd()
		{
			return new EvaluatorParameters()
			{
				Holes = -200,
				Points = 10,
			}
			.Calc();
		}
		private Stopwatch sw = Stopwatch.StartNew();

		public BattleSimulator(MT19937Generator rnd)
		{
			File = new FileInfo("parameters.xml");
			SearchDepth = AppConfig.Data.SearchDepth;

			Rnd = rnd;
			Randomizer = new ParameterRandomizer(rnd);

			Bots = new Bots();
			Results = new ConcurrentQueue<BattlePairing>();
		}

		public bool InParallel { get; set; }
		public bool LogGames { get; set; }
		public int Simulations { get; set; }
		public int SearchDepth { get; set; }

		public MT19937Generator Rnd { get; set; }
		public ParameterRandomizer Randomizer { get; set; }
		public Bots Bots { get; protected set; }
		public BotData BestBot { get; protected set; }
		public FileInfo File { get; set; }

		public ConcurrentQueue<BattlePairing> Results { get; protected set; }

		public void Run()
		{
			lock (lockList)
			{
				if (File.Exists)
				{
					Bots.AddRange(SimulationBotCollection.Load(File));
				}
				if (Bots.Count < 2)
				{
					Bots.Clear();
					Bots.Add(GetDef(), GetEnd());
					Bots.Add(GetDef(), GetEnd());
					BestBot = Bots.GetHighestElo();
				}
			}
				
			var keepRunning = true;

			while (true)
			{
				LogRankings();
				LogStatus();

				if (!keepRunning) { break; }

				Bots.CloneBots(Randomizer);
				Bots.Shrink();

				var pairings = Bots.GetPairings(Rnd);
				var copy = pairings.ToList();

				if (InParallel)
				{
					keepRunning = SimulateParallel(copy);
				}
				else
				{
					keepRunning = Simulate(copy);
				}
				Bots.Process(Results);
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

		private bool SimulateParallel(IEnumerable<BattlePairing> pairings)
		{
			var result = true;
			try
			{
				Parallel.ForEach(pairings, p =>
				{
					var rnd = new MT19937Generator();
					RunSimulation(p.Bot0, p.Bot1, rnd);
				});
				return result;
			}
			catch { return false; }
		}

		private void RunSimulation(BotData bot0, BotData bot1, MT19937Generator rnd)
		{
			Console.Write("\r{0:d\\.hh\\:mm\\:ss} {1:#,##0} ({2:0.00} /sec) Last ID: {3}  ",
				sw.Elapsed,
				Simulations,
				Simulations / sw.Elapsed.TotalSeconds,
				Bots.LastId);
			
			var simulation = new BattleSimulation(bot0, bot1, SearchDepth);
			var result = simulation.Run(rnd, LogGames);

			Results.Enqueue(new BattlePairing(bot0, bot1) { Result = result });

			lock (lockElo)
			{
				Simulations++;
			}
		}

		private void LogRankings()
		{
			var sorted = Bots.ByElo().ToList();
			
			BestBot = Bots.GetHighestStableElo();

			Console.Clear();
			var max = Math.Min(Console.WindowHeight - 2, Bots.Count);
		
			for (var pos = 1; pos <= max; pos++)
			{
				var bot = sorted[pos - 1];
				if (bot == BestBot)
				{
					Console.ForegroundColor = ConsoleColor.Yellow;
				}
				else if (bot.IsStable)
				{
					Console.ForegroundColor = ConsoleColor.White;
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Gray;
				}
				Console.Write("{0,3}", pos);
				Console.Write(" {0:0000.0}", bot.Elo);
				Console.Write(", Runs: {0,6}", bot.Runs);
				Console.Write(" ({0:0.000} pt {1:00.00} #)", bot.PointsAvg, bot.TurnsAvg);
				Console.Write(", ID: {0,5}", bot.Id);
				if (bot.Locked) { Console.Write("*"); }
				Console.Write(" (par: {0}, gen: {1})", bot.ParentId, bot.Generation);
				Console.WriteLine();
			}
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine();
		}

		private void LogStatus()
		{
			Bots.Save(File);

			var sorted = Bots.ByElo().ToList();

			using (var writer = new StreamWriter("parameters.cs", false))
			{
				foreach (var bot in sorted)
				{
					writer.WriteLine("// Elo: {0:0}, Avg: {4:0.000}, Runs: {1}, ID: {2}, Parent: {3}, Gen: {5}", bot.Elo, bot.Runs, bot.Id, bot.ParentId, bot.PointsAvg, bot.Generation);
					writer.WriteLine(bot.ParametersToString(bot.DefPars));
					writer.WriteLine(bot.ParametersToString(bot.EndPars));
					writer.WriteLine();
				}
			}
		}
	}
}
