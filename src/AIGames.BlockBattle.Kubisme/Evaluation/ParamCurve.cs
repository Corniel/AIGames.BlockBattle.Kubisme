﻿using System;
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
		public const int DefaultLength = 22;

		public ParamCurve() : this(0) { }
		public ParamCurve(int value) : this(0, 1, value) { }
		public ParamCurve(double a, double power, int delta)
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
		public int Delta { get; set; }

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "new ParamCurve({0}, {1}, {2})", A, Power, Delta);
		}

		public int[] Calculate() { return Calculate(DefaultLength); }
		public int[] Calculate(int length)
		{
			var values = new int[length];
			
			for (var firstFilled = 1; firstFilled <= length; firstFilled++)
			{
				var score = A * Math.Pow(firstFilled, Power) + Delta;
				values[firstFilled - 1] = (int)Math.Round(score);
			}
			return values;
		}
	}
}
