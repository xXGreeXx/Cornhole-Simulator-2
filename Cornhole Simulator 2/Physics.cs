using System;
using System.Collections.Generic;
using System.Drawing;

namespace Cornhole_Simulator_2
{
    class Physics
    {
        //constructor
        public Physics()
        {

        }

        //simulate bag physics
        public void SimulatePhysicsForBeanBags(List<BeanBag> InputBeanBags)
        {
            foreach (BeanBag bag in InputBeanBags)
            {
                //calculate bag velocity
                if (bag.BagVelocityX > 0 || bag.BagVelocityY > 0)
                {
                    int finalVelocityX = bag.BagVelocityX / 6;
                    int finalVelocityY = bag.BagVelocityY / 3;
                    float finalVelocityZ = bag.BagVelocityY / 25F;

                    bag.BagX += finalVelocityX;


                    if (bag.BagY > Game.positionOfSkyToGround)
                    {
                        bag.BagY -= finalVelocityY;
                    }

                    if (bag.BagZ > 3)
                    {
                        bag.BagZ -= finalVelocityZ;
                    }
                    else
                    {
                        bag.BagVelocityX = 0;
                        bag.BagVelocityY = 0;
                    }
                }

                if (bag.BagVelocityX > 0)
                {
                    bag.BagVelocityX -= 8;
                }
                if (bag.BagVelocityY > 0)
                {
                    bag.BagVelocityY -= 1;
                }
            }
        }
    }
}
