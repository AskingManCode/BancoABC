using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimuladorCajero_BancoABC
{
    public partial class PreguntaInicial : Form
    {
        public PreguntaInicial()
        {
            InitializeComponent();
        }

        private void PreguntaInicial_Load(object sender, EventArgs e)
        {

        }

        int CajerosActivos;

        private void btnAceptar_Click(object sender, EventArgs e)
        {

            try
            {
                CajerosActivos = Convert.ToInt32(txtBoxN_Cajeros.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("El valor ingresado es invalido", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (CajerosActivos < 1)
            {
                MessageBox.Show("El valor ingresado no puede ser menor a 1", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; i < CajerosActivos; i++)
            {
                PantallaMenu PM = new PantallaMenu(i+1);
                PM.Show();    
            }

            this.Hide();
        }
    }
}
