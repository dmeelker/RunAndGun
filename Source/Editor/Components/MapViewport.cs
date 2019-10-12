using Editor.Levels;
using Editor.Types;
using SharedTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Editor.Components
{
    public class MapViewport : Control
    {
        public enum EditMode
        {
            CollisionMap,
            Enemies
        }

        public Point ViewOffset => viewOffset;
        public Point CenterPoint => viewOffset.Add(new Point(Width / 2, Height / 2));

        private Level level;
        private Point viewOffset;
        private Point lastMouseLocation;
        public BlockType SelectedBlockType = BlockType.Solid;

        private EditMode editMode = EditMode.CollisionMap;

        private Dictionary<EnemyType, DirectionalImages> enemyImages = new Dictionary<EnemyType, DirectionalImages>();

        private Enemy selectedEnemy;
        private Point enemyClickOffset;

        private class DirectionalImages
        {
            public Image Left;
            public Image Right;
        }

        public Level Level {
            get => level;
            set 
            {
                level = value;
                Redraw();
            }
        }

        public MapViewport()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);

            enemyImages.Add(EnemyType.PistolGrunt, LoadDirectionalImages("Resources/Enemies/PistolGrunt.png"));
            enemyImages.Add(EnemyType.ShotgunGrunt, LoadDirectionalImages("Resources/Enemies/ShotgunGrunt.png"));
        }


        private DirectionalImages LoadDirectionalImages(string file)
        {
            var image = new DirectionalImages
            {
                Left = Image.FromFile(file),
                Right = Image.FromFile(file),
            };

            image.Left.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return image;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            e.Graphics.Clear(Color.Black);

            if(level != null)
            {
                e.Graphics.DrawImage(level.Image, -viewOffset.X, -viewOffset.Y);

                DrawCollisionMap(e.Graphics);
                DrawEnemies(e.Graphics);
            }
        }

        private void DrawCollisionMap(Graphics g)
        {
            var tileCount = new Point((Width / Level.BlockSize) + 2, (Height / Level.BlockSize) + 2);
            var tileStart = new Point(viewOffset.X / Level.BlockSize, viewOffset.Y / Level.BlockSize);
            var tileEnd = tileStart.Add(tileCount);
            var drawStart = new Point(-(viewOffset.X % Level.BlockSize), -(viewOffset.Y % Level.BlockSize));
            var drawLocation = drawStart;

            for (int x = tileStart.X; x < tileEnd.X; x++)
            {
                drawLocation.Y = drawStart.Y;
                for (int y = tileStart.Y; y < tileEnd.Y; y++)
                {
                    var blockType = level.CollisionMap[x, y];
                    if (blockType == SharedTypes.BlockType.Solid)
                        g.FillRectangle(Brushes.LightGray, drawLocation.X, drawLocation.Y, Level.BlockSize, Level.BlockSize);
                    if (blockType == SharedTypes.BlockType.ProjectilePassingSolid)
                        g.FillRectangle(Brushes.LightBlue, drawLocation.X, drawLocation.Y, Level.BlockSize, Level.BlockSize);

                    drawLocation.Y += Level.BlockSize;
                }
                drawLocation.X += Level.BlockSize;
            }
        }

        private void DrawEnemies(Graphics graphics)
        {
            foreach(var enemy in Level.Enemies)
            {
                var image = enemy.Direction == Direction.Left ? enemyImages[enemy.Type].Left : enemyImages[enemy.Type].Right;
                graphics.DrawImage(image, enemy.X - viewOffset.X, enemy.Y - viewOffset.Y);

                if(enemy == selectedEnemy)
                {
                    graphics.DrawRectangle(Pens.Blue, enemy.X - viewOffset.X, enemy.Y - viewOffset.Y, image.Width + 2, image.Height + 2);
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Middle)
                lastMouseLocation = e.Location;
            else
            {
                if (editMode == EditMode.CollisionMap)
                {
                    if (e.Button == MouseButtons.Left)
                        DrawCollisionMap(e.Location.Add(viewOffset), SelectedBlockType);
                    else if (e.Button == MouseButtons.Right)
                        DrawCollisionMap(e.Location.Add(viewOffset), BlockType.Open);
                }
                else if(editMode == EditMode.Enemies)
                {
                    selectedEnemy = GetEnemyFromPosition(viewOffset.X + e.X, viewOffset.Y + e.Y);
                    enemyClickOffset = new Point(viewOffset.X + e.X - selectedEnemy.X, viewOffset.Y + e.Y - selectedEnemy.Y);
                    Redraw();
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button == MouseButtons.Middle)
            {
                viewOffset = new Point(
                    viewOffset.X - (e.X - lastMouseLocation.X),
                    viewOffset.Y - (e.Y - lastMouseLocation.Y));

                lastMouseLocation = e.Location;

                Redraw();
            }
            else
            {
                if (editMode == EditMode.CollisionMap)
                {
                    if (e.Button == MouseButtons.Left)
                        DrawCollisionMap(e.Location.Add(viewOffset), SelectedBlockType);
                    else if (e.Button == MouseButtons.Right)
                        DrawCollisionMap(e.Location.Add(viewOffset), BlockType.Open);
                }
                else if (editMode == EditMode.Enemies)
                {
                    if(e.Button == MouseButtons.Left && selectedEnemy != null)
                    {
                        selectedEnemy.X = viewOffset.X + e.X - enemyClickOffset.X;
                        selectedEnemy.Y = viewOffset.Y + e.Y - enemyClickOffset.Y;
                        Redraw();
                    }
                }
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            switch(editMode)
            {
                case EditMode.Enemies:
                    if (selectedEnemy == null)
                        return;

                    if(e.KeyCode == Keys.Delete)
                    {
                        DeleteSelectedEnemy();
                    }
                    else if(e.KeyCode == Keys.Space)
                    {
                        RotateSelectedEnemy();
                    }
                    break;
            }
        }

        private void RotateSelectedEnemy()
        {
            selectedEnemy.Direction = selectedEnemy.Direction == Direction.Left ? Direction.Right : Direction.Left;
            Redraw();
        }

        private void DeleteSelectedEnemy()
        {
            level.Enemies.Remove(selectedEnemy);
            selectedEnemy = null;
            Redraw();
        }

        private Enemy GetEnemyFromPosition(int x, int y)
        {
            foreach(var enemy in level.Enemies)
            {
                if (x >= enemy.X && x < enemy.X + 30 && y >= enemy.Y && y < enemy.Y + Height)
                    return enemy;
            }

            return null;
        }

        private void DrawCollisionMap(Point point, BlockType blockType)
        {
            var clickedCell = point.Divide(Level.BlockSize);
            Level.CollisionMap[clickedCell.X, clickedCell.Y] = blockType;
            Redraw();
        }

        public void Redraw()
        {
            Invalidate();
        }

        public void SetEditMode(EditMode mode)
        {
            editMode = mode;
        }
    }
}
