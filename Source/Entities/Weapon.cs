using SDL2;
using SdlTest.Sprites;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Entities
{
    public abstract class Weapon
    {
        public abstract string Name { get; }
        
        public virtual void Update(uint time) { }
        public abstract void Fire(uint time, Entity source, Vector location, Vector vector);
        public abstract void Render(IntPtr rendererId, Vector location, Vector vector);

        public abstract void Reload(uint time);
    }

    public abstract class ReloadableWeapon : Weapon
    {
        public enum State
        {
            ReadyToFire,
            Firing,
            Reloading
        }

        public int ClipSize { get; private set; }
        public int ClipContent { get; private set; }
        public int AmmoReserve { get; private set; }

        private State state = State.ReadyToFire;
        private uint stateEntranceTime = 0;
        private readonly int fireDuration;
        private readonly int reloadDuration;


        public ReloadableWeapon(int clipSize, int fireDuration, int reloadDuration)
        {
            ClipSize = clipSize;
            ClipContent = clipSize;
            AmmoReserve = 0;

            this.fireDuration = fireDuration;
            this.reloadDuration = reloadDuration;
        }

        private void ChangeState(State state, uint time)
        {
            Console.WriteLine($"new state: {state}");
            this.state = state;
            stateEntranceTime = time;
        }

        public override void Reload(uint time)
        {
            if (state != State.ReadyToFire)
                return;

            if (AmmoReserve == 0)
            {
                Console.WriteLine("Out of ammo!");
                return;
            }

            var ammoToLoad = ClipSize - ClipContent;
            ammoToLoad = Math.Min(ammoToLoad, AmmoReserve);

            ClipContent += ammoToLoad;
            AmmoReserve -= ammoToLoad;
            WriteAmmoUpdate();

            ChangeState(State.Reloading, time);
        }

        public override void Update(uint time) 
        {
            switch (state)
            {
                case State.ReadyToFire:
                    break;
                case State.Firing:
                    if (time - stateEntranceTime >= fireDuration)
                        ChangeState(State.ReadyToFire, time);
                    break;
                case State.Reloading:
                    if (time - stateEntranceTime >= reloadDuration)
                        ChangeState(State.ReadyToFire, time);
                    break;
            }
        }

        public override void Fire(uint time, Entity source, Vector location, Vector vector)
        {
            if (state != State.ReadyToFire)
                return;

            if (!CanFire())
            {
                Console.WriteLine("Reload!");
                return; // Reload needed
            }

            FireInternal(time, source, location, vector);
            ReduceAmmo();

            ChangeState(State.Firing, time);
        }

        protected abstract void FireInternal(uint time, Entity source, Vector location, Vector vector);

        protected bool CanFire() => ClipContent > 0;
        protected void ReduceAmmo()
        {
            ClipContent--;
            WriteAmmoUpdate();
        }

        public void AddAmmo(int amount)
        {
            AmmoReserve += amount;
        }

        protected void WriteAmmoUpdate()
        {
            Console.WriteLine($"Loaded ammo: {ClipContent} Total ammo: {AmmoReserve}");
        }
    }

    public class Pistol : ReloadableWeapon
    {
        public override string Name => "Pistol";
        private readonly Sprite sprite;

        public Pistol() : base(12, 100, 1500)
        {
            sprite = Services.SpriteManager["shotgun"];
            AddAmmo(1000);
        }       

        protected override void FireInternal(uint time, Entity source, Vector location, Vector vector)
        {
            var sourceLocation = location + (vector.ToUnit() * sprite.Width);
            var projectile = new Projectile(source, sourceLocation, vector * 40, 2, 800);
            Services.EntityManager.Add(projectile);
        }

        public override void Render(IntPtr rendererId, Vector location, Vector vector)
        {
            var angle = vector.AngleInDegrees;
            var center = new Vector(0, sprite.Height / 2);
            var flip = angle > -90 && angle < 90 ? SDL.SDL_RendererFlip.SDL_FLIP_NONE : SDL.SDL_RendererFlip.SDL_FLIP_VERTICAL;

            sprite.DrawEx(rendererId, (int)location.X, (int)location.Y, vector.AngleInDegrees, center, flip);
        }        
    }

    public class Shotgun : ReloadableWeapon
    {
        public override string Name => "Shotgun";
        private readonly Sprite sprite;

        public Shotgun() : base(8, 400, 3000)
        {
            sprite = Services.SpriteManager["shotgun"];
            AddAmmo(1000);
        }

        protected override void FireInternal(uint time, Entity source, Vector location, Vector vector)
        {
            for (var i = 0; i < 10; i++)
            {
                var angle = vector.AngleInDegrees + Services.Random.Next(-5, 5);
                var projectileVector = Vector.FromAngleInDegrees(angle) * Services.Random.Next(35, 40);
                var sourceLocation = location + (vector.ToUnit() * sprite.Width);

                var projectile = new Projectile(source, sourceLocation, projectileVector, 1, 300);
                Services.EntityManager.Add(projectile);
            }
        }

        public override void Render(IntPtr rendererId, Vector location, Vector vector)
        {
            var angle = vector.AngleInDegrees;
            var center = new Vector(0, sprite.Height / 2);
            var flip = angle > -90 && angle < 90 ? SDL.SDL_RendererFlip.SDL_FLIP_NONE : SDL.SDL_RendererFlip.SDL_FLIP_VERTICAL;

            sprite.DrawEx(rendererId, (int)location.X, (int)location.Y, vector.AngleInDegrees, center, flip);
        }
    }
}
