using AIGames.BlockBattle.Kubisme.Communication;
using AIGames.BlockBattle.Kubisme.DecisionMaking;
using AIGames.BlockBattle.Kubisme.Evaluation;
using AIGames.BlockBattle.Kubisme.Models;
using System;

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
				Generator = new SimpleMoveGenerator(),
				MaximumDuration = TimeSpan.FromMilliseconds(500),
			};
		}
		public NodeDecisionMaker DecisionMaker { get; set; }
		public Settings Settings { get; set; }
		public GameState State { get; set; }
		public Field Field { get; set; }

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
		}

		public BotResponse GetResponse(TimeSpan time)
		{
			var path = DecisionMaker.GetMove(Field, State.Position, Current, Next);
			var move = MoveInstruction.Create(Current[path.Option], State.Position, path.Target);
			return new BotResponse()
			{
				Move = move,
				Log = DecisionMaker.Pars.ToString(),
			};
		}
	}
}
