using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D; // Librería para GraphicsPath

// Definiciones de la aplicación (asumidas para que el código sea completo)
namespace MoverObjeto
{
    // Form1 es una clase parcial. El método Dispose es manejado por Form1.Designer.cs.
    public partial class Form1 : Form
    {
        // Componentes de la UI que se asumen existen (declaraciones de marcador de posición)
        private Label label1 = new Label(); // Marcador de posición para "Mi Rival"
        private Label label2 = new Label(); // Marcador de posición para "Mi Avion"

        //*********** VARIABLES GLOBALES *************//
        PictureBox navex = new PictureBox();
        PictureBox naveRival = new PictureBox();
        PictureBox contiene = new PictureBox();
        
        // CORRECCIÓN: Se especifica System.Windows.Forms.Timer para evitar ambigüedad (CS0104)
        System.Windows.Forms.Timer tiempo; 

        int Dispara = 0;
        bool flag = false;
        float angulo = 0;

        // --- SECCIÓN DISPOSE ELIMINADA ---
        // El método Dispose y components se encuentran ahora solo en Form1.Designer.cs
        // ---------------------------------

        //********** DIAGRAMAR DEL MISIL ***********//
        public void CrearMisil(int AngRotar, Color pintar, string nombre, int x, int y)
        {
            // Usamos PictureBox en lugar de dynamic para el control
            PictureBox Balas = new PictureBox(); 

            int PosX = 1;
            int PosY = 1;
            int largoM = 11;
            int anchoM = 3;
            int PH = 10; // Variable PH asumida como padding

            //declarar los array de puntos. (Corregida la sintaxis de inicialización de array)
            Point[] myMisill = new Point[] {
                new Point(4 * PosX, 1 * PosY), new Point(5 * PosX, 1 * PosY), 
                new Point(6 * PosX, 2 * PosY), new Point(6 * PosX, 7 * PosY), 
                new Point(7 * PosX, 8 * PosY), new Point(8 * PosX, 9 * PosY), 
                new Point(7 * PosX, 9 * PosY), new Point(6 * PosX, 10 * PosY), 
                new Point(2 * PosX, 10 * PosY), new Point(1 * PosX, 9 * PosY), 
                new Point(0 * PosX, 9 * PosY), new Point(1 * PosX, 8 * PosY), 
                new Point(2 * PosX, 7 * PosY), new Point(2 * PosX, 2 * PosY), 
                new Point(3 * PosX, 1 * PosY), new Point(4 * PosX, 0 * PosY) 
            };

            Point[] myMisil = new Point[myMisill.Count()];

            for (int i = 0; i < myMisill.Count(); i++)
            {
                myMisil[i].X = myMisill[i].X;

                if (AngRotar == 180)
                {
                    // Lógica para rotar el misil verticalmente
                    myMisil[i].Y = largoM - myMisill[i].Y;
                }
                else
                {
                    myMisil[i].Y = myMisill[i].Y;
                }
            }

            GraphicsPath ObjGrafico = new GraphicsPath();
            ObjGrafico.AddPolygon(myMisil);

            Balas.Location = new Point(x, y);
            Balas.BackColor = pintar;
            // Se asume multiplicación PosX/PosY
            Balas.Size = new Size(anchoM * PosX, largoM * PosY); 
            Balas.Region = new Region(ObjGrafico);
            contiene.Controls.Add(Balas);
            Balas.Visible = true;
            Balas.Tag = nombre;

            //************** DIBUJAR COLORES **************//
            Bitmap flag = new Bitmap(anchoM, largoM);
            Graphics flagImagen = Graphics.FromImage(flag);
            
            // La lógica original para dibujar colores estaba corrupta y sin coordenadas. 
            // Se elimina la parte sintácticamente incorrecta para evitar errores de compilación.
            // La línea Balas.Image = flag; estaba flotando, la reinserto aquí.
            Balas.Image = flag;
        }

