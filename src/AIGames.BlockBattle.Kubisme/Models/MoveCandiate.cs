namespace AIGames.BlockBattle.Kubisme.Models
{
	public struct MoveCandiate
	{
		public readonly MovePath Path;
		public readonly Field Field;

		public MoveCandiate(MovePath path, Field field)
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
