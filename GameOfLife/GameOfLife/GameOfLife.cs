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
        enum State { MAINMENU, INSTRUCTIONS, QUITTING, NUMBEROFPLAYERS, COLLEGEORCAREER, 
                     PLAYERSTURN, MOVING, CHOOSESTOCK, CHOOSEJOB, CHOOSESALARY, 
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

        private readonly int[] locationsOfLIFEs = { 4, 7, 9, 11, 18, 20, 23, 24, 29, 37, 53, 54, 
                                                    63, 65, 66, 71, 74, 83, 89, 100, 103, 104, 
                                                    123, 130, 132, 135, 138, 140, 142, 144 };
        
        private DataModel.DataModel model;
        private ComputerAI.ComputerAI computer;

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

        private bool gameEnded; //Igaz, ha mindenki a célba ért, és kiírásra került a végeredmény. 
        private double elapsedSinceMoving; //Mennyi idõ telt el az utolsó stepForward óta
        
        private int stepsLeft; //Hányat lép még a játékos a pörgetés után
        private int numberSpun; //Hányat pörgetett a játékos
        private List<Int32> threeCareers;
        private List<Int32> threeSalaries;



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

        private Texture2D houseInsImg;
        private Texture2D carInsImg;

        private Texture2D houseImg;
        private Texture2D collegeImg;
        private Texture2D careerImg;


        Texture2D[] stockes = new Texture2D[9];
        private Texture2D stock1;
        private Texture2D stock2;
        private Texture2D stock3;
        private Texture2D stock4;
        private Texture2D stock5;
        private Texture2D stock6;
        private Texture2D stock7;
        private Texture2D stock8;
        private Texture2D stock9;

        Texture2D[] careers = new Texture2D[9];
        private Texture2D career1;
        private Texture2D career2;
        private Texture2D career3;
        private Texture2D career4;
        private Texture2D career5;
        private Texture2D career6;
        private Texture2D career7;
        private Texture2D career8;
        private Texture2D career9;

        Texture2D[] houses = new Texture2D[9];
        private Texture2D house1;
        private Texture2D house2;
        private Texture2D house3;
        private Texture2D house4;
        private Texture2D house5;
        private Texture2D house6;
        private Texture2D house7;
        private Texture2D house8;
        private Texture2D house9;

        Texture2D[] salaries = new Texture2D[9];
        private Texture2D salary1;
        private Texture2D salary2;
        private Texture2D salary3;
        private Texture2D salary4;
        private Texture2D salary5;
        private Texture2D salary6;
        private Texture2D salary7;
        private Texture2D salary8;
        private Texture2D salary9;

        private SpriteFont instructionsFont;
        private SpriteFont titleFont;

        private String output; //Ez a szöveg jelenik meg a bal felsõ sarokban. Ez mindig látható, ezzel tájékoztatjuk a játékost

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

            gameEnded = true;
            elapsedSinceMoving = 0;
            
            stepsLeft = 0;
            numberSpun = 0;

            output = "";

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

            houseInsImg = Content.Load<Texture2D>("house_ins");
            carInsImg = Content.Load<Texture2D>("car_ins");

            houseImg = Content.Load<Texture2D>("house");
            collegeImg = Content.Load<Texture2D>("college");
            careerImg = Content.Load<Texture2D>("career");

            stock1 = Content.Load<Texture2D>("stock_1");
            stock2 = Content.Load<Texture2D>("stock_2");
            stock3 = Content.Load<Texture2D>("stock_3");
            stock4 = Content.Load<Texture2D>("stock_4");
            stock5 = Content.Load<Texture2D>("stock_5");
            stock6 = Content.Load<Texture2D>("stock_6");
            stock7 = Content.Load<Texture2D>("stock_7");
            stock8 = Content.Load<Texture2D>("stock_8");
            stock9 = Content.Load<Texture2D>("stock_9");
            stockes[0] = stock1;
            stockes[1] = stock2;
            stockes[2] = stock3;
            stockes[3] = stock4;
            stockes[4] = stock5;
            stockes[5] = stock6;
            stockes[6] = stock7;
            stockes[7] = stock8;
            stockes[8] = stock9;

            career1 = Content.Load<Texture2D>("career1");
            career2 = Content.Load<Texture2D>("career2");
            career3 = Content.Load<Texture2D>("career3");
            career4 = Content.Load<Texture2D>("career4");
            career5 = Content.Load<Texture2D>("career5");
            career6 = Content.Load<Texture2D>("career6");
            career7 = Content.Load<Texture2D>("career7");
            career8 = Content.Load<Texture2D>("career8");
            career9 = Content.Load<Texture2D>("career9");
            careers[0] = career1;
            careers[1] = career2;
            careers[2] = career3;
            careers[3] = career4;
            careers[4] = career5;
            careers[5] = career6;
            careers[6] = career7;
            careers[7] = career8;
            careers[8] = career9;

            salary1 = Content.Load<Texture2D>("salary1");
            salary2 = Content.Load<Texture2D>("salary2");
            salary3 = Content.Load<Texture2D>("salary3");
            salary4 = Content.Load<Texture2D>("salary4");
            salary5 = Content.Load<Texture2D>("salary5");
            salary6 = Content.Load<Texture2D>("salary6");
            salary7 = Content.Load<Texture2D>("salary7");
            salary8 = Content.Load<Texture2D>("salary8");
            salary9 = Content.Load<Texture2D>("salary9");
            salaries[0] = salary1;
            salaries[1] = salary2;
            salaries[2] = salary3;
            salaries[3] = salary4;
            salaries[4] = salary5;
            salaries[5] = salary6;
            salaries[6] = salary7;
            salaries[7] = salary8;
            salaries[8] = salary9;

            house1 = Content.Load<Texture2D>("house1");
            house2 = Content.Load<Texture2D>("house2");
            house3 = Content.Load<Texture2D>("house3");
            house4 = Content.Load<Texture2D>("house4");
            house5 = Content.Load<Texture2D>("house5");
            house6 = Content.Load<Texture2D>("house6");
            house7 = Content.Load<Texture2D>("house7");
            house8 = Content.Load<Texture2D>("house8");
            house9 = Content.Load<Texture2D>("house9");
            houses[0] = house1;
            houses[1] = house2;
            houses[2] = house3;
            houses[3] = house4;
            houses[4] = house5;
            houses[5] = house6;
            houses[6] = house7;
            houses[7] = house8;
            houses[8] = house9;

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
            //TODO modellbe: ha valaki nem egyetemista, ne kapjon egyetemista munkát
            //TODO Drawba:   részvény megjelenítése, ha vett egyet
            newKeyboardState = Keyboard.GetState();

            switch (gameState)
            {
                #region Update: MAINMENU
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
                                gameEnded = false;
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
                #endregion

                #region Update: INSTRUCTIONS
                case State.INSTRUCTIONS:
                    //"Esc", "Space", "Enter" lekezelése - visszalépés
                    if ((oldKeyboardState.IsKeyUp(Keys.Escape) && newKeyboardState.IsKeyDown(Keys.Escape)) ||
                        (oldKeyboardState.IsKeyUp(Keys.Space)  && newKeyboardState.IsKeyDown(Keys.Space) )  ||
                        (oldKeyboardState.IsKeyUp(Keys.Enter)  && newKeyboardState.IsKeyDown(Keys.Enter) ) )
                    {
                        gameState = State.MAINMENU;
                    }
                    break;
                #endregion

                #region Update: NUMBEROFPLAYERS
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

                            int i = 0;
                            foreach (int player in numberOfPlayers)
                            {
                                switch (player)
                                {
                                    case 1: //Férfi és ember
                                        ++i;
                                        playerList.Add(new Tuple<string, bool, bool>(i+". játékos", false, false));
                                        break;
                                    case 2: //Nõ és ember
                                        ++i;
                                        playerList.Add(new Tuple<string, bool, bool>(i+". játékos", true, false));
                                        break;
                                    case 3: //Férfi és gép
                                        ++i;
                                        playerList.Add(new Tuple<string, bool, bool>(i+". játékos", false, true));
                                        break;
                                    case 4: //Nõ és gép
                                        ++i;
                                        playerList.Add(new Tuple<string, bool, bool>(i+". játékos", true, true));
                                        break;
                                }
                            }
                            
                            model = new DataModel.DataModel(noOfPlayers, playerList);
                            computer = new ComputerAI.ComputerAI(model);
                            stepsLeft = 0;
                            numberSpun = 0;
                            arrowPosition = 0;
                            model.ActualPlayer = 0;
                            gameState = State.PLAYERSTURN;
                        }
                    }
                    break;
                #endregion

                #region Update: PLAYERSTURN
                case State.PLAYERSTURN:
                    if (model.PlayerLocation(model.ActualPlayer) != 0 &&
                        !model.IsRetired(model.ActualPlayer) &&
                        !model.PlayerLoseNextRound(model.ActualPlayer))
                    {
                        //"Fel" lekezelése - következõ menüpontra mutat a kurzor. De csak akkor, ha nem gép köre van
                        if (oldKeyboardState.IsKeyUp(Keys.Up) && newKeyboardState.IsKeyDown(Keys.Up) && !model.PlayerPc(model.ActualPlayer))
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
                        //"Le" lekezelése - elõzõ menüpontra mutat a kurzor. De csak akkor, ha nem gép köre van
                        if (oldKeyboardState.IsKeyUp(Keys.Down) && newKeyboardState.IsKeyDown(Keys.Down) && !model.PlayerPc(model.ActualPlayer))
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

                        //Ha gép köre van, õ dönt
                        if (model.PlayerPc(model.ActualPlayer))
                        {
                            arrowPosition = computer.computerTurn();
                        }

                        //"Space" és "Enter" lekezelése - menüpont aktiválása
                        //Vagy ha gép köre van, akkor az elõbb meghozott döntést aktiváljuk
                        if ((oldKeyboardState.IsKeyUp(Keys.Space) && newKeyboardState.IsKeyDown(Keys.Space) && !model.PlayerPc(model.ActualPlayer)) ||
                            (oldKeyboardState.IsKeyUp(Keys.Enter) && newKeyboardState.IsKeyDown(Keys.Enter) && !model.PlayerPc(model.ActualPlayer)) ||
                            model.PlayerPc(model.ActualPlayer))
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
                                    if (model.PayBackLoan(model.ActualPlayer,25000))
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
                                    }
                                    break;

                                case 5: //Mentés
                                    //TODO mentés
                                    break;

                                case 6: //Kilépés
                                    arrowPosition = 0;
                                    gameState = State.MAINMENU;
                                    break;

                                default: //Pörgetés
                                    spin();
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
                            output = "Egyetem (1-es gomb) vagy karrier (2-es gomb)?";
                        }

                        //Ha a játékos kimarad egy körbõl
                        if (model.PlayerLoseNextRound(model.ActualPlayer))
                        {
                            //TODO kimenet = a játékos kimaradt egy körbõl, megint te következel!
                            model.SetLoseNextRound(model.ActualPlayer, false);
                            EndTurn();
                        }

                        //Ha a játékos már nyugdíjas
                        if (model.IsRetired(model.ActualPlayer))
                        {
                            EndTurn();
                        }
                    }
                    break;
                #endregion

                #region Update: COLLEGEORCAREER
                case State.COLLEGEORCAREER:
                    //"1" lekezelése, ha nem gép köre van - a játékos az egyetemet választotta
                    //Ha gép köre van, és "1"-et mondott, akkor lekezeljük
                    if ((oldKeyboardState.IsKeyUp(Keys.D1) && newKeyboardState.IsKeyDown(Keys.D1) && !model.PlayerPc(model.ActualPlayer)) ||
                        (model.PlayerPc(model.ActualPlayer) && computer.firstFork() == Keys.D1) )
                    {
                        model.SetPlayerLocation(model.ActualPlayer,1);
                        model.GetStudentLoan(model.ActualPlayer);

                        gameState = State.PLAYERSTURN;
                        output = "";
                    }
                    //"2" lekezelése, ha nem gép köre van - a játékos a karriert választotta
                    //Ha gép köre van, és "2"-t mondott, akkor lekezeljük
                    if ((oldKeyboardState.IsKeyUp(Keys.D2) && newKeyboardState.IsKeyDown(Keys.D2) && !model.PlayerPc(model.ActualPlayer)) ||
                        (model.PlayerPc(model.ActualPlayer) && computer.firstFork() == Keys.D2) )
                    {
                        model.SetPlayerLocation(model.ActualPlayer,13);
                        model.GiveCareer(model.ActualPlayer);
                        model.GiveSalary(model.ActualPlayer);

                        gameState = State.PLAYERSTURN;
                        output = "";
                    }
                    break;
                #endregion

                #region Update: MOVING
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
                        elapsedSinceMoving += gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (elapsedSinceMoving >= 1000) //Ha az utolsó lépés óta eltelt legalább 1 másodperc, lépünk
                        {
                            stepForward();
                            elapsedSinceMoving = 0;
                        }
                        
                    }
                    //Ha elfogyott a lépésünk, aktiválódik a mezõ hatása
                    if (gameState == State.MOVING && stepsLeft == 0)
                    {
                        EffectOfField(model.PlayerLocation(model.ActualPlayer));
                        //Ha az EffectOfField nem módosította a játék állapotát, sem a hátralévõ lépések számát, akkor vége a körnek
                        if (gameState == State.MOVING && stepsLeft == 0)
                        {
                            EndTurn();
                        }
                    }
                    break;
                #endregion

                #region Update: ATFORK1
                case State.ATFORK1:
                    //"1" lekezelése, ha nem gép köre van - a játékost elõre léptetjük eggyel az 1. útvonalon
                    //Ha gép köre van, és "1"-et mondott, lekezeljük
                    if ((oldKeyboardState.IsKeyUp(Keys.D1) && newKeyboardState.IsKeyDown(Keys.D1) && !model.PlayerPc(model.ActualPlayer)) ||
                        (model.PlayerPc(model.ActualPlayer) && computer.atFork() == Keys.D1))
                    {
                        model.SetPlayerLocation(model.ActualPlayer, 51);
                        --stepsLeft;
                        gameState = State.MOVING;
                    }
                    //"2" lekezelése, ha nem gép köre van - a játékost elõre léptetjük eggyel az 2. útvonalon
                    //Ha gép köre van, és "2"-t mondott, lekezeljük
                    if ((oldKeyboardState.IsKeyUp(Keys.D2) && newKeyboardState.IsKeyDown(Keys.D2) && !model.PlayerPc(model.ActualPlayer)) ||
                        (model.PlayerPc(model.ActualPlayer) && computer.atFork() == Keys.D2))
                    {
                        model.SetPlayerLocation(model.ActualPlayer, 57);
                        --stepsLeft;
                        gameState = State.MOVING;
                    }
                    break;
                #endregion

                #region Update: ATFORK2
                case State.ATFORK2:
                    //"1" lekezelése, ha nem gép köre van - a játékost elõre léptetjük eggyel az 1. útvonalon
                    //Ha gép köre van, és "1"-et mondott, lekezeljük
                    if ((oldKeyboardState.IsKeyUp(Keys.D1) && newKeyboardState.IsKeyDown(Keys.D1) && !model.PlayerPc(model.ActualPlayer)) ||
                        (model.PlayerPc(model.ActualPlayer) && computer.atFork() == Keys.D1))
                    {
                        model.SetPlayerLocation(model.ActualPlayer, 87);
                        --stepsLeft;
                        gameState = State.MOVING;
                    }
                    //"2" lekezelése, ha nem gép köre van - a játékost elõre léptetjük eggyel a 2. útvonalon
                    //Ha gép köre van, és "2"-t mondott, lekezeljük
                    if ((oldKeyboardState.IsKeyUp(Keys.D2) && newKeyboardState.IsKeyDown(Keys.D2) && !model.PlayerPc(model.ActualPlayer)) ||
                        (model.PlayerPc(model.ActualPlayer) && computer.atFork() == Keys.D2))
                    {
                        model.SetPlayerLocation(model.ActualPlayer, 94);
                        --stepsLeft;
                        gameState = State.MOVING;
                    }
                    break;
                #endregion

                #region Update: CHOOSERETIREMENT
                case State.CHOOSERETIREMENT:
                    //Ha vége van a játéknak, akkor is ebben az állapotban vagyunk! Csak ilyenkor a gameEnded nem engedi hogy az enteren kívül bármit is nyomjunk

                    //"1" lekezelése, ha nem gép köre van - a játékos a vidéki házba megy nyugdíjba
                    //Ha gép köre van, és "1"-et mondott, lekezeljük
                    if (!gameEnded &&
                        ((oldKeyboardState.IsKeyUp(Keys.D1) && newKeyboardState.IsKeyDown(Keys.D1) && !model.PlayerPc(model.ActualPlayer)) ||
                        (model.PlayerPc(model.ActualPlayer) && computer.selectRetire() == Keys.D1)))
                    {
                        model.SetPlayerLocation(model.ActualPlayer, 151);
                        model.Retire(model.ActualPlayer, false);

                        //Ha mindenki nyugdíjba ment, kiszámoljuk a gyõztest, és befejezõdik a játék
                        if (model.IsEveryoneRetired)
                        {
                            int winner = model.EndGame()[0];
                            output = model.PlayerName(winner) + " nyert!";
                            gameEnded = true;
                        }
                        else
                        {
                            EndTurn(); //Ha még nem ment mindenki nyugdíjba, akkor jön a következõ játékos
                        }
                    }
                    //"1" lekezelése, ha nem gép köre van - a játékos a milliomosok nyaralójába megy nyugdíjba
                    //Ha gép köre van, és "2"-t mondott, lekezeljük
                    if (!gameEnded &&
                        ((oldKeyboardState.IsKeyUp(Keys.D2) && newKeyboardState.IsKeyDown(Keys.D2) && !model.PlayerPc(model.ActualPlayer)) ||
                        (model.PlayerPc(model.ActualPlayer) && computer.selectRetire() == Keys.D2)))
                    {
                        model.SetPlayerLocation(model.ActualPlayer, 150);
                        model.Retire(model.ActualPlayer, true);

                        //Ha mindenki nyugdíjba ment, kiszámoljuk a gyõztest, és befejezõdik a játék
                        if (model.IsEveryoneRetired)
                        {
                            int winner = model.EndGame()[0];
                            output = model.PlayerName(winner) + " nyert!";
                            gameEnded = true;
                        }
                        else
                        {
                            EndTurn(); //Ha még nem ment mindenki nyugdíjba, akkor jön a következõ játékos
                        }
                    }

                    //Ha vége van a játéknak, már csak egy enterre várunk, majd visszamegyünk a fõmenübe
                    if (oldKeyboardState.IsKeyUp(Keys.Enter) && newKeyboardState.IsKeyDown(Keys.Enter) && gameEnded)
                    {
                        arrowPosition = 0;
                        gameState = State.MAINMENU;
                    }
                    break;
                #endregion

                #region Update: CHOOSESTOCK
                case State.CHOOSESTOCK:
                    /* Az arrowPosition tárolja a kiválasztott részvényt
                     * 0 -> az 1. részvényt választotta
                     * 1 -> a  2. részvényt választotta
                     * ...
                     * 8 -> a  9. részvényt választotta
                     * 9 -> még nem választott
                     */

                    //"1" lekezelése, ha nem gép köre van - 1. részvényt választotta, ha az még szabad
                    if ((oldKeyboardState.IsKeyUp(Keys.D1) && newKeyboardState.IsKeyDown(Keys.D1) && !model.PlayerPc(model.ActualPlayer)) &&
                        model.GetStockCardAvailability(0))
                    {
                        arrowPosition = 0;
                    }
                    if ((oldKeyboardState.IsKeyUp(Keys.D2) && newKeyboardState.IsKeyDown(Keys.D2) && !model.PlayerPc(model.ActualPlayer)) &&
                        model.GetStockCardAvailability(1))
                    {
                        arrowPosition = 1;
                    }
                    if ((oldKeyboardState.IsKeyUp(Keys.D3) && newKeyboardState.IsKeyDown(Keys.D3) && !model.PlayerPc(model.ActualPlayer)) &&
                        model.GetStockCardAvailability(2))
                    {
                        arrowPosition = 2;
                    }
                    if ((oldKeyboardState.IsKeyUp(Keys.D4) && newKeyboardState.IsKeyDown(Keys.D4) && !model.PlayerPc(model.ActualPlayer)) &&
                        model.GetStockCardAvailability(3))
                    {
                        arrowPosition = 3;
                    }
                    if ((oldKeyboardState.IsKeyUp(Keys.D5) && newKeyboardState.IsKeyDown(Keys.D5) && !model.PlayerPc(model.ActualPlayer)) &&
                        model.GetStockCardAvailability(4))
                    {
                        arrowPosition = 4;
                    }
                    if ((oldKeyboardState.IsKeyUp(Keys.D6) && newKeyboardState.IsKeyDown(Keys.D6) && !model.PlayerPc(model.ActualPlayer)) &&
                        model.GetStockCardAvailability(5))
                    {
                        arrowPosition = 5;
                    }
                    if ((oldKeyboardState.IsKeyUp(Keys.D7) && newKeyboardState.IsKeyDown(Keys.D7) && !model.PlayerPc(model.ActualPlayer)) &&
                        model.GetStockCardAvailability(6))
                    {
                        arrowPosition = 6;
                    }
                    if ((oldKeyboardState.IsKeyUp(Keys.D8) && newKeyboardState.IsKeyDown(Keys.D8) && !model.PlayerPc(model.ActualPlayer)) &&
                        model.GetStockCardAvailability(7))
                    {
                        arrowPosition = 7;
                    }
                    if ((oldKeyboardState.IsKeyUp(Keys.D9) && newKeyboardState.IsKeyDown(Keys.D9) && !model.PlayerPc(model.ActualPlayer)) &&
                        model.GetStockCardAvailability(8))
                    {
                        arrowPosition = 8;
                    }

                    //Ha gép köre van, akkor a gép választ
                    if (model.PlayerPc(model.ActualPlayer))
                    {
                        arrowPosition = computer.buyStock();
                    }
                    
                    //Ha a játékos választott, megveszi az adott részvényt
                    if (arrowPosition != 9)
                    {
                        if (model.BuyStock(model.ActualPlayer, arrowPosition))
                        {
                            output = "Sikeresen megvásároltad a " + (arrowPosition+1) + ". részvényt.";
                            arrowPosition = 0;
                            gameState = State.PLAYERSTURN;
                        }
                        else
                        {
                            output = "Nincs elég pénzed részvényt vásárolni!";
                            arrowPosition = 3;
                            gameState = State.PLAYERSTURN;
                        }
                    }
                    break;
                #endregion

                #region Update: CHOOSEJOB
                case State.CHOOSEJOB:
                    /* Az arrowPosition tárolja a kiválasztott munkát
                     * 0 -> az 1. munkát választotta
                     * 1 -> a  2. munkát választotta
                     * 2 -> a  3. munkát választotta
                     * 3 -> még nem választott
                     */

                    if (oldKeyboardState.IsKeyUp(Keys.D1) && newKeyboardState.IsKeyDown(Keys.D1) && !model.PlayerPc(model.ActualPlayer))
                    {
                        arrowPosition = 0;
                    }
                    if (oldKeyboardState.IsKeyUp(Keys.D2) && newKeyboardState.IsKeyDown(Keys.D2) && !model.PlayerPc(model.ActualPlayer))
                    {
                        arrowPosition = 1;
                    }
                    if (oldKeyboardState.IsKeyUp(Keys.D3) && newKeyboardState.IsKeyDown(Keys.D3) && !model.PlayerPc(model.ActualPlayer))
                    {
                        arrowPosition = 2;
                    }

                    //Ha gép köre van, õ dönt
                    if (model.PlayerPc(model.ActualPlayer))
                    {
                        arrowPosition = computer.selectJob(threeCareers);
                    }

                    if (arrowPosition != 3)
                    {
                        //Beállítjuk a kiválasztott karriert
                        model.SetCareer(model.ActualPlayer, threeCareers[arrowPosition]);

                        //Beállítjuk a három fizetést, amit a munka kiválasztása után fogunk felkínálni
                        threeSalaries = model.GiveThreeSalary();

                        /*
                        //Ha a játékos egy kék mezõ segítségével érte el a munkaváltást (azaz nem most végzett az egyetemen), akkor az elsõ fizetés az eddigi fizetése
                        if (model.PlayerLocation(model.ActualPlayer) != 12)
                        {
                            threeSalaries[0] = model.PlayerSalary(model.ActualPlayer);
                        }
                        */

                        Console.WriteLine("A három fizetés: " + threeSalaries[0] + " " + threeSalaries[1] + " " + threeSalaries[2]);

                        //A karrier kiválasztása után következik a fizetés kiválasztása
                        arrowPosition = 3;
                        gameState = State.CHOOSESALARY;
                    }

                    break;
                #endregion

                #region Update: CHOOSESALARY
                case State.CHOOSESALARY:
                    /* Az arrowPosition tárolja a kiválasztott fizetést
                     * 0 -> az 1. fizetést választotta
                     * 1 -> a  2. fizetést választotta
                     * 2 -> a  3. fizetést választotta
                     * 3 -> még nem választott
                     */

                    if (oldKeyboardState.IsKeyUp(Keys.D1) && newKeyboardState.IsKeyDown(Keys.D1) && !model.PlayerPc(model.ActualPlayer))
                    {
                        arrowPosition = 0;
                    }
                    if (oldKeyboardState.IsKeyUp(Keys.D2) && newKeyboardState.IsKeyDown(Keys.D2) && !model.PlayerPc(model.ActualPlayer))
                    {
                        arrowPosition = 1;
                    }
                    if (oldKeyboardState.IsKeyUp(Keys.D3) && newKeyboardState.IsKeyDown(Keys.D3) && !model.PlayerPc(model.ActualPlayer))
                    {
                        arrowPosition = 2;
                    }

                    //Ha gép köre van, õ dönt
                    if (model.PlayerPc(model.ActualPlayer))
                    {
                        arrowPosition = computer.selectSalary(threeSalaries);
                    }

                    if (arrowPosition != 3)
                    {
                        //Beállítjuk a kiválasztott fizetést
                        model.SetSalary(model.ActualPlayer, threeSalaries[arrowPosition]);

                        //Ha a játékos most végzett az egyetemen, újra pörgethet
                        if (model.PlayerLocation(model.ActualPlayer) == 12)
                        {
                            spin();
                        }
                        else
                        {
                            EndTurn();
                        }
                    }

                    break;
                #endregion

                #region Update: CHANGEJOB
                case State.CHANGEJOB:

                    //"I" lekezelése, ha nem gép köre van - elkezdjük a munkaváltást
                    //Ha gép köre van, és szeretne munkát váltani, lekezeljük
                    if ((oldKeyboardState.IsKeyUp(Keys.I) && newKeyboardState.IsKeyDown(Keys.I) && !model.PlayerPc(model.ActualPlayer)) ||
                        (model.PlayerPc(model.ActualPlayer) && computer.blueFieldChangeJob()))
                    {
                        //Fizetünk a tanárnak 20000-et
                        model.PayForSomeone(20000, model.ActualPlayer, 6);

                        //Kisorsoljuk a három állást, amelybõl az 1. az eddigi munkája
                        threeCareers = model.GiveThreeCareer();
                        threeCareers[0] = model.PlayerCareerCard(model.ActualPlayer);

                        Console.WriteLine("A három karrier: " + threeCareers[0] + " " + threeCareers[1] + " " + threeCareers[2]);
                        arrowPosition = 3; //== még nem választott
                        gameState = State.CHOOSEJOB;
                    }

                    //"N" lekezelése, ha nem gép köre van - vége van a körnek
                    //Ha gép köre van, és nem szeretne munkát váltani, lekezeljük
                    if ((oldKeyboardState.IsKeyUp(Keys.N) && newKeyboardState.IsKeyDown(Keys.N) && !model.PlayerPc(model.ActualPlayer)) ||
                        (model.PlayerPc(model.ActualPlayer) && !computer.blueFieldChangeJob()))
                    {
                        EndTurn();
                    }
                    break;
                #endregion

                #region Update: TRADESALARY
                case State.TRADESALARY:

                    //"I" lekezelése, ha nem gép köre van - elkezdjük a cserét
                    //Ha gép köre van, és szeretne cserélni, lekezeljük
                    if ((oldKeyboardState.IsKeyUp(Keys.I) && newKeyboardState.IsKeyDown(Keys.I) && !model.PlayerPc(model.ActualPlayer)) ||
                        (model.PlayerPc(model.ActualPlayer) && computer.blueFieldTradeSalary()))
                    {
                        arrowPosition = 6; //== még nem választott
                        gameState = State.TRADEWITHWHO;
                    }

                    //"N" lekezelése, ha nem gép köre van - vége van a körnek
                    //Ha gép köre van, és nem szeretne cserélni, lekezeljük
                    if ((oldKeyboardState.IsKeyUp(Keys.N) && newKeyboardState.IsKeyDown(Keys.N) && !model.PlayerPc(model.ActualPlayer)) ||
                        (model.PlayerPc(model.ActualPlayer) && !computer.blueFieldTradeSalary()))
                    {
                        EndTurn();
                    }
                    break;
                #endregion

                #region Update: TRADEWITHWHO
                case State.TRADEWITHWHO:
                    /* Az arrowPosition tárolja a kiválasztott játékost
                     * 0 -> az 1. játékost választotta
                     * 1 -> a  2. játékost választotta
                     * ...
                     * 5 -> a  6. játékost választotta
                     * 6 -> még nem választott
                     */

                    //"1" lekezelése, ha nem gép köre van, és létezik a kiválasztott játékos, és nem önmagát választotta - az 1. játékost választotta
                    if ((oldKeyboardState.IsKeyUp(Keys.D1) && newKeyboardState.IsKeyDown(Keys.D1) && !model.PlayerPc(model.ActualPlayer)) &&
                        model.ActualPlayer != 0 && //1. játékos indexe = 0
                        1 <=model.NumberOfPlayers)
                    {
                        arrowPosition = 0;
                    }
                    if ((oldKeyboardState.IsKeyUp(Keys.D2) && newKeyboardState.IsKeyDown(Keys.D2) && !model.PlayerPc(model.ActualPlayer)) &&
                        model.ActualPlayer != 1 &&
                        2 <= model.NumberOfPlayers)
                    {
                        arrowPosition = 1;
                    }
                    if ((oldKeyboardState.IsKeyUp(Keys.D3) && newKeyboardState.IsKeyDown(Keys.D3) && !model.PlayerPc(model.ActualPlayer)) &&
                        model.ActualPlayer != 2 &&
                        3 <= model.NumberOfPlayers)
                    {
                        arrowPosition = 2;
                    }
                    if ((oldKeyboardState.IsKeyUp(Keys.D4) && newKeyboardState.IsKeyDown(Keys.D4) && !model.PlayerPc(model.ActualPlayer)) &&
                        model.ActualPlayer != 3 &&
                        4 <= model.NumberOfPlayers)
                    {
                        arrowPosition = 3;
                    }
                    if ((oldKeyboardState.IsKeyUp(Keys.D5) && newKeyboardState.IsKeyDown(Keys.D5) && !model.PlayerPc(model.ActualPlayer)) &&
                        model.ActualPlayer != 4 &&
                        5 <= model.NumberOfPlayers)
                    {
                        arrowPosition = 4;
                    }
                    if ((oldKeyboardState.IsKeyUp(Keys.D6) && newKeyboardState.IsKeyDown(Keys.D6) && !model.PlayerPc(model.ActualPlayer)) &&
                        model.ActualPlayer != 5 &&
                        6 <= model.NumberOfPlayers)
                    {
                        arrowPosition = 5;
                    }

                    //Ha gép köre van, õ dönt
                    if (model.PlayerPc(model.ActualPlayer))
                    {
                        arrowPosition = computer.tradeSalary();
                    }

                    //Ha a játékos döntött, kicserüljük a két fizetést
                    if (arrowPosition != 6)
                    {
                        model.TradeSalary(model.ActualPlayer, arrowPosition);
                        EndTurn();
                    }
                    break;
                #endregion

            }//switch (gameState)

            oldKeyboardState = newKeyboardState;
            base.Update(gameTime);
        }

        /// <summary>
        /// Pörgetés: értéket ad a stepsLeftnek (és a numberSpunnak), kiírja, ha valaki pénzt kapott a pörgetés miatt, végül gameState=MOVING
        /// </summary>
        private void spin()
        {
            Tuple<int, int> spinResult = model.Spin(model.ActualPlayer);
            stepsLeft = spinResult.Item1;
            numberSpun = spinResult.Item1;

            //Ha valaki pénzt kapott
            if (spinResult.Item2 != -1)
            {
                if (spinResult.Item1 == 10)
                {
                    //TODO kimenet: gyorshajtás! Az x. játékos kap 10.000-ret
                }
                else
                {
                    //TODO kimenet: az x. játékos kap 10.000-et a részvénye miatt.
                }
            }

            //Pörgetés után jön a léptetés
            gameState = State.MOVING;
            elapsedSinceMoving = 0;
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

            Console.WriteLine(model.PlayerName(model.ActualPlayer) + " a " + model.PlayerLocation(model.ActualPlayer) + " mezõre lépett. Még " + stepsLeft + "-t fog lépni.");

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

        }

        /// <summary>
        /// Kifejti a paraméterben megadott sorszámú mezõ hatását.
        /// <param name="fieldNumber">A megadott mezõ sorszáma, melyre rálépett a játékos.</param>
        /// </summary>
        private void EffectOfField(int fieldNumber)
        {
            if (locationsOfLIFEs.Contains(fieldNumber))
            {
                //TODO StealLifeCard
                model.GetLifeCard(model.ActualPlayer);
            }

            switch (fieldNumber)
            {
                case 2:
                    model.GiveMoney(model.ActualPlayer, 20000);
                    break;

                case 3:
                case 8:
                case 15:
                    model.PayMoney(model.ActualPlayer, 5000);
                    break;

                case 5:
                    model.GiveMoney(model.ActualPlayer, 5000);
                    break;

                case 6:
                case 10:
                case 19:
                case 26:
                    model.SetLoseNextRound(model.ActualPlayer, true);
                    break;

                case 12:
                    threeCareers = model.GiveThreeCareer();

                    Console.WriteLine("A három karrier: " + threeCareers[0] + " " + threeCareers[1] + " " + threeCareers[2]);
                    arrowPosition = 3;
                    gameState = State.CHOOSEJOB;
                    break;

                case 16:
                case 22:
                case 44:
                case 75:
                    model.GiveMoney(model.ActualPlayer, 10000);
                    break;

                case 21:
                    model.PayForSomeone(5000, model.ActualPlayer, 8);
                    break;

                case 27:
                    model.Marry(model.ActualPlayer);
                    model.GetLifeCard(model.ActualPlayer);
                    spin();
                    break;

                case 28:
                    model.PayMoney(model.ActualPlayer, 10000);
                    break;

                case 30:
                    model.PayForSomeone(10000, model.ActualPlayer, 3);
                    break;

                case 31:
                    if (!model.PlayerCarInsurance(model.ActualPlayer))
                    {
                        model.PayMoney(model.ActualPlayer, 10000);
                    }
                    break;

                case 32:
                    model.PayForSomeone(10000, model.ActualPlayer, 4);
                    break;

                case 33:
                case 70:
                    output = "Váltasz munkát? (I/N)";
                    gameState = State.CHANGEJOB;
                    break;

                case 35:
                case 78:
                case 97:
                case 115:
                case 128:
                    model.PayForSomeone(model.PlayerTax(model.ActualPlayer), model.ActualPlayer, 7);
                    break;

                case 36:
                    model.GiveMoney(model.ActualPlayer, 50000);
                    break;

                case 38:
                    model.GiveHouse(model.ActualPlayer);
                    spin();
                    break;

                case 40:
                case 118:
                    model.GiveCareer(model.ActualPlayer);
                    model.GiveSalary(model.ActualPlayer);
                    break;

                case 41:
                case 59:
                case 67:
                    model.OneChild(model.ActualPlayer, false);
                    model.GetLifeCard(model.ActualPlayer);
                    break;

                case 42:
                case 51:
                    model.PayForSomeone(5000, model.ActualPlayer, 3);
                    break;

                case 43:
                case 48:
                case 61:
                    model.OneChild(model.ActualPlayer, true);
                    model.GetLifeCard(model.ActualPlayer);
                    break;

                case 46:
                case 86:
                    model.TwoChildren(model.ActualPlayer);
                    model.GetLifeCard(model.ActualPlayer);
                    break; //TODO valszeg nem kell hogy fiú-e vagy lány

                case 47:
                    model.PayForSomeone(20000, model.ActualPlayer, 2);
                    break;

                case 49:
                    model.PayForSomeone(5000, model.ActualPlayer, 0);
                    break;

                case 50:
                    if (!model.PlayerHouseInsurance(model.ActualPlayer))
                    {
                        model.PayMoney(model.ActualPlayer, 40000);
                    }
                    break;

                case 52:
                        model.FreeStock(model.ActualPlayer);
                    break;

                case 56:
                    if (!model.PlayerCarInsurance(model.ActualPlayer))
                    {
                        model.PayMoney(model.ActualPlayer, 15000);
                    }
                    break;

                case 57:
                    model.PayForSomeone(5000, model.ActualPlayer, 8);
                    break;

                case 58:
                case 64:
                case 77:
                case 91:
                case 109:
                case 122:
                case 133:
                    output = "Elcseréled a fizetésed?";
                    gameState = State.TRADESALARY;
                    break;

                case 62:
                    if (!model.PlayerHouseInsurance(model.ActualPlayer))
                    {
                        model.PayMoney(model.ActualPlayer, 15000);
                    }
                    break;

                case 69:
                case 80:
                    model.PayForSomeone(25000, model.ActualPlayer, 4);
                    break;

                case 72:
                    model.PayForSomeone(20000, model.ActualPlayer, 1);
                    break;

                case 76:
                case 119:
                case 124:
                case 129:
                case 136:
                case 141:
                    if (!model.IsFirst(model.ActualPlayer))
                    {
                        spin();
                    }
                    break;

                case 79:
                    model.PayForSomeone(25000, model.ActualPlayer, 2);
                    break;

                case 82:
                case 111:
                    model.LoseStock(model.ActualPlayer);
                    break;

                case 84:
                case 102:
                    model.PayForSomeone(model.PlayerChildrenNumber(model.ActualPlayer) * 5000, model.ActualPlayer, 6);
                    break;

                case 85:
                case 96:
                    model.GiveMoney(model.ActualPlayer, 80000);
                    break;

                case 87:
                    model.PayForSomeone(15000, model.ActualPlayer, 0);
                    break;

                case 90:
                    model.PayForSomeone(35000, model.ActualPlayer, 1);
                    break;

                case 92:
                    model.PayForSomeone(25000, model.ActualPlayer, 3);
                    break;

                case 93:
                    model.GiveMoney(model.ActualPlayer, 75000);
                    break;

                case 94:
                    model.PayForSomeone(15000, model.ActualPlayer, 5);
                    break;

                case 99:
                    model.PayForSomeone(25000, model.ActualPlayer, 1);
                    break;

                case 101:
                    model.GiveMoney(model.ActualPlayer, 95000);
                    break;

                case 105:
                    model.PayMoney(model.ActualPlayer, 90000);
                    break;

                case 107:
                    if (!model.PlayerHouseInsurance(model.ActualPlayer))
                    {
                        model.PayMoney(model.ActualPlayer, 50000);
                    }
                    break;

                case 108:
                    model.GiveMoney(model.ActualPlayer, 100000);
                    break;

                case 110:
                    model.PayForSomeone(30000, model.ActualPlayer, 2);
                    break;

                case 112:
                    if (!model.PlayerHouseInsurance(model.ActualPlayer))
                    {
                        model.PayMoney(model.ActualPlayer, 125000);
                    }
                    break;

                case 114:
                    model.PayForSomeone(25000, model.ActualPlayer, 8);
                    break;

                case 116:
                    model.PayForSomeone(30000, model.ActualPlayer, 3);
                    break;

                case 117:
                    model.PayForSomeone(35000, model.ActualPlayer, 2);
                    break;

                case 121:
                    model.PayForSomeone(100000, model.ActualPlayer, 0);
                    break;

                case 125:
                    model.PayForSomeone(100000, model.ActualPlayer, 8);
                    break;

                case 126:
                    model.PayForSomeone(model.PlayerChildrenNumber(model.ActualPlayer) * 50000, model.ActualPlayer, 6);
                    break;

                case 131:
                    model.PayForSomeone(125000, model.ActualPlayer, 1);
                    break;

                case 137:
                    model.PayForSomeone(65000, model.ActualPlayer, 2);
                    break;

                case 143:
                    model.PayForSomeone(45000, model.ActualPlayer, 4);
                    break;

                case 146:
                    model.PayForSomeone(35000, model.ActualPlayer, 0);
                    break;

                case 147:
                    model.PayForSomeone(55000, model.ActualPlayer, 4);
                    break;

                case 148:
                    model.GiveMoney(model.ActualPlayer, numberSpun * 20000);
                    break;

                case 149:
                    gameState = State.CHOOSERETIREMENT;
                    break;


            }//switch (fieldNumber)
        }

        /// <summary>
        /// Véget vet a körnek, és jön a következõ játékos: meghívja a model.NextPlayer()-t, és gameState=PLAYERSTURN
        /// </summary>
        private void EndTurn()
        {
            arrowPosition = 0;
            model.NextPlayer();
            gameState = State.PLAYERSTURN;

            #region Writing to console

            Console.WriteLine();
            Console.WriteLine("A " + model.PlayerName(model.ActualPlayer) + " következik.");
            Console.WriteLine("Hol van: " + model.PlayerLocation(model.ActualPlayer));
            Console.WriteLine("Életkártyája: " + model.PlayerLifeCardNumber(model.ActualPlayer));
            Console.WriteLine("Gyerekek száma: " + model.PlayerChildrenNumber(model.ActualPlayer));
            String children = "Gyerekei: ";
            foreach (int child in model.PlayerChildren(model.ActualPlayer))
            {
                children = children + child + ", ";
            }
            Console.WriteLine(children);

            try
            {
                String job;
                switch (model.PlayerCareerCard(model.ActualPlayer))
                {
                    case 0:
                        job = " (Szupersztár)";
                        break;

                    case 1:
                        job = " (Mûvész)";
                        break;

                    case 2:
                        job = " (Sportoló)";
                        break;

                    case 3:
                        job = " (Üzletkötõ)";
                        break;

                    case 4:
                        job = " (Utazási ügynök)";
                        break;

                    case 5:
                        job = " (Rendõr)";
                        break;

                    case 6:
                        job = " (Tanár)";
                        break;

                    case 7:
                        job = " (Könyvelõ)";
                        break;

                    case 8:
                        job = " (Orvos)";
                        break;
                    default:
                        job = " (Semmi)";
                        break;
                }
                Console.WriteLine("Munkája: " + model.PlayerCareerCard(model.ActualPlayer) + job);
            }
            catch(Exception)
            {
                Console.WriteLine("Nincs munkája.");
            }

            try
            {
                Console.WriteLine("Fizetése: " + model.PlayerSalary(model.ActualPlayer));
            }
            catch (Exception)
            {
                Console.WriteLine("Nincs fizetése");
            }

            try
            {
                String house;
                switch (model.PlayerSalary(model.ActualPlayer))
                {
                    case 0:
                        house = " (40 000)";
                        break;

                    case 1:
                        house = " (60 000)";
                        break;

                    case 2:
                        house = " (80 000)";
                        break;

                    case 3:
                        house = " (100 000)";
                        break;

                    case 4:
                        house = " (120 000)";
                        break;

                    case 5:
                        house = " (140 000)";
                        break;

                    case 6:
                        house = " (160 000)";
                        break;

                    case 7:
                        house = " (180 000)";
                        break;

                    case 8:
                        house = " (200 000)";
                        break;
                    default:
                        house = " (0)";
                        break;
                }
                Console.WriteLine("Háza: " + model.PlayerHouseCard(model.ActualPlayer) + house);
            }
            catch(Exception)
            {
                Console.WriteLine("Nincs háza.");
            }
            #endregion
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
                #region Draw: MAINMENU
                case State.MAINMENU:
                    spriteBatch.Draw(menuBackground, new Rectangle(0, 0, 956, 835), Color.White);

                    spriteBatch.Draw(newGameBtn, new Rectangle(400, 370, 158, 58), Color.White);
                    spriteBatch.Draw(openGameBtn, new Rectangle(400, 448, 158, 58), Color.White);
                    spriteBatch.Draw(instructionsBtn, new Rectangle(400, 526, 158, 58), Color.White);
                    spriteBatch.Draw(escapeBtn, new Rectangle(400, 604, 158, 58), Color.White);
                    spriteBatch.Draw(arrow, new Rectangle(365, 383 + arrowPosition * 78, 31, 31), Color.White);
                    break;
                #endregion;

                #region Draw: INSTRUCTIONS
                case State.INSTRUCTIONS:
                    GraphicsDevice.Clear(new Color(51,88,161));
                    String title = "A JÁTÉK SZABÁLYAI";
                    String instructions = "A játék célja:\nA játékosnak mindenkinél több pénzt kell összegyûjtenie, mielõtt mindenki nyugdíjba megy.\n\nA játék menete:\nA játék kezdésekor a játékosok eldöntik, hogy egyetemre mennek, vagy azonnal a \nkarrier-építésbe kezdenek. \nA játékosok egymást követõen megforgatják a pörgetõt, elõrehaladnak a táblán és mindig azt \nteszik, ami az adott mezõn szerepel. \nMinden játékos felveszi a fizetését, ha egy zöld mezõre érkezik, vagy áthalad rajta. \nMinden játékos magához vesz egy életzsetont, ha ÉLET feliratú mezõre lép. \nAmikor egy játékos célba ér, eldönti, hogyan megy nyugdíjba. \nA játék végén a játékosok összeszámolják készpénzüket, és életzsetonjaik értékét. \nA leggazdagabb játékos nyer.";
                    spriteBatch.DrawString(titleFont, title, new Vector2(30, 30), Color.White);
                    spriteBatch.DrawString(instructionsFont, instructions, new Vector2(30, 70), Color.White);
                    break;
                #endregion

                #region Draw: NUMBEROFPLAYERS
                case State.NUMBEROFPLAYERS:
                    spriteBatch.Draw(choosePlayerBackground, new Rectangle(0, 0, 956, 835), Color.White);

                    switch (numberOfPlayers[arrowPosition])
                    {
                        case 0:
                            img[arrowPosition] = emptyProfile;
                            break;
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
                    }
                    
                    spriteBatch.Draw(img[0], new Rectangle(138, 180, 200, 200), Color.White);
                    spriteBatch.Draw(img[1], new Rectangle(378, 180, 200, 200), Color.White);
                    spriteBatch.Draw(img[2], new Rectangle(618, 180, 200, 200), Color.White);
                    spriteBatch.Draw(img[3], new Rectangle(138, 420, 200, 200), Color.White);
                    spriteBatch.Draw(img[4], new Rectangle(378, 420, 200, 200), Color.White);
                    spriteBatch.Draw(img[5], new Rectangle(618, 420, 200, 200), Color.White);

                    spriteBatch.Draw(arrowUp, new Rectangle(223 + (arrowPosition % 3) * 240, 383 + Convert.ToInt32(arrowPosition > 2) * 240, 31, 31), Color.White);
                    break;
                #endregion

                #region Draw: COLLEGEORCAREER
                case State.COLLEGEORCAREER:
                    DrawUI();
                    break;
                #endregion

                #region Draw: ATFORK1
                case State.ATFORK1:
                    DrawUI();
                    break;
                #endregion

                #region Draw: ATFORK2
                case State.ATFORK2:
                    DrawUI();
                    break;
                #endregion

                #region Draw: CHANGEJOB
                case State.CHANGEJOB:
                    DrawUI();
                    break;
                #endregion

                #region Draw: CHOOSEJOB
                case State.CHOOSEJOB:
                    DrawUI();
                    for (int i = 0; i < 3; ++i)
                    {
                        spriteBatch.Draw(careers[threeCareers[i]], new Rectangle(233 + (i % 3) * 170, 265, 150, 230), Color.White);
                    }
                    break;
                #endregion

                #region Draw: CHOOSERETIREMENT
                case State.CHOOSERETIREMENT:
                    DrawUI();
                    break;
                #endregion

                #region Draw: CHOOSESALARY
                case State.CHOOSESALARY:
                    DrawUI();
                    for (int i = 0; i < 3; ++i)
                    {
                        spriteBatch.Draw(salaries[threeSalaries[i]], new Rectangle(233 + (i % 3) * 170, 265, 150, 230), Color.White);
                    }
                    break;
                #endregion

                #region Draw: TRADEWITHWHO
                case State.TRADEWITHWHO:
                    DrawUI();
                    break;
                #endregion

                #region Draw: TRADESALARY
                case State.TRADESALARY:
                    DrawUI();
                    break;
                #endregion

                #region Draw: CHOOSESTOCK
                case State.CHOOSESTOCK:
                    DrawUI();
                    for (int i = 0; i < 9; ++i)
                    {
                        if (model.GetStockCardAvailability(i))
                        {
                            spriteBatch.Draw(stockes[i], new Rectangle(78 + (i % 5) * 160, 65 + (Convert.ToInt32(i > 4)) * 240, 150, 230), Color.White);
                        }
                    }
                    break;
                #endregion

                #region Draw: MOVING
                case State.MOVING:
                    DrawUI();
                    spriteBatch.DrawString(titleFont, numberSpun.ToString(), new Vector2(240, 710), Color.White);
                    break;
                #endregion

                #region Draw: PLAYERSTURN
                case State.PLAYERSTURN: //T.SZ. TODO: jöhet a case MOVING. Oda is kell egy DrawUI(), meg valahogy kirajzolni a megfelelõ helyre a bábut
                    DrawUI();

                    int arrowX, arrowY;
                    switch (arrowPosition)
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
                    break;
                #endregion
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawUI() //T.SZ. kiemeltem a közös részt. A kurzor kirajzolása nincs benne
        {
            /* Ezt a függvényt a következõképp kell használni.
            Egyáltalán nem kell használni:                   MAINMENU, INSTRUCTIONS, NUMBEROFPLAYERS
            Használni kell, majd utána a kurzort kirajzolni: PLAYERSTURN
            Használni kell, de utána nem kell a kurzor:      QUITTING, COLLEGEORCAREER, MOVING, CHOOSESTOCK, CHOOSEJOB, CHOOSESALARY, CHANGEJOB, ATFORK1, ATFORK2, TRADESALARY, TRADEWITHWHO, CHOOSERETIREMENT
            */
            spriteBatch.Draw(palya2, new Rectangle(0, 0, 956, 835), Color.White);

            spriteBatch.Draw(saveBtn, new Rectangle(660, 15, 105, 35), Color.White);
            spriteBatch.Draw(escapeBtn2, new Rectangle(820, 15, 105, 35), Color.White);

            // Ha a játékosnak van háza, és még nincs otthonbiztosítása...
            if (model.PlayerHouseCard(model.ActualPlayer) != 9 && (!model.PlayerHouseInsurance(model.ActualPlayer)))
            {
                spriteBatch.Draw(buyHouseInsBtn, new Rectangle(343, 660, 143, 53), Color.White);
            }
            //Ha a játékosnak van otthonbiztosítása...
            else if (model.PlayerHouseInsurance(model.ActualPlayer))
            {
                spriteBatch.Draw(houseInsImg, new Rectangle(343, 660, 143, 53), Color.White);
            }
            // Ha a játékosnak még nincs jármûbiztosítása...
            if (!model.PlayerCarInsurance(model.ActualPlayer))
            {
                spriteBatch.Draw(buyCarInsBtn, new Rectangle(526, 660, 143, 53), Color.White);
            }
            else
            {
                spriteBatch.Draw(carInsImg, new Rectangle(526, 660, 143, 53), Color.White);
            }
            // Ha a játékos még nem vásárolt részvényt...
            if (model.PlayerStockCard(model.ActualPlayer) == 9)
            {
                spriteBatch.Draw(buyStockBtn, new Rectangle(343, 743, 143, 53), Color.White);
            }
            // Ha a játékosnak van hitele...
            if (model.PlayerLoan(model.ActualPlayer) != 0)
            {
                spriteBatch.Draw(payBackLoanBtn, new Rectangle(526, 743, 143, 53), Color.White);
            }
            spriteBatch.Draw(spinBtn, new Rectangle(60, 680, 143, 97), Color.White);

            String playersName = model.PlayerName(model.ActualPlayer);
            spriteBatch.DrawString(titleFont, playersName, new Vector2(20, 595), Color.White);
            String playersMoney = "$ " + model.PlayerMoney(model.ActualPlayer).ToString();
            spriteBatch.DrawString(titleFont, playersMoney, new Vector2(250, 595), Color.White);
            String playersLoan = "$ " + model.PlayerLoan(model.ActualPlayer).ToString();
            spriteBatch.DrawString(titleFont, playersLoan, new Vector2(430, 595), Color.White);
            String playersCard = model.PlayerLifeCardNumber(model.ActualPlayer).ToString();
            spriteBatch.DrawString(titleFont, playersCard, new Vector2(630, 595), Color.White);

            spriteBatch.DrawString(titleFont, output, new Vector2(10, 10), Color.White); //T.SZ. itt volt egy if gameState = COLLEGEORCAREER, de már nem kell, ugyanis az outputot mindig ki kell írni a bal felsõ sarokba. A tartalmát az update adja meg

            if (model.PlayerGender(model.ActualPlayer))
            {
                spriteBatch.Draw(girl, new Rectangle(735, 595, 20, 52), Color.White);
                if (model.PlayerMarried(model.ActualPlayer))
                {
                    spriteBatch.Draw(boy, new Rectangle(760, 594, 20, 52), Color.White);
                    for (int i = 0; i < model.PlayerChildrenNumber(model.ActualPlayer); ++i)
                    {
                        spriteBatch.Draw(empty, new Rectangle(785 + i * 17, 607, 14, 36), Color.White);
                    }
                }
            }
            else
            {
                spriteBatch.Draw(boy, new Rectangle(735, 594, 20, 52), Color.White);
                if (model.PlayerMarried(model.ActualPlayer))
                {
                    spriteBatch.Draw(girl, new Rectangle(760, 595, 20, 52), Color.White);
                    for (int i = 0; i < model.PlayerChildrenNumber(model.ActualPlayer); ++i)
                    {
                        spriteBatch.Draw(empty, new Rectangle(785 + i * 17, 607, 14, 36), Color.White);
                    }
                }
            }
        }

        #endregion
    }
}