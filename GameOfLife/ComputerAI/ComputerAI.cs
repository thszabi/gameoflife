﻿using System;
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
            return true; //T.SZ. elég egy bool érték, és mindenhol Trade-ként hívjuk, legyen itt is Trade
        }

        public Int32 buyStock()
        {
            return 0; //T.SZ. visszatérési érték: 0-8 között egy szám, hogy melyik részvényt akarja megvenni. Olyat vegyen, ami még nem foglalt! Csak akkor vegyen, ha van pénze!
        }

        public Int32 selectJob(List<Int32> jobs)
        {
            return 0; //T.SZ. visszatérési érték: 0,1,2. Attól függően, hogy hanyadik munkát akarja
        }

        public Int32 selectSalary(List<Int32> salaries)
        {
            return 0; //T.SZ. visszatérési érték: 0,1,2. Attól függően, hogy hanyadik fizetést akarja
        }

        public Int32 tradeSalary()
        {
            return 0; //T.SZ. itt is legyen trade, mert mindenhol Trade-nek hívjuk. Visszatérési érték: a játékos sorszáma, akivel cserélni akar (0-5 között, lásd update függvény, TRADEWITHWHO region)
            //T.SZ. olyan játékost válasszon, aki nem önmaga, és létező játékos, pl. ha ketten játszanak, ne válassza a 3. játékost
        }

        public Keys selectRetire()
        {
            return Keys.D1; //T.SZ. visszatérési érték: Keys.D1, ha vidéki ház; Keys.D2, ha milliomosok háza
        }
    }
}
