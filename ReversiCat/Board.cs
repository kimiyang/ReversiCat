﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReversiCat
{
    public class Board
    {
        public Board parent;
        public int currentPlayer = 1;   //-1 Black player 1 White player
        
        public int noOfPieces = 0;
        public int gameMode = 0; //0: Player vs Player; 1: Player vs AI
        public int startPlayer = 0; //-1: Player; 1: AI

        public Position[,] positions = new Position[8, 8];

        public Board()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    positions[i, j] = new Position();
                    positions[i, j].color = 0;
                }

            }
            positions[3, 3].color = -1;
            positions[3, 4].color = 1;
            positions[4, 3].color = 1;
            positions[4, 4].color = -1;

            DistributeWeight();
        }


        private void DistributeWeight()
        {
            //Four corners
            positions[0, 0].weight = 20;
            positions[7, 7].weight = 20;
            positions[0, 7].weight = 20;
            positions[7, 0].weight = 20;

            //red crosses
            positions[0, 1].weight = 0;
            positions[0, 6].weight = 0;
            positions[1, 0].weight = 0;
            positions[1, 7].weight = 0;
            positions[6, 0].weight = 0;
            positions[6, 7].weight = 0;
            positions[7, 1].weight = 0;
            positions[7, 6].weight = 0;

            //black crosses
            positions[1, 1].weight = -10;
            positions[1, 6].weight = -10;
            positions[6, 1].weight = -10;
            positions[6, 6].weight = -10;

            //
            for (int i = 2; i <= 5; i++)
            {
                //red lines
                positions[i, 0].weight = 8;
                positions[i, 7].weight = 8;

                //black lines
                positions[i, 1].weight = 3;
                positions[i, 6].weight = 3;

                //green lines
                positions[i, 2].weight = 7;
                positions[i, 5].weight = 7;
            }

            for (int j = 2; j <= 5; j++)
            {
                //red lines
                positions[0, j].weight = 8;
                positions[7, j].weight = 8;

                //black lines
                positions[1, j].weight = 3;
                positions[6, j].weight = 3;

                //green lines
                positions[2, j].weight = 7;
                positions[5, j].weight = 7;
            }

        }


        /// <summary>
        /// Place a piece on (direcX, direcY)
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns>0: Successfully placed; 1: Game Ended; 2: No possible Move for current player; 3: Error in AI</returns>
        public int Play(int direcX, int direcY, out string result, bool isAI)
        {
            if (direcX != -2 || direcY != -2)
            {
                if (positions[direcX, direcY].color == 0)
                {
                    if (FlipPiece(direcX, direcY, false, currentPlayer) != null)
                    {
                        if (noOfPieces == 64)
                        {
                            result = EndGame();
                            return 1;
                        }
                    }
                    else if (isAI)
                    {
                        result = "Error: AI try to put in invalid place " + direcX.ToString() + "/" + direcY.ToString();
                        return 3;
                    }
                    else
                    {
                        result = "";
                        return -1;
                    }

                }
                else if (!isAI)
                {
                    result = "";
                    return -1;
                }
            }
            int statusCode = -1;
            int noPossibleMoves = GetNoPossibleMoves(currentPlayer);
            if (noPossibleMoves == 0)
            {
                currentPlayer = -currentPlayer;

                noPossibleMoves = GetNoPossibleMoves(currentPlayer);
                if (noPossibleMoves == 0)
                {
                    result = EndGame();
                    statusCode = 1;
                }

                result = currentPlayer == 1 ? "No possible move for black player. White player plays again" : "No possible move for white player. Black player plays again";
                statusCode = 2;
            }
            else
            {
                result = currentPlayer == 1 ? "White player playing..." : "Black player playing...";
                statusCode = 0;
            }
            if (IsCurrentPlayerAI() && gameMode == 1 && (statusCode == 0 || statusCode == 2) && isAI)
            {
                if (AIPlay() == -1)
                {
                    result = "Error: AI failed to move";
                    statusCode = 3;
                }
            }
            return statusCode;

        }

        public bool IsCurrentPlayerAI()
        {
            if (gameMode == 0)
                return false;
            else
                return currentPlayer * startPlayer == 1;
        }

        public int AIPlay()
        {
            string s;
            AICore ai = new AICore();
            int x;
            int y;
            ai.MakeBestMove(out x, out y, this.CloneBoard());
            if (x == -1 && y == -1)
            {
                return -1;
            }
            else
                Play(x, y, out s,true);
            return 0;
        }

        public int GetNoPossibleMoves(int player)
        {
            int result = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (positions[i, j].color == 0 && FlipPiece(i, j, true, player) != null)
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        public int ComputeScore(int player)
        {
            int result = 0;
            foreach (Position pos in positions)
            {
                result += pos.color;
            }
            return player == 1 ? result : -result;
        }


        public string EndGame()
        {
            int result = 0;
            foreach (Position pos in positions)
            {
                result += pos.color;
            }
            if (result > 0)
                return "White player won!";
            else if (result < 0)
                return "Black player won!";
            else
                return "Draw Game!";
        }

        public Board FlipPiece(int x, int y, bool CheckOnly, int player)
        {
            List<Position> processedPosition = new List<Position>();
            List<Position> positionToFlip = new List<Position>();
            if (positions[x, y].color != 0)
                return null;
            int i = 1;
            bool toFlip = false;
            //flip top left
            while ((x - i) >= 0 && (y - i) >= 0)
            {
                if (positions[x - i, y - i].color == player)
                {
                    toFlip = true;
                    break;
                }
                else if (positions[x - i, y - i].color == 0)
                    break;
                else
                    processedPosition.Add(positions[x - i, y - i]);
                i++;
            }
            if (toFlip)
            {
                positionToFlip.AddRange(processedPosition);
            }
            i = 1;
            processedPosition.Clear();
            toFlip = false;

            //flip top right
            while ((x - i) >= 0 && (y + i) <= 7)
            {
                if (positions[x - i, y + i].color == player)
                {
                    toFlip = true;
                    break;
                }
                else if (positions[x - i, y + i].color == 0)
                    break;
                else
                    processedPosition.Add(positions[x - i, y + i]);
                i++;
            }
            if (toFlip)
            {
                positionToFlip.AddRange(processedPosition);
            }
            i = 1;
            processedPosition.Clear();
            toFlip = false;

            //flip bottom left 
            while ((x + i) <= 7 && (y - i) >= 0)
            {
                if (positions[x + i, y - i].color == player)
                {
                    toFlip = true;
                    break;
                }
                else if (positions[x + i, y - i].color == 0)
                    break;
                else
                    processedPosition.Add(positions[x + i, y - i]);
                i++;
            }
            if (toFlip)
            {
                positionToFlip.AddRange(processedPosition);
            }
            i = 1;
            processedPosition.Clear();
            toFlip = false;

            //flip bottom right
            while ((x + i) <= 7 && (y + i) <= 7)
            {
                if (positions[x + i, y + i].color == player)
                {
                    toFlip = true;
                    break;
                }
                else if (positions[x + i, y + i].color == 0)
                    break;
                else
                    processedPosition.Add(positions[x + i, y + i]);
                i++;
            }
            if (toFlip)
            {
                positionToFlip.AddRange(processedPosition);
            }
            i = 1;
            processedPosition.Clear();
            toFlip = false;

            //flip top
            while ((x - i) >= 0)
            {
                if (positions[x - i, y].color == player)
                {
                    toFlip = true;
                    break;
                }
                else if (positions[x - i, y].color == 0)
                    break;
                else
                    processedPosition.Add(positions[x - i, y]);
                i++;
            }
            if (toFlip)
            {
                positionToFlip.AddRange(processedPosition);
            }
            i = 1;
            processedPosition.Clear();
            toFlip = false;

            //flip bottom
            while ((x + i) <= 7)
            {
                if (positions[x + i, y].color == player)
                {
                    toFlip = true;
                    break;
                }
                else if (positions[x + i, y].color == 0)
                    break;
                else
                    processedPosition.Add(positions[x + i, y]);
                i++;
            }
            if (toFlip)
            {
                positionToFlip.AddRange(processedPosition);
            }
            i = 1;
            processedPosition.Clear();
            toFlip = false;

            //flip left
            while ((y - i) >= 0)
            {
                if (positions[x, y - i].color == player)
                {
                    toFlip = true;
                    break;
                }
                else if (positions[x, y - i].color == 0)
                    break;
                else
                    processedPosition.Add(positions[x, y - i]);
                i++;
            }
            if (toFlip)
            {
                positionToFlip.AddRange(processedPosition);
            }
            i = 1;
            processedPosition.Clear();
            toFlip = false;

            //flip top
            while ((y + i) <= 7)
            {
                if (positions[x, y + i].color == player)
                {
                    toFlip = true;
                    break;
                }
                else if (positions[x, y + i].color == 0)
                    break;
                else
                    processedPosition.Add(positions[x, y + i]);
                i++;
            }
            if (toFlip)
            {
                positionToFlip.AddRange(processedPosition);
            }

            if (!CheckOnly)
            {
                foreach (Position pos in positionToFlip)
                    pos.color = currentPlayer;
            }

            if (positionToFlip.Count > 0 && !CheckOnly)
            {
                noOfPieces++;
                positions[x, y].color = currentPlayer;
                currentPlayer = -currentPlayer;
                //resultBoard = new Board();
                //resultBoard.parent = this;
                //resultBoard.currentPlayer = this.currentPlayer;
                //resultBoard.noOfPieces = this.noOfPieces;
                //resultBoard.gameMode = this.gameMode;
                //resultBoard.startPlayer = this.startPlayer;
                //for (i = 0; i < 8; i++)
                //    for (int j = 0; j < 8; j++)
                //    {
                //        resultBoard.positions[i, j].color = this.positions[i, j].color;
                //        resultBoard.positions[i, j].weight = this.positions[i, j].weight;
                //    }
                return this;
            }
            else if (positionToFlip.Count > 0 && CheckOnly)
                return this;
            else
                return null;
        }

        public Board CloneBoard()
        {
            Board resultBoard = new Board();
            resultBoard.currentPlayer = this.currentPlayer;
            resultBoard.noOfPieces = this.noOfPieces;
            resultBoard.gameMode = this.gameMode;
            resultBoard.startPlayer = this.startPlayer;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    resultBoard.positions[i, j].color = this.positions[i, j].color;
                    resultBoard.positions[i, j].weight = this.positions[i, j].weight;
                }
            return resultBoard;
        }

    }
}
