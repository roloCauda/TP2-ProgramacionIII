using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    class Imagen
    {
        public Imagen()
        {
            //puede nacer con un ID y no tener set en ID
        }

        public int IdImagen { get; set; }
        public int IdArticulo { get; set; }
        public string ImagenURL { get; set; }
    }
}
