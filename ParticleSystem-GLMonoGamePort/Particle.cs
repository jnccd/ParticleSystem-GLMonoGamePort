using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ParticleSystem
{
    public class Marker : Particle
    {
        public bool Positive;
        int Cooldown;

        public Marker(Vector2 Pos, Vector2 Vel, bool Positive) : base(Pos, Vel)
        {
            this.Pos = Pos;
            this.Vel = Vel;
            this.Positive = Positive;

            this.Cooldown = 0;
        }

        public new void Update()
        {
            if (Pos.X < 0 + 200)
            {
                Vel.X += 1;
            }

            if (Pos.X > GameValues.ScreenSize.X - 200)
            {
                Vel.X -= 1;
            }

            if (Pos.Y < 0 + 200)
            {
                Vel.Y += 1;
            }

            if (Pos.Y > GameValues.ScreenSize.Y - 200)
            {
                Vel.Y -= 1;
            }

            Vel.X /= GameValues.Friction + 0.3f;
            Vel.Y /= GameValues.Friction + 0.3f;

            Pos += Vel;
        }
        public new void Draw(SpriteBatch spriteBatch)
        {
            if (Positive)
            {
                spriteBatch.Draw(Assets.Default, new Rectangle((int)Pos.X + (int)ParticleManager.Camera.X, (int)Pos.Y + (int)ParticleManager.Camera.Y, 3, 3), Color.Red);
            }
            else
            {
                spriteBatch.Draw(Assets.Default, new Rectangle((int)Pos.X + (int)ParticleManager.Camera.X, (int)Pos.Y + (int)ParticleManager.Camera.X, 3, 3), Color.Blue);
            }
        }

        public new void GetPulledBy(Vector2 Puller, bool Direction, float ForceMultiplier)
        {
            PullLength = Vector2.Distance(Puller, Pos);
            Vector2 PullVektor = Puller - Pos;
            ForceMagnitude = GameValues.GravForce / (PullLength * PullLength);
            ForceAngle = (float)Math.Atan2(PullVektor.X, PullVektor.Y);

            if (ForceMagnitude > 0.8f)
            {
                ForceMagnitude = 0.8f;
            }
            ForceMagnitude *= ForceMultiplier;

            if (Direction)
            {
                Vel.X += ForceMagnitude * (float)Math.Sin(ForceAngle) * 0.1f;
                Vel.Y += ForceMagnitude * (float)Math.Cos(ForceAngle) * 0.1f;
            }
            else
            {
                Vel.X -= ForceMagnitude * (float)Math.Sin(ForceAngle) * 0.1f;
                Vel.Y -= ForceMagnitude * (float)Math.Cos(ForceAngle) * 0.1f;
            }
        }
        public void MarkerUpdate(List<Marker> ListOfMarkers, List<Particle> ListOfParticles)
        {
            for (int i1 = 0; i1 < ListOfMarkers.Count; i1++)
            {
                for (int i2 = 0; i2 < ListOfMarkers.Count; i2++)
                {
                    if (ListOfMarkers[i1] != ListOfMarkers[i2])
                    {
                        if (ListOfMarkers[i1].Positive)
                        {
                            // i1 = Proton
                            if (ListOfMarkers[i2].Positive)
                            {
                                //double ZwischenSpeicher1 = Convert.ToDouble(ListOfMarkers[i1].Pos.X - ListOfMarkers[i2].Pos.X);
                                //double ZwischenSpeicher2 = Convert.ToDouble(ListOfMarkers[i1].Pos.Y - ListOfMarkers[i2].Pos.Y);

                                //if (ZwischenSpeicher1 * ZwischenSpeicher1 + ZwischenSpeicher2 * ZwischenSpeicher2 < 32000)
                                //{
                                //    ListOfMarkers[i1].Vel.X += Convert.ToSingle(ZwischenSpeicher1) * 0.001f;
                                //    ListOfMarkers[i1].Vel.Y += Convert.ToSingle(ZwischenSpeicher2) * 0.001f;
                                //}

                                ListOfMarkers[i1].GetPulledBy(ListOfMarkers[i2].Pos, false, 1f);
                            }
                            else
                            {
                                ListOfMarkers[i1].GetPulledBy(ListOfMarkers[i2].Pos, true, 1f);
                            }
                        }
                        else
                        {
                            // i1 = electron
                            if (ListOfMarkers[i2].Positive)
                            {
                                ListOfMarkers[i1].GetPulledBy(ListOfMarkers[i2].Pos, true, 1f);
                            }
                            else
                            {
                                //double ZwischenSpeicher1 = Convert.ToDouble(ListOfMarkers[i1].Pos.X - ListOfMarkers[i2].Pos.X);
                                //double ZwischenSpeicher2 = Convert.ToDouble(ListOfMarkers[i1].Pos.Y - ListOfMarkers[i2].Pos.Y);

                                //if (ZwischenSpeicher1 * ZwischenSpeicher1 + ZwischenSpeicher2 * ZwischenSpeicher2 < 32000)
                                //{
                                //    ListOfMarkers[i1].Vel.X += Convert.ToSingle(ZwischenSpeicher1) * 0.0005f;
                                //    ListOfMarkers[i1].Vel.Y += Convert.ToSingle(ZwischenSpeicher2) * 0.0005f;
                                //}

                                ListOfMarkers[i1].GetPulledBy(ListOfMarkers[i2].Pos, false, 1f);
                            }
                        }

                        // true = anziehen
                        // false = abstoßen
                    }
                }

                if (ListOfMarkers[i1].Cooldown > 0)
                {
                    ListOfMarkers[i1].Cooldown--;
                }
            }
        }
    }

    public class Particle
    {
        public Vector2 Pos;
        public Vector2 Vel;
        public Color C;
        public Vector2 OriginalPos;

        public float PullLength;
        public float ForceMagnitude;
        public float ForceAngle;

        RenderTarget2D RT;

        public Particle(Vector2 Pos, Vector2 Vel)
        {
            this.Pos = Pos;
            this.Vel = Vel;
            C = Color.LimeGreen;
            OriginalPos = Pos;
        }
        public Particle(Vector2 Pos, Vector2 Vel, Color C)
        {
            this.Pos = Pos;
            this.Vel = Vel;
            this.C = C;
            OriginalPos = Pos;
        }
        public Particle(int PosX, int PosY, int VelX, int VelY)
        {
            Pos.X = PosX;
            Pos.Y = PosY;
            Vel.X = VelX;
            Vel.Y = VelY;
            C = Color.LimeGreen;
            OriginalPos = Pos;
        }
        public Particle(int PosX, int PosY, int VelX, int VelY, float Depth, float DepthVel)
        {
            Pos.X = PosX;
            Pos.Y = PosY;
            Vel.X = VelX;
            Vel.Y = VelY;
            C = Color.LimeGreen;
            OriginalPos = Pos;
        }

        public void GetPulledBy(Vector2 Puller, bool Direction)
        {
            PullLength = Vector2.Distance(Puller, Pos);
            Vector2 PullVektor = Puller - Pos;
            ForceMagnitude = GameValues.GravForce / (PullLength * PullLength);
            ForceAngle = (float)Math.Atan2(PullVektor.X, PullVektor.Y);

            if (ForceMagnitude > 0.7f)
            {
                ForceMagnitude = 0.7f;
            }

            if (Direction)
            {
                Vel.X += ForceMagnitude * (float)Math.Sin(ForceAngle);
                Vel.Y += ForceMagnitude * (float)Math.Cos(ForceAngle);
            }
            else
            {
                Vel.X -= ForceMagnitude * (float)Math.Sin(ForceAngle);
                Vel.Y -= ForceMagnitude * (float)Math.Cos(ForceAngle);
            }
        }
        public void GetPulledBy(Vector2 Puller, bool Direction, float Strength)
        {
            PullLength = Vector2.Distance(Puller, Pos);
            Vector2 PullVektor = Puller - Pos;
            ForceMagnitude = GameValues.GravForce / (PullLength * PullLength);
            ForceAngle = (float)Math.Atan2(PullVektor.X, PullVektor.Y);

            if (ForceMagnitude > 0.7f)
            {
                ForceMagnitude = 0.7f;
            }

            ForceMagnitude *= Strength;

            if (Direction)
            {
                Vel.X += ForceMagnitude * (float)Math.Sin(ForceAngle);
                Vel.Y += ForceMagnitude * (float)Math.Cos(ForceAngle);
            }
            else
            {
                Vel.X -= ForceMagnitude * (float)Math.Sin(ForceAngle);
                Vel.Y -= ForceMagnitude * (float)Math.Cos(ForceAngle);
            }
        }
        public void OrbitAround(Vector2 Point, bool Direction)
        {
            PullLength = Vector2.Distance(Point, Pos);
            Vector2 PullVektor = Point - Pos;
            ForceMagnitude = GameValues.GravForce / (PullLength);
            ForceAngle = (float)Math.Atan2(PullVektor.X, PullVektor.Y) + 2.80f;

            if (ForceMagnitude > 0.7f)
            {
                ForceMagnitude = 0.7f;
            }

            //ForceMagnitude *= 2;

            Vel.X -= ForceMagnitude * (float)Math.Sin(ForceAngle);
            Vel.Y -= ForceMagnitude * (float)Math.Cos(ForceAngle);

            Vel.X /= 1.05f;
            Vel.Y /= 1.05f;
        }

        public void Update()
        {
            if (ControlHandler.CurKS.IsKeyDown(Keys.Tab) && ParticleManager.Timer % 4 == 0)
            {
                if (Pos.X > OriginalPos.X - 10 && Pos.X < OriginalPos.X + 10 && Pos.Y > OriginalPos.Y - 10 && Pos.Y < OriginalPos.Y + 10)
                {
                    Pos = OriginalPos;
                    Vel = Vector2.Zero;
                }
                else
                {
                    Vector2 Diff = (OriginalPos - Pos);
                    Diff.Normalize();
                    Vel = Diff * 3f;
                }
            }

            if (GameValues.FrictionEnabled)
            {
                Vel.X /= GameValues.Friction;
                Vel.Y /= GameValues.Friction;
            }

            if (GameValues.GravityMode == 1)
            {
                Vel.Y += 0.05f;
            }

            if (GameValues.GravityMode == 2)
            {
                float Force = 1000 / ((Pos.Y - GameValues.ScreenSize.Y / 2) * (Pos.Y - GameValues.ScreenSize.Y / 2));
                if (Force > 1)
                    Force = 1;

                if (this.Pos.Y > GameValues.ScreenSize.Y / 2)
                {
                    Vel.Y -= Force;
                }

                if (this.Pos.Y < GameValues.ScreenSize.Y / 2)
                {
                    Vel.Y += Force;
                }
            }

            if (Pos.X < 0)
            {
                Vel.X *= -1;
            }

            if (Pos.X > GameValues.ScreenSize.X)
            {
                Vel.X *= -1;
            }

            if (Pos.Y < 0)
            {
                Vel.Y *= -1;
            }

            if (Pos.Y > GameValues.ScreenSize.Y)
            {
                Vel.Y *= -1f;
            }

            Pos += Vel;

            if (Pos.X < 0 - 25 || Pos.X > GameValues.ScreenSize.X + 25 || Pos.Y < 0 - 25 || Pos.Y > GameValues.ScreenSize.Y + 25)
            {
                Vel = Vector2.Zero;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.Default, Pos + ParticleManager.Camera, C * ParticleManager.ParticleVisibility);
        }
        public void DrawAsArrow(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.Arrow, Pos + ParticleManager.Camera, null, C * ParticleManager.ParticleVisibility, (float)Math.Atan2(Vel.Y, Vel.X), 
                new Vector2(Assets.Arrow.Width / 2, Assets.Arrow.Height / 2), new Vector2(Vel.LengthSquared() / 1000, 0.02f), SpriteEffects.None, 0);
        }
        public void DrawAsBigCircle(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.BigCircle, new Rectangle((int)Pos.X - GameValues.WaterSize / 2 + (int)ParticleManager.Camera.X, (int)Pos.Y - GameValues.WaterSize / 2 + (int)ParticleManager.Camera.Y, GameValues.WaterSize, GameValues.WaterSize), Color.White);
        }
        public void DrawAsSmallerCircle(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.SmallerCircle, new Rectangle((int)Pos.X - GameValues.WaterSize / 2 + (int)ParticleManager.Camera.X, (int)Pos.Y - GameValues.WaterSize / 2 + (int)ParticleManager.Camera.Y, GameValues.WaterSize, GameValues.WaterSize), Color.White);
        }
        public Texture2D DrawAsLight(SpriteBatch SB, GraphicsDevice GD)
        {
            if (RT == null)
                RT = new RenderTarget2D(GD, (int)GameValues.ScreenSize.X, (int)GameValues.ScreenSize.Y);
            
            GD.SetRenderTarget(RT);
            GD.Clear(Color.Transparent);
            SB.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            Assets.Light.Parameters["Pos"].SetValue(Pos);
            Assets.Light.CurrentTechnique.Passes[0].Apply();
            SB.Draw(Assets.Default, new Rectangle(0, 0, (int)GameValues.ScreenSize.X, (int)GameValues.ScreenSize.Y), Color.Black);
            SB.End();
            return RT;
        }
    }
}
