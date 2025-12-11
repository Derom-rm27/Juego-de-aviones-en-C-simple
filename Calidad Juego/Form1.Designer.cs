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

        private System.Windows.Forms.Panel panelEncabezado;
        private Label labelTitulo = null!;
        private Label labelEstado = null!;
        private Label labelIndicaciones = null!;
        private Label label1 = null!;
        private Label label2 = null!;
        private System.Windows.Forms.Button botonReintentar;

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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelEncabezado = new System.Windows.Forms.Panel();
            labelEstado = new System.Windows.Forms.Label();
            labelTitulo = new System.Windows.Forms.Label();
            labelIndicaciones = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            botonReintentar = new System.Windows.Forms.Button();
            panelEncabezado.SuspendLayout();
            SuspendLayout();
            // 
            // panelEncabezado
            // 
            panelEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)((byte)23)), ((int)((byte)36)), ((int)((byte)66)));
            panelEncabezado.Controls.Add(labelEstado);
            panelEncabezado.Controls.Add(labelTitulo);
            panelEncabezado.Dock = System.Windows.Forms.DockStyle.Top;
            panelEncabezado.Location = new System.Drawing.Point(0, 0);
            panelEncabezado.Name = "panelEncabezado";
            panelEncabezado.Size = new System.Drawing.Size(711, 70);
            panelEncabezado.TabIndex = 0;
            // 
            // labelEstado
            // 
            labelEstado.AutoSize = true;
            labelEstado.Font = new System.Drawing.Font("Segoe UI", 9F);
            labelEstado.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)201)), ((int)((byte)236)), ((int)((byte)255)));
            labelEstado.Location = new System.Drawing.Point(12, 40);
            labelEstado.Name = "labelEstado";
            labelEstado.Size = new System.Drawing.Size(325, 20);
            labelEstado.TabIndex = 1;
            labelEstado.Text = "Mantén presionadas las flechas para maniobrar.";
            // 
            // labelTitulo
            // 
            labelTitulo.AutoSize = true;
            labelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            labelTitulo.ForeColor = System.Drawing.Color.White;
            labelTitulo.Location = new System.Drawing.Point(12, 10);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new System.Drawing.Size(215, 32);
            labelTitulo.TabIndex = 0;
            labelTitulo.Text = "Juego de Aviones";
            // 
            // labelIndicaciones
            // 
            labelIndicaciones.AutoSize = true;
            labelIndicaciones.Font = new System.Drawing.Font("Segoe UI", 9F);
            labelIndicaciones.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)215)), ((int)((byte)228)), ((int)((byte)250)));
            labelIndicaciones.Location = new System.Drawing.Point(12, 80);
            labelIndicaciones.Name = "labelIndicaciones";
            labelIndicaciones.Size = new System.Drawing.Size(609, 20);
            labelIndicaciones.TabIndex = 1;
            labelIndicaciones.Text = ("Espacio para disparar, Enter para reiniciar: supera niveles, meteoritos y escuadr" + "ones rivales.");
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            label1.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)216)), ((int)((byte)235)), ((int)((byte)255)));
            label1.Location = new System.Drawing.Point(12, 110);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(218, 23);
            label1.TabIndex = 2;
            label1.Text = "Nivel 1 - Rivales activos: 0";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            label2.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)216)), ((int)((byte)235)), ((int)((byte)255)));
            label2.Location = new System.Drawing.Point(12, 136);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(157, 23);
            label2.TabIndex = 3;
            label2.Text = "Vida del Avión: 20";
            // 
            // botonReintentar
            // 
            botonReintentar.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            botonReintentar.BackColor = System.Drawing.Color.FromArgb(((int)((byte)37)), ((int)((byte)54)), ((int)((byte)92)));
            botonReintentar.FlatAppearance.BorderSize = 0;
            botonReintentar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            botonReintentar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            botonReintentar.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)228)), ((int)((byte)239)), ((int)((byte)255)));
            botonReintentar.Location = new System.Drawing.Point(555, 516);
            botonReintentar.Name = "botonReintentar";
            botonReintentar.Size = new System.Drawing.Size(144, 32);
            botonReintentar.TabIndex = 4;
            botonReintentar.Text = "Jugar de nuevo";
            botonReintentar.UseVisualStyleBackColor = false;
            botonReintentar.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(((int)((byte)13)), ((int)((byte)19)), ((int)((byte)33)));
            ClientSize = new System.Drawing.Size(711, 560);
            Controls.Add(botonReintentar);
            Controls.Add(labelIndicaciones);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(panelEncabezado);
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Juego de Aviones";
            panelEncabezado.ResumeLayout(false);
            panelEncabezado.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
