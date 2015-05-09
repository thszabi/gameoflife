using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameOfLife
{
    class MiniGameModel
    {
        private List<List<Int16>> table;    //A táblát tartalmazó listák listája

        //A két játékost tartalmazó lista:
        //0. elem = 1. játékos (üldözött)
        //1. elem = 2. játékos (üldöző)
        private List<Point> players;

        public enum Direction { UP, RIGHT, DOWN, LEFT };    //Ezzel a felsorolással adható meg, milyen irányba akar menni a játékos

        public MiniGameModel()
        {
            // 0 = padló
            // 1 = fal
            List<Int16> sor0  = new List<Int16> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            List<Int16> sor1  = new List<Int16> { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
            List<Int16> sor2  = new List<Int16> { 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 0, 1 };
            List<Int16> sor3  = new List<Int16> { 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1 };
            List<Int16> sor4  = new List<Int16> { 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1 };
            List<Int16> sor5  = new List<Int16> { 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1 };
            List<Int16> sor6  = new List<Int16> { 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0 };
            List<Int16> sor7  = new List<Int16> { 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1 };
            List<Int16> sor8  = new List<Int16> { 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1 };
            List<Int16> sor9  = new List<Int16> { 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1 };
            List<Int16> sor10 = new List<Int16> { 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1 };
            List<Int16> sor11 = new List<Int16> { 1, 0, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1 };
            List<Int16> sor12 = new List<Int16> { 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1 };
            List<Int16> sor13 = new List<Int16> { 1, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 1 };
            List<Int16> sor14 = new List<Int16> { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
            List<Int16> sor15 = new List<Int16> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            table = new List<List<Int16>> { sor0, sor1, sor2, sor3, sor4, sor5, sor6, sor7, sor8, sor9, sor10, sor11, sor12, sor13, sor14, sor15 };

            players = new List<Point> { new Point(1, 14), new Point(14, 1)};
        }

        /// <summary>
        /// Kezdőállapotba állítja a minigame-et. Új minigame kezdésekor ezt kell meghívni.
        /// </summary>
        public void ReInitialize()
        {
            players[0] = new Point(1, 14); //Üldöző pozíciója
            players[1] = new Point(14, 1); //Üldözött pozíciója
        }

        /// <summary>
        /// A tábla X. sorában és Y. oszlopában lévő értéket adja vissza. Túlindexelés esetén 1-et (azaz falat) ad vissza.
        /// </summary>
        /// <param name="X">Hanyadik sor</param>
        /// <param name="Y">Hanyadik oszlop</param>
        /// <returns>0 = üres, 1 = fal</returns>
        public Int16 GetPosition(int X, int Y)
        {
            if (X < 0 || X > 15 ||
                Y < 0 || Y > 15)
            {
                return 1;
            }

            return table[X][Y];
        }

        /// <summary>
        /// Megadja, hogy a paraméterben megadott sorszámú játékos hanyadik oszlopban áll a táblán.
        /// </summary>
        /// <param name="playerNum">1 = 1. játékos (üldözött), 2 = 2. játékos (üldöző)</param>
        /// <returns>[0-15] közötti szám: melyik oszlopban áll a playerNum számú játékos</returns>
        public int GetPlayerPositionX(int playerNum)
        {
            return players[playerNum-1].X;
        }

        /// <summary>
        /// Megadja, hogy a paraméterben megadott sorszámú játékos hanyadik sorban áll a táblán.
        /// </summary>
        /// <param name="playerNum">1 = 1. játékos (üldözött), 2 = 2. játékos (üldöző)</param>
        /// <returns>[0-15] közötti szám: melyik sorban áll a playerNum számú játékos</returns>
        public int GetPlayerPositionY(int playerNum)
        {
            return players[playerNum-1].Y;
        }

        /// <summary>
        /// Elmozdít egy játékost a paraméterben megadott irányba. Ha az elmozdulás nem lehetséges, akkor nem történik semmi.
        /// </summary>
        /// <param name="playerNum">1 = 1. játékos (üldözött), 2 = 2. játékos (üldöző)</param>
        /// <param name="dir">Az az irány, amely felé el szeretnénk mozdulni</param>
        public void Move(int playerNum, Direction dir)
        {
            --playerNum; //[1-2] -> [0-1]

            Point direction = new Point(0, 0); //Ez a pont tárolja az irányt ((X,Y) formában), amerre el szeretne mozdulni a játékos
            switch (dir)
            {
                case Direction.RIGHT:
                    direction.X = 1;
                    break;
                case Direction.DOWN:
                    direction.Y = 1;
                    break;
                case Direction.LEFT:
                    direction.X = -1;
                    break;
                case Direction.UP:
                    direction.Y = -1;
                    break;
            }

            //Kívánt pozíció: jelenlegi pozíció + irány
            Point desiredPosition = new Point(players[playerNum].X + direction.X, players[playerNum].Y + direction.Y);

            if (GetPosition(desiredPosition.Y, desiredPosition.X) == 0)
            {
                players[playerNum] = desiredPosition;
            }
        }

        /// <summary>
        /// Áthelyezi a paraméterben megadott játékost a paraméterben megadott pozícióra
        /// </summary>
        /// <param name="playerNum">1 = 1. játékos (üldözött), 2 = 2. játékos (üldöző)</param>
        /// <param name="destination">A pont, (X,Y) formátumban, ahova át kell tenni a játékost</param>
        public void Teleport(int playerNum, Point destination)
        {
            players[playerNum-1] = destination;
        }

        /// <summary>
        /// Leellenőrzi, hogy a két játékos pozíciója ugyanaz-e, azaz az üldöző elkapta-e az üldözöttet
        /// </summary>
        /// <returns>Igaz, ha a két játékos pozíciója ugyanaz, azaz az üldöző elkapta az üldözöttet</returns>
        public bool IsPlayerCaught()
        {
            return (players[0] == players[1]);
        }

        /// <summary>
        /// Leellenőrzi, hogy az üldözött játékos kijutott-e a pályáról, azaz elérte-e a pálya bal vagy jobb szélét
        /// </summary>
        /// <returns>Igaz, ha az üldözött játékos a pálya bal szélén vagy a jobb szélén van</returns>
        public bool PlayerEscaped()
        {
            return (players[0].X == 0 || players[0].X == 15);
        }
    }
}
