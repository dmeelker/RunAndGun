using SDL2;
using SdlTest.Components;
using SdlTest.Entities;
using SdlTest.Levels;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest
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
            var weapon = Services.Game.Player.Character.Weapon;

            font.Render(ren, weapon.Name, 700, 500);
            font.Render(ren, $"{weapon.ClipContent} / {weapon.AmmoReserve}", 700, 520);

            font.Render(ren, $"HP: {Services.Game.Player.Character.Hitpoints}/{CharacterComponent.MaxHitpoints}", 0, 500);
            font.Render(ren, $"Armor: {Services.Game.Player.Character.Armor}/{CharacterComponent.MaxArmor}", 0, 520);
            
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
                    if (blockType == BlockType.Block)
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
