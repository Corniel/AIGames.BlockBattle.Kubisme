using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace AIGames.BlockBattle.Kubisme
{
	public class OpponentEvaluator
	{
		public IFieldGenerator Generator { get; set; }


		public OpponentEvaluation Evaluate(Field field, Block current, Block next, bool do3Ply = false)
		{
			var sw = Stopwatch.StartNew();
			var count = 0;

			var points0 = field.Points;
			
			var minFilled1 = -1;
			var maxPoints1 = points0;

			var minFilled2 = -1;
			var maxPoints2 = points0;

			Dictionary<Block, int> minFilled3 = null;
			Dictionary<Block, int> maxPoints3 = null;

			if (do3Ply)
			{
				minFilled3 = new Dictionary<Block, int>();
				maxPoints3 = new Dictionary<Block, int>();

				foreach (var block in Block.All)
				{
					minFilled3[block] = -1;
					maxPoints3[block] = points0;
				}
			}

			foreach (var response1 in Generator.GetFields(field, current, true))
			{
				count++;
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
					count++;
					if (response2.IsNone) { continue; }

					var deeper = false;

					if (response2.Points > maxPoints2)
					{
						maxPoints2 = response2.Points;
						deeper = true;
					}
					if (response2.FirstFilled > minFilled2)
					{
						minFilled2 = response2.FirstFilled;
						deeper = true;
					}
					if (do3Ply && deeper)
					{
						foreach (var block in Block.All)
						{
							foreach (var response3 in Generator.GetFields(response2, block, true))
							{
								count++;
								if (response3.IsNone) { continue; }

								if (response3.Points > maxPoints3[block])
								{
									maxPoints3[block] = response3.Points;
								}
								if (response3.FirstFilled > minFilled3[block])
								{
									minFilled3[block] = response3.FirstFilled;
								}
							}
						}
					}
				}
			}

			Console.WriteLine(count);
			Console.WriteLine(sw.Elapsed);

			return new OpponentEvaluation(points0, maxPoints1, maxPoints2, minFilled1, minFilled2, minFilled3, maxPoints3)
			{
				Count = count,
			};
		}
	}
}
