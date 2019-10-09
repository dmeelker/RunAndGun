using Editor.Components;
using Editor.Levels;
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
        }

        private void LoadLevel()
        {
            var fileData = FileFormats.Levels.Loader.Load(@"..\..\..\Game\Resources\Levels\level1.json");

            level = FileFormatConverter.ConvertFromFileFormat(fileData);
            level = new Level(5000, 600);
            level.Image = Image.FromFile(@"..\..\..\Game\Resources\Backdrops\city.png");
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            var fileFormat = FileFormatConverter.ConvertToFileFormat(level);
            FileFormats.Levels.Saver.Save(fileFormat, @"..\..\..\Game\Resources\Levels\level1.json");
        }
    }
}
