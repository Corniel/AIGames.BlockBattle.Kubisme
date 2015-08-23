using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AIGames.BlockBattle.Kubisme
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public struct TempPath
	{
		public TempPath(Block block, Position position, BlockPath path)
		{
			Block = block;
			Position = position;
			Path = path;
		}

		public readonly Block Block;
		public readonly Position Position;
		public readonly BlockPath Path;

		public TempPath TurnLeft()
		{
			return new TempPath(Block.TurnLeft(), Block.TurnLeft(Position), Path.AddTurnLeft());
		}
		public TempPath TurnRight()
		{
			return new TempPath(Block.TurnRight(), Block.TurnRight(Position), Path.AddTurnRight());
		}

		public TempPath Left()
		{
			return new TempPath(Block, Position.Left, Path.AddLeft());
		}
		public TempPath Right()
		{
			return new TempPath(Block, Position.Right, Path.AddRight());
		}
		
		public TempPath Down()
		{
			return new TempPath(Block, Position.Down, Path.AddDown());
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never), ExcludeFromCodeCoverage]
		private string DebuggerDisplay
		{
			get { return string.Format("{0}, ({1}), Path: {2}", Block.DebuggerDisplay, Position, Path); }
		}
	}
}
