﻿using AIGames.BlockBattle.Kubisme.Communication;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class KubismeBot : IBot
	{
		public KubismeBot() : this(new MT19937Generator(17)) { }
		public KubismeBot(MT19937Generator rnd)
		{
			DecisionMaker = new NodeDecisionMaker(rnd)
			{
				Evaluator = new Evaluator(),
				Generator = new MoveGenerator(),
				MaximumDepth = 10,
				MaximumDuration = TimeSpan.FromMilliseconds(700),
				DefaultEvaluation = EvaluatorParameters.GetDefault(),
			};
		}
		public NodeDecisionMaker DecisionMaker { get; set; }
		public Settings Settings { get; set; }
		public GameState State { get; set; }
		public Field Field { get; set; }
		public Field Opponent { get; set; }

		public virtual Block Current { get { return Block.Select(State.ThisPiece); } }
		public virtual Block Next { get { return Block.Select(State.NextPiece); } }

		public void ApplySettings(Settings settings)
		{
			Settings = settings;
		}

		public void Update(GameState state)
		{
			State = state;
			Field = Field.Create(State, Settings.YourBot);
			Opponent = Field.Create(State, Settings.OppoBot);
		}

		public BotResponse GetResponse(TimeSpan time)
		{
			SetDuration(time);

			var path = DecisionMaker.GetMove(Field, Opponent, Current, Next, State.Round);
			var move = new MoveInstruction(path.Moves.ToArray());

			var response = new BotResponse()
			{
				Move = move,
				Log = DecisionMaker.GetLog(),
			};
			return response;
		}

		protected virtual void SetDuration(TimeSpan time)
		{
			// Take 1/3 of the thinking time up to 1.2 seconds.
			var max = Math.Min(time.TotalMilliseconds / 3, 1200);
			// Take 500 ms or if you're really getting out of time 3/4 of the max.
			var min = Math.Min(500, (max * 3) / 4);

			DecisionMaker.MaximumDuration = TimeSpan.FromMilliseconds(max);
			DecisionMaker.MinimumDuration = TimeSpan.FromMilliseconds(min);
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never), ExcludeFromCodeCoverage]
		private string DebuggerDisplay
		{
			get
			{
				return string.Format("{0:00} Current: {1}, Next: {2}", State.Round, Current.Name, Next.Name);
			}
		}
	}
}
