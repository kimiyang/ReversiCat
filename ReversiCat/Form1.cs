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


    public partial class ReversiForm : Form
    {

        int currentPlayer = 1;   //-1 Black player 1 White player

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
            Point hitPoint = new Point(e.X, e.Y);
            int direcX = hitPoint.X / 80;
            int direcY = hitPoint.Y / 80;

            positions[direcX, direcY].color = currentPlayer;
            currentPlayer = -currentPlayer;
            this.Refresh();

        }

        private void ReversiForm_Load(object sender, EventArgs e)
        {

        }
    }


}
