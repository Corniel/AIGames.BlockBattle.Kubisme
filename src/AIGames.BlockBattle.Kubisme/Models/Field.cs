using AIGames.BlockBattle.Kubisme.Communication;
using System;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme
{
	public struct Field
	{
		public static readonly Field None = new Field(-1, 0, 0, new Row[0]);
		public static readonly Field Empty = Field.Create(0, 0, @"..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........");

		private readonly Row[] rows;
		public readonly short Points;
		public readonly byte Combo;
		public readonly byte FirstFilled;

		public Field(short pt, byte combo, byte freeRows, params Row[] rs)
		{
			rows = rs;
			Points = pt;
			Combo = combo;
			FirstFilled = freeRows;
		}

		public Field(short pt, byte combo, params Row[] rs) :
			this(pt, combo, GetFirstNoneEmptyRow(rs), rs) { }

		private static byte GetFirstNoneEmptyRow(Row[] rows)
		{
			for (byte r = 0; r < rows.Length; r++)
			{
				if (rows[r].row != Row.Empty) { return r; }
			}
			return (byte)rows.Length;
		}

		public int Count { get { return rows.Sum(r => Row.Count[r.row]); } }

		public int RowCount { get { return rows.Length; } }

		public Row this[int row] { get { return rows[row]; } }

		public enum TestResult
		{
			False = 0,
			True = 1,
			Retry = 2,
		}

		public TestResult Test(Block block, Position pos)
		{
			return Test(block, pos.Col, pos.Row);
		}

		public TestResult Test(Block block, int col, int row)
		{
			var lineMax = 3 - block.Bottom;
			// free space.
			if (lineMax + row < FirstFilled - 1) { return TestResult.Retry; }

			var lineMin = block.Top;

			var fl = lineMax + row + 1 - RowCount;
			// Not high enough.
			if (fl > 0)
			{
				return TestResult.False;
			}
			// The block does not fit. This can lead to false and a retry.
			var fit = row + lineMin >= 0;

			var hasFloor = fl == 0;

			for (var l = lineMax; l >= lineMin; l--)
			{
				// There is no fit, but also no overlap, so a drop can be tested.
				if (!fit && row + l < 0)
				{
					return TestResult.Retry;
				}

				var line = block[l, col];

				// if no floor detected yet.
				if (!hasFloor)
				{
					var floor = rows[row + l + 1].row;
					hasFloor = (floor & line) != 0;
				}

				var current = rows[row + l];
				var merged = current.row & line;

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
			byte free = (byte)(pos.Row + block.Top);
			short cleared = 0;
			var lineMax = 4 - block.Bottom;

			for (var line = block.Top; line < lineMax; line++)
			{
				var l = pos.Row + line;
				if (l >= 0 && l < RowCount)
				{
					rs[l] = rs[l].AddBlock(block[line], pos.Col);
				}
			}
			for (var r = RowCount - 1; r >= 0; r--)
			{
				if (rs[r].row == Row.Filled)
				{
					rs[r] = new Row(Row.Empty);
					cleared++;
				}
				else
				{
					if (r < cleared)
					{
						rs[r] = new Row(Row.Empty);
					}
					if (cleared > 0)
					{
						rs[r + cleared] = rs[r];
					}
				}
			}
			if (cleared == 4)
			{
				pt += 4;
			}
			if (cleared > 0)
			{
				free += (byte)cleared;
				pt += combo;
				pt += cleared;
				combo++;
			}
			else { combo = 0; }
			return new Field(pt, combo, free, rs);
		}

		/// <summary>Returns a field, with locked rows.</summary>
		public Field LockRows(int count)
		{
			if (count >= RowCount) { return None; }
			for(var i = 0; i < count; i++)
			{
				if (rows[i].row != Row.Empty)
				{
					// A lock will lead to death.
					return None;
				}
			}
			var rs = new Row[rows.Length - count];
			Array.Copy(rows, count, rs, 0, rs.Length);
			return new Field(Points, Combo, (byte)(FirstFilled - count), rs);
		}

		public override string ToString() { return String.Join("|", rows); }

		public static Field Create(GameState state, PlayerName name)
		{
			var rows = new Row[state[name].Field.GetLength(0)];
			
			for (var r = 0; r < rows.GetLength(0); r++)
			{
				var row = Row.Create(state, name, r);
				if (row.row == Row.Locked)
				{
					Array.Resize(ref rows, r);
					break;
				}
				rows[r] = row;
			}
			var field = new Field((short)state[name].RowPoints, (byte)state[name].Combo, rows);

			return field;
		}

		public static Field Create(int pt, int combo, string str)
		{
			var lines = str.Split(new String[] { "|", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			var rows = new Row[lines.Length];

			for (var r = 0; r < rows.Length; r++)
			{
				var row = Row.Create(lines[r].Trim());
				if (row.row == Row.Locked)
				{
					Array.Resize(ref rows, r);
					break;
				}
				rows[r] = row;
			}
			return new Field((short)pt, (byte)combo, rows);
		}
	}
}
