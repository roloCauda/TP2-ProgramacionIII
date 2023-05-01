using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Data.SqlClient;
using dominio;

namespace negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("select Id, Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio from ARTICULOS");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.IdArticulo = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Precio = (decimal)datos.Lector["Precio"];

                    aux.IdMarca = new Marca();
                    aux.IdMarca.IdMarca = (int)datos.Lector["IdMarca"];

                    aux.IdCategoria = new Categoria();
                    aux.IdCategoria.IdCategoria = (int)datos.Lector["IdCategoria"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void agregar(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("Insert into ARTICULO (Id, Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) values (" + nuevo.IdArticulo + ", '" + nuevo.Codigo + "', '" + nuevo.Nombre + "', '" + nuevo.Descripcion + "', 1, @IdMarca, @IdCategoria, '" + nuevo.Precio + "')");
                datos.setearParametro("@IdMarca", nuevo.IdMarca.IdMarca);
                datos.setearParametro("@IdCategoria", nuevo.IdCategoria.IdCategoria);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        /*public void modificar(Articulo art)
        {
        
        }*/

        /*public List<Articulo> filtrar()
        {


        }*/

        /*public void eliminar(int id)
        {

        }*/

        /*public void eliminarLogico(int id)
        {

        }*/
    }
}
