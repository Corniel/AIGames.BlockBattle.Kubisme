﻿using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme
{
	/// <summary>Gets the T block.</summary>
	/// <remarks>
	/// .X..
	/// XXX.
	/// ....
	/// ....
	/// </remarks>
	public class BlockT : Block
	{
		public override string Name { get { return "T"; } }

		public override byte[] Lines { get { return lines; } }
		private static byte[] lines = new byte[] { 2, 7 };

		public override int Width { get { return 3; } }
	}

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
	}

	/// <summary>Gets the T block rotated twice.</summary>
	/// <remarks>
	/// XXX.
	/// .X..
	/// ....
	/// ....
	/// </remarks>
	public class BlockTUturn : BlockT
	{
		public override RotationType Rotation { get { return RotationType.Uturn; } }

		public override byte[] Lines { get { return lines; } }
		private static byte[] lines = new byte[] { 7, 2 };

		public override int Width { get { return 3; } }

		public override IEnumerable<int> GetColumns(Field field) { return GetColumnsJLTUTurn(field); }
		public override BlockPath GetPath(Field field, int column) { return pathsJLTUturn[column]; }
	}

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
	}
}