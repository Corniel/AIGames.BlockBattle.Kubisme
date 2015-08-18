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
				Evaluator = new SimpleEvaluator()
				{
					Parameters = SimpleParameters.GetDefault(),
				},
				Generator = new MoveGenerator(),
				MaximumDepth = 7,
				MaximumDuration = TimeSpan.FromMilliseconds(700),
			};
			Predictor = new PointsPredictor();
		}
		public NodeDecisionMaker DecisionMaker { get; set; }
		public PointsPredictor Predictor { get; set; }
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
			DecisionMaker.Points = Predictor.GetPoints(Opponent, Current, Next);

			var path = DecisionMaker.GetMove(Field, Current, Next, State.Round);
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