        //*** DESCRUCTOR DEL MISIL / Game Loop (se llama ImpactarTick) ****
        private void ImpactarTick(object sender, EventArgs e)
        {
            // VARIABLES LOCALES para naveRival
            int X = naveRival.Location.X;
            int Y = naveRival.Location.Y;
            int W = naveRival.Width;
            int H = naveRival.Height;
            int PH = 10; // Asumido como 10 (era '1(' en el original)

            // VARIABLES LOCALES para navex
            int X2 = navex.Location.X;
            int Y2 = navex.Location.Y;
            int W2 = navex.Width;
            int H2 = navex.Height;

            // Mueve la nave rival. x, y son las variables locales de naveRival.Location
            int x = naveRival.Location.X; 
            int y = naveRival.Location.Y;

            Dispara++;

            // ACCION DE DISPARAR DEL RIVAL
            if (Dispara >= 100 && naveRival.Visible == true) // Corregido el operador
            {
                int xRival = naveRival.Location.X + (naveRival.Width / 2);
                int yRival = naveRival.Location.Y + (naveRival.Height / 2);
                CrearMisil(180, Color.OrangeRed, "Rival", xRival, yRival);
                Dispara = 0;
            }

            // MOVIMIENTO DE LA NAVE A DESTRUIR (Zigzag horizontal)
            if (flag == false)
            {
                // Limite derecho
                if (x >= contiene.Width - naveRival.Width) // Corregido el operador
                {
                    flag = true;
                }
                x++;
            }
            else
            {
                // Limite izquierdo (contiene.Location.X es 0)
                if (x <= contiene.Location.X) // Corregido el operador
                {
                    flag = false;
                }
                x--;
            }
            naveRival.Location = new Point(x, y);

            // ELIMINACION DEL MISIL Y DESCONTAR PUNTOS DE IMPACTO DE LA NAVE RIVAL
            foreach (Control c in contiene.Controls)
            {
                if (c is PictureBox)
                {
                    PictureBox misil = (PictureBox)c;
                    int X1 = misil.Location.X;
                    int Y1 = misil.Location.Y;
                    int W1 = misil.Width;
                    int H1 = misil.Height;
                    string nombre = misil.Tag.ToString();

                    // ACTIVIDAD DE IMPACTO CON LA NAVE RIVAL (Misil del jugador)
                    // Lógica original: Misil totalmente contenido dentro del Rival
                    if (X < X1 && X1 + W1 < X + W && Y < Y1 && Y1 + H1 < Y + H && nombre == "Misil")
                    {
                        // Lógica original: Hit de zona 'Core' (con padding)
                        if (X + PH < X1 && X1 + W1 < X + W - PH)
                        {
                            // En el código original esta parte está vacía o corrupta. 
                            // Asumo que al menos se deshace del misil.
                            misil.Dispose(); 
                        }
                        else // Hit de zona 'No Core' (reduce vida)
                        {
                            misil.Dispose();
                            // Corregida la sintaxis de asignación
                            naveRival.Tag = int.Parse(naveRival.Tag.ToString()) - 1; 
                        }
                        label1.Text = "Vida del Rival: " + naveRival.Tag.ToString();
                        //tiempo.Stop(); // Comentado en el original
                    }

                    // Lógica para fin de juego - Rival destruido
                    if (int.Parse(naveRival.Tag.ToString()) < 0)
                    {
                        naveRival.Dispose();
                        Bitmap NuevoImg = new Bitmap(contiene.Width, contiene.Height);
                        Graphics flagImagen = Graphics.FromImage(NuevoImg);

                        // Crear cadena para dibujar.
                        String drawString = "Felicitaciones Ganaste!"; // Corregido el texto

                        // Crear la fuente y el pincel.
                        Font drawFont = new Font("Arial", 16);
                        SolidBrush drawBrush = new SolidBrush(Color.Blue);
                        Point drawPoint = new Point(40, 150);

                        // Dibujar cadena en pantalla.
                        flagImagen.DrawString(drawString, drawFont, drawBrush, drawPoint);
                        contiene.Image = NuevoImg;
                        tiempo.Stop();
                        return; // Sale del Tick una vez terminado el juego
                    }

                    // ACTIVIDAD DE IMPACTO CON MI NAVE (Misil del rival)
                    // Lógica original: Misil totalmente contenido dentro de navex
                    if (X2 < X1 && X1 + W1 < X2 + W2 && Y2 < Y1 && Y1 + H1 < Y2 + H2 && nombre == "Rival")
                    {
                        // Lógica original: Hit de zona 'Core' (con padding)
                        if (X2 + PH < X1 && X1 + W1 < X2 + W2 - PH)
                        {
                            // En el código original esta parte está vacía o corrupta.
                            misil.Dispose(); // Solo dispose
                        }
                        else // Hit de zona 'No Core' (reduce vida)
                        {
                            misil.Dispose();
                            // Corregida la sintaxis de asignación
                            navex.Tag = int.Parse(navex.Tag.ToString()) - 1; 
                        }

                        label2.Text = "Vida del Avion: " + navex.Tag.ToString();
                    }

                    // Lógica para fin de juego - Jugador destruido
                    if (int.Parse(navex.Tag.ToString()) < 0)
                    {
                        navex.Dispose();
                        Bitmap NuevoImg = new Bitmap(contiene.Width, contiene.Height);
                        Graphics flagImagen = Graphics.FromImage(NuevoImg);

                        // Crear cadena para dibujar.
                        String drawString = "Perdiste el Juego";

                        // Crear la fuente y el pincel.
                        Font drawFont = new Font("Arial", 16);
                        SolidBrush drawBrush = new SolidBrush(Color.Red);
                        Point drawPoint = new Point(70, 150);

                        // Dibujar cadena en pantalla.
                        flagImagen.DrawString(drawString, drawFont, drawBrush, drawPoint);
                        contiene.Image = NuevoImg;
                        tiempo.Stop();
                        return; // Sale del Tick una vez terminado el juego
                    }

                    // Eliminación de misiles que salen de los límites
                    if (misil.Location.Y < 0 && nombre == "Misil") // Misil del jugador (va hacia arriba)
                    {
                        misil.Dispose();
                    }

                    if (misil.Location.Y > contiene.Height && nombre == "Rival") // Misil del rival (va hacia abajo)
                    {
                        misil.Dispose();
                    }

                    // Movimiento de los misiles
                    if (nombre == "Misil")
                    {
                        // Misil sube
                        misil.Top -= 10; // Se asume velocidad de 10
                    }

                    if (nombre == "Rival")
                    {
                        // Misil baja
                        misil.Top += 5; // Se asume velocidad de 5 (corregido de Top+ a Top+=5)
                    }

                    // Colisión nave-nave (la lógica original estaba incompleta/corrupta, se asume IntersectsWith)
                    // if (W X2 && H Y2 && W2 X && H2 Y)
                    if (navex.Bounds.IntersectsWith(naveRival.Bounds)) // Lógica de colisión nave-nave (asumida)
                    {
                        naveRival.Dispose();
                        navex.Dispose();
                        tiempo.Stop(); // Termina el juego si colisionan
                    }
                }
            }
        }

