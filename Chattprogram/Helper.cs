using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace slutlig
{
    public static class Helper
    {
        private static Texture2D pixel;

        public static Texture2D GetWhitePixel(this SpriteBatch spriteBatch)
        {
            if (pixel == null)
            {
                pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false,
                    SurfaceFormat.Color);
                pixel.SetData(new[] { Color.White });
            }
            return pixel;
        }
    }
}
