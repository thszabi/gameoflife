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

        BoardGame boardGame;
        MiniGame miniGame;
        private enum GameState { BOARDGAME, MINIGAME};
        private GameState gameState;



        #region Variables for Draw


        public SpriteBatch spriteBatch;
        private GraphicsDeviceManager graphics;



        #endregion
        #region Constructor



        public GameOfLife()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 956;
            graphics.PreferredBackBufferHeight = 835;
            graphics.ApplyChanges();

            gameState = GameState.BOARDGAME;
            boardGame = new BoardGame(this);
            miniGame = new MiniGame(this);
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
            boardGame.Initialize();
            miniGame.Initialize();

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

            boardGame.LoadContent();
            miniGame.LoadContent();
        }



        #endregion
        #region UnloadContent



        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            boardGame.UnloadContent();
            miniGame.UnloadContent();
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
                case GameState.BOARDGAME:
                    boardGame.Update(gameTime);
                break;
                case GameState.MINIGAME:
                    miniGame.Update(gameTime);
                break;
            }

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
            switch (gameState)
            {
                case GameState.BOARDGAME:
                    boardGame.Draw(gameTime);
                    break;
                case GameState.MINIGAME:
                    miniGame.Draw(gameTime);
                    break;
            }
            
            base.Draw(gameTime);
        }



        #endregion
        #region Public functions



        public void StartMiniGame(int player1Num, int player2Num, bool player1AI, bool player2AI)
        {
            miniGame.StartMiniGame(player1Num, player2Num, player1AI, player2AI);
            gameState = GameState.MINIGAME;
        }

        public void MiniGameEnded(int winnerPlayerNum)
        {
            boardGame.MiniGameEnded(winnerPlayerNum);
            gameState = GameState.BOARDGAME;
        }



        #endregion
    }
}