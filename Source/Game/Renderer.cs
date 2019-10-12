using SDL2;
using Game.Components;
using Game.Entities;
using Game.Levels;
using Game.Types;
using System;
using System.Collections.Generic;
using System.Text;
using SharedTypes;
using Game.Sprites;

namespace Game
{
    public class Renderer
    {
        private readonly Game game;
        public Point ViewOffset;
        private Point viewSize = new Point(Program.WindowWidth, Program.WindowHeight);
        private Point viewSizeHalf = new Point(Program.WindowWidth / 2, Program.WindowHeight / 2);
        private double horizontalOffset = 0;

        private int fps;
        private int fpsCounter;
        private uint lastFpsUpdateTime = SDL.SDL_GetTicks();

        public Renderer(Game game)
        {
            this.game = game;
        }

        public void Render(IntPtr ren, uint time)
        {
            SDL.SDL_SetRenderDrawColor(ren, 0, 0, 0, 0);
            SDL.SDL_RenderClear(ren);

            Services.Sprites["backdrop"].Draw(ren, ViewOffset * -1);

            //RenderLevel(ren, Services.Session.Level);
            Services.Game.Entities.RenderEntities(ren, ViewOffset);

            var font = Services.Fonts["big"];
            var character = Services.Game.Player.Character;
            var weapon = character.Weapon;

            font.Render(ren, weapon.Name, 700, 500);
            font.Render(ren, $"{weapon.ClipContent} / {weapon.AmmoReserve}", 700, 520);

            font.Render(ren, $"HP: {character.Hitpoints}/{character.MaxHitpoints}", 0, 500);
            font.Render(ren, $"Armor: {character.Armor}/{character.MaxArmor}", 0, 520);

            RenderBars(ren);

            RenderReloadAndAmmoIndicator(ren, time, font, weapon);

            SDL.SDL_RenderPresent(ren);
            fpsCounter++;

            if (time - lastFpsUpdateTime >= 1000)
            {
                fps = fpsCounter;
                fpsCounter = 0;
                lastFpsUpdateTime = time;

                Program.SetWindowTitle("FPS: " + fps);
            }
        }

        private void RenderBars(IntPtr ren)
        {
            var frameSprite = Services.Sprites["healthAndArmorFrame"];
            var healtSprite = Services.Sprites["healthBar"];
            var armorSprite = Services.Sprites["armorBar"];

            var location = new Point(400 - (frameSprite.Width / 2), 540);
            var character = Services.Game.Player.Character;

            frameSprite.Draw(ren, location);

            var armorPercentage = character.Armor / (double)character.MaxArmor;
            var hpPercentage = character.Hitpoints / (double)character.MaxHitpoints;

            armorSprite.Draw(ren, location.Add(3, 3), new Rect(0, 0, armorSprite.Width * armorPercentage, armorSprite.Height));
            healtSprite.Draw(ren, location.Add(3, 18), new Rect(0, 0, healtSprite.Width * hpPercentage, healtSprite.Height));
        }

        private void RenderReloadAndAmmoIndicator(IntPtr ren, uint time, Text.Font font, Weapons.Weapon weapon)
        {
            if (weapon.ReloadNeeded && time % 1000 > 500)
            {
                var reloadMessage = weapon.ReloadPossible ? "RELOAD!" : "OUT OF AMMO!";
                var stringWidth = font.GetStringWidth(reloadMessage);
                font.Render(ren, reloadMessage, viewSizeHalf.X - (stringWidth / 2), 100);
            }
        }

        private void RenderLevel(IntPtr ren, Level level)
        {
            var blockSprite = Services.Sprites["block"];

            var tileCount = new Point((Program.WindowWidth / Level.BlockSize) + 2, (Program.WindowHeight / Level.BlockSize) + 2);
            var tileStart = new Point(ViewOffset.X / Level.BlockSize, ViewOffset.Y / Level.BlockSize);
            var tileEnd = tileStart + tileCount;
            var drawStart = new Point(-(ViewOffset.X % Level.BlockSize), -(ViewOffset.Y % Level.BlockSize));
            var drawLocation = drawStart;

            for (int x = tileStart.X; x < tileEnd.X; x++)
            {
                drawLocation.Y = drawStart.Y;
                for (int y = tileStart.Y; y < tileEnd.Y; y++)
                {
                    var blockType = level.GetBlock(x, y);
                    if (blockType == BlockType.Solid)
                        blockSprite.Draw(ren, drawLocation);

                    drawLocation.Y += Level.BlockSize;
                }
                drawLocation.X += Level.BlockSize;
            }
        }

        public void FollowPlayer(PlayerEntity player, int timePassed)
        {
            var rate = player.Moving ? 4.0 : 10.0;
            var change = timePassed / rate;

            if (player.Character.Direction == SharedTypes.Direction.Left)
                horizontalOffset = Math.Max(horizontalOffset - change, -200);
            else
                horizontalOffset = Math.Min(horizontalOffset + change, 200);

            ViewOffset = player.Location.ToPoint() - viewSizeHalf;
            ViewOffset.X += (int) horizontalOffset;
            ViewOffset.Y = Math.Min(ViewOffset.Y, Services.Game.Level.HeightInPixels - Program.WindowHeight);
        }
    }
}
