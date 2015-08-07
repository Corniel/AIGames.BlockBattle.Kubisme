using AIGames.BlockBattle.Kubisme.Communication;
using System;
using System.Linq;
using Troschuetz.Random.Generators;

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

		public ushort this[int row] { get { return rows[row]; } }

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
					var floor = rows[row + l + 1];
					hasFloor = (floor & line) != 0;
				}

				var current = rows[row + l];
				var merged = current & line;

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
			var rs = new ushort[rows.Length];
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
					rs[l] = Row.AddBlock(rs[l], block[line], pos.Col);
				}
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
					if (r < cleared)
					{
						rs[r] = Row.Empty;
					}
					if (cleared > 0)
					{
						rs[r + cleared] = rs[r];
					}
				}
			}
			if (cleared > 0)
			{
				free += (byte)cleared;

				switch (cleared)
				{
					case 1: pt += SingleLineClear; break;
					case 2: pt += DoubleLineClear; break;
					case 3: pt += TripleLineClear; break;
					case 4: pt += QuadrupleLineClear; break;
				}
				// perfect clear 
				if (free == RowCount)
				{
					pt += PerfectClear;
				}
				pt += combo++;
			}
			else { combo = 0; }
			return new Field(pt, combo, free, rs);
		}

		public Field Garbage(int count, MT19937Generator rnd)
		{
			var garbage = new ushort[count];

			var prev = -1;
			for(var i = 0;i< count;i++)
			{
				var index = rnd.Next(i == 0 ? 10 : 9);
				if(index == prev){index++;}
				garbage[i] = Row.Garbage[index];
				prev = index;
			}
			return Garbage(garbage);
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

		public override string ToString() { return String.Join("|", rows.Select(r=> Row.ToString(r))); }

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
			var field = new Field((short)state[name].RowPoints, (byte)state[name].Combo, rows);

			return field;
		}

		public static Field Create(int pt, int combo, string str)
		{
			var lines = str.Split(new String[] { "|", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			var rows = new ushort[lines.Length];

			for (var r = 0; r < rows.Length; r++)
			{
				var row = Row.Create(lines[r].Trim());
				if (row == Row.Locked)
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
