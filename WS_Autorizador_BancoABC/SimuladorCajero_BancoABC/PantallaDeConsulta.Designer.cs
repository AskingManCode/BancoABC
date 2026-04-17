namespace SimuladorCajero_BancoABC
{
    partial class PantallaDeConsulta
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
            label2 = new Label();
            label1 = new Label();
            txtNumeroDeTarjeta = new TextBox();
            txtPIN = new TextBox();
            label6 = new Label();
            label7 = new Label();
            dtpVencimiento = new DateTimePicker();
            txtCodigoVerificacion = new TextBox();
            btnConsultarSaldo = new Button();
            PanelConsulta = new Panel();
            lblSaldoEnVerde = new Label();
            lbl1 = new Label();
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            button13 = new Button();
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
            panel2 = new Panel();
            button1 = new Button();
            PanelConsulta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 20.25F);
            label2.ForeColor = SystemColors.ActiveCaptionText;
            label2.Location = new Point(17, 18);
            label2.Name = "label2";
            label2.Size = new Size(234, 37);
            label2.TabIndex = 7;
            label2.Text = "Numero de tarjeta";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(13, 116);
            label1.Name = "label1";
            label1.Size = new Size(59, 37);
            label1.TabIndex = 8;
            label1.Text = "PIN";
            // 
            // txtNumeroDeTarjeta
            // 
            txtNumeroDeTarjeta.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNumeroDeTarjeta.Location = new Point(297, 12);
            txtNumeroDeTarjeta.Name = "txtNumeroDeTarjeta";
            txtNumeroDeTarjeta.Size = new Size(223, 43);
            txtNumeroDeTarjeta.TabIndex = 9;
            txtNumeroDeTarjeta.Enter += txtNumeroDeTarjeta_Enter;
            // 
            // txtPIN
            // 
            txtPIN.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPIN.Location = new Point(297, 110);
            txtPIN.Name = "txtPIN";
            txtPIN.Size = new Size(222, 43);
            txtPIN.TabIndex = 10;
            txtPIN.Enter += txtPIN_Enter;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 20.25F);
            label6.ForeColor = SystemColors.ActiveCaptionText;
            label6.Location = new Point(12, 165);
            label6.Name = "label6";
            label6.Size = new Size(278, 37);
            label6.TabIndex = 12;
            label6.Text = "codigo de verificación";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 20.25F);
            label7.ForeColor = SystemColors.ActiveCaptionText;
            label7.Location = new Point(16, 67);
            label7.Name = "label7";
            label7.Size = new Size(274, 37);
            label7.TabIndex = 13;
            label7.Text = "Fecha de vencimiento";
            // 
            // dtpVencimiento
            // 
            dtpVencimiento.CustomFormat = "MM/yyyy";
            dtpVencimiento.DropDownAlign = LeftRightAlignment.Right;
            dtpVencimiento.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpVencimiento.Format = DateTimePickerFormat.Custom;
            dtpVencimiento.Location = new Point(297, 61);
            dtpVencimiento.Name = "dtpVencimiento";
            dtpVencimiento.ShowUpDown = true;
            dtpVencimiento.Size = new Size(223, 43);
            dtpVencimiento.TabIndex = 14;
            // 
            // txtCodigoVerificacion
            // 
            txtCodigoVerificacion.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCodigoVerificacion.Location = new Point(297, 159);
            txtCodigoVerificacion.Name = "txtCodigoVerificacion";
            txtCodigoVerificacion.Size = new Size(223, 43);
            txtCodigoVerificacion.TabIndex = 15;
            txtCodigoVerificacion.Enter += txtCodigoVerificacion_Enter;
            // 
            // btnConsultarSaldo
            // 
            btnConsultarSaldo.BackColor = Color.FromArgb(0, 192, 0);
            btnConsultarSaldo.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnConsultarSaldo.ForeColor = SystemColors.ControlLightLight;
            btnConsultarSaldo.Location = new Point(53, 313);
            btnConsultarSaldo.Name = "btnConsultarSaldo";
            btnConsultarSaldo.Size = new Size(192, 57);
            btnConsultarSaldo.TabIndex = 16;
            btnConsultarSaldo.Text = "Confirmar";
            btnConsultarSaldo.UseVisualStyleBackColor = false;
            btnConsultarSaldo.Click += btnConsultarSaldo_Click;
            // 
            // PanelConsulta
            // 
            PanelConsulta.BackColor = SystemColors.MenuHighlight;
            PanelConsulta.Controls.Add(lblSaldoEnVerde);
            PanelConsulta.Controls.Add(lbl1);
            PanelConsulta.Location = new Point(22, 20);
            PanelConsulta.Name = "PanelConsulta";
            PanelConsulta.Size = new Size(626, 115);
            PanelConsulta.TabIndex = 0;
            PanelConsulta.Visible = false;
            // 
            // lblSaldoEnVerde
            // 
            lblSaldoEnVerde.AutoSize = true;
            lblSaldoEnVerde.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSaldoEnVerde.ForeColor = Color.Chartreuse;
            lblSaldoEnVerde.Location = new Point(99, 9);
            lblSaldoEnVerde.Name = "lblSaldoEnVerde";
            lblSaldoEnVerde.Size = new Size(0, 30);
            lblSaldoEnVerde.TabIndex = 1;
            // 
            // lbl1
            // 
            lbl1.AutoSize = true;
            lbl1.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl1.ForeColor = SystemColors.ButtonHighlight;
            lbl1.Location = new Point(12, 9);
            lbl1.Name = "lbl1";
            lbl1.Size = new Size(81, 30);
            lbl1.TabIndex = 0;
            lbl1.Text = "Saldo : ";
            lbl1.Visible = false;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.WhatsApp_Image_2026_02_01_at_1_31_59_AM;
            pictureBox1.Location = new Point(552, 470);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(110, 110);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.HighlightText;
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(PanelConsulta);
            panel1.Location = new Point(539, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(680, 601);
            panel1.TabIndex = 11;
            // 
            // button13
            // 
            button13.BackColor = SystemColors.ControlLightLight;
            button13.FlatStyle = FlatStyle.Popup;
            button13.Font = new Font("Segoe UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button13.Location = new Point(305, 448);
            button13.Name = "button13";
            button13.Size = new Size(75, 64);
            button13.TabIndex = 41;
            button13.Text = "<--";
            button13.UseVisualStyleBackColor = false;
            button13.Click += btnBorrar_Click;
            // 
            // button12
            // 
            button12.BackColor = SystemColors.ControlLightLight;
            button12.FlatStyle = FlatStyle.Popup;
            button12.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button12.Location = new Point(224, 448);
            button12.Name = "button12";
            button12.Size = new Size(75, 64);
            button12.TabIndex = 40;
            button12.Text = "0";
            button12.UseVisualStyleBackColor = false;
            button12.Click += btnNumero_Click;
            // 
            // button11
            // 
            button11.BackColor = SystemColors.ControlLightLight;
            button11.FlatStyle = FlatStyle.Popup;
            button11.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button11.Location = new Point(143, 308);
            button11.Name = "button11";
            button11.Size = new Size(75, 64);
            button11.TabIndex = 39;
            button11.Text = "4";
            button11.UseVisualStyleBackColor = false;
            button11.Click += btnNumero_Click;
            // 
            // button10
            // 
            button10.BackColor = SystemColors.ControlLightLight;
            button10.FlatStyle = FlatStyle.Popup;
            button10.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button10.Location = new Point(143, 238);
            button10.Name = "button10";
            button10.Size = new Size(75, 64);
            button10.TabIndex = 31;
            button10.Text = "1";
            button10.UseVisualStyleBackColor = false;
            button10.Click += btnNumero_Click;
            // 
            // button9
            // 
            button9.BackColor = SystemColors.ControlLightLight;
            button9.FlatStyle = FlatStyle.Popup;
            button9.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button9.Location = new Point(143, 378);
            button9.Name = "button9";
            button9.Size = new Size(75, 64);
            button9.TabIndex = 38;
            button9.Text = "7";
            button9.UseVisualStyleBackColor = false;
            button9.Click += btnNumero_Click;
            // 
            // button3
            // 
            button3.BackColor = SystemColors.ControlLightLight;
            button3.FlatStyle = FlatStyle.Popup;
            button3.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button3.Location = new Point(224, 238);
            button3.Name = "button3";
            button3.Size = new Size(75, 64);
            button3.TabIndex = 32;
            button3.Text = "2";
            button3.UseVisualStyleBackColor = false;
            button3.Click += btnNumero_Click;
            // 
            // button8
            // 
            button8.BackColor = SystemColors.ControlLightLight;
            button8.FlatStyle = FlatStyle.Popup;
            button8.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button8.Location = new Point(224, 378);
            button8.Name = "button8";
            button8.Size = new Size(75, 64);
            button8.TabIndex = 37;
            button8.Text = "8";
            button8.UseVisualStyleBackColor = false;
            button8.Click += btnNumero_Click;
            // 
            // button4
            // 
            button4.BackColor = SystemColors.ControlLightLight;
            button4.FlatStyle = FlatStyle.Popup;
            button4.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button4.Location = new Point(224, 308);
            button4.Name = "button4";
            button4.Size = new Size(75, 64);
            button4.TabIndex = 33;
            button4.Text = "5";
            button4.UseVisualStyleBackColor = false;
            button4.Click += btnNumero_Click;
            // 
            // button7
            // 
            button7.BackColor = SystemColors.ControlLightLight;
            button7.FlatStyle = FlatStyle.Popup;
            button7.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button7.Location = new Point(305, 378);
            button7.Name = "button7";
            button7.Size = new Size(75, 64);
            button7.TabIndex = 36;
            button7.Text = "9";
            button7.UseVisualStyleBackColor = false;
            button7.Click += btnNumero_Click;
            // 
            // button5
            // 
            button5.BackColor = SystemColors.ControlLightLight;
            button5.FlatStyle = FlatStyle.Popup;
            button5.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button5.Location = new Point(305, 238);
            button5.Name = "button5";
            button5.Size = new Size(75, 64);
            button5.TabIndex = 34;
            button5.Text = "3";
            button5.UseVisualStyleBackColor = false;
            button5.Click += btnNumero_Click;
            // 
            // button6
            // 
            button6.BackColor = SystemColors.ControlLightLight;
            button6.FlatStyle = FlatStyle.Popup;
            button6.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button6.Location = new Point(305, 308);
            button6.Name = "button6";
            button6.Size = new Size(75, 64);
            button6.TabIndex = 35;
            button6.Text = "6";
            button6.UseVisualStyleBackColor = false;
            button6.Click += btnNumero_Click;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.WindowFrame;
            panel2.Controls.Add(button1);
            panel2.Controls.Add(btnConsultarSaldo);
            panel2.Location = new Point(17, 222);
            panel2.Name = "panel2";
            panel2.Size = new Size(503, 391);
            panel2.TabIndex = 42;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(255, 128, 0);
            button1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = SystemColors.ControlLightLight;
            button1.Location = new Point(260, 314);
            button1.Name = "button1";
            button1.Size = new Size(192, 56);
            button1.TabIndex = 17;
            button1.Text = "Limpiar";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // PantallaDeConsulta
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ScrollBar;
            ClientSize = new Size(1231, 625);
            Controls.Add(button13);
            Controls.Add(button12);
            Controls.Add(button11);
            Controls.Add(button10);
            Controls.Add(button9);
            Controls.Add(button3);
            Controls.Add(button8);
            Controls.Add(button4);
            Controls.Add(button7);
            Controls.Add(button5);
            Controls.Add(button6);
            Controls.Add(txtCodigoVerificacion);
            Controls.Add(dtpVencimiento);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(panel1);
            Controls.Add(txtPIN);
            Controls.Add(txtNumeroDeTarjeta);
            Controls.Add(label1);
            Controls.Add(label2);
            Controls.Add(panel2);
            Name = "PantallaDeConsulta";
            Text = "PantallaDeConsulta";
            PanelConsulta.ResumeLayout(false);
            PanelConsulta.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label2;
        private Label label1;
        private TextBox txtNumeroDeTarjeta;
        private TextBox txtPIN;
        private Label label6;
        private Label label7;
        private DateTimePicker dtpVencimiento;
        private TextBox txtCodigoVerificacion;
        private Button btnConsultarSaldo;
        private Panel PanelConsulta;
        private Label lbl1;
        private PictureBox pictureBox1;
        private Panel panel1;
        private Button button13;
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
        private Panel panel2;
        private Button button1;
        private Label lblSaldoEnVerde;
    }
}
