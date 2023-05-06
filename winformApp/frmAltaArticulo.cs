using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
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
        
        private Articulo articulo = null;

        public frmAltaArticulo()
        {
            InitializeComponent();

            Text = "Agregar Artículo";

        }
        public frmAltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;

            Text = "Modificar Artículo";
        }
    
        private void frmAltaArticulo_Load_1(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            try
            {
                //cargo los desplegables
                cboMarca.DataSource = marcaNegocio.listar();
                cboMarca.ValueMember = "IdMarca";
                cboMarca.DisplayMember = "Descripcion";
                cboCategoria.DataSource = categoriaNegocio.listar();
                cboCategoria.ValueMember = "IdCategoria";
                cboCategoria.DisplayMember = "Descripcion";

                if (articulo != null){
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtURLImagen.Text = articulo.ImagenURL.ToString();
                    txtPrecio.Text = articulo.Precio.ToString();
                    cargarImagen(articulo.ImagenURL.ToString());
                    cboMarca.SelectedValue = articulo.IdMarca.IdMarca;
                    cboCategoria.SelectedValue = articulo.IdCategoria.IdCategoria;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Imagen nuevoIMG = new Imagen(); //creo e instancio a la Imagen
            ArticuloNegocio negocio = new ArticuloNegocio(); //para conectar a la BD
            ImagenNegocio negocioIMG = new ImagenNegocio(); //para conectar a la BD

            try
            {
                if(articulo == null)
                {
                    articulo = new Articulo();
                }
                articulo.Codigo = txtCodigo.Text; //si fuese de un tipo distinto a string, deberia castearlo, ej. int.Parse
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.IdMarca = (Marca)cboMarca.SelectedItem; //trae el item seleccionado, pero hay que decirle de que tipo es
                articulo.IdCategoria = (Categoria)cboCategoria.SelectedItem;
                articulo.Precio = decimal.Parse(txtPrecio.Text);

                nuevoIMG.IdImagen = 0;
                nuevoIMG.ImagenURL = txtURLImagen.Text;

                if(articulo.IdArticulo != 0)
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    negocio.agregar(articulo);
                    negocioIMG.agregar(nuevoIMG);
                    MessageBox.Show("Agregado exitosamente");
                }

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
