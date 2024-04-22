using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace ManejadorCartas
{
    public class Carta
    {
        public int numero { get; set; }
        public string palo { get; set; }
        public int valor { get; set; }
        public string imagen { get; set; }

        public Carta()
        {

        }

        public static List<Carta> RepartirMano()
        {
            List<Carta> manoCartas = new List<Carta>();
            List<Carta> cartas;

            try
            {
                string json = File.ReadAllText(@"../../../../ManejadorCartas/Cartas.json");
                cartas = JsonSerializer.Deserialize<List<Carta>>(json);

                // Elegir seis cartas no repetidas
                Random random = new Random();
                List<Carta> cartasElegidas = new List<Carta>();

                while (cartasElegidas.Count < 6)
                {
                    int indiceAleatorio = random.Next(cartas.Count);
                    Carta cartaElegida = cartas[indiceAleatorio];

                    if (!cartasElegidas.Contains(cartaElegida))
                    {
                        cartasElegidas.Add(cartaElegida);
                    }
                }

                // Asignar las tres primeras cartas al primer jugador
                manoCartas.AddRange(cartasElegidas.Take(3));

                // Asignar las tres últimas cartas al segundo jugador
                manoCartas.AddRange(cartasElegidas.Skip(3));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer el archivo JSON: {ex.Message}");
            }
            return manoCartas;
        }


        public int ManejarQuienGanaTirada(JugadorUno j1, JugadorDos j2, int valorCartaJugadorUno, int valorCartaJugadorDos)
        {
            //0 si hay un empate.
            //1 si JugadorUno gana la tirada.
            //2 si JugadorDos gana la tirada.
            

            if (valorCartaJugadorUno == valorCartaJugadorDos)
            {
                return 0; // Empate
            }
            else if (valorCartaJugadorUno > valorCartaJugadorDos)
            {
                return 1;
            }
            else // valorCartaJugadorUno < valorCartaJugadorDos
            {
                return 2;
            }
        }

        public JugadorPadre ElegirPrimerJugador(JugadorUno j1, JugadorDos j2)
        {
            Random random = new Random();
            int numeroAleatorio = random.Next(2); // Genera un número aleatorio entre 0 y 1

            // Elige al jugador según el número aleatorio
            return (numeroAleatorio == 0) ? (JugadorPadre)j1 : (JugadorPadre)j2;
        }


    }
}
