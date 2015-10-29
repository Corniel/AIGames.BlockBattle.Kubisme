using AIGames.BlockBattle.Kubisme.Communication;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme
{
	public struct Field : IEquatable<Field>
	{
		public const short SingleLineClear = 0;
		public const short DoubleLineClear = 3;
		public const short TripleLineClear = 6;
		public const short QuadrupleLineClear = 10;
		public const short SingleTSpin = 5;
		public const short DoubleTSpin = 10;
		public const short PerfectClear = 18;

		public static readonly Field None = new Field(-1, 0, 0, 0, new ushort[0]);
		public static readonly Field Empty = new Field(0, 0, 0, 0, new ushort[20]);

		private readonly ushort[] rows;
		public readonly short Points;
		public readonly byte Combo;
		public readonly byte Skips;
		public readonly byte FirstFilled;

		public Field(short pt, byte combo, byte skips, byte freeRows, params ushort[] rs)
		{
			Points = pt;
			Combo = combo;
			Skips = skips;
			FirstFilled = freeRows;
			rows = rs;
		}

		public Field(short pt, byte combo, byte skips, params ushort[] rs) :
			this(pt, combo, skips, GetFirstNoneEmptyRow(rs), rs) { }

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

		/// <summary>Returns true if the field is a none field.</summary>
		public bool IsNone { get { return Points == None.Points; } }

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
			byte skips = Skips;
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
				else if (cleared > 0)
				{
					if (block == Block.T[Block.RotationType.Uturn] &&
						// Not on the top row.
						pos.Row > 0 &&
						// At least one blockade
						(BlockTUturn.TSpinTopMask[pos.Col] & rows[pos.Row - 1]) != 0 &&
						// The head of the T should be a perfect fit.
						(BlockTUturn.TSpinTopBorderMask[pos.Col] & rows[pos.Row]) == BlockTUturn.TSpinTopBorderMask[pos.Col] &&
						// The tail of the T should be a perfect fit.
						(BlockTUturn.TSpinTopMask[pos.Col] & rows[pos.Row + 1]) == BlockTUturn.TSpinTopMask[pos.Col])
					{
						if (cleared == 1)
						{
							pt += SingleTSpin;
						}
						else
						{
							pt += DoubleTSpin;
							skips++;
						}
					}
					else
					{
						switch (cleared)
						{
							case 1: pt += SingleLineClear; break;
							case 2: pt += DoubleLineClear; break;
							case 3: pt += TripleLineClear; break;
							case 4: pt += QuadrupleLineClear; skips++; break;
						}
					}
				}
				// only if points are earned, combo is added and increased.
				if (pt > Points)
				{
					pt += combo++;
				}
			}
			return new Field(pt, combo, skips, free, rs);
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

		/// <summary>Skips a block.</summary>
		public Field SkipBlock()
		{
			var rs = new ushort[rows.Length];
			Array.Copy(rows, 0, rs, 0, rs.Length);
			return new Field(Points, 0, (byte)(Skips - 1), rs);
		}

		public override string ToString() { return String.Join("|", Rows); }

		#region IEquatable

		/// <summary>Gets the hash.</summary>
		/// <remarks>
		/// No pre-computation, as it slows down the search speed 10 times.
		/// </remarks>
		public override int GetHashCode()
		{
			var h = Points & 3;
			h |= Combo << 2;
			for (var i = rows.Length - 1; i >= 0; i--)
			{
				h ^= rows[i] << i;
			}
			return h;
		}

		public override bool Equals(object obj)
		{
			return Equals((Field)obj);
		}

		public bool Equals(Field other)
		{
			if (
				this.Points == other.Points &&
				this.Combo == other.Combo &&
				this.FirstFilled == other.FirstFilled)
			{
				for (var i = rows.Length - 1; i >= 0; i--)
				{
					if (this.rows[i] != other.rows[i]) { return false; }
				}
				return true;
			}
			return false;
		}

		#endregion

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
			var field = new Field((short)state[name].Points, (byte)state[name].Combo, (byte)state[name].Skips, rows);

			return field;
		}

		public static Field Create(int pt, int combo, int skips, string str)
		{
			var lines = str.Split(new String[] { "|", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			var rows = new ushort[lines.Length];

			for (var r = 0; r < rows.Length; r++)
			{
				var row = Row.Create(lines[r].Trim());
				rows[r] = row;
			}
			return new Field((short)pt, (byte)combo,(byte)skips, rows);
		}
	}
}
