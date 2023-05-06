using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winformApp
{
    public partial class frmArticulo : Form
    {
        private List<Articulo> ListaArticulos;

        public frmArticulo()
        {
            InitializeComponent();
        }

        private void frmArticulo_Load(object sender, EventArgs e) //evento
        {
            try
            {
                cargar();
                cboCampo.Items.Add("Codigo");
                cboCampo.Items.Add("Nombre");
                cboCampo.Items.Add("Descripcion");
                //cboCampo.Items.Add("Marca");
                //cboCampo.Items.Add("Categoria");
                cboCampo.Items.Add("Precio");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulo alta = new frmAltaArticulo();
            alta.ShowDialog();

            //no me esta cargando completa la lista con el nuevo articulo
            cargar();
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxArticulo.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }

        public void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                ListaArticulos = negocio.listar(); //le cargo la lista que me trae de la BD de Articulos
                dgvArticulo.DataSource = ListaArticulos; //le paso la lista a la grilla
                ocultarColumnas();
                cargarImagen(ListaArticulos[0].ImagenURL.ImagenURL);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            dgvArticulo.Columns["IdArticulo"].Visible = false;
            dgvArticulo.Columns["ImagenURL"].Visible = false;
        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulo.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenURL.ImagenURL);
            }
        }

        

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro.Length > 0)
            {
                //esta expresion lamda, actua como un for each, en cada vuelta guarda un objeto en x y lo evalua segun el filtro dado
                //toUpper es para que compare todo por igual
                //Contains(metodo de las cadenas) es para que busque que contenga el filtro
                listaFiltrada = ListaArticulos.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = ListaArticulos;
            }

            //limpio la grilla
            dgvArticulo.DataSource = null;
            //le cargo mi nueva lista
            dgvArticulo.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            if (opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            
            try
            {
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dgvArticulo.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
            frmAltaArticulo modificar = new frmAltaArticulo(seleccionado);
            modificar.ShowDialog();
        }

    }
}