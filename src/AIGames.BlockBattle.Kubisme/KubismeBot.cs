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
			DecisionMaker = new DecisionMaker()
			{
				Evaluator = new SimpleEvaluator(),
				Generator = new MoveGenerator(),
			};
		}
		public DecisionMaker DecisionMaker { get; set; }
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

		public MoveInstruction GetMove(TimeSpan time)
		{
			var path = DecisionMaker.GetMove(Field, State.Position, Current, Next);
			return MoveInstruction.Create(State.Position, path);
		}
	}
}
