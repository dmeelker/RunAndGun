using FileFormats.Levels;
using Game.Entities;
using Game.Entities.Collectables;
using Game.Entities.Enemies;
using Game.Levels;
using Game.Particle;
using Game.Physics;
using Game.Types;
using Game.Weapons;
using SharedTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    public class Game
    {
        public EntityManager Entities { get; private set; } = new EntityManager();
        public PhysicsSystem Physics { get; private set; } = new PhysicsSystem();
        public ParticleSystem Particles { get; private set; } = new ParticleSystem();

        public Level Level { get; private set; }
        public PlayerEntity Player { get; private set; }
        private Renderer renderer;
        private InputHandler inputHandler;

        public Game()
        {
            renderer = new Renderer(this);
            inputHandler = new InputHandler(this, renderer);
        }

        public void LoadLevel(FileFormats.Levels.LevelFile levelData)
        {
            Entities.Clear();

            Entities.Add(new Crate(new Vector(600, 330)));
            Entities.Add(new WeaponCollectable(WeaponType.Shotgun, new Vector(300, 330)));
            Entities.Add(new ArmorCollectable(new Vector(200, 330)));
            Entities.Add(new MedpackCollectable(new Vector(240, 330)));

            //Particles.AddEmitter(new SprayEmitter(Services.Time) { 
            //    Location = new Vector(300, 330),
            //    Vector = new Vector(0, -5),
            //    SpreadInDegrees = 30,
            //    ParticleInterval = 1000,
            //    ParticleFactory = (time) => new Particle.Particle(time)
            //    {
            //        MaxAge = 1000,
            //        Sprite = Services.Sprites["round-particle"],
            //        ScaleFunction = EasingFunctions.AnimateScalar(1, 5, EasingFunctions.EaseInQuad),
            //        VelocityFunction = EasingFunctions.AnimateScalar(10, 3, EasingFunctions.EaseInQuad),
            //        RotationFunction = EasingFunctions.AnimateScalar(0, 360, EasingFunctions.Linear),
            //        ColorFunction = EasingFunctions.AnimateColor(new Color(255, 0, 0, 255), new Color(96, 96, 96, 50), EasingFunctions.Linear)
            //    }
            //});

            Particles.AddEmitter(new CircleBurstEmitter(Services.Time)
            {
                Location = new Vector(200, 330),
                ParticleCount = 100,
                ParticleFactory = (time) => new Particle.Particle(time)
                {
                    MaxAge = 500,
                    Sprite = Services.Sprites["round-particle"],
                    Color = new Color(100, 100, 100, 100),
                    Scale = 2,
                    ScaleFunction = EasingFunctions.AnimateScalar(2, .2, EasingFunctions.EaseInQuad),
                    VelocityFunction = EasingFunctions.AnimateScalar(20, Services.Random.Next(1,5), EasingFunctions.EaseOutQuad),
                    //RotationFunction = EasingFunctions.AnimateScalar(0, 360, EasingFunctions.Linear),
                    ColorFunction = EasingFunctions.AnimateColor(new Color(100, 100, 100, 255), new Color(57, 57, 130, 255), EasingFunctions.EaseInQuad)
                }
            });

            LoadCollisionData(levelData);
            LoadEnemies(levelData.Enemies);
            InitializePlayer();
        }

        public void Update(uint time, int timePassed)
        {
            inputHandler.HandleInput(time);
            Entities.UpdateEntities(time, timePassed);
            Particles.Update(new FrameTime() { Time = time, TicksPassed = timePassed });

            renderer.FollowPlayer(Player, timePassed);
        }

        private void LoadCollisionData(FileFormats.Levels.LevelFile levelData)
        {
            Level = new Level(levelData.Width, levelData.Height);
            for (int y = 0; y < levelData.CollisionMap.Height; y++)
            {
                for (int x = 0; x < levelData.CollisionMap.Width; x++)
                {
                    Level.Cells[x, y] = levelData.CollisionMap.Data[y][x];
                }
            }
        }

        private void LoadEnemies(LevelFile.Enemy[] enemies)
        {
            if (enemies == null)
                return;

            foreach(var enemy in enemies)
            {
                var enemyEntity = EnemyFactory.CreateEnemy(enemy.Type, new Types.Vector(enemy.X, enemy.Y), enemy.Direction);
                Entities.Add(enemyEntity);
            }
        }

        private void InitializePlayer()
        {
            Player = new PlayerEntity(new Vector(30, 30));
            Player.AddWeapon(new Pistol());
            Player.AddWeapon(new SubmachineGun());
            Player.AddWeapon(new SniperRifle());
            Entities.Add(Player);
        }

        public void Render(IntPtr ren, uint time)
        {
            renderer.Render(ren, time);
        }

        public void Close()
        {
            Program.quit = true;
        }
    }
}
