using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class ImagenNegocio
    {

        public List<Imagen> listar(int idArticulo)
        {
            List<Imagen> lista = new List<Imagen>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("select Id, IdArticulo, ImagenUrl from Imagenes where idArticulo = " + idArticulo);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Imagen aux = new Imagen();
                    int v = (int)datos.Lector["Id"];
                    aux.IdImagen = v;
                    aux.IdArticulo = (int)datos.Lector["IdArticulo"];
                    aux.ImagenURL = (string)datos.Lector["ImagenUrl"];

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

        public void agregar(Imagen nuevoIMG)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                //las comillas dobles definen las cadenas en c#, las comillas simples definen las canedas en SQLServer
                //ver como pasarle el ID de articulo, es Identity
                datos.setearConsulta("Insert into IMAGENES (IdArticulo, ImagenURL) values (@IdArticulo, @ImagenURL)");
                datos.setearParametro("@IdArticulo", nuevoIMG.IdArticulo);
                datos.setearParametro("@ImagenURL", nuevoIMG.ImagenURL);

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
    }
}
