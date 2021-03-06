﻿using System;
using System.Collections.Generic;

namespace GameOfLife.DataModel
{
    [Serializable]
    struct Player
    {
        public Boolean actual;
        public Int32 careerCard;
        public Boolean carInsurance;
        public List<Int32> children;
        public Int32 childrenNumber;
        public Boolean gender;
        public Int32 houseCard;
        public Boolean houseInsurance;
        public Int32 lifeCardNumber;
        public Int32 loan;
        public Int32 location;
        public Boolean loseNextRound;
        public Boolean married;
        public Int32 money;
        public String name;
        public Boolean pc;
        public Int32 retired;
        public Int32 salaryCard;
        public Int32 stockCard;
        public Boolean deg;

        public Player(String playerName, Boolean isFemale, Boolean isPc)
        {
            actual = false;
            /* 0 = Szupersztár
             * 1 = Művész
             * 2 = Sportoló
             * 3 = Üzletkötő
             * 4 = Utazási ügynök
             * 5 = Rendőr
             * 6 = Tanár
             * 7 = Könyvelő
             * 8 = Orvos
             */
            careerCard = 9;                 // 0-8 -> karrier kártya száma, 9 -> még nincs karrier kártyája
            carInsurance = false;
            children = new List<int>();
            childrenNumber = 0;
            gender = isFemale;              // true -> nő, false -> férfi
            houseCard = 9;                  // 0-8 -> birtoklap száma, 9 -> még nincs birtoklapja
            houseInsurance = false;
            lifeCardNumber = 0;
            loan = 0;
            location = 0;                   // 0-151 -> mező száma, 0 -> start mező, 151 -> Vidéki ház, 150 -> Milliomosok nyaralója
            loseNextRound = false;
            married = false;
            money = 0;
            name = playerName;
            pc = isPc;
            retired = 0;                    // 0 -> nem nyugdíjas, 1 -> Vidéki ház, 2 -> Milliomosok nyaralója
            salaryCard = 9;                 // 0-8 -> fizetés kártya száma, 9 -> még nincs fizetés kártyája
            stockCard = 9;                  // 0-8 -> részvény száma, 9 -> még nincs részvénye
            deg = false;                   // true -> rendelkezik diplomával
        }
    }
}
