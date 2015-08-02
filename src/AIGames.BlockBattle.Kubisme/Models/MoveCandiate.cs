namespace AIGames.BlockBattle.Kubisme
{
	public struct MoveCandiate
	{
		public static readonly MoveCandiate None = new MoveCandiate(BlockPath.None, Field.None);

		public readonly BlockPath Path;
		public readonly Field Field;

		public MoveCandiate(BlockPath path, Field field)
		{
			Path = path;
			Field = field;
		}

		public override string ToString()
		{
			return Path.ToString();
		}
	}
}
