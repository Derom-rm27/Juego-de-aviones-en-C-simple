using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Calidad_Juego.Modelos
{
    internal sealed class NavePlantilla
    {
        private NavePlantilla(int tipo, IEnumerable<PointF[]> contornos, SizeF tamanoReferencia, bool usarLineas = false, float escalaPreferida = 1f)
        {
            Tipo = tipo;
            Contornos = contornos.Select(contorno => contorno.ToArray()).ToArray();
            TamanoReferencia = tamanoReferencia;
            UsarLineas = usarLineas;
            EscalaPreferida = escalaPreferida;
        }

        public int Tipo { get; }
        public IReadOnlyList<PointF[]> Contornos { get; }
        public SizeF TamanoReferencia { get; }
        public bool UsarLineas { get; }
        public float EscalaPreferida { get; }

        public static NavePlantilla CrearDesdeDatos(int tipo, IEnumerable<PointF[]> contornos, SizeF tamanoReferencia, bool usarLineas = false, float escalaPreferida = 1f)
        {
            return new NavePlantilla(tipo, contornos, tamanoReferencia, usarLineas, escalaPreferida);
        }

        public static NavePlantilla Obtener(int tipo)
        {
            return tipo switch
            {
                1 => Tipo1,
                2 => Tipo2,
                3 => Tipo3,
                4 => NavePrincipal,
                _ => Tipo1
            };
        }

        public NaveFiguraEscalada Escalar(float escala = 1f)
        {
            float minX = Contornos.Min(contorno => contorno.Min(p => p.X));
            float minY = Contornos.Min(contorno => contorno.Min(p => p.Y));
            float maxX = Contornos.Max(contorno => contorno.Max(p => p.X));
            float maxY = Contornos.Max(contorno => contorno.Max(p => p.Y));

            float anchoBase = Math.Max(1f, maxX - minX);
            float altoBase = Math.Max(1f, maxY - minY);
            float factorX = TamanoReferencia.Width > 0 ? TamanoReferencia.Width / anchoBase : 1f;
            float factorY = TamanoReferencia.Height > 0 ? TamanoReferencia.Height / altoBase : 1f;
            float factor = Math.Min(factorX, factorY) * Math.Max(0.05f, EscalaPreferida) * Math.Max(0.1f, escala);

            var contornosEscalados = new List<Point[]>();
            foreach (var contorno in Contornos)
            {
                contornosEscalados.Add(contorno
                    .Select(p => new Point(
                        (int)Math.Round((p.X - minX) * factor),
                        (int)Math.Round((p.Y - minY) * factor)))
                    .ToArray());
            }

            Size tamano = new(
                Math.Max(1, (int)Math.Ceiling(anchoBase * factor)),
                Math.Max(1, (int)Math.Ceiling(altoBase * factor)));

            return new NaveFiguraEscalada(contornosEscalados, tamano, UsarLineas);
        }

        private static PointF[] Convertir(Point[] puntos)
        {
            return puntos.Select(p => new PointF(p.X, p.Y)).ToArray();
        }

        private static readonly NavePlantilla NavePrincipal = CrearDesdeDatos(
            4,
            DatosNavesGrandes.ObtenerNaveCompleta(),
            new SizeF(280f, 260f),
            usarLineas: true,
            escalaPreferida: 0.35f);

        private static readonly NavePlantilla Tipo1 = new(
            1,
            new[]
            {
                Convertir(new[]
                {
                    new Point(29, 0), new Point(30, 1), new Point(30, 6), new Point(31, 6), new Point(31, 11), new Point(32, 11),
                    new Point(32, 17), new Point(35, 17), new Point(35, 16), new Point(37, 16), new Point(37, 17),
                    new Point(38, 18), new Point(38, 28), new Point(39, 28), new Point(42, 39), new Point(44, 45),
                    new Point(50, 51), new Point(51, 51), new Point(51, 52), new Point(58, 59), new Point(58, 66),
                    new Point(44, 66), new Point(39, 71), new Point(35, 71), new Point(35, 74), new Point(32, 77),
                    new Point(26, 77), new Point(23, 74), new Point(23, 71), new Point(19, 71), new Point(14, 66),
                    new Point(0, 66), new Point(0, 59), new Point(7, 52), new Point(7, 51), new Point(8, 51),
                    new Point(14, 45), new Point(16, 39), new Point(19, 28), new Point(20, 28), new Point(20, 18),
                    new Point(21, 17), new Point(21, 16), new Point(23, 16), new Point(23, 17), new Point(26, 17),
                    new Point(26, 11), new Point(27, 11), new Point(27, 6), new Point(28, 6), new Point(28, 1), new Point(29, 0)
                })
            },
            new SizeF(58, 77));

        private static readonly NavePlantilla Tipo2 = new(
            2,
            new[]
            {
                Convertir(new[]
                {
                    new Point(24, 0), new Point(29, 5), new Point(29, 18), new Point(32, 21), new Point(34, 21),
                    new Point(38, 17), new Point(41, 20), new Point(41, 30), new Point(47, 36), new Point(47, 41),
                    new Point(41, 41), new Point(38, 44), new Point(36, 44), new Point(33, 41), new Point(30, 41),
                    new Point(25, 46), new Point(22, 46), new Point(17, 41), new Point(14, 41), new Point(11, 44),
                    new Point(9, 44), new Point(6, 41), new Point(0, 41), new Point(0, 36), new Point(6, 30),
                    new Point(6, 20), new Point(9, 17), new Point(13, 21), new Point(15, 21), new Point(18, 18),
                    new Point(18, 5), new Point(23, 0)
                })
            },
            new SizeF(47, 46),
            usarLineas: true);

        private static readonly NavePlantilla Tipo3 = new(
            3,
            new[]
            {
                Convertir(new[]
                {
                    new Point(25, 5), new Point(26, 54), new Point(26, 5), new Point(26, 50), new Point(27, 50),
                    new Point(28, 50), new Point(29, 50), new Point(30, 51), new Point(31, 51), new Point(32, 52),
                    new Point(32, 49), new Point(31, 48), new Point(30, 47), new Point(29, 46), new Point(28, 45),
                    new Point(27, 44), new Point(27, 36), new Point(28, 35), new Point(28, 25), new Point(29, 25),
                    new Point(30, 25), new Point(31, 25), new Point(32, 26), new Point(33, 26), new Point(34, 27),
                    new Point(35, 28), new Point(36, 8), new Point(37, 29), new Point(38, 30), new Point(39, 30),
                    new Point(40, 31), new Point(41, 32), new Point(42, 32), new Point(43, 33), new Point(44, 34),
                    new Point(45, 35), new Point(46, 36), new Point(47, 36), new Point(48, 36), new Point(49, 37),
                    new Point(50, 37), new Point(51, 38), new Point(51, 37), new Point(51, 36), new Point(50, 35),
                    new Point(50, 35), new Point(37, 15), new Point(37, 15), new Point(36, 14), new Point(35, 14),
                    new Point(34, 15), new Point(34, 21), new Point(28, 15), new Point(28, 7), new Point(27, 6),
                    new Point(26, 5), new Point(25, 5), new Point(24, 6), new Point(23, 7), new Point(23, 15),
                    new Point(17, 21), new Point(17, 15), new Point(16, 14), new Point(15, 14), new Point(14, 15),
                    new Point(14, 22), new Point(13, 25), new Point(12, 30), new Point(11, 31), new Point(10, 32),
                    new Point(9, 32), new Point(8, 33), new Point(7, 34), new Point(6, 35), new Point(5, 36),
                    new Point(4, 35), new Point(3, 36), new Point(2, 37), new Point(1, 37), new Point(0, 38),
                    new Point(0, 39), new Point(0, 40), new Point(1, 40), new Point(2, 40), new Point(3, 40),
                    new Point(4, 41), new Point(5, 42), new Point(6, 42), new Point(7, 42), new Point(8, 41),
                    new Point(9, 40), new Point(10, 40), new Point(11, 40), new Point(12, 40), new Point(13, 40),
                    new Point(14, 41), new Point(15, 42), new Point(16, 41), new Point(17, 40), new Point(18, 40),
                    new Point(19, 40), new Point(20, 40), new Point(21, 40), new Point(22, 40), new Point(23, 40),
                    new Point(24, 41), new Point(25, 42), new Point(26, 42), new Point(27, 42), new Point(28, 41),
                    new Point(29, 40), new Point(30, 40), new Point(31, 40), new Point(32, 40), new Point(33, 39),
                    new Point(33, 38), new Point(33, 37), new Point(33, 36), new Point(33, 35), new Point(33, 34),
                    new Point(33, 33), new Point(32, 32), new Point(31, 31), new Point(30, 30), new Point(29, 29),
                    new Point(28, 28), new Point(27, 27), new Point(26, 26), new Point(25, 25), new Point(24, 24),
                    new Point(23, 23), new Point(22, 22), new Point(22, 21), new Point(22, 20), new Point(22, 19),
                    new Point(22, 18), new Point(22, 17), new Point(22, 16), new Point(22, 15), new Point(21, 14),
                    new Point(20, 13), new Point(20, 12), new Point(20, 11), new Point(21, 10), new Point(22, 9),
                    new Point(23, 9), new Point(24, 9), new Point(25, 9), new Point(26, 9), new Point(25, 8),
                    new Point(24, 7), new Point(23, 6), new Point(22, 5), new Point(21, 4), new Point(20, 3),
                    new Point(19, 3), new Point(18, 2), new Point(17, 1), new Point(16, 0)
                })
            },
            new SizeF(51, 54));

    }

    internal sealed record NaveFiguraEscalada(IReadOnlyList<Point[]> Contornos, Size Tamano, bool UsarLineas);
}
