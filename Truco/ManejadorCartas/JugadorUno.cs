using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManejadorCartas
{
    public class JugadorUno : JugadorPadre
    {
        public JugadorUno()
        {

        }

        public JugadorUno(string nombreJugador, string apodoJugador, int edadJugador, int valorCartaJugada, string fotoJugador, int puntaje) 
        : base(nombreJugador, apodoJugador, edadJugador,valorCartaJugada, fotoJugador, puntaje)
        {
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
        //proximo paso clase carta
    }
}
