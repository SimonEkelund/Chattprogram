using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chattprogram
{
    class UserSection
    {
        List<string> usernames = new List<string>();

        List<Vector2> userListPos = new List<Vector2>();

        bool flag = false;

        public UserSection(List<string> usernames, List<Vector2> userListPos, bool flag)
        {
            this.usernames = usernames;
            this.userListPos = userListPos;
            this.flag = flag;
        }


        public void GenerateUserSection(string[] split)
        {

            flag = false;
            userListPos.Clear();
            usernames.Clear();
            int posY = 170;

            for (int i = 1; i < split.Length; i++)
            {
                Vector2 pos = new Vector2(220, posY);
                usernames.Add(split[i]);
                userListPos.Add(pos);
                posY += 50;
            }
            flag = true;

        }

        public void DrawUserSection(SpriteBatch sb, SpriteFont font)
        {
            for (int i = 0; i < usernames.Count; i++)
            {
                sb.DrawString(font, usernames[i], userListPos[i], Color.Black);
            }
        }
    }
}
