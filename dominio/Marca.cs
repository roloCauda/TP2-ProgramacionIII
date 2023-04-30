using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    class Marca
    {
        public Marca()
        {
            //puede nacer con un ID y no tener set en ID
        }

        public int IdMarca { get; set; }
        public string Nombre { get; set; }
        //no se si deberia tener que proveedores la tienen

        public override string ToString()
        {
            return Nombre;         
        }
    }
}
