using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReversiCat
{
    class AICore
    {

        Board originalBoard;


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


        protected int Mobility(int player)
        {
            return originalBoard.GetNoPossibleMoves(player);
        }
        protected int ComputeWeight()
        {
            return 0;
        }


        protected int Evaluation(int player)
        {
            return W_WEIGTH * ComputeWeight() + W_MOBILITY * Mobility(player);
        }



        protected int AlphaBeta(int alpha, int beta, int pass, int depth, int player, Board previousBoard)
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
                    Board nextBoard = previousBoard.CloneBoard().FlipPiece(i, j, false, player);
                    if (nextBoard != null)
                    {
                        int value = -AlphaBeta(-beta, -alpha, 0, depth - 1, -player, nextBoard);


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
                    return previousBoard.ComputeScore(player) * 100; 
                }
                //DoPassMove();
                bestValue  = -AlphaBeta(-beta, -alpha, 1, depth,-player,previousBoard);
                //UnDoPassMove();
            }

            bestMoveX = subBestX;
            bestMoveY = subBestY;
            return bestValue;
        }

        public void MakeBestMove(out int X, out int Y, Board board)
        {
            this.originalBoard = board;
            AlphaBeta(-64, 64, 0, 2, originalBoard.currentPlayer, board);
            X = this.bestMoveX;
            Y = this.bestMoveY;
        }

    }
}
