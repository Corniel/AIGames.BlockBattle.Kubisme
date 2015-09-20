using AIGames.BlockBattle.Kubisme.Communication;
using NUnit.Framework;
using System;
using System.IO;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Communication
{
	public class ConsolePlatformTester : ConsolePlatform
	{
		public ConsolePlatformTester(string path)
			: base(LoadInput(path), Console.Out, new StringWriter()) {}

		public override void DoRun(IBot bot)
		{
			base.DoRun(bot);

			var log = Logger.ToString();
			if (!String.IsNullOrEmpty(log))
			{
				Console.Error.Write(log);
				Assert.Fail(log);
			}

		}
		
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