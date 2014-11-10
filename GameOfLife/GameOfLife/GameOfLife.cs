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
                     CHANGEJOB, ATFORK, TRADESALARY, TRADEWITHWHO, CHOOSERETIREMENT, QUITTING };

        #region Variables for Update



        private DataModel.DataModel model;

        private State gameState;

        private int arrowPosition;



        #endregion
        #region Variables for Draw



        GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Texture2D startGameBtn;
        private Texture2D arrow;
        private Texture2D newGameBtn;
        private Texture2D instructionsBtn;
        private Texture2D openGameBtn;
        private Texture2D escapeBtn;
        private Texture2D menu_img;

        private SpriteFont font;



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

            startGameBtn = Content.Load<Texture2D>("startgamebtn");
            arrow = Content.Load<Texture2D>("arrow");

            menu_img = Content.Load<Texture2D>("menu_img");

            newGameBtn = Content.Load<Texture2D>("new_game_btn");
            instructionsBtn = Content.Load<Texture2D>("instructions_btn");
            openGameBtn = Content.Load<Texture2D>("open_game_btn");
            escapeBtn = Content.Load<Texture2D>("esc_btn");

            font = Content.Load<SpriteFont>("Instructions");
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
                                break;
                            case 1:
                                //Load game
                                break;
                            case 2:
                                gameState = State.INSTRUCTIONS;
                                break;
                            default:
                                //Quit
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
            graphics.PreferredBackBufferHeight = 750;
            graphics.PreferredBackBufferWidth = 560;
            graphics.ApplyChanges();

            spriteBatch.Begin();

            switch (gameState)
            {
                case State.MAINMENU:
                    spriteBatch.Draw(menu_img, new Rectangle(0, 0, 560, 750), Color.White);

                    spriteBatch.Draw(newGameBtn, new Rectangle(200, 320, 158, 58), Color.White);
                    spriteBatch.Draw(openGameBtn, new Rectangle(200, 398, 158, 58), Color.White);
                    spriteBatch.Draw(instructionsBtn, new Rectangle(200, 476, 158, 58), Color.White);
                    spriteBatch.Draw(escapeBtn, new Rectangle(200, 554, 158, 58), Color.White);
                    spriteBatch.Draw(arrow, new Rectangle(165, 333 + arrowPosition * 78, 31, 31), Color.White);
                    break;

                case State.INSTRUCTIONS:
                    String instructions = "A jatek celja, hogy bla bla bla\nAki a legtobb penzt osszegyujti, bla bla bla\nJo szorakozast!";
                    spriteBatch.DrawString(font, instructions, new Vector2(20, 20), Color.Red);
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }



        #endregion
    }
}
