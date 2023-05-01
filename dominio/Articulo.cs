using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Articulo
    {
        public int IdArticulo { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Marca IdMarca { get; set; }
        public Categoria IdCategoria { get; set; }
        public List<Imagen> ListaImagenes { get; set; }
        public float Precio { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
