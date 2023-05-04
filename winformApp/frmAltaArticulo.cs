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
            Articulo nuevoArt = new Articulo(); //creo e instancio al Articulo
            Imagen nuevoIMG = new Imagen(); //creo e instancio a la Imagen
            ArticuloNegocio negocio = new ArticuloNegocio(); //para conectar a la BD
            ImagenNegocio negocioIMG = new ImagenNegocio(); //para conectar a la BD

            try
            {
                nuevoArt.Codigo = txtCodigo.Text; //si fuese de un tipo distinto a string, deberia castearlo, ej. int.Parse
                nuevoArt.Nombre = txtNombre.Text;
                nuevoArt.Descripcion = txtDescripcion.Text;
                nuevoArt.IdMarca = (Marca)cboMarca.SelectedItem; //trae el item seleccionado, pero hay que decirle de que tipo es
                nuevoArt.IdCategoria = (Categoria)cboCategoria.SelectedItem;

                nuevoIMG.ImagenURL = txtURLImagen.Text;

                nuevoArt.Precio = decimal.Parse(txtPrecio.Text);

                negocio.agregar(nuevoArt);
                negocioIMG.agregar(nuevoIMG);
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

        

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxArticuloAlta.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxArticuloAlta.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }

        private void txtURLImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtURLImagen.Text);
        }
    }
}
