﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hada
{
    class Tablero
    {
        public int TamTablero { get; private set; }

        private List<Coordenada> coordenadasDisparadas;
        private List<Coordenada> coordenadasTocadas;
        private List<Barco> barcos;
        private List<Barco> barcosEliminados;
        private Dictionary<Coordenada, string> casillasTablero;

        public event EventHandler<EventArgs> eventoFinPartida;

        public Tablero(int tamTablero, List<Barco> barcos)
        {
            if (tamTablero < 4 || tamTablero > 9)
                throw new ArgumentException("El tamaño del tablero debe estar entre 4 y 9.");

            TamTablero = tamTablero;
            this.barcos = barcos;
            coordenadasDisparadas = new List<Coordenada>();
            coordenadasTocadas = new List<Coordenada>();
            barcosEliminados = new List<Barco>();
            casillasTablero = new Dictionary<Coordenada, string>();

            foreach (var barco in this.barcos)
            {
                barco.eventoTocado += cuandoEventoTocado;
                barco.eventoHundido += cuandoEventoHundido;
            }

            inicializaCasillasTablero();
        }

        private void inicializaCasillasTablero()
        {
            for (int i = 0; i < TamTablero; i++)
            {
                for (int j = 0; j < TamTablero; j++)
                {
                    Coordenada c = new Coordenada(i, j);
                    bool ocupado = false;
                    foreach (var barco in barcos)
                    {
                        foreach (var coord in barco.CoordenadasBarco.Keys)
                        {
                            if (coord.Equals(c))
                            {
                                casillasTablero[c] = barco.Nombre;
                                ocupado = true;
                                break;
                            }
                        }
                        if (ocupado) break;
                    }
                    if (!ocupado)
                    {
                        casillasTablero[c] = "AGUA";
                    }
                }
            }
        }

        public void Disparar(Coordenada c)
        {
            if (c.Fila < 0 || c.Fila >= TamTablero || c.Columna < 0 || c.Columna >= TamTablero)
            {
                Console.WriteLine($"La coordenada {c.ToString()} está fuera de las dimensiones del tablero.");
                return;
            }

            coordenadasDisparadas.Add(c);

            foreach (var barco in barcos)
            {
                barco.Disparo(c);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var barco in barcos)
            {
                sb.Append($"[{barco.Nombre}] - DAÑOS: [{barco.NumDanyos}] - HUNDIDO: [{barco.hundido()}] - COORDENADAS: ");
                foreach (var coord in barco.CoordenadasBarco.Keys)
                {
                    sb.Append($"[{coord.ToString()} :{barco.CoordenadasBarco[coord]}] ");
                }
                sb.AppendLine();
            }

            sb.AppendLine();
            sb.Append("Coordenadas disparadas: ");
            if (coordenadasDisparadas.Count > 0)
            {
                foreach (var c in coordenadasDisparadas)
                {
                    sb.Append($"{c.ToString()} ");
                }
            }
            sb.AppendLine();

            sb.Append("Coordenadas tocadas: ");
            if (coordenadasTocadas.Count > 0)
            {
                foreach (var c in coordenadasTocadas)
                {
                    sb.Append($"{c.ToString()} ");
                }
            }

            for (int i = 0; i < 4; i++)
            {
                sb.AppendLine();
            }

            sb.AppendLine("CASILLAS TABLERO");
            sb.AppendLine("-------");
            sb.AppendLine(DibujarTablero());

            return sb.ToString();
        }

        private string DibujarTablero()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < TamTablero; i++)
            {
                for (int j = 0; j < TamTablero; j++)
                {
                    Coordenada c = new Coordenada(i, j);
                    if (casillasTablero.ContainsKey(c))
                    {
                        string valor = casillasTablero[c];
                        if (valor == "AGUA")
                        {
                            sb.Append("[AGUA] ");
                        }
                        else if (valor.EndsWith("_T"))
                        {

                            sb.Append($"[{valor}] ");
                        }
                        else
                        {

                            sb.Append($"[{valor}] ");
                        }
                    }
                    else
                    {
                        sb.Append("[AGUA] ");
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private void cuandoEventoTocado(object sender, TocadoArgs e)
        {
            foreach (var key in casillasTablero.Keys)
            {
                if (key.Equals(e.CoordenadaImpacto))
                {
                    if (!casillasTablero[key].EndsWith("_T"))
                        casillasTablero[key] = casillasTablero[key] + "_T";
                    break;
                }
            }

            bool existe = false;
            foreach (var coord in coordenadasTocadas)
            {
                if (coord.Equals(e.CoordenadaImpacto))
                {
                    existe = true;
                    break;
                }
            }
            if (!existe)
                coordenadasTocadas.Add(e.CoordenadaImpacto);

            Console.WriteLine($"TABLERO: Barco [{e.Nombre}] tocado en Coordenada: {e.CoordenadaImpacto.ToString()}");
        }

        private void cuandoEventoHundido(object sender, HundidoArgs e)
        {
            Console.WriteLine($"TABLERO: Barco [{e.Nombre}] hundido!!");

            Barco barcoEliminado = null;
            foreach (var barco in barcos)
            {
                if (barco.Nombre == e.Nombre)
                {
                    barcoEliminado = barco;
                    break;
                }
            }
            if (barcoEliminado != null && !barcosEliminados.Contains(barcoEliminado))
                barcosEliminados.Add(barcoEliminado);

            if (barcosEliminados.Count == barcos.Count)
            {
                eventoFinPartida?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
