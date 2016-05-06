using System;
using System.Globalization;
using System.Xml.Serialization;

namespace AIGames.BlockBattle.Kubisme
{
	/// <summary>Represents a curve that can be used to get a range of parameter values.</summary>
	/// <remarks>
	/// A curve of the form.
	/// 
	/// <code>Score(firstFilled) = a * Math.Pow(firstFilled, power) + delta</code>
	/// </remarks>
	[Serializable]
	public class ParamCurve
	{
		public ParamCurve() : this(0) { }
		public ParamCurve(int value) : this(0, 0, value) { }
		public ParamCurve(double a, double power, double delta)
		{
			A = a;
			Power = power;
			Delta = delta;
		}

		[XmlAttribute("a")]
		public double A { get; set; }

		[XmlAttribute("power")]
		public double Power { get; set; }


		[XmlAttribute("delta")]
		public double Delta { get; set; }

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "new ParamCurve({0}, {1}, {2})", A, Power, Delta);
		}

		public int[] Calculate() { return Calculate(22); }
		public int[] Calculate(int length)
		{
			var values = new int[length];
			
			for (var firstFilled = 0; firstFilled < length; firstFilled++)
			{
				var score = A * Math.Pow(firstFilled, Power) + Delta;
				values[firstFilled] = (int)Math.Round(score);
			}
			return values;
		}
	}
}
