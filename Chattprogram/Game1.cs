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
using System.IO;


namespace Chattprogram
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        static List<string> messages = new List<string>();
        List<Vector2> positions;
        List<int> randomized = new List<int>();
        static List<Vector2> userListPos = new List<Vector2>();
        static List<Profile> profiles = new List<Profile>();

        static RequestWindow requestWindow;

        MouseState mouseState;
        MouseState oldmouseState;
        KeyboardState keyBoardState;
        KeyboardState oldKeyBoardState;

        Texture2D reqWindow;
        Texture2D line;

        static Color profileColor = Color.Black;
        Color colour;

        int time = 0;
        int second = 0;
        int second2 = 0;
        static int gameState = 0;

        static string chattpartner;
        static string requester;

        string myUsername;

        static Vector2 messagePos;

        static bool requested = false;

        static bool usersAdded = false;

        static List<Vector2> messagePositions = new List<Vector2>();
        static List<string> usernames = new List<string>();
        List<string> funfacts = new List<string>();

        Texture2D chattWindow;

        Texture2D sendButton;

        Rectangle send;
        Rectangle refresh;

        private static readonly Socket ClientSocket =
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private const int Port = 65002;

        TextBox textBox;

        Random random = new Random();

        SpriteFont normalText;
        SpriteFont chattwindowText;
        SpriteFont sendText;
        SpriteFont huge;

        static bool connected = false;
        bool taken = true;


        public void Funfacts(SpriteBatch sb)
        {
            if (second % 10 == 0 && second > 0)
            {
                taken = true;
                second = 0;
                while (taken == true)
                {
                    taken = false;

                    for(int i = 0; i < 4; i++)
                    {
                        randomized[i] = random.Next(0, 18);
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        if (taken)
                        {
                            break;
                        }
                        for (int j = 0; j < 4; j++)
                        {
                            if (randomized[i] == randomized[j] && i != j)
                            {
                                taken = true;
                                break;
                            }
                            else
                            {
                                taken = false;
                            }
                        }
                    }

                }
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
            Thread readServerThread = new Thread(new ThreadStart(ReceiveResponse));

            readServerThread.Start();
            //Console.Clear();
            connected = true;
        }


        private static void ReceiveResponse()
        {
            var buffer = new byte[2048];
            try
            {
                int received = ClientSocket.Receive(buffer, SocketFlags.None);
                if (received == 0)
                    return;
                string message = (Encoding.UTF8.GetString(buffer, 0, received));

                string[] messageSplit = message.Split(' ');

                
                if (messageSplit[0] == "erf77")
                {
                    usersAdded = false;
                    int posY = 170;
                    profiles.Clear();

                    for (int i = 1; i < messageSplit.Length - 1; i += 2)
                    {
                        Vector2 pos = new Vector2(220, posY);
                        Profile profile = new Profile(pos, profiles, new Rectangle(220, posY, 80, 20), messageSplit[i], messageSplit[i + 1], profileColor);
                        
                        profiles.Add(profile);
                        usernames.Add(messageSplit[i]);
                        userListPos.Add(pos);
                        posY += 50;
                    }
                    usersAdded = true;
                }
                else if (messageSplit[0] == "kl90")
                {
                    requested = true;
                    requester = messageSplit[1];
                    
                }
                else if (messageSplit[0] == "yp82")
                {
                    
                    if (messageSplit[1] == "none")
                    {
                        
                        chattpartner = messageSplit[1];
                        gameState = 3;
                    }
                    else
                    {
                        messagePos = new Vector2(390, 130);
                        chattpartner = messageSplit[1];
                        gameState = 3;
                        messages.Clear();
                        messagePositions.Clear();
                    }
                    
                }
                else
                {
                    if (gameState == 3)
                    {
                        messages.Add("(" + chattpartner + "): " + message);
                        messagePos.Y += 30;
                        messagePositions.Add(messagePos);
                    }
                    
                }

                ReceiveResponse();
            }
            catch (System.Net.Sockets.SocketException)
            {
                Debug.WriteLine("Frånkopplar från server...");
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

            reqWindow = Content.Load<Texture2D>("requestWindow");

            line = Content.Load<Texture2D>("lodrättsträck");

            sendButton = Content.Load<Texture2D>("fullsend2");
            send = new Rectangle(395, 420, 405, 30);
            refresh = new Rectangle(205, 105, 25, 25);

            requestWindow = new RequestWindow(reqWindow, sendButton, new Rectangle(510, 260, 100, 50), new Rectangle(660, 260, 100, 50), "Accept", "Decline");

            chattwindowText = Content.Load<SpriteFont>("chattwindowText");

            connectToServerThread = new Thread(ConnectToServer);

            textBox = new TextBox(new Rectangle(400, 450, 385, 50), 70, "", GraphicsDevice, chattwindowText, Color.Black, Color.Black, 70);

            spriteBatch = new SpriteBatch(GraphicsDevice);

            huge = Content.Load<SpriteFont>("huge");
            normalText = Content.Load<SpriteFont>("normalText");
            sendText = Content.Load<SpriteFont>("sendText");

            randomized = new List<int> { random.Next(0, 18), random.Next(0, 18), random.Next(0, 18), random.Next(0, 18) };

            chattWindow = Content.Load<Texture2D>("vitt");

            StreamReader sr = new StreamReader("../../../../Textfile1.txt");
            string funfact = sr.ReadLine();
            funfact = funfact.Replace("\\n", Environment.NewLine);

            while (funfact != null)
            {
                funfacts.Add(funfact);
                funfact = sr.ReadLine();
                if (funfact != null)
                {
                    funfact = funfact.Replace("\\n", Environment.NewLine);
                }
                
            }

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

            if (usernames.Count == 2)
            {
                usersAdded = true;
            }

            if (gameState > 0)
            {
                textBox.Active = true;
            }

            for (int i = 0; i < profiles.Count; i++)
            {
                if (profiles[i].rectangle.Contains(mouseState.Position))
                {
                    profiles[i].color = Color.Gray;
                }
                else
                {
                    profiles[i].color = Color.Black;
                }
            }

            if (mouseState.ScrollWheelValue != oldmouseState.ScrollWheelValue)
            {
                if (oldmouseState.ScrollWheelValue < mouseState.ScrollWheelValue)
                {
                    for (int i = 0; i < messages.Count; i++)
                    {
                        messagePositions[i] = new Vector2(messagePositions[i].X, messagePositions[i].Y + 10);
                    }
                }
                if (oldmouseState.ScrollWheelValue > mouseState.ScrollWheelValue)
                {
                    for (int i = 0; i < messages.Count; i++)
                    {
                        messagePositions[i] = new Vector2(messagePositions[i].X, messagePositions[i].Y - 10);
                    }
                }
            }


            if (mouseState.LeftButton == ButtonState.Pressed && oldmouseState.LeftButton == ButtonState.Released)
            {
                
                if (requested)
                {
                    requestWindow.FunctionRequestWindow(Mouse.GetState(), ClientSocket, ref requested, requester, messages, messagePositions);
                }

                for (int i = 0; i < profiles.Count; i++)
                {
                    if (profiles[i].rectangle.Contains(mouseState.Position))
                    {
                        ClientSocket.Send(Encoding.UTF8.GetBytes("swt5 " + profiles[i].name + " " + myUsername), SocketFlags.None);
                    }
                }

                if (send.Contains(mouseState.Position) && gameState == 1)
                {
                    if (gameState == 1)
                    {
                        myUsername = textBox.Text.String;
                        ClientSocket.Send(Encoding.UTF8.GetBytes("wea238g " + (textBox.Text.String)), SocketFlags.None);
                        gameState = 2;
                        textBox.Clear();
                    }
                }

                if (gameState > 1)
                {
                    if (send.Contains(mouseState.Position))
                    {
                        ClientSocket.Send(Encoding.UTF8.GetBytes(textBox.Text.String), SocketFlags.None);

                        if (gameState == 3)
                        {
                            messages.Add("(Du): " + textBox.Text.String);
                            messagePos.Y += 30;
                            messagePositions.Add(messagePos);
                        }

                        textBox.Clear();
                    }
                    if (refresh.Contains(mouseState.Position))
                    {
                        ClientSocket.Send(Encoding.UTF8.GetBytes("erf77 "), SocketFlags.None);
                    }
                }
                
            }

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

            Funfacts(spriteBatch);

            textBox.Draw(spriteBatch);

            spriteBatch.DrawString(normalText, (mouseState.X + " " + mouseState.Y).ToString(), new Vector2(100, 100), Color.Black);

            if (gameState == 0)
            {
                if (connected)
                {
                    spriteBatch.DrawString(normalText, "Connected to server", new Vector2(430, 300), Color.Green);
                    second2++;
                    

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
                spriteBatch.DrawString(chattwindowText, "V Choose username V", new Vector2(490, 375), Color.Gray);
            }
            
            if (usersAdded)
            {
                profiles.Sort();
                for (int i = 0; i < profiles.Count; i++)
                {
                    profiles[i].DrawProfiles(spriteBatch, sendButton, chattwindowText, i);
                }
            }
            
            if (gameState > 0)
            {
                spriteBatch.Draw(sendButton, send, null, colour);
                spriteBatch.DrawString(sendText, "SEND", new Vector2(490, 427), Color.Black);
            }
            
            if (gameState > 1)
            {
                spriteBatch.Draw(line, new Vector2(10, 100), Color.White);
                spriteBatch.Draw(sendButton, refresh, null, Color.White);
                spriteBatch.DrawString(normalText, "Logged in as: " + myUsername, new Vector2(630, 110), Color.Black);
            }

            for (int i = 0; i < messages.Count; i++)
            {
                if (messagePositions[i].Y >= 130 && messagePositions[i].Y < 410)
                {
                    spriteBatch.DrawString(chattwindowText, messages[i], messagePositions[i], Color.Black);
                }
            }

            time += 1;

            spriteBatch.DrawString(normalText, time + " " + second, new Vector2(50, 30), Color.Black);

            if (time % 60 == 0)
            {
                second += 1;
                time = 0;
            }

            if (gameState == 3)
            {
                spriteBatch.DrawString(normalText, "Chattpartner: " + chattpartner, new Vector2(480, 110), Color.Blue);
            }

            if (requested)
            {
                requestWindow.DrawRequestWindow(spriteBatch, normalText, chattwindowText, requester);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
