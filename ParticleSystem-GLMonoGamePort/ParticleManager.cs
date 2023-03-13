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
    public static class ParticleManager
    {
        public static List<Particle> ListOfParticles = new List<Particle>();
        public static Vector2 Camera = Vector2.Zero;
        const float WaterRepelSize = 3f;
        const float WaterRepelStrength = 0.02f;
        public static int Timer = 0;

        public static float ParticleVisibility = 0.75f;

        public static void Create()
        {
            for (int ix = 0; ix < GameValues.ScreenSize.X; ix += 3)
            {
                for (int iy = 0; iy < GameValues.ScreenSize.Y; iy += 4)
                {
                    AddParticle(new Particle(ix, iy, 0, 0));
                }
            }
        }
        public static void AddParticle(Particle P)
        {
            ListOfParticles.Add(P);
        }
        public static void ClearParticles()
        {
            ListOfParticles.Clear();
        }
        public static List<Particle> GetAllParticles()
        {
            return ListOfParticles;
        }

        public static void Update(GraphicsDevice GD)
        {
            Timer++;

            ControlHandler.HandleControls(ListOfParticles, GD);

            UpdateParticles();
        }
        public static void UpdateParticles()
        {
            for (int i = 0; i < ListOfParticles.Count; i++)
                ListOfParticles[i].Update();
        }

        public static void DrawAsPoints(SpriteBatch spriteBatch)
        {
            lock (ListOfParticles)
            {
                for (int i = 0; i < ListOfParticles.Count; i++)
                {
                    ListOfParticles[i].Draw(spriteBatch);
                }
            }
        }
        public static Texture2D DrawAsPointsToTexture(GraphicsDevice GD)
        {
            Color[,] Col2D = new Color[(int)GameValues.ScreenSize.X, (int)GameValues.ScreenSize.Y];

            for (int x = 0; x < Col2D.GetLength(0); x++)
                for (int y = 0; y < Col2D.GetLength(1); y++)
                    Col2D[x, y] = Color.Black;

            List<Particle> P = GetAllParticles();
            for (int i = 0; i < P.Count; i++)
            {
                if (P[i].Pos.X > 0 && P[i].Pos.X < GameValues.ScreenSize.X && P[i].Pos.Y > 0 && P[i].Pos.Y < GameValues.ScreenSize.Y)
                    Col2D[(int)P[i].Pos.X, (int)P[i].Pos.Y] = P[i].C;
            }

            Color[] Col1D = new Color[Col2D.GetLength(0) * Col2D.GetLength(1)];
            for (int x = 0; x < Col2D.GetLength(0); x++)
                for (int y = 0; y < Col2D.GetLength(1); y++)
                    Col1D[x * y] = Col2D[x, y];

            Texture2D Output = new Texture2D(GD, Col2D.GetLength(0), Col2D.GetLength(1));
            Output.SetData(Col1D);

            return Output;
        }
        public static void DrawAsArrows(SpriteBatch spriteBatch)
        {
            lock (ListOfParticles)
            {
                for (int i = 0; i < ListOfParticles.Count; i++)
                {
                    ListOfParticles[i].DrawAsArrow(spriteBatch);
                }
            }
        }
        public static void DrawText(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Assets.Font, ListOfParticles.Count.ToString(), new Vector2(5, 5), Color.LimeGreen);
            EingabenAnzeige.Draw(spriteBatch);
        }
    }
}
