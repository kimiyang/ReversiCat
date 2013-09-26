using System;
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
            positions[3, 3].color = 1;
            positions[3, 4].color = -1;
            positions[4, 3].color = -1;
            positions[4, 4].color = 1;
        }

        /// <summary>
        /// Place a piece on (direcX, direcY)
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns>0: Successfully placed; 1: Game Ended; 2: No possible Move for current player</returns>
        public int Play(int direcX, int direcY, out string result)
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

            }
            int noPossibleMoves = GetNoPossibleMoves(currentPlayer);
            if (noPossibleMoves == 0)
            {
                currentPlayer = -currentPlayer;

                noPossibleMoves = GetNoPossibleMoves(currentPlayer);
                if (noPossibleMoves == 0)
                {
                    result = EndGame();
                    return 1;
                }

                result = currentPlayer == 1 ? "No possible move for black player. White player plays again" : "No possible move for white player. Black player plays again";
                return 2;
            }
            else
            {
                result = currentPlayer == 1 ? "White player playing..." : "Black player playing...";
                return 0;
            }

        }

        public bool IsCurrentPlayerAI()
        {
            if (gameMode == 0)
                return false;
            else
                return currentPlayer * startPlayer == 1;
        }

        public void AIPlay()
        {
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

            Board resultBoard = null;
            if (positionToFlip.Count > 0 && !CheckOnly)
            {
                noOfPieces++;
                positions[x, y].color = currentPlayer;
                currentPlayer = -currentPlayer;
                resultBoard = new Board();
                resultBoard.parent = this;
                resultBoard.currentPlayer = this.currentPlayer;
                resultBoard.noOfPieces = this.noOfPieces;
                resultBoard.gameMode = this.gameMode;
                resultBoard.startPlayer = this.startPlayer;
                for (i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                    {
                        resultBoard.positions[i, j].color = this.positions[i, j].color;
                        resultBoard.positions[i, j].weight = this.positions[i, j].weight;
                    }

            }
            else if (positionToFlip.Count > 0 && CheckOnly)
                resultBoard = new Board();
            return resultBoard;
        }

    }
}
