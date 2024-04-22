namespace ManejadorCartas
{
    public abstract class JugadorPadre
    {
        protected string nombreJugador;
        protected string apodoJugador;
        protected int edadJugador;
        protected int valorCartaJugada;
        protected string fotoJugador;
        protected int puntaje;

        protected JugadorPadre()
        {
        }

        protected JugadorPadre(string nombreJugador, string apodoJugador, int edadJugador, int valorCartaJugada, string fotoJugador, int puntaje)
        {
            this.puntaje = puntaje;
            this.nombreJugador = nombreJugador;
            this.apodoJugador = apodoJugador;
            this.edadJugador = edadJugador;
            this.valorCartaJugada = valorCartaJugada;
            this.fotoJugador = fotoJugador;
        }

        public string NombreJugador
        {
            get { return nombreJugador; }
            set { nombreJugador = value; }
        }
        public int Puntaje
        {
            get { return puntaje; }
            set { puntaje = value; }
        }

        public string ApodoJugador
        {
            get { return apodoJugador; }
            set { apodoJugador = value; }
        }

        public int EdadJugador
        {
            get { return edadJugador; }
            set { edadJugador = value; }
        }

        public int ValorCartaJugada
        {
            get { return valorCartaJugada; }
            set { valorCartaJugada = value; }
        }
        public string FotoJugador
        {
            get { return fotoJugador; }
            set { fotoJugador = value; }
        }

        public virtual string ObtenerNombre()
        {
            return nombreJugador;
        }
    }
}