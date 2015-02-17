using System;
using System.Collections.Generic;
using GameOfLife.DataModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataModelUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        public DataModel model;

        private void CreatePlayers()
        {
            List<Tuple<string, bool, bool>> players = new List<Tuple<string, bool, bool>>();
            players.Add(new Tuple<string, bool, bool>("Juli", true, false));
            players.Add(new Tuple<string, bool, bool>("Sanyi", false, false));
            players.Add(new Tuple<string, bool, bool>("Zoli", false, false));
            players.Add(new Tuple<string, bool, bool>("Amanda", true, true));
            model = new DataModel(4, players);
        }

        [TestMethod]
        public void CarInsurance_False()
        {
            CreatePlayers();
            Assert.IsFalse(model.BuyCarInsurance(1));
        }
        
        [TestMethod]
        public void CarInsurance_True()
        {
            CreatePlayers();
            model.GiveMoney(1, 20000);
            Assert.IsTrue(model.BuyCarInsurance(1));
            Assert.AreEqual(model.PlayerMoney(1), 10000);
            Assert.IsTrue(model.PlayerCarInsurance(1));
        }
        
        [TestMethod]
        public void CarInsurance_Money()
        {
            CreatePlayers();
            model.GiveMoney(1, 20000);
            model.BuyCarInsurance(1);
            Assert.AreEqual(model.PlayerMoney(1), 10000);
        }

        [TestMethod]
        public void CarInsurance_HasCarInsurance()
        {
            CreatePlayers();
            model.GiveMoney(1, 20000);
            model.BuyCarInsurance(1);
            Assert.IsTrue(model.PlayerCarInsurance(1));
        }

        [TestMethod]
        public void GiveHouse()
        {
            CreatePlayers();
            model.GiveMoney(1, 40000);
            model.GiveHouse(1);
            Assert.AreNotEqual(9, model.PlayerHouseCard(1));
        }

        [TestMethod]
        public void HouseInsurance()
        {
            CreatePlayers();
            model.GiveMoney(1, 1000000);
            model.GiveHouse(1);
            Assert.IsTrue(model.BuyHouseInsurance(1));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void HouseInsurance_Exception_Nohouse()
        {
            CreatePlayers();
            model.BuyHouseInsurance(1);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void HouseInsurance_Exception_HasIns()
        {
            CreatePlayers();
            model.GiveMoney(1, 1000000);
            model.GiveHouse(1);
            model.BuyHouseInsurance(1);
            model.BuyHouseInsurance(1);
        }

        [TestMethod]
        public void BuyStock()
        {
            CreatePlayers();
            model.GiveMoney(1, 50000);
            Assert.IsTrue(model.BuyStock(1, 5));
        }

        [TestMethod]
        public void FreeStock()
        {
            CreatePlayers();
            model.GiveMoney(1, 50000);
            model.FreeStock(1);
            Assert.AreNotEqual(9, model.PlayerStockCard(1));
            Assert.AreEqual(50000, model.PlayerMoney(1));
        }

        [TestMethod]
        public void GiveSalary()
        {
            CreatePlayers();
            model.GiveSalary(1);
            Assert.AreNotEqual(0, model.PlayerSalary(1));
        }

        [TestMethod]
        public void TradeSalary()
        {
            CreatePlayers();
            Int32 salary0 = model.GiveSalary(0);
            Int32 salary1 = model.GiveSalary(1);
            model.TradeSalary(0, 1);
            Assert.AreEqual(salary1, model.PlayerSalary(0));
            Assert.AreEqual(salary0, model.PlayerSalary(1));
        }

        [TestMethod]
        public void ExistCareer()
        {
            CreatePlayers();
            Int32 card = model.GiveCareer(1);
            Assert.AreEqual(1, model.ExistCareer(card));
        }

        [TestMethod]
        public void GetLifeCard()
        {
            CreatePlayers();
            model.GetLifeCard(1);
            Assert.AreEqual(1, model.PlayerLifeCardNumber(1));
            model.GetLifeCard(1);
            Assert.AreEqual(2, model.PlayerLifeCardNumber(1));
        }

        [TestMethod]
        public void GetLoan()
        {
            CreatePlayers();
            model.GetLoan(1, 5000);
            Assert.AreEqual(5000, model.PlayerMoney(1));
            Assert.AreEqual(5000, model.PlayerLoan(1));
        }

        [TestMethod]
        public void GetStudentLoan()
        {
            CreatePlayers();
            model.GetStudentLoan(1);
            Assert.AreEqual(0, model.PlayerMoney(1));
            Assert.AreEqual(40000, model.PlayerLoan(1));
        }

        [TestMethod]
        public void Marry()
        {
            CreatePlayers();
            model.Marry(1);
            Assert.IsTrue(model.PlayerMarried(1));
        }

        [TestMethod]
        public void GiveMoney()
        {
            CreatePlayers();
            model.GiveMoney(1, 5000);
            Assert.AreEqual(5000, model.PlayerMoney(1));
        }

        [TestMethod]
        public void IsFirst_roundzero()
        {
            CreatePlayers();
            Assert.IsTrue(model.IsFirst(1));
        }

        [TestMethod]
        public void IsFirst()
        {
            CreatePlayers();
            model.SetPlayerLocation(1, 10);
            Assert.IsTrue(model.IsFirst(1));
            Assert.IsFalse(model.IsFirst(0));
        }

        [TestMethod]
        public void SetLoseNextRound()
        {
            CreatePlayers();
            model.SetLoseNextRound(1, true);
            Assert.IsTrue(model.PlayerLoseNextRound(1));
        }

        [TestMethod]
        public void LoseStock()
        {
            CreatePlayers();
            model.FreeStock(1);
            Assert.AreNotEqual(9, model.PlayerStockCard(1));
            model.LoseStock(1);
            Assert.AreEqual(9, model.PlayerStockCard(1));
        }

        [TestMethod]
        public void GiveCareer()
        {
            CreatePlayers();
            Int32 career = model.GiveCareer(1);
            Assert.AreNotEqual(9, model.PlayerCareerCard(1));
            Assert.AreEqual(1, model.ExistCareer(career));
        }

        [TestMethod]
        public void OneChild()
        {
            CreatePlayers();
            model.OneChild(1, true);
            Assert.AreEqual(model.PlayerChildrenNumber(1), 1);
        }

        [TestMethod]
        public void PayBackLoan_True()
        {
            CreatePlayers();
            model.GetLoan(1, 2000);
            Assert.IsTrue(model.PayBackLoan(1, 1000));
            Assert.AreEqual(1000, model.PlayerMoney(1));
            Assert.AreEqual(1000, model.PlayerLoan(1));
        }

        [TestMethod]
        public void PayBackLoan_False()
        {
            CreatePlayers();
            model.GetLoan(1, 1000);
            model.PayMoney(1, 1000);
            Assert.IsFalse(model.PayBackLoan(1, 1000));
        }

        [TestMethod]
        public void PayDay()
        {
            CreatePlayers();
            Int32 money = model.GiveSalary(1);
            model.PayDay(1);
            Assert.AreEqual(money, model.PlayerMoney(1));
        }

        [TestMethod]
        public void PayMoney_True()
        {
            CreatePlayers();
            model.GiveMoney(1, 1000);
            Assert.IsTrue(model.PayMoney(1, 1000));
            Assert.AreEqual(0, model.PlayerMoney(1));
        }

        [TestMethod]
        public void PayMoney_False()
        {
            CreatePlayers();
            Assert.IsFalse(model.PayMoney(1, 1000));
        }

        [TestMethod]
        public void StealLifeCard()
        {
            CreatePlayers();
            model.ActualPlayer = 3;
            model.NextPlayer();
            Assert.AreEqual(model.ActualPlayer, 0);
        }
        
        [TestMethod]
        public void StockCardAvailability()
        {
            CreatePlayers();
            model.GiveMoney(1, 100000);
            Assert.IsTrue(model.GetStockCardAvailability(1));
            model.BuyStock(1, 1);
            Assert.IsFalse(model.GetStockCardAvailability(1));
        }
        
        [TestMethod]
        public void Retire()
        {
            CreatePlayers();
            Assert.IsFalse(model.IsRetired(1));
            model.Retire(1,true);
            Assert.IsTrue(model.IsRetired(1));
        }

        [TestMethod]
        public void Degree()
        {
            CreatePlayers();
            Assert.IsFalse(model.PlayerDegree(1));
            model.TakeDegree(1);
            Assert.IsTrue(model.PlayerDegree(1));
        }

        [TestMethod]
        public void TwoChildren()
        {
            CreatePlayers();
            model.TwoChildren(1);
            Assert.AreEqual(2, model.PlayerChildrenNumber(1));
        }

        [TestMethod]
        public void Save()
        {
            CreatePlayers();
            model.TakeDegree(1);
            model.OneChild(1, true);
            model.GiveMoney(0,100);
            model.GiveMoney(1,10000);
            model.GiveCareer(0);
            model.GetLoan(0,10000);
            model.Save("./blabla", true);
        }

        [TestMethod]
        public void Load()
        {
            model = new DataModel("./blabla");
            Assert.IsTrue(model.PlayerDegree(1));
            Assert.AreEqual(model.PlayerChildrenNumber(1),1);
            Assert.AreEqual(model.PlayerMoney(0), 10100);
            Assert.AreEqual(model.PlayerMoney(1), 10000);
            Assert.AreEqual(model.PlayerLoan(0), 10000);
            Assert.AreNotEqual(model.PlayerCareerCard(0), 9);
            Assert.AreEqual(model.PlayerCareerCard(1),9);
        }
    }
}
