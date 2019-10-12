using Editor.Components;
using Editor.Levels;
using SharedTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Editor
{
    public partial class MainForm : Form
    {
        private MapViewport viewport;
        private Level level;

        public MainForm()
        {
            InitializeComponent();


            LoadLevel();

            viewport = new MapViewport() { 
                Dock = DockStyle.Fill,
                Level = level
            };
            Controls.Add(viewport);

            ChangeMode(MapViewport.EditMode.CollisionMap);
        }

        private void LoadLevel()
        {
            var fileData = FileFormats.Levels.Loader.Load(@"..\..\..\Game\Resources\Levels\level1.json");

            level = FileFormatConverter.ConvertFromFileFormat(fileData);
            //level = new Level(5000, 600);
            level.Image = Image.FromFile(@"..\..\..\Game\Resources\Backdrops\city.png");
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            var fileFormat = FileFormatConverter.ConvertToFileFormat(level);
            FileFormats.Levels.Saver.Save(fileFormat, @"..\..\..\Game\Resources\Levels\level1.json");
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D1)
                viewport.SelectedBlockType = SharedTypes.BlockType.Solid;
            else if(e.KeyCode == Keys.D2)
                viewport.SelectedBlockType = SharedTypes.BlockType.ProjectilePassingSolid;
        }

        private void collisionModeButton_Click(object sender, EventArgs e)
        {
            ChangeMode(MapViewport.EditMode.CollisionMap);
        }

        private void enemyModeButton_Click(object sender, EventArgs e)
        {
            ChangeMode(MapViewport.EditMode.Enemies);
        }

        private void ChangeMode(MapViewport.EditMode editMode)
        {
            collisionModeButton.Checked = editMode == MapViewport.EditMode.CollisionMap;
            enemyModeButton.Checked = editMode == MapViewport.EditMode.Enemies;
            viewport.SetEditMode(editMode);
        }

        private void addPistonGruntItem_Click(object sender, EventArgs e)
        {
            AddEnemy(EnemyType.PistolGrunt);
        }

        private void AddEnemy(EnemyType type)
        {
            var location = viewport.CenterPoint;

            var enemy = new Enemy()
            {
                X = location.X,
                Y = location.Y,
                Direction = SharedTypes.Direction.Left,
                Type = type
            };

            level.Enemies.Add(enemy);
            viewport.Redraw();

            ChangeMode(MapViewport.EditMode.Enemies);
        }

        private void addShotgunGruntItem_Click(object sender, EventArgs e)
        {
            AddEnemy(EnemyType.ShotgunGrunt);
        }
    }
}
