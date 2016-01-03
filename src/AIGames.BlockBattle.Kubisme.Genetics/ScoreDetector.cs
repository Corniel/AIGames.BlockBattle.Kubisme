namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public static class ScoreDetector
	{
		public static BlockBattleScore GetScore(Field before, Field after, Block block)
		{
			var points = after.Points - before.Points;
			if (points == 0)
			{
				// Skip.
				if (after.Skips < before.Skips)
				{
					return BlockBattleScore.None;
				}
				// Combo was not reset, so single clear.
				if (after.Combo > 0)
				{
					return BlockBattleScore.SingleLineClear;
				}
				var delta = after.Count - before.Count;
				
				switch (delta)
				{
					// Just added 1 block.
					case 4:
						return BlockBattleScore.None; 
					// Cleared more blocks than added by garbage.
					case -6:
					// -6 + (8)
					case 2:
					// -6 + (9)
					case 3:
					// -6 + (8 + 9)
					case 11:
					// -6 + (8 + 9 + 8)
					case 19:
					// -6 + (9 + 8 + 9)
					case 20:
						// -6 + (9 + 8 + 9 + 8)
					case 28:
						return BlockBattleScore.SingleLineClear;
				}
				return BlockBattleScore.None; 
			}

			// correct for gained combo;
			points -= (after.Combo - 1);

			switch (points)
			{
				case Field.PerfectClear: return BlockBattleScore.PerfectClear;
				case Field.SingleTSpin: return BlockBattleScore.SingleTSpin;
				case Field.DoubleLineClear: return BlockBattleScore.DoubleLineClear;
				case Field.TripleLineClear: return BlockBattleScore.TripleLineClear;
				
				case 10:
					return block == Block.T ?
						BlockBattleScore.DoubleTSpin :
						BlockBattleScore.QuadrupleLineClear;

				default:
					return BlockBattleScore.None;
			}

		}
	}
}
