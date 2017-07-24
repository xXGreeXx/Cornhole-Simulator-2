using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cornhole_Simulator_2
{
    public partial class Game : Form
    {
        //define global variables
        Bitmap grass = Cornhole_Simulator_2.Properties.Resources.grass;
        Bitmap board = Cornhole_Simulator_2.Properties.Resources.cornHoleBoard;
        Bitmap blueBag = Cornhole_Simulator_2.Properties.Resources.blueBag;
        Bitmap redBag = Cornhole_Simulator_2.Properties.Resources.redBag;
        int amountOFBagsForPlayer1 = 4;
        int amountOfBagsForPlayer2 = 4;
        int player1Score = 0;
        int player2Score = 0;

        //initialize
        public Game()
        {
            InitializeComponent();

            timer.Start();
        }

        //update timer tick
        private void timer_Tick(object sender, EventArgs e)
        {
            canvas.Refresh();
        }

        //rendering engine
        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int width = canvas.Width;
            int height = canvas.Height;
            float sizeOfBoard = 0.85F;

            //draw background
            g.FillRectangle(Brushes.SkyBlue, 0, 0, width, 250);
            g.DrawImage(grass, 0, 250, width, height - 250);

            //draw board
            g.DrawImage(board, width / 2 - board.Width * sizeOfBoard / 2, 240, board.Width * sizeOfBoard, board.Height * sizeOfBoard);

            //draw bags left
            g.DrawRectangle(Pens.Black, 2, 25, 47, 203);
            g.DrawRectangle(Pens.Black, width - blueBag.Width, 25, 47, 203);

            for (int i = 0; i < amountOFBagsForPlayer1; i++)
            {
                g.DrawImage(redBag, 0, 0 + (redBag.Width * i + 25), redBag.Width, redBag.Height);
            }
            for (int i = 0; i < amountOFBagsForPlayer1; i++)
            {
                g.DrawImage(blueBag, width - blueBag.Width - 2, 0 + (blueBag.Width * i + 25), blueBag.Width, blueBag.Height);
            }

            //draw scores
            g.DrawString(player1Score.ToString() + "-" + player2Score.ToString(), new Font(this.Font.FontFamily, 25, FontStyle.Bold), Brushes.Black, width / 2 - 30, 0);
        }
    }
}
    