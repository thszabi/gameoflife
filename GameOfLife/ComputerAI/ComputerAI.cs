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
        private List<Int32> _careerPriority = new List<int> { 8, 6, 9, 7, 3, 5, 4, 1, 2, 10 };

        public ComputerAI(DataModel.DataModel receivedModel)
        {
            model = receivedModel;
        }

        public Int32 computerTurn()
        {
            Random rnd = new Random();

            // 4 - Hitel visszafizetése
            if( model.PlayerLoan(model.ActualPlayer) > 0 && (model.PlayerMoney(model.ActualPlayer) - 25000) > 50000)
            {
                return 4;
            }

            // 1 - Otthonbiztosítás vásárlása
            if(model.PlayerHouseCard(model.ActualPlayer) != 9 && model.PlayerHouseInsurance(model.ActualPlayer) == false
                && (model.PlayerMoney(model.ActualPlayer)-50000) > model.GetInsuranceForHouseCard(model.PlayerHouseCard(model.ActualPlayer)))
            {
                Int32 decision = rnd.Next(1, 101);
                if(model.PlayerLocation(model.ActualPlayer) < 50)
                {
                    Int32 percentage = 100 - (model.PlayerHouseCard(model.ActualPlayer) * 10);
                    if(percentage >= decision)
                    {
                        return 1;
                    }
                }
                if (model.PlayerLocation(model.ActualPlayer) >= 50 && model.PlayerLocation(model.ActualPlayer) < 62)
                {
                    Int32 percentage = 100 - (model.PlayerHouseCard(model.ActualPlayer) * 10) -5;
                    if (percentage >= decision)
                    {
                        return 1;
                    }
                }
                if (model.PlayerLocation(model.ActualPlayer) >= 62 && model.PlayerLocation(model.ActualPlayer) < 107)
                {
                    Int32 percentage = 100 - (model.PlayerHouseCard(model.ActualPlayer) * 10) - 10;
                    if (percentage >= decision)
                    {
                        return 1;
                    }
                }
                if (model.PlayerLocation(model.ActualPlayer) >= 107 && model.PlayerLocation(model.ActualPlayer) < 112)
                {
                    Int32 percentage = 100 - (model.PlayerHouseCard(model.ActualPlayer) * 10) - 15;
                    if (percentage >= decision)
                    {
                        return 1;
                    }
                }
            }

            // 2 - Gépjárműbiztosítás vásárlása
            if(model.PlayerCarInsurance(model.ActualPlayer) == false && (model.PlayerMoney(model.ActualPlayer)-50000) >= 10000)
            {
                Int32 decision = rnd.Next(1, 101);
                if(model.PlayerLocation(model.ActualPlayer) < 31)
                {
                    if(20 >= decision)
                    {
                        return 2;
                    }
                }
                else if(model.PlayerLocation(model.ActualPlayer) >= 31 && model.PlayerLocation(model.ActualPlayer) < 56)
                {
                    if(10 >= decision)
                    {
                        return 2;
                    }
                }
            }

            // 3 - Részvény vásárlása
            if (model.PlayerStockCard(model.ActualPlayer) == 9 && model.PlayerMoney(model.ActualPlayer) >= 100000)
            {
                Int32 decision = rnd.Next(1, 101);
                Double percentage = (double)80 - (((double)model.PlayerLocation(model.ActualPlayer) / 188) * 100);
                if(percentage>decision)
                {
                    return 3;
                }
            }

            // 0 - Pörgetés
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

        public Keys atFork1()
        {
            Random rnd = new Random();
            Double numberOfPlayers = model.NumberOfPlayers - 1;
            Double HigherSalary = 0;
            for (int i = 0; i < model.NumberOfPlayers; ++i)
            {
                if (model.PlayerSalary(i) > model.PlayerSalary(model.ActualPlayer))
                {
                    HigherSalary = HigherSalary + 1;
                }
            }
            Double percentage = (HigherSalary/numberOfPlayers) * 100;
            Int32 decision = rnd.Next(1, 101);
            if(decision < percentage)
            {
                return Keys.D2;
            }
            else
            {
                return Keys.D1;
            }
        }

        public Keys atFork2()
        {
            Random rnd = new Random();
            Double numberOfPlayers = model.NumberOfPlayers - 1;
            Double HigherSalary = 0;
            for (int i = 0; i < model.NumberOfPlayers; ++i)
            {
                if (model.PlayerSalary(i) > model.PlayerSalary(model.ActualPlayer))
                {
                    HigherSalary = HigherSalary + 1;
                }
            }
            Double percentage = (HigherSalary / numberOfPlayers) * 100;
            Int32 decision = rnd.Next(1, 101);
            if (decision < percentage)
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
            Random rnd = new Random();
            Int32 decision = rnd.Next(1, 101);
            if(_careerPriority[model.PlayerCareerCard(model.ActualPlayer)]*10>=decision)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool blueFieldTradeSalary()
        {
            Int32 mySalary = model.PlayerSalary(model.ActualPlayer);
            Int32 maxSalary = mySalary;
            for (int i = 0; i < model.NumberOfPlayers; ++i )
            {
                if(model.PlayerSalary(i)>maxSalary)
                {
                    maxSalary = model.PlayerSalary(i);
                }
            }
            if(maxSalary > mySalary)
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
            Int32 priority0 = _careerPriority[jobs[0]];
            Int32 priority1 = _careerPriority[jobs[1]];
            Int32 priority2 = _careerPriority[jobs[2]];
            if(priority0 > priority1 && priority0 > priority2)
            {
                return 0;
            }
            else
            {
                if(priority1 > priority0 && priority1 > priority2)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
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
            Int32 myMoney = model.PlayerMoney(model.ActualPlayer);
            Int32 maxMoney = 0;
            for (int i = 0; i < model.NumberOfPlayers; ++i)
            {
                if(model.PlayerMoney(i) > maxMoney)
                {
                    maxMoney = model.PlayerMoney(i);
                }
            }
            if(maxMoney == myMoney || (myMoney+ (maxMoney*(0.1)) >= maxMoney))
            {
                return Keys.D2;
            }
            else
            {
                return Keys.D1;
            }
            //T.SZ. visszatérési érték: Keys.D1, ha vidéki ház; Keys.D2, ha milliomosok háza
        }
    }
}
