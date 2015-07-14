using AIGames.BlockBattle.Kubisme.DecisionMaking;
using AIGames.BlockBattle.Kubisme.Models;
using System.Collections.Generic;
using System.Diagnostics;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.Genetics.Models
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
		public OpponentProfile Profile { get; set; }
		public Stopwatch Stopwatch { get; protected set; }

		public SimScore Run()
		{
			this.Stopwatch = Stopwatch.StartNew();

			var current = Block.All[Rnd.Next(7)];
			var next = Block.All[Rnd.Next(7)];
			var field = Field.Empty;

			while (true)
			{
				var move = DecisionMaker.GetMove(field, Position.Start, current, next);

				if (move.Equals(MovePath.None))
				{
					Stopwatch.Stop();
					return SimScore.Lost(Turns.Count, field.Points);
				}

				field = field.Apply(current[move.Option], move.Target);
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

	}
}
