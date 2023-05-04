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
        
        public List<Imagen> listar()
        {
            List<Imagen> lista = new List<Imagen>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("select Id, ImagenURL from IMAGENES");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Imagen aux = new Imagen();
                    aux.IdImagen = (int)datos.Lector["Id"];
                    aux.ImagenURL = (string)datos.Lector["ImagenURL"];

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
                datos.setearConsulta("Insert into IMAGENES (IdArticulo,ImagenURL) values ('" + nuevoIMG.ImagenURL + "')");

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
