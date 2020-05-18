using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace Chattprogram
{
    class RequestWindow
    {
        Texture2D window;
        Texture2D button;
        Rectangle acceptButton;
        Rectangle declineButton;
        string accept;
        string decline;
        

        public RequestWindow()
        {
            window = null;
            button = null;
            acceptButton = new Rectangle(460, 270, 100, 50);
            declineButton = new Rectangle(610, 270, 100, 50);
            accept = "";
            decline = "";
            
        }

        public RequestWindow(Texture2D window, Texture2D button, Rectangle acceptButton, Rectangle declineButton, string accept, string decline)
        {
            this.window = window;
            this.button = button;
            this.acceptButton = acceptButton;
            this.declineButton = declineButton;
            this.accept = accept;
            this.decline = decline;
            
        }

        public void FunctionRequestWindow(MouseState mouseState, Socket clientSocket, ref bool requested, string requester, List<string> messages, List<Vector2> messagePositions)
        {
            if (acceptButton.Contains(mouseState.Position))
            {
                clientSocket.Send(Encoding.UTF8.GetBytes("acc47 " + requester));
                messages.Clear();
                messagePositions.Clear();
                
                requested = false;
            }
            if (declineButton.Contains(mouseState.Position))
            {
                clientSocket.Send(Encoding.UTF8.GetBytes("decc47 "));
                requested = false;
            }
        }

        public void DrawRequestWindow(SpriteBatch sb, SpriteFont title, SpriteFont boxes, string requester)
        {
            sb.Draw(window, new Rectangle(490, 180, 300, 200), Color.White);
            sb.Draw(button, acceptButton, Color.Green);
            sb.Draw(button, declineButton, Color.Red);

            sb.DrawString(title, "Request from: [" + requester + "]", new Vector2(510, 200), Color.Black);
            sb.DrawString(boxes, "Accept", new Vector2(520, 270), Color.Black);
            sb.DrawString(boxes, "Decline", new Vector2(670, 270), Color.Black);
        }

    }
}
