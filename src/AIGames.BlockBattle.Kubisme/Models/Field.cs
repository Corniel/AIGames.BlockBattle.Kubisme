﻿using AIGames.BlockBattle.Kubisme.Communication;
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
					if (rows[r].row != Row.Empty) { return r; }
				}
				return rows.Length;
			}
		}

		public int Count { get { return rows.Sum(r => Row.Count[r.row]); } }

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
			var lineMax = 4 - block.Bottom;

			for (var line = 0; line < lineMax; line++)
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

		/// <summary>Returns a field, with locked rows.</summary>
		public Field LockRows(int count)
		{
			for(var i = 0; i < count; i++)
			{
				if (rows[i].row != Row.Empty)
				{
					// A lock will lead to death.
					return new Field(Points, 0, new Row[0]);
				}
			}
			var rs = new Row[rows.Length - count];
			Array.Copy(rows, count, rs, 0, rs.Length);
			return new Field(Points, Combo, rs);
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