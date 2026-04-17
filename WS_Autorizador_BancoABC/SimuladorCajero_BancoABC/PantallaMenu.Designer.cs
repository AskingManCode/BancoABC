namespace SimuladorDeCajeroABC
{
    partial class PantallaMenu
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            retirosToolStripMenuItem = new ToolStripMenuItem();
            cambioDePINToolStripMenuItem = new ToolStripMenuItem();
            consultaToolStripMenuItem = new ToolStripMenuItem();
            salirToolStripMenuItem = new ToolStripMenuItem();
            label1 = new Label();
            PanelContenido = new Panel();
            lblIdentificadorDeCajero = new Label();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            menuStrip1.SuspendLayout();
            PanelContenido.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = SystemColors.Highlight;
            menuStrip1.Items.AddRange(new ToolStripItem[] { retirosToolStripMenuItem, cambioDePINToolStripMenuItem, consultaToolStripMenuItem, salirToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1243, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // retirosToolStripMenuItem
            // 
            retirosToolStripMenuItem.ForeColor = SystemColors.ButtonHighlight;
            retirosToolStripMenuItem.Name = "retirosToolStripMenuItem";
            retirosToolStripMenuItem.Size = new Size(55, 20);
            retirosToolStripMenuItem.Text = "Retiros";
            retirosToolStripMenuItem.Click += retirosToolStripMenuItem_Click;
            // 
            // cambioDePINToolStripMenuItem
            // 
            cambioDePINToolStripMenuItem.ForeColor = SystemColors.ButtonHighlight;
            cambioDePINToolStripMenuItem.Name = "cambioDePINToolStripMenuItem";
            cambioDePINToolStripMenuItem.Size = new Size(99, 20);
            cambioDePINToolStripMenuItem.Text = "Cambio de PIN";
            cambioDePINToolStripMenuItem.Click += cambioDePINToolStripMenuItem_Click;
            // 
            // consultaToolStripMenuItem
            // 
            consultaToolStripMenuItem.ForeColor = SystemColors.ButtonHighlight;
            consultaToolStripMenuItem.Name = "consultaToolStripMenuItem";
            consultaToolStripMenuItem.Size = new Size(66, 20);
            consultaToolStripMenuItem.Text = "Consulta";
            consultaToolStripMenuItem.Click += consultaToolStripMenuItem_Click;
            // 
            // salirToolStripMenuItem
            // 
            salirToolStripMenuItem.ForeColor = SystemColors.ButtonHighlight;
            salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            salirToolStripMenuItem.Size = new Size(41, 20);
            salirToolStripMenuItem.Text = "Salir";
            salirToolStripMenuItem.Click += salirToolStripMenuItem_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(485, 623);
            label1.Name = "label1";
            label1.Size = new Size(278, 21);
            label1.TabIndex = 2;
            label1.Text = "Servicio de cajeros automaticos - 2026";
            // 
            // PanelContenido
            // 
            PanelContenido.BackColor = SystemColors.ControlLightLight;
            PanelContenido.Controls.Add(lblIdentificadorDeCajero);
            PanelContenido.Controls.Add(label2);
            PanelContenido.Controls.Add(pictureBox1);
            PanelContenido.Dock = DockStyle.Fill;
            PanelContenido.Location = new Point(0, 24);
            PanelContenido.Name = "PanelContenido";
            PanelContenido.Size = new Size(1243, 643);
            PanelContenido.TabIndex = 3;
            // 
            // lblIdentificadorDeCajero
            // 
            lblIdentificadorDeCajero.AutoSize = true;
            lblIdentificadorDeCajero.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblIdentificadorDeCajero.Location = new Point(902, 9);
            lblIdentificadorDeCajero.Name = "lblIdentificadorDeCajero";
            lblIdentificadorDeCajero.Size = new Size(211, 25);
            lblIdentificadorDeCajero.TabIndex = 2;
            lblIdentificadorDeCajero.Text = "Identificador de cajero :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(513, 586);
            label2.Name = "label2";
            label2.Size = new Size(227, 25);
            label2.TabIndex = 1;
            label2.Text = "Servicio de cajeros - 2026";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.WhatsApp_Image_2026_02_01_at_1_31_59_AM;
            pictureBox1.Location = new Point(337, 37);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(568, 522);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // PantallaMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.HotTrack;
            ClientSize = new Size(1243, 667);
            Controls.Add(PanelContenido);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "PantallaMenu";
            Text = "Bienvenido";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            PanelContenido.ResumeLayout(false);
            PanelContenido.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem retirosToolStripMenuItem;
        private ToolStripMenuItem cambioDePINToolStripMenuItem;
        private ToolStripMenuItem consultaToolStripMenuItem;
        private ToolStripMenuItem salirToolStripMenuItem;
        private Label label1;
        private Panel PanelContenido;
        private Label label2;
        private PictureBox pictureBox1;
        private Label lblIdentificadorDeCajero;
    }
}
