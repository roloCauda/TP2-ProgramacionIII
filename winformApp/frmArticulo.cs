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
        int IndiceImagen;

        public frmArticulo()
        {
            InitializeComponent();
        }

        private void frmArticulo_Load(object sender, EventArgs e) //evento
        {
            
            
            try
            {
                cargar();
                //le agrego con ADD las opciones que quiero que aparezcan
                cboCampo.Items.Add("Codigo");
                cboCampo.Items.Add("Nombre");
                cboCampo.Items.Add("Descripcion");
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

            cargar();
        }

        private void cargarImagen(List<Imagen> listaImagenes, int indiceImagen)
        {
            if (listaImagenes.Count > 0)
            {
                if (indiceImagen < 0)
                {
                    indiceImagen = listaImagenes.Count - 1;
                }
                else if (indiceImagen >= listaImagenes.Count)
                {
                    indiceImagen = 0;
                }

                string imagenURL = listaImagenes[indiceImagen].ImagenURL;
                pbxArticulo.Load(imagenURL);
            }
            else
            {
                pbxArticulo.Image = null;
            }
        }

        public void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                ListaArticulos = negocio.listar();
                dgvArticulo.DataSource = ListaArticulos;
                ocultarColumnas();
                /*rgarImagen(ListaArticulos[0].ListaImagenes, 0);*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            dgvArticulo.Columns["IdArticulo"].Visible = false;
           // dgvArticulo.Columns["ImagenURL"].Visible = false;
        }

        private void dgvArticulo_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulo.CurrentRow != null)
            {
                IndiceImagen = 0;

                ImagenNegocio negocio = new ImagenNegocio();
                
                Articulo seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
                int IdArt = seleccionado.IdArticulo;
                List<Imagen> listaImagenes = negocio.listar(IdArt);

                try
                {
                    pbxArticulo.ImageLocation = listaImagenes[0].ImagenURL;
                }
                catch (Exception)
                {

                    pbxArticulo.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
                }
                
                
            }
        }
        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (dgvArticulo.CurrentRow != null)
            {
                ImagenNegocio negocio = new ImagenNegocio();

                Articulo seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
                int IdArt = seleccionado.IdArticulo;
                int indiceMin = negocio.listar(IdArt).Count + 1;

                if (IndiceImagen != indiceMin && IndiceImagen > 0)
                {
                    IndiceImagen--;
                    /*cargarImagen()*/
                    pbxArticulo.ImageLocation = negocio.listar(IdArt)[IndiceImagen].ImagenURL;
                }
            }
        }

        private void btnPosterior_Click(object sender, EventArgs e)
        {
            if (dgvArticulo.CurrentRow != null)
            {
                ImagenNegocio negocio = new ImagenNegocio();

                Articulo seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
                int IdArt = seleccionado.IdArticulo;
                int indiceMax = negocio.listar(IdArt).Count - 1;

                if(IndiceImagen != indiceMax && IndiceImagen >= 0)
                {
                    IndiceImagen++;
                    /*cargarImagen()*/
                    pbxArticulo.ImageLocation = negocio.listar(IdArt)[IndiceImagen].ImagenURL;
                } 
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e) //filtro rapido busca en la grilla que ya esta cargada
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro.Length > 0)
            {
                //esta expresion lamda, actua como un for each, en cada vuelta guarda un objeto en x y lo evalua segun el filtro dado
                //toUpper es para que compare todo por igual
                //Contains(metodo de las cadenas) es para que busque que contenga el filtro
                listaFiltrada = ListaArticulos.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper())); //filtra por nombre
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

        private void btnFiltro_Click(object sender, EventArgs e) //este filtro va a la BD y busca con el select
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            
            try
            {
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();  
                string filtro = txtFiltroAV.Text;
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

        private void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            txtFiltroAV.Text = "";
            
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("Estás seguro?", "Eliminar Artículo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado.IdArticulo);
                    cargar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

    }
}