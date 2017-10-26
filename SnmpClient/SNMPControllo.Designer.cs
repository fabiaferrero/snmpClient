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
            this.radioButtonComplete = new System.Windows.Forms.RadioButton();
            this.radioBottonUptime = new System.Windows.Forms.RadioButton();
            this.radioButtonMAC = new System.Windows.Forms.RadioButton();
            this.radioButtonYellow = new System.Windows.Forms.RadioButton();
            this.radioButtonCiano = new System.Windows.Forms.RadioButton();
            this.radioButtonMagenta = new System.Windows.Forms.RadioButton();
            this.radioButtonBlack = new System.Windows.Forms.RadioButton();
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
            this.CheckStampante.Location = new System.Drawing.Point(16, 62);
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
            // radioButtonComplete
            // 
            this.radioButtonComplete.AutoSize = true;
            this.radioButtonComplete.Location = new System.Drawing.Point(215, 26);
            this.radioButtonComplete.Name = "radioButtonComplete";
            this.radioButtonComplete.Size = new System.Drawing.Size(152, 21);
            this.radioButtonComplete.TabIndex = 7;
            this.radioButtonComplete.TabStop = true;
            this.radioButtonComplete.Text = "Stampe Completate";
            this.radioButtonComplete.UseVisualStyleBackColor = true;
            // 
            // radioBottonUptime
            // 
            this.radioBottonUptime.AutoSize = true;
            this.radioBottonUptime.Location = new System.Drawing.Point(215, 54);
            this.radioBottonUptime.Name = "radioBottonUptime";
            this.radioBottonUptime.Size = new System.Drawing.Size(73, 21);
            this.radioBottonUptime.TabIndex = 8;
            this.radioBottonUptime.TabStop = true;
            this.radioBottonUptime.Text = "Uptime";
            this.radioBottonUptime.UseVisualStyleBackColor = true;
            // 
            // radioButtonMAC
            // 
            this.radioButtonMAC.AutoSize = true;
            this.radioButtonMAC.Location = new System.Drawing.Point(215, 81);
            this.radioButtonMAC.Name = "radioButtonMAC";
            this.radioButtonMAC.Size = new System.Drawing.Size(58, 21);
            this.radioButtonMAC.TabIndex = 10;
            this.radioButtonMAC.TabStop = true;
            this.radioButtonMAC.Text = "MAC";
            this.radioButtonMAC.UseVisualStyleBackColor = true;
            // 
            // radioButtonYellow
            // 
            this.radioButtonYellow.AutoSize = true;
            this.radioButtonYellow.Location = new System.Drawing.Point(215, 108);
            this.radioButtonYellow.Name = "radioButtonYellow";
            this.radioButtonYellow.Size = new System.Drawing.Size(69, 21);
            this.radioButtonYellow.TabIndex = 11;
            this.radioButtonYellow.TabStop = true;
            this.radioButtonYellow.Text = "Yellow";
            this.radioButtonYellow.UseVisualStyleBackColor = true;
            // 
            // radioButtonCiano
            // 
            this.radioButtonCiano.AutoSize = true;
            this.radioButtonCiano.Location = new System.Drawing.Point(215, 135);
            this.radioButtonCiano.Name = "radioButtonCiano";
            this.radioButtonCiano.Size = new System.Drawing.Size(65, 21);
            this.radioButtonCiano.TabIndex = 12;
            this.radioButtonCiano.TabStop = true;
            this.radioButtonCiano.Text = "Ciano";
            this.radioButtonCiano.UseVisualStyleBackColor = true;
            // 
            // radioButtonMagenta
            // 
            this.radioButtonMagenta.AutoSize = true;
            this.radioButtonMagenta.Location = new System.Drawing.Point(215, 162);
            this.radioButtonMagenta.Name = "radioButtonMagenta";
            this.radioButtonMagenta.Size = new System.Drawing.Size(84, 21);
            this.radioButtonMagenta.TabIndex = 13;
            this.radioButtonMagenta.TabStop = true;
            this.radioButtonMagenta.Text = "Magenta";
            this.radioButtonMagenta.UseVisualStyleBackColor = true;
            // 
            // radioButtonBlack
            // 
            this.radioButtonBlack.AutoSize = true;
            this.radioButtonBlack.Location = new System.Drawing.Point(215, 189);
            this.radioButtonBlack.Name = "radioButtonBlack";
            this.radioButtonBlack.Size = new System.Drawing.Size(63, 21);
            this.radioButtonBlack.TabIndex = 14;
            this.radioButtonBlack.TabStop = true;
            this.radioButtonBlack.Text = "Black";
            this.radioButtonBlack.UseVisualStyleBackColor = true;
            // 
            // SNMPControllo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 232);
            this.Controls.Add(this.radioButtonBlack);
            this.Controls.Add(this.radioButtonMagenta);
            this.Controls.Add(this.radioButtonCiano);
            this.Controls.Add(this.radioButtonYellow);
            this.Controls.Add(this.radioButtonMAC);
            this.Controls.Add(this.radioBottonUptime);
            this.Controls.Add(this.radioButtonComplete);
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
        private System.Windows.Forms.RadioButton radioButtonComplete;
        private System.Windows.Forms.RadioButton radioBottonUptime;
        private System.Windows.Forms.RadioButton radioButtonMAC;
        private System.Windows.Forms.RadioButton radioButtonYellow;
        private System.Windows.Forms.RadioButton radioButtonCiano;
        private System.Windows.Forms.RadioButton radioButtonMagenta;
        private System.Windows.Forms.RadioButton radioButtonBlack;
    }
}

