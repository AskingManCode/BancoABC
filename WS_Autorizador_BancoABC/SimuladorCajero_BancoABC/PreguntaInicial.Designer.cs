namespace SimuladorDeCajeroABC
{
    partial class PreguntaInicial
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
            label1 = new Label();
            btnAceptar = new Button();
            txtBoxN_Cajeros = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ControlLightLight;
            label1.Location = new Point(76, 33);
            label1.Name = "label1";
            label1.Size = new Size(222, 21);
            label1.TabIndex = 0;
            label1.Text = "Cuantos cajeros serán usados?";
            // 
            // btnAceptar
            // 
            btnAceptar.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAceptar.Location = new Point(152, 97);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(75, 33);
            btnAceptar.TabIndex = 2;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            // 
            // txtBoxN_Cajeros
            // 
            txtBoxN_Cajeros.Location = new Point(110, 57);
            txtBoxN_Cajeros.Name = "txtBoxN_Cajeros";
            txtBoxN_Cajeros.Size = new Size(152, 23);
            txtBoxN_Cajeros.TabIndex = 3;
            // 
            // PreguntaInicial
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.MenuHighlight;
            ClientSize = new Size(378, 152);
            Controls.Add(txtBoxN_Cajeros);
            Controls.Add(btnAceptar);
            Controls.Add(label1);
            Name = "PreguntaInicial";
            Text = "Numero de cajeros";
            Load += PreguntaInicial_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btnAceptar;
        private TextBox txtBoxN_Cajeros;
    }
}