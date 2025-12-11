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

        private Label label1 = null!;
        private Label label2 = null!;

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
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(141, 19);
            label1.TabIndex = 0;
            label1.Text = "Vida del Rival: 50";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(12, 35);
            label2.Name = "label2";
            label2.Size = new Size(154, 19);
            label2.TabIndex = 1;
            label2.Text = "Vida del Avión: ❤❤❤";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(340, 520);
            Controls.Add(label2);
            Controls.Add(label1);
            KeyPreview = true;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Juego de Aviones";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
