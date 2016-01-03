using Qowaiv;
using System;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	[Serializable]
	public class BotStats
	{
		[XmlAttribute("runs")]
		public long Runs { get; set; }
		[XmlAttribute("pt")]
		public long Points { get; set; }
		[XmlAttribute("turns")]
		public long Turns { get; set; }
		[XmlAttribute("w")]
		public long Wins { get; set; }
		[XmlAttribute("d")]
		public long Draws { get; set; }
		[XmlAttribute("l")]
		public long Losses { get; set; }

		[XmlAttribute("r1")]
		public long SingleLineClear { get; set; }
		[XmlAttribute("r2")]
		public long DoubleLineClear { get; set; }
		[XmlAttribute("r3")]
		public long TripleLineClear { get; set; }
		[XmlAttribute("r4")]
		public long QuadrupleLineClear { get; set; }
		
		[XmlAttribute("t1")]
		public long SingleTSpin { get; set; }
		[XmlAttribute("t2")]
		public long DoubleTSpin { get; set; }
		[XmlAttribute("clear")]
		public long PerfectClear { get; set; }

		public double PointsAvg { get { return Turns == 0 ? 0 : (double)Points / (double)Turns; } }
		public double TurnsAvg { get { return Runs == 0 ? 0 : (double)Turns / (double)Runs; } }

		public Percentage ScoreAvg { get { return Runs == 0 ? 0 : (double)(Wins + Draws * 0.5) / (double)Runs; } }
		public Percentage PerfectClearAvg { get { return Runs == 0 ? 0 : (double)PerfectClear / (double)Runs; } }
		public Percentage SingleTSpinAvg { get { return Turns == 0 ? 0 : 7.0 * (double)SingleTSpin / (double)Turns; } }
		public Percentage DoubleTSpinAvg { get { return Turns == 0 ? 0 : 7.0 * (double)DoubleTSpin / (double)Turns; } }
		public Percentage TetrisAvg { get { return Turns == 0 ? 0 : 7.0 * (double)QuadrupleLineClear / (double)Turns; } }

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendFormat("Runs: {0,5} ", Runs);
			sb.Append('(');
			sb.AppendFormat(CultureInfo.InvariantCulture, "{0:0.0%}, ", ScoreAvg);
			sb.AppendFormat(CultureInfo.InvariantCulture, "PT: {0:0.000}, ", PointsAvg);
			sb.AppendFormat(CultureInfo.InvariantCulture, "#: {0:0.0}, ", TurnsAvg);
			sb.AppendFormat(CultureInfo.InvariantCulture, "T1: {0:0.00%}, ", SingleTSpinAvg);
			sb.AppendFormat(CultureInfo.InvariantCulture, "T2: {0:0.00%}, ", DoubleTSpinAvg);
			sb.AppendFormat(CultureInfo.InvariantCulture, "I4: {0:0.00%}, ", TetrisAvg);
			sb.AppendFormat(CultureInfo.InvariantCulture, "CL: {0:0.00%}", PerfectClearAvg);
			sb.Append(')');
			return sb.ToString();
		}

		public void Update(Field before, Field after, Block block)
		{
			var score = ScoreDetector.GetScore(before, after, block);
			switch (score)
			{
				case BlockBattleScore.SingleLineClear: SingleLineClear++; break;
				case BlockBattleScore.DoubleLineClear: DoubleLineClear++;break;
				case BlockBattleScore.TripleLineClear: TripleLineClear++;break;
				case BlockBattleScore.QuadrupleLineClear: QuadrupleLineClear++;break;
				case BlockBattleScore.SingleTSpin: SingleTSpin++; break;
				case BlockBattleScore.DoubleTSpin: DoubleTSpin++; break;
				case BlockBattleScore.PerfectClear: PerfectClear++; break;
			}
		}

		public void Update(BattleSimulation.Result result)
		{
			Runs++;
			Turns += result.Turns;
			Points += result.Points0;
			Merge(result.Stats0);

			switch (result.Outcome)
			{
				case BattleSimulation.Outcome.Win: Wins++;break;
				case BattleSimulation.Outcome.Draw: Draws++; break;
				case BattleSimulation.Outcome.Loss: Losses++; break;
			}
		}

		public void Merge(BotStats other)
		{
			SingleLineClear += other.SingleLineClear;
			DoubleLineClear += other.DoubleLineClear;
			TripleLineClear += other.TripleLineClear;
			QuadrupleLineClear += other.QuadrupleLineClear;

			SingleTSpin += other.SingleTSpin;
			DoubleTSpin += other.DoubleTSpin;
			PerfectClear += other.PerfectClear;
		}
	}
}
