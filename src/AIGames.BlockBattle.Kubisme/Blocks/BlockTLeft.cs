using System;
using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme
{
	/// <summary>Gets the T block rotated left.</summary>
	/// <remarks>
	/// .X..
	/// XX..
	/// .X..
	/// ....
	/// </remarks>
	public class BlockTLeft : BlockT
	{
		public override RotationType Rotation { get { return RotationType.Left; } }

		public override byte[] Lines { get { return lines; } }
		private static byte[] lines = new byte[] { 2, 3, 2 };

		public override int Width { get { return 2; } }

		public override IEnumerable<int> GetColumns(Field field) { return GetColumnsJLTLeft(field); }
		public override BlockPath GetPath(Field field, int column) { return pathsJLTLeft[column]; }

		#region Rotation

		public override Block TurnLeft() { return this[RotationType.Uturn]; }
		public override Block TurnRight() { return this[RotationType.None]; }

		public override Position TurnLeft(Position position) { return new Position(position.Col, position.Row + 1); }
		public override Position TurnRight(Position position) { return new Position(position.Col, position.Row); }

		#endregion

		/// <summary>Returns true if a T-spin was applied.</summary>
		public override bool IsTSpin(Position pos, ushort[] rows)
		{
			return
				pos.Col < 8 &&
				// There were 3 concatenated holes at the expected place.
				(~rows[pos.Row + 1] & TSpinRow1Mask[pos.Col]) == TSpinRow1Mask[pos.Col] &&
				(TSpinTopMask[pos.Col] & rows[pos.Row]) != 0 &&
				// The tail of the T should be a perfect fit.
				(TSpinTopMask[pos.Col] & rows[pos.Row + 2]) == TSpinTopMask[pos.Col];
		}
	}
}
