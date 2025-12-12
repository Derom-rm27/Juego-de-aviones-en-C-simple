using System;

namespace Calidad_Juego.Modelos
{
    internal static class NaveFactory
    {
        public const int TipoNavePrincipal = 4;

        private static NaveFiguraEscalada? navePrincipalCache;

        public static NaveFiguraEscalada ObtenerNavePrincipal(float escala = 1f)
        {
            if (Math.Abs(escala - 1f) < 0.01f && navePrincipalCache != null)
            {
                return navePrincipalCache;
            }

            var plantilla = NavePlantilla.Obtener(TipoNavePrincipal);
            var figura = plantilla.Escalar(escala);

            if (Math.Abs(escala - 1f) < 0.01f)
            {
                navePrincipalCache = figura;
            }

            return figura;
        }
    }
}
