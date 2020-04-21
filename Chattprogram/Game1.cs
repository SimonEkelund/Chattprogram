using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Chattprogram
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Random random = new Random();

        SpriteFont normalText;

        Texture2D chattWindow;

        bool taken = true;

        int time = 0;
        int second = 0;
        int minute = 0;

        List<string> funfacts;

        List<Vector2> positions;

        List<int> randomized;

        List<int> takenList = new List<int>();

        public void Funfacts(SpriteBatch sb)
        {
            if (second % 10 == 0 && second > 0)
            {
                second = 0;
                while (taken == true)
                {
                    takenList.Clear();

                    taken = false;

                    randomized[0] = random.Next(0, 18);
                    takenList.Add(randomized[0]);

                    for (int i = 1; i < 4; i++)
                    {
                        randomized[i] = random.Next(0, 18);

                        if (randomized[i] == takenList[i - 1])
                        {
                            taken = true;
                            break;
                        }
                        else
                        {
                            takenList.Add(randomized[i]);
                        }
                        
                    }

                }

                
            }



            for (int i = 0; i < 4; i++)
            {
                sb.DrawString(normalText, funfacts[randomized[i]], positions[i], Color.Black);
            }

            

        }



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            IsMouseVisible = true;

            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            normalText = Content.Load<SpriteFont>("normalText");

            randomized = new List<int> { random.Next(0, 18), random.Next(0, 18), random.Next(0, 18),
            random.Next(0, 18)};

            chattWindow = Content.Load<Texture2D>("vitt");

            funfacts = new List<string> { "Peppa pig is 7 feet tall", "George W Bush invaded my \nkitchen for oil",
            "The Corona virus caused a whole \nnew meaning of stranger danger", "Cooking a bat lead to the \nshortage of toilet paper in \nthe US",
            "Handing out your Bank Account \ndetails may lead to devastating \nconsequences", "If your body could digest \nuranium consuming one gram of \nit would make you 2267 kg \nheavier",
            "People are catfished around the \nworld more than 1800 times per \nmonth, stay safe", "Kik, Omegle, Whatsapp, \nMessenger and Snapchat \nhas herpes",
            "Once a kid got a triple kill in cod \nand asked his mom to get the \ncamera", "Be nice chatting in this forum, \notherwise you might be sent to \nthe gulag",
            "Successful men like Bezos and \nBill Gates have killed on \nnumerous occasions", "THERZZ A BOMB IN ZERE \nsaid Arnold Schwarznegger",
            "There are no trustworthy \npersons in the White House, \nthey are all snakes", "The Ranch has recovered more \npersons than all hospitals put \ntogether",
            "Never give out personal \ninformation to anyone, \neven if it is Indian \ntech support", "Mike Tyson was once offered to \nbox Putin for 80 000 000 dollars, \nhe never accepted",
            "Milk is cereal sauce, \nchange my mind", "Poop are dense farts", "WAZUP"};

            positions = new List<Vector2> { new Vector2(5, 180), new Vector2(5, 380), new Vector2(805, 180), 
            new Vector2(805, 380)};

            

            // TODO: use this.Content to load your game content here
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }


        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(chattWindow, new Vector2(200, 100), Color.White);

            spriteBatch.DrawString(normalText, time + " " + second, new Vector2(50, 30), Color.Black);

            time += 1;

            if (time % 60 == 0)
            {
                second += 1;
                time = 0;
            }

            Funfacts(spriteBatch);



            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
