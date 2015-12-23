using System;

namespace AIGames.BlockBattle.Kubisme.UnitTests
{
	public struct TestField : IEquatable<TestField>
	{
		private readonly Field Field;

		public TestField(Field field)
		{
			Field = field;
		}

		public override bool Equals(object obj)
		{
			return base.Equals((TestField)obj);
		}
		public bool Equals(TestField other)
		{
			return
				Field.Points == other.Field.Points &&
				Field.Combo == other.Field.Combo &&
				Field.Skips == other.Field.Skips &&
				Field.FirstFilled == other.Field.FirstFilled &&
				Field.ToString() == other.Field.ToString();
		}
		public override int GetHashCode() { return ToString().GetHashCode(); }

		public override string ToString()
		{
			return string.Format("Points: {0}, Combo: {1}, Skips: {2}, FirstedFilled: {3}, Rows: {4}",
				Field.Points,
				Field.Combo,
				Field.Skips,
				Field.FirstFilled,
				Field);
		}
	}
}
