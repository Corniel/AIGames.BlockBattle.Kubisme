using System;
using System.Globalization;

namespace AIGames.BlockBattle.Kubisme
{
	public static class Scores
	{
		public const int Draw = 0;

		private const int Max = +1000000;
		private const int Min = -1000000;

		private static readonly int Win = Wins(512);
		private static readonly int Loss = Loses(512);

		public static int Wins(int ply) { return Max - ply; }
		public static int Loses(int ply) { return Min + ply; }

		/// <summary>Return true if the score indicates a losing position.</summary>
		public static bool IsLosing(int score)
		{
			return score <= Loss;
		}

		public static string GetFormatted(int score)
		{
			if (score >= Scores.Win)
			{
				var ply = (Scores.Max - score);
				return String.Format(CultureInfo.InvariantCulture, "+oo {0}", ply);
			}
			if (score <= Scores.Loss)
			{
				var ply = (score - Scores.Min);
				return String.Format(CultureInfo.InvariantCulture, "-oo {0}", ply);
			}

			var str = "";
			if (score > 0) { str = "+"; }
			else if (score == 0) { str = "="; }
			str += ((double)score / -100.0).ToString("0.00", CultureInfo.InvariantCulture);
			return str;
		}
	}
}
