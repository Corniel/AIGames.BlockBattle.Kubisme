using AIGames.BlockBattle.Kubisme.Communication;
using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme
{
	/// <summary>Gets the J block.</summary>
	/// <remarks>
	/// X...
	/// XXX.
	/// ....
	/// ....
	/// </remarks>
	public class BlockJ : Block
	{
		public override string Name { get { return "J"; } }

		public override byte[] Lines { get { return lines; } }
		private static byte[] lines = new byte[] { 1, 7 };

		public override int Width { get { return 3; } }

		#region Rotation

		public override Block TurnLeft() { return this[RotationType.Left]; }
		public override Block TurnRight() { return this[RotationType.Right]; }

		public override Position TurnLeft(Position position) { return new Position(position.Col, position.Row); }
		public override Position TurnRight(Position position) { return new Position(position.Col + 1, position.Row); }

		#endregion
	}

	/// <summary>Gets the J block rotated left.</summary>
	/// <remarks>
	/// .X..
	/// .X..
	/// XX..
	/// ....
	/// </remarks>
	public class BlockJLeft : BlockJ
	{
		public override RotationType Rotation { get { return RotationType.Left; } }

		public override byte[] Lines { get { return lines; } }
		private static byte[] lines = new byte[] { 2, 2, 3 };

		public override int Width { get { return 2; } }

		public override IEnumerable<int> GetColumns(Field field) { return GetColumnsJLTLeft(field); }
		public override BlockPath GetPath(Field field, int column) { return pathsJLTLeft[column]; }

		#region Rotation

		public override Block TurnLeft() { return this[RotationType.Uturn]; }
		public override Block TurnRight() { return this[RotationType.None]; }

		public override Position TurnLeft(Position position) { return new Position(position.Col, position.Row + 1); }
		public override Position TurnRight(Position position) { return new Position(position.Col, position.Row); }

		#endregion
	}

	/// <summary>Gets the J block rotated twice.</summary>
	/// <remarks>
	/// XXX.
	/// ..X.
	/// ....
	/// ....
	/// </remarks>
	public class BlockJUturn : BlockJ
	{
		public override RotationType Rotation { get { return RotationType.Uturn; } }

		public override byte[] Lines { get { return lines; } }
		private static byte[] lines = new byte[] { 7, 4 };

		public override int Width { get { return 3; } }

		public override IEnumerable<int> GetColumns(Field field) { return GetColumnsJLTUTurn(field); }
		public override BlockPath GetPath(Field field, int column) { return pathsJLTUturn[column]; }

		#region Rotation

		public override Block TurnLeft() { return this[RotationType.Right]; }
		public override Block TurnRight() { return this[RotationType.Left]; }

		public override Position TurnLeft(Position position) { return new Position(position.Col + 1, position.Row - 1); }
		public override Position TurnRight(Position position) { return new Position(position.Col, position.Row - 1); }

		#endregion
	}

	/// <summary>Gets the J block rotated right.</summary>
	/// <remarks>
	/// XX.. 
	/// X...
	/// X... 
	/// .... 
	/// </remarks>
	public class BlockJRight : BlockJ
	{
		public override RotationType Rotation { get { return RotationType.Right; } }

		public override byte[] Lines { get { return lines; } }
		private static byte[] lines = new byte[] { 3, 1, 1 };

		public override int Width { get { return 2; } }

		public override IEnumerable<int> GetColumns(Field field) { return GetColumnsJLTRight(field); }
		public override BlockPath GetPath(Field field, int column) { return pathsJLTRight[column]; }

		#region Rotation

		public override Block TurnLeft() { return this[RotationType.None]; }
		public override Block TurnRight() { return this[RotationType.Uturn]; }

		public override Position TurnLeft(Position position) { return new Position(position.Col - 1, position.Row); }
		public override Position TurnRight(Position position) { return new Position(position.Col - 1, position.Row + 1); }

		#endregion
	}
}
