using System;
using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme.Communication
{
	public class Settings
	{
		public  PlayerName YourBot { get; set; }


		public bool Apply(IInstruction instruction)
		{
			if (Mapping.ContainsKey(instruction.GetType()))
			{
				Mapping[instruction.GetType()].Invoke(instruction, this);
				return true;
			}
			return false;
		}

		private static Dictionary<Type, Action<IInstruction, Settings>> Mapping = new Dictionary<Type, Action<IInstruction, Settings>>()
		{
			{ typeof(YourBotInstruction), (instruction, settings) =>{ settings.YourBot = ((YourBotInstruction)instruction).Name; }},

		};
	}
}
