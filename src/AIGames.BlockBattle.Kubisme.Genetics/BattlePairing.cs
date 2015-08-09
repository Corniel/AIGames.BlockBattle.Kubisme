namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class BattlePairing
	{
		public BattlePairing(BotData b0, BotData b1)
		{
			Bot0 = b0;
			Bot1 = b1;
		}
		public BotData Bot0 { get; set; }
		public BotData Bot1 { get; set; }

		public BattleSimulation.Result Result { get; set; }

		public override string ToString()
		{
			return string.Format(
				"{0} - {1}{2}",
				Bot0.Id,
				Bot1.Id,
				Result == null ? "" : " : " + Result.Outcome.ToString());
		}
	}
}
