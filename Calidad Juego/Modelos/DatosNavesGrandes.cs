using System.Collections.Generic;
using System.Drawing;

namespace Calidad_Juego.Modelos
{
    // Este es el archivo coordinador
    public static partial class DatosNavesGrandes
    {
        public static List<PointF[]> ObtenerNaveCompleta()
        {
            var naveCompleta = new List<PointF[]>();

            CargarParte1(naveCompleta);
            CargarParte2(naveCompleta);
            CargarParte3(naveCompleta);
            CargarParte4(naveCompleta);
            CargarParte5(naveCompleta);
            CargarParte6(naveCompleta);
            CargarParte7(naveCompleta);
            CargarParte8(naveCompleta);
            CargarParte9(naveCompleta);
            CargarParte10(naveCompleta);
            CargarParte11(naveCompleta);
            CargarParte12(naveCompleta);
            CargarParte13(naveCompleta);

            return naveCompleta;
        }
    }
}