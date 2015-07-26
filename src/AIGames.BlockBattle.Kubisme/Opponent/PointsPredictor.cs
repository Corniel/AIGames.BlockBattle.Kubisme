namespace AIGames.BlockBattle.Kubisme
{
	public class PointsPredictor
	{
		public PointsPredictor()
		{
			Generator = new MoveGenerator();
		}

		public MoveGenerator Generator { get; set; }

		public int[] GetPoints(Field field, Block current, Block next)
		{
			var points = new int[10];
			points[0] = field.Points;
			points[1] = points[0];
			points[2] = points[0];
			var combo = 0;
			for (var i = 3; i < points.Length; i++)
			{
				points[i] = points[i - 1] + ++combo;
			}

			foreach (var depth1 in Generator.GetFields(field, current, Position.Start, false))
			{
				if (depth1.Points > points[1])
				{
					points[1] = depth1.Points;
				}
				foreach (var depth2 in Generator.GetFields(depth1, next, Position.Start, false))
				{
					if (depth2.Points > points[2])
					{
						points[2] = depth2.Points;
						combo = depth2.Combo;
						for (var i = 3; i < points.Length; i++)
						{
							points[i] = points[i - 1] + ++combo;
						}
					}
				}
			}
			return points;
		}
	}
}
