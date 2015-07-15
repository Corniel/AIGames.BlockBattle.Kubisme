using System.Collections.Generic;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class SimScores : List<SimScore>
	{
		public int CountWin { get { return this.Count(sc => sc.IsWin); } }
		public int CountDraw { get { return this.Count(sc => sc.IsDraw); } }
		public int CountLost { get { return this.Count(sc => sc.IsLost); } }

		public double Score
		{
			get
			{
				if (Count == 0) { return 0; }
				double points = CountWin + CountDraw * 0.5;
				return points / Count;
			}
		}
		public double WinningLength
		{
			get
			{
				if (!this.Any(sc => sc.IsWin)) { return 0; }
				return Wins.Average(sc => sc.Turns);
			}
		}
		public double LosingLength
		{
			get
			{
				if (!this.Any(sc => sc.IsLost)) { return 0; }
				return Losts.Average(sc => sc.Turns);
			}
		}

		public IEnumerable<SimScore> Wins { get { return this.Where(sc => sc.IsWin); } }
		public IEnumerable<SimScore> Draws { get { return this.Where(sc => sc.IsDraw); } }
		public IEnumerable<SimScore> Losts { get { return this.Where(sc => sc.IsLost); } }
	}
}
