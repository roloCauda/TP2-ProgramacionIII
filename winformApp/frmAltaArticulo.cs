using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace winformApp
{
    public partial class frmAltaArticulo : Form
    {
        public frmAltaArticulo()
        {
            InitializeComponent();
        }
    
        private void frmAltaArticulo_Load_1(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            try
            {
                //cargo los desplegables
                cboMarca.DataSource = marcaNegocio.listar();
                cboCategoria.DataSource = categoriaNegocio.listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Articulo nuevo = new Articulo(); //creo e instancio al Articulo
            ArticuloNegocio negocio = new ArticuloNegocio(); //para conectar a la BD

            try
            {
                nuevo.Codigo = txtCodigo.Text; //si fuese de un tipo distinto a string, deberia castearlo, ej. int.Parse
                nuevo.Nombre = txtNombre.Text;
                nuevo.Descripcion = txtDescripcion.Text;
                nuevo.IdMarca = (Marca)cboMarca.SelectedItem; //trae el item seleccionado, pero hay que decirle de que tipo es
                nuevo.IdCategoria = (Categoria)cboCategoria.SelectedItem;

                //imagen
                nuevo.Precio = decimal.Parse(txtPrecio.Text);

                negocio.agregar(nuevo);
                MessageBox.Show("Agregado exitosamente");
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
