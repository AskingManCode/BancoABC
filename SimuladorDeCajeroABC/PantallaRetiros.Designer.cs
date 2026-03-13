namespace SimuladorDeCajeroABC
{
    partial class PantallaRetiros
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
            button1 = new Button();
            dtpVencimiento = new DateTimePicker();
            txtMontoRetiro = new TextBox();
            label5 = new Label();
            txtCodigoVerficacion = new TextBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            txtPIN = new TextBox();
            txtNumeroDeTarjeta = new TextBox();
            button2 = new Button();
            button10 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            button8 = new Button();
            button9 = new Button();
            button11 = new Button();
            button12 = new Button();
            panel1 = new Panel();
            btnVolver = new Button();
            button13 = new Button();
            panel2 = new Panel();
            lblCodigoEnRojo = new Label();
            lblInstruccionesCodigo = new Label();
            lblCodigoDeSeguridad = new Label();
            txtIngresarCodigo = new TextBox();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(0, 192, 0);
            button1.FlatStyle = FlatStyle.Popup;
            button1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = SystemColors.ControlLightLight;
            button1.Location = new Point(426, 17);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(224, 85);
            button1.TabIndex = 0;
            button1.Text = "Confirmar ";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // dtpVencimiento
            // 
            dtpVencimiento.CustomFormat = "MM/yyyy";
            dtpVencimiento.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpVencimiento.Format = DateTimePickerFormat.Custom;
            dtpVencimiento.Location = new Point(678, 237);
            dtpVencimiento.Margin = new Padding(3, 4, 3, 4);
            dtpVencimiento.Name = "dtpVencimiento";
            dtpVencimiento.ShowUpDown = true;
            dtpVencimiento.Size = new Size(367, 61);
            dtpVencimiento.TabIndex = 12;
            // 
            // txtMontoRetiro
            // 
            txtMontoRetiro.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtMontoRetiro.Location = new Point(586, 312);
            txtMontoRetiro.Margin = new Padding(3, 4, 3, 4);
            txtMontoRetiro.Name = "txtMontoRetiro";
            txtMontoRetiro.Size = new Size(459, 61);
            txtMontoRetiro.TabIndex = 11;
            txtMontoRetiro.Enter += txtMontoRetiro_Enter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.ForeColor = SystemColors.ActiveCaptionText;
            label5.Location = new Point(282, 319);
            label5.Name = "label5";
            label5.Size = new Size(305, 54);
            label5.TabIndex = 10;
            label5.Text = "Monto de retiro";
            // 
            // txtCodigoVerficacion
            // 
            txtCodigoVerficacion.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCodigoVerficacion.Location = new Point(678, 163);
            txtCodigoVerficacion.Margin = new Padding(3, 4, 3, 4);
            txtCodigoVerficacion.Name = "txtCodigoVerficacion";
            txtCodigoVerficacion.Size = new Size(367, 61);
            txtCodigoVerficacion.TabIndex = 9;
            txtCodigoVerficacion.UseSystemPasswordChar = true;
            txtCodigoVerficacion.Enter += txtCodigoVerficacion_Enter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = SystemColors.ActiveCaptionText;
            label4.Location = new Point(282, 169);
            label4.Name = "label4";
            label4.Size = new Size(418, 54);
            label4.TabIndex = 8;
            label4.Text = "Codigo de verificación";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = SystemColors.ActiveCaptionText;
            label3.Location = new Point(286, 93);
            label3.Name = "label3";
            label3.Size = new Size(86, 54);
            label3.TabIndex = 7;
            label3.Text = "PIN";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ActiveCaptionText;
            label2.Location = new Point(286, 244);
            label2.Name = "label2";
            label2.Size = new Size(408, 54);
            label2.TabIndex = 6;
            label2.Text = "Fecha de vencimiento";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(286, 19);
            label1.Name = "label1";
            label1.Size = new Size(351, 54);
            label1.TabIndex = 5;
            label1.Text = "Número de Tarjeta";
            // 
            // txtPIN
            // 
            txtPIN.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPIN.Location = new Point(678, 87);
            txtPIN.Margin = new Padding(3, 4, 3, 4);
            txtPIN.Name = "txtPIN";
            txtPIN.Size = new Size(367, 61);
            txtPIN.TabIndex = 4;
            txtPIN.UseSystemPasswordChar = true;
            txtPIN.Enter += txtPIN_Enter;
            // 
            // txtNumeroDeTarjeta
            // 
            txtNumeroDeTarjeta.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNumeroDeTarjeta.HideSelection = false;
            txtNumeroDeTarjeta.Location = new Point(678, 12);
            txtNumeroDeTarjeta.Margin = new Padding(3, 4, 3, 4);
            txtNumeroDeTarjeta.Name = "txtNumeroDeTarjeta";
            txtNumeroDeTarjeta.Size = new Size(367, 61);
            txtNumeroDeTarjeta.TabIndex = 1;
            txtNumeroDeTarjeta.Enter += txtNumeroDeTarjeta_Enter;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(255, 128, 0);
            button2.FlatStyle = FlatStyle.Popup;
            button2.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.ForeColor = SystemColors.ControlLightLight;
            button2.Location = new Point(426, 112);
            button2.Margin = new Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new Size(224, 85);
            button2.TabIndex = 2;
            button2.Text = "Limpiar";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button10
            // 
            button10.BackColor = SystemColors.ControlLightLight;
            button10.FlatStyle = FlatStyle.Popup;
            button10.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button10.Location = new Point(122, 17);
            button10.Margin = new Padding(3, 4, 3, 4);
            button10.Name = "button10";
            button10.Size = new Size(86, 85);
            button10.TabIndex = 20;
            button10.Text = "1";
            button10.UseVisualStyleBackColor = false;
            button10.Click += btnNumero_Click;
            // 
            // button3
            // 
            button3.BackColor = SystemColors.ControlLightLight;
            button3.FlatStyle = FlatStyle.Popup;
            button3.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button3.Location = new Point(215, 17);
            button3.Margin = new Padding(3, 4, 3, 4);
            button3.Name = "button3";
            button3.Size = new Size(86, 85);
            button3.TabIndex = 21;
            button3.Text = "2";
            button3.UseVisualStyleBackColor = false;
            button3.Click += btnNumero_Click;
            // 
            // button4
            // 
            button4.BackColor = SystemColors.ControlLightLight;
            button4.FlatStyle = FlatStyle.Popup;
            button4.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button4.Location = new Point(215, 111);
            button4.Margin = new Padding(3, 4, 3, 4);
            button4.Name = "button4";
            button4.Size = new Size(86, 85);
            button4.TabIndex = 22;
            button4.Text = "5";
            button4.UseVisualStyleBackColor = false;
            button4.Click += btnNumero_Click;
            // 
            // button5
            // 
            button5.BackColor = SystemColors.ControlLightLight;
            button5.FlatStyle = FlatStyle.Popup;
            button5.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button5.Location = new Point(307, 17);
            button5.Margin = new Padding(3, 4, 3, 4);
            button5.Name = "button5";
            button5.Size = new Size(86, 85);
            button5.TabIndex = 23;
            button5.Text = "3";
            button5.UseVisualStyleBackColor = false;
            button5.Click += btnNumero_Click;
            // 
            // button6
            // 
            button6.BackColor = SystemColors.ControlLightLight;
            button6.FlatStyle = FlatStyle.Popup;
            button6.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button6.Location = new Point(307, 111);
            button6.Margin = new Padding(3, 4, 3, 4);
            button6.Name = "button6";
            button6.Size = new Size(86, 85);
            button6.TabIndex = 24;
            button6.Text = "6";
            button6.UseVisualStyleBackColor = false;
            button6.Click += btnNumero_Click;
            // 
            // button7
            // 
            button7.BackColor = SystemColors.ControlLightLight;
            button7.FlatStyle = FlatStyle.Popup;
            button7.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button7.Location = new Point(307, 204);
            button7.Margin = new Padding(3, 4, 3, 4);
            button7.Name = "button7";
            button7.Size = new Size(86, 85);
            button7.TabIndex = 25;
            button7.Text = "9";
            button7.UseVisualStyleBackColor = false;
            button7.Click += btnNumero_Click;
            // 
            // button8
            // 
            button8.BackColor = SystemColors.ControlLightLight;
            button8.FlatStyle = FlatStyle.Popup;
            button8.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button8.Location = new Point(215, 204);
            button8.Margin = new Padding(3, 4, 3, 4);
            button8.Name = "button8";
            button8.Size = new Size(86, 85);
            button8.TabIndex = 26;
            button8.Text = "8";
            button8.UseVisualStyleBackColor = false;
            button8.Click += btnNumero_Click;
            // 
            // button9
            // 
            button9.BackColor = SystemColors.ControlLightLight;
            button9.FlatStyle = FlatStyle.Popup;
            button9.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button9.Location = new Point(122, 204);
            button9.Margin = new Padding(3, 4, 3, 4);
            button9.Name = "button9";
            button9.Size = new Size(86, 85);
            button9.TabIndex = 27;
            button9.Text = "7";
            button9.UseVisualStyleBackColor = false;
            button9.Click += btnNumero_Click;
            // 
            // button11
            // 
            button11.BackColor = SystemColors.ControlLightLight;
            button11.FlatStyle = FlatStyle.Popup;
            button11.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button11.Location = new Point(122, 111);
            button11.Margin = new Padding(3, 4, 3, 4);
            button11.Name = "button11";
            button11.Size = new Size(86, 85);
            button11.TabIndex = 28;
            button11.Text = "4";
            button11.UseVisualStyleBackColor = false;
            button11.Click += btnNumero_Click;
            // 
            // button12
            // 
            button12.BackColor = SystemColors.ControlLightLight;
            button12.FlatStyle = FlatStyle.Popup;
            button12.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button12.Location = new Point(215, 297);
            button12.Margin = new Padding(3, 4, 3, 4);
            button12.Name = "button12";
            button12.Size = new Size(86, 85);
            button12.TabIndex = 29;
            button12.Text = "0";
            button12.UseVisualStyleBackColor = false;
            button12.Click += btnNumero_Click;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.WindowFrame;
            panel1.Controls.Add(btnVolver);
            panel1.Controls.Add(button13);
            panel1.Controls.Add(button12);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button11);
            panel1.Controls.Add(button10);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(button9);
            panel1.Controls.Add(button3);
            panel1.Controls.Add(button8);
            panel1.Controls.Add(button4);
            panel1.Controls.Add(button7);
            panel1.Controls.Add(button5);
            panel1.Controls.Add(button6);
            panel1.Location = new Point(286, 416);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(760, 403);
            panel1.TabIndex = 30;
            // 
            // btnVolver
            // 
            btnVolver.BackColor = Color.FromArgb(128, 255, 255);
            btnVolver.FlatStyle = FlatStyle.Popup;
            btnVolver.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnVolver.Location = new Point(426, 205);
            btnVolver.Margin = new Padding(3, 4, 3, 4);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(224, 84);
            btnVolver.TabIndex = 31;
            btnVolver.Text = "Volver";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Visible = false;
            // 
            // button13
            // 
            button13.BackColor = SystemColors.ControlLightLight;
            button13.FlatStyle = FlatStyle.Popup;
            button13.Font = new Font("Segoe UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button13.Location = new Point(307, 297);
            button13.Margin = new Padding(3, 4, 3, 4);
            button13.Name = "button13";
            button13.Size = new Size(86, 85);
            button13.TabIndex = 30;
            button13.Text = "<--";
            button13.UseVisualStyleBackColor = false;
            button13.Click += btnBorrar_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(lblCodigoEnRojo);
            panel2.Controls.Add(lblInstruccionesCodigo);
            panel2.Controls.Add(lblCodigoDeSeguridad);
            panel2.Controls.Add(txtIngresarCodigo);
            panel2.Location = new Point(271, 12);
            panel2.Margin = new Padding(3, 4, 3, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(775, 375);
            panel2.TabIndex = 31;
            panel2.Visible = false;
            // 
            // lblCodigoEnRojo
            // 
            lblCodigoEnRojo.AutoSize = true;
            lblCodigoEnRojo.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCodigoEnRojo.ForeColor = Color.Red;
            lblCodigoEnRojo.Location = new Point(441, 157);
            lblCodigoEnRojo.Name = "lblCodigoEnRojo";
            lblCodigoEnRojo.Size = new Size(0, 41);
            lblCodigoEnRojo.TabIndex = 3;
            lblCodigoEnRojo.Visible = false;
            // 
            // lblInstruccionesCodigo
            // 
            lblInstruccionesCodigo.AutoSize = true;
            lblInstruccionesCodigo.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblInstruccionesCodigo.Location = new Point(48, 113);
            lblInstruccionesCodigo.Name = "lblInstruccionesCodigo";
            lblInstruccionesCodigo.Size = new Size(768, 32);
            lblInstruccionesCodigo.TabIndex = 2;
            lblInstruccionesCodigo.Text = "Ingrese su codigo de verficación en el espacio para confirmar la acción";
            lblInstruccionesCodigo.Visible = false;
            // 
            // lblCodigoDeSeguridad
            // 
            lblCodigoDeSeguridad.AutoSize = true;
            lblCodigoDeSeguridad.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCodigoDeSeguridad.Location = new Point(93, 157);
            lblCodigoDeSeguridad.Name = "lblCodigoDeSeguridad";
            lblCodigoDeSeguridad.Size = new Size(387, 41);
            lblCodigoDeSeguridad.TabIndex = 1;
            lblCodigoDeSeguridad.Text = "Su Codigo autorización es : ";
            lblCodigoDeSeguridad.Visible = false;
            // 
            // txtIngresarCodigo
            // 
            txtIngresarCodigo.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtIngresarCodigo.Location = new Point(199, 232);
            txtIngresarCodigo.Margin = new Padding(3, 4, 3, 4);
            txtIngresarCodigo.Name = "txtIngresarCodigo";
            txtIngresarCodigo.Size = new Size(399, 61);
            txtIngresarCodigo.TabIndex = 0;
            txtIngresarCodigo.Visible = false;
            txtIngresarCodigo.Enter += txtIngresarCodigo_Enter;
            // 
            // PantallaRetiros
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ScrollBar;
            ClientSize = new Size(1407, 848);
            Controls.Add(panel2);
            Controls.Add(dtpVencimiento);
            Controls.Add(txtMontoRetiro);
            Controls.Add(label5);
            Controls.Add(txtCodigoVerficacion);
            Controls.Add(label4);
            Controls.Add(txtNumeroDeTarjeta);
            Controls.Add(label3);
            Controls.Add(txtPIN);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(panel1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "PantallaRetiros";
            Text = "PantallaRetiros";
            FormClosed += PantallaRetiros_FormClosed;
            Load += PantallaRetiros_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox txtPIN;
        private TextBox txtNumeroDeTarjeta;
        private TextBox txtCodigoVerficacion;
        private Label label4;
        private TextBox txtMontoRetiro;
        private Label label5;
        private DateTimePicker dtpVencimiento;
        private Button button2;
        private Button button10;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
        private Button button9;
        private Button button11;
        private Button button12;
        private Panel panel1;
        private Button button13;
        private Panel panel2;
        private TextBox txtIngresarCodigo;
        private Label lblCodigoDeSeguridad;
        private Label lblInstruccionesCodigo;
        private Label lblCodigoEnRojo;
        private Button btnVolver;
    }
}