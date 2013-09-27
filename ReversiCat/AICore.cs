using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReversiCat
{
    class AICore
    {

        Board originalBoard;


        protected int W_WEIGTH = 5;
        protected int W_SELF_MOBILITY = 1;
        protected int W_OPP_MOBILITY = 1;
        protected const int MIN_VALUE = -9999999;

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
        protected int ComputeWeight(Board board,int player)
        {
            int result = 0;
            foreach (Position pos in board.positions)
            {
                result += (pos.color * pos.weight);
            }
            return player == 1 ? result : -result;

        }


        protected int Evaluation(Board board,int player)
        {
            //int num = 0;
            //for (int i = 0; i < 8; i++)
            //{
            //    for (int j = 0; j < 8; j++)
            //    {
            //        if (board.positions[i, j].color != 0)
            //            num++;
            //    }
            //}
            //if(num > 10)
            //{
                //W_MOBILITY = 1;
            //    W_WEIGTH = 3;
            //}
            return W_WEIGTH * ComputeWeight(board, player) + W_SELF_MOBILITY * Mobility(player);// -W_OPP_MOBILITY * Mobility(-player);
        }



        protected int AlphaBeta(int alpha, int beta, int pass, int depth, int player, Board previousBoard)
        {
            int subBestX = -1;
            int subBestY = -1;
            int bestValue = MIN_VALUE;
            if (depth <= 0)
                return Evaluation(previousBoard,player);

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

            if (bestValue <= MIN_VALUE)
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




        protected int PVS(int alpha, int beta, int pass, int depth, int player, Board previousBoard)
        {
            int subBestX = -1;
            int subBestY = -1;
            int bestValue = MIN_VALUE;
            if (depth <= 0)
                return Evaluation(previousBoard, player);

            //Try every possible position
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int value = 0;
                    Board nextBoard = previousBoard.CloneBoard().FlipPiece(i, j, false, player);
                    if (nextBoard != null)
                    {
                        if (bestValue == alpha)
                        {
                            value = -PVS(-alpha-1, -alpha, 0, depth - 1, -player, nextBoard);
                            if (value <= alpha)
                                continue;

                            if (value > beta)
                                return value;

                            bestValue = alpha = value;
                        }
                        value = -PVS(-beta, -alpha, 0, depth - 1, -player, nextBoard);


                        if (value > beta)
                            return value;
                        if (value > bestValue)
                        {
                            bestValue = value;
                            subBestX = i;
                            subBestY = j;

                            if (value > alpha)
                                alpha = value;
                        }
                    }
                }
            }

            if (bestValue <= MIN_VALUE)
            {
                if (pass == 0)
                {
                    //here may be needed to be modified
                    //since the game is over, the player can win then the weight should be
                    //very large to assure he can win
                    return previousBoard.ComputeScore(player) * 100;
                }
                //DoPassMove();
                bestValue = -PVS(-beta, -alpha, 1, depth, -player, previousBoard);
                //UnDoPassMove();
            }

            bestMoveX = subBestX;
            bestMoveY = subBestY;
            return bestValue;
        }




        public void MakeBestMove(out int X, out int Y, Board board)
        {
            this.originalBoard = board;
            AlphaBeta(-50000, 50000, 0, 7, originalBoard.currentPlayer, board);
            //PVS(-50000, 50000, 0, 6, originalBoard.currentPlayer, board);
            X = this.bestMoveX;
            Y = this.bestMoveY;
        }

    }
}
