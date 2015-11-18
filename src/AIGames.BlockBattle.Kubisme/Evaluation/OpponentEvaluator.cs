namespace AIGames.BlockBattle.Kubisme
{
	public class OpponentEvaluator
	{
		public IFieldGenerator Generator { get; set; }


		public OpponentEvaluation Evaluate(Field field, Block current)
		{
			var points = field.Points;
			var minFilled = 0;
			var maxPoints = points;

			foreach (var response in Generator.GetFields(field, current, true))
			{
				if (response.Points > maxPoints)
				{
					maxPoints = response.Points;
				}
				if (response.FirstFilled > minFilled)
				{
					minFilled = response.FirstFilled;
				}
			}
			return new OpponentEvaluation(minFilled, maxPoints / 3 - points / 3, points);
		}
	}
}
