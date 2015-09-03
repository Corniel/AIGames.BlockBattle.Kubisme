using AIGames.BlockBattle.Kubisme.Communication;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class KubismeBot : IBot
	{
		public KubismeBot()
		{
			DecisionMaker = new NodeDecisionMaker()
			{
				Evaluator = new ComplexEvaluator()
				{
					Parameters = ComplexParameters.GetDefault(),
				},
				Generator = new MoveGenerator(),
				MaximumDepth = 6,
				MaximumDuration = TimeSpan.FromMilliseconds(700),
			};
			Predictor = new OpponentGenerator();
		}
		public NodeDecisionMaker DecisionMaker { get; set; }
		public IOpponentGenerator Predictor { get; set; }
		public Settings Settings { get; set; }
		public GameState State { get; set; }
		public Field Field { get; set; }
		public Field Opponent { get; set; }

		public Block Current { get { return Block.Select(State.ThisPiece); } }
		public Block Next { get { return Block.Select(State.NextPiece); } }

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
			var ms = Math.Min(time.TotalMilliseconds / 3, 700);
			DecisionMaker.MaximumDuration = TimeSpan.FromMinutes(ms);

			if (time.TotalMilliseconds < 5000)
			{
				DecisionMaker.MaximumDepth = 5;
			}
			else
			{
				DecisionMaker.MaximumDepth = 6;
			}

			var opponent = Predictor.Create(State.Round, Opponent, Current, Next, Math.Min(16, DecisionMaker.MaximumDepth));
			((ComplexEvaluator)DecisionMaker.Evaluator).Opponent = opponent;
			((ComplexEvaluator)DecisionMaker.Evaluator).Initial = Field;

			var path = DecisionMaker.GetMove(Field, opponent, Current, Next, State.Round);
			var move = new MoveInstruction(path.Moves.ToArray());

			var response = new BotResponse()
			{
				Move = move,
				Log = DecisionMaker.GetLog(),
			};
			return response;
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
