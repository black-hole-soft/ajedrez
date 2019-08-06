namespace Ajedrez
{
    partial class frmAjedrez
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAjedrez));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnReiniciar = new System.Windows.Forms.Button();
            this.btnRendirse = new System.Windows.Forms.Button();
            this.tiempo = new System.Windows.Forms.Timer(this.components);
            this.btnTiempo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(553, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 150);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(553, 151);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(320, 427);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // btnReiniciar
            // 
            this.btnReiniciar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReiniciar.BackgroundImage")));
            this.btnReiniciar.Font = new System.Drawing.Font("GothicE", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReiniciar.Location = new System.Drawing.Point(573, 434);
            this.btnReiniciar.Name = "btnReiniciar";
            this.btnReiniciar.Size = new System.Drawing.Size(200, 50);
            this.btnReiniciar.TabIndex = 2;
            this.btnReiniciar.Text = "Reiniciar";
            this.btnReiniciar.UseVisualStyleBackColor = true;
            this.btnReiniciar.Click += new System.EventHandler(this.btnReiniciar_Click);
            // 
            // btnRendirse
            // 
            this.btnRendirse.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRendirse.BackgroundImage")));
            this.btnRendirse.Font = new System.Drawing.Font("GothicE", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRendirse.Location = new System.Drawing.Point(573, 490);
            this.btnRendirse.Name = "btnRendirse";
            this.btnRendirse.Size = new System.Drawing.Size(200, 50);
            this.btnRendirse.TabIndex = 3;
            this.btnRendirse.Text = "Rendirse";
            this.btnRendirse.UseVisualStyleBackColor = true;
            this.btnRendirse.Click += new System.EventHandler(this.btnRendirse_Click);
            // 
            // tiempo
            // 
            this.tiempo.Enabled = true;
            this.tiempo.Interval = 1000;
            this.tiempo.Tick += new System.EventHandler(this.tiempo_Tick);
            // 
            // btnTiempo
            // 
            this.btnTiempo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnTiempo.BackgroundImage")));
            this.btnTiempo.Enabled = false;
            this.btnTiempo.Font = new System.Drawing.Font("Copperplate Gothic Light", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTiempo.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btnTiempo.Location = new System.Drawing.Point(573, 378);
            this.btnTiempo.Name = "btnTiempo";
            this.btnTiempo.Size = new System.Drawing.Size(200, 50);
            this.btnTiempo.TabIndex = 4;
            this.btnTiempo.Text = "00:00:00";
            this.btnTiempo.UseVisualStyleBackColor = true;
            // 
            // frmAjedrez
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DodgerBlue;
            this.ClientSize = new System.Drawing.Size(874, 552);
            this.Controls.Add(this.btnTiempo);
            this.Controls.Add(this.btnRendirse);
            this.Controls.Add(this.btnReiniciar);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "frmAjedrez";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ajedrez";
            this.Load += new System.EventHandler(this.frmAjedrez_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnReiniciar;
        private System.Windows.Forms.Button btnRendirse;
        private System.Windows.Forms.Timer tiempo;
        private System.Windows.Forms.Button btnTiempo;

    }
}

