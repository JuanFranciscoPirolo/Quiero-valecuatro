using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManejadorCartas
{
    public class JugadorDos : JugadorPadre
    {
        public JugadorDos()
        {
        }

        public JugadorDos(string nombreJugador, string apodoJugador, int edadJugador, int valorCartaJugada, string fotoJugador, int puntaje)
        : base(nombreJugador, apodoJugador, edadJugador, valorCartaJugada, fotoJugador, puntaje)
        {
            this.puntaje = puntaje;
            this.nombreJugador = nombreJugador;
            this.apodoJugador = apodoJugador;
            this.edadJugador = edadJugador;
            this.valorCartaJugada = valorCartaJugada;
            this.fotoJugador = fotoJugador;
        }

        public override string ObtenerNombre()
        {
            return this.nombreJugador;
        }
    }
}
