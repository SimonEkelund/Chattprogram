using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chattprogram
{
    class Profile
    {
        Vector2 pos;
        List<Profile> profiles;
        public Rectangle rectangle;
        public string name;
        string text;
        public Color color;

        public Profile(Vector2 pos, List<Profile> profiles, Rectangle rectangle, string name, string text, Color color)
        {
            this.pos = pos;
            this.profiles = profiles;
            this.rectangle = rectangle;
            this.name = name;
            this.text = text;
            this.color = color;
        }
        

        public void FunctionProfiles(MouseState mouseState, Socket ClientSocket)
        {
            for (int i = 0; i < profiles.Count; i++)
            {
                if (profiles[i].rectangle.Contains(mouseState.Position))
                {
                    
                    ClientSocket.Send(Encoding.UTF8.GetBytes("swt5 " + profiles[i].name), SocketFlags.None);

                }
            }
        }

        public void DrawProfiles(SpriteBatch sb, Texture2D texture, SpriteFont spriteFont, int index)
        {
            
                sb.Draw(texture, profiles[index].rectangle, Color.White);
                sb.DrawString(spriteFont, profiles[index].text, pos, profiles[index].color);
            
            
        }

    }
}
