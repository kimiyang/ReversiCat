using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace ReversiCat
{


    public partial class btnRestart : Form
    {
        private Board board = new Board();
        private bool lockProcess; //not to respond to click event if current processing is not finished yet

        public void btn_Click(object sender, EventArgs args)
        {
            board.init();
            Console.WrinteLine("Hello");
            groupBox1.Visible = false;
            panel1.Visible = true;
            label1.Visible = StatusLbl.Visible = CurrentResultLbl.Visible = true;
            this.saveToolStripMenuItem.Enabled = true;
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
            if (board.startPlayer == 1)
            {
                string result;
                board.Play(-2, -2, out result, true);
                StatusLbl.Text = result;
            }
            this.Refresh();
        }

        public void radioBtn_Click(object sender, EventArgs args)
        {
            panel2.Visible = radioButton2.Checked;
        }

        public void newGameToolStripMenuItem_Click(object sender, EventArgs args)
        {
            this.groupBox1.Visible = panel1.Visible = true;
        }

        public void saveGameToolStripMenuItem_Click(object sender, EventArgs args)
        {
            panel3.Visible = true;
        }

        public void loadGameToolStripMenuItem_Click(object sender, EventArgs args)
        {
            openFileDialog1.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
        }

        public void exitGameToolStripMenuItem_Click(object sender, EventArgs args)
        {
            this.Close();
        }

        public void undoToolStripMenuItem_Click(object sender, EventArgs args)
        {
            board.Undo();
            this.undoToolStripMenuItem.Enabled = false;
            this.Refresh();
        }

        public void save(string filename)
        {
            File.WriteAllBytes(Path.GetDirectoryName(Application.ExecutablePath) + "//" + filename + ".sav", this.board.SaveToFile());
        }

        public void load(object sender, CancelEventArgs e)
        {
            byte[] data = File.ReadAllBytes(openFileDialog1.FileName);
            if (board.LoadFile(data))
            {
                panel1.Visible = true;
                StatusLbl.Text = board.GetCurrentGameStatusText();
                lockProcess = false;
                label1.Visible = StatusLbl.Visible = CurrentResultLbl.Visible = true;
                this.saveToolStripMenuItem.Enabled = true;
                StatusLbl.Text = board.GetCurrentGameStatusText();
                this.Refresh();
            }
            else
            {
                MessageBox.Show("Invalid or corrupted file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void save_Click(object sender, EventArgs args)
        {
            if (textBox1.Text.Length > 0)
            {
                if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "//" + textBox1.Text + ".sav"))
                {
                    if (MessageBox.Show("A file with the same name already exists. Do you want to overwrite?", "File Exists", MessageBoxButtons.YesNo) ==  DialogResult.Yes)
                        save(textBox1.Text);
                }
                else
                    save(textBox1.Text);
            }
            else
            {
                textBox1.Focus();
            }
            panel3.Visible = false;

        }

        public void cancel_Click(object sender, EventArgs args)
        {
            panel3.Visible = false;
        }

        public btnRestart()
        {
            InitializeComponent();
            StatusLbl.Text = "White player start playing...";
            CurrentResultLbl.Text = board.ComputeNoPieces();
            this.button1.Click += btn_Click;
            this.newGameToolStripMenuItem.Click += newGameToolStripMenuItem_Click;
            this.saveToolStripMenuItem.Click += saveGameToolStripMenuItem_Click;
            this.loadToolStripMenuItem.Click += loadGameToolStripMenuItem_Click;
            this.exitToolStripMenuItem.Click += exitGameToolStripMenuItem_Click;
            this.undoToolStripMenuItem.Click += undoToolStripMenuItem_Click;
            this.openFileDialog1.FileOk += load;
            this.openFileDialog1.Filter = "Save Files(*.sav)|*.sav";
            this.radioButton1.Click += radioBtn_Click;
            this.radioButton2.Click += radioBtn_Click;
            this.button2.Click += save_Click;
            this.button3.Click += cancel_Click;
            this.saveToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Enabled = false;
            panel2.Visible = groupBox1.Visible = panel1.Visible = panel3.Visible = false;
            label1.Visible = StatusLbl.Visible = CurrentResultLbl.Visible = false;
            radioButton1.Select();
            radioButton3.Select();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            // Create pen.  
            Pen blackPen = new Pen(Color.Black, 3);
            Pen redPen = new Pen(Color.Red, 2);
            // Create brush
            Brush blackBrush = new SolidBrush(Color.Black);
            Brush whiteBrush = new SolidBrush(Color.White);
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
                        g.FillEllipse(board.positions[i, j].color == 1 ? whiteBrush : blackBrush, i * 80 + 10, j * 80 + 10, width, height);
                        if (i == board.lastPlayX && j == board.lastPlayY)
                        {
                            g.DrawEllipse(redPen, i * 80 + 9, j * 80 + 9, width + 2, height + 2);
                        }
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
            if (result == 1)
                this.saveToolStripMenuItem.Enabled = false;
            if (resultStatusText != "")
            {
                StatusLbl.Text = resultStatusText;
                CurrentResultLbl.Text = board.ComputeNoPieces();
            }
            if (result != -1)
                Refresh();
            
            if (board.IsCurrentPlayerAI())
            {
                System.Threading.Thread.Sleep(200);
                result = board.Play(direcX, direcY, out resultStatusText, true);
                if (result == 1)
                    this.saveToolStripMenuItem.Enabled = false;
                if (resultStatusText != "")
                {
                    StatusLbl.Text = resultStatusText;
                    CurrentResultLbl.Text = board.ComputeNoPieces();
                }
                if (result != -1)
                {
                    Refresh();
                    this.undoToolStripMenuItem.Enabled = true;
                }
            }
            lockProcess = false;
            
        }


        private void ReversiForm_Load(object sender, EventArgs e)
        {

        }

    }


}
