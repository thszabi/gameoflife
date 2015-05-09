using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameOfLife
{
    class MiniGameComputerAI
    {
        //Egy mutató a minigame játékmodelljére. Ennek segítségével kérdezhető le a játékosok pozíciója, és a pálya egyes pontjainak elemei
        private readonly MiniGameModel model;

        public MiniGameComputerAI(MiniGameModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// Ezzel a függvénnyel lehet megkérni a számítógépet, hogy lépjen az adott játékossal.
        /// </summary>
        /// <param name="playerNum">A játékos sorszáma, akivel lépni kell. 1 = 1. játékos (üldözött), 2 = 2. játékos (üldöző)</param>
        /// <returns>(X,Y) pont, mely azt a pontot tartalmazza, ahova a játékos lépni akar</returns>
        public Point Move(int playerNum)
        {
            bool stop = false; //Igazzá válik, ha megtaláltuk a célpontot
            
            //A kiinduló pont: az adott játékos pozíciója
            Point start = new Point(model.GetPlayerPositionX(playerNum), model.GetPlayerPositionY(playerNum));

            //A célpont:
            //Üldözött játékos esetén a képernyő széle
            //Üldöző játékos esetén az üldözött játékos
            Point goal;
            if (playerNum == 1)
            {
                //Ha az üldözött játékosról van szó, akkor megnézzük, melyik kijárat van közelebb, és az lesz a célpont
                if (model.GetPlayerPositionX(1) <= 7)
                {
                    goal = new Point(0, 6);
                }
                else
                {
                    goal = new Point(15, 6);
                }

                //Ha a a bal oldali kijáratot blokkolja az üldöző játékos, akkor a másik kijárat a célpont
                if (model.GetPlayerPositionY(2) == 6 && model.GetPlayerPositionX(2) <= 3)
                {
                    goal = new Point(15, 6);
                }
                //Ha a a jobb oldali kijáratot blokkolja az üldöző játékos, akkor a másik kijárat a célpont
                if (model.GetPlayerPositionY(2) == 6 && model.GetPlayerPositionX(2) >= 11)
                {
                    goal = new Point(0, 6);
                }
            }
            else
            {
                //Ha az üldöző játékosról van szó, akkor a célpont az üldözött játékos pozíciója
                goal = new Point(model.GetPlayerPositionX(1), model.GetPlayerPositionY(1));
            }

            //Flooding algoritmus:  A startból indulva elkezdünk "terjeszkedni", és a már bejárt terület határait alkotó pontok egyikét
            //                      terjesztjük ki minden egyes lépésben
            List<Point> frontier = new List<Point>();   //A határpontokat tartalmazó lista
            frontier.Add(start);                        //A startból indulunk

            //Számon tartjuk, hogy melyik pontba melyik ponton keresztül vezet az út
            //Kulcs: az adott pont, a tábla egy pontja, (X,Y) formátumban
            //Érték: a Kulcsban megadott pontot megelőző pont azon az úton, amelyik a startból indul, és a kulcsba vezet
            Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();
            cameFrom.Add(start, new Point(-1, -1)); //A start pontnak nincs szülője

            //Main loop
            while (frontier.Count != 0 && !stop)
            {
                Point current = frontier[0]; //Kiveszünk egy pontot a határpontok közül
                frontier.RemoveAt(0);

                //Összegyűjtük az aktuális pont szomszédait
                List<Point> neighbors = new List<Point>();

                //Csak az számít szomszédnak, ami nem fal, és nincs rajta az üldöző játékos (hiszen az üldöző játékosba nem szeretnénk belemenni)
                //Jobbra
                if (model.GetPosition(current.Y, current.X+1) == 0 &&
                       (model.GetPlayerPositionX(2) != current.X + 1 ||
                        model.GetPlayerPositionY(2) != current.Y))
                {
                    neighbors.Add(new Point(current.X + 1, current.Y));
                }

                //Balra
                if (model.GetPosition(current.Y, current.X-1) == 0 &&
                       (model.GetPlayerPositionX(2) != current.X - 1 ||
                        model.GetPlayerPositionY(2) != current.Y))
                {
                    neighbors.Add(new Point(current.X - 1, current.Y));
                }

                //Le
                if (model.GetPosition(current.Y+1, current.X) == 0 &&
                       (model.GetPlayerPositionX(2) != current.X ||
                        model.GetPlayerPositionY(2) != current.Y + 1))
                {
                    neighbors.Add(new Point(current.X, current.Y + 1));
                }

                //Fel
                if (model.GetPosition(current.Y-1, current.X) == 0 &&
                       (model.GetPlayerPositionX(2) != current.X ||
                        model.GetPlayerPositionY(2) != current.Y - 1))
                {
                    neighbors.Add(new Point(current.X, current.Y - 1));
                }

                //Minden szomszédra megnézzük, hogy ismerjük-e már a hozzá vezető utat
                for (int i = 0; i < neighbors.Count; ++i)
                {
                    if (!cameFrom.ContainsKey(neighbors[i]))
                    {
                        //Ha nem ismerjük, akkor hozzáadjuk a feldolgozandó pontok listájához (a határhoz), és felírjuk a map-be a hozzá vezető pontot
                        frontier.Add(neighbors[i]);
                        cameFrom.Add(neighbors[i], current);
                        //Ha a vizsgált szomszéd a célpont, akkor megállhatunk az algoritmussal
                        stop = (stop || neighbors[i] == goal);
                    }
                }
            }//Main loop end

            if (stop)
            {
                //Ha véget ért az algoritmus, elindulunk a célponttól visszafelé a célponttól a startig vezető úton,
                //egészen a startot követő pontig. Ott megállunk, és a startot követő pontot adjuk vissza.
                Point current = goal;
                List<Point> path = new List<Point>(); //A célponttól a startba vezető útvonalat tárolja
                path.Add(goal);
                while (current != start)
                {
                    current = cameFrom[current]; //A map segítségével egyet visszalépünk az aktuális pont szülejére
                    if (current != start) //A startot már nem engedjük bekerülni az útvonalba
                    {
                        path.Add(current);
                    }
                }
                return path[path.Count - 1];//A ciklus végén a startot követő pont lesz az útvonal utolsó eleme
                                            //(hiszen a startot már nem engedtük belekerülni az útvonalba)
            }
            else
            {
                return start; //Ez az ág akkor teljesül, ha nem találtunk utat a célpontba. Ez ebben a játékban elvileg lehetetlen.
            }
        }

    }
}
