﻿using AIGames.BlockBattle.Kubisme.Models;

namespace AIGames.BlockBattle.Kubisme.Evaluation
{
	public interface IEvaluator
	{
		int GetScore(Field field);
		IParameters Parameters { get; set; }
		int WinScore { get; }
		int DrawScore { get; }
		int LostScore { get; }
	}
}
