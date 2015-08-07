using AIGames.BlockBattle.Kubisme.Communication;
using System;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme
{
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
				MaximumDepth = 6,
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
			var ms = Math.Min(time.TotalMilliseconds / 2, 700);
			DecisionMaker.MaximumDuration = TimeSpan.FromMinutes(ms);
			DecisionMaker.Points = Predictor.GetPoints(Opponent, Current, Next);

			var path = DecisionMaker.GetMove(Field, Current, Next, State.Round);
			var move = new MoveInstruction(path.Moves.ToArray());
			
			return new BotResponse()
			{
				Move = move,
				Log = DecisionMaker.GetLog(),
			};
		}
	}
}
