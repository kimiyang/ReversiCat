using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ReversiCat
{



    //wenhua created...123
    public partial class ReversiForm : Form
    {

        int currentPlayer = 1;   //-1 Black player 1 White player
        bool lockProcess; //not to respond to click event if current processing is not finished yet
        int noOfPieces = 0;
        //test test

        //sdfadsfa


        int Mobility()
        {
            return 0;
        }

        int Evaluation()
        {
            return 0;
        }
        // 
        Position[,] positions = new Position[8, 8];

        protected void InitilizePosition()
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


        public ReversiForm()
        {
            InitializeComponent();
            StatusLbl.Text = "White player start playing...";
            InitilizePosition();
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            // Create pen.  
            Pen blackPen = new Pen(Color.Black, 3);
            // Create location and size of ellipse.  
            float x = 0.0F;
            float y = 0.0F;
            //float width = 150.0F;  
            //float height = 150.0F;

            int iX = 0;
            int iY = 0;

            for (int i = 0; i < 9; i++)
            {
                iX = 0;
                iY = i;
                Point p1 = new Point(iX, iY);
                iX = 640;
                Point p2 = new Point(iX, iY);
                g.DrawLine(blackPen, 0, i * 80, 640, i * 80);
            }
            for (int i = 0; i < 9; i++)
            {
                iX = 0;
                iY = i;
                Point p1 = new Point(iX, iY);
                iX = 640;
                Point p2 = new Point(iX, iY);
                g.DrawLine(blackPen, i * 80, 0, i * 80, 640);
            }

            //Draw circles

            float width = 60.0F;
            float height = 60.0F;

            //g.FillEllipse
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (positions[i, j].color != 0)
                    {
                        g.FillEllipse(new SolidBrush(positions[i, j].color == 1 ? Color.White : Color.Black), i * 80 + 10, j * 80 + 10, width, height);
                    }
                }
            }

            g.Dispose();

        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (lockProcess)
                return;
            else
                lockProcess = true;
            Point hitPoint = new Point(e.X, e.Y);
            int direcX = hitPoint.X / 80;
            int direcY = hitPoint.Y / 80;

            if (positions[direcX, direcY].color == 0)
            {
                if (FlipPiece(direcX, direcY, false))
                {
                    noOfPieces++;
                    positions[direcX, direcY].color = currentPlayer;
                    currentPlayer = -currentPlayer;
                    this.Refresh();
                    if (noOfPieces == 64)
                    {
                        EndGame();
                        return;
                    }
                }

            }
            int noPossibleMoves = GetNoPossibleMoves();
            if (noPossibleMoves == 0)
            {
                currentPlayer = -currentPlayer;
                
                noPossibleMoves = GetNoPossibleMoves();
                if (noPossibleMoves == 0)
                {
                    EndGame();
                    return;
                }

                StatusLbl.Text = currentPlayer == 1 ? "No possible move for black player. White player plays again" : "No possible move for white player. Black player plays again";
            }
            else
                StatusLbl.Text = currentPlayer == 1 ? "White player playing..." : "Black player playing...";

            lockProcess = false;
        }

        private int GetNoPossibleMoves()
        {
            int result = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (positions[i, j].color == 0 && FlipPiece(i, j, true))
                    {
                        result ++;
                    }
                }
            }
            return result;
        }


        private void EndGame()
        {
            int result = 0;
            foreach (Position pos in positions)
            {
                result += pos.color;
            }
            if (result > 0)
                StatusLbl.Text = "White player won!";
            else if (result < 0)
                StatusLbl.Text = "Black player won!";
            else
                StatusLbl.Text = "Draw Game!";
        }

        private bool FlipPiece(int x, int y, bool CheckOnly)
        {
            List<Position> processedPosition = new List<Position>();
            List<Position> positionToFlip = new List<Position>();
            int i = 1;
            bool toFlip = false;
            //flip top left
            while ((x- i) >= 0 && (y - i) >= 0)
            {
                if (positions[x - i, y - i].color == currentPlayer)
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
                if (positions[x - i, y + i].color == currentPlayer)
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
                if (positions[x + i, y - i].color == currentPlayer)
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
                if (positions[x + i, y + i].color == currentPlayer)
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
                if (positions[x - i, y].color == currentPlayer)
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
                if (positions[x + i, y].color == currentPlayer)
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
                if (positions[x, y - i].color == currentPlayer)
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
                if (positions[x, y + i].color == currentPlayer)
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

            return positionToFlip.Count > 0;
        }

        private void ReversiForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }


    public class Position
    {
        public int color; //0 no draw -1 black 1 white
        public int weight;
    }

}
