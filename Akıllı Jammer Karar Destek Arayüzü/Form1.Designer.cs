namespace Akıllı_Jammer_Karar_Destek_Arayüzü
{
    partial class Form1
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
            btnBaglan = new Button();
            button2 = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            btnOku = new Button();
            numFrekans = new NumericUpDown();
            cmbBirim = new ComboBox();
            rtbKonsol = new RichTextBox();
            numOrnekleme = new NumericUpDown();
            numBantGenisligi = new NumericUpDown();
            cmbOrneklemeBirim = new ComboBox();
            cmbBantBirim = new ComboBox();
            grpDonanim = new GroupBox();
            lblRxKazanciDegeri = new Label();
            btnDcOffset = new Button();
            chkBiasTee = new CheckBox();
            chkAGC = new CheckBox();
            label5 = new Label();
            label4 = new Label();
            trbTxGain = new TrackBar();
            trbRxKazanci = new TrackBar();
            trbOlcek = new TrackBar();
            label6 = new Label();
            picGrafik = new PictureBox();
            groupBox1 = new GroupBox();
            rdoTestModu = new RadioButton();
            rdoTaarruzModu = new RadioButton();
            rdoIzlemeModu = new RadioButton();
            lblOlcekDegeri = new Label();
            cmbHedefProfilleri = new ComboBox();
            trbSquelch = new TrackBar();
            lblTehditDurumu = new Label();
            chkAlarmAktif = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)numFrekans).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numOrnekleme).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numBantGenisligi).BeginInit();
            grpDonanim.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trbTxGain).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trbRxKazanci).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trbOlcek).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picGrafik).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trbSquelch).BeginInit();
            SuspendLayout();
            // 
            // btnBaglan
            // 
            btnBaglan.ForeColor = SystemColors.ActiveCaptionText;
            btnBaglan.Location = new Point(830, 248);
            btnBaglan.Name = "btnBaglan";
            btnBaglan.Size = new Size(108, 95);
            btnBaglan.TabIndex = 0;
            btnBaglan.Text = "Cihaza Bağlan ve Hazırla";
            btnBaglan.UseVisualStyleBackColor = true;
            btnBaglan.Click += btnBaglan_Click;
            // 
            // button2
            // 
            button2.Location = new Point(0, 0);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(1065, 99);
            label1.Name = "label1";
            label1.Size = new Size(93, 20);
            label1.TabIndex = 1;
            label1.Text = "Frekans (Hz):";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = SystemColors.ActiveCaptionText;
            label2.Location = new Point(1064, 171);
            label2.Name = "label2";
            label2.Size = new Size(111, 20);
            label2.TabIndex = 3;
            label2.Text = "Örnekleme Hızı";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = SystemColors.ActiveCaptionText;
            label3.Location = new Point(1064, 250);
            label3.Name = "label3";
            label3.Size = new Size(100, 20);
            label3.TabIndex = 4;
            label3.Text = "Bant Genişliği";
            // 
            // btnOku
            // 
            btnOku.Enabled = false;
            btnOku.ForeColor = SystemColors.ActiveCaptionText;
            btnOku.Location = new Point(947, 248);
            btnOku.Name = "btnOku";
            btnOku.Size = new Size(108, 95);
            btnOku.TabIndex = 7;
            btnOku.Text = "Hızlı Veri Oku";
            btnOku.UseVisualStyleBackColor = true;
            btnOku.Click += btnOku_Click;
            // 
            // numFrekans
            // 
            numFrekans.DecimalPlaces = 3;
            numFrekans.Location = new Point(1065, 122);
            numFrekans.Maximum = new decimal(new int[] { 1705032704, 1, 0, 0 });
            numFrekans.Name = "numFrekans";
            numFrekans.Size = new Size(150, 27);
            numFrekans.TabIndex = 9;
            numFrekans.ThousandsSeparator = true;
            numFrekans.Value = new decimal(new int[] { 70, 0, 0, 0 });
            // 
            // cmbBirim
            // 
            cmbBirim.Cursor = Cursors.Hand;
            cmbBirim.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBirim.FormattingEnabled = true;
            cmbBirim.Items.AddRange(new object[] { "MHz", "GHz" });
            cmbBirim.Location = new Point(1222, 122);
            cmbBirim.Name = "cmbBirim";
            cmbBirim.Size = new Size(72, 28);
            cmbBirim.TabIndex = 10;
            cmbBirim.SelectedIndexChanged += cmbBirim_SelectedIndexChanged;
            // 
            // rtbKonsol
            // 
            rtbKonsol.Location = new Point(830, 99);
            rtbKonsol.Name = "rtbKonsol";
            rtbKonsol.Size = new Size(225, 143);
            rtbKonsol.TabIndex = 11;
            rtbKonsol.Text = "";
            // 
            // numOrnekleme
            // 
            numOrnekleme.DecimalPlaces = 3;
            numOrnekleme.Location = new Point(1065, 194);
            numOrnekleme.Maximum = new decimal(new int[] { 61440000, 0, 0, 0 });
            numOrnekleme.Name = "numOrnekleme";
            numOrnekleme.Size = new Size(150, 27);
            numOrnekleme.TabIndex = 15;
            numOrnekleme.ThousandsSeparator = true;
            numOrnekleme.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // numBantGenisligi
            // 
            numBantGenisligi.DecimalPlaces = 3;
            numBantGenisligi.Location = new Point(1065, 273);
            numBantGenisligi.Maximum = new decimal(new int[] { 56000000, 0, 0, 0 });
            numBantGenisligi.Name = "numBantGenisligi";
            numBantGenisligi.Size = new Size(150, 27);
            numBantGenisligi.TabIndex = 16;
            numBantGenisligi.ThousandsSeparator = true;
            numBantGenisligi.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // cmbOrneklemeBirim
            // 
            cmbOrneklemeBirim.Cursor = Cursors.Hand;
            cmbOrneklemeBirim.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOrneklemeBirim.FormattingEnabled = true;
            cmbOrneklemeBirim.Items.AddRange(new object[] { "kSps ", "MSps " });
            cmbOrneklemeBirim.Location = new Point(1221, 194);
            cmbOrneklemeBirim.Name = "cmbOrneklemeBirim";
            cmbOrneklemeBirim.Size = new Size(73, 28);
            cmbOrneklemeBirim.TabIndex = 17;
            cmbOrneklemeBirim.SelectedIndexChanged += cmbOrneklemeBirim_SelectedIndexChanged;
            // 
            // cmbBantBirim
            // 
            cmbBantBirim.Cursor = Cursors.Hand;
            cmbBantBirim.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBantBirim.FormattingEnabled = true;
            cmbBantBirim.Items.AddRange(new object[] { "kHz", "MHz" });
            cmbBantBirim.Location = new Point(1222, 273);
            cmbBantBirim.Name = "cmbBantBirim";
            cmbBantBirim.RightToLeft = RightToLeft.No;
            cmbBantBirim.Size = new Size(73, 28);
            cmbBantBirim.TabIndex = 18;
            cmbBantBirim.Tag = "";
            cmbBantBirim.SelectedIndexChanged += cmbBantBirim_SelectedIndexChanged;
            // 
            // grpDonanim
            // 
            grpDonanim.Controls.Add(lblRxKazanciDegeri);
            grpDonanim.Controls.Add(btnDcOffset);
            grpDonanim.Controls.Add(chkBiasTee);
            grpDonanim.Controls.Add(chkAGC);
            grpDonanim.Controls.Add(label5);
            grpDonanim.Controls.Add(label4);
            grpDonanim.Controls.Add(trbTxGain);
            grpDonanim.Controls.Add(trbRxKazanci);
            grpDonanim.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            grpDonanim.ForeColor = SystemColors.ActiveCaptionText;
            grpDonanim.Location = new Point(1314, 99);
            grpDonanim.Name = "grpDonanim";
            grpDonanim.Size = new Size(296, 315);
            grpDonanim.TabIndex = 19;
            grpDonanim.TabStop = false;
            grpDonanim.Text = "Gelişmiş RF Kontrolleri";
            grpDonanim.Enter += grpDonanim_Enter;
            // 
            // lblRxKazanciDegeri
            // 
            lblRxKazanciDegeri.AutoSize = true;
            lblRxKazanciDegeri.BackColor = Color.White;
            lblRxKazanciDegeri.Location = new Point(236, 52);
            lblRxKazanciDegeri.Name = "lblRxKazanciDegeri";
            lblRxKazanciDegeri.Size = new Size(44, 23);
            lblRxKazanciDegeri.TabIndex = 8;
            lblRxKazanciDegeri.Text = "0 dB";
            // 
            // btnDcOffset
            // 
            btnDcOffset.BackColor = Color.Red;
            btnDcOffset.Location = new Point(27, 233);
            btnDcOffset.Name = "btnDcOffset";
            btnDcOffset.Size = new Size(229, 68);
            btnDcOffset.TabIndex = 7;
            btnDcOffset.Text = "DC Offset Kalibrasyonu (LO Temizle)";
            btnDcOffset.UseVisualStyleBackColor = false;
            btnDcOffset.Click += btnDcOffset_Click;
            // 
            // chkBiasTee
            // 
            chkBiasTee.AutoSize = true;
            chkBiasTee.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            chkBiasTee.Location = new Point(27, 193);
            chkBiasTee.Name = "chkBiasTee";
            chkBiasTee.Size = new Size(200, 24);
            chkBiasTee.TabIndex = 5;
            chkBiasTee.Text = "Bias-Tee (Aktif Anten 5V)";
            chkBiasTee.UseVisualStyleBackColor = true;
            chkBiasTee.CheckedChanged += chkBiasTee_CheckedChanged;
            // 
            // chkAGC
            // 
            chkAGC.AutoSize = true;
            chkAGC.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            chkAGC.Location = new Point(27, 163);
            chkAGC.Name = "chkAGC";
            chkAGC.Size = new Size(253, 24);
            chkAGC.TabIndex = 4;
            chkAGC.Text = "AGC (Otomatik Kazanç Kontrolü)";
            chkAGC.UseVisualStyleBackColor = true;
            chkAGC.CheckedChanged += chkAGC_CheckedChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label5.Location = new Point(27, 88);
            label5.Name = "label5";
            label5.Size = new Size(221, 20);
            label5.TabIndex = 3;
            label5.Text = "TX Kazancı (Taarruz Çıkış Gücü)";
            label5.Click += label5_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label4.Location = new Point(27, 29);
            label4.Name = "label4";
            label4.Size = new Size(154, 20);
            label4.TabIndex = 2;
            label4.Text = "RX Kazancı (Dinleme)";
            // 
            // trbTxGain
            // 
            trbTxGain.Location = new Point(37, 115);
            trbTxGain.Maximum = 89;
            trbTxGain.Name = "trbTxGain";
            trbTxGain.Size = new Size(203, 56);
            trbTxGain.TabIndex = 1;
            trbTxGain.TickFrequency = 5;
            // 
            // trbRxKazanci
            // 
            trbRxKazanci.Location = new Point(37, 52);
            trbRxKazanci.Maximum = 60;
            trbRxKazanci.Name = "trbRxKazanci";
            trbRxKazanci.Size = new Size(203, 56);
            trbRxKazanci.TabIndex = 0;
            trbRxKazanci.Scroll += trbRxKazanci_Scroll;
            // 
            // trbOlcek
            // 
            trbOlcek.BackColor = Color.WhiteSmoke;
            trbOlcek.Location = new Point(22, 440);
            trbOlcek.Maximum = 10000;
            trbOlcek.Minimum = 100;
            trbOlcek.Name = "trbOlcek";
            trbOlcek.Size = new Size(802, 56);
            trbOlcek.TabIndex = 20;
            trbOlcek.TickFrequency = 1000;
            trbOlcek.Value = 4000;
            trbOlcek.Scroll += trbOlcek_Scroll;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label6.ForeColor = SystemColors.ActiveCaptionText;
            label6.Location = new Point(287, 414);
            label6.Name = "label6";
            label6.Size = new Size(238, 23);
            label6.TabIndex = 21;
            label6.Text = "Osiloskop Ölçeği (VOLTS/DIV)";
            label6.Click += label6_Click;
            // 
            // picGrafik
            // 
            picGrafik.BackColor = Color.Black;
            picGrafik.Location = new Point(21, 27);
            picGrafik.Name = "picGrafik";
            picGrafik.Size = new Size(803, 387);
            picGrafik.TabIndex = 22;
            picGrafik.TabStop = false;
            picGrafik.Click += picGrafik_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(rdoTestModu);
            groupBox1.Controls.Add(rdoTaarruzModu);
            groupBox1.Controls.Add(rdoIzlemeModu);
            groupBox1.Location = new Point(893, 25);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(615, 68);
            groupBox1.TabIndex = 23;
            groupBox1.TabStop = false;
            groupBox1.Text = "Operasyon Modu";
            // 
            // rdoTestModu
            // 
            rdoTestModu.AutoSize = true;
            rdoTestModu.BackColor = Color.Black;
            rdoTestModu.Location = new Point(415, 34);
            rdoTestModu.Name = "rdoTestModu";
            rdoTestModu.Size = new Size(178, 24);
            rdoTestModu.TabIndex = 2;
            rdoTestModu.Text = "Dahili Test (Loopback)";
            rdoTestModu.UseVisualStyleBackColor = false;
            rdoTestModu.CheckedChanged += rdoTestModu_CheckedChanged;
            // 
            // rdoTaarruzModu
            // 
            rdoTaarruzModu.AutoSize = true;
            rdoTaarruzModu.BackColor = Color.Black;
            rdoTaarruzModu.Location = new Point(223, 34);
            rdoTaarruzModu.Name = "rdoTaarruzModu";
            rdoTaarruzModu.Size = new Size(186, 24);
            rdoTaarruzModu.TabIndex = 1;
            rdoTaarruzModu.Text = "Taarruz Modu (TX Aktif)";
            rdoTaarruzModu.UseVisualStyleBackColor = false;
            rdoTaarruzModu.CheckedChanged += rdoTaarruzModu_CheckedChanged;
            // 
            // rdoIzlemeModu
            // 
            rdoIzlemeModu.AutoSize = true;
            rdoIzlemeModu.BackColor = Color.Black;
            rdoIzlemeModu.Checked = true;
            rdoIzlemeModu.Location = new Point(16, 34);
            rdoIzlemeModu.Name = "rdoIzlemeModu";
            rdoIzlemeModu.Size = new Size(201, 24);
            rdoIzlemeModu.TabIndex = 0;
            rdoIzlemeModu.TabStop = true;
            rdoIzlemeModu.Text = "İzleme Modu (RX Sadece)";
            rdoIzlemeModu.UseVisualStyleBackColor = false;
            rdoIzlemeModu.CheckedChanged += rdoIzlemeModu_CheckedChanged;
            // 
            // lblOlcekDegeri
            // 
            lblOlcekDegeri.AutoSize = true;
            lblOlcekDegeri.BackColor = Color.White;
            lblOlcekDegeri.ForeColor = SystemColors.ActiveCaptionText;
            lblOlcekDegeri.Location = new Point(287, 499);
            lblOlcekDegeri.Name = "lblOlcekDegeri";
            lblOlcekDegeri.Size = new Size(63, 20);
            lblOlcekDegeri.TabIndex = 24;
            lblOlcekDegeri.Text = "4000 dB";
            // 
            // cmbHedefProfilleri
            // 
            cmbHedefProfilleri.Cursor = Cursors.Hand;
            cmbHedefProfilleri.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbHedefProfilleri.FormattingEnabled = true;
            cmbHedefProfilleri.Items.AddRange(new object[] { "-- Manuel Giriş --", "DJI Drone / Wi-Fi (2.4 GHz)", "Taktik Telsiz (UHF 433 MHz)", "GSM / LTE Telefon (1.75 GHz)" });
            cmbHedefProfilleri.Location = new Point(1067, 353);
            cmbHedefProfilleri.Name = "cmbHedefProfilleri";
            cmbHedefProfilleri.Size = new Size(228, 28);
            cmbHedefProfilleri.TabIndex = 25;
            cmbHedefProfilleri.SelectedIndexChanged += cmbHedefProfilleri_SelectedIndexChanged;
            // 
            // trbSquelch
            // 
            trbSquelch.BackColor = Color.WhiteSmoke;
            trbSquelch.Location = new Point(830, 387);
            trbSquelch.Maximum = 5000;
            trbSquelch.Minimum = 500;
            trbSquelch.Name = "trbSquelch";
            trbSquelch.Size = new Size(225, 56);
            trbSquelch.TabIndex = 9;
            trbSquelch.Value = 500;
            trbSquelch.Scroll += trbSquelch_Scroll;
            // 
            // lblTehditDurumu
            // 
            lblTehditDurumu.AutoSize = true;
            lblTehditDurumu.BackColor = Color.DarkGreen;
            lblTehditDurumu.BorderStyle = BorderStyle.FixedSingle;
            lblTehditDurumu.Font = new Font("Segoe UI Black", 18F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblTehditDurumu.ForeColor = SystemColors.ButtonFace;
            lblTehditDurumu.Location = new Point(830, 451);
            lblTehditDurumu.Name = "lblTehditDurumu";
            lblTehditDurumu.Size = new Size(359, 43);
            lblTehditDurumu.TabIndex = 26;
            lblTehditDurumu.Text = "TEMİZ - DİNLENİYOR...";
            // 
            // chkAlarmAktif
            // 
            chkAlarmAktif.AutoSize = true;
            chkAlarmAktif.BackColor = Color.White;
            chkAlarmAktif.Checked = true;
            chkAlarmAktif.CheckState = CheckState.Checked;
            chkAlarmAktif.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            chkAlarmAktif.ForeColor = SystemColors.ActiveCaptionText;
            chkAlarmAktif.Location = new Point(830, 353);
            chkAlarmAktif.Name = "chkAlarmAktif";
            chkAlarmAktif.Size = new Size(223, 27);
            chkAlarmAktif.TabIndex = 9;
            chkAlarmAktif.Text = "Sinyal Tehdit Alarmı Aktif";
            chkAlarmAktif.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkGray;
            ClientSize = new Size(1483, 557);
            Controls.Add(chkAlarmAktif);
            Controls.Add(lblTehditDurumu);
            Controls.Add(trbSquelch);
            Controls.Add(cmbHedefProfilleri);
            Controls.Add(lblOlcekDegeri);
            Controls.Add(groupBox1);
            Controls.Add(picGrafik);
            Controls.Add(label6);
            Controls.Add(trbOlcek);
            Controls.Add(grpDonanim);
            Controls.Add(cmbBantBirim);
            Controls.Add(cmbOrneklemeBirim);
            Controls.Add(numBantGenisligi);
            Controls.Add(numOrnekleme);
            Controls.Add(rtbKonsol);
            Controls.Add(cmbBirim);
            Controls.Add(numFrekans);
            Controls.Add(btnOku);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnBaglan);
            ForeColor = SystemColors.ButtonHighlight;
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)numFrekans).EndInit();
            ((System.ComponentModel.ISupportInitialize)numOrnekleme).EndInit();
            ((System.ComponentModel.ISupportInitialize)numBantGenisligi).EndInit();
            grpDonanim.ResumeLayout(false);
            grpDonanim.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trbTxGain).EndInit();
            ((System.ComponentModel.ISupportInitialize)trbRxKazanci).EndInit();
            ((System.ComponentModel.ISupportInitialize)trbOlcek).EndInit();
            ((System.ComponentModel.ISupportInitialize)picGrafik).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trbSquelch).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnBaglan;
        private Button button2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button btnOku;
        private NumericUpDown numFrekans;
        private ComboBox cmbBirim;
        private RichTextBox rtbKonsol;
        private NumericUpDown numOrnekleme;
        private NumericUpDown numBantGenisligi;
        private ComboBox cmbOrneklemeBirim;
        private ComboBox cmbBantBirim;
        private GroupBox grpDonanim;
        private TrackBar trbTxGain;
        private TrackBar trbRxKazanci;
        private Label label5;
        private Label label4;
        private CheckBox chkBiasTee;
        private CheckBox chkAGC;
        private TrackBar trbOlcek;
        private Label label6;
        private PictureBox picGrafik;
        private GroupBox groupBox1;
        private RadioButton rdoTestModu;
        private RadioButton rdoTaarruzModu;
        private RadioButton rdoIzlemeModu;
        private Button btnDcOffset;
        private Label lblRxKazanciDegeri;
        private Label lblOlcekDegeri;
        private ComboBox cmbHedefProfilleri;
        private TrackBar trbSquelch;
        private Label lblTehditDurumu;
        private CheckBox chkAlarmAktif;
    }
}