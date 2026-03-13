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

namespace SimuladorDeCajeroABC
{
    public partial class PantallaCambioDePIN : Form
    {
        private string CodigoCajero;
        private TextBox textBoxActivo;

        public PantallaCambioDePIN(string codigo)
        {
            InitializeComponent();
            this.CodigoCajero = codigo;
        }
        
        private async void btnConfirmarCambioDePIN_Click(object sender, EventArgs e)
        {
            if (!ValidarCamposVacios())
            {
                return;
            }

            try
            {
                AutorizadorWS.AutorizadorServiceClient cliente =
                new AutorizadorWS.AutorizadorServiceClient();

                AutorizadorWS.RespuestaSimple respuesta =
                await cliente.CambiarPINAsync(
                    txtNumeroDeTarjeta.Text,
                    txtPIN_actual.Text,
                    txtPIN_nuevo.Text,
                    dtpVencimiento.Text,
                    txtCodigoDeVerificacion.Text,
                    CodigoCajero
                );

                if (respuesta.Resultado)
                {
                    MessageBox.Show(
                        "El PIN ha sido cambiado correctamente",
                        "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    limpiarCampos();
                }
                else
                {
                    MessageBox.Show(
                        respuesta.Mensaje,
                        "No se puede realizar la acción",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch
            {
                MessageBox.Show(
                    "No se pudo conectar con el autorizador",
                    "Error de conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
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