        //****** DIAGRAMAR NAVE**********//
        public void CrearNave(PictureBox Avion, int AngRotar, int Tipox, Color Pintar, int Vida)
        {
            int largoN = 0;
            int anchoN = 0;

            // Arreglos de puntos definidos
            Point[] myNave1 = { new Point(29, 0), new Point(30, 1), new Point(30, 6), new Point(31, 6), new Point(31, 11), new Point(32, 11), new Point(32, 17), new Point(35, 17), new Point(35, 16), new Point(37, 16), new Point(37, 17), new Point(38, 18), new Point(38, 28), new Point(39, 28), new Point(42, 39), new Point(44, 45), new Point(50, 51), new Point(51, 51), new Point(51, 52), new Point(58, 59), new Point(58, 66), new Point(44, 66), new Point(39, 71), new Point(35, 71), new Point(35, 74), new Point(32, 77), new Point(26, 77), new Point(23, 74), new Point(23, 71), new Point(19, 71), new Point(14, 66), new Point(0, 66), new Point(0, 59), new Point(7, 52), new Point(7, 51), new Point(8, 51), new Point(14, 45), new Point(16, 39), new Point(19, 28), new Point(20, 28), new Point(20, 18), new Point(21, 17), new Point(21, 16), new Point(23, 16), new Point(23, 17), new Point(26, 17), new Point(26, 11), new Point(27, 11), new Point(27, 6), new Point(28, 6), new Point(28, 1), new Point(29, 0) };
            Point[] myNave2 = { new Point(24, 0), new Point(29, 5), new Point(29, 18), new Point(32, 21), new Point(34, 21), new Point(38, 17), new Point(41, 20), new Point(41, 30), new Point(47, 36), new Point(47, 41), new Point(41, 41), new Point(38, 44), new Point(36, 44), new Point(33, 41), new Point(30, 41), new Point(25, 46), new Point(22, 46), new Point(17, 41), new Point(14, 41), new Point(11, 44), new Point(9, 44), new Point(6, 41), new Point(0, 41), new Point(0, 36), new Point(6, 30), new Point(6, 20), new Point(9, 17), new Point(13, 21), new Point(15, 21), new Point(18, 18), new Point(18, 5), new Point(23, 0) };
            Point[] myNave3 = { new Point(25, 5), new Point(26, 54), new Point(26, 5), new Point(26, 50), new Point(27, 50), new Point(28, 50), new Point(29, 50), new Point(30, 51), new Point(31, 51), new Point(32, 52), new Point(32, 49), new Point(31, 48), new Point(30, 47), new Point(29, 46), new Point(28, 45), new Point(27, 44), new Point(27, 36), new Point(28, 35), new Point(28, 25), new Point(29, 25), new Point(30, 25), new Point(31, 25), new Point(32, 26), new Point(33, 26), new Point(34, 27), new Point(35, 28), new Point(36, 8), new Point(37, 29), new Point(38, 30), new Point(39, 30), new Point(40, 31), new Point(41, 32), new Point(42, 32), new Point(43, 33), new Point(44, 34), new Point(45, 35), new Point(46, 36), new Point(47, 36), new Point(48, 36), new Point(49, 37), new Point(50, 37), new Point(51, 38), new Point(51, 37), new Point(51, 36), new Point(50, 35), new Point(50, 35), new Point(37, 15), new Point(37, 15), new Point(36, 14), new Point(35, 14), new Point(34, 15), new Point(34, 21), new Point(28, 15), new Point(28, 7), new Point(27, 6), new Point(26, 5), new Point(25, 5), new Point(24, 6), new Point(23, 7), new Point(23, 15), new Point(17, 21), new Point(17, 15), new Point(16, 14), new Point(15, 14), new Point(14, 15), new Point(14, 22), new Point(13, 25), new Point(12, 30), new Point(11, 31), new Point(10, 32), new Point(9, 32), new Point(8, 33), new Point(7, 34), new Point(6, 35), new Point(5, 36), new Point(4, 35), new Point(3, 36), new Point(2, 37), new Point(1, 37), new Point(0, 38), new Point(0, 39), new Point(0, 40), new Point(1, 40), new Point(2, 40), new Point(3, 40), new Point(4, 41), new Point(5, 42), new Point(6, 42), new Point(7, 42), new Point(8, 41), new Point(9, 40), new Point(10, 40), new Point(11, 40), new Point(12, 40), new Point(13, 40), new Point(14, 41), new Point(15, 42), new Point(16, 41), new Point(17, 40), new Point(18, 40), new Point(19, 40), new Point(20, 40), new Point(21, 40), new Point(22, 40), new Point(23, 40), new Point(24, 41), new Point(25, 42), new Point(26, 42), new Point(27, 42), new Point(28, 41), new Point(29, 40), new Point(30, 40), new Point(31, 40), new Point(32, 40), new Point(33, 39), new Point(33, 38), new Point(33, 37), new Point(33, 36), new Point(33, 35), new Point(33, 34), new Point(33, 33), new Point(32, 32), new Point(31, 31), new Point(30, 30), new Point(29, 29), new Point(28, 28), new Point(27, 27), new Point(26, 26), new Point(25, 25), new Point(24, 24), new Point(23, 23), new Point(22, 22), new Point(22, 21), new Point(22, 20), new Point(22, 19), new Point(22, 18), new Point(22, 17), new Point(22, 16), new Point(22, 15), new Point(21, 14), new Point(20, 13), new Point(20, 12), new Point(20, 11), new Point(21, 10), new Point(22, 9), new Point(23, 9), new Point(24, 9), new Point(25, 9), new Point(26, 9), new Point(25, 8), new Point(24, 7), new Point(23, 6), new Point(22, 5), new Point(21, 4), new Point(20, 3), new Point(19, 3), new Point(18, 2), new Point(17, 1), new Point(16, 0) };


            Point[] myNave;

            //***********INSERTAR EL OBJETO************//
            GraphicsPath ObjGrafico = new GraphicsPath();

            if (Tipox == 1)
            {
                largoN = 77;
                anchoN = 58;

                // rotar
                myNave = new Point[myNave1.Count()];

                for (int i = 0; i < myNave1.Count(); i++)
                {
                    myNave[i].X = myNave1[i].X;
                    if (AngRotar == 180)
                    {
                        myNave[i].Y = largoN - myNave1[i].Y;
                    }
                    else
                    {
                        myNave[i].Y = myNave1[i].Y;
                    }
                }
                ObjGrafico.AddPolygon(myNave);
            }
            else if (Tipox == 2)
            {
                largoN = 46; // Asumido * 1
                anchoN = 47; // Asumido * 1

                // rotar
                myNave = new Point[myNave2.Count()];

                for (int i = 0; i < myNave2.Count(); i++)
                {
                    myNave[i].X = myNave2[i].X * 1;
                    if (AngRotar == 180)
                    {
                        myNave[i].Y = largoN - myNave2[i].Y; // Corregida la multiplicación innecesaria * 1
                    }
                    else
                    {
                        myNave[i].Y = myNave2[i].Y * 1;
                    }
                }
                //ObjGrafico.AddPolygon(myNave);
                ObjGrafico.AddLines(myNave); // El original usaba AddLines para Tipox=2
            }
            else if (Tipox == 3)
            {
                largoN = 54;
                anchoN = 51; // rotar
                myNave = new Point[myNave3.Count()];

                for (int i = 0; i < myNave3.Count(); i++)
                {
                    myNave[i].X = myNave3[i].X;
                    if (AngRotar == 180)
                    {
                        myNave[i].Y = largoN - myNave3[i].Y;
                    }
                    else
                    {
                        myNave[i].Y = myNave3[i].Y;
                    }
                }
                ObjGrafico.AddPolygon(myNave);
            }

            Avion.BackColor = Pintar;
            Avion.Size = new Size(anchoN, largoN);
            Avion.Region = new Region(ObjGrafico);
            Avion.Location = new Point(0, 0);

            //*********INSERTAR LA NAVE AL CONTENDOR***********//
            contiene.Controls.Add(Avion);

            Bitmap Imagen = new Bitmap(Avion.Width, Avion.Height);
            Graphics PintaImg = Graphics.FromImage(Imagen);

            // Puntos para colorear (Originalmente para nave 2, pero usado en todos)
            Point[] Colorea = { new Point(24, 2), new Point(27, 5), new Point(27, 18), new Point(31, 22), new Point(34, 22), new Point(37, 19), new Point(38, 19), new Point(39, 20), new Point(39, 30), new Point(45, 36), new Point(45, 39), new Point(41, 39), new Point(38, 42), new Point(35, 42), new Point(32, 39), new Point(30, 39), new Point(25, 44), new Point(21, 44), new Point(16, 39), new Point(14, 39), new Point(11, 42), new Point(8, 42), new Point(5, 39), new Point(1, 39), new Point(1, 36), new Point(7, 30), new Point(7, 20), new Point(8, 19), new Point(9, 19), new Point(12, 22), new Point(15, 22), new Point(19, 18), new Point(19, 5), new Point(22, 2) };

            //PintaImg.FillPolygon(Brushes.DarkGreen, Colorea); // Comentado en el original

            // Corregida la definición de poix (asumiendo que se quería dibujar el borde de la nave)
            // Ya que myNave2 no siempre se usa, usaré ObjGrafico.PathData.Points
            PintaImg.DrawPolygon(Pens.Black, ObjGrafico.PathData.Points); 

            Avion.Image = Imagen;
            //NaveCorre (Avion, Tipox, AngRotar); // Comentado en el original
            Avion.Tag = Vida;
            Avion.Visible = true;
        }

