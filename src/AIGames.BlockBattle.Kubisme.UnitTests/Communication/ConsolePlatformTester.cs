using AIGames.BlockBattle.Kubisme.Communication;
using System;
using System.IO;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Communication
{
	public class ConsolePlatformTester : ConsolePlatform
	{
		public ConsolePlatformTester(string path)
			: base(LoadInput(path), Console.Out) { }

		private static TextReader LoadInput(string path)
		{
			return
				new StreamReader(
					typeof(ConsolePlatformTester)
					.Assembly
					.GetManifestResourceStream("AIGames.BlockBattle.Kubisme.UnitTests.Communication." + path));
		}
	}
}