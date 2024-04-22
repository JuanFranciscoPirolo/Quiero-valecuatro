using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_TRUCO
{
    public partial class ElegirPersonaje : Form
    {

        public ElegirPersonaje()
        {
            InitializeComponent();
        }

        private void ElegirPersonaje_Load(object sender, EventArgs e)
        {
            SoundPlayer sonido = new SoundPlayer();
            sonido.SoundLocation = @"../../../../UI TRUCO/sonidos/gardel.wav";
            sonido.Stop();

            RedondearImagen(pictureDiego);
            //RedondearImagen(pictureLuisito);
            RedondearImagen(pictureMessi);
            RedondearImagen(pictureMomo);

        }
        private void RedondearImagen(PictureBox pictureBox)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, pictureBox.Width, pictureBox.Height);
            pictureBox.Region = new Region(path);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SoundPlayer sonido = new SoundPlayer();
            sonido.SoundLocation = @"../../../../UI TRUCO/sonidos/luisito.wav";
            sonido.Play();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SoundPlayer sonido = new SoundPlayer();
            sonido.SoundLocation = @"../../../../UI TRUCO/sonidos/messi.wav";
            sonido.Play();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SoundPlayer sonido = new SoundPlayer();
            sonido.SoundLocation = @"../../../../UI TRUCO/sonidos/momo.wav";
            sonido.Play();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SoundPlayer sonido = new SoundPlayer();
            sonido.SoundLocation = @"../../../../UI TRUCO/sonidos/maradona.wav";
            sonido.Play();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int checkBoxMarcados = 0;
            string personajeElegido = "";

            // Recorre todos los controles del formulario
            foreach (Control control in Controls)
            {
                // Verifica si el control es un CheckBox y está marcado
                if (control is CheckBox checkBox && checkBox.Checked)
                {
                    checkBoxMarcados++;
                    personajeElegido = ObtenerNombrePersonaje(checkBox);
                    this.Close();
                    frmPartidaTruco partidaTruco = new frmPartidaTruco(personajeElegido);
                    partidaTruco.Show();
                }
            }

            // Muestra un mensaje de error si no hay ningún CheckBox marcado o hay más de dos marcados
            if (checkBoxMarcados == 0)
            {
                MessageBox.Show("Por favor, selecciona al menos un personaje", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (checkBoxMarcados > 1)
            {
                MessageBox.Show("Por favor, selecciona un solo personaje.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private string ObtenerNombrePersonaje(CheckBox checkBox)
        {
            if (checkBox == checkBox1)
                return "maradona";
            else if (checkBox == checkBox2)
                return "messi";
            else if (checkBox == checkBox3)
                return "momo";
            else
                return "";
        }

        private void pictureMessi_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void pictureMomo_Click(object sender, EventArgs e)
        {
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void pictureDiego_Click(object sender, EventArgs e)
        {
        }
    }
}
