using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    class Articulo
    {
        public Articulo()
        {
            //puede nacer con un ID y no tener set en ID
        }

        public int IdArticulo { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Marca IdMarca { get; set; }
        public Categoria IdCAtegoria { get; set; } //podria pertenecer a mas de una categoria
        public List<Imagen> ListaImagenes { get; set; }
        public float Precio { get; set; }
        public Proveedor IdProveedor { get; set; } //podria pertenecer a mas de una proveedor

        public override string ToString()
        {
            return Nombre;
        }
    }
}
