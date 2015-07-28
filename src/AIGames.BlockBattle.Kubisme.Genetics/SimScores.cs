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
			var count = Math.Min(Count, rght.Count);

			var scoreCount = 1 + (count >> 1);

			int leftS = (int)(Score * scoreCount);
			int rghtS = (int)(rght.Score * scoreCount);

			// Higher is better.
			var compare = rghtS.CompareTo(leftS);
			if (compare != 0) { return compare; }

			var lengthCount = 1 + (count >> 4);

			var leftL = (int)(LosingLength * lengthCount);
			var rghtL = (int)(rght.LosingLength * lengthCount);

			// Higher is better.
			compare = rghtL.CompareTo(leftL);
			if (compare != 0) { return compare; }

			var leftW = (int)(WinningLength * lengthCount);
			var rghtW = (int)(rght.WinningLength * lengthCount);

			// If no win, ignore.
			if (leftW > 0 && rghtW > 0)
			{
				// Lower is better.
				compare = leftW.CompareTo(rghtW);
				if (compare != 0) { return compare; }
			}

			// Rounded no differences, so unrounded.

			// Higher is better.
			compare = rght.Score.CompareTo(Score);
			if (compare != 0) { return compare; }

			// Higher is better.
			compare = rght.LosingLength.CompareTo(LosingLength);
			if (compare != 0) { return compare; }

			// Lower is better.
			compare = WinningLength.CompareTo(rght.WinningLength);
			return compare;
		}
	}
}
