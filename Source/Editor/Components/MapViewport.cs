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
        private Level level;
        private Point viewOffset;
        private Point lastMouseLocation;
        public BlockType SelectedBlockType = BlockType.Solid;

        private Dictionary<EnemyType, Image> enemyImages = new Dictionary<EnemyType, Image>();

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

            enemyImages.Add(EnemyType.PistolGrunt, Image.FromFile("Resources/Enemies/PistolGrunt.png"));
            enemyImages.Add(EnemyType.ShotgunGrunt, Image.FromFile("Resources/Enemies/ShotgunGrunt.png"));
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
                var image = enemyImages[enemy.Type];
                graphics.DrawImage(image, enemy.X - viewOffset.X, enemy.Y - viewOffset.Y);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Middle)
                lastMouseLocation = e.Location;
            else if (e.Button == MouseButtons.Left)
                DrawCollisionMap(e.Location.Add(viewOffset), SelectedBlockType);
            else if (e.Button == MouseButtons.Right)
                DrawCollisionMap(e.Location.Add(viewOffset), BlockType.Open);
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
            else if (e.Button == MouseButtons.Left)
                DrawCollisionMap(e.Location.Add(viewOffset), SelectedBlockType);
            else if (e.Button == MouseButtons.Right)
                DrawCollisionMap(e.Location.Add(viewOffset), BlockType.Open);
        }

        private void DrawCollisionMap(Point point, BlockType blockType)
        {
            var clickedCell = point.Divide(Level.BlockSize);
            Level.CollisionMap[clickedCell.X, clickedCell.Y] = blockType;
            Redraw();
        }

        private void Redraw()
        {
            Invalidate();
        }
    }
}
