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
        public List<BeanBag> SimulatePhysicsForBeanBags(List<BeanBag> InputBeanBags)
        {
            List<BeanBag> OutputBeanBags = new List<BeanBag>();

            foreach (BeanBag bag in InputBeanBags)
            {
                if (bag.BagVelocityX > 0 || bag.BagVelocityY > 0)
                {
                    int finalVelocityX = bag.BagVelocityX / 6;
                    int finalVelocityY = bag.BagVelocityY / 2;
                    float finalVelocityZ = bag.BagVelocityY / 15F;

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
                    bag.BagVelocityY -= 2;
                }

                OutputBeanBags.Add(bag);
            }

            return OutputBeanBags;
        }
    }
}
