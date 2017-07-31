using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        public static float positionOfSkyToGround = 0;
        int amountOfBagsForPlayer1 = 4;
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
        List<BeanBag> beanBags = new List<BeanBag>();
        Boolean recordMovements = false;
        int xVelocity = 0;
        int yVelocity = 0;
        int pastX = 0;
        int pastY = 0;
        Physics physicsEngine = new Physics();
        Boolean thrown = false;
        int bagStartPositionX = 0;
        int bagStartPositionY = 0;
        Boolean changeRound = true;
        int round = 1;
        int roundFadeCycle = 255;

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
            positionOfSkyToGround = (250F / 770F) * canvas.Height;

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
            if (turnFadeCycle >= 4 && !changeRound)
            {
                if (turn.Equals("player1")) { g.DrawString("Player One's Turn", new Font(FontFamily.GenericSansSerif, 30, FontStyle.Bold), new SolidBrush(Color.FromArgb(turnFadeCycle, Color.Red)), width / 2 - 165, 140); }
                else if (turn.Equals("player2")) { g.DrawString("Player Two's Turn", new Font(FontFamily.GenericSansSerif, 30, FontStyle.Bold), new SolidBrush(Color.FromArgb(turnFadeCycle, Color.Blue)), width / 2 - 165, 140); }
                turnFadeCycle -= 5;
            }

            //draw bags left
            g.DrawRectangle(Pens.Black, 2, 25, 47, 203);
            g.DrawRectangle(Pens.Black, width - blueBag.Width, 25, 47, 203);

            for (int i = 0; i < amountOfBagsForPlayer1; i++)
            {
                g.DrawImage(redBag, 0, 0 + (redBag.Width * i + 25), redBag.Width, redBag.Height);
            }
            for (int i = 0; i < amountOfBagsForPlayer2; i++)
            {
                g.DrawImage(blueBag, width - blueBag.Width - 2, 0 + (blueBag.Width * i + 25), blueBag.Width, blueBag.Height);
            }

            //draw scores
            g.DrawString(player1Score.ToString() + "-" + player2Score.ToString(), new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold), Brushes.Black, width / 2 - 30, 0);

            //draw round
            if (changeRound && roundFadeCycle >= 4 && started)
            {
                g.DrawString("Round: " + round.ToString(), new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold), new SolidBrush(Color.FromArgb(roundFadeCycle, Color.Black)), width / 2 - 70, 150);

                roundFadeCycle -= 4;
            }
            if (roundFadeCycle <= 4)
            {
                changeRound = false;
            }

            //call other functions
            callPhysicsEngine();
            calculateIfNextPlayerTurn();

            //draw hand
            float angularSpeedIncrease = 2.05F;

            int xPositionOfArm1 = width / 2;
            int yPositionOfArm1 = height - arm1.Height + 140;
            int bottomOfArm1 = height;

            int xPositionOfArm2 = xPositionOfArm1;
            int yPositionOfArm2 = height - arm1.Height + 40;
            int bottomOfArm2 = yPositionOfArm1;

            Point[] pointsForArm1 = {
                new Point(xPositionOfArm1 - positionOfHandX + 10 - 60 + -(positionOfHandY / 30), yPositionOfArm1),
                new Point(xPositionOfArm1 + arm1.Width - positionOfHandX - 10 - 60 - -(positionOfHandY / 30), yPositionOfArm1),
                new Point(xPositionOfArm1 + -(positionOfHandY / 30), bottomOfArm1),
                new Point(xPositionOfArm1 - arm1.Width / 2 - -(positionOfHandY / 30), bottomOfArm1),
            };
            Point[] pointsForArm2 = {
                new Point((int)(xPositionOfArm2 - (positionOfHandX * angularSpeedIncrease)) + 70 - 120 + -(positionOfHandY / 20), yPositionOfArm2),
                new Point((int)(xPositionOfArm2 - (positionOfHandX * angularSpeedIncrease)) + arm2.Width - 10 - 110 - -(positionOfHandY / 20), yPositionOfArm2),
                new Point(xPositionOfArm2 - positionOfHandX + 65 - 60 - -(positionOfHandY / 30), bottomOfArm2 + 10),
                new Point(xPositionOfArm2 - positionOfHandX - arm1.Width / 2 + 50 - 60 + -(positionOfHandY / 30), bottomOfArm2 + 10)
            };

            g.FillPolygon(new SolidBrush(Color.FromArgb(255, Color.DarkOrange.R, Color.DarkOrange.G + 30, Color.DarkOrange.B + 30)), pointsForArm1);
            g.FillPolygon(Brushes.DarkOrange, pointsForArm2);

            //draw bag in hand
            Point[] pointsForBagInHand =
            {
                new Point((int)(xPositionOfArm2 - (positionOfHandX * angularSpeedIncrease)) + 70 - 120 + -(positionOfHandY / 20), yPositionOfArm2),
                new Point((int)(xPositionOfArm2 - (positionOfHandX * angularSpeedIncrease)) + arm2.Width - 10 - 110 - -(positionOfHandY / 20), yPositionOfArm2),
                new Point(xPositionOfArm2 - positionOfHandX + 65 - 60 - -(positionOfHandY / 20), bottomOfArm2 + 10 - 80),
                new Point(xPositionOfArm2 - positionOfHandX - arm1.Width / 2 + 50 - 60 + -(positionOfHandY / 20), bottomOfArm2 + 10 - 80)
             };

            bagStartPositionX = (int)(xPositionOfArm2 - (positionOfHandX * angularSpeedIncrease)) + 70 - 120 + -(positionOfHandY / 20);
            bagStartPositionY = yPositionOfArm2;

            if (!thrown)
            {
                if (turn.Equals("player1")) { g.FillPolygon(Brushes.Red, pointsForBagInHand); }
                else if (turn.Equals("player2")) { g.FillPolygon(Brushes.Blue, pointsForBagInHand); }
            }

            //draw bean bags
            foreach (BeanBag bag in beanBags)
            {
                float sizeBaseOnZ = 1.5F * (bag.BagZ / redBag.Width);

                if (bag.playerIDOfBag.Equals(1)) { g.DrawImage(redBag, bag.BagX, bag.BagY, redBag.Width * sizeBaseOnZ, redBag.Height * sizeBaseOnZ); }
                else { g.DrawImage(blueBag, bag.BagX, bag.BagY, blueBag.Width * sizeBaseOnZ, blueBag.Height * sizeBaseOnZ); }
            }
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

                if (recordMovements)
                {
                    if (pastX == 0 || pastY == 0)
                    {
                        pastX = x;
                        pastY = y;
                    }
                    else
                    {
                        xVelocity += Math.Abs(x - pastX);
                        yVelocity += Math.Abs(y - pastY);
                    }
                }
            }
        }

        //canvas mouse down handler
        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;

            if (started)
            {
                recordMovements = true;
            }
        }

        //canvas mouse up handler
        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;
            int width = canvas.Width;
            int height = canvas.Height;

            if (started && !thrown && !changeRound)
            {
                float z = 50.0F;
                String dir = "left";

                if (positionOfHandX < width / 2)
                {
                    dir = "left";
                }
                else
                {
                    dir = "right";
                }
                
                if (turn.Equals("player1"))
                {
                    BeanBag bagToAdd = new BeanBag(bagStartPositionX, bagStartPositionY, z, xVelocity, yVelocity, 1, dir);
                    beanBags.Add(bagToAdd);
                }
                else if (turn.Equals("player2"))
                {
                    BeanBag bagToAdd = new BeanBag(bagStartPositionX, bagStartPositionY, z, xVelocity, yVelocity, 2, dir);
                    beanBags.Add(bagToAdd);
                }

                pastX = 0;
                pastY = 0;
                xVelocity = 0;
                yVelocity = 0;
                thrown = true;
                recordMovements = false;
            }

            if (!started)
            {
                started = true;
                turn = "player1";
                amountOfBagsForPlayer1--;
            }
        }

        //call phyics engine
        private void callPhysicsEngine()
        {
            physicsEngine.SimulatePhysicsForBeanBags(beanBags);
        }

        //calculate if it is the next players turn
        private void calculateIfNextPlayerTurn()
        {
            Boolean check = true;

            if (beanBags.Count <= 0)
            {
                check = false;
            }

            foreach (BeanBag bag in beanBags)
            {
                if (bag.BagVelocityX > 0 || bag.BagVelocityY > 0)
                {
                    check = false;
                }
            }

            if (check && thrown)
            {
                if (amountOfBagsForPlayer1 == 0 && amountOfBagsForPlayer2 == 0)
                {
                    changeRound = true;
                    roundFadeCycle = 255;
                    round++;

                    amountOfBagsForPlayer1 = 4;
                    amountOfBagsForPlayer2 = 4;

                    calculatePointsAfterRound();
                }

                if (turn.Equals("player1"))
                {
                    turn = "player2";
                    amountOfBagsForPlayer2--;
                    thrown = false;
                    turnFadeCycle = 255;
                }
                else if (turn.Equals("player2"))
                {
                    turn = "player1";
                    amountOfBagsForPlayer1--;
                    thrown = false;
                    turnFadeCycle = 255;
                }
            }
        }

        //calculate points after round
        private void calculatePointsAfterRound()
        {



            beanBags = new List<BeanBag>();
        }
    }
}
    