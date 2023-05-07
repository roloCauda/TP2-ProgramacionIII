using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

                    aux.IdImagen = (int)datos.Lector["Id"];
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

        public void agregar(List<string> lista , int ID)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                int tamLista = lista.Count;

                for(int x=0; x<tamLista; x++)
                {
                    datos.setearConsulta("Insert into IMAGENES (IdArticulo, ImagenURL) values (@IdArticulo, @ImagenURL)");
                    datos.limpiarParametros(datos);
                    datos.setearParametro("@IdArticulo", ID);
                    datos.setearParametro("@ImagenURL", lista[x]);

                    datos.ejecutarAccion(); 
                }

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

        public void modificar(List<string> lista, int iDArticulo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                int tamLista = lista.Count;

                for (int x = 0; x < tamLista; x++)
                {
                    datos.setearConsulta("select count (*) from IMAGENES where ImagenUrl=@imagenURL and IdArticulo=@idArticulo");
                    datos.limpiarParametros(datos);
                    datos.setearParametro("@imagenURL", lista[x]);
                    datos.setearParametro("@idArticulo", iDArticulo);
                    int cantidad = (int)datos.ejecutarEscalar();

                    if (cantidad == 0) //No la encontro, entonces la agregamos
                    {
                        datos.setearConsulta("Insert into IMAGENES (IdArticulo, ImagenURL) values (@idArticulo, @imagenURL)");
                        datos.limpiarParametros(datos);
                        datos.setearParametro("@imagenURL", lista[x]);
                        datos.setearParametro("@idArticulo", iDArticulo);
                        datos.ejecutarAccion();
                    }
                    cantidad = 0;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { datos.cerrarConexion();}

        }
    }
}
