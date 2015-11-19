using System;
using System.Globalization;
using System.Text;

namespace AIGames.BlockBattle.Kubisme
{
	public struct PlyLog
	{
		public PlyLog(int round, BlockPath move, int score, byte depth, TimeSpan elapsed, int evaluations)
		{
			Ply = round;
			Move = move;
			Score = score;
			Depth = depth;
			Elapsed = elapsed;
			Evaluations = evaluations;
		}

		public readonly int Ply;
		public readonly BlockPath Move;
		public readonly int Score;
		public readonly byte Depth;
		public readonly TimeSpan Elapsed;
		public readonly int Evaluations;

		public override string ToString()
		{
			var sb = new StringBuilder()
				.AppendFormat(CultureInfo.InvariantCulture, "{0:00}/{1:00}. ", Ply, Depth)
				.AppendFormat(CultureInfo.InvariantCulture, "{0} ", Scores.GetFormatted(Score))
				.AppendFormat(CultureInfo.InvariantCulture, "{0:0.000}s ", Elapsed.TotalSeconds)
				.AppendFormat(CultureInfo.InvariantCulture, "({0:#,##0.0}kN, {1:#,##0.0}kN/s): ", Evaluations/1000.0, (double)Evaluations / Elapsed.TotalMilliseconds)
				.AppendFormat(CultureInfo.InvariantCulture, "{{{0}}}", Move);

			return sb.ToString();
		}
	}
}
