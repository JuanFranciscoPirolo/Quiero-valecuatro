using ManejadorCartas;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Media;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace UI_TRUCO
{
    public partial class frmPartidaTruco : Form
    {
        //vaciar primero la lista
        private bool banderaMazo = false;
        private bool banderaQuiero = false;
        private bool banderaNoQuiero = false;
        private int cartaSeleccionadaPorJugador = -1; // Inicializado con un valor que no corresponde a ninguna carta
        string[] trucoMaradona = { @"../../../../UI TRUCO/MaradonaTruco/truco1.wav", @"../../../../UI TRUCO/MaradonaTruco/truco2.wav", @"../../../../UI TRUCO/MaradonaTruco/truco3.wav" };
        string[] trucoMessi = { @"../../../../UI TRUCO/MessiTruco/truco1.wav", @"../../../../UI TRUCO/MessiTruco/truco2.wav", @"../../../../UI TRUCO/MessiTruco/truco3.wav" };
        string[] trucoLuisito = { @"../../../../UI TRUCO/LuisitoTruco/truco2.wav", @"../../../../UI TRUCO/LuisitoTruco/truco3.wav" };
        string[] trucoMomo = { @"../../../../UI TRUCO/MomoTruco/truco2.wav", @"../../../../UI TRUCO/MomoTruco/truco1.wav", @"../../../../UI TRUCO/MomoTruco/truco3.wav" };
        private bool noEsTurnoPrimerJugador = false;
        private System.Timers.Timer contadorTimer;
        private bool continuarPartida = true;
        private Thread hiloPartida;
        private int tiempoRestante = 5;
        private string personajeElegido;
        //List<Carta> primerJugador = Carta.RepartirMano();
        //List<Carta> segundoJugador = Carta.RepartirMano();
        // Nuevo: Añadir un Label para mostrar el número
        private Label lblContador;

        // Nuevo: Añadir un Panel para cubrir todo el formulario
        private Panel panelNegro;

        public frmPartidaTruco(string personaje)
        {
            InitializeComponent();
            this.personajeElegido = personaje;
            this.MaximizeBox = false;
            this.DoubleBuffered = true;
        }

        private void frmPartidaTruco_Load(object sender, EventArgs e)
        {
            btnRealEnvido.Visible = false;
            btnFaltaEnvido.Visible = false;
            btnRetruco.Visible = false;
            btnValecuatro.Visible = false;
            btnNoQuieroo.Visible = false;
            btnQuieroo.Visible = false;
            JugadorDos jugadorDos = new JugadorDos();
            jugadorDos.FotoJugador = @"../../../../UI TRUCO/resources/jugadorDos.jpg";
            SoundPlayer sonido = new SoundPlayer();
            sonido.SoundLocation = @"../../../../UI TRUCO/sonidos/gardel.wav";
            sonido.Stop();
            string foto = ObtenerFoto();

            // Nuevo: Crear el Label para mostrar el número
            lblContador = new Label();
            lblContador.AutoSize = true;
            lblContador.Font = new Font("Arial", 24, FontStyle.Bold);
            lblContador.BackColor = Color.Black;
            lblContador.Location = new Point((1364 - lblContador.Width) / 2, (800 - lblContador.Height) / 2);


            Controls.Add(lblContador);

            // Nuevo: Crear el Panel para cubrir todo el formulario
            panelNegro = new Panel();
            panelNegro.BackColor = Color.Black;
            panelNegro.Dock = DockStyle.Fill;
            panelNegro.Visible = false;
            Controls.Add(panelNegro);

            DialogResult resultado = MessageBox.Show("Empezarás la partida cuando presiones OK", "La partida empezará en...", MessageBoxButtons.OK);

            if (resultado == DialogResult.OK)
            {
                IniciarContador();
            }

            PictureBox fotoPerfil = new PictureBox();
            PictureBox fotoPerfilDos = new PictureBox();

            // Establecer propiedades del PictureBox
            fotoPerfilDos.Width = 100;
            fotoPerfilDos.Height = 100;
            fotoPerfilDos.SizeMode = PictureBoxSizeMode.StretchImage;
            fotoPerfilDos.ImageLocation = jugadorDos.FotoJugador;

            fotoPerfil.Width = 100;
            fotoPerfil.Height = 100;
            fotoPerfil.SizeMode = PictureBoxSizeMode.StretchImage;
            fotoPerfil.ImageLocation = foto;
            RedondearImagen(fotoPerfil);
            RedondearImagen(fotoPerfilDos);

            // Agregar el PictureBox al formulario
            Controls.Add(fotoPerfil);
            Controls.Add(fotoPerfilDos);

            // Puedes ajustar la posición del PictureBox si es necesario
            fotoPerfil.Location = new Point(1150, 600);
            fotoPerfilDos.Location = new Point(80, 150);
        }

        private void IniciarContador()
        {
            // Establecer el Panel para cubrir todo el formulario
            panelNegro.Location = new Point(0, 0);
            panelNegro.Size = new Size(ClientSize.Width, ClientSize.Height);
            panelNegro.BringToFront(); // Asegurarse de que esté en la parte superior
            panelNegro.Visible = true;
            lblContador.ForeColor = Color.White;
            lblContador.BackColor = Color.Black;
            lblContador.BringToFront();
            contadorTimer = new System.Timers.Timer();
            contadorTimer.Interval = 1000;
            contadorTimer.Elapsed += ContadorTimer_Elapsed;
            contadorTimer.Start();
        }

        private void ContadorTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (IsHandleCreated)  // Verificar si el formulario está aún en ejecución
            {
                if (tiempoRestante > 0)
                {
                    MostrarContadorEnUI(tiempoRestante.ToString());
                    tiempoRestante--;
                }
                else
                {
                    contadorTimer.Stop();
                    MostrarContadorEnUI("");
                    // Nuevo: Ocultar el panel negro cuando el contador termina
                    panelNegro.Invoke((MethodInvoker)(() => panelNegro.Visible = false));
                    IniciarHiloPartida();
                }
            }
        }



        private void MostrarContadorEnUI(string texto)
        {
            if (lblContador.InvokeRequired)
            {
                lblContador.BeginInvoke((MethodInvoker)(() => lblContador.Text = texto));
            }
            else
            {
                lblContador.Text = texto;
                lblContador.ForeColor = Color.White;
            }
        }

        private void IniciarHiloPartida()
        {
            hiloPartida = new Thread(new ThreadStart(ManejarPartida));
            hiloPartida.Start();



        }
        private bool envidoVisible = true;
        private void ManejarPartida()
        {
            //la carta no se puede mover mas de una vez, o sea no se puede usar una carta mas de una vez.
            //arreglar bucle while
            //pasar a metodo todo este choclo del bot
            //ojo con empardar
            //manejar quien gana la tirada y mostrarle en un messagebox

            bool segundaManoPrimerJugador = false;
            int primeraRonda = 1;
            bool enMano = true;
            bool repartir = false;
            JugadorUno jugadorUno = new JugadorUno();
            JugadorDos jugadorDos = new JugadorDos();
            bool finalizarMano = true;
            bool tirarPrimeroBot = false;
            int ganador = 0;
            int puntajeUno = jugadorUno.Puntaje = 0;
            int puntajeDos = jugadorDos.Puntaje = 0;
            int puntosEnManoJugadorUno = 0;
            int puntosEnManoJugadorDos = 0;
            bool banderaRonda = true;
            Carta carta = new Carta();
            int contadorRondas = 0;
            JugadorPadre primerJugador = carta.ElegirPrimerJugador(jugadorUno, jugadorDos);
            //si ya paso la primer mano que no cante envido el bot, preguntando si contadortirado es mayor a 0 que no cante
            while (puntajeUno < 31 || puntajeDos < 31)
            {
                int contadorTirado = 0;
                bool envidoTerminado = false;
                //falta mazo y flor.
                bool envidoVisible = true;
                int sumadoresEnvidos = 0;
                bool faltaEnvidoQuerido = false;
                int contadorEnvidos = 0;
                int sumadorPuntosEnvido = 0;
                bool envidoQuerido = false;
                bool envidoEnvidoQuerido = false;
                bool realEnvidoQuerido = false;
                int cantarUnaSolaVezDos = 0;
                int cantarUnaSolaVezTres = 0;
                int cantarUnaSolaVezCuatro = 0;
                int cantarUnaSolaVezCinco = 0;
                int unaSolaVez = 0;
                int contadorTirada = 0;
                tirarPrimeroBot = false;
                contadorRondas++;
                // Desasociar eventos antes de continuar con el próximo bucle
                int trucoCantado = 0;
                bool quisoTruco = false;
                bool quisoRetruco = false;
                bool quisoValecuatro = false;
                bool ganadorManoAnterior = false;
                int preguntarUnaVez = 0;
                int cartaATirar = 0;
                int cantarUnaSolaVez = 0;
                int preguntarPorPrimeraVez = 0;
                bool seEmpatoPrimeraTirada = false;
                puntosEnManoJugadorUno = 0;
                puntosEnManoJugadorDos = 0;
                int multiplicadorTruco = 1;
                bool primeraTirada = false;
                Task.Delay(2500).Wait();
                List<Carta> Cartas = AsignarImagenCarta();

                ResetearPosicionesPictureBox();
                RestablecerCartas();
                banderaMazo = false;
                finalizarMano = true;

                primerJugador = (contadorRondas % 2 == 0) ? jugadorDos : jugadorUno;

                txtPuntuacion1.Invoke((MethodInvoker)delegate
                {
                    txtPuntuacion1.Text = puntajeDos.ToString();
                });

                // Actualizar puntaje del jugador 2 en el hilo de la interfaz de usuario
                txtPuntuacion2.Invoke((MethodInvoker)delegate
                {
                    txtPuntuacion2.Text = puntajeUno.ToString();
                });
                this.Invoke((MethodInvoker)delegate
                {
                    btnTruco.Visible = true;
                    btnEnvido.Visible = true;
                    btnFlor.Visible = true;
                    btnRetruco.Visible = false;
                    btnMazo.Visible = true;
                });
                while (finalizarMano)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        btnValecuatro.Visible = false;
                        btnTruco.Visible = true;
                        btnEnvido.Visible = true;
                        btnFlor.Visible = true;
                        btnRetruco.Visible = false;
                        btnMazo.Visible = true;
                    });
                    if (contadorTirado > 0)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            btnFlor.Visible = false;
                            btnEnvido.Visible = false;
                            btnRealEnvido.Visible = false;
                            btnFaltaEnvido.Visible = false;
                        });
                    }
                    //cantar los otros envidos y tambien tiene que cANTAR EN LA OTRA MANO
                    //en el jardin del alma la esperanza es flor

                    if (primerJugador == jugadorDos)
                    {

                        string decisionBot = TomarDecisionBot(Cartas[3].valor, Cartas[4].valor, Cartas[5].valor);

                        if (cantarUnaSolaVezDos == 0)
                        {
                            tirarPrimeroBot = true;
                            string cantoBot = ManejarTrucoBot(Cartas[3].valor, Cartas[4].valor, Cartas[5].valor);
                            cantarUnaSolaVezDos++;

                            //pongo cantar envido, porque si o si tiene que ser en la primer mano
                            //cantar envido solo en la primer mano, pasa la segunda mano y eliminar botones
                            //decisionBot = "cantar envido"; //ojo eh
                            //error cuando canto envido si el primer jugador es el segundo
                            if (decisionBot == "cantar envido")
                            {

                                string tipoEnvido = "envido";
                                if (contadorTirado == 0)
                                {
                                    ReproducirAudioBot(tipoEnvido); // Reproducir el audio del tipo de Envido obtenido
                                                                    //aca delay
                                                                    //Task.Delay(6500).Wait();
                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        btnEnvido.Visible = true;
                                        btnTruco.Visible = false;
                                        btnQuieroo.Top -= 55;
                                        btnNoQuieroo.Top -= 55;
                                        btnNoQuieroo.Visible = true;
                                        btnQuieroo.Visible = true;
                                        btnRealEnvido.Visible = true;
                                        btnFaltaEnvido.Visible = true;
                                        btnFlor.Visible = false;
                                    });

                                    btnQuieroo.Click += (sender, e) =>
                                    {
                                        //error puntaje real envido, suma de a 30
                                        envidoTerminado = true;
                                        if (cantarUnaSolaVez == 0)
                                        {
                                            btnEnvido.Visible = false;
                                            btnRealEnvido.Visible = false;
                                            btnFaltaEnvido.Visible = false;
                                            JugadorPadre jugadorGanador = ManejarEnvidoGeneral(jugadorUno, jugadorDos, Cartas, puntajeUno, puntajeDos, tipoEnvido, sumadorPuntosEnvido);

                                            int puntaje = 0;
                                            if (tipoEnvido == "envido")
                                            {
                                                puntaje = 2;
                                            }
                                            else if (tipoEnvido == "real envido")
                                            {
                                                puntaje = 3;
                                            }
                                            else
                                            {
                                                puntaje = 30;
                                            }
                                            if (jugadorGanador == null)
                                            {
                                                if (primerJugador == jugadorUno)
                                                {
                                                    puntajeUno += puntaje;
                                                }
                                                else
                                                {
                                                    puntajeDos += puntaje;
                                                }
                                            }
                                            else if (jugadorGanador == jugadorUno)
                                            {
                                                // Acciones si gana jugadorUno
                                                puntajeUno += puntaje;
                                            }
                                            else if (jugadorGanador == jugadorDos)
                                            {
                                                // Acciones si gana jugadorDos
                                                puntajeDos += puntaje;
                                            }
                                        }
                                    };

                                    btnNoQuieroo.Click += (sender, e) =>
                                    {
                                        puntajeDos += 2;
                                        btnEnvido.Visible = false;
                                        btnRealEnvido.Visible = false;
                                        btnFaltaEnvido.Visible = false;
                                    };
                                    btnEnvido.Click += (sender, e) =>
                                    {
                                        if (envidoVisible)
                                        {
                                            btnTruco.Visible = true;
                                            btnEnvido.Visible = true;
                                            btnFlor.Visible = false;
                                            btnRetruco.Visible = false;
                                            btnMazo.Visible = false;
                                            btnRealEnvido.Visible = true;
                                            btnFaltaEnvido.Visible = true;

                                        }
                                        else
                                        {
                                            string audio = "";
                                            SoundPlayer sonidoTruco = new SoundPlayer();

                                            switch (personajeElegido.ToLower())
                                            {
                                                case "maradona":
                                                    audio = @"../../../../UI TRUCO/MaradonaEnvido/envido1.wav";
                                                    break;
                                                case "messi":
                                                    audio = @"../../../../UI TRUCO/MessiEnvido/envido1.wav";
                                                    break;
                                                case "momo":
                                                    audio = @"../../../../UI TRUCO/MomoEnvido/envido1.wav";
                                                    break;
                                                default:
                                                    break;
                                                    sonidoTruco.SoundLocation = audio;
                                                    sonidoTruco.Play();
                                            }

                                            // Cambiar la visibilidad de los botones al estado contrario
                                            btnTruco.Visible = true;
                                            btnEnvido.Visible = true;
                                            btnFlor.Visible = false;
                                            btnRetruco.Visible = false;
                                            btnMazo.Visible = true;
                                            btnRealEnvido.Visible = false;
                                            btnFaltaEnvido.Visible = false;
                                        }
                                        btnRetruco.Visible = false;
                                        // Cambiar el estado de visibilidad para la próxima vez que se haga clic en el botón
                                        envidoVisible = !envidoVisible;
                                        btnQuieroo.Visible = false;
                                        btnNoQuieroo.Visible = false;
                                        btnEnvido.Visible = false;
                                        btnRealEnvido.Visible = false;
                                        //basicamente preguntar una sola vez
                                        if (cantarUnaSolaVezCinco == 0)
                                        {
                                            cantarUnaSolaVezCinco++;
                                            Random random = new Random();
                                            int numeroAleatorioOcho = random.Next(1, 3);
                                            Task.Delay(6500).Wait();
                                            if (numeroAleatorioOcho == 1)
                                            {

                                                ReproducirAudioBot("quiero");
                                                envidoTerminado = true;
                                                JugadorPadre jugadorGanador = ManejarEnvidoGeneral(jugadorUno, jugadorDos, Cartas, puntajeUno, puntajeDos, tipoEnvido, sumadorPuntosEnvido);

                                                if (jugadorGanador == null)
                                                {
                                                    if (primerJugador == jugadorUno)
                                                    {
                                                        puntajeUno += 4;
                                                    }
                                                    else
                                                    {
                                                        puntajeDos += 4;
                                                    }
                                                }
                                                else if (jugadorGanador == jugadorUno)
                                                {
                                                    // Acciones si gana jugadorUno
                                                    puntajeUno += 4;
                                                }
                                                else if (jugadorGanador == jugadorDos)
                                                {
                                                    // Acciones si gana jugadorDos
                                                    puntajeDos += 4;
                                                }
                                            }
                                            else
                                            {
                                                if (cantarUnaSolaVezTres == 0)
                                                {
                                                    ReproducirAudioBot("no quiero");
                                                }
                                            }
                                        }

                                    };
                                    btnRealEnvido.Click += (sender, e) =>
                                    {
                                        if (preguntarPorPrimeraVez == 0)
                                        {
                                            preguntarPorPrimeraVez++;
                                            List<int> numerosMismoPalo = ObtenerNumerosMismoPalo(Cartas);
                                            string canto = ManejarEnvidoBot(numerosMismoPalo[0], numerosMismoPalo[1]);
                                            btnRealEnvido.Visible = false;
                                            Task.Delay(6500).Wait();
                                            if (canto == "real envido")
                                            {
                                                ReproducirAudioBot("quiero");
                                                envidoTerminado = true;
                                                JugadorPadre jugadorGanador = ManejarEnvidoGeneral(jugadorUno, jugadorDos, Cartas, puntajeUno, puntajeDos, tipoEnvido, sumadorPuntosEnvido);

                                                if (jugadorGanador == null)
                                                {
                                                    if (primerJugador == jugadorUno)
                                                    {
                                                        puntajeUno += 5;
                                                    }
                                                    else
                                                    {
                                                        puntajeDos += 5;
                                                    }
                                                }
                                                else if (jugadorGanador == jugadorUno)
                                                {
                                                    // Acciones si gana jugadorUno
                                                    puntajeUno += 5;
                                                }
                                                else if (jugadorGanador == jugadorDos)
                                                {
                                                    // Acciones si gana jugadorDos
                                                    puntajeDos += 5;
                                                }
                                            }
                                            else
                                            {

                                                if (cantarUnaSolaVezCuatro == 0)
                                                {
                                                    ReproducirAudioBot("no quiero");
                                                }
                                            }
                                        }
                                    };
                                    btnFaltaEnvido.Click += (sender, e) =>
                                    {
                                        btnRealEnvido.Visible = false;
                                        List<int> numerosMismoPalo = ObtenerNumerosMismoPalo(Cartas);
                                        string canto = ManejarEnvidoBot(numerosMismoPalo[0], numerosMismoPalo[1]);
                                        Task.Delay(6500).Wait();
                                        if (preguntarUnaVez == 0)
                                        {
                                            preguntarUnaVez++;
                                            if (canto == "falta envido")
                                            {

                                                ReproducirAudioBot("quiero");
                                                envidoTerminado = true;
                                                JugadorPadre jugadorGanador = ManejarEnvidoGeneral(jugadorUno, jugadorDos, Cartas, puntajeUno, puntajeDos, tipoEnvido, sumadorPuntosEnvido);

                                                if (jugadorGanador == null)
                                                {
                                                    if (primerJugador == jugadorUno)
                                                    {
                                                        puntajeUno += 30;
                                                    }
                                                    else
                                                    {
                                                        puntajeDos += 30;
                                                    }
                                                }
                                                else if (jugadorGanador == jugadorUno)
                                                {
                                                    // Acciones si gana jugadorUno
                                                    puntajeUno += 30;
                                                }
                                                else if (jugadorGanador == jugadorDos)
                                                {
                                                    // Acciones si gana jugadorDos
                                                    puntajeDos += 30;
                                                }
                                            }
                                            else
                                            {
                                                if (cantarUnaSolaVezCinco == 0)
                                                {
                                                    cantarUnaSolaVezCinco++;
                                                    ReproducirAudioBot("no quiero");
                                                }
                                            }
                                        }
                                    };

                                }
                            }
                            
                        }
                        else //segunda mano
                        {
                            //hola
                            this.Invoke((MethodInvoker)delegate
                            {
                                btnEnvido.Visible = false;
                                btnFlor.Visible = false;
                                btnRealEnvido.Visible = false;
                            });
                            //seguir con el envido
                            //que espere un segundo antes de empezar otra mano cuando el jugador pierde la mano y tira una carta baja(no se puede ve rlo que tira)
                            //despues del finalizar mano tengo que darle un delay al hilo.
                            //quiero que cante truco envido en cualquier momento siendo mano o no, pero que no afecte el funcionamieno de la tirada.
                            //canta truco solo si la primer mano la pierdo. arreglar eso
                            //if contador == numeroaleatorio, esto para cantar truco en diferentes manos o valecuatro si se quiere
                            //error en parda, mano bot.
                            //manejar retruco y valecuatro cuando canto yo, manejar envidi real envido falta envido. 
                            //en parda si es el el que es mano me hace tirar a mi, fijate si con tirarprimerobot = true funciona.
                            //checkiar truco, retruco valecuatro, sumar punto cuando no quiero
                            Random random = new Random();
                            int numeroAleatorio = random.Next(1, 3); // Genera un número aleatorio entre 1 (inclusive) y 2 inclusive
                            int numeroAleatorioDos = random.Next(1, 3);
                            int numeroAleatorioTres = random.Next(1, 3);
                            int numeroAleatorioCuatro = random.Next(contadorTirada, 4);
                            if (numeroAleatorio == 1)
                            {
                                if (trucoCantado == 0)
                                {
                                    trucoCantado++;
                                    ReproducirAudioBot("truco");
                                    bool resultado = Task.Run(async () => await BotAsincrono("truco")).Result;
                                    if (resultado)
                                    {

                                        this.Invoke((MethodInvoker)delegate
                                        {
                                            btnTruco.Visible = false;
                                            btnEnvido.Visible = false;
                                            btnFlor.Visible = false;
                                            btnRetruco.Visible = true;
                                        });
                                        quisoTruco = true;
                                        btnRetruco.Click += (sender, e) =>
                                        {
                                            if (numeroAleatorioDos == 1)
                                            {
                                                quisoRetruco = true;
                                                ReproducirAudioBot("quiero");
                                                multiplicadorTruco = 3;
                                                if (numeroAleatorioTres == 1)
                                                {
                                                    ReproducirAudioBot("valecuatro");
                                                    quisoValecuatro = true;
                                                    bool resultadoDos = Task.Run(async () => await BotAsincrono("valecuatro")).Result;

                                                    if (resultadoDos)
                                                    {
                                                        quisoValecuatro = true;
                                                        multiplicadorTruco = 4;
                                                    }
                                                    else
                                                    {
                                                        multiplicadorTruco = 3;
                                                        puntosEnManoJugadorDos += 3;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                multiplicadorTruco = 2;
                                                ReproducirAudioBot("no quiero");
                                                puntosEnManoJugadorUno += 3;
                                            }
                                        };


                                        multiplicadorTruco = 2;
                                    }
                                    else
                                    {

                                        puntosEnManoJugadorDos += 3;
                                        puntajeDos++;
                                        MessageBox.Show("¡Tirá cualquier carta si deseas empezar otra mano!");
                                    }
                                }
                            }
                            else
                            {
                            }




                        }
                    }
                    if (primerJugador == jugadorUno || primerJugador == jugadorDos)
                    {
                        bool primerClicEnvido = true;
                        Random random = new Random();
                        int numeroAleatorioCinco = random.Next(1, 4);
                        int numeroAleatorioSeis = random.Next(1, 4);
                        int numeroAleatorioSiete = random.Next(1, 3);
                        string tipoEnvidoUno = CantarEnvidoProbabilistico();

                        btnMazo.Click += (sender, e) =>
                        {
                            if (quisoValecuatro)
                            {
                                puntosEnManoJugadorDos += 4;
                                MessageBox.Show("¡Tirá cualquier carta si deseas empezar otra mano!");
                                //puntajeDos += 3;
                                quisoRetruco = false;
                            }
                            if (quisoRetruco)
                            {
                                //MessageBox.Show("¡Tirá cualquier carta si deseas empezar otra mano!");
                                puntosEnManoJugadorDos += 4;
                                //puntajeDos += 2;
                                quisoTruco = false;
                            }
                            if (quisoTruco)
                            {
                                MessageBox.Show("¡Tirá cualquier carta si deseas empezar otra mano!");
                                puntosEnManoJugadorDos += 4;
                                //puntajeDos += 1;
                            }
                            else
                            {
                                MessageBox.Show("¡Tirá cualquier carta si deseas empezar otra mano!");
                                puntosEnManoJugadorDos += 4;
                                //puntajeDos++;
                            }

                        };
                        btnEnvido.Click += (sender, e) =>
                        {
                            if (envidoVisible)
                            {
                                btnTruco.Visible = true;
                                btnEnvido.Visible = true;
                                btnFlor.Visible = false;
                                btnRetruco.Visible = false;
                                btnMazo.Visible = false;
                                btnRealEnvido.Visible = true;
                                btnFaltaEnvido.Visible = true;

                            }
                            else
                            {
                                string audio = "";
                                SoundPlayer sonidoTruco = new SoundPlayer();

                                switch (personajeElegido.ToLower())
                                {
                                    case "maradona":
                                        audio = @"../../../../UI TRUCO/MaradonaEnvido/envido1.wav";
                                        break;
                                    case "messi":
                                        audio = @"../../../../UI TRUCO/MessiEnvido/envido1.wav";
                                        break;
                                    case "momo":
                                        audio = @"../../../../UI TRUCO/MomoEnvido/envido1.wav";
                                        break;
                                    default:
                                        break;

                                }
                                sonidoTruco.SoundLocation = audio;
                                sonidoTruco.Play();
                                // Cambiar la visibilidad de los botones al estado contrario
                                btnTruco.Visible = true;
                                btnEnvido.Visible = true;
                                btnFlor.Visible = true;
                                btnRetruco.Visible = false;
                                btnMazo.Visible = true;
                                btnRealEnvido.Visible = false;
                                btnFaltaEnvido.Visible = false;
                            }
                            btnRetruco.Visible = false;
                            // Cambiar el estado de visibilidad para la próxima vez que se haga clic en el botón
                            envidoVisible = !envidoVisible; if (primerClicEnvido)
                            {
                                primerClicEnvido = false;
                            }
                            else
                            {
                                if (tipoEnvidoUno == "envido")
                                {
                                    contadorEnvidos++;
                                }



                                //quiere el envido, puntaje 2
                                numeroAleatorioCinco = 1; 
                                if (numeroAleatorioCinco == 3)
                                {
                                    btnEnvido.Visible = false;
                                    btnRealEnvido.Visible = false;
                                    btnMazo.Visible = true;
                                    btnTruco.Visible = true;
                                    btnFaltaEnvido.Visible = false;
                                    btnFlor.Visible = false;
                                    sumadorPuntosEnvido += 2;
                                    envidoQuerido = true;
                                    Task.Delay(6500).Wait();
                                    ReproducirAudioBot("quiero");
                                    btnFaltaEnvido.Visible = false;
                                    JugadorPadre jugadorGanador = ManejarEnvidoGeneral(jugadorUno, jugadorDos, Cartas, puntajeUno, puntajeDos, tipoEnvidoUno, sumadorPuntosEnvido);


                                    if (jugadorGanador == null)
                                    {
                                        if (primerJugador == jugadorUno)
                                        {
                                            puntajeUno += sumadorPuntosEnvido;
                                        }
                                        else
                                        {
                                            puntajeDos += sumadorPuntosEnvido;
                                        }
                                    }
                                    else if (jugadorGanador == jugadorUno)
                                    {
                                        // Acciones si gana jugadorUno
                                        puntajeUno += sumadorPuntosEnvido;
                                    }
                                    else if (jugadorGanador == jugadorDos)
                                    {
                                        // Acciones si gana jugadorDos
                                        puntajeDos += sumadorPuntosEnvido;
                                    }
                                }
                                else if (numeroAleatorioCinco == 1) //canta envido, envido
                                {
                                    if (cantarUnaSolaVezCinco == 0)
                                    {
                                        cantarUnaSolaVezCinco++;
                                        envidoEnvidoQuerido = true;
                                        //poner para que solo sea una vez
                                        Task.Delay(6500).Wait();
                                        ReproducirAudioBot("envido");
                                        sumadoresEnvidos += 4;
                                        this.Invoke((MethodInvoker)delegate
                                        {
                                            //btnTruco.Visible = false;
                                            btnEnvido.Visible = false;
                                            btnQuieroo.Top -= 55;
                                            btnNoQuieroo.Top -= 55;
                                            btnNoQuieroo.Visible = true;
                                            btnQuieroo.Visible = true;
                                            btnRealEnvido.Visible = true;
                                            btnFaltaEnvido.Visible = true;
                                            btnFlor.Visible = false;
                                        });
                                        btnRealEnvido.Click += (sender, e) =>
                                        {
                                            if(cantarUnaSolaVezCinco == 0)
                                            {
                                                cantarUnaSolaVezCinco++;
                                                btnFaltaEnvido.Visible = false;
                                                ReproducirAudioBot("quiero");
                                                JugadorPadre jugadorGanador = ManejarEnvidoGeneral(jugadorUno, jugadorDos, Cartas, puntajeUno, puntajeDos, tipoEnvidoUno, sumadorPuntosEnvido);

                                                if (jugadorGanador == null)
                                                {
                                                    if (primerJugador == jugadorUno)
                                                    {
                                                        puntajeUno += 7;
                                                    }
                                                    else
                                                    {
                                                        puntajeDos += 7;
                                                    }
                                                }
                                                else if (jugadorGanador == jugadorUno)
                                                {
                                                    puntajeUno += 7;
                                                }
                                                else if (jugadorGanador == jugadorDos)
                                                {
                                                    puntajeDos += 7;
                                                }
                                            }
                                        };
                                        btnQuieroo.Click += (sender, e) => //quiero (YO) envido envido
                                        {
                                            btnRealEnvido.Visible = false;
                                            btnTruco.Visible = true;
                                            btnRetruco.Visible = false;
                                            btnFaltaEnvido.Visible = false;
                                            sumadorPuntosEnvido += 4;
                                            //preguntar una sola vez
                                            if (cantarUnaSolaVezCuatro == 0)
                                            {
                                                cantarUnaSolaVezCuatro++;
                                                JugadorPadre jugadorGanador = ManejarEnvidoGeneral(jugadorUno, jugadorDos, Cartas, puntajeUno, puntajeDos, tipoEnvidoUno, sumadorPuntosEnvido);

                                                if (jugadorGanador == null)
                                                {
                                                    if (primerJugador == jugadorUno)
                                                    {
                                                        puntajeUno += sumadorPuntosEnvido;
                                                    }
                                                    else
                                                    {
                                                        puntajeDos += sumadorPuntosEnvido;
                                                    }
                                                }
                                                else if (jugadorGanador == jugadorUno)
                                                {
                                                    puntajeUno += sumadorPuntosEnvido;
                                                }
                                                else if (jugadorGanador == jugadorDos)
                                                {
                                                    puntajeDos += sumadorPuntosEnvido;
                                                }
                                                envidoEnvidoQuerido = true;
                                            }
                                        };
                                        btnNoQuieroo.Click += (sender, e) =>
                                        {
                                            /// sumar puntajes
                                            puntajeDos += 2;
                                            btnFlor.Visible = false;
                                        };
                                    }
                                    else
                                    {
                                        puntajeUno += sumadoresEnvidos;
                                        ReproducirAudioBot("no quiero");
                                        btnRealEnvido.Visible = false;
                                        btnFaltaEnvido.Visible = false;
                                        btnEnvido.Visible = false;
                                        btnFlor.Visible = false;
                                        puntajeUno += 1;
                                    }
                                }
                            }
                        };
                        btnFaltaEnvido.Click += (sender, e) =>
                        {
                            btnQuieroo.Visible = false;
                            btnNoQuieroo.Visible = false;
                            btnEnvido.Visible = false;
                            btnRealEnvido.Visible = false;
                            btnFaltaEnvido.Visible = false;
                            btnMazo.Visible = true;
                            if (numeroAleatorioSiete == 1)
                            {
                                Task.Delay(6500).Wait();
                                ReproducirAudioBot("no quiero");
                                if (envidoEnvidoQuerido && realEnvidoQuerido)
                                {
                                    puntajeUno += 7;
                                }
                                else if (envidoEnvidoQuerido)
                                {
                                    puntajeUno += 4;
                                }
                                else if (realEnvidoQuerido)
                                {
                                    puntajeUno += 3;
                                }
                                else
                                {
                                    puntajeUno += 1;
                                }
                            }
                            else
                            {
                                Task.Delay(6500).Wait();
                                ReproducirAudioBot("quiero");
                                btnFaltaEnvido.Visible = false;
                                JugadorPadre jugadorGanador = ManejarEnvidoGeneral(jugadorUno, jugadorDos, Cartas, puntajeUno, puntajeDos, tipoEnvidoUno, sumadorPuntosEnvido);
                                if (jugadorGanador == null)
                                {
                                    if (primerJugador == jugadorUno)
                                    {
                                        puntajeUno += 30;
                                    }
                                    else
                                    {
                                        puntajeDos += 30;
                                    }
                                }
                                else if (jugadorGanador == jugadorUno)
                                {
                                    // Acciones si gana jugadorUno
                                    puntajeUno += 30;
                                }
                                else if (jugadorGanador == jugadorDos)
                                {
                                    // Acciones si gana jugadorDos
                                    puntajeDos += 30;
                                }
                            }
                        };
                        btnRealEnvido.Click += (sender, e) =>
                        {
                            sumadoresEnvidos += 2;
                            btnQuieroo.Visible = false;
                            btnNoQuieroo.Visible = false;
                            //poner para que solo sea una vez
                            btnFlor.Visible = false;
                            Task.Delay(6500).Wait();
                            btnRealEnvido.Visible = false;
                            btnMazo.Visible = true;
                            btnTruco.Visible = true;
                            //numeroAleatorioSeis = 1;
                            if (numeroAleatorioSeis == 1)
                            {
                                ReproducirAudioBot("quiero");
                                btnFaltaEnvido.Visible = false;
                                sumadorPuntosEnvido += 3;
                                JugadorPadre jugadorGanador = ManejarEnvidoGeneral(jugadorUno, jugadorDos, Cartas, puntajeUno, puntajeDos, tipoEnvidoUno, sumadorPuntosEnvido);
                                if (jugadorGanador == null)
                                {
                                    // Realizar acciones en caso de empate
                                    //sumar puntos cuando no quiero envido
                                }
                                else if (jugadorGanador == jugadorUno)
                                {
                                    // Acciones si gana jugadorUno
                                    if (envidoEnvidoQuerido)
                                    {
                                        puntajeUno += 7;
                                    }
                                    else
                                    {
                                        puntajeUno += sumadorPuntosEnvido;
                                    }
                                }
                                else if (jugadorGanador == jugadorDos)
                                {
                                    // Acciones si gana jugadorUno
                                    if (envidoEnvidoQuerido)
                                    {
                                        puntajeDos += 7;
                                    }
                                    else
                                    {
                                        puntajeDos += sumadorPuntosEnvido;
                                    }
                                }
                                realEnvidoQuerido = true;
                                btnFaltaEnvido.Visible = false;
                                btnMazo.Visible = true;
                            }
                            else if (numeroAleatorioSeis == 2)
                            {
                                sumadoresEnvidos += 1;
                                btnFaltaEnvido.Visible = false;
                                btnMazo.Visible = true;
                                ReproducirAudioBot("falta envido");
                                //btnTruco.Visible = false;
                                btnEnvido.Visible = false;
                                btnQuieroo.Top -= 55;
                                btnNoQuieroo.Top -= 55;
                                btnRealEnvido.Visible = false;
                                btnFaltaEnvido.Visible = false;
                                btnNoQuieroo.Visible = true;
                                btnQuieroo.Visible = true;

                                btnQuieroo.Click += (sender, e) =>
                                {
                                    sumadorPuntosEnvido += 30;
                                    JugadorPadre jugadorGanador = ManejarEnvidoGeneral(jugadorUno, jugadorDos, Cartas, puntajeUno, puntajeDos, tipoEnvidoUno, sumadorPuntosEnvido);
                                    if (jugadorGanador == null)
                                    {
                                        if (primerJugador == jugadorUno)
                                        {
                                            puntajeUno += sumadorPuntosEnvido;
                                        }
                                        else
                                        {
                                            puntajeDos += sumadorPuntosEnvido;
                                        }
                                    }
                                    else if (jugadorGanador == jugadorUno)
                                    {
                                        // Acciones si gana jugadorUno
                                        puntajeUno += sumadorPuntosEnvido;
                                    }
                                    else if (jugadorGanador == jugadorDos)
                                    {
                                        // Acciones si gana jugadorDos
                                        puntajeDos += sumadorPuntosEnvido;
                                    }
                                    faltaEnvidoQuerido = true;
                                };
                                btnNoQuieroo.Click += (sender, e) =>
                                {
                                    if (envidoEnvidoQuerido && realEnvidoQuerido)
                                    {
                                        puntajeDos += 7;
                                    }
                                    else if (envidoEnvidoQuerido)
                                    {
                                        puntajeDos += 4;
                                    }
                                    else if (realEnvidoQuerido)
                                    {
                                        puntajeDos += 3;
                                    }
                                    else
                                    {
                                        puntajeDos += 1;
                                    }

                                };
                            }
                            else if (numeroAleatorioCinco == 2)
                            {
                                puntajeUno += sumadoresEnvidos;
                                ReproducirAudioBot("no quiero");
                                btnRealEnvido.Visible = false;
                                btnFaltaEnvido.Visible = false;
                                btnMazo.Visible = true;
                                //sumar puntajes
                            }
                        };

                        if (quisoTruco == false)
                        {

                            btnTruco.Click += (sender, e) =>
                            {
                                quisoTruco = true;
                                string canto = ManejarTrucoBot(Cartas[3].valor, Cartas[4].valor, Cartas[5].valor);

                                if (cantarUnaSolaVez == 0)
                                {
                                    cantarUnaSolaVez++;
                                    Task.Delay(6500).Wait();
                                }
                                if (canto == "truco" || canto == "retruco" || canto == "valecuatro")
                                {

                                    ReproducirAudioBot("quiero");
                                    multiplicadorTruco = 2;
                                    this.Invoke((MethodInvoker)delegate
                                        {
                                            btnEnvido.Visible = false;
                                            btnTruco.Visible = false;
                                            btnRetruco.Visible = false;
                                            btnFlor.Visible = false;
                                            btnValecuatro.Visible = false;
                                            btnMazo.Visible = true;
                                        });
                                    if (canto == "retruco" || canto == "valecuatro")
                                    {

                                        ReproducirAudioBot("retruco");
                                        MostrarBotonesQuieroNoQuiero();
                                        btnQuieroo.Click += (sender, e) =>
                                            {
                                                btnEnvido.Visible = false;
                                                btnTruco.Visible = false;
                                                btnRetruco.Visible = false;
                                                btnFlor.Visible = false;
                                                btnValecuatro.Visible = true;
                                                btnMazo.Visible = true;
                                                quisoRetruco = true;
                                                multiplicadorTruco = 3;
                                                btnValecuatro.Click += (sender, e) =>
                                                {
                                                    if (canto == "valecuatro")
                                                    {
                                                        quisoValecuatro = true;
                                                        quisoRetruco = false;
                                                        Task.Delay(6500).Wait();
                                                        ReproducirAudioBot("quiero");
                                                        multiplicadorTruco = 4;
                                                        btnValecuatro.Visible = false;
                                                    }
                                                    else
                                                    {
                                                        Task.Delay(6500).Wait();
                                                        ReproducirAudioBot("no quiero");
                                                        multiplicadorTruco = 3;
                                                        puntosEnManoJugadorDos += 3;
                                                        MessageBox.Show("¡Tirá cualquier carta si deseas empezar otra mano!");
                                                    }
                                                };

                                            };
                                        btnNoQuieroo.Click += (sender, e) =>
                                            {
                                                quisoRetruco = true;
                                                btnEnvido.Visible = false;
                                                btnTruco.Visible = false;
                                                btnRetruco.Visible = false;
                                                btnFlor.Visible = false;
                                                btnValecuatro.Visible = false;
                                                btnMazo.Visible = true;
                                                multiplicadorTruco = 2;
                                                puntosEnManoJugadorDos += 3;
                                                MessageBox.Show("¡Tirá cualquier carta si deseas empezar otra mano!");
                                            };
                                    }
                                }
                                else
                                {
                                    if (unaSolaVez == 0)
                                    {
                                        multiplicadorTruco = 1;
                                        unaSolaVez++;
                                        ReproducirAudioBot("no quiero");
                                        puntosEnManoJugadorUno += 3;
                                        MessageBox.Show("¡Tirá cualquier carta si deseas empezar otra mano!");
                                    }
                                }
                            };
                        }
                        else
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                btnTruco.Visible = false;
                            });
                        }
                        if (Cartas[3].valor == -1 && Cartas[4].valor != -1 && Cartas[5].valor != -1)
                        {
                            contadorTirado++;
                            preguntarPorPrimeraVez++;
                            primeraTirada = true;
                        }
                        else if (Cartas[3].valor != -1 && Cartas[4].valor == -1 && Cartas[5].valor != -1)
                        {
                            contadorTirado++;
                            preguntarPorPrimeraVez++;
                            primeraTirada = true;
                        }
                        else if (Cartas[3].valor != -1 && Cartas[4].valor != -1 && Cartas[5].valor == -1)
                        {
                            contadorTirado++;
                            preguntarPorPrimeraVez++;
                            primeraTirada = true;
                        }

                        if (preguntarPorPrimeraVez == 0)
                        {
                            preguntarPorPrimeraVez++;
                        }
                        segundaManoPrimerJugador = true;



                        if (tirarPrimeroBot)
                        {
                            primeraTirada = false;
                            int numeroAleatorio = new Random().Next(39, 52);
                            cartaATirar = ManejarTiradaBot(Cartas[3].valor, Cartas[4].valor, Cartas[5].valor, numeroAleatorio);
                            MoverYReducirBotSegunCarta(cartaATirar, Cartas);
                            if (Cartas[3].valor == -1)
                            {
                                primerCartaSj.ImageLocation = Cartas[3].imagen;
                            }
                            if (Cartas[4].valor == -1)
                            {
                                segundaCartaSj.ImageLocation = Cartas[4].imagen;
                            }
                            if (Cartas[5].valor == -1)
                            {
                                terceraCartaSj.ImageLocation = Cartas[5].imagen;
                            }
                            int cartaTirada = CartaTiradaPorJugadorUno(Cartas);
                            ganador = carta.ManejarQuienGanaTirada(jugadorUno, jugadorDos, cartaTirada, cartaATirar);
                            if (ganador == 2)
                            {
                                ganador = 0;
                            }
                            else if (ganador == 1)
                            {
                                ganador = 1;
                            }
                            else
                            {
                                ganador = 2;
                            }

                        }
                        else
                        {
                            ganador = ManejarMano(Cartas);
                            if (Cartas[3].valor == -1)
                            {
                                primerCartaSj.ImageLocation = Cartas[3].imagen;
                                contadorTirado++;
                            }
                            if (Cartas[4].valor == -1)
                            {
                                segundaCartaSj.ImageLocation = Cartas[4].imagen;
                                contadorTirado++;
                            }
                            if (Cartas[5].valor == -1)
                            {
                                contadorTirado++;
                                terceraCartaSj.ImageLocation = Cartas[5].imagen;
                            }
                        }

                        if (ganador == 2) //empate
                        {
                            contadorTirada++;
                            tirarPrimeroBot = false;

                            if (puntosEnManoJugadorDos == 1 && puntosEnManoJugadorUno == 1)
                            {
                                if (primerJugador == jugadorUno)
                                {
                                    puntosEnManoJugadorUno += 3;
                                }
                                else
                                {
                                    puntosEnManoJugadorDos += 3;
                                }
                            }

                            if (puntosEnManoJugadorUno == 1)
                            {
                                puntosEnManoJugadorUno += 3;
                            }
                            else if (puntosEnManoJugadorDos == 1)
                            {
                                puntosEnManoJugadorDos += 3;

                            }



                        }
                        else if (ganador == 1) //gana jugador uno
                        {
                            tirarPrimeroBot = false;
                            if (primeraTirada)
                            {
                                seEmpatoPrimeraTirada = true;

                            }
                            puntosEnManoJugadorUno++;
                            if (seEmpatoPrimeraTirada)
                            {
                                puntosEnManoJugadorUno += 3;
                            }
                            primerJugador = jugadorUno;



                        }

                        else if (ganador == 0) //gana jugador dos
                        {
                            if (puntosEnManoJugadorUno == 0 && puntosEnManoJugadorDos == 0 && contadorTirada > 0)
                            {
                                puntosEnManoJugadorDos += 3;
                            }
                            tirarPrimeroBot = true;
                            puntosEnManoJugadorDos++;
                        }
                        ////puntos 

                        if (puntosEnManoJugadorUno > 1)
                        {
                            puntajeUno += multiplicadorTruco;
                            finalizarMano = false;
                        }
                        else if (puntosEnManoJugadorDos > 1)
                        {
                            puntajeDos += multiplicadorTruco;
                            finalizarMano = false;
                        }
                    }
                    else
                    {
                        noEsTurnoPrimerJugador = true;
                        segundaManoPrimerJugador = true;
                    }
                }
                if (puntajeUno > puntajeDos)
                {
                    MessageBox.Show("¡¡Haz ganado!!");
                    Application.Exit();

                }

                else
                {
                    MessageBox.Show("Haz perdido :(");
                    Application.Exit();
                }
            }

        }

        private JugadorPadre ManejarEnvidoGeneral(JugadorPadre jugadorUno, JugadorPadre jugadorDos, List<Carta> Cartas, int puntajeUno, int puntajeDos, string tipoEnvidoUno, int puntos)
        {
            //error aca...
            List<int> numerosMismoPalo = ObtenerNumerosMismoPalo(Cartas);
            int puntosEnvido = CalcularPuntajeEnvido(numerosMismoPalo[0], numerosMismoPalo[1]); // Jugador 1
            int puntosEnvidoDos = CalcularPuntajeEnvido(numerosMismoPalo[2], numerosMismoPalo[3]); // Jugador 2

            if (puntosEnvido == 0)
            {
                // Calcula los puntos del Envido para el primer jugador
                puntosEnvido = Math.Max(Math.Max(Cartas[0].numero <= 7 ? Cartas[0].numero : 0,
                                                  Cartas[1].numero <= 7 ? Cartas[1].numero : 0),
                                        Cartas[2].numero <= 7 ? Cartas[2].numero : 0);
            }

            if (puntosEnvidoDos == 0)
            {
                // Calcula los puntos del Envido para el segundo jugador
                puntosEnvidoDos = Math.Max(Math.Max(Cartas[3].numero <= 7 ? Cartas[3].numero : 0,
                                                     Cartas[4].numero <= 7 ? Cartas[4].numero : 0),
                                            Cartas[5].numero <= 7 ? Cartas[5].numero : 0);
            }

            JugadorPadre ganadorEnvido = null;

            if (puntosEnvido > puntosEnvidoDos)
            {
                ganadorEnvido = jugadorUno;
            }
            else if (puntosEnvido < puntosEnvidoDos)
            {
                ganadorEnvido = jugadorDos;
            }

            if (ganadorEnvido == null)
            {
                MessageBox.Show("¡Empate! :(\n" +
                                 "Él: " + puntosEnvidoDos + "\n" +
                                 "Vos: " + puntosEnvido);
            }
            else
            {
                string mensaje;

                if (ganadorEnvido == jugadorUno)
                {
                    mensaje = "¡Ganaste!\n";

                    puntajeUno += puntos;
                }
                else // ganadorEnvido == jugadorDos
                {
                    mensaje = "¡Perdiste! :(\n";

                    puntajeDos += puntos;
                }

                MessageBox.Show(mensaje + "Él: " + puntosEnvidoDos + "\n" +
                                "Vos: " + puntosEnvido);

            }

            return ganadorEnvido;
        }

        private string CantarEnvidoProbabilistico()
        {
            Random rand = new Random();
            int probabilidad = rand.Next(1, 101); // Generar un número aleatorio entre 1 y 100

            if (probabilidad <= 60)
            {
                return "envido"; // Cantar Envido con un 60% de probabilidad
            }
            else if (probabilidad <= 80)
            {
                return "real envido"; // Cantar Real Envido con un 20% de probabilidad
            }
            else
            {
                return "falta envido"; // Cantar Falta Envido con un 10% de probabilidad
            }
        }


        private async Task<bool> BotAsincrono(string cantoBot)
        {
            // Esperar 2 segundos
            await Task.Delay(2000);

            // Llamar al método CantarEsperarBot después de esperar 2 segundos
            bool canto = CantarEsperarBot(cantoBot);
            return canto;

        }
        private bool CantarEsperarBot(string cantoBot)
        {
            //ReproducirAudioBot(cantoBot);
            MostrarBotonesQuieroNoQuiero();

            // Deshabilitar otros controles o lógica mientras se espera la respuesta del usuario
            DeshabilitarControles();

            // Crear un objeto TaskCompletionSource para esperar el clic del usuario
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            // Manejadores de eventos para los clics en los botones
            EventHandler clicQuieroHandler = null;
            EventHandler clicNoQuieroHandler = null;

            clicQuieroHandler = (sender, e) =>
            {
                banderaQuiero = true;

                // Establecer el resultado de la tarea como true y quitar los manejadores de eventos
                tcs.SetResult(true);
                btnQuieroo.Click -= clicQuieroHandler;
                btnNoQuieroo.Click -= clicNoQuieroHandler;
            };

            clicNoQuieroHandler = (sender, e) =>
            {
                banderaNoQuiero = true;

                // Establecer el resultado de la tarea como false y quitar los manejadores de eventos
                tcs.SetResult(false);
                btnQuieroo.Click -= clicQuieroHandler;
                btnNoQuieroo.Click -= clicNoQuieroHandler;
            };

            // Asociar manejadores de eventos a los botones
            btnQuieroo.Click += clicQuieroHandler;
            btnNoQuieroo.Click += clicNoQuieroHandler;

            // Esperar hasta que la tarea se complete (hasta que el usuario haga clic en uno de los botones)
            while (!tcs.Task.IsCompleted)
            {
                Application.DoEvents(); // Permitir que la aplicación siga respondiendo
            }

            // Reactivar otros controles o lógica después de que se completa la tarea
            HabilitarControles();

            // Devolver el resultado de la tarea cuando el usuario hace clic en uno de los botones
            return tcs.Task.Result;
        }

        private void DeshabilitarControles()
        {
            // Invocar en el hilo de la interfaz de usuario para deshabilitar los controles
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    DeshabilitarControles();
                });
            }
            else
            {
                btnEnvido.Visible = false;
                btnTruco.Visible = false;
                btnMazo.Visible = false;
                btnFlor.Visible = false;
            }
        }

        private void HabilitarControles()
        {
            // Invocar en el hilo de la interfaz de usuario para habilitar los controles
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    HabilitarControles();
                });
            }
            else
            {
                btnEnvido.Visible = true;
                btnTruco.Visible = true;
                btnMazo.Visible = true;
                btnFlor.Visible = true;
            }
        }



        private string TomarDecisionBot(int cartaUno, int cartaDos, int cartaTres)
        {
            Random random = new Random();
            int decision = random.Next(1, 3); // Generar un número aleatorio entre 1 y 2

            //decision = 1;
            if (decision == 1)
            {
                return "cantar truco";
            }
            else if (decision == 2)
            {
                return "cantar envido";
            }

            return "";

        }
        private void RestablecerCartas()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    RestablecerCartas();
                });
            }
            else
            {
                // Código para restablecer el estado de las cartas
                primerCarta.Enabled = true;
                segundaCarta.Enabled = true;
                tercerCarta.Enabled = true;
                // Otras acciones si es necesario...
            }
        }

        //solucionar quien tira primero, como se tira, etc. despues recien se puede hacer todo
        private int ManejarMano(List<Carta> Cartas)
        {
            JugadorDos jugadorDos = new JugadorDos();
            JugadorUno jugadorUno = new JugadorUno();

            Carta carta = new Carta();

            int cartaTirada = CartaTiradaPorJugadorUno(Cartas);

            int cartaATirar = ManejarTiradaBot(Cartas[3].valor, Cartas[4].valor, Cartas[5].valor, cartaTirada);
            string canto = ManejarTrucoBot(Cartas[3].valor, Cartas[4].valor, Cartas[5].valor);



            MoverYReducirBotSegunCarta(cartaATirar, Cartas);
            int ganador = carta.ManejarQuienGanaTirada(jugadorUno, jugadorDos, cartaTirada, cartaATirar);

            if (ganador == 1)
            {
                //MessageBox.Show("Ganador: Jugador numero uno");
                return 1;
            }
            else if (ganador == 2)
            {
                // MessageBox.Show("Ganador: Jugador numero dos");
                return 0;
            }
            else
            {
                // MessageBox.Show("EMPATE");
                return 2;
            }


        }
        private void ResetearPosicionesPictureBox()
        {
            if (primerCarta.InvokeRequired)
            {
                primerCarta.Invoke(new MethodInvoker(() =>
                {
                    primerCarta.Location = new Point(416, 456);
                    primerCarta.Size = new Size(128, 185);
                }));
            }
            else
            {
                primerCarta.Location = new Point(416, 456);
                primerCarta.Size = new Size(128, 185);
            }

            if (segundaCarta.InvokeRequired)
            {
                segundaCarta.Invoke(new MethodInvoker(() =>
                {
                    segundaCarta.Location = new Point(596, 456);
                    segundaCarta.Size = new Size(128, 185);
                }));
            }
            else
            {
                segundaCarta.Location = new Point(596, 456);
            }

            if (tercerCarta.InvokeRequired)
            {
                tercerCarta.Invoke(new MethodInvoker(() =>
                {
                    tercerCarta.Location = new Point(784, 456);
                    tercerCarta.Size = new Size(128, 185);
                }));
            }
            else
            {
                tercerCarta.Location = new Point(784, 456);
            }
            if (primerCartaSj.InvokeRequired)
            {
                primerCartaSj.Invoke(new MethodInvoker(() =>
                {
                    primerCartaSj.Location = new Point(416, 62);
                    primerCartaSj.Size = new Size(128, 185);
                }));
            }
            else
            {
                primerCartaSj.Location = new Point(416, 62);
                primerCartaSj.Size = new Size(128, 185);
            }

            if (segundaCartaSj.InvokeRequired)
            {
                segundaCartaSj.Invoke(new MethodInvoker(() =>
                {
                    segundaCartaSj.Location = new Point(596, 62);
                    segundaCartaSj.Size = new Size(128, 185);
                }));
            }
            else
            {
                segundaCartaSj.Location = new Point(596, 62);
                segundaCarta.Size = new Size(128, 185);
            }

            if (terceraCartaSj.InvokeRequired)
            {
                terceraCartaSj.Invoke(new MethodInvoker(() =>
                {
                    terceraCartaSj.Location = new Point(784, 62);
                    terceraCartaSj.Size = new Size(128, 185);
                }));
            }
            else
            {
                terceraCartaSj.Location = new Point(784, 62);
                terceraCartaSj.Size = new Size(128, 185);

            }

        }


        private void MoverYReducirBotSegunCarta(int cartaATirar, List<Carta> Cartas)
        {
            if (Cartas[3].valor != -1 && cartaATirar == Cartas[3].valor)
            {
                MoverYReducirBotConRetraso("primerCartaSj");
                Cartas[3].valor = -1; // Marcar la carta como utilizada
            }
            else if (Cartas[4].valor != -1 && cartaATirar == Cartas[4].valor)
            {
                MoverYReducirBotConRetraso("segundaCartaSj");
                Cartas[4].valor = -1; // Marcar la carta como utilizada
            }
            else if (Cartas[5].valor != -1 && cartaATirar == Cartas[5].valor)
            {
                MoverYReducirBotConRetraso("terceraCartaSj");
                Cartas[5].valor = -1; // Marcar la carta como utilizada
            }
        }

        private void MoverYReducirBotConRetraso(string nombreCarta)
        {
            Thread.Sleep(1000); // Esperar un segundo (1000 milisegundos) antes de realizar la acción

            // Llamar al método para mover y reducir la carta del bot
            MoverYReducirBot(nombreCarta);
        }




        private int CartaTiradaPorJugadorUno(List<Carta> Cartas)
        {
            ManualResetEvent waitEvent = new ManualResetEvent(false);
            int valorCarta = -1; // Inicializado con un valor que no corresponde a ninguna carta

            // Evento Click para la primera carta
            primerCarta.Click += (sender, e) =>
            {
                // Obtener el valor de la carta y almacenarlo en la variable
                valorCarta = Cartas[0].valor;
                waitEvent.Set(); // Indicar que se completó la operación
            };

            // Evento Click para la segunda carta
            segundaCarta.Click += (sender, e) =>
            {
                // Obtener el valor de la carta y almacenarlo en la variable
                valorCarta = Cartas[1].valor;
                waitEvent.Set(); // Indicar que se completó la operación
            };

            // Evento Click para la tercera carta
            tercerCarta.Click += (sender, e) =>
            {
                // Obtener el valor de la carta y almacenarlo en la variable
                valorCarta = Cartas[2].valor;
                waitEvent.Set(); // Indicar que se completó la operación
            };

            // Esperar hasta que se complete la operación (se haga clic en una carta)
            waitEvent.WaitOne();

            // Resto del código si es necesario...

            return valorCarta;
        }


        private JugadorPadre ManejarTruco(JugadorUno j1, JugadorDos j2, int valorCartaJugadorUno, int valorCartaJugadorDos)
        {
            Carta carta = new Carta();
            bool seguirMano = true;
            int jugadorUno = 0;
            int jugadorDos = 0;

            while (seguirMano)
            {
                //esto son las manos, si los jugadores ganan dos manos se corta el bucle
                if (jugadorUno > 1 || jugadorDos > 1)
                {
                    int ganador = carta.ManejarQuienGanaTirada(j1, j2, valorCartaJugadorUno, valorCartaJugadorDos);

                    if (ganador == 0)
                    {
                        jugadorUno++;
                        jugadorDos++;
                    }
                    else if (ganador == 1)
                    {
                        jugadorUno++;
                    }
                    else
                    {
                        jugadorDos++;
                    }
                }
                else
                {
                    seguirMano = false;
                }

            }

            if (jugadorUno > jugadorDos)
            {
                return j1;
            }
            else if (jugadorUno < jugadorDos)
            {
                return j2;
            }

            // En caso de empate o si el bucle no se ejecutó, devolvemos null
            return null;


        }
        private JugadorPadre ManejarGanadorEnvido(JugadorUno j1, JugadorDos j2, int valorCartaUnoJugadorUno, int valorCartaDosJugadorUno, int valorCartaUnoJugadorDos, int valorCartaDosJugadorDos)
        {
            //devuelvo el jugador que gano, despues, sumo para ver los puntajes en el otro lado
            int puntajeEnvidoJugadorUno = CalcularPuntajeEnvido(valorCartaUnoJugadorUno, valorCartaDosJugadorUno);
            int puntajeEnvidoJugadorDos = CalcularPuntajeEnvido(valorCartaUnoJugadorDos, valorCartaDosJugadorDos);

            if (puntajeEnvidoJugadorUno > puntajeEnvidoJugadorDos)
            {
                return j1;
            }
            else if (puntajeEnvidoJugadorUno == puntajeEnvidoJugadorDos)
            {
                return null;
            }
            else
            {
                return j2;
            }
        }

        private static int CalcularPuntajeEnvido(int carta1, int carta2)
        {
            //arreglar cartas repetidas
            // Verificar si alguna carta es menor a 1
            if (carta1 < 1 || carta2 < 1)
            {
                return 0;
            }

            // Verificar si al menos una de las cartas es menor a 10
            if (carta1 < 10 || carta2 < 10)
            {
                // Verificar si ambas cartas son menores a 10
                if (carta1 < 10 && carta2 < 10)
                {
                    return 20 + carta1 + carta2;
                }
                else
                {
                    // Si alguna de las cartas es mayor o igual a 10, sumar solo la menor
                    return 20 + Math.Min(carta1, carta2);
                }
            }
            else if ((carta1 == 12 && carta2 == 10) || (carta1 == 10 && carta2 == 12))
            {
                return 20;
            }
            else if ((carta1 == 11 && carta2 == 10) || (carta1 == 10 && carta2 == 11))
            {
                return 20;
            }
            else if ((carta1 == 12 && carta2 == 11) || (carta1 == 11 && carta2 == 12))
            {
                return 20;
            }

            return 0; // Valor por defecto si no se cumple ninguna condición
        }




        private int ManejarTiradaBot(int valorCartaUno, int valorCartaDos, int valorCartaTres, int valorCartaTirada)
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

            // Asegurarse de que la carta seleccionada no sea -1
            //en la primer mano si le tiro la mas grande me tira la del medio
            if (cartaMasAlta != -1 && cartaMasAlta < valorCartaTirada)
            {
                if (cartaMasBaja != -1)
                {
                    return cartaMasBaja;
                }
                else if (cartaDelMedio != -1)
                {
                    return cartaDelMedio;
                }
                else { return cartaMasAlta; }
            }
            else if (cartaDelMedio != -1 && cartaDelMedio > valorCartaTirada)
            {
                if (cartaMasBaja != -1 && cartaMasBaja > valorCartaTirada)
                { return cartaMasBaja; }
                return cartaDelMedio;
            }
            else if (cartaMasAlta != -1 && cartaDelMedio <= valorCartaTirada)
            {
                return cartaMasAlta;
            }
            else
            {
                if (cartaDelMedio != -1)
                {
                    return cartaDelMedio;
                }
                else if (cartaMasBaja != -1)
                {
                    return cartaMasBaja;
                }
                else if (cartaMasAlta != -1)
                {
                    return cartaMasAlta;
                }
                else { return -1; }
            }
        }


        private bool TerminarMano()
        {
            return false;
        }
        private bool TerminarPartida(int p1, int p2)
        {
            if (p1 == 30 || p2 == 30)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        private string ManejarEnvidoBot(int cartaEnvidoUno, int cartaEnvidoDos)
        {
            int puntosEnvido = CalcularPuntajeEnvido(cartaEnvidoUno, cartaEnvidoDos);
            if (puntosEnvido > 23 && puntosEnvido < 26)
            {
                return "envido";
            }
            else if (puntosEnvido > 26 && puntosEnvido < 28)
            {
                return "real envido";
            }
            else if (puntosEnvido > 28)
            {
                return "falta envido";
            }
            else
            {
                return "no quiero";
            }
        }

        private string ManejarTrucoBot(int valorCartaUno, int valorCartaDos, int valorCartaTres)
        {
            int sumaCartas = valorCartaUno + valorCartaDos + valorCartaTres;
            float promedioCartas = sumaCartas / 3;

            // Reducir aún más los rangos para hacer más fácil que el bot cante truco, retruco y valecuatro
            if (promedioCartas > 38 && promedioCartas < 43)
            {
                return "truco";
            }
            else if (promedioCartas > 40 && promedioCartas < 45)
            {
                return "retruco";
            }
            else if (promedioCartas > 42)
            {
                return "valecuatro";
            }
            else
            {
                return "no quiero";
            }
        }


        private List<Carta> AsignarImagenCarta()
        {
            //en este metodo asigno la imagen a cada carta y al mismo tiempo retorno las cartas que se repartieron,
            //las 3 primeras son del primer jugador
            // y las ultimas 3 son del bot\
            List<Carta> mazoCartas = Carta.RepartirMano();

            primerCarta.ImageLocation = mazoCartas[0].imagen;
            segundaCarta.ImageLocation = mazoCartas[1].imagen;
            tercerCarta.ImageLocation = mazoCartas[2].imagen;

            // Asignar las tres últimas cartas al segundo jugador
            //primerCartaSj.ImageLocation = mazoCartas[3].imagen;
            //segundaCartaSj.ImageLocation = mazoCartas[4].imagen;
            //terceraCartaSj.ImageLocation = mazoCartas[5].imagen;
            
            primerCartaSj.ImageLocation = @"../../../../ManejadorCartas/ImagenesCartas/default.jpeg";
            segundaCartaSj.ImageLocation = @"../../../../ManejadorCartas/ImagenesCartas/default.jpeg";
            terceraCartaSj.ImageLocation = @"../../../../ManejadorCartas/ImagenesCartas/default.jpeg";
            
            return mazoCartas;
        }

        private int Carta_Click(object sender, EventArgs e, int valorCarta)
        {
            return valorCarta;
        }
        private void RedondearImagen(PictureBox pictureBox)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, pictureBox.Width, pictureBox.Height);
            pictureBox.Region = new Region(path);
        }

        private string ObtenerFoto()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Todos los archivos|*.*";
                openFileDialog.Title = "Seleccionar una foto";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private void frmPartidaTruco_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hiloPartida != null && hiloPartida.IsAlive)
            {
                hiloPartida.Abort();
            }
            Application.Exit();
        }
        private void btnTruco_Click_1(object sender, EventArgs e)
        {
            btnEnvido.Visible = false;
            btnMazo.Visible = true;
            btnFlor.Visible = false;
            btnRealEnvido.Visible = false;
            btnFaltaEnvido.Visible = false;

            // Verificar si es el turno de jugadorUno
            if (noEsTurnoPrimerJugador)
            {
                MessageBox.Show("No es tu turno");
                return;
            }
            else
            {
                ReproducirAudio();
            }

        }

        private string DevolverRandomAudio(string[] audios)
        {
            Random audioRandom = new Random();

            List<string> audiosLista = new List<string>(audios);

            if (audiosLista.Count == 0)
            {
                return null;
            }

            int indiceAleatorio = audioRandom.Next(audiosLista.Count);

            return audiosLista[indiceAleatorio];
        }

        private void ReproducirAudio()
        {
            string[] sonidos = ObtenerSonidosSegunPersonaje();

            if (sonidos != null && sonidos.Length > 0)
            {
                string audio = DevolverRandomAudio(sonidos);
                SoundPlayer sonidoTruco = new SoundPlayer();
                sonidoTruco.SoundLocation = audio;
                sonidoTruco.Play();
            }
        }

        private void ReproducirAudioBot(string canto)
        {

            string audio = "";
            SoundPlayer sonidoTruco = new SoundPlayer();

            if (canto == "truco")
            {
                audio = @"../../../../UI TRUCO/CantoBot/truco.wav";
            }
            else if (canto == "retruco")
            {
                audio = @"../../../../UI TRUCO/CantoBot/retruco.wav";
            }
            else if (canto == "valecuatro")
            {
                audio = @"../../../../UI TRUCO/CantoBot/valecuatro.wav";
            }
            else if (canto == "no quiero")
            {
                audio = @"../../../../UI TRUCO/CantoBot/noQuiero.wav";
            }
            else if (canto == "quiero")
            {
                audio = @"../../../../UI TRUCO/CantoBot/quiero.wav";
            }
            else if (canto == "envido")
            {
                audio = @"../../../../UI TRUCO/CantoBot/envido.wav";
            }
            else if (canto == "real envido")
            {
                audio = @"../../../../UI TRUCO/CantoBot/real envido.wav";
            }
            else if (canto == "falta envido")
            {
                audio = @"../../../../UI TRUCO/CantoBot/faltaEnvido.wav";

            }
            else if (canto == "mazo")
            {
                audio = @"../../../../UI TRUCO/CantoBot/mazo.wav";

            }
            else
            {
                audio = @"../../../../UI TRUCO/CantoBot/felicitaciones.wav";
            }

            sonidoTruco.SoundLocation = audio;
            sonidoTruco.Play();

        }

        private string[] ObtenerSonidosSegunPersonaje()
        {
            switch (personajeElegido.ToLower())
            {
                case "maradona":
                    return trucoMaradona;
                case "messi":
                    return trucoMessi;
                case "luisito":
                    return trucoLuisito;
                case "momo":
                    return trucoMomo;
                default:
                    return null;
            }
        }

        private void btnTruco_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand; // Cambia el cursor a una manito
        }

        private void btnTruco_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default; // Cambia el cursor de vuelta al cursor predeterminado
        }


        private void btnEnvido_Click(object sender, EventArgs e)
        {
            
            


        }

        private void btnEnvido_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand; // Cambia el cursor a una manito
        }

        private void btnEnvido_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default; // Cambia el cursor de vuelta al cursor predeterminado

        }

        private void btnFlor_Click(object sender, EventArgs e)
        {
            // Verificar si es el turno de jugadorUno
            if (noEsTurnoPrimerJugador)
            {
                MessageBox.Show("No es tu turno");
                return;
            }
            string audio = "";
            SoundPlayer sonidoTruco = new SoundPlayer();

            switch (personajeElegido.ToLower())
            {
                case "maradona":
                    audio = @"../../../../UI TRUCO/MaradonaFlor/flor1.wav";
                    break;
                case "messi":
                    audio = @"../../../../UI TRUCO/MessiFlor/flor1.wav";
                    break;
                case "momo":
                    audio = @"../../../../UI TRUCO/MomoFlor/flor1.wav";
                    break;
                default:
                    break;

            }
            sonidoTruco.SoundLocation = audio;
            sonidoTruco.Play();
        }

        private void btnFlor_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand; // Cambia el cursor a una manito
        }

        private void btnFlor_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void btnMazo_Click(object sender, EventArgs e)
        {
            banderaMazo = true;
            string audio = "";
            SoundPlayer sonidoTruco = new SoundPlayer();

            switch (personajeElegido.ToLower())
            {
                case "maradona":
                    audio = @"../../../../UI TRUCO/MaradonaMazo/mazo1.wav";
                    break;
                case "messi":
                    audio = @"../../../../UI TRUCO/MessiMazo/mazo1.wav";
                    break;
                case "momo":
                    audio = @"../../../../UI TRUCO/MomoMazo/mazo1.wav";
                    break;
                default:
                    break;

            }
            sonidoTruco.SoundLocation = audio;
            sonidoTruco.Play();
        }

        private void btnMazo_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void btnMazo_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void primerCarta_Click(object sender, EventArgs e)
        {
            MoverYReducir("primerCarta");

        }


        private List<int> ObtenerNumerosMismoPalo(List<Carta> cartas)
        {
            List<int> numerosCartasMismoPalo = new List<int>();

            if (cartas.Count >= 6) // Aseguramos que haya al menos 6 cartas en total
            {
                // Agregar números de cartas del mismo palo para el conjunto de cartas del índice 0 al 2
                if (cartas[0].palo == cartas[1].palo)
                {
                    numerosCartasMismoPalo.Add(cartas[0].numero);
                    numerosCartasMismoPalo.Add(cartas[1].numero);
                }

                else if (cartas[0].palo == cartas[2].palo)
                {
                    numerosCartasMismoPalo.Add(cartas[0].numero);
                    numerosCartasMismoPalo.Add(cartas[2].numero);
                }
                else if (cartas[1].palo == cartas[2].palo)
                {
                    numerosCartasMismoPalo.Add(cartas[1].numero);
                    numerosCartasMismoPalo.Add(cartas[2].numero);
                }

                // Agregar números de cartas del mismo palo para el conjunto de cartas del índice 3 al 5
                if (cartas[3].palo == cartas[4].palo)
                {
                    numerosCartasMismoPalo.Add(cartas[3].numero);
                    numerosCartasMismoPalo.Add(cartas[4].numero);
                }
                else if (cartas[3].palo == cartas[5].palo)
                {
                    numerosCartasMismoPalo.Add(cartas[3].numero);
                    numerosCartasMismoPalo.Add(cartas[5].numero);
                }
                else if (cartas[4].palo == cartas[5].palo)
                {
                    numerosCartasMismoPalo.Add(cartas[4].numero);
                    numerosCartasMismoPalo.Add(cartas[5].numero);
                }

                // Si la lista tiene menos de 4 elementos, agregamos ceros para completarla
                while (numerosCartasMismoPalo.Count < 4)
                {
                    numerosCartasMismoPalo.Add(0);
                }
            }

            return numerosCartasMismoPalo;
        }



        private void segundaCarta_Click(object sender, EventArgs e)
        {
            MoverYReducir("segundaCarta");


        }

        private void tercerCarta_Click(object sender, EventArgs e)
        {
            MoverYReducir("tercerCarta");
        }
        private void MoverYReducir(string nombreCarta)
        {
            PictureBox carta = ObtenerPictureBoxPorNombre(nombreCarta);

            // Verificar si ya se hizo clic en la carta
            if (carta.Enabled)
            {
                // Deshabilitar la carta para evitar clics adicionales
                carta.Enabled = false;

                // Ajustar la posición de la carta en el eje Y
                int nuevaPosicionY = carta.Location.Y - 85; // Puedes ajustar el valor según tus necesidades
                carta.Location = new Point(carta.Location.X, nuevaPosicionY);

                // Reducir un poco el tamaño de la carta
                int nuevoAncho = (int)(carta.Width * 0.9); // Puedes ajustar el factor de reducción según tus necesidades
                int nuevoAlto = (int)(carta.Height * 0.9); // Puedes ajustar el factor de reducción según tus necesidades
                carta.Size = new Size(nuevoAncho, nuevoAlto);

                // Forzar el repintado del control
                carta.Invalidate();

                // Resto del código que deseas ejecutar al hacer clic en la carta...
            }
        }



        private async void MostrarBotonesQuieroNoQuiero()
        {
            if (this.btnQuieroo.InvokeRequired || this.btnNoQuieroo.InvokeRequired)
            {
                this.Invoke((MethodInvoker)async delegate
                {
                    await Task.Delay(2000); // Esperar 3 segundos
                    MostrarBotonesQuieroNoQuiero();
                });
            }
            else
            {
                await Task.Delay(2000); // Esperar 3 segundos
                btnMazo.Visible = false;
                btnValecuatro.Visible = false;
                btnQuieroo.Visible = true;
                btnNoQuieroo.Visible = true;
            }
        }

        private void MoverYReducirBot(string nombreCarta)
        {
            PictureBox cartaBot = ObtenerPictureBoxPorNombre(nombreCarta);

            if (cartaBot != null)
            {
                if (cartaBot.InvokeRequired)
                {
                    // Acceder al control desde el hilo de la interfaz de usuario
                    cartaBot.Invoke((MethodInvoker)(() => MoverYReducirBot(nombreCarta)));
                }
                else
                {
                    // Modificar la posición de la carta del bot en el eje Y
                    int nuevaPosicionYBot = cartaBot.Location.Y + 92;
                    cartaBot.Location = new Point(cartaBot.Location.X, nuevaPosicionYBot);
                    
                    // Reducir un poco el tamaño de la carta del bot
                    int nuevoAnchoBot = (int)(cartaBot.Width * 0.9);
                    int nuevoAltoBot = (int)(cartaBot.Height * 0.9);
                    
                    cartaBot.Size = new Size(nuevoAnchoBot, nuevoAltoBot);
                    cartaBot.Invalidate();

                    // Resto del código que deseas ejecutar al hacer clic en la carta del bot...
                }
            }
        }



        private PictureBox ObtenerPictureBoxPorNombre(string nombre)
        {
            // Buscar el control PictureBox por su nombre en el formulario
            foreach (Control control in Controls)
            {
                if (control is PictureBox pictureBox && pictureBox.Name == nombre)
                {
                    return pictureBox;
                }
            }

            return null;
        }

        private void btnQuieroo_Click(object sender, EventArgs e)
        {
            btnQuieroo.Visible = false;
            btnNoQuieroo.Visible = false;
            banderaQuiero = true;
            string audio = "";
            SoundPlayer sonidoTruco = new SoundPlayer();

            switch (personajeElegido.ToLower())
            {
                case "maradona":
                    audio = @"../../../../UI TRUCO/MaradonaQuiero/quiero1.wav";
                    break;
                case "messi":
                    audio = @"../../../../UI TRUCO/MessiQuiero/quiero1.wav";
                    break;
                case "momo":
                    audio = @"../../../../UI TRUCO/MomoQuiero/quiero1.wav";
                    break;
                default:
                    break;

            }
            sonidoTruco.SoundLocation = audio;
            sonidoTruco.Play();
        }

        private void btnNoQuieroo_Click(object sender, EventArgs e)
        {
            btnNoQuieroo.Visible = false;
            btnQuieroo.Visible = false;
            banderaNoQuiero = true;
            string audio = "";
            SoundPlayer sonidoTruco = new SoundPlayer();

            switch (personajeElegido.ToLower())
            {
                case "maradona":
                    audio = @"../../../../UI TRUCO/MaradonaNoQuiero/noquiero1.wav";
                    break;
                case "messi":
                    audio = @"../../../../UI TRUCO/MessiNoQuiero/noquiero1.wav";
                    break;
                case "momo":
                    audio = @"../../../../UI TRUCO/MomoNoQuiero/noquiero1.wav";
                    break;
                default:
                    break;

            }
            sonidoTruco.SoundLocation = audio;
            sonidoTruco.Play();
        }

        private void btnRetruco_Click(object sender, EventArgs e)
        {
            btnRetruco.Visible = false;
            string audio = "";
            SoundPlayer sonidoTruco = new SoundPlayer();

            switch (personajeElegido.ToLower())
            {
                case "maradona":
                    audio = @"../../../../UI TRUCO/MaradonaRetruco/retruco1.wav";
                    break;
                case "messi":
                    audio = @"../../../../UI TRUCO/MessiRetruco/retruco1.wav";
                    break;
                case "momo":
                    audio = @"../../../../UI TRUCO/MomoRetruco/retruco1.wav";
                    break;
                default:
                    break;

            }
            sonidoTruco.SoundLocation = audio;
            sonidoTruco.Play();

        }

        private void btnValecuatro_Click(object sender, EventArgs e)
        {
            string audio = "";
            SoundPlayer sonidoTruco = new SoundPlayer();

            switch (personajeElegido.ToLower())
            {
                case "maradona":
                    audio = @"../../../../UI TRUCO/MaradonaValecuatro/valecuatro1.wav";
                    break;
                case "messi":
                    audio = @"../../../../UI TRUCO/MessiValecuatro/valecuatro1.wav";
                    break;
                case "momo":
                    audio = @"../../../../UI TRUCO/MomoValecuatro/valecuatro1.wav";
                    break;
                default:
                    break;

            }
            sonidoTruco.SoundLocation = audio;
            sonidoTruco.Play();
        }

        private void btnRealEnvido_Click(object sender, EventArgs e)
        {
            btnFaltaEnvido.Visible = false;
            btnEnvido.Visible = false;
            btnValecuatro.Visible = false;
            btnRealEnvido.Visible = false;
            string audio = "";
            SoundPlayer sonidoTruco = new SoundPlayer();

            switch (personajeElegido.ToLower())
            {
                case "maradona":
                    audio = @"../../../../UI TRUCO/MaradonaRealEnvido/realenvido1.wav";
                    break;
                case "messi":
                    audio = @"../../../../UI TRUCO/MessiRealEnvido/realenvido1.wav";
                    break;
                case "momo":
                    audio = @"../../../../UI TRUCO/MomoRealEnvido/realenvido1.wav";
                    break;
                default:
                    break;

            }
            sonidoTruco.SoundLocation = audio;
            sonidoTruco.Play();
        }

        private void btnFaltaEnvido_Click(object sender, EventArgs e)
        {
            string audio = "";
            SoundPlayer sonidoTruco = new SoundPlayer();

            switch (personajeElegido.ToLower())
            {
                case "maradona":
                    audio = @"../../../../UI TRUCO/MaradonaFaltaEnvido/envido1.wav";
                    break;
                case "messi":
                    audio = @"../../../../UI TRUCO/MessiFaltaEnvido/faltaenvido1.wav";
                    break;
                case "momo":
                    audio = @"../../../../UI TRUCO/MomoFaltaEnvido/faltaenvido1.wav";
                    break;
                default:
                    break;

            }
            sonidoTruco.SoundLocation = audio;
            sonidoTruco.Play();
        }
    }
}
