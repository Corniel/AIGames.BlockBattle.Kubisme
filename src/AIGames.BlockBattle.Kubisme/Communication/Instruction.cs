using System;
using System.Collections.Generic;
using System.IO;

namespace AIGames.BlockBattle.Kubisme.Communication
{
	public static class Instruction
	{
		public static IInstruction Parse(string line)
		{
			var splited = line.Split(' ');

			switch (splited[0])
			{
				case "action": return RequestMoveInstruction.Parse(splited);
				case "settings": return SettingsInstruction.Parse(splited);
				case "update": return UpdateInstruction.Parse(splited);
			}
			return null;
		}

		/// <summary>Reads the instructions from the reader.</summary>
		public static IEnumerable<IInstruction> Read(TextReader reader)
		{
			if (reader == null) { throw new ArgumentNullException("reader"); }

			string line;
			while ((line = reader.ReadLine()) != null)
			{
				var instruction = Parse(line);
				if (instruction != null)
				{
					yield return instruction;
				}
			}
		}
	}
}
