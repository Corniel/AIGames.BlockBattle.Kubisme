using AIGames.BlockBattle.Kubisme.Genetics.Serialization;
using Qowaiv.Statistics;
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

		/// <summary>Gets the last (highest) ID.</summary>
		public int LastId { get; private set; } 

		protected int Capacity { get { return AppConfig.Data.BotCapacity; } }
		protected int PairingsRandom { get { return AppConfig.Data.PairingsRandom; } }
		protected Elo Bottom { get { return AppConfig.Data.EloBottom; } }

		public BotData Add(EvaluatorParameters defPars)
		{
			var id = GetNewId();
			var bot = new BotData(id, defPars);
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
				TryAdd(item.Id, item);
			}
			lock (locker)
			{
				var max = Keys.Max();
				if (max > LastId)
				{
					LastId = max;
				}
			}
		}

		public void Shrink()
		{
			var ids = new List<int>();
			lock (locker)
			{
				var sorted = ByElo().ToList();

				var lowest = GetLowestStableElo();

				// Lock if allowed.
				if (sorted[0].ShouldBeLocked)
				{
					sorted[0].Locked = true;
				}
				ids = sorted
					.Skip(Capacity)
					.Where(item => !item.Locked)
					.Select(item => item.Id).ToList();

				if (lowest != null)
				{
					ids.AddRange(sorted
						.Where(item => !item.IsStable && item.Elo < lowest.Elo)
						.Select(item => item.Id));
				}
			
			}
			foreach (var id in ids)
			{
				BotData bot;
				TryRemove(id, out bot);
			}
		}

		public void CloneBots(ParameterRandomizer rnd)
		{
			var count = 0;
			var test = Capacity;
			while (test >= Count)
			{
				count++;
				test >>= 1;
			}

			var highest = GetHighestElo();
			if (highest.IsStable)
			{
				Add(highest, rnd);

				if (count <= 0) { count = 1; }
				for (var i = 0; i < count; i++)
				{
					var stable = GetStable().OrderByDescending(bot => bot.GetWeight(rnd)).FirstOrDefault();
					if (stable != null)
					{
						Add(stable, rnd);
					}
				}
			}
		}

		public List<BattlePairing> GetPairings(MT19937Generator rnd)
		{
			lock (locker)
			{
				var best = GetHighestElo();
				var stable = GetHighestStableElo();

				var pairings = new List<BattlePairing>();
				
				// if the best is not the best stable, only pair the top unstable.
				if (best != stable && stable != null)
				{
					foreach (var bot in GetUnstable().Where(b => b.Elo >= stable.Elo))
					{
						pairings.AddRange(PairOther(bot));
					}
				}
				else
				{
					// Pair best.
					pairings.AddRange(PairOther(best));
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
					
					bot0.Stats.Update(result);
					bot1.Stats.Update(result.Mirror());
					
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
					
					bot0.Elo = elo0;
					bot1.Elo = elo1;

					if (bot0.IsStable || bot1.IsStable)
					{
						bot0.UpdateK();
						bot1.UpdateK();
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
		public BotData GetLowestStableElo()
		{
			return GetStable()
				.OrderBy(item => item.Elo)
				.FirstOrDefault();
		}
		
		private int GetNewId()
		{
			lock (locker)
			{
				return ++LastId;
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
