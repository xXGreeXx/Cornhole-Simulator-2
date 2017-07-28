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

        //constructor
        public BeanBag(int x, int y, int z)
        {
            BagX = x;
            BagY = y;
            BagZ = z;
        }

    }
}
