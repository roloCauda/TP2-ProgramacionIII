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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace winformApp
{
    public partial class frmAltaArticulo : Form
    {
        private Articulo articulo = null;
        private List<Imagen> ListaImagenes = null;
        private List<string> ListaStringImagenes = new List<string>();
        int IndiceImagen = -1;

        public frmAltaArticulo()
        {
            InitializeComponent();
            

        Text = "Agregar Artículo";

        }
        public frmAltaArticulo(Articulo articulo, int IdArt)
        {
            InitializeComponent();
            ImagenNegocio negocio = new ImagenNegocio();

            this.articulo = articulo;
            ListaImagenes = negocio.listar(IdArt);

            CargarListaEnTextBox(ListaImagenes);

            Text = "Modificar Artículo";
        }

        private void btnAgregarImagenURL_Click(object sender, EventArgs e)
        {
            if(txtURLImagen.Text != null)
            {
                txtListaImagenes.AppendText(txtURLImagen.Text + Environment.NewLine);
                ListaStringImagenes.Add(txtURLImagen.Text);
                
                if (IndiceImagen == -1)
                { 
                    IndiceImagen = 0;
                }
                else
                {
                    IndiceImagen++;
                }
                cargarImagen(ListaStringImagenes[IndiceImagen]);
                txtURLImagen.Text = "";
            }
        }

        private void CargarListaEnTextBox(List<Imagen> lista)
        {
            txtListaImagenes.Text = String.Join(Environment.NewLine, lista.Select(img => img.ImagenURL));
        }

        private void btnAltaAnterior_Click(object sender, EventArgs e)
        {
            if (IndiceImagen > 0)
            {
                IndiceImagen--;
                cargarImagen(ListaStringImagenes[IndiceImagen]);
            }
        }

        private void btnAltaPosterior_Click(object sender, EventArgs e)
        {
            if(IndiceImagen < ListaStringImagenes.Count - 1)
            {
                IndiceImagen++;
                cargarImagen(ListaStringImagenes[IndiceImagen]);
            }
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
                    
                    txtPrecio.Text = articulo.Precio.ToString();
                   
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
           
            ArticuloNegocio negocio = new ArticuloNegocio(); //para conectar a la BD
            ImagenNegocio negocioIMG = new ImagenNegocio(); //para conectar a la BD

            try
            {
                if(articulo == null) //si crea
                {
                    articulo = new Articulo();
                    Marca nuevaMarca = new Marca();
                    Categoria nuevaCategoria = new Categoria();
                }
                articulo.Codigo = txtCodigo.Text; //si fuese de un tipo distinto a string, deberia castearlo, ej. int.Parse
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.IdMarca = (Marca)cboMarca.SelectedItem; //trae el item seleccionado, pero hay que decirle de que tipo es
                articulo.IdCategoria = (Categoria)cboCategoria.SelectedItem;
                articulo.Precio = decimal.Parse(txtPrecio.Text);

                if(articulo.IdArticulo != 0) //si modifica
                {
                    //nuevoIMG.IdImagen = 0; si se modifica la imagen tengo que pasarle el ID de la imagen
                    //articulo.IdArticulo =
                    //nuevoIMG.ImagenURL = txtURLImagen.Text;
                    negocio.modificar(articulo);
                    negocioIMG.modificar(ListaStringImagenes, articulo);
                   //MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    int nuevoId = negocio.agregar(articulo);

                    negocioIMG.agregar(ListaStringImagenes, nuevoId);
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

    }
}
