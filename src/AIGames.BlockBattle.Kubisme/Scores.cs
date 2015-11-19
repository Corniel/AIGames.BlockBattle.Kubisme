﻿using System;
using System.Globalization;

namespace AIGames.BlockBattle.Kubisme
{
	public static class Scores
	{
		public const int Draw = 0;

		public const int Max = 1000000;

		public static readonly int Win = Wins(512);
		public static readonly int Loss = Loses(512);

		public static int Wins(int ply) { return Max - ply; }
		public static int Loses(int ply) { return -Max + ply; }

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
				var ply = (score - Scores.Max);
				return String.Format(CultureInfo.InvariantCulture, "-oo {0}", ply);
			}

			var str = "";
			if (score > 0) { str = "+"; }
			else if (score == 0) { str = "="; }
			str += ((double)score / -(double)EvaluatorParameters.GetDefault().Holes).ToString("0.00", CultureInfo.InvariantCulture);
			return str;
		}
	}
}