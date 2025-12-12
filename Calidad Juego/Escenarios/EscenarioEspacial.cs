using System.Drawing;
using System.Drawing.Drawing2D;

namespace Calidad_Juego.Escenarios
{
    internal static class EscenarioEspacial
    {
        public static Image CrearFondo(Size tamano, Random generador)
        {
            int ancho = Math.Max(1, tamano.Width);
            int alto = Math.Max(1, tamano.Height);

            Bitmap fondo = new(ancho, alto);

            using (Graphics grafico = Graphics.FromImage(fondo))
            {
                grafico.SmoothingMode = SmoothingMode.AntiAlias;
                using var gradiente = new LinearGradientBrush(new Rectangle(Point.Empty, fondo.Size),
                    Color.FromArgb(12, 28, 58),
                    Color.FromArgb(55, 99, 158),
                    LinearGradientMode.Vertical);
                grafico.FillRectangle(gradiente, new Rectangle(Point.Empty, fondo.Size));

                using var plumaMalla = new Pen(Color.FromArgb(40, 200, 220, 255), 1f);
                for (int y = 30; y < alto; y += 40)
                {
                    grafico.DrawLine(plumaMalla, 0, y, ancho, y);
                }

                for (int x = 30; x < ancho; x += 40)
                {
                    grafico.DrawLine(plumaMalla, x, 0, x, alto);
                }

                int cantidadEstrellas = Math.Max(40, (ancho * alto) / 2500);
                for (int i = 0; i < cantidadEstrellas; i++)
                {
                    int puntoX = generador.Next(0, ancho);
                    int puntoY = generador.Next(0, alto);
                    int brillo = generador.Next(180, 255);
                    using var pincelEstrella = new SolidBrush(Color.FromArgb(brillo, Color.White));
                    grafico.FillEllipse(pincelEstrella, puntoX, puntoY, 2, 2);
                }

                using var pincelNebulosa = new SolidBrush(Color.FromArgb(60, 255, 255, 255));
                grafico.FillEllipse(pincelNebulosa, ancho / 4, alto / 3, ancho / 2, alto / 3);
            }

            return fondo;
        }
    }
}
