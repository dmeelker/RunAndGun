﻿using SdlTest.Entities;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Weapons
{
    public enum WeaponType
    {
        Pistol,
        Shotgun
    }

    public abstract class Weapon
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
        public bool InfiniteAmmo { get; set; } = false;

        private State state = State.ReadyToFire;
        private uint stateEntranceTime = 0;
        private readonly int fireDuration;
        private readonly int reloadDuration;


        public Weapon(int clipSize, int fireDuration, int reloadDuration)
        {
            ClipSize = clipSize;
            ClipContent = clipSize;

            this.fireDuration = fireDuration;
            this.reloadDuration = reloadDuration;
        }

        private void ChangeState(State state, uint time)
        {
            Console.WriteLine($"new state: {state}");
            this.state = state;
            stateEntranceTime = time;
        }

        public void Reload(uint time)
        {
            if (state != State.ReadyToFire)
                return;

            if (!InfiniteAmmo && AmmoReserve == 0)
            {
                Console.WriteLine("Out of ammo!");
                return;
            }

            var ammoToLoad = ClipSize - ClipContent;

            if (!InfiniteAmmo)
            {
                ammoToLoad = Math.Min(ammoToLoad, AmmoReserve);
                AmmoReserve -= ammoToLoad;
            }

            ClipContent += ammoToLoad;
            WriteAmmoUpdate();

            ChangeState(State.Reloading, time);
        }

        public void Update(uint time)
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

        public void Fire(uint time, Entity source, Vector location, Vector vector)
        {
            if (state != State.ReadyToFire)
                return;

            if (ReloadNeeded)
            {
                Console.WriteLine("Reload!");
                return; // Reload needed
            }

            FireInternal(time, source, location, vector);
            ReduceAmmo();

            ChangeState(State.Firing, time);
        }

        public abstract string Name { get; }
        public abstract WeaponType WeaponType { get; }
        protected abstract void FireInternal(uint time, Entity source, Vector location, Vector vector);
        public abstract void Render(IntPtr rendererId, Vector location, Vector vector);

        public bool ReloadNeeded => ClipContent == 0;
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
}
