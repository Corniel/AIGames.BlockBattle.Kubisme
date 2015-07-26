using System;
using System.Collections.Generic;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.Communication
{
	public class GameState
	{
		public GameState()
		{
			Player1 = new Player();
			Player2 = new Player();
		}

		public int Round { get; set; }
		public PieceType ThisPiece { get; set; }
		public PieceType NextPiece { get; set; }

		public Position Position { get; set; }

		public Player this[PlayerName name]
		{
			get
			{
				switch (name)
				{
					
					case PlayerName.Player1: return Player1;
					case PlayerName.Player2: return Player2;
					case PlayerName.None:
					default: return null;
				}
			}
		}

		public Player Player1 { get; set; }
		public Player Player2 { get; set; }

		public bool Apply(IInstruction instruction)
		{
			if (Mapping.ContainsKey(instruction.GetType()))
			{
				Mapping[instruction.GetType()].Invoke(instruction, this);
				return true;
			}
			return false;
		}

		public class Player
		{
			public int RowPoints { get; set; }
			public int Combo { get; set; }
			public int[,] Field { get; set; }
		}

		public static GameState Create(IEnumerable<IInstruction> instructions)
		{
			var state = new GameState();

			foreach(var instruction in instructions.Where(i => Mapping.ContainsKey(i.GetType())))
			{
				Mapping[instruction.GetType()].Invoke(instruction, state);
			}
			return state;
		}

		private static Dictionary<Type, Action<IInstruction, GameState>> Mapping = new Dictionary<Type, Action<IInstruction, GameState>>()
		{
			{
				typeof(RowPointsInstruction), (instruction, state) =>
				{
					var inst = (RowPointsInstruction)instruction;
					state[inst.Name].RowPoints = inst.Points;
				}
			},
			{
				typeof(ComboInstruction), (instruction, state) =>
				{
					var inst = (ComboInstruction)instruction;
					state[inst.Name].Combo = inst.Points;
				}
			},
			{ 
				typeof(FieldInstruction), (instruction, state) =>
				{
					var inst = (FieldInstruction)instruction;
					state[inst.Name].Field = inst.Field;
				}
			},
			{ 
				typeof(ThisPiecePositionInstruction), (instruction, state) =>
				{
					var inst = (ThisPiecePositionInstruction)instruction;
					state.Position = inst.Position;
				}
			},
			{ 
				typeof(RoundInstruction), (instruction, state) => { state.Round = ((RoundInstruction)instruction).Value; }
			},
			{ 
				typeof(ThisPieceInstruction), (instruction, state) => { state.ThisPiece = ((ThisPieceInstruction)instruction).Piece; }
			},
			{ 
				typeof(NextPieceInstruction), (instruction, state) => { state.NextPiece = ((NextPieceInstruction)instruction).Piece; }
			},
		};

		
	}
}
