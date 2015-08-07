using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class GameSimulation
	{
		public GameSimulation()
		{
			this.Turns = new List<Field>();
		}
		public MT19937Generator Rnd { get; set; }
		public IDecisionMaker DecisionMaker { get; set; }
		public List<Field> Turns { get; protected set; }
		public IOpponentProfile Profile { get; set; }
		public Stopwatch Stopwatch { get; protected set; }

		public SimScore Run()
		{
			this.Stopwatch = Stopwatch.StartNew();

			var current = Block.All[Rnd.Next(7)];
			var next = Block.All[Rnd.Next(7)];
			var field = Field.Empty;

			while (true)
			{
				var move = DecisionMaker.GetMove(field, current, next, Turns.Count + 1);

				if (move.Equals(BlockPath.None))
				{
					Stopwatch.Stop();
					return SimScore.Lost(Turns.Count, field.Points);
				}

				field = DecisionMaker.BestField;
				Turns.Add(field);

				field = Profile.Apply(field, Turns.Count);
				var opponentAlive = Profile.IsAlive(field, Turns.Count);

				if (field.RowCount == 0)
				{
					Stopwatch.Stop();
					return opponentAlive ? SimScore.Lost(Turns.Count, field.Points) : SimScore.Draw(Turns.Count, field.Points);
				}
				if (!opponentAlive)
				{
					Stopwatch.Stop();
					return SimScore.Win(Turns.Count, field.Points);
				}
				current = next;
				next = Block.All[Rnd.Next(7)];
			}
		}

		public void Draw(DirectoryInfo dir)
		{
			var drawer = new FieldVisualizer(4);

			for (var turn = 1; turn < Turns.Count; turn++)
			{
				drawer.Draw(Turns[turn], dir, turn);
			}
		}
	}
}
