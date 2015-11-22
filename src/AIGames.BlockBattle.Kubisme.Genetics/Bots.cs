using AIGames.BlockBattle.Kubisme.Genetics.Serialization;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class Bots : ConcurrentDictionary<int, BotData>
	{
		private static readonly object locker = new object();

		/// <summary>Gets the average Elo.</summary>
		public Elo AverageElo { get { return Values.Select(bot => bot.Elo).Avarage(); } }

		public int LastId { get { return m_LastId; } }
		private int m_LastId;

		protected int Capacity { get { return AppConfig.Data.BotCapacity; } }
		protected int PairingsRandom { get { return AppConfig.Data.PairingsRandom; } }
		protected Elo Bottom { get { return AppConfig.Data.EloBottom; } }

		public BotData Add(EvaluatorParameters defPars, EvaluatorParameters endPars)
		{
			var id = GetNewId();
			var bot = new BotData(id, defPars, endPars);
			TryAdd(id, bot);
			return bot;
		}

		public BotData Add(BotData parent, ParameterRandomizer rnd)
		{
			var id = GetNewId();
			var bot = new BotData(id, parent, rnd);
			TryAdd(id, bot);
			return bot;
		}

		public void AddRange(IEnumerable<BotData> items)
		{
			foreach (var item in items)
			{
				item.DefPars.Calc();
				item.EndPars.Calc();
				TryAdd(item.Id, item);
			}
			lock (locker)
			{
				var max = Keys.Max();
				if (max > m_LastId)
				{
					m_LastId = max;
				}
			}
		}

		public void Shrink()
		{
			var ids = new List<int>();
			lock (locker)
			{
				var sorted = ByElo().ToList();

				// Lock if allowed.
				if (sorted[0].ShouldBeLocked)
				{
					sorted[0].Locked = true;
				}
				ids = sorted
					.Skip(Capacity)
					.Where(item => !item.Locked)
					.Select(item => item.Id)
					.ToList();
			}
			foreach (var id in ids)
			{
				BotData bot;
				TryRemove(id, out bot);
			}
		}

		public void CloneBots(ParameterRandomizer rnd)
		{
			var highest = GetHighestElo();
			if (highest.IsStable)
			{
				CloneBot(rnd, highest, AppConfig.Data.CopyHighestElo);
				CloneBot(rnd, GetHighestAvg(), AppConfig.Data.CopyHighestScore);
				CloneBot(rnd, GetHighestTurnsAvg(), AppConfig.Data.CopyHighestTurnsAvg);
			}
		}
		private void CloneBot( ParameterRandomizer rnd, BotData bot, int copies)
		{
			if (bot == null) { return; }
			for (var i = 0; i < copies; i++)
			{
				Add(bot, rnd);
			}
		}

		public List<BattlePairing> GetPairings(MT19937Generator rnd)
		{
			lock (locker)
			{
				var best = GetHighestElo();
				var stable = GetHighestStableElo();

				var pairings = new List<BattlePairing>();
				// Pair best.
				pairings.AddRange(PairOther(best));
				pairings.AddRange(PairOther(GetHighestTurnsAvg()));
				pairings.AddRange(PairOther(GetHighestAvg()));

				// if the best is not the best stable, only pair the best.
				if (best != stable && stable != null)
				{
					pairings.AddRange(PairOther(best));
				}
				else
				{
					// Pair random.
					pairings.AddRange(GetRandomPairings(rnd));

					// Pair unstable.
					foreach (var bot in GetUnstable())
					{
						pairings.AddRange(PairOther(bot));
					}
				}
				return pairings;
			}
		}

		public void Process(ConcurrentQueue<BattlePairing> results)
		{
			var ids = new List<int>();
			lock (locker)
			{
				BattlePairing pairing;
				while (results.TryDequeue(out pairing))
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
							elo0 = bot0.Elo + (1.0 - z0) * bot0.K;
							elo1 = bot1.Elo + (0.0 - z1) * bot1.K;
							break;

						case BattleSimulation.Outcome.Draw:
							elo0 = bot0.Elo + (0.5 - z0) * bot0.K;
							elo1 = bot1.Elo + (0.5 - z1) * bot1.K;
							break;

						case BattleSimulation.Outcome.Loss:
							elo0 = bot0.Elo + (0.0 - z0) * bot0.K;
							elo1 = bot1.Elo + (1.0 - z1) * bot1.K;
							break;
					}
					if (bot1.IsStable)
					{
						bot0.Elo = elo0;
						bot0.UpdateK();
					}
					if (bot0.IsStable)
					{
						bot1.Elo = elo1;
						bot1.UpdateK();
					}
					if (!bot0.IsStable && !bot1.IsStable)
					{
						bot0.Elo = elo0;
						bot1.Elo = elo1;
					}
				}
				var avg = AverageElo;
				var dif = avg - AppConfig.Data.EloInitial;
				foreach (var bot in Values)
				{
					bot.Elo -= dif;
				}


				// Add elo's that are to low too.
				ids = ByElo()
					.Skip(2)
					.Where(item => !item.Locked && item.Elo < Bottom)
					.Select(item => item.Id)
					.ToList();
			}
			foreach (var id in ids)
			{
				BotData bot;
				TryRemove(id, out bot);
			}
		}

		private IEnumerable<BattlePairing> PairOther(BotData bot0)
		{
			if (bot0 == null) { yield break; }
			foreach (var bot1 in Values)
			{
				if (bot1 != bot0)
				{
					yield return new BattlePairing(bot0, bot1);
				}
			}
		}
		private IEnumerable<BattlePairing> GetRandomPairings(MT19937Generator rnd)
		{
			var count = Count;
			var bots = Values.ToArray();
			for (var match = 0; match < PairingsRandom; match++)
			{
				var index0 = rnd.Next(count);
				var index1 = rnd.Next(count);

				if (index0 != index1)
				{
					yield return new BattlePairing(bots[index0], bots[index1]);
				}
			}
		}

		public IEnumerable<BotData> GetStable()
		{
			return Values.Where(item =>item.IsStable);
		}
		public IEnumerable<BotData> GetUnstable()
		{
			return Values.Where(item => !item.IsStable);
		}
		public IEnumerable<BotData> ByElo()
		{
			return Values
				.OrderByDescending(item => item.Elo);
		}

		public BotData GetHighestElo()
		{
			return ByElo()
				.FirstOrDefault();
		}
		public BotData GetHighestStableElo()
		{
			return GetStable()
				.OrderByDescending(item => item.Elo)
				.FirstOrDefault();
		}

		public BotData GetHighestAvg()
		{
			return GetStable().OrderByDescending(bot => bot.PointsAvg).FirstOrDefault();
		}
		public BotData GetHighestTurnsAvg()
		{
			return GetStable().OrderByDescending(bot => bot.TurnsAvg).FirstOrDefault();
		}
		
		private int GetNewId()
		{
			lock (locker)
			{
				return ++m_LastId;
			}
		}

		#region I/O

		public void Save(FileInfo file)
		{
			using (var stream = new FileStream(file.FullName, FileMode.Create, FileAccess.Write))
			{
				Save(stream);
			}
		}
		public void Save(Stream stream)
		{
			var collection = new SimulationBotCollection();
			collection.AddRange(ByElo());
			collection.Save(stream);
		}

		public static Bots Load(FileInfo file)
		{
			using (var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
			{
				return Load(stream);
			}
		}
		public static Bots Load(Stream stream)
		{
			var collection = SimulationBotCollection.Load(stream);
			var bots = new Bots();
			bots.AddRange(collection);
			return bots;
		}

		#endregion
	}
}
