// Autor: Delfin Estanis Roque Mullisaca
// Fecha: 07/11/2025
// Versión: 1.0
// Universidad: UNIVERSIDAD NACIONAL DE JULIACA
// Descripción: Lógica principal del juego de aviones sin modificar la estructura base.

using System;
using System.Collections.Generic;
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
        private bool moverJugadorHaciaIzquierda;
        private bool moverJugadorHaciaDerecha;
        private bool moverJugadorHaciaArriba;
        private bool moverJugadorHaciaAbajo;
        private float angulo;
        private bool disparoActivo;
        private int recargaDisparo;
        private int nivelActual;
        private int enemigosRestantes;
        private bool enTransicionNivel;

        private const int NivelMaximo = 5;

        private readonly List<PictureBox> enemigosActivos = new();
        private readonly List<PictureBox> obstaculosActivos = new();
        private readonly Dictionary<PictureBox, int> contadorDisparoEnemigo = new();
        private readonly Dictionary<PictureBox, int> direccionHorizontalEnemiga = new();
        private readonly Dictionary<PictureBox, float> velocidadHorizontalEnemiga = new();
        private readonly Dictionary<PictureBox, PointF> velocidadObstaculos = new();
        private readonly Dictionary<PictureBox, int> faseObstaculo = new();
        private readonly Dictionary<PictureBox, BarraVidaEnemigo> barrasVidaEnemigo = new();

        private sealed class BarraVidaEnemigo
        {
            public BarraVidaEnemigo(Panel contenedor, Panel relleno, int vidaMaxima)
            {
                Contenedor = contenedor;
                Relleno = relleno;
                VidaMaxima = vidaMaxima;
            }

            public Panel Contenedor { get; }
            public Panel Relleno { get; }
            public int VidaMaxima { get; set; }
        }

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
            moverJugadorHaciaIzquierda = false;
            moverJugadorHaciaDerecha = false;
            moverJugadorHaciaArriba = false;
            moverJugadorHaciaAbajo = false;
            angulo = 0;
            disparoActivo = false;
            recargaDisparo = 0;
            nivelActual = 1;
            enemigosRestantes = 0;
            enTransicionNivel = false;

            botonReintentar.Visible = false;
            botonReintentar.Enabled = false;

            enemigosActivos.Clear();
            obstaculosActivos.Clear();
            contadorDisparoEnemigo.Clear();
            direccionHorizontalEnemiga.Clear();
            velocidadHorizontalEnemiga.Clear();
            velocidadObstaculos.Clear();
            faseObstaculo.Clear();

            labelEstado.Text = "Mantén presionadas las flechas para maniobrar.";
            labelIndicaciones.Text = "Espacio para disparar, Enter para reiniciar: supera niveles, meteoritos y escuadrones rivales.";
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

            ConfigurarNivelActual();

            tiempo?.Stop();
            tiempo?.Dispose();
            tiempo = new System.Windows.Forms.Timer
            {
                Interval = 16
            };
            tiempo.Tick += ImpactarTick;
            tiempo.Start();
        }

        private void ConfigurarNivelActual()
        {
            disparoActivo = false;
            recargaDisparo = 0;
            enTransicionNivel = false;

            LimpiarElementosTemporales();

            int vidaJugador = CalcularVidaJugador();
            CrearNave(navex, 0, 1, Color.SeaGreen, vidaJugador);
            RestablecerJugadorParaNivel(vidaJugador);

            ConfigurarEnemigosNivel();
            ConfigurarObstaculosNivel();
            ActualizarIndicadoresNivel(false);
            navex.BringToFront();
        }

        private void LimpiarElementosTemporales()
        {
            foreach (var misil in contiene.Controls
                         .OfType<PictureBox>()
                         .Where(pb => pb.Tag is string nombre && (nombre == "Misil" || nombre == "Rival"))
                         .ToArray())
            {
                misil.Dispose();
            }

            foreach (var enemigo in enemigosActivos.ToArray())
            {
                QuitarBarraVidaEnemigo(enemigo);

                if (ReferenceEquals(enemigo, naveRival))
                {
                    contiene.Controls.Remove(enemigo);
                    enemigo.Visible = false;
                }
                else
                {
                    contiene.Controls.Remove(enemigo);
                    enemigo.Dispose();
                }
            }

            enemigosActivos.Clear();
            contadorDisparoEnemigo.Clear();
            direccionHorizontalEnemiga.Clear();
            velocidadHorizontalEnemiga.Clear();

            foreach (var enemigo in barrasVidaEnemigo.Keys.ToArray())
            {
                QuitarBarraVidaEnemigo(enemigo);
            }

            foreach (var obstaculo in obstaculosActivos.ToArray())
            {
                contiene.Controls.Remove(obstaculo);
                obstaculo.Dispose();
            }

            obstaculosActivos.Clear();
            velocidadObstaculos.Clear();
            faseObstaculo.Clear();
        }

        private int CalcularVidaJugador()
        {
            int vidaBase = 20 + Math.Max(0, (nivelActual - 1) * 2);
            return Math.Min(40, vidaBase);
        }

        private void RestablecerJugadorParaNivel(int vidaJugador)
        {
            navex.Tag = vidaJugador;
            navex.Visible = true;

            int posicionXJugador = Math.Max(0, (contiene.Width - navex.Width) / 2);
            int posicionYJugador = Math.Max(contiene.Height - navex.Height - 30, contiene.Height / 2 + 50);
            posicionYJugador = Math.Min(posicionYJugador, Math.Max(0, contiene.Height - navex.Height));

            navex.Location = new Point(posicionXJugador, posicionYJugador);

            if (!contiene.Controls.Contains(navex))
            {
                contiene.Controls.Add(navex);
            }

            navex.BringToFront();
            label2.Text = $"Vida del Avión: {vidaJugador}";
        }

        private void ConfigurarEnemigosNivel()
        {
            enemigosActivos.Clear();
            contadorDisparoEnemigo.Clear();
            direccionHorizontalEnemiga.Clear();
            velocidadHorizontalEnemiga.Clear();

            int cantidadEnemigos = Math.Min(1 + nivelActual, 6);
            if (cantidadEnemigos <= 0)
            {
                enemigosRestantes = 0;
                return;
            }

            int anchoDisponible = Math.Max(1, contiene.Width - 60);
            int separacion = cantidadEnemigos <= 1 ? 0 : anchoDisponible / (cantidadEnemigos - 1);

            for (int i = 0; i < cantidadEnemigos; i++)
            {
                PictureBox enemigo = i == 0 ? naveRival : new PictureBox();

                int tipo = (nivelActual + i) % 3 + 1;
                Color color = Color.FromArgb(
                    Math.Clamp(70 + (nivelActual * 25) + (i * 15), 60, 240),
                    Math.Clamp(80 + (i * 30), 70, 240),
                    Math.Clamp(180 - (nivelActual * 10) + (i * 5), 90, 230));
                int vida = 18 + (nivelActual * 4) + (i * 3);

                CrearNave(enemigo, 180, tipo, color, vida);

                int posicionX = 30 + (i * separacion);
                posicionX = Math.Clamp(posicionX, 0, Math.Max(0, contiene.Width - enemigo.Width));
                int posicionY = 40 + (i % 2 == 0 ? 0 : 26);
                enemigo.Location = new Point(posicionX, posicionY);
                enemigo.Visible = true;

                if (!contiene.Controls.Contains(enemigo))
                {
                    contiene.Controls.Add(enemigo);
                }

                enemigo.BringToFront();

                enemigosActivos.Add(enemigo);
                contadorDisparoEnemigo[enemigo] = GeneradorAleatorio.Next(10, 35);
                direccionHorizontalEnemiga[enemigo] = i % 2 == 0 ? 1 : -1;
                velocidadHorizontalEnemiga[enemigo] = 1.6f + Math.Min(3.5f, nivelActual * 0.45f) + (i * 0.25f);

                ConfigurarBarraVidaEnemigo(enemigo, vida);
            }

            enemigosRestantes = ContarEnemigosVivos();
        }

        private void ConfigurarBarraVidaEnemigo(PictureBox enemigo, int vidaMaxima)
        {
            if (vidaMaxima <= 0)
            {
                return;
            }

            if (!barrasVidaEnemigo.TryGetValue(enemigo, out var barra))
            {
                var contenedor = new Panel
                {
                    Size = new Size(enemigo.Width, 6),
                    BackColor = Color.FromArgb(140, Color.Black),
                    Visible = enemigo.Visible,
                    Tag = "BarraVidaEnemigo"
                };

                var relleno = new Panel
                {
                    Size = new Size(Math.Max(0, contenedor.Width - 2), 4),
                    BackColor = Color.SpringGreen,
                    Location = new Point(1, 1)
                };

                contenedor.Controls.Add(relleno);
                contiene.Controls.Add(contenedor);

                barra = new BarraVidaEnemigo(contenedor, relleno, vidaMaxima);
                barrasVidaEnemigo[enemigo] = barra;
            }
            else
            {
                barra.VidaMaxima = vidaMaxima;
                barra.Contenedor.Size = new Size(enemigo.Width, barra.Contenedor.Height);
                if (barra.Contenedor.Parent != contiene)
                {
                    barra.Contenedor.Parent?.Controls.Remove(barra.Contenedor);
                    contiene.Controls.Add(barra.Contenedor);
                }

                barra.Contenedor.Visible = enemigo.Visible;
            }

            ActualizarPosicionBarraVidaEnemigo(enemigo);
            ActualizarBarraVidaEnemigo(enemigo, vidaMaxima);
        }

        private void ActualizarPosicionBarraVidaEnemigo(PictureBox enemigo)
        {
            if (!barrasVidaEnemigo.TryGetValue(enemigo, out var barra))
            {
                return;
            }

            barra.Contenedor.Size = new Size(enemigo.Width, barra.Contenedor.Height);
            int posicionX = enemigo.Left;
            int posicionY = Math.Max(0, enemigo.Top - barra.Contenedor.Height - 4);
            barra.Contenedor.Location = new Point(posicionX, posicionY);

            if (enemigo.Visible && Convert.ToInt32(enemigo.Tag) > 0)
            {
                barra.Contenedor.Visible = true;
                barra.Contenedor.BringToFront();
            }
            else
            {
                barra.Contenedor.Visible = false;
            }
        }

        private void ActualizarBarraVidaEnemigo(PictureBox enemigo, int vidaActual)
        {
            if (!barrasVidaEnemigo.TryGetValue(enemigo, out var barra) || barra.VidaMaxima <= 0)
            {
                return;
            }

            int anchoDisponible = Math.Max(0, barra.Contenedor.Width - 2);
            int anchoRelleno = vidaActual <= 0
                ? 0
                : (int)Math.Round((double)vidaActual / barra.VidaMaxima * anchoDisponible);

            barra.Relleno.Width = Math.Clamp(anchoRelleno, 0, anchoDisponible);

            barra.Relleno.BackColor = vidaActual <= barra.VidaMaxima * 0.3
                ? Color.OrangeRed
                : vidaActual <= barra.VidaMaxima * 0.6
                    ? Color.Gold
                    : Color.SpringGreen;

            barra.Contenedor.Visible = enemigo.Visible && vidaActual > 0;
            if (barra.Contenedor.Visible)
            {
                barra.Contenedor.BringToFront();
            }
        }

        private void QuitarBarraVidaEnemigo(PictureBox enemigo)
        {
            if (!barrasVidaEnemigo.TryGetValue(enemigo, out var barra))
            {
                return;
            }

            barrasVidaEnemigo.Remove(enemigo);

            if (barra.Contenedor.Parent != null)
            {
                barra.Contenedor.Parent.Controls.Remove(barra.Contenedor);
            }

            barra.Contenedor.Dispose();
        }

        private void ConfigurarObstaculosNivel()
        {
            foreach (var obstaculo in obstaculosActivos.ToArray())
            {
                contiene.Controls.Remove(obstaculo);
                obstaculo.Dispose();
            }

            obstaculosActivos.Clear();
            velocidadObstaculos.Clear();
            faseObstaculo.Clear();

            int cantidadObstaculos = Math.Min(3 + nivelActual, 8);
            for (int i = 0; i < cantidadObstaculos; i++)
            {
                int tamano = GeneradorAleatorio.Next(24, 48);
                var obstaculo = new PictureBox
                {
                    Size = new Size(tamano, tamano),
                    BackColor = Color.Transparent,
                    Tag = "Obstaculo"
                };

                obstaculo.Image = CrearImagenObstaculo(obstaculo.Size, i);
                obstaculo.Location = new Point(
                    GeneradorAleatorio.Next(0, Math.Max(1, contiene.Width - obstaculo.Width)),
                    GeneradorAleatorio.Next(100, Math.Max(120, contiene.Height / 2)));

                contiene.Controls.Add(obstaculo);
                obstaculo.BringToFront();

                obstaculosActivos.Add(obstaculo);
                velocidadObstaculos[obstaculo] = new PointF(
                    (float)(GeneradorAleatorio.NextDouble() * 1.6 - 0.8),
                    0.8f + (nivelActual * 0.25f));
                faseObstaculo[obstaculo] = GeneradorAleatorio.Next(0, 360);
            }
        }

        private static Image CrearImagenObstaculo(Size tamano, int indice)
        {
            var meteorito = new Bitmap(tamano.Width, tamano.Height);
            using (var grafico = Graphics.FromImage(meteorito))
            {
                grafico.SmoothingMode = SmoothingMode.AntiAlias;
                grafico.Clear(Color.Transparent);

                Rectangle contenedor = new Rectangle(Point.Empty, tamano);
                using var camino = new GraphicsPath();
                camino.AddEllipse(contenedor);

                using var gradiente = new PathGradientBrush(camino)
                {
                    CenterColor = Color.FromArgb(220, 255, 200, 120),
                    SurroundColors = new[]
                    {
                        Color.FromArgb(255, 120 + (indice * 10 % 80), 60, 25),
                        Color.FromArgb(255, 90 + (indice * 15 % 90), 40, 18),
                        Color.FromArgb(255, 110 + (indice * 12 % 70), 55, 20)
                    }
                };

                grafico.FillEllipse(gradiente, contenedor);

                using var contorno = new Pen(Color.FromArgb(200, 40, 20), 2f);
                grafico.DrawEllipse(contorno, contenedor);

                using var brillo = new LinearGradientBrush(
                    new Rectangle(0, 0, tamano.Width, tamano.Height / 2),
                    Color.FromArgb(180, Color.WhiteSmoke),
                    Color.Transparent,
                    LinearGradientMode.Vertical);
                grafico.FillEllipse(brillo, new Rectangle(4, 4, tamano.Width - 8, tamano.Height / 2));
            }

            return meteorito;
        }

        private int ContarEnemigosVivos()
        {
            return enemigosActivos.Count(enemigo => enemigo.Visible && Convert.ToInt32(enemigo.Tag) > 0);
        }

        private void ActualizarIndicadoresNivel(bool resaltarEnemigos)
        {
            enemigosRestantes = ContarEnemigosVivos();
            label1.Text = $"Nivel {nivelActual} - Rivales activos: {enemigosRestantes}";
            if (resaltarEnemigos)
            {
                DestacarEtiqueta(label1);
            }

            int vidaJugador = Math.Max(0, Convert.ToInt32(navex.Tag));
            label2.Text = $"Vida del Avión: {vidaJugador}";
        }

        private void AvanzarNivel()
        {
            if (enTransicionNivel)
            {
                return;
            }

            enTransicionNivel = true;
            tiempo?.Stop();

            if (nivelActual >= NivelMaximo)
            {
                MostrarMensajeFinal(true);
                return;
            }

            labelEstado.Text = $"Nivel {nivelActual} completado. Preparando el siguiente desafío...";

            var transicion = new System.Windows.Forms.Timer { Interval = 1100 };
            transicion.Tick += (_, _) =>
            {
                transicion.Stop();
                transicion.Dispose();

                nivelActual++;
                ConfigurarNivelActual();

                enTransicionNivel = false;
                tiempo?.Start();
            };
            transicion.Start();
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
                if (enTransicionNivel)
                {
                    return;
                }

                ActualizarDisparoJugador();
                MoverJugador();
                MoverNaveRival();
                MoverObstaculos();
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

        private void ActualizarVida(PictureBox objetivo, Label indicador, int puntos = 1)
        {
            int vidaActual = Math.Max(0, Convert.ToInt32(objetivo.Tag) - puntos);
            objetivo.Tag = vidaActual;

            if (objetivo == navex)
            {
                label2.Text = $"Vida del Avión: {vidaActual}";
                DestacarEtiqueta(label2);

                if (vidaActual <= 0)
                {
                    navex.Visible = false;
                    MostrarMensajeFinal(false);
                }

                return;
            }

            if (!enemigosActivos.Contains(objetivo))
            {
                return;
            }

            ActualizarBarraVidaEnemigo(objetivo, vidaActual);

            if (vidaActual <= 0)
            {
                EliminarEnemigo(objetivo);
            }
            else
            {
                ActualizarIndicadoresNivel(true);
            }
        }

        private void MostrarMensajeFinal(bool jugadorGana)
        {
            moverJugadorHaciaArriba = false;
            moverJugadorHaciaAbajo = false;
            moverJugadorHaciaDerecha = false;
            moverJugadorHaciaIzquierda = false;
            disparoActivo = false;
            enTransicionNivel = false;

            tiempo?.Stop();

            labelEstado.Text = jugadorGana
                ? $"Operación completada. Alcanzaste el nivel {nivelActual}."
                : $"Misión fallida en el nivel {nivelActual}. Pulsa Enter para reintentar.";

            LimpiarElementosTemporales();
            navex.Visible = false;

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
                    ? $"Control total del sector tras superar {NivelMaximo} niveles."
                    : "El escuadrón rival dominó el cielo, pero la academia confía en tu regreso.";

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
                    if (tiempo == null || !tiempo.Enabled || !navex.Visible || botonReintentar.Visible)
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
            if (enemigosActivos.Count == 0)
            {
                return;
            }

            foreach (var enemigo in enemigosActivos.ToArray())
            {
                if (!enemigo.Visible)
                {
                    continue;
                }

                int contador = contadorDisparoEnemigo.TryGetValue(enemigo, out int valorActual) ? valorActual + 1 : 1;
                contadorDisparoEnemigo[enemigo] = contador;

                int cadencia = ObtenerCadenciaDisparoNivel(enemigo);
                if (contador >= cadencia)
                {
                    int xRival = enemigo.Left + (enemigo.Width / 2);
                    int yRival = enemigo.Top + (enemigo.Height / 2);
                    CrearMisil(180, Color.OrangeRed, "Rival", xRival, yRival);
                    contadorDisparoEnemigo[enemigo] = 0;
                }

                float velocidad = velocidadHorizontalEnemiga.TryGetValue(enemigo, out float velocidadGuardada)
                    ? velocidadGuardada
                    : 2f;
                int direccion = direccionHorizontalEnemiga.TryGetValue(enemigo, out int direccionGuardada)
                    ? direccionGuardada
                    : 1;

                float nuevoX = enemigo.Left + (velocidad * direccion);
                if (nuevoX <= 0)
                {
                    nuevoX = 0;
                    direccion = 1;
                }
                else if (nuevoX >= contiene.Width - enemigo.Width)
                {
                    nuevoX = contiene.Width - enemigo.Width;
                    direccion = -1;
                }

                direccionHorizontalEnemiga[enemigo] = direccion;
                enemigo.Left = (int)Math.Round(nuevoX);

                double factorTiempo = (Environment.TickCount / 120.0) + enemigosActivos.IndexOf(enemigo);
                int oscilacion = (int)Math.Round(Math.Sin(factorTiempo) * 2.5);
                int nuevoY = Math.Clamp(enemigo.Top + oscilacion, 20, Math.Max(20, contiene.Height / 2 - 40));
                enemigo.Top = nuevoY;

                ActualizarPosicionBarraVidaEnemigo(enemigo);
            }
        }

        private int ObtenerCadenciaDisparoNivel(PictureBox enemigo)
        {
            int indice = Math.Max(0, enemigosActivos.IndexOf(enemigo));
            int cadenciaBase = Math.Max(32, 110 - (nivelActual * 12));
            int ajuste = Math.Max(0, indice * 5);
            return Math.Max(28, cadenciaBase - ajuste);
        }

        private void ProcesarProyectiles()
        {
            var proyectiles = contiene.Controls
                .OfType<PictureBox>()
                .Where(control => control.Tag is string nombre && (nombre == "Misil" || nombre == "Rival"))
                .ToArray();

            foreach (var misil in proyectiles)
            {
                if (misil.IsDisposed)
                {
                    continue;
                }

                string nombre = misil.Tag as string ?? string.Empty;
                Rectangle posicionActual = misil.Bounds;

                if (nombre == "Misil")
                {
                    bool impacto = false;

                    foreach (var obstaculo in obstaculosActivos.ToArray())
                    {
                        if (!obstaculo.Visible)
                        {
                            continue;
                        }

                        if (posicionActual.IntersectsWith(obstaculo.Bounds))
                        {
                            misil.Dispose();
                            ReubicarObstaculo(obstaculo);
                            impacto = true;
                            break;
                        }
                    }

                    if (impacto)
                    {
                        continue;
                    }

                    foreach (var enemigo in enemigosActivos.ToArray())
                    {
                        if (!enemigo.Visible)
                        {
                            continue;
                        }

                        if (MetricaImpactoCompleto(posicionActual, enemigo.Bounds))
                        {
                            misil.Dispose();
                            if (!ZonaCentral(posicionActual, enemigo.Bounds))
                            {
                                ActualizarVida(enemigo, label1);
                            }

                            impacto = true;
                            break;
                        }
                    }

                    if (impacto)
                    {
                        continue;
                    }

                    misil.Top -= VelocidadMisilJugador;
                    if (misil.Bottom <= 0)
                    {
                        misil.Dispose();
                    }
                }
                else if (nombre == "Rival")
                {
                    bool interrumpido = false;

                    foreach (var obstaculo in obstaculosActivos.ToArray())
                    {
                        if (!obstaculo.Visible)
                        {
                            continue;
                        }

                        if (posicionActual.IntersectsWith(obstaculo.Bounds))
                        {
                            misil.Dispose();
                            ReubicarObstaculo(obstaculo);
                            interrumpido = true;
                            break;
                        }
                    }

                    if (interrumpido)
                    {
                        continue;
                    }

                    if (navex.Visible && MetricaImpactoCompleto(posicionActual, navex.Bounds))
                    {
                        misil.Dispose();
                        if (!ZonaCentral(posicionActual, navex.Bounds))
                        {
                            ActualizarVida(navex, label2);
                        }

                        continue;
                    }

                    misil.Top += ObtenerVelocidadMisilRival();
                    if (misil.Top >= contiene.Height)
                    {
                        misil.Dispose();
                    }
                }
            }
        }

        private void MoverObstaculos()
        {
            if (obstaculosActivos.Count == 0)
            {
                return;
            }

            foreach (var obstaculo in obstaculosActivos.ToArray())
            {
                if (obstaculo.IsDisposed)
                {
                    continue;
                }

                PointF velocidad = velocidadObstaculos.TryGetValue(obstaculo, out PointF velocidadActual)
                    ? velocidadActual
                    : new PointF(0f, 1f + (nivelActual * 0.2f));

                int fase = faseObstaculo.TryGetValue(obstaculo, out int faseActual) ? faseActual : 0;
                fase = (fase + 6) % 360;
                faseObstaculo[obstaculo] = fase;

                float desplazamientoSuave = (float)Math.Sin(fase * (Math.PI / 180f)) * 2f;

                int nuevoX = (int)Math.Round(obstaculo.Left + velocidad.X + desplazamientoSuave);
                int nuevoY = (int)Math.Round(obstaculo.Top + velocidad.Y);

                if (nuevoX <= 0 || nuevoX >= contiene.Width - obstaculo.Width)
                {
                    velocidad.X *= -1;
                    velocidadObstaculos[obstaculo] = velocidad;
                    nuevoX = Math.Clamp(nuevoX, 0, Math.Max(0, contiene.Width - obstaculo.Width));
                }
                else
                {
                    velocidadObstaculos[obstaculo] = velocidad;
                }

                if (nuevoY >= contiene.Height - obstaculo.Height)
                {
                    ReubicarObstaculo(obstaculo);
                    continue;
                }

                obstaculo.Location = new Point(nuevoX, nuevoY);
            }
        }

        private void ReubicarObstaculo(PictureBox obstaculo)
        {
            if (obstaculo.IsDisposed)
            {
                return;
            }

            int x = GeneradorAleatorio.Next(0, Math.Max(1, contiene.Width - obstaculo.Width));
            int y = GeneradorAleatorio.Next(40, Math.Max(80, contiene.Height / 3));
            obstaculo.Location = new Point(x, y);

            velocidadObstaculos[obstaculo] = new PointF(
                (float)(GeneradorAleatorio.NextDouble() * 1.6 - 0.8),
                0.9f + (nivelActual * 0.25f));
            faseObstaculo[obstaculo] = GeneradorAleatorio.Next(0, 360);
        }

        private void ObstaculoImpactaJugador(PictureBox obstaculo)
        {
            if (!navex.Visible)
            {
                return;
            }

            ActualizarVida(navex, label2, 2);
            ReubicarObstaculo(obstaculo);
        }

        private int ObtenerVelocidadMisilRival()
        {
            return Math.Min(12, VelocidadMisilRival + Math.Max(0, nivelActual - 1));
        }

        private void EliminarEnemigo(PictureBox enemigo)
        {
            enemigo.Visible = false;
            enemigosActivos.Remove(enemigo);
            contadorDisparoEnemigo.Remove(enemigo);
            direccionHorizontalEnemiga.Remove(enemigo);
            velocidadHorizontalEnemiga.Remove(enemigo);
            QuitarBarraVidaEnemigo(enemigo);

            if (!ReferenceEquals(enemigo, naveRival))
            {
                contiene.Controls.Remove(enemigo);
                enemigo.Dispose();
            }

            ActualizarIndicadoresNivel(true);

            if (!navex.Visible)
            {
                return;
            }

            if (ContarEnemigosVivos() <= 0)
            {
                AvanzarNivel();
            }
        }

        private void VerificarColisionDirecta()
        {
            if (!navex.Visible)
            {
                return;
            }

            foreach (var enemigo in enemigosActivos.ToArray())
            {
                if (!enemigo.Visible)
                {
                    continue;
                }

                if (!navex.Bounds.IntersectsWith(enemigo.Bounds))
                {
                    continue;
                }

                ActualizarVida(navex, label2, 3);
                if (navex.Visible && Convert.ToInt32(navex.Tag) > 0)
                {
                    EliminarEnemigo(enemigo);
                }
                break;
            }

            foreach (var obstaculo in obstaculosActivos.ToArray())
            {
                if (!obstaculo.Visible)
                {
                    continue;
                }

                if (navex.Bounds.IntersectsWith(obstaculo.Bounds))
                {
                    ObstaculoImpactaJugador(obstaculo);
                }
            }
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
