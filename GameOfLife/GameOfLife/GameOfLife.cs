using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameOfLife
{
    /// <summary>
    /// This is the main type for the game
    /// </summary>
    public class GameOfLife : Microsoft.Xna.Framework.Game
    {
        enum State { MAINMENU, INSTRUCTIONS, LOADGAME, QUITTING, NUMBEROFPLAYERS, COLLEGEORCAREER, 
                     PLAYERSTURN, SAVEGAME, MOVING, CHOOSESTOCK, CHOOSEJOB, CHOOSESALARY, 
                     CHANGEJOB, ATFORK1, ATFORK2, TRADESALARY, TRADEWITHWHO, CHOOSERETIREMENT };

        #region Variables for Update



        /// <summary>
        /// A Fizetésnapok sorszámai
        /// </summary>
        private readonly int[] locationsOfPayDays = { 14, 17, 25, 34, 39, 45, 55, 60, 68, 73, 81,
                                                      88, 95, 98, 106, 113, 120, 127, 134, 139, 145 };
        /// <summary>
        /// A STOP mezõk sorszámai
        /// </summary>
        private readonly int[] locationsOfStops = { 12, 27, 38, 149};
        
        private DataModel.DataModel model;

        private State gameState;
        private KeyboardState oldKeyboardState; //Az elõzõ update során érvényes billentyûzetállapot
        private KeyboardState newKeyboardState; //A jelenlegi billentyûzetállapot

        /// <summary>
        /// A játékban szereplõ kurzor helye
        /// MAINMENU:
        /// 0 - Új játék
        /// 1 - Betöltés
        /// 2 - Játékszabályok
        /// 3 - Kilépés
        /// NUMBEROFPLAYERS:
        /// i - az i. játékos képére mutat (i = {0..5})
        /// PLAYERSTURN:
        /// 0 - Pörgetés
        /// 1 - Otthonbiztosítás vásárlása
        /// 2 - Gépjármûbiztosítás vásárlása
        /// 3 - Részvény vásárlása
        /// 4 - Hitel visszafizetése
        /// 5 - Mentés
        /// 6 - Kilépés
        /// CHOOSESTOCK
        /// i - i+1. részvényre mutat (i = {0..8})
        /// 9 - Mégse
        /// </summary>
        private int arrowPosition;

        /// <summary>
        /// Egy 6 elemû tömb, amelyben minden játékoshoz tartozik egy szám:
        /// 0, ha a játékos nem játszik
        /// 1, ha a játékos férfi és ember
        /// 2, ha a játékos nõ és ember
        /// 3, ha a játékos férfi és gép
        /// 4, ha a játékos nõ és gép
        /// </summary>
        private int[] numberOfPlayers;

        private int stepsLeft;



        #endregion
        #region Variables for Draw



        GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //Fõmenühöz
        private Texture2D arrow;
        private Texture2D arrowUp;
        private Texture2D newGameBtn;
        private Texture2D instructionsBtn;
        private Texture2D openGameBtn;
        private Texture2D escapeBtn;
        private Texture2D menuBackground; 
        
        //Játékos kiválasztásához
        private Texture2D choosePlayerBackground;
        private Texture2D emptyProfile;
        private Texture2D man;
        private Texture2D woman;
        private Texture2D manAI;
        private Texture2D womanAI;
        Texture2D[] img = new Texture2D[6];

        //
        private Texture2D boy;
        private Texture2D girl;
        private Texture2D empty;


        private Texture2D saveBtn;
        private Texture2D escapeBtn2;
        private Texture2D spinBtn;

        private Texture2D payBackLoanBtn;
        private Texture2D buyCarInsBtn;
        private Texture2D buyHouseInsBtn;
        private Texture2D buyStockBtn;


        private SpriteFont instructionsFont;
        private SpriteFont titleFont;

        private Texture2D palya2;

        #endregion
        #region Constructor



        public GameOfLife()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 835;
            graphics.PreferredBackBufferWidth = 956;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }



        #endregion
        #region Initialize



        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            arrowPosition = 0;
            gameState = State.MAINMENU;
            oldKeyboardState = Keyboard.GetState();
            newKeyboardState = Keyboard.GetState();

            stepsLeft = 0;

            base.Initialize();
        }



        #endregion
        #region LoadContent



        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            arrow = Content.Load<Texture2D>("arrow");
            newGameBtn = Content.Load<Texture2D>("new_game_btn");
            instructionsBtn = Content.Load<Texture2D>("instructions_btn");
            openGameBtn = Content.Load<Texture2D>("open_game_btn");
            escapeBtn = Content.Load<Texture2D>("esc_btn");
            menuBackground = Content.Load<Texture2D>("menu_img");

            arrowUp = Content.Load<Texture2D>("arrow_up");
            choosePlayerBackground = Content.Load<Texture2D>("empty_img");
            emptyProfile = Content.Load<Texture2D>("empty_profile");
            man = Content.Load<Texture2D>("man_profile");
            woman = Content.Load<Texture2D>("woman_profile");
            manAI = Content.Load<Texture2D>("man_m_profile");
            womanAI = Content.Load<Texture2D>("woman_m_profile");
            
            boy= Content.Load<Texture2D>("boy");
            girl= Content.Load<Texture2D>("girl");
            empty = Content.Load<Texture2D>("empty");
            saveBtn = Content.Load<Texture2D>("save_btn");
            escapeBtn2 = Content.Load<Texture2D>("esc_btn2");
            spinBtn = Content.Load<Texture2D>("spin_btn");
            payBackLoanBtn = Content.Load<Texture2D>("pay_back_loan_btn");
            buyCarInsBtn = Content.Load<Texture2D>("buy_car_ins_btn");
            buyHouseInsBtn = Content.Load<Texture2D>("buy_house_ins_btn");
            buyStockBtn = Content.Load<Texture2D>("buy_stock_btn");

            instructionsFont = Content.Load<SpriteFont>("Instructions");
            titleFont = Content.Load<SpriteFont>("Instructions_title");

            palya2 = Content.Load<Texture2D>("palya3");
        }



        #endregion
        #region UnloadContent



        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }



        #endregion
        #region Update



        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            newKeyboardState = Keyboard.GetState();

            switch (gameState)
            {
                case State.MAINMENU:
                    //"Fel" lekezelése - kurzor felfelé léptetése
                    if (oldKeyboardState.IsKeyUp(Keys.Up) && newKeyboardState.IsKeyDown(Keys.Up))
                    {
                        arrowPosition = arrowPosition == 0 ? 3 : arrowPosition - 1;
                    }
                    //"Le" lekezelése - kurzor lefelé léptetése
                    if (oldKeyboardState.IsKeyUp(Keys.Down) && newKeyboardState.IsKeyDown(Keys.Down))
                    {
                        arrowPosition = (arrowPosition + 1) % 4;
                    }
                    //"Space" és "Enter" lekezelése - menüpont aktiválása
                    if ((oldKeyboardState.IsKeyUp(Keys.Space) && newKeyboardState.IsKeyDown(Keys.Space)) ||
                        (oldKeyboardState.IsKeyUp(Keys.Enter) && newKeyboardState.IsKeyDown(Keys.Enter)))
                    {
                        switch (arrowPosition)
                        {
                            case 0: //Új játék
                                gameState = State.NUMBEROFPLAYERS;
                                arrowPosition = 0;
                                numberOfPlayers = new int[6];
                                for (int i = 0; i < 6; ++i)
                                {
                                    numberOfPlayers[i] = 0;
                                    img[i] = emptyProfile;
                                }
                                break;

                            case 1: //Betöltés
                                //TODO Load game
                                break;

                            case 2: //Játékszabályok
                                gameState = State.INSTRUCTIONS;
                                break;

                            default: //Kilépés
                                //TODO "Are you sure?..."
                                this.Exit();
                                break;

                        }//switch(arrowPosition)
                    }//Space/Enter lekezelése
                    break;

                case State.INSTRUCTIONS:
                    //"Esc", "Space", "Enter" lekezelése - visszalépés
                    if ((oldKeyboardState.IsKeyUp(Keys.Escape) && newKeyboardState.IsKeyDown(Keys.Escape)) ||
                        (oldKeyboardState.IsKeyUp(Keys.Space)  && newKeyboardState.IsKeyDown(Keys.Space) )  ||
                        (oldKeyboardState.IsKeyUp(Keys.Enter)  && newKeyboardState.IsKeyDown(Keys.Enter) ) )
                    {
                        gameState = State.MAINMENU;
                    }
                    break;

                case State.NUMBEROFPLAYERS:
                    //"Jobbra" lekezelése - következõ képre mutat a kurzor
                    if (oldKeyboardState.IsKeyUp(Keys.Right) && newKeyboardState.IsKeyDown(Keys.Right))
                    {
                        arrowPosition = (arrowPosition+1) % 6;
                    }
                    //"Balra" lekezelése - elõzõ képre mutat a kurzor
                    if (oldKeyboardState.IsKeyUp(Keys.Left) && newKeyboardState.IsKeyDown(Keys.Left))
                    {
                        arrowPosition = arrowPosition == 0 ? 5 : arrowPosition - 1;
                    }
                    //"Fel" lekezelése - az aktuális játékos állapota megváltozik
                    if (oldKeyboardState.IsKeyUp(Keys.Up) && newKeyboardState.IsKeyDown(Keys.Up))
                    {
                        numberOfPlayers[arrowPosition] = (numberOfPlayers[arrowPosition] + 1) % 5;
                    }
                    //"Le" lekezelése - az aktuális játék állapota megváltozik
                    if (oldKeyboardState.IsKeyUp(Keys.Down) && newKeyboardState.IsKeyDown(Keys.Down))
                    {
                        numberOfPlayers[arrowPosition] = numberOfPlayers[arrowPosition] == 0 ? 4 : numberOfPlayers[arrowPosition] - 1;
                    }
                    //"Enter" lekezelése - ha van elég játékos, indul a játék
                    if (oldKeyboardState.IsKeyUp(Keys.Enter) && newKeyboardState.IsKeyDown(Keys.Enter))
                    {
                        //Játékosok összeszámolása
                        int noOfPlayers = 0;
                        foreach (int player in numberOfPlayers)
                        {
                            if (player != 0)
                            {
                                ++noOfPlayers;
                            }
                        }

                        //ha nincs elég játékos, nem csinálunk semmit
                        if (noOfPlayers > 1)
                        {
                            List<Tuple<string, bool, bool>> playerList = new List<Tuple<string, bool, bool>>();

                            foreach (int player in numberOfPlayers)
                            {
                                switch (player)
                                {
                                    case 1: //Férfi és ember
                                        playerList.Add(new Tuple<string, bool, bool>(player+". játékos", false, false));
                                        break;
                                    case 2: //Nõ és ember
                                        playerList.Add(new Tuple<string, bool, bool>(player+". játékos", true, false));
                                        break;
                                    case 3: //Férfi és gép
                                        playerList.Add(new Tuple<string, bool, bool>(player+". játékos", false, true));
                                        break;
                                    case 4: //Nõ és gép
                                        playerList.Add(new Tuple<string, bool, bool>(player+". játékos", true, true));
                                        break;
                                }
                            }
                            
                            model = new DataModel.DataModel(noOfPlayers, playerList);
                            gameState = State.PLAYERSTURN;
                            arrowPosition = 0;
                            //QUESTION: A currentPlayer-t én állítgatom, vagy a model?
                        }
                    }
                    break;

                case State.PLAYERSTURN:
                    //TODO gép-e vagy sem
                    if (model.PlayerLocation(model.ActualPlayer) != 0 &&
                        !model.IsRetired(model.ActualPlayer) &&
                        !model.PlayerLoseNextRound(model.ActualPlayer))
                    {
                        //"Fel" lekezelése - következõ menüpontra mutat a kurzor
                        if (oldKeyboardState.IsKeyUp(Keys.Up) && newKeyboardState.IsKeyDown(Keys.Up))
                        {
                            arrowPosition = (arrowPosition + 1) % 7;
                            //Ha van biztosítása, arra a menüpontra nem mutathat a kurzor (nem vehet még egyet)
                            //Addig léptetjük a kurzort, amíg érvényes menüpontra nem lép
                            if (arrowPosition == 1 &&
                                    (model.PlayerHouseInsurance(model.ActualPlayer) ||
                                     model.PlayerHouseCard(model.ActualPlayer) == 9))
                            {
                                ++arrowPosition;
                            }
                            if (arrowPosition == 2 && model.PlayerCarInsurance(model.ActualPlayer))
                            {
                                ++arrowPosition;
                            }
                            if (arrowPosition == 3 && model.PlayerStockCard(model.ActualPlayer) != 9)
                            {
                                ++arrowPosition;
                            }
                            if (arrowPosition == 4 && model.PlayerLoan(model.ActualPlayer) == 0)
                            {
                                ++arrowPosition;
                            }
                        }
                        //"Le" lekezelése - elõzõ menüpontra mutat a kurzor
                        if (oldKeyboardState.IsKeyUp(Keys.Down) && newKeyboardState.IsKeyDown(Keys.Down))
                        {
                            arrowPosition = arrowPosition == 0 ? 6 : arrowPosition - 1;
                            //Ha nincs kölcsöne, a visszafizetés menüpontra nem mutathat a kurzor (nem vehet még egyet)
                            //Addig léptetjük a kurzort, amíg érvényes menüpontra nem lép
                            if (arrowPosition == 4 && model.PlayerLoan(model.ActualPlayer) == 0)
                            {
                                --arrowPosition;
                            }
                            if (arrowPosition == 3 && model.PlayerStockCard(model.ActualPlayer) != 9)
                            {
                                --arrowPosition;
                            }
                            if (arrowPosition == 2 && model.PlayerCarInsurance(model.ActualPlayer))
                            {
                                --arrowPosition;
                            }
                            if (arrowPosition == 1 &&
                                    (model.PlayerHouseInsurance(model.ActualPlayer) ||
                                     model.PlayerHouseCard(model.ActualPlayer) == 9))
                            {
                                --arrowPosition;
                            }
                        }
                        //"Space" és "Enter" lekezelése - menüpont aktiválása
                        if ((oldKeyboardState.IsKeyUp(Keys.Space) && newKeyboardState.IsKeyDown(Keys.Space)) ||
                            (oldKeyboardState.IsKeyUp(Keys.Enter) && newKeyboardState.IsKeyDown(Keys.Enter)))
                        {
                            switch (arrowPosition)
                            {
                                case 1: //Otthonbiztosítás vásárlása
                                    if (model.BuyHouseInsurance(model.ActualPlayer))
                                    {
                                        //TODO kimenet: sikeres vásárlás
                                        arrowPosition = 0;
                                    }
                                    else
                                    {
                                        //TODO kimenet: sikertelen vásárlás
                                    }
                                    break;

                                case 2: //Gépjármûbiztosítás vásárlása
                                    if (model.BuyCarInsurance(model.ActualPlayer))
                                    {
                                        //TODO kimenet: sikeres vásárlás
                                        arrowPosition = 0;
                                    }
                                    else
                                    {
                                        //TODO kimenet: sikertelen vásárlás
                                    }
                                    break;

                                case 3: //Részvény vásárlása
                                    gameState = State.CHOOSESTOCK;
                                    arrowPosition = 9;
                                    break;

                                case 4: //Hitel visszafizetése
                                    /*if (model.PayBackLoan(model.ActualPlayer))
                                    {
                                        //TODO kimenet: sikeres visszafizetés
                                        if (model.PlayerLoan(model.ActualPlayer) == 0)
                                        {
                                            arrowPosition = 0;
                                        }
                                    }
                                    else
                                    {
                                        //TODO kimenet: sikertelen visszafizetés
                                    }*/
                                    break;

                                case 5: //Mentés
                                    //TODO mentés
                                    break;

                                case 6: //Kilépés
                                    //TODO kilépés
                                    break;

                                default: //Pörgetés
                                    Tuple<int, int> spinResult = model.Spin(model.ActualPlayer);
                                    stepsLeft = spinResult.Item1;
                                    
                                    //Ha valaki pénzt kapott
                                    if (spinResult.Item2 != -1)
                                    {
                                        if (spinResult.Item1 == 10)
                                        {
                                            //TODO kimenet: gyorshajtás! Az x. játékos kap 10.000-ret
                                        }
                                        else
                                        {
                                            //TODO kimenet: az x. játékos kap 10.000-ret a részvénye miatt.
                                        }
                                    }

                                    //Pörgetés után jön a léptetés
                                    gameState = State.MOVING;

                                    break;
                            }
                        }
                    }
                    else
                    {
                        //Ha a játékos még nem döntött az egyetem és karrier között
                        if (model.PlayerLocation(model.ActualPlayer) == 0)
                        {
                            gameState = State.COLLEGEORCAREER;
                        }
                        //Ha a játékos már nyugdíjba vonult vagy kimarad egy körbõl
                        else
                        {
                            model.NextPlayer();
                        }
                    }
                    break;

                case State.COLLEGEORCAREER:
                    //"1" lekezelése - a játékos az egyetemet választotta
                    if (oldKeyboardState.IsKeyUp(Keys.D1) && newKeyboardState.IsKeyDown(Keys.D1))
                    {
                        model.SetPlayerLocation(model.ActualPlayer,1);
                        //TODO GiveLoanOnly
                        model.GetLoan(model.ActualPlayer, 40000);
                        model.PayMoney(model.ActualPlayer, 40000);

                        gameState = State.PLAYERSTURN;
                    }
                    //"2" lekezelése - a játékos a karriert választotta
                    if (oldKeyboardState.IsKeyUp(Keys.D2) && newKeyboardState.IsKeyDown(Keys.D2))
                    {
                        model.SetPlayerLocation(model.ActualPlayer,13);
                        //model.NeedCareer(model.ActualPlayer);
                        //model.NeedSalary(model.ActualPlayer);

                        gameState = State.PLAYERSTURN;
                    }
                    break;

                case State.MOVING:
                    //Az 50. mezõ után elágazás következik
                    if (model.PlayerLocation(model.ActualPlayer) == 50)
                    {
                        gameState = State.ATFORK1;
                    }
                    //A  86. mezõ után elágazás következik
                    if (model.PlayerLocation(model.ActualPlayer) == 86)
                    {
                        gameState = State.ATFORK2;
                    }

                    if (gameState == State.MOVING && stepsLeft != 0)
                    {
                        stepForward();
                        //TODO várakozás
                    }
                    //Ha elfogyott a lépésünk, aktiválódik a mezõ hatása
                    if (gameState == State.MOVING && stepsLeft == 0)
                    {
                        EffectOfField(model.PlayerLocation(model.ActualPlayer));
                        model.NextPlayer();
                        gameState = State.PLAYERSTURN;
                    }
                    break;

                case State.ATFORK1:
                    //"1" lekezelése - a játékost elõre léptetjük eggyel az 1. útvonalon
                    if (oldKeyboardState.IsKeyUp(Keys.D1) && newKeyboardState.IsKeyDown(Keys.D1))
                    {
                        model.SetPlayerLocation(model.ActualPlayer, 51);
                        --stepsLeft;
                        gameState = State.MOVING;
                    }
                    //"2" lekezelése - a játékost elõre léptetjük eggyel az 2. útvonalon
                    if (oldKeyboardState.IsKeyUp(Keys.D2) && newKeyboardState.IsKeyDown(Keys.D2))
                    {
                        model.SetPlayerLocation(model.ActualPlayer, 57);
                        --stepsLeft;
                        gameState = State.MOVING;
                    }
                    break;

                case State.ATFORK2:
                    //"1" lekezelése - a játékost elõre léptetjük eggyel az 1. útvonalon
                    if (oldKeyboardState.IsKeyUp(Keys.D1) && newKeyboardState.IsKeyDown(Keys.D1))
                    {
                        model.SetPlayerLocation(model.ActualPlayer, 87);
                        --stepsLeft;
                        gameState = State.MOVING;
                    }
                    //"2" lekezelése - a játékost elõre léptetjük eggyel az 2. útvonalon
                    if (oldKeyboardState.IsKeyUp(Keys.D2) && newKeyboardState.IsKeyDown(Keys.D2))
                    {
                        model.SetPlayerLocation(model.ActualPlayer, 94);
                        --stepsLeft;
                        gameState = State.MOVING;
                    }
                    break;

                case State.CHOOSERETIREMENT:
                    //"1" lekezelése - a játékos a vidéki házba megy nyugdíjba
                    if (oldKeyboardState.IsKeyUp(Keys.D1) && newKeyboardState.IsKeyDown(Keys.D1))
                    {
                        model.SetPlayerLocation(model.ActualPlayer, 151);
                        model.Retire(model.ActualPlayer, false);
                        //TODO check for ending
                        model.NextPlayer();
                        gameState = State.PLAYERSTURN;
                    }
                    //"2" lekezelése - a játékos a milliomosok nyaralójába megy nyugdíjba
                    if (oldKeyboardState.IsKeyUp(Keys.D2) && newKeyboardState.IsKeyDown(Keys.D2))
                    {
                        model.SetPlayerLocation(model.ActualPlayer, 150);
                        model.Retire(model.ActualPlayer, true);
                        //TODO check for ending
                        model.NextPlayer();
                        gameState = State.PLAYERSTURN;
                    }
                    break;

                /*Nincs kész!
                case State.CHOOSESTOCK:
                    //"Fel" lekezelése - a következõ elérhetõ részvényre ugrik a kurzor
                    if (oldKeyboardState.IsKeyUp(Keys.Up) && newKeyboardState.IsKeyDown(Keys.Up))
                    {
                        arrowPosition = (arrowPosition + 1) % 10;
                        while (!model.IsStockCardFree(arrowPosition) && arrowPosition != 9)
                        {
                            arrowPosition = (arrowPosition + 1) % 10;
                        }
                    }
                    //"Le" lekezelése - az elõzõ elérhetõ részvényre ugrik a kurzor
                    if (oldKeyboardState.IsKeyUp(Keys.Down) && newKeyboardState.IsKeyDown(Keys.Down))
                    {
                        arrowPosition = arrowPosition == 0 ? 9 : arrowPosition - 1;
                        while (!model.IsStockCardFree(arrowPosition) && arrowPosition != 9)
                        {
                            arrowPosition = arrowPosition == 0 ? 9 : arrowPosition - 1;
                        }
                    }
                    //"Space" és "Enter" lekezelése - kiválasztott részvény megvásárlása vagy visszalépés
                    if ((oldKeyboardState.IsKeyUp(Keys.Space) && newKeyboardState.IsKeyDown(Keys.Space)) ||
                        (oldKeyboardState.IsKeyUp(Keys.Enter) && newKeyboardState.IsKeyDown(Keys.Enter)))
                    {
                        //Ha nem a "mégse"-n áll a kurzor
                        if (arrowPosition != 9)
                        {
                            if (true)//(model.BuyStock(model.ActualPlayer, arrowPosition))
                            {
                                //TODO kimenet: sikeres részvény
                                arrowPosition = 0;
                            }
                            else
                            {
                                //TODO kimenet: sikertelen részvény
                                arrowPosition = 3;
                            }
                        }
                        else
                        {
                            arrowPosition = 3;
                        }
                        gameState = State.PLAYERSTURN;
                    }
                    break;*/
            }//switch (gameState)

            oldKeyboardState = newKeyboardState;
            base.Update(gameTime);
        }

        /// <summary>
        /// Az aktuális játékos eggyel elõre léptetése.
        /// A léptetés után ellenõrzi, hogy STOP-ra vagy Fizetésnapra lépett-e.
        /// </summary>
        private void stepForward()
        {
            if (model.PlayerLocation(model.ActualPlayer) == 0 ||
                model.PlayerLocation(model.ActualPlayer) == 50 ||
                model.PlayerLocation(model.ActualPlayer) == 86 ||
                model.PlayerLocation(model.ActualPlayer) == 149 ||
                model.PlayerLocation(model.ActualPlayer) == 150 ||
                model.PlayerLocation(model.ActualPlayer) == 151)
            {
                throw new Exception("Error");
            }


            //Elõreléptetés speciális esetei: elágazások
            switch (model.PlayerLocation(model.ActualPlayer))
            {
                case 12: //Egyetemista végzett
                    model.SetPlayerLocation(model.ActualPlayer, 17);
                    break;

                case 56: //FORK1
                    model.SetPlayerLocation(model.ActualPlayer, 64);
                    break;

                case 93: //FORK2
                    model.SetPlayerLocation(model.ActualPlayer, 98);
                    break;
                default:
                    model.SetPlayerLocation(model.ActualPlayer, model.PlayerLocation(model.ActualPlayer)+1);
                    break;
            }

            --stepsLeft;

            Console.WriteLine(model.ActualPlayer + ". jatekos a " + model.PlayerLocation(model.ActualPlayer) + " mezore lepett. Meg " + stepsLeft + "-ot fog lepni.");

            //Ha STOP-ra lépett
            if (locationsOfStops.Contains(model.PlayerLocation(model.ActualPlayer)))
            {
                stepsLeft = 0;
            }

            //Ha fizetésnapra lépett
            if (locationsOfPayDays.Contains(model.PlayerLocation(model.ActualPlayer)))
            {
                model.PayDay(model.ActualPlayer);
                //TODO kimenet: fizetésnap
            }

            //Ha nyugdíjba kell menni, a játékosnak ki kell választania, hova megy nyugdíjba
            if (model.PlayerLocation(model.ActualPlayer) == 149)
            {
                gameState = State.CHOOSERETIREMENT;
            }
        }

        /// <summary>
        /// Kifejti a paraméterben megadott sorszámú mezõ hatását.
        /// <param name="fieldNumber">A megadott mezõ sorszáma, melyre rálépett a játékos.</param>
        /// </summary>
        private void EffectOfField(int fieldNumber)
        {

        }



        #endregion
        #region Draw



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            switch (gameState)
            {
                case State.MAINMENU:
                    spriteBatch.Draw(menuBackground, new Rectangle(0, 0, 956, 835), Color.White);

                    spriteBatch.Draw(newGameBtn, new Rectangle(400, 370, 158, 58), Color.White);
                    spriteBatch.Draw(openGameBtn, new Rectangle(400, 448, 158, 58), Color.White);
                    spriteBatch.Draw(instructionsBtn, new Rectangle(400, 526, 158, 58), Color.White);
                    spriteBatch.Draw(escapeBtn, new Rectangle(400, 604, 158, 58), Color.White);
                    spriteBatch.Draw(arrow, new Rectangle(365, 383 + arrowPosition * 78, 31, 31), Color.White);
                    break;

                case State.INSTRUCTIONS:
                    GraphicsDevice.Clear(new Color(51,88,161));
                    String title = "A JÁTÉK SZABÁLYAI";
                    String instructions = "A játék célja:\nA játékosnak mindenkinél több pénzt kell összegyûjtenie, mielõtt mindenki nyugdíjba megy.\n\nA játék menete:\nA játék kezdésekor a játékosok eldöntik, hogy egyetemre mennek, vagy azonnal a \nkarrier-építésbe kezdenek. \nA játékosok egymást követõen megforgatják a pörgetõt, elõrehaladnak a táblán és mindig azt \nteszik, ami az adott mezõn szerepel. \nMinden játékos felveszi a fizetését, ha egy zöld mezõre érkezik, vagy áthalad rajta. \nMinden játékos magához vesz egy életzsetont, ha ÉLET feliratú mezõre lép. \nAmikor egy játékos célba ér, eldönti, hogyan megy nyugdíjba. \nA játék végén a játékosok összeszámolják készpénzüket, és életzsetonjaik értékét. \nA leggazdagabb játékos nyer.";
                    spriteBatch.DrawString(titleFont, title, new Vector2(30, 30), Color.White);
                    spriteBatch.DrawString(instructionsFont, instructions, new Vector2(30, 70), Color.White);
                    break;

                case State.NUMBEROFPLAYERS:
                    spriteBatch.Draw(choosePlayerBackground, new Rectangle(0, 0, 956, 835), Color.White);

                    switch (numberOfPlayers[arrowPosition])
                    {
                        case 1:
                            img[arrowPosition] = man;
                            break;
                        case 2:
                            img[arrowPosition] = woman;
                            break;
                        case 3:
                            img[arrowPosition] = manAI;
                            break;
                        case 4:
                            img[arrowPosition] = womanAI;
                            break;
                        case 5:
                            img[arrowPosition] = emptyProfile;
                            break;
                    }
                    
                    spriteBatch.Draw(img[0], new Rectangle(138, 180, 200, 200), Color.White);
                    spriteBatch.Draw(img[1], new Rectangle(378, 180, 200, 200), Color.White);
                    spriteBatch.Draw(img[2], new Rectangle(618, 180, 200, 200), Color.White);
                    spriteBatch.Draw(img[3], new Rectangle(138, 420, 200, 200), Color.White);
                    spriteBatch.Draw(img[4], new Rectangle(378, 420, 200, 200), Color.White);
                    spriteBatch.Draw(img[5], new Rectangle(618, 420, 200, 200), Color.White);

                    spriteBatch.Draw(arrowUp, new Rectangle(223 + (arrowPosition % 3) * 240, 383 + Convert.ToInt32(arrowPosition > 2) * 240, 31, 31), Color.White);
                    break;

                case State.COLLEGEORCAREER: case State.PLAYERSTURN:
                    spriteBatch.Draw(palya2, new Rectangle(0, 0, 956, 835), Color.White);

                    spriteBatch.Draw(saveBtn, new Rectangle(660, 15, 105, 35), Color.White);
                    spriteBatch.Draw(escapeBtn2, new Rectangle(820, 15, 105, 35), Color.White);

                    spriteBatch.Draw(buyHouseInsBtn, new Rectangle(343, 660, 143, 53), Color.White);
                    spriteBatch.Draw(buyCarInsBtn, new Rectangle(526, 660, 143, 53), Color.White);
                    spriteBatch.Draw(buyStockBtn, new Rectangle(343, 743, 143, 53), Color.White);
                    spriteBatch.Draw(payBackLoanBtn, new Rectangle(526, 743, 143, 53), Color.White);

                    spriteBatch.Draw(spinBtn, new Rectangle(60, 680, 143, 97), Color.White);
                    int arrowX, arrowY;
                    switch(arrowPosition)
                    {
                        case 0:
                            arrowX = 26; arrowY = 713; break;
                        case 1:
                            arrowX = 310; arrowY = 670; break;
                        case 2:
                            arrowX = 492; arrowY = 670; break;
                        case 3:
                            arrowX = 310; arrowY = 755; break;
                        case 4:
                            arrowX = 492; arrowY = 755; break;
                        case 5:
                            arrowX = 626; arrowY = 18; break;
                        default:
                            arrowX = 786; arrowY = 18; break;
                    }
                    spriteBatch.Draw(arrow, new Rectangle(arrowX, arrowY, 31, 31), Color.White);

                    String playersName = model.PlayerName(model.ActualPlayer);
                    spriteBatch.DrawString(titleFont, playersName, new Vector2(20, 595), Color.White);
                    String playersMoney = "$ " + model.PlayerMoney(model.ActualPlayer).ToString();
                    spriteBatch.DrawString(titleFont, playersMoney, new Vector2(250, 595), Color.White);
                    String playersLoan = "$ " + model.PlayerLoan(model.ActualPlayer).ToString();
                    spriteBatch.DrawString(titleFont, playersLoan, new Vector2(430, 595), Color.White);
                    String playersCard = model.PlayerLifeCardNumber(model.ActualPlayer).ToString();
                    spriteBatch.DrawString(titleFont, playersCard, new Vector2(630, 595), Color.White);

                    if (gameState == State.COLLEGEORCAREER)
                    {
                        String choose = "Egyetem (1-es gomb) vagy karrier (2-es gomb)?";
                        spriteBatch.DrawString(titleFont, choose, new Vector2(10, 10), Color.White);
                    }

                    if (model.PlayerGender(model.ActualPlayer)) { 
                        spriteBatch.Draw(girl, new Rectangle(780, 595, 20, 52), Color.White);
                        if (model.PlayerMarried(model.ActualPlayer)) {
                            spriteBatch.Draw(boy, new Rectangle(805, 594, 20, 52), Color.White);
                            if (model.PlayerChildrenNumber(model.ActualPlayer) >= 1){
                                spriteBatch.Draw(empty, new Rectangle(830, 595, 20, 52), Color.White);
                                if (model.PlayerChildrenNumber(model.ActualPlayer) == 2) {
                                    spriteBatch.Draw(empty, new Rectangle(855, 595, 20, 52), Color.White);
                                }
                            }
                        }
                    }
                    else {
                        spriteBatch.Draw(boy, new Rectangle(775, 588, 20, 52), Color.White);
                        if (model.PlayerMarried(model.ActualPlayer)) {
                            spriteBatch.Draw(girl, new Rectangle(805, 594, 20, 52), Color.White);
                            if (model.PlayerChildrenNumber(model.ActualPlayer) >= 1){
                                spriteBatch.Draw(empty, new Rectangle(830, 595, 20, 52), Color.White);
                                if (model.PlayerChildrenNumber(model.ActualPlayer) == 2) {
                                    spriteBatch.Draw(empty, new Rectangle(855, 595, 20, 52), Color.White);
                                }
                            }
                        }
                    }
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }



        #endregion
    }
}
