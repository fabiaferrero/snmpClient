namespace SnmpClient
{
    partial class SNMPControllo
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.CheckStampante = new System.Windows.Forms.Button();
            this.discoverBotton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CheckStampante
            // 
            this.CheckStampante.AutoSize = true;
            this.CheckStampante.BackColor = System.Drawing.Color.LightSteelBlue;
            this.CheckStampante.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CheckStampante.FlatAppearance.BorderSize = 0;
            this.CheckStampante.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.CheckStampante.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheckStampante.Location = new System.Drawing.Point(214, 25);
            this.CheckStampante.Margin = new System.Windows.Forms.Padding(4);
            this.CheckStampante.Name = "CheckStampante";
            this.CheckStampante.Size = new System.Drawing.Size(147, 31);
            this.CheckStampante.TabIndex = 0;
            this.CheckStampante.Text = "Check Stampante";
            this.CheckStampante.UseVisualStyleBackColor = false;
            this.CheckStampante.Click += new System.EventHandler(this.CheckStampanteBotton_Click);
            // 
            // discoverBotton
            // 
            this.discoverBotton.BackColor = System.Drawing.Color.LightSteelBlue;
            this.discoverBotton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.discoverBotton.FlatAppearance.BorderSize = 0;
            this.discoverBotton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.discoverBotton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.discoverBotton.Location = new System.Drawing.Point(16, 26);
            this.discoverBotton.Margin = new System.Windows.Forms.Padding(4);
            this.discoverBotton.Name = "discoverBotton";
            this.discoverBotton.Size = new System.Drawing.Size(147, 28);
            this.discoverBotton.TabIndex = 6;
            this.discoverBotton.Text = "Discover Stampanti";
            this.discoverBotton.UseVisualStyleBackColor = false;
            this.discoverBotton.Click += new System.EventHandler(this.discoverBotton_ClickAsync);
            // 
            // SNMPControllo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 94);
            this.Controls.Add(this.discoverBotton);
            this.Controls.Add(this.CheckStampante);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SNMPControllo";
            this.Text = "SNMP Controllo Stampante";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CheckStampante;
        private System.Windows.Forms.Button discoverBotton;
    }
}

