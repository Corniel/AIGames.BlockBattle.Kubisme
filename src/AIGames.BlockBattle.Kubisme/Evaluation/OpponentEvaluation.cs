using System;

namespace AIGames.BlockBattle.Kubisme
{
	public struct OpponentEvaluation
	{
		public OpponentEvaluation(int minimumFirstFilled, int maximumGarbage, int pointsOld)
		{
			MinimumFirstFilled = minimumFirstFilled;
			MaximumGarbage = maximumGarbage;
			PointsOld = pointsOld;
		}
		public readonly int MinimumFirstFilled;
		public readonly int MaximumGarbage;
		public readonly int PointsOld;

		public override string ToString()
		{
			return String.Format("MinFilled: {0}, MaxGarbage: {1}", MinimumFirstFilled, MaximumGarbage);
		}
	}
}
