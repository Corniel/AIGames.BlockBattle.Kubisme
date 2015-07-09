using AIGames.BlockBattle.Kubisme.Communication;
using System;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.Models
{
	public struct Field
	{
		public static readonly Field Empty = Field.Create(0, 0, @"..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........");

		private readonly Row[] rows;
		public readonly short Points;
		public readonly byte Combo;

		public Field(short pt, byte combo, params Row[] rs)
		{
			rows = rs;
			Points = pt;
			Combo = combo;
		}

		public int FirstNoneEmptyRow
		{
			get
			{
				for (var r = 0; r < rows.Length; r++)
				{
					if (rows[r] != Row.Empty) { return r; }
				}
				return rows.Length;
			}
		}

		public int Count { get { return rows.Sum(r => r.Count); } }

		public int RowCount { get { return rows.Length; } }

		public Row this[int row] { get { return rows[row]; } }

		internal enum TestResult
		{
			False = 0,
			True = 1,
			Retry = 2,
		}
		internal TestResult Test(Block block, int col, int row)
		{
			var lineMin = block.Top;
			// The block does not fit.
			if (row + lineMin < 0) { return TestResult.False; }

			var lineMax = 3 - block.Bottom;

			var hasFloor = lineMax + row + 1 == RowCount;
					
			for (var l  = lineMax; l >=  lineMin; l--)
			{
				var line = block[l];
				var shifted = col > 0 ?  (line << col) : (line >> (-col));

				// if no floor detected yet.
				if (!hasFloor)
				{
					var floor = rows[row + l + 1].row;
					hasFloor = (floor & shifted) != 0;
				}

				var current = rows[row + l];
				var merged = current.row & shifted;
				
				// overlap.
				if (merged != 0) 
				{ 
					return TestResult.False; 
				}
			}
			return hasFloor ? TestResult.True : TestResult.Retry;
		}

		public Field Apply(Block block, Position pos)
		{
			var rs = new Row[rows.Length];
			Array.Copy(rows, rs, RowCount);

			short pt = Points;
			byte combo = Combo;
			short cleared = 0;

			for (var line = 0; line < 4; line++)
			{
				var l = pos.Row + line;
				if (l >= 0 && l < RowCount)
				{
					rs[l] = rs[l].AddBlock(block[line], pos.Col);
				}
			}
			for (var r = RowCount - 1; r >= 0; r--)
			{
				if (rs[r] == Row.Filled)
				{
					rs[r] = Row.Empty;
					cleared++;
				}
				else if (r < cleared)
				{
					rs[r] = Row.Empty;
				}
				else if (cleared > 0)
				{
					rs[r + cleared] = rs[r];
				}
			}
			if (cleared == 4)
			{
				cleared = 8;
			}
			if (cleared > 0)
			{
				pt += combo;
				pt += cleared;
				combo++;
			}
			else { combo = 0; }
			return new Field(pt, combo, rs);
		}

		public Field Remove(Block block, Position pos)
		{
			var rs = new Row[rows.Length];
			Array.Copy(rows, rs, RowCount);

			for (var i = 0; i < 4; i++)
			{
				var l = pos.Row + i;
				if (l >= 0)
				{
					var line = block[i];
					rs[l] = rs[l].RemoveBlock(line, pos.Col);
				}
			}
			return new Field(Points, Combo, rs);
		}


		public override string ToString() { return String.Join("|", rows); }

		public static Field Create(GameState state, PlayerName name)
		{
			var rows = new Row[state[name].Field.GetLength(0)];
			
			// Ignore the first row, as there is put in the current block.
			for (var r = 1; r < rows.GetLength(0); r++)
			{
				rows[r] = Row.Create(state, name, r);
			}
			var field = new Field((short)state[name].RowPoints, (byte)state[name].Combo, rows);

			return field;
		}

		public static Field Create(int pt, int combo, string str)
		{
			var lines = str.Split(new String[] { "|", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			var rows = new Row[lines.Length];

			for (var i = 0; i < rows.Length; i++)
			{
				var line = lines[i];
				rows[i] = Row.Create(line.Trim());
			}
			return new Field((short)pt, (byte)combo, rows);
		}
	}
}
