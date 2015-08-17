using AIGames.BlockBattle.Kubisme.Communication;
using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme
{
	/// <summary>Gets the L block.</summary>
	/// <remarks>
	///  ..X. 
	///  XXX. 
	///  .... 
	///  .... 
	/// </remarks>
	public class BlockL : Block
	{
		public override string Name { get { return "L"; } }

		public override byte[] Lines { get { return lines; } }
		private static byte[] lines = new byte[] { 4, 7 };

		public override int Width { get { return 3; } }
	}

	/// <summary>Gets the L block rotated left.</summary>
	/// <remarks>
	/// XX..
	/// .X..
	/// .X..
	/// ....
	/// </remarks>
	public class BlockLLeft : BlockL
	{
		public override RotationType Rotation { get { return RotationType.Left; } }

		public override byte[] Lines { get { return lines; } }
		private static byte[] lines = new byte[] { 3, 2, 2 };
		public override int Width { get { return 2; } }

		public override IEnumerable<int> GetColumns(Field field) { return GetColumnsJLTLeft(field); }
		public override BlockPath GetPath(Field field, int column) { return pathsJLTLeft[column]; }
	}

	/// <summary>Gets the L block rotated twice.</summary>
	/// <remarks>
	/// XXX.
	/// X...
	/// ....
	/// ....
	/// </remarks>
	public class BlockLUturn : BlockL
	{
		public override RotationType Rotation { get { return RotationType.Uturn; } }

		public override byte[] Lines { get { return lines; } }
		private static byte[] lines = new byte[] { 7, 1 };

		public override IEnumerable<int> GetColumns(Field field) { return GetColumnsJLTUTurn(field); }
		public override BlockPath GetPath(Field field, int column) { return pathsJLTUturn[column]; }
	}

	/// <summary>Gets the L block rotated right.</summary>
	/// <remarks>
	/// .X.. 
	/// .X.. 
	/// .XX. 
	/// .... 
	/// </remarks>
	public class BlockLRight : BlockL
	{
		public override RotationType Rotation { get { return RotationType.Right; } }

		public override byte[] Lines { get { return lines; } }
		private static byte[] lines = new byte[] { 1, 1, 3 };

		public override int Width { get { return 2; } }

		public override IEnumerable<int> GetColumns(Field field) { return GetColumnsJLTRight(field); }
		public override BlockPath GetPath(Field field, int column) { return pathsJLTRight[column]; }
	}
}
