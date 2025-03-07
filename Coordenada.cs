using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hada
{
    public class Coordenada
    {
        private int fila;
        private int columna;

        public int Fila
        {
            get { return fila; }
            set
            {
                if (value < 0 || value > 9)
                    throw new ArgumentOutOfRangeException("Fila", "La fila debe estar entre 0 y 9.");
                fila = value;
            }
        }

        public int Columna
        {
            get { return columna; }
            set
            {
                if (value < 0 || value > 9)
                    throw new ArgumentOutOfRangeException("Columna", "La columna debe estar entre 0 y 9.");
                columna = value;
            }
        }

        public Coordenada()
        {
            Fila = 0;
            Columna = 0;
        }

        public Coordenada(int fila, int columna)
        {
            Fila = fila;
            Columna = columna;
        }

        public Coordenada(string fila, string columna)
        {
            if (!int.TryParse(fila, out int f) || !int.TryParse(columna, out int c))
                throw new ArgumentException("Las cadenas no representan números válidos.");
            Fila = f;
            Columna = c;
        }

        public Coordenada(Coordenada otra)
        {
            if (otra == null)
                throw new ArgumentNullException("otra");
            Fila = otra.Fila;
            Columna = otra.Columna;
        }

        public override string ToString()
        {
            return $"({Fila},{Columna})";
        }

        public override int GetHashCode()
        {
            return Fila.GetHashCode() ^ Columna.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Coordenada otra)
                return Equals(otra);
            return false;
        }

        public bool Equals(Coordenada otra)
        {
            if (otra == null)
                return false;
            return this.Fila == otra.Fila && this.Columna == otra.Columna;
        }
    }
}
