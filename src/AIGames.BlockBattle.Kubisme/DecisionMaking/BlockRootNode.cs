using System;
using System.Collections.Generic;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme
{
	public class BlockRootNode : BlockNode<Block1Node>
	{
		public BlockRootNode(Field field) : base(field, 0) { }

		public override byte Depth { get { return 0; } }

		public BlockPath BestMove { get { return Children == null || Children.Count == 0 ? BlockPath.None : Children[0].Path; } }
		public Field BestField { get { return Children == null || Children.Count == 0 ? Field.None : Children[0].Field; } }

		/// <summary>Applies the search on the current node.</summary>
		/// <param name="depth">
		/// The maximum depth to search.
		/// </param>
		/// <param name="pars">
		/// The parameters needed to apply the search.
		/// </param>
		public override void Apply(byte depth, ApplyParameters pars)
		{
			if (depth > Depth && depth <= pars.MaximumDepth && pars.HasTimeLeft)
			{
				if (Children == null)
				{
					BranchingFactor = pars.Current.BranchingFactor0;
					var block = GetBlock(pars);
					Children = new BlockNodes<Block1Node>(block);
				
					foreach (var candidate in pars.Generator.GetMoves(Field, block, true))
					{
						Block1Node child = Create(candidate.Field, candidate.Path, pars);

						// We can kill our opponent.
						if (child.Field .Points> Field.Points)
						{
							var garbageNew = child.Field.Points / 3;
							if (garbageNew - pars.Garbage > pars.Opponent.FirstFilled1)
							{
								child.SetFinalScore(Scores.Wins(1));
							}
						}
						Children.InsertSorted(child);
					}
				}
				else
				{
					Children.Apply(depth, pars, BranchingFactor);
				}
				Score = Children.GetScore(this);
			}
		}

		protected Block1Node Create(Field field, BlockPath path, ApplyParameters pars)
		{
			var score = pars.Evaluator.GetScore(field, pars.Parameters);
			pars.Evaluations++;
			return new Block1Node(field, path, score, pars.Next.BranchingFactor1);
		}
		protected override Block1Node Create(Field field, ApplyParameters pars) { throw new NotImplementedException(); }

		protected override Block GetBlock(ApplyParameters pars) { return pars.Current; }
	}
}
