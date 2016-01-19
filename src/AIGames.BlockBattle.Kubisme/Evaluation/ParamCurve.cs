using System;
using System.Globalization;
using System.Xml.Serialization;

namespace AIGames.BlockBattle.Kubisme
{
	[Serializable]
	public class ParamCurve
	{
		public ParamCurve() : this(0) { }
		public ParamCurve(int value) : this(value, value, 1, 1) { }
		public ParamCurve(int start, int end, double factor, double power)
		{
			Start = start;
			End = end;
			Factor = factor;
			Power = power;
		}

		[XmlAttribute("start")]
		public int Start { get; set; }
		[XmlAttribute("end")]
		public int End { get; set; }
		[XmlAttribute("factor")]
		public double Factor { get; set; }
		[XmlAttribute("power")]
		public double Power { get; set; }

		public override string ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "new ParamCurve({0}, {1}, {2})", Start, End, Factor);
		}

		public int[] Calculate(int length = 22)
		{
			var values = new int[length];
			values[0] = Start;

			double delta = End - Start;	

			for (var i = 1; i < length; i++)
			{
				var f = Math.Pow(i, Power) / (length - 1.0);
				var val = Start + delta * Math.Pow(f, Factor);
				values[i] = (int)Math.Round(val);
			}
			return values;
		}
	}
}
