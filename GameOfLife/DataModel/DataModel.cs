using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GameOfLife.DataModel
{
    [Serializable]
    public class DataModel
    {
        private readonly Int32[] _costForHouseCards = { 40000, 60000, 80000, 100000, 120000, 140000, 160000, 180000, 200000 };
        private readonly Int32[] _insuranceForHouseCards = { 10000, 15000, 20000, 25000, 30000, 35000, 40000, 45000, 50000 };
        private Player[] _playerList;
        private Int32 _playerNumber;
        private List<Int32> _remainedCareerCards;
        private List<Int32> _remainedHouseCards;
        private Int32 _remainedLifeCards;
        private List<Int32> _remainedSalaryCards;
        private List<Int32> _remainedStockCards;
        private List<Int32> _remainedPlayers;
        private Int32 _actualPlayer;
        private List<Int32> _rewardForLifeCards;
        private List<String> _awardForLifeCards;
        private readonly Int32[] _moneyForSalaryCards = { 20000, 30000, 40000, 50000, 60000, 70000, 80000, 90000, 100000 };
        private readonly Int32[] _taxForSalaryCards = { 5000, 10000, 15000, 20000, 25000, 30000, 35000, 40000, 45000 };

        #region Get Game Data

        public Int32 ActualPlayer { get{ return _actualPlayer; } set { _actualPlayer = value; } }
        public Int32 NumberOfPlayers { get { return _playerNumber; } }
        public Boolean IsEveryoneRetired { get { return _remainedPlayers.Count == 0; } }

        #endregion

        #region Get Player Data

        /// <summary>
        /// Játékos karrier kártyája
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        public Int32 PlayerCareerCard(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            return _playerList[playerNum].careerCard;
        }

        /// <summary>
        /// Játékos fizetés kártyája
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        public Int32 PlayerSalaryCard(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            return _playerList[playerNum].salaryCard;
        }

        /// <summary>
        /// Játékos rendelkezik-e gépjármű biztosítással.
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        public Boolean PlayerCarInsurance(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            return _playerList[playerNum].carInsurance;
        }

        /// <summary>
        /// Játékos gyerekei
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        public List<Int32> PlayerChildren(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            return _playerList[playerNum].children;
        }

        /// <summary>
        /// Játékos gyerekeinek száma
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        public Int32 PlayerChildrenNumber(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            return _playerList[playerNum].childrenNumber;
        }

        /// <summary>
        /// Játékos neme
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        public Boolean PlayerGender(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            return _playerList[playerNum].gender;
        }

        /// <summary>
        /// Játékos diplomás-e
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        public Boolean PlayerDegree(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            return _playerList[playerNum].deg;
        }

        /// <summary>
        /// Játékos birtoklevelének száma
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        public Int32 PlayerHouseCard(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            return _playerList[playerNum].houseCard;
        }

        /// <summary>
        /// Játékos otthon biztosítása
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <returns></returns>
        public Boolean PlayerHouseInsurance(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            return _playerList[playerNum].houseInsurance;
        }

        /// <summary>
        /// Játékos életkártyáinak száma
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        public Int32 PlayerLifeCardNumber(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            return _playerList[playerNum].lifeCardNumber;
        }

        /// <summary>
        /// Játékos kölcsönének összege
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        public Int32 PlayerLoan(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            return _playerList[playerNum].loan;
        }

        /// <summary>
        /// Játékos helye a táblán
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <returns>Mező száma</returns>
        public Int32 PlayerLocation(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            return _playerList[playerNum].location;
        }

        /// <summary>
        /// Játékos házas-e
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        public Boolean PlayerMarried(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            return _playerList[playerNum].married;
        }

        /// <summary>
        /// Játékos pénze
        /// </summary>
        /// <param name="playerNum">Játékos sz áma</param>
        public Int32 PlayerMoney(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            return _playerList[playerNum].money;
        }

        /// <summary>
        /// Játékos neve
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        public String PlayerName(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            return _playerList[playerNum].name;
        }

        /// <summary>
        /// Játékos fizetése
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        public Int32 PlayerSalary(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            if (_playerList[playerNum].salaryCard == 9)
                return 0;
            return _moneyForSalaryCards[_playerList[playerNum].salaryCard];
        }

        /// <summary>
        /// A játékos fizetéséhez tartozó adó értéke
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        public Int32 PlayerTax(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            if (_playerList[playerNum].salaryCard == 9)
                throw new ArgumentException("A játékosnak nincs fizetése");
            return _taxForSalaryCards[_playerList[playerNum].salaryCard];
        }

        /// <summary>
        /// Játékos részvényének száma
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        public Int32 PlayerStockCard(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            return _playerList[playerNum].stockCard;
        }

        public Boolean GetStockCardAvailability(Int32 stockcard)
        {
            if (stockcard >= 9)
                throw new ArgumentException("A részvény száma hibás", "stockcard");
            return _remainedStockCards.Contains(stockcard);
        }

        /// <summary>
        /// AI-e a játékos
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        public Boolean PlayerPc(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            return _playerList[playerNum].pc;
        }

        /// <summary>
        /// Kimarad-e a játékos az aktuális körből
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        public Boolean PlayerLoseNextRound(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            return _playerList[playerNum].loseNextRound;
        }

        /// <summary>
        /// Nyugdíjas-e
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <returns><c>true</c> A játékos nyugdíjas. <c>false</c> A járékos még nem ment nyugdíjba.</returns>
        public bool IsRetired(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            return (_playerList[playerNum].retired != 0);
        }

        #endregion

        #region Game Effects

        /// <summary>
        /// Járműbiztosítás vétele
        /// </summary>
        /// <param name="playerNum">Az aktuális játékos száma</param>
        /// <returns><c>true</c>, ha minden helyes volt. <c>false</c>, ha nincs elég pénze.</returns>
        public Boolean BuyCarInsurance(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            if (_playerList[playerNum].carInsurance)
                throw new Exception("A játékos már kötött biztosítást.");
            if (_playerList[playerNum].money < 10000)
                return false;
            _playerList[playerNum].money -= 10000;
            _playerList[playerNum].carInsurance = true;
            return true;
        }

        /// <summary>
        /// Otthon biztosítás vétele
        /// </summary>
        /// <param name="playerNum">Az aktuális játékos száma</param>
        /// <returns><c>true</c>, ha minden helyes volt. <c>false</c>, ha nincs elég pénze.</returns>
        public Boolean BuyHouseInsurance(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma!", "playerNum");
            if (_playerList[playerNum].houseInsurance)
                throw new Exception("A játékos már kötött biztosítást.");
            if (_playerList[playerNum].houseCard == 9)
                throw new Exception("Az aktuális játékosnak nincs még háza!");
            if (_playerList[playerNum].money < _insuranceForHouseCards[_playerList[playerNum].houseCard])
                return false;
            _playerList[playerNum].money -= _insuranceForHouseCards[_playerList[playerNum].houseCard];
            _playerList[playerNum].houseInsurance = true;
            return true;
        }

        /// <summary>
        /// Részvény vásárlás
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <param name="stockNum">Részvény száma</param>
        public Boolean BuyStock(Int32 playerNum, Int32 stockNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            if (!_remainedStockCards.Contains(stockNum))
                throw new ArgumentException("A megadott részvényt már birtokolja valaki, vagy nem megfelelő a részvény száma!", "stockNum");
            if (_playerList[playerNum].stockCard != 9)
                throw new ArgumentException("A megadott játékos már rendelkezik részvénnyel!", "stockNum");
            if (_playerList[playerNum].money < 50000)
                return false;
            _playerList[playerNum].stockCard = stockNum;
            _remainedStockCards.Remove(stockNum);
            _playerList[playerNum].money -= 50000;
            return true;
        }

        /// <summary>
        /// A két játékos fizetést cserél
        /// </summary>
        /// <param name="playerNum1">Egyik játékos száma</param>
        /// <param name="playerNum2">Másik játékos száma</param>
        public void TradeSalary(Int32 playerNum1, Int32 playerNum2)
        {
            if (playerNum1 >= _playerList.Length)
                throw new ArgumentException("Az első játékos száma nagyobb, mint a játékosok száma", "playerNum1");
            if (playerNum2 >= _playerList.Length)
                throw new ArgumentException("Az második játékos száma nagyobb, mint a játékosok száma", "playerNum2");
            if (_playerList[playerNum1].salaryCard == 9 || _playerList[playerNum2].salaryCard == 9)
                throw new ArgumentException("A megadott játékosok valamelyikének még nincs fizetése!");
            Int32 temp = _playerList[playerNum1].salaryCard;
            _playerList[playerNum1].salaryCard = _playerList[playerNum2].salaryCard;
            _playerList[playerNum2].salaryCard = temp;
        }

        /// <summary>
        /// Karrier keresése
        /// </summary>
        /// <param name="careerNum">Karrier kártya száma</param>
        /// <returns><c>-1</c>, ha nem birtokolja senki az állást. Míg ha valaki birtokolja, visszaadja a játékos számát.</returns>
        public Int32 ExistCareer(Int32 careerNum)
        {
            if (careerNum > 8 || careerNum < 0)
                throw new ArgumentException("A megadott munka nem létezik!", "careerNum");
            if (_remainedCareerCards.Contains(careerNum))
                return -1;
            Int32 temp = 0;
            foreach (Player player in _playerList)
            {
                if (player.careerCard == careerNum)
                    return temp;
                ++temp;
            }
            throw new Exception("Hiba történt a karrier keresése közben!");
        }

        /// <summary>
        /// Megadja a fizetés összegét egy adott fizetés kártyához
        /// </summary>
        /// <param name="cardnum">Fizetés kártya száma</param>
        /// <returns>Fizetés</returns>
        public Int32 GetMoneyForSalaryCard(Int32 cardnum)
        {
            return _moneyForSalaryCards[cardnum];
        }

        /// <summary>
        /// Megadja a biztosítás összegét egy adott birtoklevélhez
        /// </summary>
        /// <param name="cardnum">Birtoklevél száma</param>
        /// <returns>Fizetés</returns>
        public Int32 GetInsuranceForHouseCard(Int32 cardnum)
        {
            return _insuranceForHouseCards[cardnum];
        }

        /// <summary>
        /// Részvény ajándékba
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <returns><c>0</c>, ha a részvényt sikeresen megkapta a játékos. <c>1</c>, ha a játékos már rendelkezett részvénnyel.
        /// <c>2</c>, ha már nincs szabad részvény.</returns>
        public Int32 FreeStock(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            if (_playerList[playerNum].stockCard != 9)
                return 1;
            if (_remainedStockCards.Count == 0)
                return 2;
            Random rnd = new Random();
            Int32 r = rnd.Next(_remainedStockCards.Count);
            Int32 stockNum = _remainedStockCards[r];
            _playerList[playerNum].money += 50000;
            BuyStock(playerNum, stockNum);
            return 0;
        }

        /// <summary>
        /// Életzseton kiosztása
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <returns><c>true</c>, ha a játékos sikeresen megkapta az életzsetonját. <c>false</c>, ha már nem maradt életzseton.</returns>
        public Boolean GetLifeCard(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            if (_remainedLifeCards == 0)
                return false;
            ++_playerList[playerNum].lifeCardNumber;
            --_remainedLifeCards;
            return true;
        }

        /// <summary>
        /// Kölcsön felvétele
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <param name="loan">Kölcsön összege</param>
        public void GetLoan(Int32 playerNum, Int32 loan)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            _playerList[playerNum].money += loan;
            _playerList[playerNum].loan += loan;
        }

        /// <summary>
        /// Diákhitel
        /// </summary>
        /// <param name="playerNum">Játékos neve</param>
        public void GetStudentLoan(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            _playerList[playerNum].loan += 50000;
        }

        /// <summary>
        /// Játékos házasságkötése
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        public void Marry(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            if (_playerList[playerNum].married)
                throw new ArgumentException("A játékos már házas", "playerNum");
            _playerList[playerNum].married = true;
        }

        /// <summary>
        /// Játékosnak pénz adása
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <param name="sum">Pénzösszeg</param>
        public void GiveMoney(Int32 playerNum, Int32 sum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            if (sum < 0)
                throw new ArgumentException("A megadott összeg hibás", "sum");
            _playerList[playerNum].money += sum;
        }

        /// <summary>
        /// Első-e a játékos a pályán.
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <returns><c>true</c>, ha ez a játékos az első. <c>false</c>, ha nem ő az első.</returns>
        public Boolean IsFirst(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            Int32 max = _playerList[0].location;
            Int32 maxPlayer = 0;
            Int32 temp = 0;
            foreach (Player player in _playerList)
            {
                if (player.location > max || player.location == max && temp == playerNum)
                {
                    max = player.location;
                    maxPlayer = temp;
                }
                ++temp;
            }
            return (maxPlayer == playerNum);
        }

        /// <summary>
        /// A játékos kimarad a következő körből
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <param name="value">Az loseNextRound értéke (igaz, vagy hamis).</param>
        public void SetLoseNextRound(Int32 playerNum, Boolean value)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            _playerList[playerNum].loseNextRound = value;
        }

        /// <summary>
        /// Részvény elvesztése
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <returns><c>true</c>, ha a játékos részvénye elveszett. <c>false</c>, ha a játékosnak nem is volt részvénye.</returns>
        public Boolean LoseStock(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            if (_playerList[playerNum].stockCard == 9)
                return false;
            _remainedStockCards.Add(_playerList[playerNum].stockCard);
            _playerList[playerNum].stockCard = 9;
            return true;
        }

        /// <summary>
        /// A játékos egy karrier kártyát húz
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <returns>A húzott karrier kártya száma.</returns>
        public Int32 GiveCareer(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            if (_remainedCareerCards.Count == 0)
                throw new Exception("Nincs több karrier kártya");
            Random rnd = new Random();
            Int32 r = rnd.Next(_remainedCareerCards.Count);
            Int32 careerCard = _remainedCareerCards[r];

            while (!_playerList[playerNum].deg && careerCard >= 6) //amíg olyat kapunk, amihez diploma kell és nincs diplománk
            {
                r = rnd.Next(_remainedCareerCards.Count);
                careerCard = _remainedCareerCards[r];
            }

            if (_playerList[playerNum].careerCard != 9)
                _remainedCareerCards.Add(_playerList[playerNum].careerCard);
            _playerList[playerNum].careerCard = careerCard;
            _remainedCareerCards.Remove(careerCard);
            return careerCard;
        }

        /// <summary>
        /// A játékos egy birtoklevelet húz
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <returns>A húzott birtoklevél száma.</returns>
        public Int32 GiveHouse(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            if (_remainedHouseCards.Count == 0)
                throw new Exception("Nincs több birtoklevél");
            Random rnd = new Random();
            Int32 r = rnd.Next(_remainedHouseCards.Count);
            Int32 houseCard = _remainedHouseCards[r];
            _playerList[playerNum].houseCard = houseCard;
            PayMoney(playerNum, _costForHouseCards[houseCard]);
            _remainedHouseCards.Remove(houseCard);
            return houseCard;
        }

        /// <summary>
        /// Életzseton szerzése bármely játékostól
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <returns>Játékos száma, akitől az életzsetont elvettük.</returns>
        public Int32 StealLifeCard(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            if (_remainedLifeCards != 0)
                throw new Exception("Még van élet kártya a pakliban");
            Random rnd = new Random();
            Int32 r = rnd.Next(_remainedPlayers.Count);
            Int32 otherPlayer = _remainedPlayers[r];
            while (otherPlayer == playerNum || _playerList[otherPlayer].lifeCardNumber == 0)
            {
                r = rnd.Next(_remainedPlayers.Count);
                otherPlayer = _remainedPlayers[r];
            }
            --_playerList[otherPlayer].lifeCardNumber;
            ++_playerList[playerNum].lifeCardNumber;
            return otherPlayer;
        }

        /// <summary>
        /// A játékos egy fizetés kártyát húz
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <returns>A húzott fizetés kártyához tartozó fizetés összege.</returns>
        public Int32 GiveSalary(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            if (_remainedSalaryCards.Count == 0)
                throw new Exception("Nincs több karrier kártya");
            Random rnd = new Random();
            Int32 r = rnd.Next(_remainedSalaryCards.Count);
            Int32 salaryCard = _remainedSalaryCards[r];
            _playerList[playerNum].salaryCard = salaryCard;
            _remainedSalaryCards.Remove(salaryCard);
            return _moneyForSalaryCards[salaryCard];
        }

        /// <summary>
        /// Randomol három különböző elemet a listából.
        /// </summary>
        /// <param name="remainedList"></param>
        /// <returns>A 3 random elem.</returns>
        private List<Int32> GiveSome(Int32 some, List<Int32> remainedList)
        {
            List<Int32> temp = new List<int>(remainedList);
            List<Int32> result = new List<int>();
            Random rnd = new Random();
            for (int i = 0; i < some; i++)
            {
                Int32 r = rnd.Next(temp.Count);
                result.Add(temp[r]);
                temp.Remove(result[i]);
            }
            return result;
        }

        /// <summary>
        /// Három karrier kártya választása
        /// </summary>
        /// <returns>A három karrier kártya száma</returns>
        public List<Int32> GiveThreeCareer(Int32 playernum = -1)
        {
            if (playernum == -1)
                return GiveSome(3, _remainedCareerCards);
            List<Int32> threeList = GiveSome(2, _remainedCareerCards);
            threeList.Add(_playerList[playernum].careerCard);
            return threeList;
        }

        /// <summary>
        /// Három fizetés kártya választása
        /// </summary>
        /// <returns>A három fizetés kártya száma</returns>
        public List<Int32> GiveThreeSalary(Int32 playernum = -1)
        {
            if (playernum == -1)
                return GiveSome(3, _remainedSalaryCards);
            List<Int32> threeList = GiveSome(2, _remainedSalaryCards);
            threeList.Add(_playerList[playernum].salaryCard);
            return threeList;
        }

        /// <summary>
        /// Egy gyerek született
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <param name="isGirl">A gyerek lány-e</param>
        /// <returns><c>true</c>, ha a gyerek sikeresen a családba került. <c>false</c>, ha a kocsiban nincs szabad hely.</returns>
        public Boolean OneChild(Int32 playerNum, Boolean isGirl)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            if (_playerList[playerNum].childrenNumber > 5)
                return false;
            if (isGirl)
                _playerList[playerNum].children.Add(0);
            else
                _playerList[playerNum].children.Add(1);
            ++_playerList[playerNum].childrenNumber;
            return true;
        }

        /// <summary>
        /// Kölcsön visszafizetése
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <param name="sum">Az összeg amennyit vissza szeretne fizetni.</param>
        /// <returns><c>true</c>, ha a kölcsönt sikeresen visszafizette. <c>false</c>, ha a játékosnak nincs elég pénze a visszafizetéshez.</returns>
        public Boolean PayBackLoan(Int32 playerNum, Int32 sum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            if (_playerList[playerNum].loan < sum)
                throw new Exception("A játékosnak nincsen ennyi kölcsöne.");
            if (sum > _playerList[playerNum].money)
                return false;
            _playerList[playerNum].money -= sum;
            _playerList[playerNum].loan -= sum;
            return true;
        }

        /// <summary>
        /// Fizetésnap
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        public void PayDay(Int32 playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            _playerList[playerNum].money += _moneyForSalaryCards[_playerList[playerNum].salaryCard];
        }

        /// <summary>
        /// Játékos fizet
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <param name="sum">Pénzösszeg</param>
        /// <returns><c>true</c>, a fizetés sikeres volt. <c>false</c>, ha a játékosnak nem volt elég pénze.</returns>
        public Boolean PayMoney(Int32 playerNum, Int32 sum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            if (sum < 0)
                throw new ArgumentException("A pénzösszeg rosszul van megadva", "sum");

            while (_playerList[playerNum].money < sum)
            {
                _playerList[playerNum].money += 20000;
                _playerList[playerNum].loan += 25000;
            }
            _playerList[playerNum].money -= sum;
            return true;
        }

        /// <summary>
        /// Egy játékos fizet, és ha létezik a carrierrel rendelkező játékos, akkor neki.
        /// </summary>
        /// <param name="sum">Pénzösszeg</param>
        /// <param name="payer">Fizető száma</param>
        /// <param name="careerCard">Karrier száma</param>
        /// <returns></returns>
        public Boolean PayForSomeone(Int32 sum, Int32 payer, Int32 careerCard)
        {
            while (_playerList[payer].money < sum)
            {
                _playerList[payer].money += 20000;
                _playerList[payer].money += 25000;
            }
            Int32 owner = -1;
            try
            {
                owner = ExistCareer(careerCard);
            }
            catch (Exception)
            {
                owner = -1;
            }
            if (owner != -1)
            {
                _playerList[owner].money += sum;
                _playerList[payer].money -= sum;
                return true;
            }
            _playerList[payer].money -= sum;
            return false;
        }

        /// <summary>
        /// Nyugdíjba menés
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <param name="toRich">A Gazdagok nyaralójába szeretne-e menni.</param>
        public void Retire(Int32 playerNum, Boolean toRich)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            if (_playerList[playerNum].retired != 0)
                throw new Exception("A játékos már nyugdíjas.");
            if (toRich)
                _playerList[playerNum].retired = 2;
            else
                _playerList[playerNum].retired = 1;
            _remainedPlayers.Remove(playerNum);
        }

        /// <summary>
        /// Karrier választása
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <param name="careerNum">Karrier kártya száma</param>
        public void SetCareer(Int32 playerNum, Int32 careerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            if (_playerList[playerNum].careerCard != 9)
                _remainedCareerCards.Add(_playerList[playerNum].careerCard);
            if (!_remainedCareerCards.Contains(careerNum))
                throw new Exception("A karrier kártya már 'foglalt'");
            _playerList[playerNum].careerCard = careerNum;
            _remainedCareerCards.Remove(careerNum);
        }

        /// <summary>
        /// Fizetés választása
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <param name="salaryNum">Fizetés kártya száma</param>
        public void SetSalary(Int32 playerNum, Int32 salaryNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");

            if (_playerList[playerNum].salaryCard != 9)
                _remainedSalaryCards.Add(_playerList[playerNum].salaryCard);
            if (!_remainedSalaryCards.Contains(salaryNum))
                throw new Exception("A fizetés kártya már 'foglalt'");
            _playerList[playerNum].salaryCard = salaryNum;
            _remainedSalaryCards.Remove(salaryNum);
        }

        /// <summary>
        /// Ikrek születése
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <returns>Első értéke azt mutatja, hogy volt-e elég hely az autóban. Második értéke a nemeket tartalmazó tömb: <c>0</c> esetén lány, <c>1</c> esetén fiú.</returns>
        public Tuple<Boolean, Int32[]> TwoChildren(int playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            Int32[] twins = new int[2];
            Random rnd = new Random();
            for (int i = 0; i < 2; i++)
            {
                Int32 r = rnd.Next(0, 1);
                twins[i] = r;
                ++_playerList[playerNum].childrenNumber;
                _playerList[playerNum].children.Add(r);
            }
            return new Tuple<bool, int[]>(true, twins);
        }

        /// <summary>
        /// A játékos megszerzi a diplomát.
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        public void TakeDegree(int playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            if (_playerList[playerNum].deg)
                throw new Exception("A játékos már egyszer ledipomázott.");
            _playerList[playerNum].deg = true;
        }

        /// <summary>
        /// Pörgetés
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <returns>Null-lal tér vissza, ha a játékos kimarad a körből. Egyébként: Első érték a pörgetett szám. Második érték a játékos száma, aki a megfelelő részvényt birtokolja</returns>
        public Tuple<Int32, Int32> Spin(int playerNum)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            Random rnd = new Random();
            Int32 steps = rnd.Next(1, 11);
            Int32 owner = -1;
            Boolean police = false;
            if (steps == 10)
            {
                try
                {
                    
                    owner = ExistCareer(5);
                    police = (owner != -1);
                }
                catch (Exception)
                {
                    owner = -1;
                }
            }
            else
            {
                try
                {
                    if (_remainedStockCards.Contains(steps - 1))
                    {
                        owner = -1;
                    }
                    else
                    {
                        Int32 temp = 0;
                        foreach (Player player in _playerList)
                        {
                            if (player.stockCard == (steps - 1))
                            {
                                owner = temp;
                            }
                            ++temp;
                        }
                    }
                }
                catch (Exception)
                {
                    owner = -1;
                }
            }
            if (owner != -1)
                _playerList[owner].money += 10000;
            if (police)
                _playerList[playerNum].money -= 10000;

            return new Tuple<int, int>(steps, owner);
        }

        /// <summary>
        /// Mező megadása a játékoshoz
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <param name="location">Mező száma</param>
        public void SetPlayerLocation(Int32 playerNum, Int32 location)
        {
            if (playerNum >= _playerList.Length)
                throw new ArgumentException("Az aktuális játékos száma nagyobb, mint a játékosok száma", "playerNum");
            if (location < 0 || location > 151)
                throw new ArgumentException("A megadott lépés-szám hibás", "location");
            _playerList[playerNum].location = location;
        }

        /// <summary>
        /// Az aktuális játékost lépteti.
        /// </summary>
        public void NextPlayer()
        {
            _actualPlayer = (_actualPlayer + 1) % _playerNumber;
        }

        /// <summary>
        /// Save game
        /// </summary>
        /// <param name="filename">Fájl neve</param>
        /// <param name="sure">Biztosan rá szeretné e menteni?</param>
        /// <returns><c>0</c>, ha a mentés sikeres volt. <c>1</c>, ha a file már létezik. <c>2</c>, ha a mentés sikertelen volt.</returns>
        public Int32 Save(String filename, Boolean sure)
        {
            if (File.Exists(filename) && !sure)
                return 1;
            try
            {
                Stream stream = File.Open(filename, FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, this);
                stream.Close();
            }
            catch (Exception)
            {
                return 2;
            }
            return 0;
        }

        public void CalculateWinnerOfVillaPrize()
        {
            for (int i = 0; i < _playerNumber; i++)
            {
                if (_playerList[i].loan > 0)
                    _playerList[i].money -= _playerList[i].loan;
            }
            Int32 temp = 0;
            Int32 maxMoney;
            List<Int32> maxPlayer = new List<int>();
            while (temp < _playerNumber && _playerList[temp].retired != 2)
            {
                ++temp;
            }
            if (temp < _playerNumber)
            {
                maxPlayer.Add(temp);
                maxMoney = _playerList[temp].money;

                for (int i = (temp+1); i < _playerNumber; i++)
                {
                    if (_playerList[i].retired == 2)
                    {
                        if (_playerList[i].money > maxMoney)
                        {
                            maxMoney = _playerList[i].money;
                            maxPlayer.Clear();
                            maxPlayer.Add(i);
                        }
                        else
                        {
                            if (_playerList[i].money == maxMoney)
                            {
                                maxPlayer.Add(i);
                            }
                        }
                    }
                }

                foreach (int max in maxPlayer)
                {
                    _playerList[max].lifeCardNumber += 4 / maxPlayer.Count;
                }
            }
        }

        public Tuple<String, Int32> RevealLifeCard(Int32 playerNum)
        {
            if (_playerList[playerNum].lifeCardNumber <= 0)
            {
                return new Tuple<String, Int32>("", 0);
            }
            else
            {
                Random rnd = new Random();
                Int32 r, rmoney;
                String award;

                r = rnd.Next(_rewardForLifeCards.Count);

                rmoney = _rewardForLifeCards[r];
                award = _awardForLifeCards[r];

                _rewardForLifeCards.Remove(rmoney);
                _awardForLifeCards.Remove(award);

                _playerList[playerNum].money += rmoney;
                --_playerList[playerNum].lifeCardNumber;
                return new Tuple<String, Int32>(award, rmoney);
            }
        }
        
        public List<Int32> EndGame()
        {
            List<Int32> winners = new List<int>();
            Int32 winnerMoney = _playerList[0].money;
            for (int i = 0; i < _playerNumber; i++)
            {
                if (_playerList[i].money > winnerMoney)
                {
                    winners.Clear();
                    winners.Add(i);
                    winnerMoney = _playerList[i].money;
                }
                else
                {
                    if (_playerList[i].money == winnerMoney)
                        winners.Add(i);
                }
            }
            return winners;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="playerNum">Játékos száma</param>
        /// <param name="playerDataList">Játékos neve, Nő-e, AI-e</param>
        public DataModel(Int32 playerNum, List<Tuple<String, Boolean, Boolean>> playerDataList)
        {
            if (playerNum < 2 || playerNum > 6)
                throw new ArgumentException("A megadott játékos-szám hibás", "playerNum");
            if (playerDataList.Count != playerNum)
                throw new ArgumentException("A játékosok adatainak megadása hibás");
            _playerList = new Player[playerNum];
            for (int i = 0; i < playerNum; i++)
            {
                _playerList[i] = new Player(playerDataList[i].Item1, playerDataList[i].Item2, playerDataList[i].Item3);
            }
            _playerNumber = playerNum;
            _remainedCareerCards = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            _remainedHouseCards = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            _remainedLifeCards = 21;
            _remainedSalaryCards = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            _remainedStockCards = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            _remainedPlayers = new List<int>();
            for (int i = 0; i < playerNum; i++)
                _remainedPlayers.Add(i);
            _rewardForLifeCards = new List<int>();
            for (int i = 0; i < 25; i++)
            {
                if (i < 7)
                    _rewardForLifeCards.Add(50000);
                else if (i < 13)
                    _rewardForLifeCards.Add(100000);
                else if (i < 18)
                    _rewardForLifeCards.Add(150000);
                else if (i < 22)
                    _rewardForLifeCards.Add(200000);
                else
                    _rewardForLifeCards.Add(250000);
            }

            _awardForLifeCards = new List<String>();

            _awardForLifeCards.Add("Megnyered a táncversenyt");
            _awardForLifeCards.Add("Feltaláltad a zsírmentes fánkot");
            _awardForLifeCards.Add("Védett területet hozol létre a delfinek számára");
            _awardForLifeCards.Add("Átúszod a La Manche-csatornát");
            _awardForLifeCards.Add("Megmászod a Mount Everest-et");
            _awardForLifeCards.Add("Új sportágat találsz ki");
            _awardForLifeCards.Add("Világrekordot futsz");
            _awardForLifeCards.Add("Új számítógépet tervezel");
            _awardForLifeCards.Add("Tiszteletbeli professzori címet kapsz");
            _awardForLifeCards.Add("Életmű-díjat kapsz");
            _awardForLifeCards.Add("Egészséges ételeket forgalmazó üzletláncot nyitsz");
            _awardForLifeCards.Add("Humanitárius díj");
            _awardForLifeCards.Add("Új bolygót fedezel fel");
            _awardForLifeCards.Add("Remekművet festesz");
            _awardForLifeCards.Add("Az általad kitalált játék jó forgalmat ér el");
            _awardForLifeCards.Add("Nagyszerű regényt írsz");
            _awardForLifeCards.Add("Szimfóniát komponálsz");
            _awardForLifeCards.Add("A család lova megnyeri a versenyt");
            _awardForLifeCards.Add("Új tanítási módszert találsz ki");
            _awardForLifeCards.Add("Gyógyírt találsz a náthára");
            _awardForLifeCards.Add("Új energiaforrást találsz");
            _awardForLifeCards.Add("Megmentetted a veszélyeztetett fajokat");
            _awardForLifeCards.Add("Nobel Békedíjat kapsz");
            _awardForLifeCards.Add("Miniszterelnök leszel");
            _awardForLifeCards.Add("Megoldod a környezetszennyezés kérdését");

        }

        /// <summary>
        /// Konstruktor fájlból
        /// </summary>
        /// <param name="filename">Fájl neve</param>
        public DataModel(String filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException();

            Stream stream = File.Open(filename, FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();
            DataModel fromFile = (DataModel)bformatter.Deserialize(stream);
            stream.Close();

            _playerList = fromFile._playerList;
            _playerNumber = fromFile._playerNumber;
            _remainedCareerCards = fromFile._remainedCareerCards;
            _remainedHouseCards = fromFile._remainedHouseCards;
            _remainedLifeCards = fromFile._remainedLifeCards;
            _remainedSalaryCards = fromFile._remainedSalaryCards;
            _remainedStockCards = fromFile._remainedStockCards;
            _remainedPlayers = fromFile._remainedPlayers;
            _actualPlayer = fromFile._actualPlayer;
            _rewardForLifeCards = fromFile._rewardForLifeCards;
            _awardForLifeCards = fromFile._awardForLifeCards;
        }

        #endregion
    }
}
