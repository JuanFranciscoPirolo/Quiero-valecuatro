using System.Media;
namespace UI_TRUCO
{
    public partial class FormularioPrincipal : Form
    {
        public FormularioPrincipal()
        {
            InitializeComponent();
            this.MaximizeBox = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SoundPlayer sonido = new SoundPlayer();
            sonido.SoundLocation = @"../../../../UI TRUCO/sonidos/gardel.wav";
            sonido.PlayLooping();
        }

        bool iniciarPartida = false;
        private void btnPartida_Click(object sender, EventArgs e)
        {
            // Muestra el formulario 'personaje' de manera modal
            ElegirPersonaje personaje = new ElegirPersonaje();
            if (personaje.ShowDialog() == DialogResult.OK)
            {
                // Si el usuario hizo clic en "Aceptar" en el formulario 'personaje'
                // Crea y muestra el formulario 'partidaDeTruco'
                // Oculta el formulario actual
                this.Hide();
            }
            else
            {
                this.Hide();
                // Si el usuario cancela en el formulario 'personaje', puedes realizar alguna acción o simplemente salir.
            }
        }

    }

}