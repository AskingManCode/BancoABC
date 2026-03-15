using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocio;

namespace SimuladorDeCajeroABC
{
    public partial class PantallaDeConsulta : Form
    {
        private string CodigoCajero;
        private TextBox textBoxActivo;

        public PantallaDeConsulta(string codigo)
        {
            InitializeComponent();
            this.CodigoCajero = codigo;
        }

        private async void btnConsultarSaldo_Click(object sender, EventArgs e)
        {
            if (!ValidarCamposVacios()) return;

            try
            {
                CifradoDeDatos cifrador = new CifradoDeDatos();
                string tarjetaCifrado = cifrador.Cifrar(txtNumeroDeTarjeta.Text.Trim());
                string cvvCifrado = cifrador.Cifrar(txtCodigoVerificacion.Text.Trim());
                string fechaCifrado = cifrador.Cifrar(dtpVencimiento.Text.Trim()); // formato dd/MM/yyyy
                string cajeroCifrado = cifrador.Cifrar(CodigoCajero.Trim());

                AutorizadorWS.AutorizadorServiceClient cliente = new AutorizadorWS.AutorizadorServiceClient();
                AutorizadorWS.RespuestaConsulta respuesta = await cliente.ConsultarSaldoAsync(
                    tarjetaCifrado,
                    cvvCifrado,
                    fechaCifrado,
                    cajeroCifrado
                );

                if (respuesta.Resultado)
                {
                    lblSaldoEnVerde.Text = respuesta.Saldo;
                    MostrarConsulta();
                    limpiarCampos();
                }
                else
                {
                    MessageBox.Show(respuesta.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void MostrarConsulta()
        {
            lbl1.Visible = true;
            PanelConsulta.Visible = true;
        }

        private void txtNumeroDeTarjeta_Enter(object sender, EventArgs e)
        {
            textBoxActivo = (TextBox)sender;
        }

        private void txtPIN_Enter(object sender, EventArgs e)
        {
            textBoxActivo = (TextBox)sender;
        }

        private void txtCodigoVerificacion_Enter(object sender, EventArgs e)
        {
            textBoxActivo = (TextBox)sender;
        }

        private void btnNumero_Click(object sender, EventArgs e)
        {
            if (textBoxActivo == null) return;

            Button boton = (Button)sender;
            textBoxActivo.Text += boton.Text;
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (textBoxActivo == null) return;

            if (textBoxActivo.Text.Length == 0) return;

            textBoxActivo.Text =
                textBoxActivo.Text.Substring(0, textBoxActivo.Text.Length - 1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            limpiarCampos();
        }

        private void limpiarCampos()
        {
            txtCodigoVerificacion.Text = "";
            txtNumeroDeTarjeta.Text = "";
        }

        public bool ValidarCamposVacios()
        {
            if (string.IsNullOrWhiteSpace(txtNumeroDeTarjeta.Text) ||
                string.IsNullOrWhiteSpace(txtCodigoVerificacion.Text))
            {
                MessageBox.Show("Debe completar todos los campos para realizar la consulta", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
    }
}