        //*********** EFECTOS DE LA NAVE PRINCIPAL ***********//
        public void NaveCorre(PictureBox Avion, int AngRotar, int velox)
        {
            Bitmap Imagen = new Bitmap(Avion.Width, Avion.Height);
            Graphics PintaImg = Graphics.FromImage(Imagen);

            // Puntos para efectos de fuego (puntos muy corruptos en el original, se limpian solo sintácticamente)
            Point[] puntoDer = { new Point(35, 28), new Point(36, 30), new Point(37, 37), new Point(38, 38), new Point(39, 41), new Point(40, 45), new Point(40, 46), new Point(42, 48), new Point(43, 49), new Point(44, 65), new Point(42, 66), new Point(38, 68), new Point(36, 69), new Point(36, 68), new Point(35, 66), new Point(35, 64), new Point(35, 63), new Point(35, 62), new Point(35, 28) };
            Point[] puntolzq = { new Point(23, 28), new Point(22, 30), new Point(21, 31), new Point(20, 37), new Point(20, 40), new Point(19, 41), new Point(18, 45), new Point(18, 46), new Point(16, 48), new Point(15, 49), new Point(15, 65), new Point(16, 66), new Point(17, 68), new Point(18, 69), new Point(20, 68), new Point(22, 66), new Point(23, 63), new Point(23, 62), new Point(23, 28) };
            Point[] puntoAtr = { new Point(29, 2), new Point(31, 19), new Point(32, 19), new Point(33, 20), new Point(33, 25), new Point(32, 26), new Point(32, 63), new Point(34, 65), new Point(34, 68), new Point(33, 69), new Point(33, 73), new Point(31, 73), new Point(29, 73), new Point(27, 73), new Point(25, 74), new Point(24, 73), new Point(24, 65), new Point(26, 68), new Point(26, 63), new Point(26, 26), new Point(27, 25), new Point(27, 21), new Point(26, 20), new Point(26, 19), new Point(27, 19), new Point(28, 20) };


            PintaImg.FillPolygon(Brushes.DarkGreen, puntoDer);
            PintaImg.FillPolygon(Brushes.DarkGreen, puntolzq);
            PintaImg.FillPolygon(Brushes.DarkGreen, puntoAtr);
            
            // Corregida la sintaxis de las llamadas a FillRectangle (se asumen coordenadas)
            PintaImg.FillRectangle(Brushes.Silver, 25, 6, 5, 1); 
            PintaImg.FillRectangle(Brushes.Silver, 32, 6, 5, 1);
            PintaImg.FillRectangle(Brushes.Silver, 29, 3, 10, 3);

            if (velox == 1)
            {
                // Efectos de propulsión de fuego (corregida la sintaxis, se asumen coordenadas)
                PintaImg.FillRectangle(Brushes.DarkOrange, 35, 65, 5, 1);
                PintaImg.FillRectangle(Brushes.Orange, 36, 66, 4, 1);
                PintaImg.FillRectangle(Brushes.Yellow, 37, 67, 2, 1);

                PintaImg.FillRectangle(Brushes.DarkOrange, 23, 65, 5, 1); // Asumido
                PintaImg.FillRectangle(Brushes.Orange, 23, 66, 4, 1); // Asumido
                PintaImg.FillRectangle(Brushes.Yellow, 23, 67, 2, 1); // Asumido
            }
            else if (velox == 2)
            {
                // Efectos de propulsión más fuerte (corregida la sintaxis, se asumen coordenadas)
                PintaImg.FillRectangle(Brushes.DarkRed, 17, 30, 10, 8);
                PintaImg.FillRectangle(Brushes.DarkRed, 35, 30, 10, 16);
                PintaImg.FillRectangle(Brushes.DarkRed, 45, 30, 5, 8);

            }
            else if (velox == 3)
            {
                // Efectos de propulsión más fuerte (corregida la sintaxis, se asumen coordenadas)
                PintaImg.FillRectangle(Brushes.DarkRed, 15, 30, 5, 8);
                PintaImg.FillRectangle(Brushes.DarkRed, 5, 30, 5, 16);
                PintaImg.FillRectangle(Brushes.DarkRed, 1, 30, 5, 8);
            }

            Avion.Image = RotateImage(Imagen, AngRotar);
        }

