using System;

namespace AIGames.BlockBattle.Kubisme
{
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class ParameterTypeAttribute : Attribute
	{
		private ParameterTypeAttribute() { }
		public ParameterTypeAttribute(ParameterType tp)
		{
			this.ParameterType = tp;
		}
		public ParameterType ParameterType { get; private set; }
	}

	[Flags]
	public enum ParameterType
	{
		Default = 0,
		Ascending = 1,
		Descending = 2,
		Positive = 4,
	}
}
