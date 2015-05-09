using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameOfLife
{
    public class MiniGame
    {

        private GameOfLife mainGame;

        #region Variables for Update



        private KeyboardState oldKeyboardState; //Az előző update során érvényes billentyűzetállapot
        private KeyboardState newKeyboardState; //A jelenlegi billentyűzetállapot

        private MiniGameModel model;
        private MiniGameComputerAI computer;
        private int player1Num;             //Az első játékos (az üldözött) sorszáma a BoardGame-ben. Ezzel határozható meg, milyen színű bábut kell kirajzolni
        private int player2Num;             //A második játékos (az üldöző) sorszáma a BoardGame-ben. Ezzel határozható meg, milyen színű bábut kell kirajzolni
        private bool player1AI;             //Az első játékos számítógép-e
        private bool player2AI;             //A második játékos számítógép-e
        private int player1Speed;           //Az első játékos sebessége ([1-5] közötti érték)
        private int player2Speed;           //A második játékos sebessége ([1-5] közötti érték)
        private const int delayUnit = 200;  //Ez az érték szorzódik meg a sebességgel = ennyit vár a számítógép két lépés között

        private bool gameEnded;             //Véget ért-e már a játék
        private double elapsedSinceEnd;     //A játék vége után 3 másodpercet várunk. Ez a változó tartalmazza, mennyit vártunk eddig
        private double player1MovementDelay;//Ennyi ezredmásodperc múlva léphet újra az első játékos
        private double player2MovementDelay;//Ennyi ezredmásodperc múlva léphet újra a második játékos



        #endregion
        #region Variables for Draw



        private Texture2D wall;
        private Texture2D floor;
        Texture2D[] playerPieces = new Texture2D[6]; //A különböző színű bábukat tartalmazó tömb

        private SpriteFont youWinFont;



        #endregion
        #region Constructor



        public MiniGame(GameOfLife game)
        {
            this.mainGame = game;

            model = new MiniGameModel();
            computer = new MiniGameComputerAI(model);
        }



        #endregion
        #region Initialize



        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public void Initialize()
        {
            oldKeyboardState = Keyboard.GetState();
            newKeyboardState = Keyboard.GetState();

            player1Num = 0;
            player2Num = 1;
            player1AI = false;
            player2AI = false;
            gameEnded = false;
            elapsedSinceEnd = 0;
            player1Speed = 1;
            player2Speed = 1;
            player1MovementDelay = delayUnit;
            player2MovementDelay = delayUnit;
        }

        /// <summary>
        /// Ezzel indítható el a minigame.
        /// </summary>
        /// <param name="player1Num">Az üldözött játékos sorszáma a BoardGame-ben</param>
        /// <param name="player2Num">Az üldöző játékos sorszáma a BoardGame-ben</param>
        /// <param name="player1AI">Az üldözött játékost számítógép vezérli?</param>
        /// <param name="player2AI">Az üldöző játékost számítógép vezérli?</param>
        public void StartMiniGame(int player1Num, int player2Num, bool player1AI, bool player2AI)
        {
            model.ReInitialize();
            this.player1Num = player1Num;
            this.player2Num = player2Num;
            this.player1AI = player1AI;
            this.player2AI = player2AI;

            gameEnded = false;
            elapsedSinceEnd = 0;

            //Kisorsoljuk az egyes játékosok sebességét. Csak akkor van jelentősége, ha van számítógép játékos
            Random rnd = new Random();
            player1Speed = rnd.Next(1, 6);
            player1MovementDelay = player1Speed * delayUnit;

            player2Speed = rnd.Next(1, 6);
            player2MovementDelay = player2Speed * delayUnit;
        }



        #endregion
        #region LoadContent



        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent()
        {
            wall = mainGame.Content.Load<Texture2D>("wall");
            floor = mainGame.Content.Load<Texture2D>("floor");

            playerPieces[0] = mainGame.Content.Load<Texture2D>("player1");
            playerPieces[1] = mainGame.Content.Load<Texture2D>("player2");
            playerPieces[2] = mainGame.Content.Load<Texture2D>("player3");
            playerPieces[3] = mainGame.Content.Load<Texture2D>("player4");
            playerPieces[4] = mainGame.Content.Load<Texture2D>("player5");
            playerPieces[5] = mainGame.Content.Load<Texture2D>("player6");

            youWinFont = mainGame.Content.Load<SpriteFont>("MiniGame");
        }



        #endregion
        #region UnloadContent



        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        public void UnloadContent()
        {
            
        }



        #endregion
        #region Update



        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            newKeyboardState = Keyboard.GetState();

            //Első játékos (az üldözött) mozgásának lekezelése, ha az ember
            if (!gameEnded && !player1AI)
            {
                if (oldKeyboardState.IsKeyUp(Keys.Up) && newKeyboardState.IsKeyDown(Keys.Up))
                {
                    model.Move(1, MiniGameModel.Direction.UP);
                }
                if (oldKeyboardState.IsKeyUp(Keys.Right) && newKeyboardState.IsKeyDown(Keys.Right))
                {
                    model.Move(1, MiniGameModel.Direction.RIGHT);
                }
                if (oldKeyboardState.IsKeyUp(Keys.Down) && newKeyboardState.IsKeyDown(Keys.Down))
                {
                    model.Move(1, MiniGameModel.Direction.DOWN);
                }
                if (oldKeyboardState.IsKeyUp(Keys.Left) && newKeyboardState.IsKeyDown(Keys.Left))
                {
                    model.Move(1, MiniGameModel.Direction.LEFT);
                }
            }

            //Második játékos (az üldöző) mozgásának lekezelése, ha az ember
            if (!gameEnded && !player2AI)
            {
                if (oldKeyboardState.IsKeyUp(Keys.W) && newKeyboardState.IsKeyDown(Keys.W))
                {
                    model.Move(2, MiniGameModel.Direction.UP);
                }
                if (oldKeyboardState.IsKeyUp(Keys.D) && newKeyboardState.IsKeyDown(Keys.D))
                {
                    model.Move(2, MiniGameModel.Direction.RIGHT);
                }
                if (oldKeyboardState.IsKeyUp(Keys.S) && newKeyboardState.IsKeyDown(Keys.S))
                {
                    model.Move(2, MiniGameModel.Direction.DOWN);
                }
                if (oldKeyboardState.IsKeyUp(Keys.A) && newKeyboardState.IsKeyDown(Keys.A))
                {
                    model.Move(2, MiniGameModel.Direction.LEFT);
                }
            }

            //Ha az első játékos (üldözött) számítógép, és lejárt a delay, akkor lekérdezzük a computer-től, hogy merre akar mozdulni
            if (!gameEnded && player1AI && player1MovementDelay <= 0)
            {
                model.Teleport(1, computer.Move(1));

                player1MovementDelay = player1Speed * delayUnit;
            }

            //Ha a második játékos (üldöző) számítógép, és lejárt a delay, akkor lekérdezzük a computer-től, hogy merre akar mozdulni
            if (!gameEnded && player2AI && player2MovementDelay <= 0)
            {
                model.Teleport(2, computer.Move(2));

                player2MovementDelay = player2Speed * delayUnit;
            }

            //Ellenőrizzük, hogy véget ért-e a játék, azaz az üldöző elkapta-e a játékost vagy az üldözött kimenekült-e
            if ( !gameEnded &&
               ( model.IsPlayerCaught() || model.PlayerEscaped() ))
            {
                gameEnded = true;
            }

            //Ha véget ért a játék, akkor várunk 3 másodpercet
            if (gameEnded)
            {
                elapsedSinceEnd += gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            //Ha lejárt a 3 másodperc, jelzünk a mainGame-nek, hogy az egyik játékos nyert
            if (elapsedSinceEnd >= 3000)
            {
                if (model.PlayerEscaped())
                {
                    mainGame.MiniGameEnded(1); //Az első játékos (az üldözött) nyert
                }
                if (model.IsPlayerCaught())
                {
                    mainGame.MiniGameEnded(2); //A második játékos (az üldöző) nyert
                }
            }

            //Az idő elteltével egyre kevesebbet kell várnia a számítógép által vezérelt játékosoknak a következő lépésig
            player1MovementDelay -= gameTime.ElapsedGameTime.TotalMilliseconds;
            player2MovementDelay -= gameTime.ElapsedGameTime.TotalMilliseconds;

            oldKeyboardState = newKeyboardState;
        }



        #endregion
        #region Draw



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime)
        {
            mainGame.GraphicsDevice.Clear(Color.CornflowerBlue);

            mainGame.spriteBatch.Begin();

            //Offset kiszámolása:
            //78 + 16*50 + 78 = 956 = ablak szélessége
            //17 + 16*50 + 17 = 835 = ablak magassága
            Point offset = new Point(78, 17);

            //Kirajzoljuk a táblát
            for (int i = 0; i < 16; ++i)
            {
                for (int j = 0; j < 16; ++j)
                {
                    switch (model.GetPosition(i,j))
                    {
                        case 0: //Üres, padló
                            mainGame.spriteBatch.Draw(floor, new Rectangle(offset.X + (50 * j), offset.Y + (50 * i), 50, 50), Color.White);
                            break;
                        case 1: //Fal
                            mainGame.spriteBatch.Draw(wall, new Rectangle(offset.X + (50 * j), offset.Y + (50 * i), 50, 50), Color.White);
                            break;
                    }
                }
            }

            //Kirajzoljuk a két játékost
            mainGame.spriteBatch.Draw(playerPieces[player1Num], new Rectangle(offset.X + model.GetPlayerPositionX(1) * 50, offset.Y + model.GetPlayerPositionY(1) * 50, 50, 50), Color.White);
            mainGame.spriteBatch.Draw(playerPieces[player2Num], new Rectangle(offset.X + model.GetPlayerPositionX(2) * 50, offset.Y + model.GetPlayerPositionY(2) * 50, 50, 50), Color.White);

            //Kiírjuk a képernyő közepére, ha valamelyik játékos nyert
            if (model.IsPlayerCaught())
            {
                mainGame.spriteBatch.DrawString(youWinFont, "Elkaptak!", new Vector2(299, 376), Color.Black);
            }
            if (model.PlayerEscaped())
            {
                mainGame.spriteBatch.DrawString(youWinFont, "Megmenekültél!", new Vector2(153, 376), Color.Black);
            }

            mainGame.spriteBatch.End();
        }



        #endregion
    }
}