        //******************CREAR ANGULO DE ROTACION******************//
        public static Image RotateImage(Image img, float rotationAngle)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height);
            Graphics gfx = Graphics.FromImage(bmp);

            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);
            gfx.RotateTransform(rotationAngle);
            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gfx.DrawImage(img, new Point(0, 0));
            gfx.Dispose();

            return bmp;
        }

        //*** MOVIMIENTO DEL TECLADO DEL USUARIO ***********//
        public void ActividadTecla(object sender, KeyEventArgs e)
        {
            //INSTRUCCIONES DE LOS BOTONES
            switch (e.KeyValue)
            {
                case 37: // flecha hacia la izquierda
                    {
                        if (navex.Left > contiene.Left) // Corregido el operador
                        {
                            navex.Left -= 10; // Corregida la sintaxis
                            angulo -= 15; // Corregida la sintaxis
                            NaveCorre(navex, 0, 1); // Parámetros asumidos
                        }
                        break;
                    }

                case 38: // flecha hacia arriba
                    {
                        if (navex.Top > contiene.Top)
                        {
                            navex.Top -= 10; // Corregida la sintaxis
                            NaveCorre(navex, 0, 1);
                            break;
                        }
                        break;
                    }

                case 39: // flecha hacia la derecha
                    {
                        if (navex.Right < contiene.Right) // Corregido el operador
                        {
                            navex.Left += 10; // Corregida la sintaxis
                            angulo += 15; // Corregida la sintaxis
                            NaveCorre(navex, 0, 1); // Parámetros asumidos
                        }
                        break;
                    }

                case 40: // flecha hacia abajo
                    {
                        if (navex.Bottom < contiene.Bottom) // Corregido el operador
                        {
                            navex.Top += 10; // Corregida la sintaxis
                            NaveCorre(navex, 0, 1); // Parámetros asumidos
                        }
                        break;
                    }

                case 32: // Barra espaciadora para disparar (asumido 32)
                    {
                        tiempo.Start();
                        int x = navex.Location.X + (navex.Width / 2);
                        int y = navex.Location.Y + (navex.Height / 2);
                        CrearMisil(0, Color.DarkMagenta, "Misil", x, y); // Corregido el nombre y la x
                        break;
                    }
            }
        }

        //******* ACTIVAR ACCIONES DE INICIALIZACION **********************//
        public void Iniciar()
        {
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.Width = 345;
            this.Height = 450;
            this.Text = "JUEGO DE AVIONES"; // Corregido el texto

            label1.Text = "Mi Rival";
            label2.Text = "Mi Avion";
            
            // Asigna el manejador de eventos (Corregida la sintaxis +=)
            this.KeyDown += new KeyEventHandler(ActividadTecla); 

            contiene.Location = new Point(0, 0);
            contiene.BackColor = Color.AliceBlue;
            contiene.Size = new Size(300, 420);
            contiene.Dock = DockStyle.Fill;
            Controls.Add(contiene);
            contiene.Visible = true;

            // contenido del formulario
            Random r = new Random();
            int aleaty = r.Next(250, 330);
            int aleatx = r.Next(50, 250);

            CrearNave(navex, 0, 1, Color.SeaGreen, 20);

            //ELEGIR NAVE DE SALIDA RIVAL
            Random sal = new Random();
            int sale = sal.Next(1, 3);
            CrearNave(naveRival, 180, sale, Color.DarkBlue, 50);

            // Modulo.Escenario(contiene, sale); // Dependencia externa removida/comentada

            navex.Location = new Point(aleatx, aleaty); // Corregida la x
            naveRival.Location = new Point(aleatx, 50); // Posición inicial asumida para el rival

            tiempo = new System.Windows.Forms.Timer(); // Inicialización
            tiempo.Interval = 1;
            tiempo.Enabled = true;
            tiempo.Tick += new EventHandler(ImpactarTick);
        }

        //*********** ARGUMENTOS GENERADOS POR EL PROGRAMA **********//
        
        // Se asume la existencia de este método para que la clase compile
        private void InitializeComponent() 
        {
            // Ahora InitializeComponent no necesita inicializar 'components' ya que Form1.Designer.cs lo hace.

            // Lógica de inicialización de componentes de Form1.Designer.cs
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Text = "Mi Rival";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 30);
            this.label2.Text = "Mi Avion";
            //
            // Form1
            //
            this.ClientSize = new System.Drawing.Size(345, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Iniciar();
        }
    }
}