using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocio;

namespace SimuladorCajero_BancoABC
{
    public partial class PantallaCambioDePIN : Form
    {
        private string CodigoCajero;
        private TextBox textBoxActivo;

        public PantallaCambioDePIN(string codigo) // Pantallita
        {
            InitializeComponent();
            this.CodigoCajero = codigo;
        }
        
        private async void btnConfirmarCambioDePIN_Click(object sender, EventArgs e)
        {
            if (!ValidarCamposVacios()) return;

            try
            {
                CifradoDeDatos cifrador = new CifradoDeDatos();
                string tarjetaCifrado = cifrador.Cifrar(txtNumeroDeTarjeta.Text.Trim());
                string pinActualCifrado = cifrador.Cifrar(txtPIN_actual.Text.Trim());
                string pinNuevoCifrado = cifrador.Cifrar(txtPIN_nuevo.Text.Trim());
                string fechaCifrado = cifrador.Cifrar(dtpVencimiento.Text.Trim());
                string cvvCifrado = cifrador.Cifrar(txtCodigoDeVerificacion.Text.Trim());
                string cajeroCifrado = cifrador.Cifrar(CodigoCajero.Trim());

                AutorizadorWS.AutorizadorServiceClient cliente = new AutorizadorWS.AutorizadorServiceClient();
                AutorizadorWS.RespuestaSimple respuesta = await cliente.CambiarPINAsync(
                    tarjetaCifrado,
                    pinActualCifrado,
                    pinNuevoCifrado,
                    fechaCifrado,
                    cvvCifrado,
                    cajeroCifrado
                );

                if (respuesta.Resultado)
                {
                    MessageBox.Show("El PIN ha sido cambiado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void txtNumeroDeTarjeta_Enter(object sender, EventArgs e)
        {
            textBoxActivo = (TextBox)sender;
        }

        private void txtPIN_actual_Enter(object sender, EventArgs e)
        {
            textBoxActivo = (TextBox)sender;
        }

        private void txtPIN_nuevo_Enter(object sender, EventArgs e)
        {
            textBoxActivo = (TextBox)sender;
        }

        private void txtCodigoDeVerificacion_Enter(object sender, EventArgs e)
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
            txtCodigoDeVerificacion.Text = "";
            txtNumeroDeTarjeta.Text = "";
            txtPIN_actual.Text = "";
            txtPIN_nuevo.Text = "";
        }

        public bool ValidarCamposVacios()
        {
            if (string.IsNullOrWhiteSpace(txtNumeroDeTarjeta.Text) ||
                string.IsNullOrWhiteSpace(txtCodigoDeVerificacion.Text) ||
                string.IsNullOrWhiteSpace(txtPIN_actual.Text) ||
                string.IsNullOrWhiteSpace(txtPIN_nuevo.Text))
            {
                MessageBox.Show(
                        "Debe completar todos los campos para solicitar el cambio de pin",
                        "Campos incompletos",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                return false;

            }
            return true;
        }
    }
}
