namespace SimuladorCajero_BancoABC
{
    partial class PantallaCambioDePIN
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
            dtpVencimiento = new DateTimePicker();
            txtPIN_nuevo = new TextBox();
            label1 = new Label();
            txtCodigoDeVerificacion = new TextBox();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            txtPIN_actual = new TextBox();
            txtNumeroDeTarjeta = new TextBox();
            btnConfirmarCambioDePIN = new Button();
            button12 = new Button();
            button11 = new Button();
            button10 = new Button();
            button9 = new Button();
            button3 = new Button();
            button8 = new Button();
            button4 = new Button();
            button7 = new Button();
            button5 = new Button();
            button6 = new Button();
            panel1 = new Panel();
            button13 = new Button();
            button1 = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dtpVencimiento
            // 
            dtpVencimiento.CustomFormat = "MM/yyyy";
            dtpVencimiento.Font = new Font("Segoe UI", 24F);
            dtpVencimiento.Format = DateTimePickerFormat.Custom;
            dtpVencimiento.Location = new Point(603, 178);
            dtpVencimiento.Name = "dtpVencimiento";
            dtpVencimiento.ShowUpDown = true;
            dtpVencimiento.Size = new Size(322, 50);
            dtpVencimiento.TabIndex = 16;
            // 
            // txtPIN_nuevo
            // 
            txtPIN_nuevo.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPIN_nuevo.Location = new Point(603, 122);
            txtPIN_nuevo.Name = "txtPIN_nuevo";
            txtPIN_nuevo.Size = new Size(322, 50);
            txtPIN_nuevo.TabIndex = 11;
            txtPIN_nuevo.UseSystemPasswordChar = true;
            txtPIN_nuevo.Enter += txtPIN_nuevo_Enter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 24F);
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(251, 127);
            label1.Name = "label1";
            label1.Size = new Size(167, 45);
            label1.TabIndex = 10;
            label1.Text = "PIN nuevo";
            // 
            // txtCodigoDeVerificacion
            // 
            txtCodigoDeVerificacion.Font = new Font("Segoe UI", 24F);
            txtCodigoDeVerificacion.Location = new Point(603, 234);
            txtCodigoDeVerificacion.Name = "txtCodigoDeVerificacion";
            txtCodigoDeVerificacion.Size = new Size(322, 50);
            txtCodigoDeVerificacion.TabIndex = 9;
            txtCodigoDeVerificacion.UseSystemPasswordChar = true;
            txtCodigoDeVerificacion.Enter += txtCodigoDeVerificacion_Enter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 24F);
            label5.ForeColor = SystemColors.ActiveCaptionText;
            label5.Location = new Point(254, 237);
            label5.Name = "label5";
            label5.Size = new Size(338, 45);
            label5.TabIndex = 8;
            label5.Text = "Codigo de verificación";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 24F);
            label6.ForeColor = SystemColors.ActiveCaptionText;
            label6.Location = new Point(254, 71);
            label6.Name = "label6";
            label6.Size = new Size(164, 45);
            label6.TabIndex = 7;
            label6.Text = "PIN actual";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 24F);
            label7.ForeColor = SystemColors.ActiveCaptionText;
            label7.Location = new Point(251, 183);
            label7.Name = "label7";
            label7.Size = new Size(330, 45);
            label7.TabIndex = 6;
            label7.Text = "Fecha de vencimiento";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.ForeColor = SystemColors.ActiveCaptionText;
            label8.Location = new Point(251, 14);
            label8.Name = "label8";
            label8.Size = new Size(283, 45);
            label8.TabIndex = 5;
            label8.Text = "Número de Tarjeta";
            // 
            // txtPIN_actual
            // 
            txtPIN_actual.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPIN_actual.Location = new Point(603, 66);
            txtPIN_actual.Name = "txtPIN_actual";
            txtPIN_actual.Size = new Size(322, 50);
            txtPIN_actual.TabIndex = 4;
            txtPIN_actual.UseSystemPasswordChar = true;
            txtPIN_actual.Enter += txtPIN_actual_Enter;
            // 
            // txtNumeroDeTarjeta
            // 
            txtNumeroDeTarjeta.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNumeroDeTarjeta.Location = new Point(603, 9);
            txtNumeroDeTarjeta.Name = "txtNumeroDeTarjeta";
            txtNumeroDeTarjeta.Size = new Size(322, 50);
            txtNumeroDeTarjeta.TabIndex = 1;
            txtNumeroDeTarjeta.Enter += txtNumeroDeTarjeta_Enter;
            // 
            // btnConfirmarCambioDePIN
            // 
            btnConfirmarCambioDePIN.BackColor = Color.FromArgb(0, 192, 0);
            btnConfirmarCambioDePIN.FlatStyle = FlatStyle.Popup;
            btnConfirmarCambioDePIN.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnConfirmarCambioDePIN.ForeColor = SystemColors.ControlLightLight;
            btnConfirmarCambioDePIN.Location = new Point(382, 21);
            btnConfirmarCambioDePIN.Name = "btnConfirmarCambioDePIN";
            btnConfirmarCambioDePIN.Size = new Size(196, 64);
            btnConfirmarCambioDePIN.TabIndex = 0;
            btnConfirmarCambioDePIN.Text = "Confirmar ";
            btnConfirmarCambioDePIN.UseVisualStyleBackColor = false;
            btnConfirmarCambioDePIN.Click += btnConfirmarCambioDePIN_Click;
            // 
            // button12
            // 
            button12.BackColor = SystemColors.ControlLightLight;
            button12.FlatStyle = FlatStyle.Popup;
            button12.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button12.Location = new Point(187, 231);
            button12.Name = "button12";
            button12.Size = new Size(75, 64);
            button12.TabIndex = 39;
            button12.Text = "0";
            button12.UseVisualStyleBackColor = false;
            button12.Click += btnNumero_Click;
            // 
            // button11
            // 
            button11.BackColor = SystemColors.ControlLightLight;
            button11.FlatStyle = FlatStyle.Popup;
            button11.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button11.Location = new Point(106, 91);
            button11.Name = "button11";
            button11.Size = new Size(75, 64);
            button11.TabIndex = 38;
            button11.Text = "4";
            button11.UseVisualStyleBackColor = false;
            button11.Click += btnNumero_Click;
            // 
            // button10
            // 
            button10.BackColor = SystemColors.ControlLightLight;
            button10.FlatStyle = FlatStyle.Popup;
            button10.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button10.Location = new Point(106, 21);
            button10.Name = "button10";
            button10.Size = new Size(75, 64);
            button10.TabIndex = 30;
            button10.Text = "1";
            button10.UseVisualStyleBackColor = false;
            button10.Click += btnNumero_Click;
            // 
            // button9
            // 
            button9.BackColor = SystemColors.ControlLightLight;
            button9.FlatStyle = FlatStyle.Popup;
            button9.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button9.Location = new Point(106, 161);
            button9.Name = "button9";
            button9.Size = new Size(75, 64);
            button9.TabIndex = 37;
            button9.Text = "7";
            button9.UseVisualStyleBackColor = false;
            button9.Click += btnNumero_Click;
            // 
            // button3
            // 
            button3.BackColor = SystemColors.ControlLightLight;
            button3.FlatStyle = FlatStyle.Popup;
            button3.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button3.Location = new Point(187, 21);
            button3.Name = "button3";
            button3.Size = new Size(75, 64);
            button3.TabIndex = 31;
            button3.Text = "2";
            button3.UseVisualStyleBackColor = false;
            button3.Click += btnNumero_Click;
            // 
            // button8
            // 
            button8.BackColor = SystemColors.ControlLightLight;
            button8.FlatStyle = FlatStyle.Popup;
            button8.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button8.Location = new Point(187, 161);
            button8.Name = "button8";
            button8.Size = new Size(75, 64);
            button8.TabIndex = 36;
            button8.Text = "8";
            button8.UseVisualStyleBackColor = false;
            button8.Click += btnNumero_Click;
            // 
            // button4
            // 
            button4.BackColor = SystemColors.ControlLightLight;
            button4.FlatStyle = FlatStyle.Popup;
            button4.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button4.Location = new Point(187, 91);
            button4.Name = "button4";
            button4.Size = new Size(75, 64);
            button4.TabIndex = 32;
            button4.Text = "5";
            button4.UseVisualStyleBackColor = false;
            button4.Click += btnNumero_Click;
            // 
            // button7
            // 
            button7.BackColor = SystemColors.ControlLightLight;
            button7.FlatStyle = FlatStyle.Popup;
            button7.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button7.Location = new Point(268, 161);
            button7.Name = "button7";
            button7.Size = new Size(75, 64);
            button7.TabIndex = 35;
            button7.Text = "9";
            button7.UseVisualStyleBackColor = false;
            button7.Click += btnNumero_Click;
            // 
            // button5
            // 
            button5.BackColor = SystemColors.ControlLightLight;
            button5.FlatStyle = FlatStyle.Popup;
            button5.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button5.Location = new Point(268, 21);
            button5.Name = "button5";
            button5.Size = new Size(75, 64);
            button5.TabIndex = 33;
            button5.Text = "3";
            button5.UseVisualStyleBackColor = false;
            button5.Click += btnNumero_Click;
            // 
            // button6
            // 
            button6.BackColor = SystemColors.ControlLightLight;
            button6.FlatStyle = FlatStyle.Popup;
            button6.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button6.Location = new Point(268, 91);
            button6.Name = "button6";
            button6.Size = new Size(75, 64);
            button6.TabIndex = 34;
            button6.Text = "6";
            button6.UseVisualStyleBackColor = false;
            button6.Click += btnNumero_Click;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.WindowFrame;
            panel1.Controls.Add(button13);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(button3);
            panel1.Controls.Add(button6);
            panel1.Controls.Add(button12);
            panel1.Controls.Add(button5);
            panel1.Controls.Add(button7);
            panel1.Controls.Add(button11);
            panel1.Controls.Add(button4);
            panel1.Controls.Add(button8);
            panel1.Controls.Add(button10);
            panel1.Controls.Add(button9);
            panel1.Controls.Add(btnConfirmarCambioDePIN);
            panel1.Location = new Point(254, 302);
            panel1.Name = "panel1";
            panel1.Size = new Size(671, 311);
            panel1.TabIndex = 40;
            // 
            // button13
            // 
            button13.BackColor = SystemColors.ControlLightLight;
            button13.FlatStyle = FlatStyle.Popup;
            button13.Font = new Font("Segoe UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button13.Location = new Point(268, 231);
            button13.Name = "button13";
            button13.Size = new Size(75, 64);
            button13.TabIndex = 41;
            button13.Text = "<--";
            button13.UseVisualStyleBackColor = false;
            button13.Click += btnBorrar_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(255, 128, 0);
            button1.FlatStyle = FlatStyle.Popup;
            button1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = SystemColors.ControlLightLight;
            button1.Location = new Point(382, 90);
            button1.Name = "button1";
            button1.Size = new Size(196, 64);
            button1.TabIndex = 41;
            button1.Text = "Limpiar";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // PantallaCambioDePIN
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ScrollBar;
            ClientSize = new Size(1231, 625);
            Controls.Add(panel1);
            Controls.Add(dtpVencimiento);
            Controls.Add(txtPIN_nuevo);
            Controls.Add(label1);
            Controls.Add(txtCodigoDeVerificacion);
            Controls.Add(label5);
            Controls.Add(label6);
            Controls.Add(label7);
            Controls.Add(label8);
            Controls.Add(txtPIN_actual);
            Controls.Add(txtNumeroDeTarjeta);
            Name = "PantallaCambioDePIN";
            Text = "PantallaCambioDePIN";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtCodigoDeVerificacion;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private TextBox txtPIN_actual;
        private TextBox txtNumeroDeTarjeta;
        private Button btnConfirmarCambioDePIN;
        private TextBox txtPIN_nuevo;
        private Label label1;
        private DateTimePicker dtpVencimiento;
        private Button button12;
        private Button button11;
        private Button button10;
        private Button button9;
        private Button button3;
        private Button button8;
        private Button button4;
        private Button button7;
        private Button button5;
        private Button button6;
        private Panel panel1;
        private Button button1;
        private Button button13;
    }
}
