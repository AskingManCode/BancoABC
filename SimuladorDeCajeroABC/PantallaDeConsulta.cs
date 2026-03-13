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
            if (!ValidarCamposVacios())
            {
                return;
            }

            try
            {
                AutorizadorWS.AutorizadorServiceClient cliente =
                new AutorizadorWS.AutorizadorServiceClient();

                            AutorizadorWS.RespuestaConsulta respuesta =
                                await cliente.ConsultarSaldoAsync(
                                    txtNumeroDeTarjeta.Text,
                                    txtCodigoVerificacion.Text,
                                    dtpVencimiento.Text,
                                    CodigoCajero
                                );

                if (respuesta.Resultado)
                {
                    MostrarConsulta();
                    lblSaldoEnVerde.Text = respuesta.Saldo;
                    limpiarCampos();
                }
                else
                {
                    MessageBox.Show(respuesta.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtPIN.Text = "";
        }

        public bool ValidarCamposVacios()
        {
            if (string.IsNullOrWhiteSpace(txtNumeroDeTarjeta.Text)|| string.IsNullOrWhiteSpace(txtCodigoVerificacion.Text) || string.IsNullOrWhiteSpace(txtPIN.Text))
            {
                MessageBox.Show(
                        "Debe completar todos los campos para realizar la consulta",
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
