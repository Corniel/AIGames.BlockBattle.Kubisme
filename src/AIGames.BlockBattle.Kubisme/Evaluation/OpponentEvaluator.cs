namespace AIGames.BlockBattle.Kubisme
{
	public class OpponentEvaluator
	{
		public IFieldGenerator Generator { get; set; }


		public OpponentEvaluation Evaluate(Field field, Block current, Block next)
		{
			var points0 = field.Points;
			
			var minFilled1 = -1;
			var maxPoints1 = points0;

			var minFilled2 = -1;
			var maxPoints2 = points0;

			foreach (var response1 in Generator.GetFields(field, current, true))
			{
				if (response1.IsNone) { continue; }

				if (response1.Points > maxPoints1)
				{
					maxPoints1 = response1.Points;
				}
				if (response1.FirstFilled > minFilled1)
				{
					minFilled1 = response1.FirstFilled;
				}

				foreach (var response2 in Generator.GetFields(response1, next, true))
				{
					if (response2.IsNone) { continue; }

					if (response2.Points > maxPoints2)
					{
						maxPoints2 = response2.Points;
					}
					if (response2.FirstFilled > minFilled2)
					{
						minFilled2 = response2.FirstFilled;
					}
				}

			}
			return new OpponentEvaluation(points0, maxPoints1, maxPoints2, minFilled1, minFilled2);
		}
	}
}
