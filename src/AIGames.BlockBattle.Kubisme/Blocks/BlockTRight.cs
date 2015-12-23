using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme
{
	/// <summary>Gets the T block rotated right.</summary>
	/// <remarks>
	/// X...
	/// XX..
	/// X...
	/// ....
	/// </remarks>
	public class BlockTRight : BlockT
	{
		public override RotationType Rotation { get { return RotationType.Right; } }

		public override byte[] Lines { get { return lines; } }
		private static byte[] lines = new byte[] { 1, 3, 1 };

		public override int Width { get { return 2; } }

		public override IEnumerable<int> GetColumns(Field field) { return GetColumnsJLTRight(field); }
		public override BlockPath GetPath(Field field, int column) { return pathsJLTRight[column]; }

		#region Rotation

		public override Block TurnLeft() { return this[RotationType.None]; }
		public override Block TurnRight() { return this[RotationType.Uturn]; }

		public override Position TurnLeft(Position position) { return new Position(position.Col - 1, position.Row); }
		public override Position TurnRight(Position position) { return new Position(position.Col - 1, position.Row + 1); }

		#endregion

		/// <summary>Returns true if a T-spin was applied.</summary>
		public override bool IsTSpin(Position pos, ushort[] rows)
		{
			var col = pos.Col - 1;
			return
				col >= 0 &&
				(TSpinTopMask[col] & rows[pos.Row]) != 0 &&
				// The tail of the T should be a perfect fit.
				(TSpinTopMask[col] & rows[pos.Row + 2]) == TSpinTopMask[col];
		}
	}
}
