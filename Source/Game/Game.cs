using FileFormats.Levels;
using SdlTest.Entities;
using SdlTest.Entities.Collectables;
using SdlTest.Entities.Enemies;
using SdlTest.Levels;
using SdlTest.Types;
using SdlTest.Weapons;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest
{
    public class Game
    {
        public EntityManager Entities { get; private set; } = new EntityManager();
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

            LoadCollisionData(levelData);
            LoadEnemies(levelData.Enemies);
            InitializePlayer();
        }

        public void Update(uint time, int timePassed)
        {
            inputHandler.HandleInput(time);
            Entities.UpdateEntities(time, timePassed);

            renderer.FollowPlayer(Player, timePassed);
        }

        private void LoadCollisionData(FileFormats.Levels.LevelFile levelData)
        {
            Level = new Level(levelData.Width, levelData.Height);
            for (int y = 0; y < levelData.CollisionMap.Height; y++)
            {
                for (int x = 0; x < levelData.CollisionMap.Width; x++)
                {
                    Level.Cells[x, y] = levelData.CollisionMap.Data[y][x] ? BlockType.Block : BlockType.Open;
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
