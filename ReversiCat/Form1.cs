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
        private Board board = new Board();
        private bool lockProcess; //not to respond to click event if current processing is not finished yet

        public void btn_Click(object sender, EventArgs args)
        {
            groupBox1.Visible = false;
            if (radioButton1.Checked)
                board.gameMode = 0;
            else
            {
                board.gameMode = 1;
                if (radioButton3.Checked)
                    board.startPlayer = -1;
                else
                    board.startPlayer = 1;
            }
        }

        public void radioBtn_Click(object sender, EventArgs args)
        {
            panel2.Visible = radioButton2.Checked;
        }


        public ReversiForm()
        {
            InitializeComponent();
            StatusLbl.Text = "White player start playing...";
            this.button1.Click += btn_Click;
            this.radioButton1.Click += radioBtn_Click;
            this.radioButton2.Click += radioBtn_Click;
            panel2.Visible = false;
            radioButton1.Select();
            radioButton3.Select();
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
                    if (board.positions[i, j].color != 0)
                    {
                        g.FillEllipse(new SolidBrush(board.positions[i, j].color == 1 ? Color.White : Color.Black), i * 80 + 10, j * 80 + 10, width, height);
                    }
                }
            }

            g.Dispose();

        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (lockProcess || board.IsCurrentPlayerAI())
                return;
            else
                lockProcess = true;
            Point hitPoint = new Point(e.X, e.Y);
            int direcX = hitPoint.X / 80;
            int direcY = hitPoint.Y / 80;
            string resultStatusText = "";
            int result = board.Play(direcX, direcY, out resultStatusText, false);
            StatusLbl.Text = resultStatusText;
            lockProcess = false;
            Refresh();
        }


        private void ReversiForm_Load(object sender, EventArgs e)
        {

        }

    }


}
