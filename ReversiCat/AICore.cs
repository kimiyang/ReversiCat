using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReversiCat
{
    class AICore
    {
        protected const int W_WEIGTH = 0;
        protected const int W_MOBILITY = 1;
        protected const int MIN_VALUE = -99999;

        protected int bestMoveX;
        protected int bestMoveY;

        protected void DoPassMove()
        {
        }
        protected void UnDoPassMove()
        {
        }


        protected int DoMove()
        {
            return 1;
        }

        protected int UnDoMove()
        {
            return 1;
        }


        protected int Mobility()
        {
            return 0;
        }
        protected int ComputeWeight()
        {
            return 0;
        }


        protected int Evaluation(int player)
        {
            return W_WEIGTH * ComputeWeight() + W_MOBILITY * Mobility();
        }

        protected int GetFinalScore()
        {
            return 0;
        }


        protected int AlphaBeta(int alpha, int beta, int pass, int depth, int player)
        {
            int subBestX = -1;
            int subBestY = -1;
            int bestValue = MIN_VALUE;
            if (depth <= 0)
                return Evaluation(player);

            //Try every possible position
            for( int i=0; i<8; i++ )
            {
                for (int j = 0; j < 8; j++)
                {
                    if (DoMove() > 0)
                    {
                        int value = -AlphaBeta(-beta, -alpha, 0, depth - 1, -player);
                        UnDoMove();

                        if (value > beta)
                            return value;
                        if (value > bestValue)
                        {
                            bestValue = value;
                            subBestX = i;
                            subBestY = j;

                            if(value > alpha)
                                alpha = value;
                        }
                    }
                }
            }

            if (bestValue == MIN_VALUE)
            {
                if (pass == 0)
                {
                    //here may be needed to be modified
                    //since the game is over, the player can win then the weight should be
                    //very large to assure he can win
                    return GetFinalScore() * 100; 
                }
                DoPassMove();
                bestValue  = -AlphaBeta(-beta, -alpha, 1, depth,-player);
                UnDoPassMove();
            }

            bestMoveX = subBestX;
            bestMoveY = subBestY;
            return bestValue;
        }

        public void MakeBestMove(out int X, out int Y)
        {
            int player = 0;
            AlphaBeta(-100, 100, 0, 8, player);
            X = this.bestMoveX;
            Y = this.bestMoveY;
        }

    }
}
