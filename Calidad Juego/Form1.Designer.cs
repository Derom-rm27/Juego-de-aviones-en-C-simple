// Autor: Delfin Estanis Roque Mullisaca
// Fecha: 07/11/2025
// Versión: 1.0
// Universidad: UNIVERSIDAD NACIONAL DE JULIACA
// Descripción: Componentes del formulario principal del juego de aviones.

using System.Drawing;
using System.Windows.Forms;

namespace Calidad_Juego
{
    partial class Form1
    {
        /// <summary>
        ///  Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer? components = null;

        private Panel panelEncabezado = null!;
        private Label labelTitulo = null!;
        private Label labelEstado = null!;
        private Label labelIndicaciones = null!;
        private Label label1 = null!;
        private Label label2 = null!;
        private Button botonReintentar = null!;

        /// <summary>
        ///  Liberar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; de lo contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        ///  Método necesario para admitir el Diseñador. No se puede modificar
        ///  el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panelEncabezado = new Panel();
            labelTitulo = new Label();
            labelEstado = new Label();
            labelIndicaciones = new Label();
            label1 = new Label();
            label2 = new Label();
            botonReintentar = new Button();
            SuspendLayout();
            // 
            // panelEncabezado
            //
            panelEncabezado.BackColor = Color.FromArgb(23, 36, 66);
            panelEncabezado.Controls.Add(labelEstado);
            panelEncabezado.Controls.Add(labelTitulo);
            panelEncabezado.Dock = DockStyle.Top;
            panelEncabezado.Location = new Point(0, 0);
            panelEncabezado.Name = "panelEncabezado";
            panelEncabezado.Size = new Size(360, 70);
            panelEncabezado.TabIndex = 0;
            //
            // labelTitulo
            //
            labelTitulo.AutoSize = true;
            labelTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            labelTitulo.ForeColor = Color.White;
            labelTitulo.Location = new Point(12, 10);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new Size(162, 25);
            labelTitulo.TabIndex = 0;
            labelTitulo.Text = "Juego de Aviones";
            //
            // labelEstado
            //
            labelEstado.AutoSize = true;
            labelEstado.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            labelEstado.ForeColor = Color.FromArgb(201, 236, 255);
            labelEstado.Location = new Point(12, 40);
            labelEstado.Name = "labelEstado";
            labelEstado.Size = new Size(173, 15);
            labelEstado.TabIndex = 1;
            labelEstado.Text = "Mantén presionadas las flechas para maniobrar.";
            //
            // labelIndicaciones
            //
            labelIndicaciones.AutoSize = true;
            labelIndicaciones.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            labelIndicaciones.ForeColor = Color.FromArgb(215, 228, 250);
            labelIndicaciones.Location = new Point(12, 80);
            labelIndicaciones.Name = "labelIndicaciones";
            labelIndicaciones.Size = new Size(255, 15);
            labelIndicaciones.TabIndex = 1;
            labelIndicaciones.Text = "Espacio para disparar, Enter para reiniciar: supera niveles, meteoritos y escuadrones rivales.";
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.FromArgb(216, 235, 255);
            label1.Location = new Point(12, 110);
            label1.Name = "label1";
            label1.Size = new Size(141, 19);
            label1.TabIndex = 2;
            label1.Text = "Nivel 1 - Rivales activos: 0";
            //
            // label2
            //
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            label2.ForeColor = Color.FromArgb(216, 235, 255);
            label2.Location = new Point(12, 136);
            label2.Name = "label2";
            label2.Size = new Size(149, 19);
            label2.TabIndex = 3;
            label2.Text = "Vida del Avión: 20";
            //
            // botonReintentar
            //
            botonReintentar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            botonReintentar.BackColor = Color.FromArgb(37, 54, 92);
            botonReintentar.FlatAppearance.BorderSize = 0;
            botonReintentar.FlatStyle = FlatStyle.Flat;
            botonReintentar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            botonReintentar.ForeColor = Color.FromArgb(228, 239, 255);
            botonReintentar.Location = new Point(204, 516);
            botonReintentar.Name = "botonReintentar";
            botonReintentar.Size = new Size(144, 32);
            botonReintentar.TabIndex = 4;
            botonReintentar.Text = "Jugar de nuevo";
            botonReintentar.UseVisualStyleBackColor = false;
            botonReintentar.Visible = false;
            //
            // Form1
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(13, 19, 33);
            ClientSize = new Size(360, 560);
            Controls.Add(botonReintentar);
            Controls.Add(labelIndicaciones);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(panelEncabezado);
            KeyPreview = true;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Juego de Aviones";
            panelEncabezado.ResumeLayout(false);
            panelEncabezado.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
