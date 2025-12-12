using System.Media;
using Calidad_Juego.Escenarios;


        private readonly SoundPlayer reproductorMusica = new();
            CrearNave(navex, 0, NaveFactory.TipoNavePrincipal, Color.SeaGreen, vidaJugador, navePrincipalFigura);
            return EscenarioEspacial.CrearFondo(tamano, GeneradorAleatorio);
        private void CrearNave(PictureBox avion, int anguloRotacion, int tipo, Color color, int vida, NaveFiguraEscalada? figuraPersonalizada = null)
            var figuraEscalada = figuraPersonalizada ?? plantilla.Escalar();
}                reproductorMusica.Stop();
            const string rutaCancion = @"C:\\Users\\derom\\Music\\Eagle Flight (OST)  Inon Zur - The Grim Falcon.wav";
                    reproductorMusica.PlayLooping();
                reproductorMusica.SoundLocation = rutaCancion;
                reproductorMusica.Load();
                reproductorMusica.PlayLooping();
