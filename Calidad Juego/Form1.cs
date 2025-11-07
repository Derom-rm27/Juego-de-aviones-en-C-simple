// Autor: Delfin Estanis Roque Mullisaca
// Fecha: 07/11/2025
// Versión: 1.0
// Universidad: UNIVERSIDAD NACIONAL DE JULIACA
// Descripción: Lógica principal del juego de aviones sin modificar la estructura base.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Calidad_Juego
{
    public partial class Form1 : Form
    {
        private const int PaddingImpacto = 10;
        private const int VelocidadJugador = 5;
        private const int VelocidadMisilJugador = 8;
        private const int VelocidadMisilRival = 5;
        private const float SuavizadoAngulo = 0.25f;
        private const int IntervaloDisparoJugador = 12;

        private static readonly Random GeneradorAleatorio = new();

        private readonly PictureBox navex = new();
        private readonly PictureBox naveRival = new();
        private readonly PictureBox contiene = new();

        private Image? fondoEscenario;

        private System.Windows.Forms.Timer? tiempo;
        private int dispara;
        private bool moverHaciaIzquierda;
        private bool moverJugadorHaciaIzquierda;
        private bool moverJugadorHaciaDerecha;
        private bool moverJugadorHaciaArriba;
        private bool moverJugadorHaciaAbajo;
        private float angulo;
        private bool disparoActivo;
        private int recargaDisparo;

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
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();

            Load += Form1_Load;
            KeyDown += ActividadTecla;
            KeyUp += DetenerActividadTecla;
            FormClosed += (_, _) => fondoEscenario?.Dispose();
            botonReintentar.Click += BotonReintentar_Click;
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            Iniciar();
        }

        private void Iniciar()
        {
            dispara = 0;
            moverHaciaIzquierda = false;
            moverJugadorHaciaIzquierda = false;
            moverJugadorHaciaDerecha = false;
            moverJugadorHaciaArriba = false;
            moverJugadorHaciaAbajo = false;
            angulo = 0;
            disparoActivo = false;
            recargaDisparo = 0;

            botonReintentar.Visible = false;
            botonReintentar.Enabled = false;

            labelEstado.Text = "Mantén presionadas las flechas para maniobrar.";
            labelIndicaciones.Text = "Presiona Espacio para disparar y Enter para reiniciar la misión.";
            labelIndicaciones.MaximumSize = new Size(ClientSize.Width - 24, 0);

            int margenSuperior = label2.Bottom + 20;
            int ancho = ClientSize.Width - 24;
            int alto = ClientSize.Height - margenSuperior - 20;

            contiene.SuspendLayout();
            contiene.BackColor = Color.Transparent;
            contiene.BorderStyle = BorderStyle.FixedSingle;
            contiene.Size = new Size(Math.Max(260, ancho), Math.Max(320, alto));
            contiene.Location = new Point(12, margenSuperior);
            contiene.TabStop = false;
            if (contiene.Image != null && !ReferenceEquals(contiene.Image, fondoEscenario))
            {
                contiene.Image.Dispose();
            }
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
            labelIndicaciones.BringToFront();

            if (fondoEscenario != null)
            {
                fondoEscenario.Dispose();
            }

            fondoEscenario = CrearFondoEscenario(contiene.Size);
            contiene.BackgroundImage = fondoEscenario;
            contiene.BackgroundImageLayout = ImageLayout.Stretch;

            contiene.Controls.Clear();

            CrearNave(navex, 0, 1, Color.SeaGreen, 20);
            CrearNave(naveRival, 180, GeneradorAleatorio.Next(1, 4), Color.DarkBlue, 50);

            int maxXJugador = Math.Max(1, contiene.Width - navex.Width - 1);
            int maxYJugador = Math.Max(1, contiene.Height - navex.Height - 30);
            int posicionXJugador = GeneradorAleatorio.Next(0, maxXJugador);
            int minYJugador = Math.Max(contiene.Height / 2, maxYJugador - 60);
            if (minYJugador >= maxYJugador)
            {
                minYJugador = Math.Max(0, maxYJugador - 20);
            }

            int posicionYJugador = GeneradorAleatorio.Next(minYJugador, Math.Max(minYJugador + 1, maxYJugador));

            navex.Location = new Point(posicionXJugador, posicionYJugador);
            navex.BringToFront();

            int maxXRival = Math.Max(1, contiene.Width - naveRival.Width - 1);
            int posicionXRival = GeneradorAleatorio.Next(0, maxXRival);
            naveRival.Location = new Point(posicionXRival, 40);
            naveRival.BringToFront();

            label1.Text = $"Vida del Rival: {naveRival.Tag}";
            label2.Text = $"Vida del Avión: {navex.Tag}";

            tiempo?.Stop();
            tiempo?.Dispose();
            tiempo = new System.Windows.Forms.Timer
            {
                Interval = 16
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
                BackColor = Color.Transparent,
                Tag = nombre,
                Location = new Point(x - 1, y - 5)
            };
            misil.Region = new Region(trayectoria);

            var imagen = new Bitmap(misil.Width, misil.Height);
            using (var grafico = Graphics.FromImage(imagen))
            {
                grafico.SmoothingMode = SmoothingMode.AntiAlias;
                grafico.Clear(Color.Transparent);

                using var gradiente = new LinearGradientBrush(new Rectangle(Point.Empty, imagen.Size),
                    Color.FromArgb(240, Color.White),
                    color,
                    LinearGradientMode.Vertical);
                grafico.FillPath(gradiente, trayectoria);

                using var contorno = new Pen(Color.FromArgb(160, color), 1f);
                grafico.DrawPath(contorno, trayectoria);
            }

            misil.Image = imagen;
            contiene.Controls.Add(misil);
            misil.BringToFront();
        }

        private void ImpactarTick(object? sender, EventArgs e)
        {
            try
            {
                ActualizarDisparoJugador();
                MoverJugador();
                MoverNaveRival();
                ProcesarProyectiles();
                VerificarColisionDirecta();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ManejarErrorColeccion(ex);
            }
        }

        private static bool MetricaImpactoCompleto(Rectangle proyectil, Rectangle objetivo)
        {
            Rectangle zonaImpacto = Rectangle.Inflate(objetivo, -PaddingImpacto, -PaddingImpacto);
            if (zonaImpacto.Width <= 0 || zonaImpacto.Height <= 0)
            {
                zonaImpacto = objetivo;
            }

            return zonaImpacto.IntersectsWith(proyectil);
        }

        private void MoverJugador()
        {
            if (!navex.Visible)
            {
                return;
            }

            int deltaX = 0;
            int deltaY = 0;

            if (moverJugadorHaciaIzquierda)
            {
                deltaX -= VelocidadJugador;
            }

            if (moverJugadorHaciaDerecha)
            {
                deltaX += VelocidadJugador;
            }

            if (moverJugadorHaciaArriba)
            {
                deltaY -= VelocidadJugador;
            }

            if (moverJugadorHaciaAbajo)
            {
                deltaY += VelocidadJugador;
            }

            int limiteX = Math.Max(0, contiene.Width - navex.Width);
            int limiteY = Math.Max(0, contiene.Height - navex.Height);

            if (deltaX == 0 && deltaY == 0)
            {
                angulo = AjustarAngulo(angulo, 0f);
                if (Math.Abs(angulo) > 0.1f)
                {
                    NaveCorre(navex, (int)Math.Round(angulo), 1);
                }

                return;
            }

            int nuevoX = Math.Clamp(navex.Left + deltaX, 0, limiteX);
            int nuevoY = Math.Clamp(navex.Top + deltaY, 0, limiteY);
            navex.Location = new Point(nuevoX, nuevoY);

            float anguloObjetivo = deltaX switch
            {
                < 0 => -12f,
                > 0 => 12f,
                _ => 0f
            };

            angulo = AjustarAngulo(angulo, anguloObjetivo);
            NaveCorre(navex, (int)Math.Round(angulo), 1);
        }

        private float AjustarAngulo(float actual, float objetivo)
        {
            float diferencia = objetivo - actual;
            if (Math.Abs(diferencia) <= 0.5f)
            {
                return objetivo;
            }

            return actual + (diferencia * SuavizadoAngulo);
        }

        private static bool ZonaCentral(Rectangle proyectil, Rectangle objetivo)
        {
            Rectangle zonaSegura = Rectangle.Inflate(objetivo, -PaddingImpacto * 2, -PaddingImpacto * 2);
            if (zonaSegura.Width <= 0 || zonaSegura.Height <= 0)
            {
                return false;
            }

            return zonaSegura.Contains(proyectil.Location) && zonaSegura.Contains(new Point(proyectil.Right, proyectil.Bottom));
        }

        private void ActualizarVida(PictureBox objetivo, Label indicador)
        {
            int vidaActual = Math.Max(0, Convert.ToInt32(objetivo.Tag) - 1);
            objetivo.Tag = vidaActual;
            indicador.Text = objetivo == naveRival
                ? $"Vida del Rival: {vidaActual}"
                : $"Vida del Avión: {vidaActual}";

            DestacarEtiqueta(indicador);

            if (vidaActual > 0)
            {
                return;
            }

            if (objetivo == naveRival)
            {
                naveRival.Visible = false;
                MostrarMensajeFinal(true);
            }
            else
            {
                navex.Visible = false;
                MostrarMensajeFinal(false);
            }
        }

        private void MostrarMensajeFinal(bool jugadorGana)
        {
            moverJugadorHaciaArriba = false;
            moverJugadorHaciaAbajo = false;
            moverJugadorHaciaDerecha = false;
            moverJugadorHaciaIzquierda = false;
            disparoActivo = false;

            tiempo?.Stop();

            labelEstado.Text = jugadorGana
                ? "¡Victoria asegurada! Presiona Enter para una nueva misión."
                : "Misión fallida. Pulsa Enter para intentarlo nuevamente.";

            foreach (Control control in contiene.Controls.Cast<Control>().ToArray())
            {
                if (control is PictureBox misil && misil != navex && misil != naveRival)
                {
                    misil.Dispose();
                }
            }

            if (contiene.Image != null && !ReferenceEquals(contiene.Image, fondoEscenario))
            {
                contiene.Image.Dispose();
            }

            var lienzo = new Bitmap(contiene.Width, contiene.Height);
            using (var grafico = Graphics.FromImage(lienzo))
            {
                grafico.SmoothingMode = SmoothingMode.AntiAlias;
                Color colorInicio = jugadorGana ? Color.FromArgb(35, 130, 90) : Color.FromArgb(150, 40, 40);
                Color colorFin = Color.FromArgb(10, 16, 28);
                using var gradiente = new LinearGradientBrush(new Rectangle(Point.Empty, contiene.Size), colorInicio, colorFin, LinearGradientMode.ForwardDiagonal);
                grafico.FillRectangle(gradiente, new Rectangle(Point.Empty, contiene.Size));

                using var fuenteTitulo = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
                using var fuenteDetalle = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
                var formato = new StringFormat { Alignment = StringAlignment.Center };

                string titulo = jugadorGana ? "¡Felicitaciones, piloto!" : "Alerta de misión";
                string detalle = jugadorGana
                    ? "Has logrado derribar a la nave rival."
                    : "La nave rival superó nuestra defensa.";

                grafico.DrawString(titulo, fuenteTitulo, Brushes.White, new RectangleF(0, 140, lienzo.Width, 32), formato);
                grafico.DrawString(detalle, fuenteDetalle, Brushes.WhiteSmoke, new RectangleF(0, 180, lienzo.Width, 60), formato);
            }

            contiene.Image = lienzo;

            botonReintentar.Visible = true;
            botonReintentar.Enabled = true;
            botonReintentar.BringToFront();
        }

        private void DestacarEtiqueta(Label indicador)
        {
            Color colorOriginal = Color.FromArgb(216, 235, 255);
            indicador.ForeColor = Color.FromArgb(255, 222, 89);

            var destello = new System.Windows.Forms.Timer
            {
                Interval = 200
            };

            destello.Tick += (_, _) =>
            {
                indicador.ForeColor = colorOriginal;
                destello.Stop();
                destello.Dispose();
            };

            destello.Start();
        }

        private static Image CrearFondoEscenario(Size tamano)
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
                    int puntoX = GeneradorAleatorio.Next(0, ancho);
                    int puntoY = GeneradorAleatorio.Next(0, alto);
                    int brillo = GeneradorAleatorio.Next(180, 255);
                    using var pincelEstrella = new SolidBrush(Color.FromArgb(brillo, Color.White));
                    grafico.FillEllipse(pincelEstrella, puntoX, puntoY, 2, 2);
                }

                using var pincelNebulosa = new SolidBrush(Color.FromArgb(60, 255, 255, 255));
                grafico.FillEllipse(pincelNebulosa, ancho / 4, alto / 3, ancho / 2, alto / 3);
            }

            return fondo;
        }

        private void CrearNave(PictureBox avion, int anguloRotacion, int tipo, Color color, int vida)
        {
            avion.Visible = false;
            avion.BackColor = Color.Transparent;
            avion.SizeMode = PictureBoxSizeMode.Normal;
            avion.TabStop = false;

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
            if (!navex.Visible && e.KeyCode != Keys.Enter)
            {
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.Left:
                    moverJugadorHaciaIzquierda = true;
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Right:
                    moverJugadorHaciaDerecha = true;
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Up:
                    moverJugadorHaciaArriba = true;
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Down:
                    moverJugadorHaciaAbajo = true;
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Space:
                    if (navex.Visible)
                    {
                        disparoActivo = true;
                        DispararMisilJugador();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    break;
                case Keys.Enter:
                    if (tiempo == null || !tiempo.Enabled || !navex.Visible || !naveRival.Visible)
                    {
                        Iniciar();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }

                    break;
            }
        }

        private void DetenerActividadTecla(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    moverJugadorHaciaIzquierda = false;
                    break;
                case Keys.Right:
                    moverJugadorHaciaDerecha = false;
                    break;
                case Keys.Up:
                    moverJugadorHaciaArriba = false;
                    break;
                case Keys.Down:
                    moverJugadorHaciaAbajo = false;
                    break;
                case Keys.Space:
                    disparoActivo = false;
                    break;
            }

            if (e.KeyCode is Keys.Left or Keys.Right or Keys.Up or Keys.Down)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void ActualizarDisparoJugador()
        {
            if (recargaDisparo > 0)
            {
                recargaDisparo--;
            }

            if (!disparoActivo || !navex.Visible)
            {
                return;
            }

            if (recargaDisparo <= 0)
            {
                DispararMisilJugador();
            }
        }

        private void DispararMisilJugador()
        {
            if (!navex.Visible)
            {
                return;
            }

            int x = navex.Left + (navex.Width / 2);
            int y = navex.Top + (navex.Height / 2);
            CrearMisil(0, Color.DarkMagenta, "Misil", x, y);
            recargaDisparo = IntervaloDisparoJugador;
        }

        private void MoverNaveRival()
        {
            if (!naveRival.Visible)
            {
                return;
            }

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

                posicionHorizontal += 2;
            }
            else
            {
                if (posicionHorizontal <= 0)
                {
                    moverHaciaIzquierda = false;
                }

                posicionHorizontal -= 2;
            }

            posicionHorizontal = Math.Clamp(posicionHorizontal, 0, Math.Max(0, contiene.Width - naveRival.Width));
            naveRival.Left = posicionHorizontal;
        }

        private void ProcesarProyectiles()
        {
            var proyectiles = contiene.Controls
                .OfType<PictureBox>()
                .Where(control => control != navex && control != naveRival)
                .ToArray();

            foreach (var misil in proyectiles)
            {
                string nombre = misil.Tag as string ?? string.Empty;
                Rectangle posicionActual = misil.Bounds;

                if (nombre == "Misil" && naveRival.Visible)
                {
                    if (MetricaImpactoCompleto(posicionActual, naveRival.Bounds))
                    {
                        misil.Dispose();
                        if (!ZonaCentral(posicionActual, naveRival.Bounds))
                        {
                            ActualizarVida(naveRival, label1);
                        }

                        continue;
                    }
                }
                else if (nombre == "Rival" && navex.Visible)
                {
                    if (MetricaImpactoCompleto(posicionActual, navex.Bounds))
                    {
                        misil.Dispose();
                        if (!ZonaCentral(posicionActual, navex.Bounds))
                        {
                            ActualizarVida(navex, label2);
                        }

                        continue;
                    }
                }

                if (misil.IsDisposed)
                {
                    continue;
                }

                if (nombre == "Misil")
                {
                    misil.Top -= VelocidadMisilJugador;
                    if (misil.Bottom <= 0)
                    {
                        misil.Dispose();
                    }
                }
                else if (nombre == "Rival")
                {
                    misil.Top += VelocidadMisilRival;
                    if (misil.Top >= contiene.Height)
                    {
                        misil.Dispose();
                    }
                }
            }
        }

        private void VerificarColisionDirecta()
        {
            if (!naveRival.Visible || !navex.Visible)
            {
                return;
            }

            if (!navex.Bounds.IntersectsWith(naveRival.Bounds))
            {
                return;
            }

            navex.Tag = 0;
            label2.Text = "Vida del Avión: 0";
            navex.Visible = false;
            naveRival.Visible = false;
            MostrarMensajeFinal(false);
        }

        private void ManejarErrorColeccion(ArgumentOutOfRangeException ex)
        {
            Debug.WriteLine($"Se controló un error de colección: {ex.Message}");
            labelEstado.Text = "Se detectó un error interno. Reiniciando misión.";
            tiempo?.Stop();

            BeginInvoke(new Action(Iniciar));
        }

        private void BotonReintentar_Click(object? sender, EventArgs e)
        {
            Iniciar();
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
    }
}
