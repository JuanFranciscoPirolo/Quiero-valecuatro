namespace UI_TRUCO
{
    partial class FormularioPrincipal
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormularioPrincipal));
            btnPartida = new Button();
            btnHistorial = new Button();
            SuspendLayout();
            // 
            // btnPartida
            // 
            btnPartida.AutoSize = true;
            btnPartida.BackColor = Color.PowderBlue;
            btnPartida.BackgroundImage = Properties.Resources.pngtree_an_old_bar_with_dark_wood_floors_and_old_carved_image_2527062;
            btnPartida.Font = new Font("Elephant", 10.1999989F, FontStyle.Regular, GraphicsUnit.Point);
            btnPartida.ForeColor = SystemColors.ButtonHighlight;
            btnPartida.Location = new Point(571, 392);
            btnPartida.Name = "btnPartida";
            btnPartida.Size = new Size(236, 115);
            btnPartida.TabIndex = 0;
            btnPartida.Text = "Iniciar la partida";
            btnPartida.UseVisualStyleBackColor = false;
            btnPartida.Click += btnPartida_Click;
            // 
            // btnHistorial
            // 
            btnHistorial.AutoSize = true;
            btnHistorial.BackColor = Color.PowderBlue;
            btnHistorial.BackgroundImage = Properties.Resources.pngtree_an_old_bar_with_dark_wood_floors_and_old_carved_image_2527062;
            btnHistorial.Font = new Font("Elephant", 10.1999989F, FontStyle.Regular, GraphicsUnit.Point);
            btnHistorial.ForeColor = SystemColors.ButtonHighlight;
            btnHistorial.Location = new Point(571, 557);
            btnHistorial.Name = "btnHistorial";
            btnHistorial.Size = new Size(236, 115);
            btnHistorial.TabIndex = 1;
            btnHistorial.Text = "Historial";
            btnHistorial.UseVisualStyleBackColor = false;
            // 
            // FormularioPrincipal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1346, 856);
            Controls.Add(btnHistorial);
            Controls.Add(btnPartida);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormularioPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "¡Quiero vale cuatro!";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnPartida;
        private Button btnHistorial;
    }
}