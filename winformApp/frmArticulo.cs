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
        private List<string> ListaStringImagenes = new List<string>();
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

        

        public void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                ListaArticulos = negocio.listar();
                dgvArticulo.DataSource = ListaArticulos;
                ocultarColumnas();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            dgvArticulo.Columns["IdArticulo"].Visible = false;
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
                    cargarImagen(listaImagenes[0].ImagenURL);
                }
                catch (Exception)
                {

                    pbxArticulo.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
                }
                
                
            }
        }
        private void btnAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvArticulo.CurrentRow != null)
                {
                    ImagenNegocio negocio = new ImagenNegocio();

                    Articulo seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
                    int IdArt = seleccionado.IdArticulo;

                    if (IndiceImagen >0)
                    {
                        IndiceImagen--;
                        cargarImagen(negocio.listar(IdArt)[IndiceImagen].ImagenURL);
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnPosterior_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvArticulo.CurrentRow != null)
                {
                    ImagenNegocio negocio = new ImagenNegocio();

                    Articulo seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
                    int IdArt = seleccionado.IdArticulo;

                    if (IndiceImagen < negocio.listar(IdArt).Count - 1)
                    {
                        IndiceImagen++;
                        cargarImagen(negocio.listar(IdArt)[IndiceImagen].ImagenURL);
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
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
                if (validarFiltro())
                {
                    return;
                }

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
            int IdArt;
            seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
            IdArt = seleccionado.IdArticulo;
            frmAltaArticulo modificar = new frmAltaArticulo(seleccionado, IdArt);
            modificar.ShowDialog();
            cargar();
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

        private bool validarFiltro()
        {
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione el campo para filtrar");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione el criterio para filtrar");
                return true;
            }
            if (cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtFiltroAV.Text))
                {
                    MessageBox.Show("Debes cargar el filtro para campo numérico");
                    return true;
                }

                if (!(soloNumeros(txtFiltroAV.Text)))
                {
                    MessageBox.Show("Solo números para el campo Filtro");
                    return true;
                }
            }
            return false;
        }

        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                { return false; }
            }
            return true;
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

    }
}