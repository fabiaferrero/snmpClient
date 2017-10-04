namespace SnmpClient
{
    partial class Form1
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
            this.StampeCompleteBotton = new System.Windows.Forms.Button();
            this.MacBotton = new System.Windows.Forms.Button();
            this.SNMPBotton = new System.Windows.Forms.Button();
            this.StampeLabel = new System.Windows.Forms.Label();
            this.IpLabel = new System.Windows.Forms.Label();
            this.VersioneLabel = new System.Windows.Forms.Label();
            this.discoverBotton = new System.Windows.Forms.Button();
            this.discoverLabel = new System.Windows.Forms.Label();
            this.magentaBotton = new System.Windows.Forms.Button();
            this.progressBarMagenta = new System.Windows.Forms.ProgressBar();
            this.progressBarCiano = new System.Windows.Forms.ProgressBar();
            this.progressBarYellow = new System.Windows.Forms.ProgressBar();
            this.progressBarBlack = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // StampeCompleteBotton
            // 
            this.StampeCompleteBotton.AutoSize = true;
            this.StampeCompleteBotton.BackColor = System.Drawing.Color.LightSteelBlue;
            this.StampeCompleteBotton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.StampeCompleteBotton.FlatAppearance.BorderSize = 0;
            this.StampeCompleteBotton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.StampeCompleteBotton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StampeCompleteBotton.Location = new System.Drawing.Point(12, 50);
            this.StampeCompleteBotton.Name = "StampeCompleteBotton";
            this.StampeCompleteBotton.Size = new System.Drawing.Size(150, 25);
            this.StampeCompleteBotton.TabIndex = 0;
            this.StampeCompleteBotton.Text = "Numero Stampe completate";
            this.StampeCompleteBotton.UseVisualStyleBackColor = false;
            this.StampeCompleteBotton.Click += new System.EventHandler(this.StampeCompleteBotton_Click);
            // 
            // MacBotton
            // 
            this.MacBotton.BackColor = System.Drawing.Color.LightSteelBlue;
            this.MacBotton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MacBotton.FlatAppearance.BorderSize = 0;
            this.MacBotton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.MacBotton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MacBotton.Location = new System.Drawing.Point(13, 81);
            this.MacBotton.Name = "MacBotton";
            this.MacBotton.Size = new System.Drawing.Size(148, 23);
            this.MacBotton.TabIndex = 1;
            this.MacBotton.Text = "MAC Stampante";
            this.MacBotton.UseVisualStyleBackColor = false;
            this.MacBotton.Click += new System.EventHandler(this.MACBotton_Click);
            // 
            // SNMPBotton
            // 
            this.SNMPBotton.BackColor = System.Drawing.Color.LightSteelBlue;
            this.SNMPBotton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.SNMPBotton.FlatAppearance.BorderSize = 0;
            this.SNMPBotton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.SNMPBotton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SNMPBotton.Location = new System.Drawing.Point(12, 111);
            this.SNMPBotton.Name = "SNMPBotton";
            this.SNMPBotton.Size = new System.Drawing.Size(150, 23);
            this.SNMPBotton.TabIndex = 2;
            this.SNMPBotton.Text = "Versione SNMP";
            this.SNMPBotton.UseVisualStyleBackColor = false;
            this.SNMPBotton.Click += new System.EventHandler(this.SNMPBotton_Click);
            // 
            // StampeLabel
            // 
            this.StampeLabel.AutoSize = true;
            this.StampeLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.StampeLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.StampeLabel.Location = new System.Drawing.Point(181, 58);
            this.StampeLabel.Name = "StampeLabel";
            this.StampeLabel.Size = new System.Drawing.Size(2, 15);
            this.StampeLabel.TabIndex = 3;
            // 
            // IpLabel
            // 
            this.IpLabel.AutoSize = true;
            this.IpLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.IpLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.IpLabel.Location = new System.Drawing.Point(181, 86);
            this.IpLabel.Name = "IpLabel";
            this.IpLabel.Size = new System.Drawing.Size(2, 15);
            this.IpLabel.TabIndex = 4;
            // 
            // VersioneLabel
            // 
            this.VersioneLabel.AutoSize = true;
            this.VersioneLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.VersioneLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.VersioneLabel.Location = new System.Drawing.Point(181, 116);
            this.VersioneLabel.Name = "VersioneLabel";
            this.VersioneLabel.Size = new System.Drawing.Size(2, 15);
            this.VersioneLabel.TabIndex = 5;
            // 
            // discoverBotton
            // 
            this.discoverBotton.BackColor = System.Drawing.Color.LightSteelBlue;
            this.discoverBotton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.discoverBotton.FlatAppearance.BorderSize = 0;
            this.discoverBotton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.discoverBotton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.discoverBotton.Location = new System.Drawing.Point(12, 21);
            this.discoverBotton.Name = "discoverBotton";
            this.discoverBotton.Size = new System.Drawing.Size(150, 23);
            this.discoverBotton.TabIndex = 6;
            this.discoverBotton.Text = "Discover Stampanti";
            this.discoverBotton.UseVisualStyleBackColor = false;
            this.discoverBotton.Click += new System.EventHandler(this.discoverBotton_Click);
            // 
            // discoverLabel
            // 
            this.discoverLabel.AutoSize = true;
            this.discoverLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.discoverLabel.Location = new System.Drawing.Point(181, 26);
            this.discoverLabel.Name = "discoverLabel";
            this.discoverLabel.Size = new System.Drawing.Size(2, 15);
            this.discoverLabel.TabIndex = 7;
            // 
            // magentaBotton
            // 
            this.magentaBotton.BackColor = System.Drawing.Color.LightSteelBlue;
            this.magentaBotton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.magentaBotton.FlatAppearance.BorderSize = 0;
            this.magentaBotton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.magentaBotton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.magentaBotton.Location = new System.Drawing.Point(12, 140);
            this.magentaBotton.Name = "magentaBotton";
            this.magentaBotton.Size = new System.Drawing.Size(150, 110);
            this.magentaBotton.TabIndex = 8;
            this.magentaBotton.Text = "Controllo colori";
            this.magentaBotton.UseVisualStyleBackColor = false;
            this.magentaBotton.Click += new System.EventHandler(this.magentaBotton_Click);
            // 
            // progressBarMagenta
            // 
            this.progressBarMagenta.ForeColor = System.Drawing.Color.Fuchsia;
            this.progressBarMagenta.Location = new System.Drawing.Point(181, 140);
            this.progressBarMagenta.Name = "progressBarMagenta";
            this.progressBarMagenta.Size = new System.Drawing.Size(100, 23);
            this.progressBarMagenta.TabIndex = 11;
            // 
            // progressBarCiano
            // 
            this.progressBarCiano.ForeColor = System.Drawing.Color.Cyan;
            this.progressBarCiano.Location = new System.Drawing.Point(181, 169);
            this.progressBarCiano.Name = "progressBarCiano";
            this.progressBarCiano.Size = new System.Drawing.Size(100, 23);
            this.progressBarCiano.TabIndex = 12;
            // 
            // progressBarYellow
            // 
            this.progressBarYellow.ForeColor = System.Drawing.Color.Yellow;
            this.progressBarYellow.Location = new System.Drawing.Point(181, 198);
            this.progressBarYellow.Name = "progressBarYellow";
            this.progressBarYellow.Size = new System.Drawing.Size(100, 23);
            this.progressBarYellow.TabIndex = 13;
            // 
            // progressBarBlack
            // 
            this.progressBarBlack.ForeColor = System.Drawing.Color.Black;
            this.progressBarBlack.Location = new System.Drawing.Point(181, 227);
            this.progressBarBlack.Name = "progressBarBlack";
            this.progressBarBlack.Size = new System.Drawing.Size(100, 23);
            this.progressBarBlack.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 298);
            this.Controls.Add(this.progressBarBlack);
            this.Controls.Add(this.progressBarYellow);
            this.Controls.Add(this.progressBarCiano);
            this.Controls.Add(this.progressBarMagenta);
            this.Controls.Add(this.magentaBotton);
            this.Controls.Add(this.discoverLabel);
            this.Controls.Add(this.discoverBotton);
            this.Controls.Add(this.VersioneLabel);
            this.Controls.Add(this.IpLabel);
            this.Controls.Add(this.StampeLabel);
            this.Controls.Add(this.SNMPBotton);
            this.Controls.Add(this.MacBotton);
            this.Controls.Add(this.StampeCompleteBotton);
            this.Name = "Form1";
            this.Text = "SNMP Controllo Stampante";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StampeCompleteBotton;
        private System.Windows.Forms.Button MacBotton;
        private System.Windows.Forms.Button SNMPBotton;
        private System.Windows.Forms.Label StampeLabel;
        private System.Windows.Forms.Label IpLabel;
        private System.Windows.Forms.Label VersioneLabel;
        private System.Windows.Forms.Button discoverBotton;
        private System.Windows.Forms.Label discoverLabel;
        private System.Windows.Forms.Button magentaBotton;
        private System.Windows.Forms.ProgressBar progressBarMagenta;
        private System.Windows.Forms.ProgressBar progressBarCiano;
        private System.Windows.Forms.ProgressBar progressBarYellow;
        private System.Windows.Forms.ProgressBar progressBarBlack;
    }
}

