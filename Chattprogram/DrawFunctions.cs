using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chattprogram
{
    class DrawFunctions
    {
        public List<SpriteFont> spriteFonts = new List<SpriteFont>();
        

        public DrawFunctions(List<SpriteFont> spriteFonts)
        {
            this.spriteFonts = spriteFonts;
        }

        public SpriteFont GetFontChattwindowText()
        {
            return spriteFonts[0];
        }

        public SpriteFont GetFontNormalText()
        {
            return spriteFonts[1];
        }
    }
}
