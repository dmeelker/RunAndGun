using SDL2;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest
{
    public class InputHandler
    {
        private readonly Game game;
        private readonly Renderer renderer;

        public InputHandler(Game game, Renderer renderer)
        {
            this.game = game;
            this.renderer = renderer;
        }

        public void HandleInput(uint time)
        {
            while (SDL.SDL_PollEvent(out var e) > 0)
            {
                if (e.type == SDL.SDL_EventType.SDL_QUIT)
                {
                    game.Close();
                }
                else if (e.type == SDL.SDL_EventType.SDL_KEYDOWN)
                {
                    if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_a)
                        game.Player.MoveLeft();
                    else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_d)
                        game.Player.MoveRight();
                    else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_w)
                        game.Player.Jump();

                }
                else if (e.type == SDL.SDL_EventType.SDL_KEYUP)
                {
                    if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_a || e.key.keysym.sym == SDL.SDL_Keycode.SDLK_d)
                        game.Player.StopMoving();
                    else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_r)
                        game.Player.Character.Reload(time);

                    else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_1)
                        game.Player.ChangeWeapon(game.Player.WeaponOrder[0]);
                    else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_2)
                        game.Player.ChangeWeapon(game.Player.WeaponOrder[1]);
                    else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_3)
                        game.Player.ChangeWeapon(game.Player.WeaponOrder[2]);
                    else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_4)
                        game.Player.ChangeWeapon(game.Player.WeaponOrder[3]);
                }
                else if (e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
                {
                    game.Player.BeginFiring(time);
                }
                else if (e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONUP)
                {
                    game.Player.StopFiring();
                }
            }

            SDL.SDL_GetMouseState(out var x, out var y);
            game.Player.AimAt(new Point(x, y) + renderer.ViewOffset);
        }
    }
}
