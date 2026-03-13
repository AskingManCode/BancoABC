using System;
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
        private string codigoAutorizacionGuardado = "";

        public PantallaRetiros(string codigo)
        {
            InitializeComponent();
            this.CodigoCajero = codigo;
        }

        CifradoDeDatos EDD = new CifradoDeDatos();

        private TextBox textBoxActivo;

        private void button1_Click(object sender, EventArgs e)
        {
            if (!conexion.EstaConectado())
            {
                MessageBox.Show(
                    "No hay conexión con el Autorizador. Intente más tarde.",
                    "Sin conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (!panel2.Visible)
            {
                ProcesarRetiro();
            }
            else
            {
                ValidarCodigoDeAutorizacion();
            }
        }

        public void ProcesarRetiro()
        {
            if (ValidarCamposVacios() == false)
            {
                return;
            }

            string FechaConFormato = Convert.ToDateTime("01/" + dtpVencimiento.Text).ToString("yyyy-MM-dd");

            var json = JsonSerializer.Serialize(new
            {
                NumeroDeTarjeta = EDD.Cifrar(txtNumeroDeTarjeta.Text),
                Pin = EDD.Cifrar(txtPIN.Text),
                FechaDeVencimiento = EDD.Cifrar(FechaConFormato),
                CodigoDeVerificacion = EDD.Cifrar(txtCodigoVerficacion.Text),
                IdentificadorDelCajero = CodigoCajero,
                TipoDeTransaccion = "Retiro",
                MontoRetiro = txtMontoRetiro.Text
            });

            conexion.Enviar(json);
            string jsonRespuesta = conexion.Recibir();

            JsonDocument respuesta = JsonDocument.Parse(jsonRespuesta);
            string status = respuesta.RootElement.GetProperty("status").GetString();


            if (status == "OK")
            {
                MostrarPanelParaCodigoDeSeguridad();
                codigoAutorizacionGuardado = respuesta.RootElement.GetProperty("Autorizacion").GetString();
                lblCodigoEnRojo.Text = codigoAutorizacionGuardado;
            }

            if (status == "1")
            {
                MessageBox.Show("Fondos insuficientes", "No se puede realizar la acción", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (status == "2")
            {
                MessageBox.Show("Datos incorrectos", "No se puede realizar la acción", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (status == "3")
            {
                MessageBox.Show("Tarjeta innactiva", "No se puede realizar la acción", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (status == "4")
            {
                MessageBox.Show("Tarjeta vencida", "No se puede realizar la acción", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (status == "5")
            {
                MessageBox.Show("Error no controlado", "No se puede realizar la acción", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        public void ValidarCodigoDeAutorizacion()
        {
            if (!ValidarCamposVacios())
            {
                return;
            }

            if (txtIngresarCodigo.Text != codigoAutorizacionGuardado)
            {
                MessageBox.Show(
                    "El código de autorización ingresado no coincide.",
                    "Código incorrecto",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            string FormatoDeFecha = Convert.ToDateTime("01/" + dtpVencimiento.Text).ToString("yyyy-MM-dd");

            MessageBox.Show(
                    "El código de autorización ingresado no coincide.",
                    FormatoDeFecha.ToString(),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
            ); 

            var json = JsonSerializer.Serialize(new
            {
                CodigoDeAutorizacion = txtIngresarCodigo.Text,
                NumeroDeTarjeta = EDD.Cifrar(txtNumeroDeTarjeta.Text),
                FechaDeVencimiento = EDD.Cifrar(FormatoDeFecha),
                CodigoDeVerificacion = EDD.Cifrar(txtCodigoVerficacion.Text),
                IdentificadorDelCajero = CodigoCajero,
                TipoDeTransaccion = "Confirmacion",
                MontoRetiro = txtMontoRetiro.Text
            });

            conexion.Enviar(json);
            string jsonRespuesta = conexion.Recibir();

            JsonDocument respuesta = JsonDocument.Parse(jsonRespuesta);
            string status = respuesta.RootElement.GetProperty("status").GetString();


            if (status == "OK")
            {
                MessageBox.Show("Retiro realizado correctamente","Accion realizada con exito",MessageBoxButtons.OK,MessageBoxIcon.Information);
                limpiarCampos();
            }

            if (status == "1")
            {
                MessageBox.Show("Fondos insuficientes", "No se puede realizar la acción", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (status == "2")
            {
                MessageBox.Show("Datos incorrectos", "No se puede realizar la acción", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (status == "3")
            {
                MessageBox.Show("Tarjeta Inactiva", "No se puede realizar la acción", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (status == "4")
            {
                MessageBox.Show("Tarjeta Vencida", "No se puede realizar la acción", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (status == "5")
            {
                MessageBox.Show("Error no controlado", "No se puede realizar la acción", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void txtNumeroDeTarjeta_Enter(object sender, EventArgs e)
        {
            textBoxActivo = (TextBox)sender;
        }

        private void txtPIN_Enter(object sender, EventArgs e)
        {
            textBoxActivo = (TextBox)sender;
        }

        private void txtCodigoVerficacion_Enter(object sender, EventArgs e)
        {
            textBoxActivo = (TextBox)sender;
        }

        private void txtMontoRetiro_Enter(object sender, EventArgs e)
        {
            textBoxActivo = (TextBox)sender;
        }

        private void btnNumero_Click(object sender, EventArgs e)
        {
            if (textBoxActivo == null) return;

            Button boton = (Button)sender;
            textBoxActivo.Text += boton.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            limpiarCampos();
        }

        private void limpiarCampos() {

            txtCodigoVerficacion.Text = "";
            txtMontoRetiro.Text = "";
            txtNumeroDeTarjeta.Text = "";
            txtPIN.Text = "";
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (textBoxActivo == null) return;

            if (textBoxActivo.Text.Length == 0) return;

            textBoxActivo.Text = textBoxActivo.Text.Substring(0, textBoxActivo.Text.Length - 1);
        }

        private void txtIngresarCodigo_Enter(object sender, EventArgs e)
        {
            textBoxActivo = (TextBox)sender;
        }

        public void MostrarPanelParaCodigoDeSeguridad()
        {
            panel2.Visible = true;
            lblInstruccionesCodigo.Visible = true;
            lblCodigoDeSeguridad.Visible = true;
            lblCodigoEnRojo.Visible = true;
            txtIngresarCodigo.Visible = true;
            btnVolver.Visible = true;
        }

        public void EsconderPanelParaCodigoDeSeguridad()
        {
            panel2.Visible = false;
            lblInstruccionesCodigo.Visible = false;
            lblCodigoDeSeguridad.Visible = false;
            lblCodigoEnRojo.Visible = false;
            txtIngresarCodigo.Visible = false;
            btnVolver.Visible = false;
        }

        private void PantallaRetiros_Load(object sender, EventArgs e)
        {
            conexion = new ConexionTCP("127.0.0.1", 5000);

            if (!conexion.Conectar())
            {
                MessageBox.Show(
                    "No se pudo conectar con el Autorizador",
                    "Error de conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void PantallaRetiros_FormClosed(object sender, FormClosedEventArgs e)
        {
            conexion.Cerrar();
        }

        public bool ValidarCamposVacios()
        {
            if (!panel2.Visible)
            {
                if (string.IsNullOrWhiteSpace(txtNumeroDeTarjeta.Text) ||
                    string.IsNullOrWhiteSpace(txtPIN.Text) ||
                    string.IsNullOrWhiteSpace(txtCodigoVerficacion.Text) ||
                    string.IsNullOrWhiteSpace(txtMontoRetiro.Text) ||
                    string.IsNullOrWhiteSpace(dtpVencimiento.Text))
                {
                    MessageBox.Show(
                        "Debe completar todos los campos para solicitar el retiro",
                        "Campos incompletos",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return false;
                }
            }
            
            else
            {
                if (string.IsNullOrWhiteSpace(txtNumeroDeTarjeta.Text) ||
                    string.IsNullOrWhiteSpace(txtPIN.Text) ||
                    string.IsNullOrWhiteSpace(txtCodigoVerficacion.Text) ||
                    string.IsNullOrWhiteSpace(txtMontoRetiro.Text) ||
                    string.IsNullOrWhiteSpace(dtpVencimiento.Text) ||
                    string.IsNullOrWhiteSpace(txtIngresarCodigo.Text))
                {
                    MessageBox.Show(
                        "Debe completar todos los campos, incluido el código de autorización",
                        "Confirmación incompleta",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return false;
                }
            }

            return true;
        }
    }
}
