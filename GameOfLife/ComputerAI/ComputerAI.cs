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
        private readonly DataModel.DataModel model;

        public ComputerAI(DataModel.DataModel receivedModel)
        {
            model = receivedModel;
        }

        public Int32 computerTurn()
        {
            /* T.SZ. Visszatérési érték a menüpont sorszáma (0-4 között):
             * 0 - Pörgetés
             * 1 - Otthonbiztosítás vásárlása
             * 2 - Gépjárműbiztosítás vásárlása
             * 3 - Részvény vásárlása
             * 4 - Hitel visszafizetése
             * 5 - Mentés
             * 6 - Kilépés
             */
            return 0;
        }

        public Keys firstFork()
        {
            Random rnd = new Random();
            Int32 decision = rnd.Next(1, 3);
            if (decision == 1)
            {
                return Keys.D1;
            }
            else
            {
                return Keys.D2;
            }
        }

        public Keys atFork()
        {
            Random rnd = new Random();
            Int32 decision = rnd.Next(1, 3);
            if (decision == 1)
            {
                return Keys.D1;
            }
            else
            {
                return Keys.D2;
            }
        }

        public bool blueFieldChangeJob()
        {
            return true; //T.SZ. elég egy sima bool érték, hogy vált-e munkát vagy sem
        }

        public bool blueFieldTradeSalary()
        {
            Int32 mySalary = model.PlayerSalary(model.ActualPlayer);
            Int32 maxSalary = mySalary;
            for (int i = 0; i < model.NumberOfPlayers; ++i )
            {
                if(model.PlayerSalary(i)>=maxSalary)
                {
                    maxSalary = model.PlayerSalary(i);
                }
            }
            if(maxSalary!=mySalary)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Int32 buyStock()
        {
            Random rnd = new Random();
            Int32 decision = rnd.Next(0, 9);
            while(model.GetStockCardAvailability(decision)!=true)
            {
                decision = (decision + 1) % 9;
            }
            return decision;
        }

        public Int32 selectJob(List<Int32> jobs)
        {
            Random rnd = new Random();
            Int32 decision = rnd.Next(0, 3);
            return decision; //T.SZ. visszatérési érték: 0,1,2. Attól függően, hogy hanyadik munkát akarja
        }

        public Int32 selectSalary(List<Int32> salaries)
        {
            int max = 0;
            for (int i = 1; i < salaries.Count; i++)
            {
                if(salaries[i]>salaries[max])
                {
                    max = i;
                }
            }
            Console.WriteLine(max);
            return max; //T.SZ. visszatérési érték: 0,1,2. Attól függően, hogy hanyadik fizetést akarja
        }

        public Int32 tradeSalary()
        {
            Int32 maxSalary = model.ActualPlayer;
            for (int i = 0; i < model.NumberOfPlayers; ++i)
            {
                if (model.PlayerSalary(i) > model.PlayerSalary(maxSalary))
                {
                    maxSalary = i;
                }
            }
            return maxSalary;
        }

        public Keys selectRetire()
        {
            return Keys.D1; //T.SZ. visszatérési érték: Keys.D1, ha vidéki ház; Keys.D2, ha milliomosok háza
        }
    }
}
