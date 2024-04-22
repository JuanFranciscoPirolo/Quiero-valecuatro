using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace UI_TRUCO
{
    public interface IAMetodos
    {
        SoundPlayer ReproducirAudio(string audio, string enLoop)
        {
            SoundPlayer sonido = new SoundPlayer();
            sonido.SoundLocation = audio;
            if (enLoop.ToLower() == "si")
            {
                sonido.PlayLooping();
            }
            else if(enLoop.ToLower() == "no")
            {
                sonido.Play();
            }
            else
            {
                sonido.Play();
            }
            return sonido;
        }
    }
}
