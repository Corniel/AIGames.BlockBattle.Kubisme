using System;
using System.Collections.Generic;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class SimScores : List<SimScore>, IComparable, IComparable<SimScores>
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

		public string ToUnitTestString()
		{
			return string.Format("Count: {0}, Score: {1:0.0%}, Winning: {2:0.00}, Losing: {3:0.00})", Count, Score, WinningLength, LosingLength);
		}

		public int CompareTo(object obj) { return CompareTo((SimScores)obj); }

		public int CompareTo(SimScores rght)
		{
			// Higher is better.
			var compare = rght.Score.CompareTo(Score);
			if (compare != 0) { return compare; }

			// Lower is better.
			compare = WinningLength.CompareTo(rght.WinningLength);
			if (compare != 0) { return compare; }

			// Higher is better.
			compare = rght.LosingLength.CompareTo(LosingLength); 
			return compare;
		}
	}
}
