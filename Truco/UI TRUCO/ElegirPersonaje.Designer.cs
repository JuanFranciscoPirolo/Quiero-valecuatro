namespace UI_TRUCO
{
    partial class ElegirPersonaje
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ElegirPersonaje));
            pictureDiego = new PictureBox();
            pictureMessi = new PictureBox();
            pictureMomo = new PictureBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            checkBox1 = new CheckBox();
            checkBox2 = new CheckBox();
            checkBox3 = new CheckBox();
            button5 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureDiego).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureMessi).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureMomo).BeginInit();
            SuspendLayout();
            // 
            // pictureDiego
            // 
            pictureDiego.Image = Properties.Resources.descarga;
            pictureDiego.Location = new Point(45, 157);
            pictureDiego.Name = "pictureDiego";
            pictureDiego.Size = new Size(156, 162);
            pictureDiego.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureDiego.TabIndex = 0;
            pictureDiego.TabStop = false;
            pictureDiego.Click += pictureDiego_Click;
            // 
            // pictureMessi
            // 
            pictureMessi.Image = (Image)resources.GetObject("pictureMessi.Image");
            pictureMessi.Location = new Point(321, 157);
            pictureMessi.Name = "pictureMessi";
            pictureMessi.Size = new Size(156, 162);
            pictureMessi.SizeMode = PictureBoxSizeMode.Zoom;
            pictureMessi.TabIndex = 1;
            pictureMessi.TabStop = false;
            pictureMessi.Click += pictureMessi_Click;
            // 
            // pictureMomo
            // 
            pictureMomo.Image = (Image)resources.GetObject("pictureMomo.Image");
            pictureMomo.Location = new Point(597, 157);
            pictureMomo.Name = "pictureMomo";
            pictureMomo.Size = new Size(156, 162);
            pictureMomo.SizeMode = PictureBoxSizeMode.Zoom;
            pictureMomo.TabIndex = 2;
            pictureMomo.TabStop = false;
            pictureMomo.Click += pictureMomo_Click;
            // 
            // button1
            // 
            button1.Location = new Point(45, 379);
            button1.Name = "button1";
            button1.Size = new Size(186, 40);
            button1.TabIndex = 4;
            button1.Text = "Reproducir sonido";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(308, 372);
            button2.Name = "button2";
            button2.Size = new Size(186, 36);
            button2.TabIndex = 5;
            button2.Text = "Reproducir sonido";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(585, 381);
            button3.Name = "button3";
            button3.Size = new Size(186, 38);
            button3.TabIndex = 6;
            button3.Text = "Reproducir sonido";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.BackColor = SystemColors.ActiveCaptionText;
            checkBox1.ForeColor = SystemColors.ButtonHighlight;
            checkBox1.Location = new Point(57, 339);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(144, 24);
            checkBox1.TabIndex = 8;
            checkBox1.Text = "Diego Maradona";
            checkBox1.UseVisualStyleBackColor = false;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.BackColor = SystemColors.ActiveCaptionText;
            checkBox2.ForeColor = SystemColors.ButtonHighlight;
            checkBox2.Location = new Point(321, 339);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(112, 24);
            checkBox2.TabIndex = 9;
            checkBox2.Text = "Lionel Messi";
            checkBox2.UseVisualStyleBackColor = false;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.BackColor = SystemColors.ActiveCaptionText;
            checkBox3.ForeColor = SystemColors.ButtonHighlight;
            checkBox3.Location = new Point(597, 339);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(75, 24);
            checkBox3.TabIndex = 10;
            checkBox3.Text = "Momo";
            checkBox3.UseVisualStyleBackColor = false;
            checkBox3.CheckedChanged += checkBox3_CheckedChanged;
            // 
            // button5
            // 
            button5.BackColor = SystemColors.ActiveCaptionText;
            button5.ForeColor = SystemColors.ButtonHighlight;
            button5.Location = new Point(606, 560);
            button5.Name = "button5";
            button5.Size = new Size(164, 61);
            button5.TabIndex = 12;
            button5.Text = "Elegir personaje";
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // ElegirPersonaje
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(800, 633);
            Controls.Add(button5);
            Controls.Add(checkBox3);
            Controls.Add(checkBox2);
            Controls.Add(checkBox1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(pictureMomo);
            Controls.Add(pictureMessi);
            Controls.Add(pictureDiego);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ElegirPersonaje";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Elegí tu personaje";
            Load += ElegirPersonaje_Load;
            ((System.ComponentModel.ISupportInitialize)pictureDiego).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureMessi).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureMomo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureDiego;
        private PictureBox pictureMessi;
        private PictureBox pictureMomo;
        private Button button1;
        private Button button2;
        private Button button3;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private Button button5;
    }
}