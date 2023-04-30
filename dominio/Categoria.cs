using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    class Categoria
    {
        public Categoria()
        {
            //puede nacer con un ID y no tener set en ID
        }

        public int IdCategoria { get; set; }
        public string Nombre { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
