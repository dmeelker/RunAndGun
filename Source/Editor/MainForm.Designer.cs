namespace Editor
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.collisionModeButton = new System.Windows.Forms.ToolStripButton();
            this.enemyModeButton = new System.Windows.Forms.ToolStripButton();
            this.addEnemiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addPistonGruntItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addShotgunGruntItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.addEnemiesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1009, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.Size = new System.Drawing.Size(98, 22);
            this.saveMenuItem.Text = "Save";
            this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.collisionModeButton,
            this.enemyModeButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1009, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip";
            // 
            // collisionModeButton
            // 
            this.collisionModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.collisionModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.collisionModeButton.Name = "collisionModeButton";
            this.collisionModeButton.Size = new System.Drawing.Size(84, 22);
            this.collisionModeButton.Text = "Collision map";
            this.collisionModeButton.Click += new System.EventHandler(this.collisionModeButton_Click);
            // 
            // enemyModeButton
            // 
            this.enemyModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.enemyModeButton.Image = ((System.Drawing.Image)(resources.GetObject("enemyModeButton.Image")));
            this.enemyModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.enemyModeButton.Name = "enemyModeButton";
            this.enemyModeButton.Size = new System.Drawing.Size(55, 22);
            this.enemyModeButton.Text = "Enemies";
            this.enemyModeButton.Click += new System.EventHandler(this.enemyModeButton_Click);
            // 
            // addEnemiesToolStripMenuItem
            // 
            this.addEnemiesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPistonGruntItem,
            this.addShotgunGruntItem});
            this.addEnemiesToolStripMenuItem.Name = "addEnemiesToolStripMenuItem";
            this.addEnemiesToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.addEnemiesToolStripMenuItem.Text = "Add Enemies";
            // 
            // addPistonGruntItem
            // 
            this.addPistonGruntItem.Name = "addPistonGruntItem";
            this.addPistonGruntItem.Size = new System.Drawing.Size(180, 22);
            this.addPistonGruntItem.Text = "Pistol Grunt";
            this.addPistonGruntItem.Click += new System.EventHandler(this.addPistonGruntItem_Click);
            // 
            // addShotgunGruntItem
            // 
            this.addShotgunGruntItem.Name = "addShotgunGruntItem";
            this.addShotgunGruntItem.Size = new System.Drawing.Size(180, 22);
            this.addShotgunGruntItem.Text = "Shotgun Grunt";
            this.addShotgunGruntItem.Click += new System.EventHandler(this.addShotgunGruntItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 630);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton collisionModeButton;
        private System.Windows.Forms.ToolStripButton enemyModeButton;
        private System.Windows.Forms.ToolStripMenuItem addEnemiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addPistonGruntItem;
        private System.Windows.Forms.ToolStripMenuItem addShotgunGruntItem;
    }
}

