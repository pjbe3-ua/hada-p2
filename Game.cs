using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hada
{
    class Game
    {
        private bool finPartida;

        public Game()
        {
            finPartida = false;
            gameLoop();
        }

        private void gameLoop()
        {
            List<Barco> barcos = new List<Barco>();
            try
            {
                barcos.Add(new Barco("THOR", 1, 'h', new Coordenada(0, 0)));   // Ocupa (0,0), (0,1), (0,2)
                barcos.Add(new Barco("LOKI", 2, 'v', new Coordenada(1, 2)));   // Ocupa (2,3), (3,3)
                barcos.Add(new Barco("MAYA", 3, 'h', new Coordenada(3, 1)));   // Ocupa (4,1), (4,2), (4,3), (4,4)
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al crear los barcos: " + ex.Message);
                return;
            }

            int tamTablero = 4;
            Tablero tablero = new Tablero(tamTablero, barcos);
            tablero.eventoFinPartida += cuandoEventoFinPartida;

            Console.WriteLine(tablero.ToString());

            while (!finPartida)
            {
                Console.WriteLine("Introduce la coordenada a la que disparar FILA,COLUMNA ('S' para Salir):");
                string input = Console.ReadLine();

                if (input.Trim().ToLower() == "s")
                {
                    Console.WriteLine("PARTIDA FINALIZADA!!");
                    break;
                }

                string[] parts = input.Split(',');
                if (parts.Length != 2)
                {
                    Console.WriteLine("Formato incorrecto. Usa el formato NUMERO,NUMERO (ej: 0,3).");
                    continue;
                }

                if (!int.TryParse(parts[0].Trim(), out int fila) || !int.TryParse(parts[1].Trim(), out int columna))
                {
                    Console.WriteLine("Formato incorrecto. Asegúrate de introducir números.");
                    continue;
                }

                try
                {
                    Coordenada coord = new Coordenada(fila, columna);
                    tablero.Disparar(coord);
                    Console.WriteLine(tablero.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al procesar la coordenada: " + ex.Message);
                }
            }
        }

        // Manejador del evento de fin de partida
        private void cuandoEventoFinPartida(object sender, EventArgs e)
        {
            Console.WriteLine("PARTIDA FINALIZADA!!");
            finPartida = true;
        }
    }
}
