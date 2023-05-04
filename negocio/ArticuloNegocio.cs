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
                datos.setearConsulta("Select A.Id, Codigo, Nombre, A.Descripcion, M.Descripcion Marca, C.Descripcion Categoria, Precio, I.ImagenUrl From ARTICULOS A, CATEGORIAS C, MARCAS M, IMAGENES I Where C.Id = A.IdCategoria And M.Id = A.IdMarca and I.IdArticulo = A.Id");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();

                    aux.IdArticulo = (int)datos.Lector["Id"];

                    //opcion 1 - validar que no sea NULL
                    //GetOrdinal es para decirle que columna ver
                    //niego si es nulo (busco que no sea nulo)
                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("Codigo"))))
                        aux.Codigo = (string)datos.Lector["Codigo"];

                    //opcion 2 - validar que no sea NULL
                    if (!(datos.Lector["Nombre"] is DBNull))
                        aux.Nombre = (string)datos.Lector["Nombre"];

                    if (!(datos.Lector["Descripcion"] is DBNull))
                        aux.Descripcion = (string)datos.Lector["Descripcion"];

                    aux.IdMarca = new Marca();
                    if (!(datos.Lector["Marca"] is DBNull))
                        aux.IdMarca.Descripcion = (string)datos.Lector["Marca"];

                    aux.IdCategoria = new Categoria();
                    if (!(datos.Lector["Categoria"] is DBNull))
                        aux.IdCategoria.Descripcion = (string)datos.Lector["Categoria"];

                    aux.ImagenURL = new Imagen();
                    //validar que no sea null
                    aux.ImagenURL.ImagenURL = (string)datos.Lector["ImagenUrl"];

                    if (!(datos.Lector["Precio"] is DBNull))
                        aux.Precio = (decimal)datos.Lector["Precio"];

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
                //las comillas dobles definen las cadenas en c#, las comillas simples definen las canedas en SQLServer
                datos.setearConsulta("Insert into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) values ('" + nuevo.Codigo + "', '" + nuevo.Nombre + "', '" + nuevo.Descripcion + "', @IdMarca, @IdCategoria, '" + nuevo.Precio + "')");
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