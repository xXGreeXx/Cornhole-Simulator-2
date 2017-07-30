using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cornhole_Simulator_2
{
    class BeanBag
    {
        //define global variables
        public int BagX { get; set; }
        public int BagY { get; set; }
        public int BagZ { get; set; }
        public int BagOrientation { get; set; } = 0;
        public int BagVelocityX { get; set; }
        public int BagVelocityY { get; set; }

        //constructor
        public BeanBag(int x, int y, int z, int xVelocity, int yVelocity)
        {
            BagX = x;
            BagY = y;
            BagZ = z;
            BagVelocityX = xVelocity;
            BagVelocityY = yVelocity;
        }
    }
}
