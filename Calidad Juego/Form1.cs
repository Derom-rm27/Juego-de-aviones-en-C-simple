// Autor: Delfin Estanis Roque Mullisaca
// Fecha: 07/11/2025
// Versión: 1.0
// Universidad: UNIVERSIDAD NACIONAL DE JULIACA
// Descripción: Lógica principal del juego de aviones sin modificar la estructura base.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Calidad_Juego
{
    public partial class Form1 : Form
    {
        private const int PaddingImpacto = 10;

        private static readonly Random GeneradorAleatorio = new();

        private readonly PictureBox navex = new();
        private readonly PictureBox naveRival = new();
        private readonly PictureBox contiene = new();

        private class ExplosionState
        {
            public int DuracionRestante { get; set; }
        }

        private System.Windows.Forms.Timer? tiempo;
        private int dispara;
        private bool moverHaciaIzquierda;
        private float angulo;

        private static readonly Point[] NaveTipo1 =
        {
            new(29, 0), new(30, 1), new(30, 6), new(31, 6), new(31, 11), new(32, 11), new(32, 17), new(35, 17),
            new(35, 16), new(37, 16), new(37, 17), new(38, 18), new(38, 28), new(39, 28), new(42, 39), new(44, 45),
            new(50, 51), new(51, 51), new(51, 52), new(58, 59), new(58, 66), new(44, 66), new(39, 71), new(35, 71),
            new(35, 74), new(32, 77), new(26, 77), new(23, 74), new(23, 71), new(19, 71), new(14, 66), new(0, 66),
            new(0, 59), new(7, 52), new(7, 51), new(8, 51), new(14, 45), new(16, 39), new(19, 28), new(20, 28),
            new(20, 18), new(21, 17), new(21, 16), new(23, 16), new(23, 17), new(26, 17), new(26, 11), new(27, 11),
            new(27, 6), new(28, 6), new(28, 1), new(29, 0)
        };

        private static readonly Point[] NaveTipo2 =
        {
            new(24, 0), new(29, 5), new(29, 18), new(32, 21), new(34, 21), new(38, 17), new(41, 20), new(41, 30),
            new(47, 36), new(47, 41), new(41, 41), new(38, 44), new(36, 44), new(33, 41), new(30, 41), new(25, 46),
            new(22, 46), new(17, 41), new(14, 41), new(11, 44), new(9, 44), new(6, 41), new(0, 41), new(0, 36),
            new(6, 30), new(6, 20), new(9, 17), new(13, 21), new(15, 21), new(18, 18), new(18, 5), new(23, 0)
        };

        private static readonly Point[] NaveTipo3 =
        {
            new(25, 5), new(26, 54), new(26, 5), new(26, 50), new(27, 50), new(28, 50), new(29, 50), new(30, 51),
            new(31, 51), new(32, 52), new(32, 49), new(31, 48), new(30, 47), new(29, 46), new(28, 45), new(27, 44),
            new(27, 36), new(28, 35), new(28, 25), new(29, 25), new(30, 25), new(31, 25), new(32, 26), new(33, 26),
            new(34, 27), new(35, 28), new(36, 8), new(37, 29), new(38, 30), new(39, 30), new(40, 31), new(41, 32),
            new(42, 32), new(43, 33), new(44, 34), new(45, 35), new(46, 36), new(47, 36), new(48, 36), new(49, 37),
            new(50, 37), new(51, 38), new(51, 37), new(51, 36), new(50, 35), new(50, 35), new(37, 15), new(37, 15),
            new(36, 14), new(35, 14), new(34, 15), new(34, 21), new(28, 15), new(28, 7), new(27, 6), new(26, 5),
            new(25, 5), new(24, 6), new(23, 7), new(23, 15), new(17, 21), new(17, 15), new(16, 14), new(15, 14),
            new(14, 15), new(14, 22), new(13, 25), new(12, 30), new(11, 31), new(10, 32), new(9, 32), new(8, 33),
            new(7, 34), new(6, 35), new(5, 36), new(4, 35), new(3, 36), new(2, 37), new(1, 37), new(0, 38),
            new(0, 39), new(0, 40), new(1, 40), new(2, 40), new(3, 40), new(4, 41), new(5, 42), new(6, 42),
            new(7, 42), new(8, 41), new(9, 40), new(10, 40), new(11, 40), new(12, 40), new(13, 40), new(14, 41),
            new(15, 42), new(16, 41), new(17, 40), new(18, 40), new(19, 40), new(20, 40), new(21, 40), new(22, 40),
            new(23, 40), new(24, 41), new(25, 42), new(26, 42), new(27, 42), new(28, 41), new(29, 40), new(30, 40),
            new(31, 40), new(32, 40), new(33, 39), new(33, 38), new(33, 37), new(33, 36), new(33, 35), new(33, 34),
            new(33, 33), new(32, 32), new(31, 31), new(30, 30), new(29, 29), new(28, 28), new(27, 27), new(26, 26),
            new(25, 25), new(24, 24), new(23, 23), new(22, 22), new(22, 21), new(22, 20), new(22, 19), new(22, 18),
            new(22, 17), new(22, 16), new(22, 15), new(21, 14), new(20, 13), new(20, 12), new(20, 11), new(21, 10),
            new(22, 9), new(23, 9), new(24, 9), new(25, 9), new(26, 9), new(25, 8), new(24, 7), new(23, 6),
            new(22, 5), new(21, 4), new(20, 3), new(19, 3), new(18, 2), new(17, 1), new(16, 0)
        };

        private static readonly Point[] MisilBase =
        {
            new(4, 1), new(5, 1), new(6, 2), new(6, 7), new(7, 8), new(8, 9), new(7, 9), new(6, 10),
            new(2, 10), new(1, 9), new(0, 9), new(1, 8), new(2, 7), new(2, 2), new(3, 1), new(4, 0)
        };

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
            KeyDown += ActividadTecla;
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            Iniciar();
        }

        private void Iniciar()
        {
            dispara = 0;
            moverHaciaIzquierda = false;
            angulo = 0;

            contiene.SuspendLayout();
            contiene.BackColor = Color.AliceBlue;
            contiene.BorderStyle = BorderStyle.FixedSingle;
            contiene.Size = new Size(300, 420);
            contiene.Location = new Point(12, 70);
            contiene.Image = null;
            contiene.Visible = true;
            contiene.ResumeLayout(false);

            if (!Controls.Contains(contiene))
            {
                Controls.Add(contiene);
            }

            contiene.SendToBack();
            label1.BringToFront();
            label2.BringToFront();

            contiene.Controls.Clear();

            CrearNave(navex, 0, 1, Color.SeaGreen, 3);
            CrearNave(naveRival, 180, GeneradorAleatorio.Next(1, 4), Color.DarkBlue, 50);

            int maxXJugador = Math.Max(1, contiene.Width - navex.Width - 1);
            int maxYJugador = Math.Max(1, contiene.Height - navex.Height - 1);
            int posicionXJugador = GeneradorAleatorio.Next(0, maxXJugador);
            int posicionYJugador = GeneradorAleatorio.Next(contiene.Height / 2, maxYJugador);

            navex.Location = new Point(posicionXJugador, posicionYJugador);
            navex.BringToFront();

            int maxXRival = Math.Max(1, contiene.Width - naveRival.Width - 1);
            int posicionXRival = GeneradorAleatorio.Next(0, maxXRival);
            naveRival.Location = new Point(posicionXRival, 40);
            naveRival.BringToFront();

            label1.Text = $"Vida del Rival: {naveRival.Tag}";
            label2.Text = $"Vida del Avión: {GenerarCorazones((int)navex.Tag)}";

            tiempo?.Stop();
            tiempo?.Dispose();
            tiempo = new System.Windows.Forms.Timer
            {
                Interval = 1,
                Enabled = true
            };
            tiempo.Tick += ImpactarTick;
            tiempo.Start();
        }

        private void CrearMisil(int anguloRotacion, Color color, string nombre, int x, int y)
        {
            var puntosRotados = RotarFigura(MisilBase, 11, anguloRotacion);
            using var trayectoria = new GraphicsPath();
            trayectoria.AddPolygon(puntosRotados);

            var misil = new PictureBox
            {
                Size = new Size(3, 11),
                BackColor = color,
                Tag = nombre,
                Location = new Point(x - 1, y - 5)
            };
            misil.Region = new Region(trayectoria);

            var imagen = new Bitmap(misil.Width, misil.Height);
            using (var grafico = Graphics.FromImage(imagen))
            {
                grafico.Clear(color);
            }

            misil.Image = imagen;
            contiene.Controls.Add(misil);
            misil.BringToFront();
        }

        private void ImpactarTick(object? sender, EventArgs e)
        {
            if (!naveRival.Visible && !navex.Visible)
            {
                tiempo?.Stop();
                return;
            }

            if (naveRival.Visible)
            {
                dispara++;
                if (dispara >= 100)
                {
                    int xRival = naveRival.Left + (naveRival.Width / 2);
                    int yRival = naveRival.Top + (naveRival.Height / 2);
                    CrearMisil(180, Color.OrangeRed, "Rival", xRival, yRival);
                    dispara = 0;
                }

                int posicionHorizontal = naveRival.Left;
                if (!moverHaciaIzquierda)
                {
                    if (posicionHorizontal >= contiene.Width - naveRival.Width)
                    {
                        moverHaciaIzquierda = true;
                    }
                    posicionHorizontal += 1;
                }
                else
                {
                    if (posicionHorizontal <= 0)
                    {
                        moverHaciaIzquierda = false;
                    }
                    posicionHorizontal -= 1;
                }

                posicionHorizontal = Math.Clamp(posicionHorizontal, 0, Math.Max(0, contiene.Width - naveRival.Width));
                naveRival.Left = posicionHorizontal;
            }

            for (int i = contiene.Controls.Count - 1; i >= 0; i--)
            {
                if (contiene.Controls[i] is not PictureBox misil)
                {
                    continue;
                }

                if (misil == navex || misil == naveRival)
                {
                    continue;
                }

                string nombre = misil.Tag as string ?? string.Empty;

                if (nombre == "Misil" && naveRival.Visible)
                {
                    if (MetricaImpactoCompleto(misil.Bounds, naveRival.Bounds))
                    {
                        if (ZonaCentral(misil.Bounds, naveRival.Bounds))
                        {
                            misil.Dispose();
                        }
                        else
                        {
                            misil.Dispose();
                            ActualizarVida(naveRival, label1);
                        }
                    }
                }

                if (nombre == "Rival" && navex.Visible)
                {
                    if (MetricaImpactoCompleto(misil.Bounds, navex.Bounds))
                    {
                        if (ZonaCentral(misil.Bounds, navex.Bounds))
                        {
                            misil.Dispose();
                        }
                        else
                        {
                            misil.Dispose();
                            ActualizarVida(navex, label2);
                        }
                    }
                }

                if (misil.Tag is string direccion)
                {
                    if (direccion == "Misil")
                    {
                        misil.Top -= 10;
                        if (misil.Bottom <= 0)
                        {
                            misil.Dispose();
                            continue;
                        }
                    }
                    else if (direccion == "Rival")
                    {
                        misil.Top += 5;
                        if (misil.Top >= contiene.Height)
                        {
                            CrearExplosion(misil.Left + (misil.Width / 2));
                            misil.Dispose();
                            continue;
                        }
                    }
                }

                if (misil.Tag is ExplosionState explosion)
                {
                    explosion.DuracionRestante--;
                    if (navex.Visible && navex.Bounds.IntersectsWith(misil.Bounds))
                    {
                        ActualizarVida(navex, label2);
                    }

                    if (explosion.DuracionRestante <= 0)
                    {
                        misil.Dispose();
                        continue;
                    }
                }
            }

            if (naveRival.Visible && navex.Visible && navex.Bounds.IntersectsWith(naveRival.Bounds))
            {
                naveRival.Dispose();
                navex.Dispose();
                tiempo?.Stop();
            }
        }

        private static bool MetricaImpactoCompleto(Rectangle proyectil, Rectangle objetivo)
        {
            return objetivo.Left < proyectil.Left && proyectil.Right < objetivo.Right &&
                   objetivo.Top < proyectil.Top && proyectil.Bottom < objetivo.Bottom;
        }

        private static bool ZonaCentral(Rectangle proyectil, Rectangle objetivo)
        {
            return objetivo.Left + PaddingImpacto < proyectil.Left &&
                   proyectil.Right < objetivo.Right - PaddingImpacto;
        }

        private void ActualizarVida(PictureBox objetivo, Label indicador)
        {
            if (objetivo.Tag is not int vidaActual)
            {
                vidaActual = Convert.ToInt32(objetivo.Tag);
            }

            vidaActual -= 1;
            objetivo.Tag = vidaActual;
            indicador.Text = objetivo == naveRival
                ? $"Vida del Rival: {vidaActual}"
                : $"Vida del Avión: {GenerarCorazones(vidaActual)}";

            if (vidaActual > 0)
            {
                return;
            }

            objetivo.Dispose();
            tiempo?.Stop();

            var resultado = new Bitmap(contiene.Width, contiene.Height);
            using var grafico = Graphics.FromImage(resultado);

            string mensaje = indicador == label1 ? "Felicitaciones ¡Ganaste!" : "Perdiste el juego";
            Color colorMensaje = indicador == label1 ? Color.Blue : Color.Red;
            using var fuente = new Font("Arial", 16, FontStyle.Bold, GraphicsUnit.Point);
            using var pincel = new SolidBrush(colorMensaje);
            grafico.Clear(Color.AliceBlue);
            grafico.DrawString(mensaje, fuente, pincel, new Point(40, 150));

            contiene.Image = resultado;
        }

        private void CrearNave(PictureBox avion, int anguloRotacion, int tipo, Color color, int vida)
        {
            avion.Visible = false;
            avion.BackColor = Color.Transparent;
            avion.SizeMode = PictureBoxSizeMode.Normal;

            Point[] figuraBase;
            int altura;
            int ancho;
            bool usarLineas = false;

            switch (tipo)
            {
                case 1:
                    figuraBase = NaveTipo1;
                    altura = 77;
                    ancho = 58;
                    break;
                case 2:
                    figuraBase = NaveTipo2;
                    altura = 46;
                    ancho = 47;
                    usarLineas = true;
                    break;
                default:
                    figuraBase = NaveTipo3;
                    altura = 54;
                    ancho = 51;
                    break;
            }

            var figuraRotada = RotarFigura(figuraBase, altura, anguloRotacion);
            using var grafico = new GraphicsPath();
            if (usarLineas)
            {
                grafico.AddLines(figuraRotada);
                grafico.CloseFigure();
            }
            else
            {
                grafico.AddPolygon(figuraRotada);
            }

            avion.Size = new Size(ancho, altura);
            avion.Region = new Region(grafico);

            var imagen = new Bitmap(avion.Width, avion.Height);
            using (var pinta = Graphics.FromImage(imagen))
            {
                pinta.SmoothingMode = SmoothingMode.AntiAlias;
                pinta.Clear(Color.Transparent);

                pinta.FillRegion(new SolidBrush(color), new Region(grafico));

                Point[] colorea =
                {
                    new(24, 2), new(27, 5), new(27, 18), new(31, 22), new(34, 22), new(37, 19), new(38, 19), new(39, 20),
                    new(39, 30), new(45, 36), new(45, 39), new(41, 39), new(38, 42), new(35, 42), new(32, 39), new(30, 39),
                    new(25, 44), new(21, 44), new(16, 39), new(14, 39), new(11, 42), new(8, 42), new(5, 39), new(1, 39),
                    new(1, 36), new(7, 30), new(7, 20), new(8, 19), new(9, 19), new(12, 22), new(15, 22), new(19, 18),
                    new(19, 5), new(22, 2)
                };

                pinta.DrawPolygon(Pens.Black, grafico.PathData.Points);
                pinta.FillPolygon(new SolidBrush(Color.FromArgb(200, Color.DarkSeaGreen)), colorea);
            }

            avion.Image = RotateImage(imagen, anguloRotacion);
            avion.Tag = vida;
            avion.Visible = true;

            if (!contiene.Controls.Contains(avion))
            {
                contiene.Controls.Add(avion);
            }
        }

        private void NaveCorre(PictureBox avion, int anguloRotacion, int velocidad)
        {
            if (avion.Image == null)
            {
                return;
            }

            var imagen = new Bitmap(avion.Width, avion.Height);
            using (var pinta = Graphics.FromImage(imagen))
            {
                pinta.Clear(Color.Transparent);

                Point[] puntoDer =
                {
                    new(35, 28), new(36, 30), new(37, 37), new(38, 38), new(39, 41), new(40, 45), new(40, 46), new(42, 48),
                    new(43, 49), new(44, 65), new(42, 66), new(38, 68), new(36, 69), new(36, 68), new(35, 66), new(35, 64),
                    new(35, 63), new(35, 62), new(35, 28)
                };

                Point[] puntoIzq =
                {
                    new(23, 28), new(22, 30), new(21, 31), new(20, 37), new(20, 40), new(19, 41), new(18, 45), new(18, 46),
                    new(16, 48), new(15, 49), new(15, 65), new(16, 66), new(17, 68), new(18, 69), new(20, 68), new(22, 66),
                    new(23, 63), new(23, 62), new(23, 28)
                };

                Point[] puntoCentral =
                {
                    new(29, 2), new(31, 19), new(32, 19), new(33, 20), new(33, 25), new(32, 26), new(32, 63), new(34, 65),
                    new(34, 68), new(33, 69), new(33, 73), new(31, 73), new(29, 73), new(27, 73), new(25, 74), new(24, 73),
                    new(24, 65), new(26, 68), new(26, 63), new(26, 26), new(27, 25), new(27, 21), new(26, 20), new(26, 19),
                    new(27, 19), new(28, 20)
                };

                pinta.FillPolygon(Brushes.DarkGreen, puntoDer);
                pinta.FillPolygon(Brushes.DarkGreen, puntoIzq);
                pinta.FillPolygon(Brushes.DarkGreen, puntoCentral);

                if (velocidad == 1)
                {
                    pinta.FillRectangle(Brushes.DarkOrange, 35, 65, 5, 1);
                    pinta.FillRectangle(Brushes.Orange, 36, 66, 4, 1);
                    pinta.FillRectangle(Brushes.Yellow, 37, 67, 2, 1);
                    pinta.FillRectangle(Brushes.DarkOrange, 23, 65, 5, 1);
                    pinta.FillRectangle(Brushes.Orange, 23, 66, 4, 1);
                    pinta.FillRectangle(Brushes.Yellow, 23, 67, 2, 1);
                }
                else if (velocidad == 2)
                {
                    pinta.FillRectangle(Brushes.DarkRed, 17, 30, 10, 8);
                    pinta.FillRectangle(Brushes.DarkRed, 35, 30, 10, 16);
                    pinta.FillRectangle(Brushes.DarkRed, 45, 30, 5, 8);
                }
                else if (velocidad == 3)
                {
                    pinta.FillRectangle(Brushes.DarkRed, 15, 30, 5, 8);
                    pinta.FillRectangle(Brushes.DarkRed, 5, 30, 5, 16);
                    pinta.FillRectangle(Brushes.DarkRed, 1, 30, 5, 8);
                }
            }

            avion.Image = RotateImage(imagen, anguloRotacion);
        }

        private static Image RotateImage(Image imagen, float anguloRotacion)
        {
            Bitmap bitmap = new(imagen.Width, imagen.Height);
            bitmap.SetResolution(imagen.HorizontalResolution, imagen.VerticalResolution);

            using (Graphics grafico = Graphics.FromImage(bitmap))
            {
                grafico.TranslateTransform(bitmap.Width / 2f, bitmap.Height / 2f);
                grafico.RotateTransform(anguloRotacion);
                grafico.TranslateTransform(-bitmap.Width / 2f, -bitmap.Height / 2f);
                grafico.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grafico.DrawImage(imagen, new Point(0, 0));
            }

            return bitmap;
        }

        private void ActividadTecla(object? sender, KeyEventArgs e)
        {
            if (!navex.Visible)
            {
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (navex.Left > 0)
                    {
                        navex.Left = Math.Max(0, navex.Left - 10);
                        angulo -= 15;
                        NaveCorre(navex, 0, 1);
                    }
                    break;
                case Keys.Up:
                    if (navex.Top > 0)
                    {
                        navex.Top = Math.Max(0, navex.Top - 10);
                        NaveCorre(navex, 0, 1);
                    }
                    break;
                case Keys.Right:
                    if (navex.Right < contiene.Width)
                    {
                        navex.Left = Math.Min(contiene.Width - navex.Width, navex.Left + 10);
                        angulo += 15;
                        NaveCorre(navex, 0, 1);
                    }
                    break;
                case Keys.Down:
                    if (navex.Bottom < contiene.Height)
                    {
                        navex.Top = Math.Min(contiene.Height - navex.Height, navex.Top + 10);
                        NaveCorre(navex, 0, 1);
                    }
                    break;
                case Keys.Space:
                    tiempo?.Start();
                    int x = navex.Left + (navex.Width / 2);
                    int y = navex.Top + (navex.Height / 2);
                    CrearMisil(0, Color.DarkMagenta, "Misil", x, y);
                    break;
            }
        }

        private static Point[] RotarFigura(Point[] figuraBase, int alto, int anguloRotacion)
        {
            Point[] resultado = new Point[figuraBase.Length];
            for (int i = 0; i < figuraBase.Length; i++)
            {
                resultado[i].X = figuraBase[i].X;
                resultado[i].Y = anguloRotacion == 180 ? alto - figuraBase[i].Y : figuraBase[i].Y;
            }

            return resultado;
        }

        private void CrearExplosion(int centroX)
        {
            const int anchoExplosion = 120;
            const int altoExplosion = 60;

            int posicionX = Math.Clamp(centroX - (anchoExplosion / 2), 0, Math.Max(0, contiene.Width - anchoExplosion));
            int posicionY = contiene.Height - altoExplosion;

            var explosion = new PictureBox
            {
                Size = new Size(anchoExplosion, altoExplosion),
                BackColor = Color.Transparent,
                Location = new Point(posicionX, posicionY),
                Tag = new ExplosionState { DuracionRestante = 40 }
            };

            var imagen = new Bitmap(explosion.Width, explosion.Height);
            using (var grafico = Graphics.FromImage(imagen))
            {
                grafico.SmoothingMode = SmoothingMode.AntiAlias;
                grafico.Clear(Color.Transparent);

                var rectangulo = new Rectangle(0, altoExplosion / 2, anchoExplosion, altoExplosion / 2);
                using var pincel = new LinearGradientBrush(rectangulo, Color.OrangeRed, Color.Yellow, LinearGradientMode.Vertical);
                grafico.FillEllipse(pincel, rectangulo);
                grafico.DrawEllipse(new Pen(Color.DarkRed, 2), rectangulo);
            }

            explosion.Image = imagen;

            contiene.Controls.Add(explosion);
            explosion.BringToFront();

            if (navex.Visible && navex.Bounds.IntersectsWith(explosion.Bounds))
            {
                ActualizarVida(navex, label2);
            }
        }

        private static string GenerarCorazones(int cantidad)
        {
            return new string('❤', Math.Max(0, cantidad));
        }
    }
}
