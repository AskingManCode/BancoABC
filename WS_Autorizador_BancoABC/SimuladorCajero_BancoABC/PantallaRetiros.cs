using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocio;

namespace SimuladorDeCajeroABC
{
    public partial class PantallaRetiros : Form
    {
        private string CodigoCajero;
        private ConexionTCP conexion;
        private TextBox textBoxActivo;

        CifradoDeDatos EDD = new CifradoDeDatos();

        public PantallaRetiros(string codigo)
        {
            InitializeComponent();
            CodigoCajero = codigo;
        }

        private void PantallaRetiros_Load(object sender, EventArgs e)
        {
            try
            {
                conexion = new ConexionTCP("127.0.0.1", 5000);
                conexion.Conectar();
                panel2.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo establecer conexión con el autorizador.\n" + ex.Message,
                    "Error de conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (conexion == null || !conexion.EstaConectado())
            {
                MessageBox.Show("No hay conexión con el autorizador.");
                return;
            }

            ProcesarRetiro();
        }

        public void ProcesarRetiro()
        {
            if (!ValidarCamposVacios()) return;

            string fecha = Convert.ToDateTime("01/" + dtpVencimiento.Text).ToString("yyyy-MM-dd");

            decimal monto;
            decimal.TryParse(txtMontoRetiro.Text, out monto);

            string pinCifrado = "";
            if (monto > 50000 && !string.IsNullOrWhiteSpace(txtPIN.Text))
            {
                pinCifrado = EDD.Cifrar(txtPIN.Text);
            }

            var json = JsonSerializer.Serialize(new
            {
                NumeroDeTarjeta = EDD.Cifrar(txtNumeroDeTarjeta.Text),
                Pin = pinCifrado,
                FechaDeVencimiento = EDD.Cifrar(fecha),
                CodigoDeVerificacion = EDD.Cifrar(txtCodigoVerficacion.Text),
                IdentificadorDelCajero = CodigoCajero,
                TipoDeTransaccion = "Retiro",
                MontoRetiro = txtMontoRetiro.Text
            });

            string respuestaJson = conexion.EnviarYRecibir(json);
            var respuesta = JsonDocument.Parse(respuestaJson);

            string status = respuesta.RootElement.GetProperty("status").GetString();

            if (status == "OK")
            {
                MessageBox.Show("Retiro autorizado");
                limpiarCampos();
                return;
            }

            MostrarError(status);
        }

        private void MostrarError(string status)
        {
            string mensaje = status switch
            {
                "1" => "Fondos insuficientes",
                "2" => "Datos incorrectos",
                "3" => "Tarjeta inactiva",
                "4" => "Tarjeta vencida",
                "5" => "Error en la transacción",
                _ => "Error no controlado"
            };

            MessageBox.Show(mensaje);
        }

        private bool ValidarCamposVacios()
        {
            if (string.IsNullOrWhiteSpace(txtNumeroDeTarjeta.Text) ||
                string.IsNullOrWhiteSpace(txtCodigoVerficacion.Text) ||
                string.IsNullOrWhiteSpace(txtMontoRetiro.Text))
            {
                MessageBox.Show("Complete todos los campos");
                return false;
            }

            decimal monto;
            decimal.TryParse(txtMontoRetiro.Text, out monto);

            if (monto > 50000 && string.IsNullOrWhiteSpace(txtPIN.Text))
            {
                MessageBox.Show("Ingrese el PIN");
                return false;
            }

            return true;
        }

        private void limpiarCampos()
        {
            txtNumeroDeTarjeta.Clear();
            txtPIN.Clear();
            txtCodigoVerficacion.Clear();
            txtMontoRetiro.Clear();
            txtIngresarCodigo.Clear();

            lblCodigoEnRojo.Text = "";
            panel2.Visible = false;

            txtNumeroDeTarjeta.Focus();
        }

        private void btnNumero_Click(object sender, EventArgs e)
        {
            if (textBoxActivo == null) return;

            Button btn = (Button)sender;
            textBoxActivo.Text += btn.Text;
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (textBoxActivo == null) return;

            if (textBoxActivo.Text.Length > 0)
                textBoxActivo.Text = textBoxActivo.Text.Substring(0, textBoxActivo.Text.Length - 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            limpiarCampos();
        }

        private void txtNumeroDeTarjeta_Enter(object sender, EventArgs e) => textBoxActivo = (TextBox)sender;
        private void txtPIN_Enter(object sender, EventArgs e) => textBoxActivo = (TextBox)sender;
        private void txtCodigoVerficacion_Enter(object sender, EventArgs e) => textBoxActivo = (TextBox)sender;
        private void txtMontoRetiro_Enter(object sender, EventArgs e) => textBoxActivo = (TextBox)sender;
        private void txtIngresarCodigo_Enter(object sender, EventArgs e) => textBoxActivo = (TextBox)sender;

        private void PantallaRetiros_FormClosed(object sender, FormClosedEventArgs e)
        {
            conexion?.Cerrar();
        }
    }
}