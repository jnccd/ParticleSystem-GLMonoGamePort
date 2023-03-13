using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ParticleSystem
{
    public static class EingabenAnzeige
    {
        static string Text = "";
        static int Transparency = 0;

        public static void SetNewText(string NewText)
        {
            Text = NewText;
            Transparency = 255;
        }

        public static void Update()
        {
            Transparency--;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Assets.Font, Text, new Vector2(12, GameValues.ScreenSize.Y - 12 - Assets.Font.MeasureString(Text).Y), Color.FromNonPremultiplied(173, 216, 230, Transparency));
        }
    }
}
