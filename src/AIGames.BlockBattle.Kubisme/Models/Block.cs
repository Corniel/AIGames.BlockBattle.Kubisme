using AIGames.BlockBattle.Kubisme.Communication;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AIGames.BlockBattle.Kubisme.Models
{
	public class Block
	{
		/// <summary>Gets the O block.</summary>
		/// <remarks>
		/// XX..
		/// XX..
		/// ....
		/// ....
		/// </remarks>
		public static readonly Block O = new Block(
			2, 2,
			0, 2,
			0, 2,
			3, 3, 0, 0,
			RotationType.None);

		/// <summary>Gets the I block.</summary>
		/// <remarks>
		/// ....  .X...
		/// XXXX  .X...
		/// ....  .X...
		/// ....  .X...
		/// </remarks>
		public static readonly Block I = new Block(
			1, 4,
			0, 0,
			1, 2,
			0, 15, 0, 0,
			RotationType.None,
			new Block(
				4, 1,
				1, 2,
				0, 0,
				2, 2, 2, 2,
				RotationType.Right));

		/// <summary>Gets the S block.</summary>
		/// <remarks>
		/// .XX.  X...
		/// XX..  XX..
		/// ....  .X..
		/// ....  ....
		/// </remarks>
		public static readonly Block S = new Block(
			2, 3,
			0, 1,
			0, 2,
			6, 3, 0, 0,
			RotationType.None,
			new Block(
				3, 2,
				0, 2,
				0, 1,
				1, 3, 2, 0,
				RotationType.Right));

		/// <summary>Gets the Z block.</summary>
		/// <remarks>
		/// XX..  .X..
		/// .XX.  XX..
		/// ....  X...
		/// ....  ....
		/// </remarks>
		public static readonly Block Z = new Block(
			2, 3,
			0, 1,
			0, 2,
			3, 6, 0, 0,
			RotationType.None,
			new Block(
				3, 2,
				0, 2,
				0, 1,
				2, 3, 1, 0,
				RotationType.Right));

		/// <summary>Gets the J block.</summary>
		/// <remarks>
		///  X...  .X..  .... .XX. 
		///  XXX.  .X..  XXX. .X.. 
		///  ....  XX..  ..X. .X.. 
		///  ....  ....  .... .... 
		/// </remarks>
		public static readonly Block J = new Block(
			2, 3,
			0, 1,
			0, 2,
			1, 7, 0, 0,
			RotationType.None,
			new Block(
				3, 2,
				0, 2,
				0, 1,
				2, 2, 3, 0,
				RotationType.Right),
			new Block(
				2, 3,
				0, 1,
				1, 1,
				0, 7, 4, 0,
				RotationType.Uturn),
			new Block(
				3, 2,
				1, 1,
				0, 1,
				6, 2, 2, 0,
				RotationType.Left));

		/// <summary>Gets the L block.</summary>
		/// <remarks>
		///  ..X.  .X..  .... XX.. 
		///  XXX.  .X..  XXX. .X.. 
		///  ....  .XX.  X... .X.. 
		///  ....  ....  .... .... 
		/// </remarks>
		public static readonly Block L = new Block(
			3, 3,
			0, 1,
			0, 2,
			4, 7, 0, 0,
			RotationType.None,
			new Block(
				3, 2,
				1, 1,
				0, 1,
				2, 2, 6, 0,
				RotationType.Right),
			new Block(
				2, 3,
				0, 1,
				1, 1,
				0, 7, 1, 0,
				RotationType.Uturn),
			new Block(
				3, 2,
				0, 2,
				0, 1,
				3, 2, 2, 0,
				RotationType.Left));

		/// <summary>Gets the T block.</summary>
		/// <remarks>
		/// .X..  .X..  ....  .X..
		/// XXX.  XX..	XXX.  .XX.
		/// ....  .X..	.X..  .X..
		/// ....  ....	....  ....
		/// </remarks>
		public static readonly Block T = new Block(
			2, 3,
			0, 1,
			0, 2,
			2, 7, 0, 0,
			RotationType.None,
			new Block(
				3, 2,
				0, 2,
				0, 1,
				2, 3, 2, 0,
				RotationType.Right),
			new Block(
				2, 3,
				0, 1,
				1, 1,
				0, 7, 2, 0,
				RotationType.Uturn),
			new Block(
				2, 3,
				1, 1,
				0, 1,
				2, 6, 2, 0,
				RotationType.Left));

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

		private Block(
			byte height, byte width,
			sbyte left, sbyte right,
			sbyte top, sbyte bottom,
			byte l0, byte l1, byte l2, byte l3,
			RotationType r, params Block[] variations)
		{
			Height = height;
			Width = width;

			Left = left;
			Right = right;

			Top = top;
			Bottom = bottom;

			Line0 = l0;
			Line1 = l1;
			Line2 = l2;
			Line3 = l3;

			Rotation = r;

			var list = variations.ToList();
			list.Insert(0, this);
			var copy = list.ToArray();
			foreach (var block in list)
			{
				block.Variations = copy;
			}
		}

		public enum RotationType
		{
			None = 0,
			Right = 1,
			Uturn = 2,
			Left = 3,
		}

		/// <summary>Gets a line based on the index.</summary>
		public byte this[int line]
		{
			get
			{
				switch (line)
				{
					case 0: return Line0;
					case 1: return Line1;
					case 2: return Line2;
					case 3: return Line3;
					default: return 0;
				}
			}
		}
		private readonly byte Line0;
		private readonly byte Line1;
		private readonly byte Line2;
		private readonly byte Line3;

		public readonly byte Height;
		public readonly byte Width;

		public readonly sbyte Left;
		public readonly sbyte Right;
		public readonly sbyte Top;
		public readonly sbyte Bottom;

		public int Count
		{
			get
			{
				return
					Bits.Count(Line0) +
					Bits.Count(Line1) +
					Bits.Count(Line2) +
					Bits.Count(Line3);
			}
		}

		public readonly RotationType Rotation;
		public Block[] Variations { get; private set; }

		public Block this[RotationType rotation]
		{
			get { return Variations[(int)rotation]; }
		}
		
		public override string ToString()
		{
			return DebuggerDisplay;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string DebuggerDisplay
		{
			get
			{
				var sb = new StringBuilder();
				for (var i = 0; i < 4; i++)
				{
					if (i > 0) { sb.Append('|'); }
					var row = this[i];
					switch (row)
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
		}
	}
}
