using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;

using System.Linq;
using System.Text;
using System.Threading;
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
        private int IndiceImagen = -1;
        private int IndiceImagenBorrar = -1;
        private List<string> ListaStringImagenesBorrar = new List<string>();
        private OpenFileDialog archivo = null;

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

            ListaStringImagenes = extraerStringsListaImagenes(ListaImagenes);

            IndiceImagenBorrar = ListaStringImagenesBorrar.Count() - 1;
            IndiceImagen = ListaStringImagenes.Count() - 1;

            if (IndiceImagen > -1)
            {
                cargarImagen(ListaStringImagenes[0]);
            }
            else
            {
                pbxArticuloAlta.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }

            Text = "Modificar Artículo";
        }

        public List<string> extraerStringsListaImagenes(List<Imagen> listaImagenes)
        {
            List<string> lista = new List<string>();

            for (int x = 0; x < listaImagenes.Count; x++)
            {
                string aux;

                aux = listaImagenes[x].ImagenURL;

                lista.Add(aux);
            }

            return lista;
        }

        private void btnAgregarImagenURL_Click(object sender, EventArgs e)
        {
            if(txtURLImagen.Text != null)
            {
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

        private void btnArhivo_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "png|*.png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtURLImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);
            }
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

                articulo.Descripcion = txtDescripcion.Text;
                articulo.IdMarca = (Marca)cboMarca.SelectedItem; //trae el item seleccionado, pero hay que decirle de que tipo es
                articulo.IdCategoria = (Categoria)cboCategoria.SelectedItem;
                if (validarNulidad(txtCodigo.Text) && validarNulidad(txtNombre.Text) && soloNumeros(txtPrecio.Text) && validarNulidad(txtPrecio.Text))
                {
                    articulo.Precio = decimal.Parse(txtPrecio.Text);
                    articulo.Nombre = txtNombre.Text;
                    articulo.Codigo = txtCodigo.Text;
                }
                else
                {
                    MessageBox.Show("Completar campos obligatorios: Código, Nombre y Precio");
                    return;
                }

                if (articulo.IdArticulo != 0) //si modifica
                {
                    negocio.modificar(articulo);
                    negocioIMG.modificar(ListaStringImagenes, ListaStringImagenesBorrar, articulo.IdArticulo);
                    MessageBox.Show("Modificado exitosamente");
                    ListaStringImagenes.Clear();
                }
                else
                {
                    int nuevoId = negocio.agregar(articulo);

                    negocioIMG.agregar(ListaStringImagenes, nuevoId);
                    MessageBox.Show("Agregado exitosamente");
                    ListaStringImagenes.Clear();
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
            ListaStringImagenes.Clear();
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

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ListaStringImagenes.Count()>0)
                {
                    ListaStringImagenesBorrar.Add(ListaStringImagenes[IndiceImagen]);

                    ListaStringImagenes.Remove(ListaStringImagenes[IndiceImagen]);

                    IndiceImagen--;

                    if (IndiceImagen > -1)
                    {
                        cargarImagen(ListaStringImagenes[0]);
                    }
                    else
                    {
                        pbxArticuloAlta.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool soloNumeros(string texto)
        {
            foreach (char caracter in texto)
            {
                if (!(char.IsNumber(caracter) || caracter == ','))
                {
                    MessageBox.Show("Por favor ingresar un número válido con coma");
                    return false;
                }
            }
            return true;
        }

        private bool validarNulidad(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                MessageBox.Show("No ingresar campos nulos");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void txtURLImagen_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblPrecio_Click(object sender, EventArgs e)
        {

        }

        private void lblImagen_Click(object sender, EventArgs e)
        {

        }

        private void cboCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblCategoria_Click(object sender, EventArgs e)
        {

        }

        private void lblCodArt_Click(object sender, EventArgs e)
        {

        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboMarca_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblMarca_Click(object sender, EventArgs e)
        {

        }

        private void lblNombre_Click(object sender, EventArgs e)
        {

        }

        private void txtDescripcion_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPrecio_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblDescripcion_Click(object sender, EventArgs e)
        {

        }
    }
}
