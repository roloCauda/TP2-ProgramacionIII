using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    class Proveedor
    {
        public Proveedor()
        {
            //puede nacer con un ID y no tener set en ID
        }
        public int IdProveedor { get; set; }
        public string Nombre { get; set; }
        public List<Marca> ListaMarcas { get; set; } //que marcas trabaja

        public override string ToString()
        {
            return Nombre;
        }
    }
}
