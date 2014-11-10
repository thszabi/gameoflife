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
        enum State { MAINMENU, INSTRUCTIONS, LOADGAME, NUMBEROFPLAYERS, COLLEGEORWORK,
                     PLAYERSTURN, SAVEGAME, MOVING, CHOOSESTOCK, CHOOSEJOB, CHOOSESALARY,
                     CHANGEJOB, ATFORK, TRADESALARY, TRADEWITHWHO, CHOOSERETIREMENT, QUITTING, TEST };

        #region Variables for Update



        private DataModel.DataModel model;

        private State gameState;

        private int arrowPosition;



        #endregion
        #region Variables for Draw



        GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //Fõmenühöz
        private Texture2D arrow;
        private Texture2D newGameBtn;
        private Texture2D instructionsBtn;
        private Texture2D openGameBtn;
        private Texture2D escapeBtn;
        private Texture2D menuBackground; 
        
        //
        private Texture2D boy;
        private Texture2D girl;
        private Texture2D empty;


        private Texture2D saveBtn;
        private Texture2D escapeBtn2;
        private Texture2D spinBtn;

        private Texture2D getLoanBtn;
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

            boy= Content.Load<Texture2D>("boy");
            girl= Content.Load<Texture2D>("girl");
            empty = Content.Load<Texture2D>("empty");
            saveBtn = Content.Load<Texture2D>("save_btn");
            escapeBtn2 = Content.Load<Texture2D>("esc_btn2");
            spinBtn = Content.Load<Texture2D>("spin_btn");
            getLoanBtn = Content.Load<Texture2D>("get_loan_btn");
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
            switch (gameState)
            {
                case State.MAINMENU:
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Up))
                    {
                        arrowPosition = arrowPosition == 0 ? 3 : arrowPosition - 1;
                    }
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Down))
                    {
                        arrowPosition = (arrowPosition + 1) % 4;
                    }
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Space))
                    {
                        switch (arrowPosition)
                        {
                            case 0:
                                //New game
                                gameState = State.TEST;
                                break;
                            case 1:
                                //Load game
                                break;
                            case 2:
                                gameState = State.INSTRUCTIONS;
                                break;
                            default:
                                //"Are you sure?..."
                                this.Exit();
                                break;
                        }
                    }
                    break;

                case State.INSTRUCTIONS:
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                    {
                        gameState = State.MAINMENU;
                    }
                    break;
            }//switch (gameState)

            base.Update(gameTime);
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

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 835;
            graphics.PreferredBackBufferWidth = 956;
            graphics.ApplyChanges();

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

                case State.TEST:
                    graphics.IsFullScreen = false;
                    graphics.PreferredBackBufferHeight = 835;
                    graphics.PreferredBackBufferWidth = 956;
                    graphics.ApplyChanges();
                    spriteBatch.Draw(palya2, new Rectangle(0, 0, 956, 835), Color.White);

                    spriteBatch.Draw(saveBtn, new Rectangle(680, 15, 105, 35), Color.White);
                    spriteBatch.Draw(escapeBtn2, new Rectangle(820, 15, 105, 35), Color.White);

                    spriteBatch.Draw(buyHouseInsBtn, new Rectangle(60, 660, 143, 53), Color.White);
                    spriteBatch.Draw(buyCarInsBtn, new Rectangle(243, 660, 143, 53), Color.White);
                    spriteBatch.Draw(buyStockBtn, new Rectangle(426, 660, 143, 53), Color.White);
                    spriteBatch.Draw(getLoanBtn, new Rectangle(60, 743, 143, 53), Color.White);
                    spriteBatch.Draw(payBackLoanBtn, new Rectangle(243, 743, 143, 53), Color.White);

                    spriteBatch.Draw(spinBtn, new Rectangle(650, 680, 143, 97), Color.White);

                    String playersName = "Játékos 1"; //model.PlayerName(model.ActualPlayer);
                    spriteBatch.DrawString(titleFont, playersName, new Vector2(20, 595), Color.White);
                    String playersMoney = "$ 1000"; // "$ " + model.PlayerMoney(model.ActualPlayer);
                    spriteBatch.DrawString(titleFont, playersMoney, new Vector2(250, 595), Color.White);
                    String playersLoan = "$ 300"; //"$ " + model.PlayerLoan(model.ActualPlayer);
                    spriteBatch.DrawString(titleFont, playersLoan, new Vector2(440, 595), Color.White);
                    String playersCard = "3"; //model.PlayerLifeCardNumber(model.ActualPlayer);
                    spriteBatch.DrawString(titleFont, playersCard, new Vector2(630, 595), Color.White);

                    //if (model.PlayerGender(model.ActualPlayer)) { 
                        spriteBatch.Draw(girl, new Rectangle(780, 595, 20, 52), Color.White);
                        //if (model.PlayerMarried(model.ActualPlayer)) {
                            spriteBatch.Draw(boy, new Rectangle(805, 594, 20, 52), Color.White);
                            //if (model.PlayerChildrenNumber(model.ActualPlayer) >= 1){
                                spriteBatch.Draw(empty, new Rectangle(830, 595, 20, 52), Color.White);
                                //if (model.PlayerChildrenNumber(model.ActualPlayer) == 2) {
                                    spriteBatch.Draw(empty, new Rectangle(855, 595, 20, 52), Color.White);
                                //}
                            //}
                        //}
                    //}
                    //else {
                        //spriteBatch.Draw(boy, new Rectangle(775, 588, 20, 52), Color.White);
                        //if (model.PlayerMarried(model.ActualPlayer)) {
                            //spriteBatch.Draw(girl, new Rectangle(805, 594, 20, 52), Color.White);
                            //if (model.PlayerChildrenNumber(model.ActualPlayer) >= 1){
                                //spriteBatch.Draw(empty, new Rectangle(830, 595, 20, 52), Color.White);
                                //if (model.PlayerChildrenNumber(model.ActualPlayer) == 2) {
                                    //spriteBatch.Draw(empty, new Rectangle(855, 595, 20, 52), Color.White);
                                //}
                            //}
                        //}
                    //}
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }



        #endregion
    }
}
