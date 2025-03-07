using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hada

{
    class Barco
    {
        public Dictionary<Coordenada, string> CoordenadasBarco { get; private set; }
        public string Nombre { get; private set; }
        public int NumDanyos { get; private set; }

        public event EventHandler<TocadoArgs> eventoTocado;
        public event EventHandler<HundidoArgs> eventoHundido;

        public Barco(string nombre, int longitud, char orientacion, Coordenada coordenadaInicio)
        {
            if (longitud <= 0)
                throw new ArgumentException("La longitud debe ser mayor que 0.");

            Nombre = nombre;
            NumDanyos = 0;
            CoordenadasBarco = new Dictionary<Coordenada, string>();

            for (int i = 0; i < longitud; i++)
            {
                Coordenada c;
                if (char.ToLower(orientacion) == 'h')
                {
                    c = new Coordenada(coordenadaInicio.Fila, coordenadaInicio.Columna + i);
                }
                else if (char.ToLower(orientacion) == 'v')
                {
                    c = new Coordenada(coordenadaInicio.Fila + i, coordenadaInicio.Columna);
                }
                else
                {
                    throw new ArgumentException("La orientación debe ser 'h' o 'v'.");
                }

                CoordenadasBarco[c] = Nombre;
            }
        }

        public void Disparo(Coordenada c)
        {
            foreach (var key in CoordenadasBarco.Keys)
            {
                if (key.Equals(c))
                {
                    if (!CoordenadasBarco[key].EndsWith("_T"))
                    {
                        CoordenadasBarco[key] = CoordenadasBarco[key] + "_T";
                        NumDanyos++;

                        eventoTocado?.Invoke(this, new TocadoArgs(Nombre, c));

                        if (hundido())
                        {
                            eventoHundido?.Invoke(this, new HundidoArgs(Nombre));
                        }
                    }
                    return;
                }
            }
        }

        public bool hundido()
        {
            foreach (var etiqueta in CoordenadasBarco.Values)
            {
                if (!etiqueta.EndsWith("_T"))
                    return false;
            }
            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Barco: {Nombre}");
            sb.AppendLine($"Daños: {NumDanyos}");
            sb.AppendLine($"Estado: {(hundido() ? "Hundido" : "A flote")}");
            sb.AppendLine("Coordenadas:");
            foreach (var kvp in CoordenadasBarco)
            {
                sb.AppendLine($"{kvp.Key.ToString()} -> {kvp.Value}");
            }
            return sb.ToString();
        }
    }
}
