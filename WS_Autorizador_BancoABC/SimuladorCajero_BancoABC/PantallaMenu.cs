namespace SimuladorCajero_BancoABC
{
    public partial class PantallaMenu : Form
    {
        private string codigo;

        public PantallaMenu(int codigo)
        {
            InitializeComponent();
            GenerarCodigoDeCajero(codigo);
            lblIdentificadorDeCajero.Text = lblIdentificadorDeCajero.Text + this.codigo.ToString();
        }

        private void AbrirPantalla(Form form)
        {
            PanelContenido.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            PanelContenido.Controls.Add(form);
            form.Show();
        }

        private void retirosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            label2.Visible = false;
            AbrirPantalla(new PantallaRetiros(codigo));
        }

        private void cambioDePINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            label2.Visible = false;
            AbrirPantalla(new PantallaCambioDePIN(codigo));
        }

        private void consultaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            label2.Visible = false;
            AbrirPantalla(new PantallaDeConsulta(codigo));
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            label2.Visible = false;
            Application.Exit();
        }

        public void GenerarCodigoDeCajero(int codigo)
        {
            this.codigo = "SIM-00" + codigo.ToString();
        }

        //Prueba de commit

    }
}
