using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using slutlig;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


namespace Chattprogram
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        MouseState mouseState;
        MouseState oldmouseState;

        KeyboardState keyBoardState;
        KeyboardState oldKeyBoardState;

        Texture2D sendButton;

        Color colour;

        Rectangle send;

        List<Användare> chatters = new List<Användare>();

        private static readonly Socket ClientSocket =
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private const int Port = 65002;

        static bool connected = false;

        TextBox textBox;

        Random random = new Random();

        SpriteFont normalText;
        SpriteFont chattwindowText;
        SpriteFont sendText;

        Texture2D chattWindow;

        SpriteFont huge;

        bool taken = true;

        int gameState = 0;

        int time = 0;
        int second = 0;
        int second2 = 0;

        List<string> funfacts;

        List<Vector2> positions;

        List<int> randomized;

        List<int> takenList = new List<int>();

        public void Funfacts(SpriteBatch sb)
        {
            if (second % 60 == 0 && second > 0)
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

                        if (randomized[i] == takenList[i - takenList.Count] || randomized[i] == takenList[i - 1])
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

                taken = true;
            }

            for (int i = 0; i < 4; i++)
            {
                sb.DrawString(normalText, funfacts[randomized[i]], positions[i], Color.Black);
            }
        }

        private static void ConnectToServer()
        {
            
            int attempts = 0;
            while (!ClientSocket.Connected)
            {
                try
                {
                    attempts++;
                    Debug.WriteLine("Connection attempt " + attempts);
                    ClientSocket.Connect(IPAddress.Parse("127.0.0.1"), Port);
                }
                catch (SocketException)
                {
                    
                }
            }
            //Console.Clear();
            connected = true;
        }

        public static void ReadServer()
        {
            ReceiveResponse();
        }

        private static void RequestLoop()
        {
            Thread readServerThread = new Thread(new ThreadStart(ReadServer));

            readServerThread.Start();

            Console.WriteLine("Skriv disconnect för att koppla från servern");
            string requestSent = string.Empty;
            try
            {
                while (requestSent.ToLower() != "disconnect")
                {
                    requestSent = Console.ReadLine();
                    ClientSocket.Send(Encoding.UTF8.GetBytes(requestSent), SocketFlags.None);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error! - Lost server.");
                Console.ReadLine();
            }
        }
        private static void ReceiveResponse()
        {
            var buffer = new byte[2048];
            try
            {
                int received = ClientSocket.Receive(buffer, SocketFlags.None);
                if (received == 0)
                    return;
                Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, received));
                ReceiveResponse();
            }
            catch (System.Net.Sockets.SocketException)
            {
                Console.WriteLine("Frånkopplar från server...");
                return;
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
            Thread connectToServerThread = new Thread(new ThreadStart(ConnectToServer));
            connectToServerThread.Start();

            slutlig.KeyboardInput.Initialize(this, 500f, 20);

            sendButton = Content.Load<Texture2D>("fullsend2");
            send = new Rectangle(395, 420, 405, 30);

            chattwindowText = Content.Load<SpriteFont>("chattwindowText");

          //  chatters.Add(new Användare("Gunther", 2.6f, new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)));
          //  chatters.Add(new Användare("Marvin Gay", 4.3f, new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)));
          //  chatters.Add(new Användare("Tom Hanks", 0f, new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)));

            connectToServerThread = new Thread(ConnectToServer);

            textBox = new TextBox(new Rectangle(400, 450, 385, 50), 70, "", GraphicsDevice, chattwindowText, Color.Black, Color.Black, 70);
            textBox.Active = true;

            spriteBatch = new SpriteBatch(GraphicsDevice);

            huge = Content.Load<SpriteFont>("huge");
            normalText = Content.Load<SpriteFont>("normalText");
            sendText = Content.Load<SpriteFont>("sendText");

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
        }

        protected override void UnloadContent()
        {
            
        }

        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //ConnectToServer();
            //RequestLoop();
            //ClientSocket.Shutdown(SocketShutdown.Both);
            //ClientSocket.Close();

            if (mouseState.LeftButton == ButtonState.Pressed && oldmouseState.LeftButton == ButtonState.Released)
            {
                if (send.Contains(mouseState.Position))
                {
                    ClientSocket.Send(Encoding.UTF8.GetBytes("wea238g " + (textBox.Text.String)), SocketFlags.None);
                    textBox.Clear();
                }
            }

            //string[] username = text.Split(' ');
            //if (username[0] == "wea238g")
            //{
            //    users.name.Add(username[1]);
            //}


            if (send.Contains(mouseState.Position))
            {
                colour = Color.LightGray;
            }
            else
            {
                colour = Color.White;
            }

            oldKeyBoardState = keyBoardState;
            keyBoardState = Keyboard.GetState();

            oldmouseState = mouseState;
            mouseState = Mouse.GetState();

            slutlig.KeyboardInput.Update();
            textBox.Update();



            base.Update(gameTime);
        }

        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.DrawString(huge, "waZup", new Vector2(355, 30), Color.Black);

            spriteBatch.Draw(chattWindow, new Vector2(200, 100), Color.White);

            //spriteBatch.DrawString(chattwindowText, textBox.Text.String, new Vector2(200, 250), Color.Black);
            
            

            textBox.Draw(spriteBatch);

            spriteBatch.Draw(sendButton, send, null, colour);

            spriteBatch.DrawString(sendText, "SEND", new Vector2(485, 427), Color.Black);

            spriteBatch.DrawString(normalText, (mouseState.X + " " + mouseState.Y).ToString(), new Vector2(100, 100), Color.Black);

            if (gameState == 0)
            {
                if (connected)
                {
                    spriteBatch.DrawString(normalText, "Connected to server", new Vector2(430, 300), Color.Green);
                    second2++;
                    gameState = 1;

                    if (second2 > 300)
                    {
                        gameState += 1;
                    }
                }
                else
                {
                    spriteBatch.DrawString(normalText, "Connecting to server", new Vector2(430, 300), Color.Red);
                }
            }

            if (gameState == 1)
            {
                spriteBatch.DrawString(chattwindowText, "V Choose username V", new Vector2(430, 330), Color.Gray);
            }
            

            time += 1;

            spriteBatch.DrawString(normalText, time + " " + second, new Vector2(50, 30), Color.Black);
            if (time % 60 == 0)
            {
                second += 1;
                time = 0;
            }

            Funfacts(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
