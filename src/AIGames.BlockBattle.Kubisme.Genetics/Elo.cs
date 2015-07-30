using AIGames.BlockBattle.Kubisme.Genetics.Conversion;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	/// <summary>Represents an Elo.</summary>
	[Serializable, DebuggerDisplay("{DebugToString()}")]
	[TypeConverter(typeof(EloTypeConverter))]
	public struct Elo : ISerializable, IXmlSerializable, IFormattable, IComparable, IComparable<Elo>
	{
		/// <summary>Represents the zero value of an Elo.</summary>
		public static readonly Elo Zero = new Elo() { m_Value = default(double) };

		#region Properties

		/// <summary>The inner value of the Elo.</summary>
		private Double m_Value;

		#endregion

		#region Methods

		/// <summary>Gets an z-score based on the two Elo's.</summary>
		/// <param name="elo0">
		/// The first Elo.
		/// </param>
		/// <param name="elo1">
		/// The second Elo.
		/// </param>
		public static double GetZScore(Elo elo0, Elo elo1)
		{
			var elo_div = elo1 - elo0;
			var z = 1 / (1 + Math.Pow(10.0, (double)elo_div / 400.0));

			return z;
		}

		#endregion

		#region Elo manipulation

		/// <summary>Increases the Elo with one.</summary>
		public Elo Increment()
		{
			return this.Add(1d);
		}
		/// <summary>Decreases the Elo with one.</summary>
		public Elo Decrement()
		{
			return this.Subtract(1d);
		}

		/// <summary>Pluses the Elo.</summary>
		public Elo Plus()
		{
			return Elo.Create(+m_Value);
		}
		/// <summary>Negates the Elo.</summary>
		public Elo Negate()
		{
			return Elo.Create(-m_Value);
		}

		/// <summary>Multiplies the current Elo with a factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public Elo Multiply(double factor) { return m_Value * factor; }

		/// <summary>Divides the current Elo by a factor.</summary>
		/// <param name="factor">
		/// The factor to divides by.
		/// </param>
		public Elo Divide(double factor) { return m_Value / factor; }

		/// <summary>Adds Elo to the current Elo.
		/// </summary>
		/// <param name="p">
		/// The percentage to add.
		/// </param>
		public Elo Add(Elo p) { return m_Value + p.m_Value; }

		/// <summary>Subtracts Elo from the current Elo.
		/// </summary>
		/// <param name="p">
		/// The percentage to Subtract.
		/// </param>
		public Elo Subtract(Elo p) { return m_Value - p.m_Value; }


		/// <summary>Increases the Elo with one.</summary>
		public static Elo operator ++(Elo elo) { return elo.Increment(); }
		/// <summary>Decreases the Elo with one.</summary>
		public static Elo operator --(Elo elo) { return elo.Decrement(); }

		/// <summary>Unitary plusses the Elo.</summary>
		public static Elo operator +(Elo elo) { return elo.Plus(); }
		/// <summary>Negates the Elo.</summary>
		public static Elo operator -(Elo elo) { return elo.Negate(); }

		/// <summary>Multiplies the Elo with the factor.</summary>
		public static Elo operator *(Elo elo, double factor) { return elo.Multiply(factor); }
		/// <summary>Divides the Elo by the factor.</summary>
		public static Elo operator /(Elo elo, double factor) { return elo.Divide(factor); }
		/// <summary>Adds the left and the right Elo.</summary>
		public static Elo operator +(Elo l, Elo r) { return l.Add(r); }
		/// <summary>Subtracts the right from the left Elo.</summary>
		public static Elo operator -(Elo l, Elo r) { return l.Subtract(r); }

		#endregion

		#region (XML) (De)serialization

		/// <summary>Initializes a new instance of Elo based on the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		private Elo(SerializationInfo info, StreamingContext context)
		{
			if (info == null) { throw new ArgumentNullException("info"); }
			m_Value = info.GetDouble("Value");
		}

		/// <summary>Adds the underlying propererty of Elo to the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null) { throw new ArgumentNullException("info"); }
			info.AddValue("Value", m_Value);
		}

		/// <summary>Gets the xml schema to (de) xml serialize an Elo.</summary>
		/// <remarks>
		/// Returns null as no schema is required.
		/// </remarks>
		XmlSchema IXmlSerializable.GetSchema() { return null; }

		/// <summary>Reads the Elo from an xml writer.</summary>
		/// <remarks>
		/// Uses the string parse function of Elo.
		/// </remarks>
		/// <param name="reader">An xml reader.</param>
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			var s = reader.ReadElementString();
			var val = Parse(s, CultureInfo.InvariantCulture);
			m_Value = val.m_Value;
		}

		/// <summary>Writes the Elo to an xml writer.</summary>
		/// <remarks>
		/// Uses the string representation of Elo.
		/// </remarks>
		/// <param name="writer">An xml writer.</param>
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			writer.WriteString(ToString("", CultureInfo.InvariantCulture));
		}

		#endregion

		#region IFormattable / ToString

		/// <summary>Returns a System.String that represents the current Elo for debug purposes.</summary>
		private string DebugToString()
		{
			return m_Value.ToString("0.#", CultureInfo.InvariantCulture);
		}

		/// <summary>Returns a System.String that represents the current Elo.</summary>
		public override string ToString()
		{
			return ToString(CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current Elo.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current Elo.</summary>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		public string ToString(IFormatProvider formatProvider)
		{
			return ToString("", formatProvider);
		}

		/// <summary>Returns a formatted System.String that represents the current Elo.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return m_Value.ToString(format, formatProvider);
		}

		#endregion

		#region IEquatable

		/// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
		/// <param name="obj">An object to compair with.</param>
		public override bool Equals(object obj) { return base.Equals(obj); }

		/// <summary>Returns the hash code for this Elo.</summary>
		/// <returns>
		/// A 32-bit signed integer hash code.
		/// </returns>
		public override int GetHashCode() { return m_Value.GetHashCode(); }

		/// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator ==(Elo left, Elo right)
		{
			return left.Equals(right);
		}

		/// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator !=(Elo left, Elo right)
		{
			return !(left == right);
		}

		#endregion

		#region IComparable

		/// <summary>Compares this instance with a specified System.Object and indicates whether
		/// this instance precedes, follows, or appears in the same position in the sort
		/// order as the specified System.Object.
		/// </summary>
		/// <param name="obj">
		/// An object that evaluates to a Elo.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.Value
		/// Condition Less than zero This instance precedes value. Zero This instance
		/// has the same position in the sort order as value. Greater than zero This
		/// instance follows value.-or- value is null.
		/// </returns>
		/// <exception cref="System.ArgumentException">
		/// value is not a Elo.
		/// </exception>
		public int CompareTo(object obj)
		{
			if (obj is Elo)
			{
				return CompareTo((Elo)obj);
			}
			throw new ArgumentException("Agrument must be an Elo.", "obj");
		}

		/// <summary>Compares this instance with a specified Elo and indicates
		/// whether this instance precedes, follows, or appears in the same position
		/// in the sort order as the specified Elo.
		/// </summary>
		/// <param name="other">
		/// The Elo to compare with this instance.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.
		/// </returns>
		public int CompareTo(Elo other) { return m_Value.CompareTo(other.m_Value); }


		/// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
		public static bool operator <(Elo l, Elo r) { return l.CompareTo(r) < 0; }

		/// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
		public static bool operator >(Elo l, Elo r) { return l.CompareTo(r) > 0; }

		/// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
		public static bool operator <=(Elo l, Elo r) { return l.CompareTo(r) <= 0; }

		/// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
		public static bool operator >=(Elo l, Elo r) { return l.CompareTo(r) >= 0; }

		#endregion

		#region (Explicit) casting

		/// <summary>Casts an Elo to a System.String.</summary>
		public static explicit operator string(Elo val) { return val.ToString(CultureInfo.CurrentCulture); }
		/// <summary>Casts a System.String to a Elo.</summary>
		public static explicit operator Elo(string str) { return Elo.Parse(str, CultureInfo.CurrentCulture); }


		/// <summary>Casts a decimal an Elo.</summary>
		public static implicit operator Elo(decimal val) { return new Elo() { m_Value = (double)val }; }
		/// <summary>Casts a decimal an Elo.</summary>
		public static implicit operator Elo(double val) { return new Elo() { m_Value = val }; }

		/// <summary>Casts an Elo to a decimal.</summary>
		public static explicit operator decimal(Elo val) { return (decimal)val.m_Value; }
		/// <summary>Casts an Elo to a double.</summary>
		public static explicit operator double(Elo val) { return val.m_Value; }

		#endregion

		#region Factory methods

		/// <summary>Converts the string to an Elo.</summary>
		/// <param name="s">
		/// A string containing an Elo to convert.
		/// </param>
		/// <returns>
		/// An Elo.
		/// </returns>
		/// <exception cref="System.FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static Elo Parse(string s)
		{
			return Parse(s, CultureInfo.CurrentCulture);
		}

		/// <summary>Converts the string to an Elo.</summary>
		/// <param name="s">
		/// A string containing an Elo to convert.
		/// </param>
		/// <param name="culture">
		/// A specified culture.
		/// </param>
		/// <returns>
		/// An Elo.
		/// </returns>
		/// <exception cref="System.FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static Elo Parse(string s, CultureInfo culture)
		{
			Elo val;
			if (Elo.TryParse(s, culture, out val))
			{
				return val;
			}
			throw new FormatException("Not an valid Elo");
		}

		/// <summary>Converts the string to an Elo.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing an Elo to convert.
		/// </param>
		/// <returns>
		/// The Elo if the string was converted successfully, otherwise Elo.Empty.
		/// </returns>
		public static Elo TryParse(string s)
		{
			Elo val;
			if (Elo.TryParse(s, out val))
			{
				return val;
			}
			return Elo.Zero;
		}

		/// <summary>Converts the string to an Elo.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing an Elo to convert.
		/// </param>
		/// <param name="result">
		/// The result of the parsing.
		/// </param>
		/// <returns>
		/// True if the string was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParse(string s, out Elo result)
		{
			return TryParse(s, CultureInfo.CurrentCulture, out result);
		}

		/// <summary>Converts the string to an Elo.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing an Elo to convert.
		/// </param>
		/// <param name="culture">
		/// A specified culture.
		/// </param>
		/// <param name="result">
		/// The result of the parsing.
		/// </param>
		/// <returns>
		/// True if the string was converted successfully, otherwise false.
		/// </returns>
		[SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Parsing is culture based by convension at Qowaiv.")]
		public static bool TryParse(string s, CultureInfo culture, out Elo result)
		{
			result = Elo.Zero;
			Double d;
			if (!string.IsNullOrEmpty(s))
			{
				var str = s.EndsWith("*") ? s.Substring(0, s.Length - 1) : s;
				if (Double.TryParse(str, NumberStyles.Number, culture, out d))
				{
					result = new Elo() { m_Value = d };
					return true;
				}
			}
			return false;
		}

		///  <summary >Creates an Elo from a Double. </summary >
		///  <param name="val" >
		/// A decimal describing an Elo.
		///  </param >
		///  <exception cref="System.FormatException" >
		/// val is not a valid Elo.
		///  </exception >
		public static Elo Create(Double val)
		{
			return new Elo() { m_Value = val };
		}

		#endregion

		#region Validation

		/// <summary>Returns true if the val represents a valid Elo, otherwise false.</summary>
		public static bool IsValid(string val)
		{
			return IsValid(val, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns true if the val represents a valid Elo, otherwise false.</summary>
		public static bool IsValid(string val, CultureInfo culture)
		{
			Elo elo;
			return TryParse(val, culture, out elo);
		}

		#endregion
	}
}