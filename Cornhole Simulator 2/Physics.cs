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




                OutputBeanBags.Add(bag);
            }

            return OutputBeanBags;
        }
    }
}
