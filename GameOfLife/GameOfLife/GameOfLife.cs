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
    /// This is the main type for your game
    /// </summary>
    public class GameOfLife : Microsoft.Xna.Framework.Game
    {
        enum States { WAITINGFORINPUT, MOVING, EFFECTOFFIELD, ATFORK1, ATFORK2, ATFORK3, ATDECISION, QUITTING };

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        DataModel.DataModel model;

        States gameState;
        int spinnedNumber;

        public GameOfLife()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            switch (gameState)
            {
                case States.WAITINGFORINPUT:
                {
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.P))
                    {
                        spinnedNumber = model.Spin(model.ActualPlayer);
                        gameState = States.MOVING;
                    }
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.O) /*&& !model.getHouseInsurance(model.getCurrentPlayer())*/)
                    {
                        /* TODO buyHouseInsurance legyen int
                         * 0 = sikeres
                         * 1 = nincs pénzed
                         * 2 = már van biztosításod
                         */
                        if (model.buyHouseInsurance(model.getCurrentPlayer()))
                        {
                            cout << "biztosítás megvéve" << endl;
                        }
                        else
                        {
                            cout << "már van biztosításod!" << endl;
                        }
                    }
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.J) /*&& !model.getCarInsurance(model.getCurrentPlayer())*/)
                    {
                        if (model.buyCarInsurance(model.getCurrentPlayer()))
                        {
                            cout << "biztosítás megvéve" << endl;
                        }
                        else
                        {
                            cout << "már van biztosításod!" << endl;
                        }
                    }
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.R) /*&& !model.getStock(model.getCurrentPlayer())*/) //TODO ez valójában egy lista
                    {
                        //TODO kirajzolni a részvényeket
                        model.buyCarInsurance(model.getCurrentPlayer());
                    }
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.F))
                    {
                        //TODO a hitel mindig 20.000
                        model.getLoan(model.getCurrentPlayer());
                        cout << "újabb hitel..." << endl;
                    }
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.V))
                    {
                        if (model.payBackLoan(model.getCurrentPlayer()))
                        {
                            cout << "hitel visszafizetve" << endl;
                        }
                        else
                        {
                            cout << "nincs elég pénzed!" << endl;
                        }
                    }
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.M))
                    {
                        if (model.save())
                        {
                            cout << "mentés sikeres" << endl;
                        }
                        else
                        {
                            cout << "mentés sikertelen!" << endl;
                        }
                    }
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.K))
                    {
                        //TODO az UI-t le kell radírozni, helyébe az I/N-t kell tenni
                    }

                    break;
                }

                case States.ATFORK1:
                {
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D1))
                    {
                        model.setPosition(model.getCurrentPlayer(), 1);
                        gameState = States.WAITINGFORINPUT;
                    }
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D2))
                    {
                        model.setPosition(model.getCurrentPlayer(), 2);
                        gameState = States.WAITINGFORINPUT;
                    }

                    break;
                }

                case States.ATFORK2:
                {
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D1))
                    {
                        model.setPosition(model.getCurrentPlayer(), 49);
                        --spinnedNumber;
                        gameState = States.MOVING;
                    }
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D2))
                    {
                        model.setPosition(model.getCurrentPlayer(), 50);
                        --spinnedNumber;
                        gameState = States.MOVING;
                    }

                    break;
                }

                case States.ATFORK3:
                {
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D1))
                    {
                        model.setPosition(model.getCurrentPlayer(), 70);
                        --spinnedNumber;
                        gameState = States.MOVING;
                    }
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D2))
                    {
                        model.setPosition(model.getCurrentPlayer(), 71);
                        --spinnedNumber;
                        gameState = States.MOVING;
                    }
                    break;
                }

                case States.MOVING:
                {
                    if (model.getPosition(model.currentPlayer()) == 0)
                    {
                        //TODO számok kirajzolása
                        gameState = States.ATFORK1;
                    }
                    if (model.getPosition(model.currentPlayer()) == 48)
                    {
                        //TODO számok kirajzolása
                        gameState = States.ATFORK2;
                    }
                    if (model.getPosition(model.currentPlayer()) == 69)
                    {
                        //TODO számok kirajzolása
                        gameState = States.ATFORK3;
                    }
                    model.setPosition(model.getCurrentPlayer(), model.getPosition(model.getCurrentPlayer()) + 1);
                    --spinnedNumber;
                    if (spinnedNumber == 0)
                    {
                        gameState = States.EFFECTOFFIELD;
                    }

                    break;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
