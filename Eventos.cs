using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hada

{
    class Eventos
    {
    }

    public class TocadoArgs : EventArgs
    {
        public string Nombre { get; private set; }
        public Coordenada CoordenadaImpacto { get; private set; }

        public TocadoArgs(string nombre, Coordenada coordenadaImpacto)
        {
            Nombre = nombre;
            CoordenadaImpacto = coordenadaImpacto;
        }
    }
}
