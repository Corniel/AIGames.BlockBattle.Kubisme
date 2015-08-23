using AIGames.BlockBattle.Kubisme.Communication;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme
{
	public struct Field
	{
		public const short SingleLineClear = 1;
		public const short DoubleLineClear = 3;
		public const short TripleLineClear = 6;
		public const short QuadrupleLineClear = 12;
		public const short PerfectClear = 24;

		public static readonly Field None = new Field(-1, 0, 0, new ushort[0]);
		public static readonly Field Empty = Field.Create(0, 0, @"..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........");

		private readonly ushort[] rows;
		public readonly short Points;
		public readonly byte Combo;
		public readonly byte FirstFilled;

		public Field(short pt, byte combo, byte freeRows, params ushort[] rs)
		{
			rows = rs;
			Points = pt;
			Combo = combo;
			FirstFilled = freeRows;
		}

		public Field(short pt, byte combo, params ushort[] rs) :
			this(pt, combo, GetFirstNoneEmptyRow(rs), rs) { }

		private static byte GetFirstNoneEmptyRow(ushort[] rows)
		{
			for (byte r = 0; r < rows.Length; r++)
			{
				if (rows[r] != Row.Empty) { return r; }
			}
			return (byte)rows.Length;
		}

		public int Count { get { return rows.Sum(r => Row.Count[r]); } }

		public int RowCount { get { return rows.Length; } }

		[ExcludeFromCodeCoverage]
		public ushort this[int row] { get { return rows[row]; } }

		public enum TestResult
		{
			False = 0,
			True = 1,
			Retry = 2,
		}

		[ExcludeFromCodeCoverage]
		public TestResult Test(Block block, Position pos)
		{
			return Test(block, pos.Col, pos.Row);
		}

		/// <summary>Tests if the block fits.</summary>
		/// <returns>
		/// Returns false if there is overlap.
		/// Else, returns true if solid ground, or retry if not.
		/// </returns>
		public TestResult Test(Block block, int col, int row)
		{
			var rowMax = block.Lines.Length - 1 + row;
			
			// Not fit, block lower than bottom.
			if (rowMax >= RowCount) { return TestResult.False; }

			var hasFloor = rowMax == RowCount - 1;

			var rowMin = row < 0 ? 0 : row;

			// From high to low, because of change on shortcuts.
			for (var r = rowMax; r >= rowMin; r--)
			{
				var l = r - row;
				var line = block[l, col];

				// if no floor detected yet.
				if (!hasFloor)
				{
					var floor = rows[r + 1];
					// overlap with the row below.
					hasFloor = (floor & line) != 0;
				}
				
				var current = rows[r];
				var merged = current & line;

				// overlap.
				if (merged != 0)
				{
					return TestResult.False;
				}
			}
			return hasFloor && row >= 0 ? TestResult.True : TestResult.Retry;
		}
		
		public Field Apply(Block block, Position pos)
		{
			var rs = new ushort[rows.Length];
			Array.Copy(rows, rs, RowCount);

			short pt = Points;
			byte combo = Combo;
			// the current position, and if the block is higher, pick that one.
			byte free = (byte)Math.Min(FirstFilled, pos.Row);
			short cleared = 0;

			for (var line = 0; line < block.Lines.Length; line++)
			{
				var l = pos.Row + line;
				// This should be checked by test methods before.
				// if (l >= 0 && l < RowCount)
				rs[l] = (ushort)(rs[l] | block[line, pos.Col]);
			}
			for (var r = RowCount - 1; r >= 0; r--)
			{
				if (rs[r] == Row.Filled)
				{
					rs[r] = Row.Empty;
					cleared++;
				}
				else
				{
					if (cleared > 0)
					{
						rs[r + cleared] = rs[r];
					}
					if (r < cleared)
					{
						rs[r] = Row.Empty;
					}
				}
			}
			if (cleared == 0) { combo = 0; }
			else
			{
				free += (byte)cleared;

				// perfect clear 
				if (free == RowCount)
				{
					pt += PerfectClear;
				}
				else
				{
					switch (cleared)
					{
						case 1: pt += SingleLineClear; break;
						case 2: pt += DoubleLineClear; break;
						case 3: pt += TripleLineClear; break;
						case 4: pt += QuadrupleLineClear; break;
					}
				}
				pt += combo++;
			}
			return new Field(pt, combo, free, rs);
		}

		/// <summary>Returns a field, with garbage rows.</summary>
		public Field Garbage(params ushort[] garbage)
		{
			if (garbage.Length > FirstFilled) { return None; }

			var count = garbage.Length;

			var rs = new ushort[RowCount];
			var copyCount = RowCount - count;
			Array.Copy(garbage, 0, rs, copyCount, count);
			Array.Copy(rows, count, rs, 0, copyCount);
			
			var free = FirstFilled - count;
			return new Field(Points, Combo, (byte)free, rs);
		}

		/// <summary>Returns a field, with a locked row.</summary>
		public Field LockRow()
		{
			if (FirstFilled == 0) { return None; }
			var rs = new ushort[rows.Length - 1];
			Array.Copy(rows, 1, rs, 0, rs.Length);
			var free = FirstFilled - 1;
			return new Field(Points, Combo, (byte)free, rs);
		}

		public override string ToString() { return String.Join("|", Rows); }

		public string[] Rows
		{
			get
			{
				return rows.Select(r => Row.ToString(r)).ToArray();
			}
		}

		public static Field Create(GameState state, PlayerName name)
		{
			var rows = new ushort[state[name].Field.GetLength(0)];
			
			for (var r = 0; r < rows.GetLength(0); r++)
			{
				var row = Row.Create(state, name, r);
				if (row == Row.Locked)
				{
					Array.Resize(ref rows, r);
					break;
				}
				rows[r] = row;
			}
			var field = new Field((short)state[name].Points, (byte)state[name].Combo, rows);

			return field;
		}

		public static Field Create(int pt, int combo, string str)
		{
			var lines = str.Split(new String[] { "|", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			var rows = new ushort[lines.Length];

			for (var r = 0; r < rows.Length; r++)
			{
				var row = Row.Create(lines[r].Trim());
				rows[r] = row;
			}
			return new Field((short)pt, (byte)combo, rows);
		}
	}
}
