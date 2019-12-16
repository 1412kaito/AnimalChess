namespace AnimalChess {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.panelGame = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.labelGiliran = new System.Windows.Forms.ToolStripMenuItem();
            this.gantiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.atasPVAIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.atasAIVPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redrawToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelGame
            // 
            this.panelGame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGame.Location = new System.Drawing.Point(0, 24);
            this.panelGame.Name = "panelGame";
            this.panelGame.Size = new System.Drawing.Size(800, 725);
            this.panelGame.TabIndex = 0;
            this.panelGame.Paint += new System.Windows.Forms.PaintEventHandler(this.panelGame_Paint);
            this.panelGame.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelGame_MouseClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelGiliran,
            this.gantiToolStripMenuItem,
            this.atasPVAIToolStripMenuItem,
            this.atasAIVPToolStripMenuItem,
            this.redrawToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // labelGiliran
            // 
            this.labelGiliran.Name = "labelGiliran";
            this.labelGiliran.Size = new System.Drawing.Size(25, 20);
            this.labelGiliran.Text = "1";
            // 
            // gantiToolStripMenuItem
            // 
            this.gantiToolStripMenuItem.Name = "gantiToolStripMenuItem";
            this.gantiToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.gantiToolStripMenuItem.Text = "ganti";
            this.gantiToolStripMenuItem.Click += new System.EventHandler(this.gantiGiliran);
            // 
            // atasPVAIToolStripMenuItem
            // 
            this.atasPVAIToolStripMenuItem.Name = "atasPVAIToolStripMenuItem";
            this.atasPVAIToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.atasPVAIToolStripMenuItem.Text = "[atas]P v AI";
            this.atasPVAIToolStripMenuItem.Click += new System.EventHandler(this.atasPVAIToolStripMenuItem_Click);
            // 
            // atasAIVPToolStripMenuItem
            // 
            this.atasAIVPToolStripMenuItem.Name = "atasAIVPToolStripMenuItem";
            this.atasAIVPToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.atasAIVPToolStripMenuItem.Text = "[atas]AI v P";
            this.atasAIVPToolStripMenuItem.Click += new System.EventHandler(this.atasAIVPToolStripMenuItem_Click);
            // 
            // redrawToolStripMenuItem
            // 
            this.redrawToolStripMenuItem.Name = "redrawToolStripMenuItem";
            this.redrawToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.redrawToolStripMenuItem.Text = "redraw";
            this.redrawToolStripMenuItem.Click += new System.EventHandler(this.redrawToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 749);
            this.Controls.Add(this.panelGame);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelGame;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem labelGiliran;
        private System.Windows.Forms.ToolStripMenuItem gantiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem atasPVAIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem atasAIVPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redrawToolStripMenuItem;
    }
}

