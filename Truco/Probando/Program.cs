using ManejadorCartas;

internal class Program
{
    public int ManejarTiradaBot(int valorCartaUno, int valorCartaDos, int valorCartaTres, int valorCartaTirada)
    {
        int cartaMasAlta, cartaDelMedio, cartaMasBaja;

        // Comparar las tres cartas y asignar los valores correspondientes
        if (valorCartaUno > valorCartaDos && valorCartaUno > valorCartaTres)
        {
            cartaMasAlta = valorCartaUno;

            if (valorCartaDos > valorCartaTres)
            {
                cartaDelMedio = valorCartaDos;
                cartaMasBaja = valorCartaTres;
            }
            else
            {
                cartaDelMedio = valorCartaTres;
                cartaMasBaja = valorCartaDos;
            }
        }
        else if (valorCartaDos > valorCartaUno && valorCartaDos > valorCartaTres)
        {
            cartaMasAlta = valorCartaDos;

            if (valorCartaUno > valorCartaTres)
            {
                cartaDelMedio = valorCartaUno;
                cartaMasBaja = valorCartaTres;
            }
            else
            {
                cartaDelMedio = valorCartaTres;
                cartaMasBaja = valorCartaUno;
            }
        }
        else
        {
            cartaMasAlta = valorCartaTres;

            if (valorCartaUno > valorCartaDos)
            {
                cartaDelMedio = valorCartaUno;
                cartaMasBaja = valorCartaDos;
            }
            else
            {
                cartaDelMedio = valorCartaDos;
                cartaMasBaja = valorCartaUno;
            }
        }
        if(cartaMasBaja > valorCartaTirada)
        {
            return cartaMasBaja;
        }
        else if (cartaDelMedio <= valorCartaTirada && cartaMasAlta < valorCartaTirada)
        {
            return cartaMasBaja;
        }
        else if(cartaMasAlta >= valorCartaTirada && cartaDelMedio < valorCartaTirada)
        {
            return cartaMasAlta;
        }
        else if( cartaMasAlta > valorCartaTirada && cartaMasBaja < valorCartaTirada && cartaDelMedio > valorCartaTirada)
        {
            return cartaDelMedio;
        }
        else if (cartaMasAlta > valorCartaTirada && cartaMasBaja < valorCartaTirada && cartaDelMedio < valorCartaTirada)
        {
            return cartaMasBaja;
        }
        else if(cartaMasBaja == valorCartaTirada && cartaDelMedio > valorCartaTirada)
        {
            return cartaDelMedio;
        }
        else if(cartaDelMedio == valorCartaTirada)
        {
            return cartaMasAlta;
        }
        else
        {
            return cartaMasAlta;
        }


    }

    public JugadorPadre ElegirPrimerJugador(JugadorUno j1, JugadorDos j2)
    {
        Random random = new Random();
        int numeroAleatorio = random.Next(2); // Genera un número aleatorio entre 0 y 1

        // Elige al jugador según el número aleatorio
        return (numeroAleatorio == 0) ? (JugadorPadre)j1 : (JugadorPadre)j2;
    }

    public int ManejarQuienGanaTirada(JugadorUno j1, JugadorDos j2, int valorCartaJugadorUno, int valorCartaJugadorDos)
    {
        // 0 si hay un empate.
        // 1 si JugadorUno gana la tirada.
        // 2 si JugadorUno pierde la tirada.

        JugadorPadre primerJugador = ElegirPrimerJugador(j1, j2);

        if (valorCartaJugadorUno == valorCartaJugadorDos)
        {
            return 0; // Empate
        }
        else if (valorCartaJugadorUno > valorCartaJugadorDos)
        {
            return primerJugador == j1 ? 1 : 2; // JugadorUno gana (1), JugadorDos pierde (2)
        }
        else // valorCartaJugadorUno < valorCartaJugadorDos
        {
            return primerJugador == j1 ? 2 : 1; // JugadorUno pierde (2), JugadorDos gana (1)
        }
    }

    private static void Main(string[] args)
    {
        JugadorUno J1 = new JugadorUno(); // Asegúrate de tener la definición de JugadorUno y JugadorDos
        JugadorDos J2 = new JugadorDos();

        Program programa = new Program();
        int resultadoTirada = programa.ManejarTiradaBot(1,2, 1, 48);
        Console.WriteLine("Resultado de la tirada: " + resultadoTirada);

        int resultadoGanador = programa.ManejarQuienGanaTirada(J1, J2, resultadoTirada, 47);
        Console.WriteLine("Resultado del ganador: " + resultadoGanador);
    }
}

// Define las clases JugadorUno y JugadorDos según tus necesidades.
public class JugadorPadre { }
public class JugadorUno : JugadorPadre { }
public class JugadorDos : JugadorPadre { }

