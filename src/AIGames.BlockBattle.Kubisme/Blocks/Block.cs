using AIGames.BlockBattle.Kubisme.Communication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace AIGames.BlockBattle.Kubisme
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public abstract class Block
	{
		public static readonly BlockO O = Block.New<BlockO>();

		public static readonly BlockI I = Block.New<BlockI>(new BlockILeft());

		public static readonly BlockS S = Block.New<BlockS>(new BlockSLeft());
		public static readonly BlockZ Z = Block.New<BlockZ>(new BlockZLeft());


		public static readonly BlockJ J = Block.New<BlockJ>(new BlockJLeft(), new BlockJUturn(), new BlockJRight());
		public static readonly BlockL L = Block.New<BlockL>(new BlockLLeft(), new BlockLUturn(), new BlockLRight());
		public static readonly BlockT T = Block.New<BlockT>(new BlockTLeft(), new BlockTUturn(), new BlockTRight());

		public static Block Select(PieceType piece)
		{
			switch (piece)
			{
				case PieceType.O: return Block.O;

				case PieceType.I: return Block.I;
				case PieceType.S: return Block.S;
				case PieceType.Z: return Block.Z;

				case PieceType.J: return Block.J;
				case PieceType.L: return Block.L;
				case PieceType.T: return Block.T;

				default: return null;
			}
		}

		public static readonly Block[] All = new Block[] { O, I, S, Z, J, L, T };

		protected Block()
		{

			Lookup = new ushort[Height, 16];

			for (var line = 0; line < Height; line++)
			{
				for (var column = 0; column < 10; column++)
				{
					Lookup[line, column] = (ushort)(Lines[line] << column);
				}
			}
			Columns = new int[11 - Width];
			for (sbyte i = 0; i < Columns.Length; i++)
			{
				Columns[i] = i;
			}
			ColumnMinimum = Columns[0];
			ColumnMaximum = Columns[Columns.Length - 1];

			switch (Rotation)
			{
				case RotationType.Left: InitialPath = BlockPath.Left; break;
				case RotationType.Uturn: InitialPath = BlockPath.Uturn; break;
				case RotationType.Right: InitialPath = BlockPath.Right; break;
				case RotationType.None:
				default: InitialPath = BlockPath.None; break;
			}
		}

		public enum RotationType
		{
			None = 0,
			Left = 1,
			Uturn = 2,
			Right = 3,
		}

		public ushort this[int line, int column] { get { return Lookup[line, column]; } }
		protected readonly ushort[,] Lookup;

		public abstract string Name { get; }

		public virtual RotationType Rotation { get { return RotationType.None; } }

		public abstract byte[] Lines { get; }

		public int Height { get { return Lines.Length; } }
		public abstract int Width { get; }

		public int Right { get { return 4 - Width; } }
		public int Bottom { get { return 4 - Height; } }

		protected readonly int[] Columns;
		public readonly int ColumnMinimum;
		public readonly int ColumnMaximum;


		public virtual int ChildCount { get { return 34; } }
		public virtual Position Start { get { return new Position(3, -1); } }

		public virtual int BranchingFactor0 { get { return 14; } }
		public virtual int BranchingFactor1 { get { return 10; } }

		public readonly Position[] Touches;

		public readonly BlockPath InitialPath;

		public int Count { get { return Lines.Sum(l => Bits.Count(l)); } }

		public Block[] Variations { get; private set; }

		public Block this[RotationType rotation]
		{
			get { return Variations[(int)rotation]; }
		}

		public abstract Block TurnLeft();
		public abstract Block TurnRight();

		public abstract Position TurnLeft(Position position);
		public abstract Position TurnRight(Position position);
		

		#region S Z J L T no rotation

		public virtual IEnumerable<int> GetColumns(Field field)
		{
			if (field.RowCount < Height) { }
			else if (field.FirstFilled > 0)
			{
				foreach (var column in Columns)
				{
					yield return column;
				}
			}
			else
			{
				var row = field[0];
				foreach (var column in Columns)
				{
					if ((accessibles[column] & row) == 0)
					{
						yield return column;
					}
				}
			}
		}

		public static readonly ushort[] accessibles = new ushort[]
		{
			0x003F,
			0x003E,
			0x003C,
			
			0X0038,

			0X0078,
			0x00F8,
			0x01F8,
			0x03F8,
		};

		public virtual BlockPath GetPath(Field field, int column)
		{
			return paths[column];
		}

		protected static readonly BlockPath[] paths = new BlockPath[]
		{
			BlockPath.Create(ActionType.Left, ActionType.Left, ActionType.Left, ActionType.Drop),
			BlockPath.Create(ActionType.Left, ActionType.Left, ActionType.Drop),
			BlockPath.Create(ActionType.Left, ActionType.Drop),
			
			BlockPath.Create(ActionType.Drop),

			BlockPath.Create(ActionType.Right, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.Right, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.Right, ActionType.Right, ActionType.Drop),
		};

		#endregion

		#region S, Z Left

		protected IEnumerable<int> GetColumnsSZLeft(Field field)
		{
			if (field.RowCount < Height) { }
			else if (field.FirstFilled > 0)
			{
				foreach (var column in Columns)
				{
					yield return column;
				}
			}
			else
			{
				var row = field[0];
				foreach (var column in Columns)
				{
					if ((accessiblesSZLeft[column] & row) == 0)
					{
						yield return column;
					}
				}
			}
		}

		public static readonly ushort[] accessiblesSZLeft = new ushort[]
		{
			0x003F,
			0x003E,
			0x003C,
			
			0X0038,
			0X0038,

			0X0078,
			0x00F8,
			0x01F8,
			0x03F8,
		};

		protected static readonly BlockPath[] pathsSZLeft = new BlockPath[]
		{
			BlockPath.Create(ActionType.Left, ActionType.Left, ActionType.Left, ActionType.TurnLeft, ActionType.Drop),
			BlockPath.Create(ActionType.Left, ActionType.Left, ActionType.TurnLeft, ActionType.Drop),
			BlockPath.Create(ActionType.Left, ActionType.TurnLeft, ActionType.Drop),

			BlockPath.Create(ActionType.TurnLeft, ActionType.Drop),
			BlockPath.Create(ActionType.TurnRight, ActionType.Drop),

			BlockPath.Create(ActionType.Right, ActionType.TurnRight, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.TurnRight, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.Right, ActionType.TurnRight, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.Right, ActionType.Right, ActionType.TurnRight, ActionType.Drop),
		};

		#endregion

		#region J L T Left

		protected IEnumerable<int> GetColumnsJLTLeft(Field field)
		{
			if (field.RowCount < Height) { }
			else if (field.FirstFilled > 0)
			{
				foreach (var column in Columns)
				{
					yield return column;
				}
			}
			else
			{
				var row = field[0];
				foreach (var column in Columns)
				{
					if ((accessiblesJLTLeft[column] & row) == 0)
					{
						yield return column;
					}
				}
			}
		}

		public static readonly ushort[] accessiblesJLTLeft = new ushort[]
		{
			0x003F,
			0x003E,
			0x003C,
			
			0X0038,

			0X0078,
			0x00F8,
			0x01F8,
			0x03F8,

			0x03F8,
		};

		protected static readonly BlockPath[] pathsJLTLeft = new BlockPath[]
		{
			BlockPath.Create(ActionType.Left, ActionType.Left, ActionType.Left, ActionType.TurnLeft, ActionType.Drop),
			BlockPath.Create(ActionType.Left, ActionType.Left, ActionType.TurnLeft, ActionType.Drop),
			BlockPath.Create(ActionType.Left, ActionType.TurnLeft, ActionType.Drop),

			BlockPath.Create(ActionType.TurnLeft, ActionType.Drop),

			BlockPath.Create(ActionType.Right, ActionType.TurnLeft, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.TurnLeft, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.Right, ActionType.TurnLeft, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.Right, ActionType.Right, ActionType.TurnLeft, ActionType.Drop),

			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.Right, ActionType.Right, ActionType.TurnLeft, ActionType.Right, ActionType.Drop),
		};
		#endregion

		#region J L T Right

		protected IEnumerable<int> GetColumnsJLTRight(Field field)
		{
			if (field.RowCount < Height) { }
			else if (field.FirstFilled > 0)
			{
				foreach (var column in Columns)
				{
					yield return column;
				}
			}
			else
			{
				var row = field[0];
				foreach (var column in Columns)
				{
					if ((accessiblesJLTRight[column] & row) == 0)
					{
						yield return column;
					}
				}
			}
		}

		public static readonly ushort[] accessiblesJLTRight = new ushort[]
		{
			0x003F,

			0x003F,
			0x003E,
			0x003C,
			
			0X0038,

			0X0078,
			0x00F8,
			0x01F8,
			0x03F8,
		};

		protected static readonly BlockPath[] pathsJLTRight = new BlockPath[]
		{
			BlockPath.Create(ActionType.Left, ActionType.Left, ActionType.Left, ActionType.TurnRight,  ActionType.Left, ActionType.Drop),

			BlockPath.Create(ActionType.Left, ActionType.Left, ActionType.Left, ActionType.TurnRight, ActionType.Drop),
			BlockPath.Create(ActionType.Left, ActionType.Left, ActionType.TurnRight, ActionType.Drop),
			BlockPath.Create(ActionType.Left, ActionType.TurnRight, ActionType.Drop),

			BlockPath.Create(ActionType.TurnRight, ActionType.Drop),

			BlockPath.Create(ActionType.Right, ActionType.TurnRight, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.TurnRight, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.Right, ActionType.TurnRight, ActionType.Drop),
			BlockPath.Create(ActionType.Right, ActionType.Right, ActionType.Right, ActionType.Right, ActionType.TurnRight, ActionType.Drop),
		};
		#endregion

		#region J L T U-turn

		protected IEnumerable<int> GetColumnsJLTUTurn(Field field)
		{
			// TODO: make flexible.
			if (field.FirstFilled > 1)
			{
				return Columns;
			}
			else
			{
				return Enumerable.Empty<int>();
			}
		}

		protected static readonly BlockPath[] pathsJLTUturn = new BlockPath[]
		{
			BlockPath.Create(ActionType.TurnLeft, ActionType.TurnLeft, ActionType.Left, ActionType.Left, ActionType.Left, ActionType.Drop),
			BlockPath.Create(ActionType.TurnLeft, ActionType.TurnLeft, ActionType.Left, ActionType.Left, ActionType.Drop),
			BlockPath.Create(ActionType.TurnLeft, ActionType.TurnLeft, ActionType.Left, ActionType.Drop),
			
			BlockPath.Create(ActionType.TurnLeft, ActionType.TurnLeft,ActionType.Drop),

			BlockPath.Create(ActionType.TurnLeft, ActionType.TurnLeft,ActionType.Right, ActionType.Drop),
			BlockPath.Create(ActionType.TurnLeft, ActionType.TurnLeft,ActionType.Right, ActionType.Right, ActionType.Drop),
			BlockPath.Create(ActionType.TurnLeft, ActionType.TurnLeft,ActionType.Right, ActionType.Right, ActionType.Right, ActionType.Drop),
			BlockPath.Create(ActionType.TurnLeft, ActionType.TurnLeft,ActionType.Right, ActionType.Right, ActionType.Right, ActionType.Right, ActionType.Drop),
		};
		#endregion

		[ExcludeFromCodeCoverage]
		public override string ToString()
		{
			var sb = new StringBuilder();
			for (var i = 0; i < Height; i++)
			{
				if (i > 0) { sb.Append('|'); }
				switch (Lines[i])
				{
					case 00: sb.Append("...."); break;
					case 01: sb.Append("X..."); break;
					case 02: sb.Append(".X.."); break;
					case 03: sb.Append("XX.."); break;

					case 04: sb.Append("..X."); break;
					case 05: sb.Append("X.X."); break;
					case 06: sb.Append(".XX."); break;
					case 07: sb.Append("XXX."); break;

					case 08: sb.Append("...X"); break;
					case 09: sb.Append("X..X"); break;
					case 10: sb.Append(".X.X"); break;
					case 11: sb.Append("XX.X"); break;

					case 12: sb.Append("..XX"); break;
					case 13: sb.Append("X.XX"); break;
					case 14: sb.Append(".XXX"); break;
					case 15: sb.Append("XXXX"); break;
					default: sb.Append("????"); break;
				}
			}
			return sb.ToString();
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never), ExcludeFromCodeCoverage]
		internal string DebuggerDisplay
		{
			get
			{
				return string.Format("{0}{1}", Name, Rotation == RotationType.None ? "" : " " + Rotation.ToString());
			}
		}

		/// <summary>Get the first row that should be tested for this block.</summary>
		public int GetMinRow(Field field)
		{
			var row = field.FirstFilled - Height;
			return row < 0 ? 0 : row;
		}

		/// <summary>Get the last row (exclusive) that should be tested for this block.</summary>
		public int GetMaxRow(Field field)
		{
			var row = field.RowCount - Height;
			return row;
		}

		/// <summary>Initializes a block.</summary>
		internal static T New<T>(params T[] variations) where T : Block
		{
			var block = Activator.CreateInstance<T>();

			var list = variations.ToList();
			list.Insert(0, block);
			foreach (var item in list)
			{
				item.Variations = list.ToArray();
			}
			return block;
		}

		public bool TouchPosition(Position position, int[] targets)
		{
			var minRow = position.Row;
			var maxRow = minRow + Height;

			for (var l = 0 ;l < Height; l++)
			{
				var line = this[l, position.Col];

				if ((line & targets[l + position.Row]) != 0)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Returns true if there is a possibility for the block to
		/// reach the current row, given the previous one.
		/// </summary>
		public virtual bool IsReachable(int currentMirrored, int prevMirrored)
		{
			var merged = currentMirrored & prevMirrored;
			var count = Row.Count[merged];

			return
				// if more then 5 count there are at least two connected cells.
				count > 5 ||
				// at least 2 open entries and 
				(count > 1 && Row.Row2BlocksConnected.Any(line => (line & merged) != 0));
		}
	}
}
