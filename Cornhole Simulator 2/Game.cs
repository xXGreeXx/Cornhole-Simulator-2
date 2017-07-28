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
        Bitmap arm1 = Cornhole_Simulator_2.Properties.Resources.arm1;
        Bitmap arm2 = Cornhole_Simulator_2.Properties.Resources.arm2;
        Bitmap arm1Shadow = Cornhole_Simulator_2.Properties.Resources.arm1Shadow;
        Bitmap arm2Shadow = Cornhole_Simulator_2.Properties.Resources.arm2Shadow;
        int amountOFBagsForPlayer1 = 4;
        int amountOfBagsForPlayer2 = 4;
        int player1Score = 0;
        int player2Score = 0;
        int positionOfHandX = 0;
        int positionOfHandY = 0;
        Boolean started = false;
        int startedCycle = 0;
        Boolean startedSwap = false;
        String turn = "";
        int turnFadeCycle = 255;
        float sizeOfBoard = 0.75F;

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

            //draw background
            float positionOfSkyToGround = (250F / 770F) * canvas.Height;

            g.FillRectangle(Brushes.SkyBlue, 0, 0, width, positionOfSkyToGround);
            g.DrawImage(grass, 0, positionOfSkyToGround, width, height - positionOfSkyToGround);

            //draw board
            g.DrawImage(board, width / 2 - board.Width * sizeOfBoard / 2, positionOfSkyToGround + 5, board.Width * sizeOfBoard, board.Height * sizeOfBoard);

            //draw not started text
            if (!started)
            {
                if (!startedSwap) { g.DrawString("Click Anywhere To Begin", new Font(FontFamily.GenericSansSerif, 30, FontStyle.Bold), Brushes.Black, width / 2 - 225, 140); }

                if (startedCycle >= 25) { startedSwap = true; }
                else if (startedCycle <= 0) { startedSwap = false; }

                if (startedSwap) { startedCycle--; }
                else { startedCycle++; }   
            }

            //draw player turn text
            if (turn.Equals("player1") && turnFadeCycle >= 4)
            {
                g.DrawString("Player One's Turn", new Font(FontFamily.GenericSansSerif, 30, FontStyle.Bold), new SolidBrush(Color.FromArgb(turnFadeCycle, Color.Red)), width / 2 - 165, 140);
                turnFadeCycle -= 5;
            }

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
            g.DrawString(player1Score.ToString() + "-" + player2Score.ToString(), new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold), Brushes.Black, width / 2 - 30, 0);

            //draw shadow for hand
            float angularSpeedIncrease = 2.55F;

            int yPositionOfArm1Shadow = height - arm1.Height + 170;
            int xPositionOfArm1Shadow = width / 2 - 70;
            int bottomOfArm1Shadow = height;

            int yPositionOfArm2Shadow = height - arm1.Height + 30;
            int xPositionOfArm2Shadow = xPositionOfArm1Shadow;
            int bottomOfArm2Shadow = yPositionOfArm1Shadow;

            Point[] shadowPointsForArm1 = {
                new Point(xPositionOfArm1Shadow - positionOfHandX, yPositionOfArm1Shadow),
                new Point(xPositionOfArm1Shadow + arm1.Width - positionOfHandX - 40, yPositionOfArm1Shadow),
                new Point(xPositionOfArm1Shadow, bottomOfArm1Shadow)};
            Point[] shadowPointsForArm2 = {
                new Point((int)(xPositionOfArm2Shadow - (positionOfHandX * angularSpeedIncrease)), yPositionOfArm2Shadow),
                new Point((int)(xPositionOfArm2Shadow - (positionOfHandX * angularSpeedIncrease)) + arm2.Width - 40, yPositionOfArm2Shadow),
                new Point(xPositionOfArm2Shadow - positionOfHandX - 20, bottomOfArm2Shadow + 10)};

            //g.DrawImage(arm1Shadow, shadowPointsForArm1);
            //g.DrawImage(arm2Shadow, shadowPointsForArm2);

            //draw hand
            int yPositionOfArm1 = height - arm1.Height + 170;
            int xPositionOfArm1 = width / 2;
            int bottomOfArm1 = height;

            int yPositionOfArm2 = height - arm1.Height + 40;
            int xPositionOfArm2 = xPositionOfArm1;
            int bottomOfArm2 = yPositionOfArm1;

            Point[] pointsForArm1 = {
                new Point(xPositionOfArm1 - positionOfHandX, yPositionOfArm1),
                new Point(xPositionOfArm1 + arm1.Width - positionOfHandX - 10, yPositionOfArm1),
                new Point(xPositionOfArm1, bottomOfArm1)};
            Point[] pointsForArm2 = {
                new Point((int)(xPositionOfArm2 - (positionOfHandX * angularSpeedIncrease)) - 10, yPositionOfArm2),
                new Point((int)(xPositionOfArm2 - (positionOfHandX * angularSpeedIncrease)) + arm2.Width - 20, yPositionOfArm2),
                new Point(xPositionOfArm2 - positionOfHandX - 30, bottomOfArm2 + 10)};

            g.DrawImage(arm1, pointsForArm1);
            g.DrawImage(arm2, pointsForArm2);

            //draw bag in hand
            Point[] pointsForBagInHand =
            {
                new Point(width / 2 - arm2.Width / 2 - (int)(positionOfHandX * angularSpeedIncrease), yPositionOfArm2),
                new Point(width / 2 + redBag.Width / 2 - (int)(positionOfHandX * angularSpeedIncrease), yPositionOfArm2),
                new Point(width / 2 - arm1.Width / 2 - 20 - (int)(positionOfHandX * angularSpeedIncrease), yPositionOfArm2 + redBag.Width)
             };

            //if (turn.Equals("player1")) { g.DrawImage(redBag, pointsForBagInHand); }
            //else if (turn.Equals("player2")) { g.DrawImage(blueBag, pointsForBagInHand); }
        }

        //canvas mouse move handler
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;
            int width = canvas.Width;
            int height = canvas.Height;

            if (started)
            {
                positionOfHandX = width / 2 - x;
                positionOfHandY = height / 2 - y;
            }
        }

        //canvas mouse down handler
        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;

            if (!started)
            {
                started = true;
                turn = "player1";
            }
        }

        //canvas mouse up handler
        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;
            int width = canvas.Width;
            int height = canvas.Height;

            if (started)
            {

            }
        }
    }
}
    