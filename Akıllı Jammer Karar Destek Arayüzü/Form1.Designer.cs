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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
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
            btnDcKalibrasyon = new Button();
            chkBiasTee = new CheckBox();
            chkAGC = new CheckBox();
            label5 = new Label();
            label4 = new Label();
            trbTxGain = new TrackBar();
            trbRxKazanci = new TrackBar();
            trbYumusatma = new TrackBar();
            label6 = new Label();
            picGrafik = new PictureBox();
            groupBox1 = new GroupBox();
            rdoTestModu = new RadioButton();
            rdoTaarruzModu = new RadioButton();
            rdoIzlemeModu = new RadioButton();
            cmbHedefProfilleri = new ComboBox();
            trbSquelch = new TrackBar();
            lblTehditDurumu = new Label();
            chkAlarmAktif = new CheckBox();
            trbTaramaHizi = new TrackBar();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            lblYumusatmaDegeri = new Label();
            lblTaramaHiziDegeri = new Label();
            lblSqulechTehtid = new Label();
            numYMax = new NumericUpDown();
            numYMin = new NumericUpDown();
            numGurultuEsigi = new NumericUpDown();
            chkGurultuEngelle = new CheckBox();
            numKirpmaYuzdesi = new NumericUpDown();
            label10 = new Label();
            lblKırpılmayanAlan = new Label();
            ((System.ComponentModel.ISupportInitialize)numFrekans).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numOrnekleme).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numBantGenisligi).BeginInit();
            grpDonanim.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trbTxGain).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trbRxKazanci).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trbYumusatma).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picGrafik).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trbSquelch).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trbTaramaHizi).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numYMax).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numYMin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numGurultuEsigi).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numKirpmaYuzdesi).BeginInit();
            SuspendLayout();
            // 
            // btnBaglan
            // 
            btnBaglan.Anchor = AnchorStyles.None;
            btnBaglan.ForeColor = SystemColors.ActiveCaptionText;
            btnBaglan.Location = new Point(1123, 326);
            btnBaglan.Name = "btnBaglan";
            btnBaglan.Size = new Size(108, 95);
            btnBaglan.TabIndex = 0;
            btnBaglan.Text = "Cihaza Bağlan ve Hazırla";
            btnBaglan.UseVisualStyleBackColor = true;
            btnBaglan.Click += btnBaglan_Click;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.None;
            button2.Location = new Point(0, 0);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 0;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(1358, 177);
            label1.Name = "label1";
            label1.Size = new Size(93, 20);
            label1.TabIndex = 1;
            label1.Text = "Frekans (Hz):";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.ForeColor = SystemColors.ActiveCaptionText;
            label2.Location = new Point(1357, 249);
            label2.Name = "label2";
            label2.Size = new Size(111, 20);
            label2.TabIndex = 3;
            label2.Text = "Örnekleme Hızı";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.None;
            label3.AutoSize = true;
            label3.ForeColor = SystemColors.ActiveCaptionText;
            label3.Location = new Point(1357, 328);
            label3.Name = "label3";
            label3.Size = new Size(100, 20);
            label3.TabIndex = 4;
            label3.Text = "Bant Genişliği";
            // 
            // btnOku
            // 
            btnOku.Anchor = AnchorStyles.None;
            btnOku.Enabled = false;
            btnOku.ForeColor = SystemColors.ActiveCaptionText;
            btnOku.Location = new Point(1240, 326);
            btnOku.Name = "btnOku";
            btnOku.Size = new Size(108, 95);
            btnOku.TabIndex = 7;
            btnOku.Text = "Hızlı Veri Oku";
            btnOku.UseVisualStyleBackColor = true;
            btnOku.Click += btnOku_Click;
            // 
            // numFrekans
            // 
            numFrekans.Anchor = AnchorStyles.None;
            numFrekans.DecimalPlaces = 3;
            numFrekans.Location = new Point(1358, 200);
            numFrekans.Maximum = new decimal(new int[] { 1705032704, 1, 0, 0 });
            numFrekans.Name = "numFrekans";
            numFrekans.Size = new Size(100, 27);
            numFrekans.TabIndex = 9;
            numFrekans.ThousandsSeparator = true;
            numFrekans.Value = new decimal(new int[] { 2000, 0, 0, 0 });
            numFrekans.ValueChanged += numFrekans_ValueChanged;
            // 
            // cmbBirim
            // 
            cmbBirim.Anchor = AnchorStyles.None;
            cmbBirim.Cursor = Cursors.Hand;
            cmbBirim.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBirim.FormattingEnabled = true;
            cmbBirim.Items.AddRange(new object[] { "MHz", "GHz" });
            cmbBirim.Location = new Point(1464, 199);
            cmbBirim.Name = "cmbBirim";
            cmbBirim.Size = new Size(72, 28);
            cmbBirim.TabIndex = 10;
            cmbBirim.SelectedIndexChanged += cmbBirim_SelectedIndexChanged;
            // 
            // rtbKonsol
            // 
            rtbKonsol.Anchor = AnchorStyles.None;
            rtbKonsol.Location = new Point(1123, 177);
            rtbKonsol.Name = "rtbKonsol";
            rtbKonsol.Size = new Size(225, 143);
            rtbKonsol.TabIndex = 11;
            rtbKonsol.Text = "";
            // 
            // numOrnekleme
            // 
            numOrnekleme.Anchor = AnchorStyles.None;
            numOrnekleme.DecimalPlaces = 3;
            numOrnekleme.Location = new Point(1358, 272);
            numOrnekleme.Maximum = new decimal(new int[] { 61440000, 0, 0, 0 });
            numOrnekleme.Name = "numOrnekleme";
            numOrnekleme.Size = new Size(100, 27);
            numOrnekleme.TabIndex = 15;
            numOrnekleme.ThousandsSeparator = true;
            numOrnekleme.Value = new decimal(new int[] { 24000, 0, 0, 0 });
            // 
            // numBantGenisligi
            // 
            numBantGenisligi.Anchor = AnchorStyles.None;
            numBantGenisligi.DecimalPlaces = 3;
            numBantGenisligi.Location = new Point(1358, 351);
            numBantGenisligi.Maximum = new decimal(new int[] { 56000000, 0, 0, 0 });
            numBantGenisligi.Name = "numBantGenisligi";
            numBantGenisligi.Size = new Size(100, 27);
            numBantGenisligi.TabIndex = 16;
            numBantGenisligi.ThousandsSeparator = true;
            numBantGenisligi.Value = new decimal(new int[] { 20000, 0, 0, 0 });
            // 
            // cmbOrneklemeBirim
            // 
            cmbOrneklemeBirim.Anchor = AnchorStyles.None;
            cmbOrneklemeBirim.Cursor = Cursors.Hand;
            cmbOrneklemeBirim.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOrneklemeBirim.FormattingEnabled = true;
            cmbOrneklemeBirim.Items.AddRange(new object[] { "kSps ", "MSps " });
            cmbOrneklemeBirim.Location = new Point(1463, 271);
            cmbOrneklemeBirim.Name = "cmbOrneklemeBirim";
            cmbOrneklemeBirim.Size = new Size(73, 28);
            cmbOrneklemeBirim.TabIndex = 17;
            cmbOrneklemeBirim.SelectedIndexChanged += cmbOrneklemeBirim_SelectedIndexChanged;
            // 
            // cmbBantBirim
            // 
            cmbBantBirim.Anchor = AnchorStyles.None;
            cmbBantBirim.Cursor = Cursors.Hand;
            cmbBantBirim.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBantBirim.FormattingEnabled = true;
            cmbBantBirim.Items.AddRange(new object[] { "kHz", "MHz" });
            cmbBantBirim.Location = new Point(1464, 350);
            cmbBantBirim.Name = "cmbBantBirim";
            cmbBantBirim.RightToLeft = RightToLeft.No;
            cmbBantBirim.Size = new Size(73, 28);
            cmbBantBirim.TabIndex = 18;
            cmbBantBirim.Tag = "";
            cmbBantBirim.SelectedIndexChanged += cmbBantBirim_SelectedIndexChanged;
            // 
            // grpDonanim
            // 
            grpDonanim.Anchor = AnchorStyles.None;
            grpDonanim.Controls.Add(lblRxKazanciDegeri);
            grpDonanim.Controls.Add(btnDcKalibrasyon);
            grpDonanim.Controls.Add(chkBiasTee);
            grpDonanim.Controls.Add(chkAGC);
            grpDonanim.Controls.Add(label5);
            grpDonanim.Controls.Add(label4);
            grpDonanim.Controls.Add(trbTxGain);
            grpDonanim.Controls.Add(trbRxKazanci);
            grpDonanim.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            grpDonanim.ForeColor = SystemColors.ActiveCaptionText;
            grpDonanim.Location = new Point(1607, 177);
            grpDonanim.Name = "grpDonanim";
            grpDonanim.Size = new Size(296, 315);
            grpDonanim.TabIndex = 19;
            grpDonanim.TabStop = false;
            grpDonanim.Text = "Gelişmiş RF Kontrolleri";
            grpDonanim.Enter += grpDonanim_Enter;
            // 
            // lblRxKazanciDegeri
            // 
            lblRxKazanciDegeri.Anchor = AnchorStyles.None;
            lblRxKazanciDegeri.AutoSize = true;
            lblRxKazanciDegeri.BackColor = Color.White;
            lblRxKazanciDegeri.Location = new Point(236, 52);
            lblRxKazanciDegeri.Name = "lblRxKazanciDegeri";
            lblRxKazanciDegeri.Size = new Size(44, 23);
            lblRxKazanciDegeri.TabIndex = 8;
            lblRxKazanciDegeri.Text = "0 dB";
            lblRxKazanciDegeri.Click += lblRxKazanciDegeri_Click;
            // 
            // btnDcKalibrasyon
            // 
            btnDcKalibrasyon.Anchor = AnchorStyles.None;
            btnDcKalibrasyon.BackColor = Color.Red;
            btnDcKalibrasyon.Location = new Point(27, 233);
            btnDcKalibrasyon.Name = "btnDcKalibrasyon";
            btnDcKalibrasyon.Size = new Size(229, 68);
            btnDcKalibrasyon.TabIndex = 7;
            btnDcKalibrasyon.Text = "DC Offset Kalibrasyonu (LO Temizle)";
            btnDcKalibrasyon.UseVisualStyleBackColor = false;
            btnDcKalibrasyon.Click += btnDcKalibrasyon_Click;
            // 
            // chkBiasTee
            // 
            chkBiasTee.Anchor = AnchorStyles.None;
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
            chkAGC.Anchor = AnchorStyles.None;
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
            label5.Anchor = AnchorStyles.None;
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
            label4.Anchor = AnchorStyles.None;
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
            trbTxGain.Anchor = AnchorStyles.None;
            trbTxGain.Location = new Point(37, 115);
            trbTxGain.Maximum = 89;
            trbTxGain.Name = "trbTxGain";
            trbTxGain.Size = new Size(203, 56);
            trbTxGain.TabIndex = 1;
            trbTxGain.TickFrequency = 5;
            // 
            // trbRxKazanci
            // 
            trbRxKazanci.Anchor = AnchorStyles.None;
            trbRxKazanci.Location = new Point(37, 52);
            trbRxKazanci.Maximum = 60;
            trbRxKazanci.Name = "trbRxKazanci";
            trbRxKazanci.Size = new Size(203, 56);
            trbRxKazanci.TabIndex = 0;
            trbRxKazanci.Scroll += trbRxKazanci_Scroll;
            trbRxKazanci.MouseUp += trbRxKazanci_MouseUp;
            // 
            // trbYumusatma
            // 
            trbYumusatma.Anchor = AnchorStyles.None;
            trbYumusatma.BackColor = Color.WhiteSmoke;
            trbYumusatma.Location = new Point(150, 663);
            trbYumusatma.Maximum = 100;
            trbYumusatma.Minimum = 1;
            trbYumusatma.Name = "trbYumusatma";
            trbYumusatma.Size = new Size(802, 56);
            trbYumusatma.TabIndex = 20;
            trbYumusatma.TickFrequency = 1000;
            trbYumusatma.Value = 1;
            trbYumusatma.Scroll += trbYumusatma_Scroll;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.None;
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label6.ForeColor = SystemColors.ActiveCaptionText;
            label6.Location = new Point(410, 637);
            label6.Name = "label6";
            label6.Size = new Size(254, 23);
            label6.TabIndex = 21;
            label6.Text = "Sinyal Yumuşatma (Video Filter)";
            label6.Click += label6_Click;
            // 
            // picGrafik
            // 
            picGrafik.BackColor = Color.Black;
            picGrafik.Location = new Point(71, 103);
            picGrafik.Name = "picGrafik";
            picGrafik.Size = new Size(1000, 500);
            picGrafik.TabIndex = 22;
            picGrafik.TabStop = false;
            picGrafik.Click += picGrafik_Click;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.None;
            groupBox1.Controls.Add(rdoTestModu);
            groupBox1.Controls.Add(rdoTaarruzModu);
            groupBox1.Controls.Add(rdoIzlemeModu);
            groupBox1.Location = new Point(1186, 103);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(615, 68);
            groupBox1.TabIndex = 23;
            groupBox1.TabStop = false;
            groupBox1.Text = "Operasyon Modu";
            // 
            // rdoTestModu
            // 
            rdoTestModu.Anchor = AnchorStyles.None;
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
            rdoTaarruzModu.Anchor = AnchorStyles.None;
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
            rdoIzlemeModu.Anchor = AnchorStyles.None;
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
            // cmbHedefProfilleri
            // 
            cmbHedefProfilleri.Anchor = AnchorStyles.None;
            cmbHedefProfilleri.Cursor = Cursors.Hand;
            cmbHedefProfilleri.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbHedefProfilleri.FormattingEnabled = true;
            cmbHedefProfilleri.Items.AddRange(new object[] { "-- Manuel Giriş --", "DJI Drone / Wi-Fi (2.4 GHz)", "Taktik Telsiz (UHF 433 MHz)", "GSM / LTE Telefon (1.75 GHz)" });
            cmbHedefProfilleri.Location = new Point(1360, 431);
            cmbHedefProfilleri.Name = "cmbHedefProfilleri";
            cmbHedefProfilleri.Size = new Size(228, 28);
            cmbHedefProfilleri.TabIndex = 25;
            cmbHedefProfilleri.SelectedIndexChanged += cmbHedefProfilleri_SelectedIndexChanged;
            // 
            // trbSquelch
            // 
            trbSquelch.Anchor = AnchorStyles.None;
            trbSquelch.BackColor = Color.WhiteSmoke;
            trbSquelch.Location = new Point(1123, 465);
            trbSquelch.Maximum = 100;
            trbSquelch.Minimum = -100;
            trbSquelch.Name = "trbSquelch";
            trbSquelch.Size = new Size(225, 56);
            trbSquelch.TabIndex = 9;
            trbSquelch.Scroll += trbSquelch_Scroll;
            // 
            // lblTehditDurumu
            // 
            lblTehditDurumu.Anchor = AnchorStyles.None;
            lblTehditDurumu.AutoSize = true;
            lblTehditDurumu.BackColor = Color.DarkGreen;
            lblTehditDurumu.BorderStyle = BorderStyle.FixedSingle;
            lblTehditDurumu.Font = new Font("Segoe UI Black", 18F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblTehditDurumu.ForeColor = SystemColors.ButtonFace;
            lblTehditDurumu.Location = new Point(159, 30);
            lblTehditDurumu.Name = "lblTehditDurumu";
            lblTehditDurumu.Size = new Size(359, 43);
            lblTehditDurumu.TabIndex = 26;
            lblTehditDurumu.Text = "TEMİZ - DİNLENİYOR...";
            lblTehditDurumu.Click += lblTehditDurumu_Click;
            // 
            // chkAlarmAktif
            // 
            chkAlarmAktif.Anchor = AnchorStyles.None;
            chkAlarmAktif.AutoSize = true;
            chkAlarmAktif.BackColor = Color.White;
            chkAlarmAktif.Checked = true;
            chkAlarmAktif.CheckState = CheckState.Checked;
            chkAlarmAktif.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            chkAlarmAktif.ForeColor = SystemColors.ActiveCaptionText;
            chkAlarmAktif.Location = new Point(1123, 431);
            chkAlarmAktif.Name = "chkAlarmAktif";
            chkAlarmAktif.Size = new Size(223, 27);
            chkAlarmAktif.TabIndex = 9;
            chkAlarmAktif.Text = "Sinyal Tehdit Alarmı Aktif";
            chkAlarmAktif.UseVisualStyleBackColor = false;
            // 
            // trbTaramaHizi
            // 
            trbTaramaHizi.Anchor = AnchorStyles.None;
            trbTaramaHizi.BackColor = Color.WhiteSmoke;
            trbTaramaHizi.Location = new Point(150, 762);
            trbTaramaHizi.Maximum = 200;
            trbTaramaHizi.Minimum = 10;
            trbTaramaHizi.Name = "trbTaramaHizi";
            trbTaramaHizi.Size = new Size(802, 56);
            trbTaramaHizi.TabIndex = 29;
            trbTaramaHizi.TickFrequency = 1000;
            trbTaramaHizi.Value = 10;
            trbTaramaHizi.Scroll += trbTaramaHizi_Scroll;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.None;
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label7.ForeColor = SystemColors.ActiveCaptionText;
            label7.Location = new Point(422, 736);
            label7.Name = "label7";
            label7.Size = new Size(221, 23);
            label7.TabIndex = 30;
            label7.Text = "Tarama Hızı (Gecikme - ms)";
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.None;
            label8.AutoSize = true;
            label8.BackColor = Color.White;
            label8.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label8.ForeColor = SystemColors.ActiveCaptionText;
            label8.Location = new Point(1206, 530);
            label8.Name = "label8";
            label8.Size = new Size(59, 23);
            label8.TabIndex = 31;
            label8.Text = "Max Y";
            label8.Click += label8_Click;
            // 
            // label9
            // 
            label9.Anchor = AnchorStyles.None;
            label9.AutoSize = true;
            label9.BackColor = Color.White;
            label9.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label9.ForeColor = SystemColors.ActiveCaptionText;
            label9.Location = new Point(1206, 563);
            label9.Name = "label9";
            label9.Size = new Size(55, 23);
            label9.TabIndex = 32;
            label9.Text = "Min Y";
            // 
            // lblYumusatmaDegeri
            // 
            lblYumusatmaDegeri.Anchor = AnchorStyles.None;
            lblYumusatmaDegeri.AutoSize = true;
            lblYumusatmaDegeri.BackColor = Color.ForestGreen;
            lblYumusatmaDegeri.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblYumusatmaDegeri.Location = new Point(509, 691);
            lblYumusatmaDegeri.Name = "lblYumusatmaDegeri";
            lblYumusatmaDegeri.Size = new Size(35, 23);
            lblYumusatmaDegeri.TabIndex = 33;
            lblYumusatmaDegeri.Text = "%1";
            lblYumusatmaDegeri.Click += lblYumusatmaDegeri_Click;
            // 
            // lblTaramaHiziDegeri
            // 
            lblTaramaHiziDegeri.Anchor = AnchorStyles.None;
            lblTaramaHiziDegeri.AutoSize = true;
            lblTaramaHiziDegeri.BackColor = Color.ForestGreen;
            lblTaramaHiziDegeri.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblTaramaHiziDegeri.Location = new Point(496, 786);
            lblTaramaHiziDegeri.Name = "lblTaramaHiziDegeri";
            lblTaramaHiziDegeri.Size = new Size(58, 23);
            lblTaramaHiziDegeri.TabIndex = 34;
            lblTaramaHiziDegeri.Text = "10 ms";
            // 
            // lblSqulechTehtid
            // 
            lblSqulechTehtid.Anchor = AnchorStyles.None;
            lblSqulechTehtid.AutoSize = true;
            lblSqulechTehtid.BackColor = Color.ForestGreen;
            lblSqulechTehtid.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblSqulechTehtid.Location = new Point(1225, 498);
            lblSqulechTehtid.Name = "lblSqulechTehtid";
            lblSqulechTehtid.Size = new Size(20, 23);
            lblSqulechTehtid.TabIndex = 35;
            lblSqulechTehtid.Text = "0";
            lblSqulechTehtid.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // numYMax
            // 
            numYMax.Anchor = AnchorStyles.None;
            numYMax.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            numYMax.Location = new Point(1123, 526);
            numYMax.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numYMax.Minimum = new decimal(new int[] { 200, 0, 0, int.MinValue });
            numYMax.Name = "numYMax";
            numYMax.Size = new Size(70, 27);
            numYMax.TabIndex = 39;
            numYMax.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // numYMin
            // 
            numYMin.Anchor = AnchorStyles.None;
            numYMin.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            numYMin.Location = new Point(1123, 559);
            numYMin.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numYMin.Minimum = new decimal(new int[] { 200, 0, 0, int.MinValue });
            numYMin.Name = "numYMin";
            numYMin.Size = new Size(66, 27);
            numYMin.TabIndex = 40;
            numYMin.Value = new decimal(new int[] { 120, 0, 0, int.MinValue });
            // 
            // numGurultuEsigi
            // 
            numGurultuEsigi.Anchor = AnchorStyles.None;
            numGurultuEsigi.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            numGurultuEsigi.Location = new Point(1295, 619);
            numGurultuEsigi.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            numGurultuEsigi.Minimum = new decimal(new int[] { 60, 0, 0, int.MinValue });
            numGurultuEsigi.Name = "numGurultuEsigi";
            numGurultuEsigi.Size = new Size(60, 30);
            numGurultuEsigi.TabIndex = 41;
            numGurultuEsigi.Value = new decimal(new int[] { 30, 0, 0, int.MinValue });
            // 
            // chkGurultuEngelle
            // 
            chkGurultuEngelle.Anchor = AnchorStyles.None;
            chkGurultuEngelle.AutoSize = true;
            chkGurultuEngelle.BackColor = Color.White;
            chkGurultuEngelle.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            chkGurultuEngelle.ForeColor = SystemColors.ActiveCaptionText;
            chkGurultuEngelle.Location = new Point(1123, 622);
            chkGurultuEngelle.Name = "chkGurultuEngelle";
            chkGurultuEngelle.Size = new Size(166, 27);
            chkGurultuEngelle.TabIndex = 43;
            chkGurultuEngelle.Text = "Gürültü Eşiği (dB)";
            chkGurultuEngelle.UseVisualStyleBackColor = false;
            // 
            // numKirpmaYuzdesi
            // 
            numKirpmaYuzdesi.Anchor = AnchorStyles.None;
            numKirpmaYuzdesi.Location = new Point(1477, 384);
            numKirpmaYuzdesi.Maximum = new decimal(new int[] { 40, 0, 0, 0 });
            numKirpmaYuzdesi.Name = "numKirpmaYuzdesi";
            numKirpmaYuzdesi.Size = new Size(60, 27);
            numKirpmaYuzdesi.TabIndex = 44;
            numKirpmaYuzdesi.Value = new decimal(new int[] { 10, 0, 0, 0 });
            numKirpmaYuzdesi.ValueChanged += numKirpmaYuzdesi_ValueChanged;
            // 
            // label10
            // 
            label10.Anchor = AnchorStyles.None;
            label10.AutoSize = true;
            label10.BackColor = Color.White;
            label10.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            label10.ForeColor = SystemColors.ActiveCaptionText;
            label10.Location = new Point(1358, 387);
            label10.Name = "label10";
            label10.Size = new Size(106, 20);
            label10.TabIndex = 45;
            label10.Text = "Kenar Kırpma ";
            // 
            // lblKırpılmayanAlan
            // 
            lblKırpılmayanAlan.Anchor = AnchorStyles.None;
            lblKırpılmayanAlan.AutoSize = true;
            lblKırpılmayanAlan.BackColor = Color.White;
            lblKırpılmayanAlan.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 162);
            lblKırpılmayanAlan.ForeColor = SystemColors.ActiveCaptionText;
            lblKırpılmayanAlan.Location = new Point(1543, 383);
            lblKırpılmayanAlan.Name = "lblKırpılmayanAlan";
            lblKırpılmayanAlan.Size = new Size(50, 28);
            lblKırpılmayanAlan.TabIndex = 9;
            lblKırpılmayanAlan.Text = "%90";
            lblKırpılmayanAlan.Click += lblKırpılmayanAlan_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoSize = true;
            AutoValidate = AutoValidate.EnablePreventFocusChange;
            BackColor = Color.DarkGray;
            ClientSize = new Size(1920, 859);
            Controls.Add(lblKırpılmayanAlan);
            Controls.Add(label10);
            Controls.Add(numKirpmaYuzdesi);
            Controls.Add(chkGurultuEngelle);
            Controls.Add(numGurultuEsigi);
            Controls.Add(numYMin);
            Controls.Add(numYMax);
            Controls.Add(lblSqulechTehtid);
            Controls.Add(lblTaramaHiziDegeri);
            Controls.Add(lblYumusatmaDegeri);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(trbTaramaHizi);
            Controls.Add(chkAlarmAktif);
            Controls.Add(lblTehditDurumu);
            Controls.Add(trbSquelch);
            Controls.Add(cmbHedefProfilleri);
            Controls.Add(groupBox1);
            Controls.Add(picGrafik);
            Controls.Add(label6);
            Controls.Add(trbYumusatma);
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
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "ATEL Akıllı Jammer Karar Destek Arayüzü";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)numFrekans).EndInit();
            ((System.ComponentModel.ISupportInitialize)numOrnekleme).EndInit();
            ((System.ComponentModel.ISupportInitialize)numBantGenisligi).EndInit();
            grpDonanim.ResumeLayout(false);
            grpDonanim.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trbTxGain).EndInit();
            ((System.ComponentModel.ISupportInitialize)trbRxKazanci).EndInit();
            ((System.ComponentModel.ISupportInitialize)trbYumusatma).EndInit();
            ((System.ComponentModel.ISupportInitialize)picGrafik).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trbSquelch).EndInit();
            ((System.ComponentModel.ISupportInitialize)trbTaramaHizi).EndInit();
            ((System.ComponentModel.ISupportInitialize)numYMax).EndInit();
            ((System.ComponentModel.ISupportInitialize)numYMin).EndInit();
            ((System.ComponentModel.ISupportInitialize)numGurultuEsigi).EndInit();
            ((System.ComponentModel.ISupportInitialize)numKirpmaYuzdesi).EndInit();
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
        private TrackBar trbYumusatma;
        private Label label6;
        private PictureBox picGrafik;
        private GroupBox groupBox1;
        private RadioButton rdoTestModu;
        private RadioButton rdoTaarruzModu;
        private RadioButton rdoIzlemeModu;
        private Button btnDcKalibrasyon;
        private Label lblRxKazanciDegeri;
        private ComboBox cmbHedefProfilleri;
        private TrackBar trbSquelch;
        private Label lblTehditDurumu;
        private CheckBox chkAlarmAktif;
        private TrackBar trbTaramaHizi;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label lblYumusatmaDegeri;
        private Label lblTaramaHiziDegeri;
        private Label lblSqulechTehtid;
        private NumericUpDown numYMax;
        private NumericUpDown numYMin;
        private NumericUpDown numGurultuEsigi;
        private CheckBox chkGurultuEngelle;
        private NumericUpDown numKirpmaYuzdesi;
        private Label label10;
        private Label lblKırpılmayanAlan;
    }
}