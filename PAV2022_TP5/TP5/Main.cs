using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TP5
{
    public partial class Main : Form
    {
        PersistenciaDatos datos;
        Producto producto;
        bool productoNuevo;

        public Main()
        {
            InitializeComponent();
            producto = new Producto("", "", 0, 0, 0);
            datos = new PersistenciaDatos();
            ActualizarGrilla(datos.ListaProductos);
        }
        #region Eventos TXTCODIGO
        //CREAR VALIDACIONES REGEX EN UNA LIBRERIA PARA IMPORTAR y ver lambda expressions
        //Regex regc = new Regex(@"(\d+){1}", RegexOptions.Singleline);
        //bool queOnda = regc.IsMatch(txtCodigo.Text);

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCodigo.Text))
            {
                if (datos.BuscarProducto(txtCodigo.Text) != null)
                {
                    //aqui se pasa por referencia el objeto
                    bindingSource1.DataSource = datos.BuscarProducto(txtCodigo.Text);
                    //a los valores de los campos los almaceno en un nuevo objeto
                    //vuelvo a asignar al binding asi no sea por referencia. Para que no se cambien las cosas si no confirmo
                    bindingSource1.DataSource = ExtraerDatosDeCampos(true);
                    productoNuevo = false;
                }
                else
                {
                    bindingSource1.DataSource = new Producto("", "", 0, 0, 0);
                    productoNuevo = true;
                    LimpiarCampos();
                }
            }
        }

        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            txtCodigo.BackColor = Color.LightGray;
            if (!string.IsNullOrEmpty(txtCodigo.Text))
            {
                if (productoNuevo)
                {
                    if (MessageBox.Show("Código no encontrado, desea crear un nuevo producto?",
                        "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        HabilitarBotonesOperaciones(true);
                        VisibilidadCamposNuevoProducto(false);
                    }
                    else
                    {
                        txtCodigo.Text = "";
                        txtCodigo.Focus();
                    }
                }
                else
                {
                    HabilitarBotonesOperaciones(true);
                    VisibilidadCamposNuevoProducto(false);
                }
            }
        }
        #endregion
        #region Eventos Botones

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (productoNuevo) AgregarProducto();
            else ModificarProducto();
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            HabilitarBotonesOperaciones(false);
            VisibilidadCamposNuevoProducto(true);
            CodigoFocus();

        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            EliminarProducto(producto.Codigo);
        }
        #endregion

        private Producto ExtraerDatosDeCampos(bool EsNuevoProducto = false)
        {
            string cod= txtCodigo.Text;
            string desc= txtDescripcion.Text;
            string est= txtEstado.Text;
            double costo= double.Parse(txtCosto.Text);
            double pFinal= double.Parse(txtPrecioFinal.Text);
            double mGan = double.Parse(txtMarganGanancia.Text);
            double cIva = double.Parse(txtCostoConIVA.Text);
            double pIva = double.Parse(txtPorcentajeIVA.Text);
            if (EsNuevoProducto) producto = new Producto(cod,desc,pIva,costo,mGan);
            else{
                producto.Codigo = cod; producto.Descripcion = desc;
                producto.PorcentajeIVA = pIva; producto.CostoSinIva = costo;
                producto.MargenGanancia = mGan;
                }
            return producto;
        }
        private void EliminarProducto(string cod)
        {
            datos.EliminarProducto(cod);
            LimpiezaFormulario();
        }
        private void ActualizarGrilla(List<Producto> lista)
        {
            grillaProdructos.DataSource = null;
            grillaProdructos.DataSource = lista;
        }
        private void AgregarProducto()
        {
            if (productoNuevo)
            {
                datos.AgregarProducto(ExtraerDatosDeCampos(true));
                LimpiezaFormulario();
            }
        }
        private void ModificarProducto()
        {
            datos.ModificarProducto(producto.Codigo, ExtraerDatosDeCampos(false));
            LimpiezaFormulario();
        }
        private void LimpiezaFormulario()
        {
            ActualizarGrilla(datos.ListaProductos);
            LimpiarCampos();
            HabilitarBotonesOperaciones(false);
            VisibilidadCamposNuevoProducto(true);
            CodigoFocus();
        }
        private void CodigoFocus()
        {            
            txtCodigo.Text = "";
            txtCodigo.Focus();
            btnGuardar.Text = "Guardar";
        }
        private void LimpiarCampos()
        {
            txtCosto.Text = "";
            txtDescripcion.Text = "";
            txtEstado.Text = "";
            txtPrecioFinal.Text = "";
            txtMarganGanancia.Text = "";
            txtCostoConIVA.Text = "";
            txtPorcentajeIVA.Text = "";
        }
        private void HabilitarBotonesOperaciones(bool visible)
        {
            btnGuardar.Enabled = visible;
            btnEliminar.Enabled = visible;
            if (productoNuevo) btnGuardar.Text = "Guardar";
            else btnGuardar.Text = "Modificar";
        }
        private void VisibilidadCamposNuevoProducto(bool visibilidad)
        {
            txtCodigo.Enabled = visibilidad;
            txtEstado.Enabled = visibilidad;
            txtCostoConIVA.Enabled = visibilidad;
        }
        private void txtCodigo_Enter(object sender, EventArgs e)
        {
            txtCodigo.BackColor = Color.LightGreen;
        }
    }
}
