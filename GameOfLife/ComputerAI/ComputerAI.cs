using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using GameOfLife.DataModel;

namespace GameOfLife.ComputerAI
{
    public class ComputerAI
    {
        private DataModel.DataModel model;

        public ComputerAI(DataModel.DataModel receivedModel)
        {
            model = receivedModel;
        }

        public Keys atFork()
        {
            return 0;
        }

        public Keys blueFieldChangeJob()
        {
            return 0;
        }

        public Keys blueFieldSwitchSalary()
        {
            return 0;
        }

        public Keys buyStock()
        {
            return 0;
        }

        public Keys computerTurn()
        {
            return 0;
        }

        public Keys firstFork()
        {
            return 0;
        }

        public Keys selectJob(Int32[] jobs)
        {
            return 0;
        }

        public Keys selectRetire()
        {
            return 0;
        }

        public Keys selectSalary(Int32[] salaries)
        {
            return 0;
        }

        public Keys switchSalary()
        {
            return 0;
        }


    }
}
