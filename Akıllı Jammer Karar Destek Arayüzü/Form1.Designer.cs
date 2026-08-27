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
            numFrekans = new NumericUpDown();
            cmbBirim = new ComboBox();
            rtbKonsol = new RichTextBox();
            numOrnekleme = new NumericUpDown();
            numBantGenisligi = new NumericUpDown();
            cmbOrneklemeBirim = new ComboBox();
            cmbBantBirim = new ComboBox();
            grpDonanim = new GroupBox();
            lblTxKazanciDegeri = new Label();
            label10 = new Label();
            lblRxKazanciDegeri = new Label();
            btnDcKalibrasyon = new Button();
            chkBiasTee = new CheckBox();
            chkAGC = new CheckBox();
            label5 = new Label();
            label4 = new Label();
            trbTxGain = new TrackBar();
            trbRxKazanci = new TrackBar();
            chkGurultuEngelle = new CheckBox();
            numGurultuEsigi = new NumericUpDown();
            numKirpmaYuzdesi = new NumericUpDown();
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
            chkMarkerAktif = new CheckBox();
            pnlGiris = new Panel();
            btnMühendis = new Button();
            btnAnalist = new Button();
            btnTaktik = new Button();
            grpGelismisAyarlar = new GroupBox();
            label14 = new Label();
            label13 = new Label();
            label12 = new Label();
            button1 = new Button();
            label11 = new Label();
            btnModMuhendis = new Button();
            btnSaldırı = new Button();
            groupBox2 = new GroupBox();
            label16 = new Label();
            label18 = new Label();
            label19 = new Label();
            cmbTxBirim = new ComboBox();
            numTxBantGenisligi = new NumericUpDown();
            numTxFrekans = new NumericUpDown();
            cmbTxBantBirim = new ComboBox();
            label20 = new Label();
            ((System.ComponentModel.ISupportInitialize)numFrekans).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numOrnekleme).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numBantGenisligi).BeginInit();
            grpDonanim.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trbTxGain).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trbRxKazanci).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numGurultuEsigi).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numKirpmaYuzdesi).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trbYumusatma).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picGrafik).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trbSquelch).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trbTaramaHizi).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numYMax).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numYMin).BeginInit();
            grpGelismisAyarlar.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numTxBantGenisligi).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numTxFrekans).BeginInit();
            SuspendLayout();
            // 
            // btnBaglan
            // 
            btnBaglan.BackColor = Color.OliveDrab;
            btnBaglan.Cursor = Cursors.Hand;
            btnBaglan.FlatAppearance.BorderColor = Color.White;
            btnBaglan.Font = new Font("Segoe UI", 18F);
            btnBaglan.ForeColor = SystemColors.ButtonFace;
            btnBaglan.Location = new Point(18, 12);
            btnBaglan.Name = "btnBaglan";
            btnBaglan.Size = new Size(62, 55);
            btnBaglan.TabIndex = 0;
            btnBaglan.Text = "🔌";
            btnBaglan.UseVisualStyleBackColor = false;
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
            label1.Location = new Point(10, 33);
            label1.Name = "label1";
            label1.Size = new Size(62, 20);
            label1.TabIndex = 1;
            label1.Text = "Frekans ";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = SystemColors.ActiveCaptionText;
            label2.Location = new Point(10, 67);
            label2.Name = "label2";
            label2.Size = new Size(111, 20);
            label2.TabIndex = 3;
            label2.Text = "Örnekleme Hızı";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = SystemColors.ActiveCaptionText;
            label3.Location = new Point(10, 101);
            label3.Name = "label3";
            label3.Size = new Size(100, 20);
            label3.TabIndex = 4;
            label3.Text = "Bant Genişliği";
            // 
            // numFrekans
            // 
            numFrekans.Cursor = Cursors.Hand;
            numFrekans.DecimalPlaces = 3;
            numFrekans.Location = new Point(135, 31);
            numFrekans.Maximum = new decimal(new int[] { 1705032704, 1, 0, 0 });
            numFrekans.Name = "numFrekans";
            numFrekans.Size = new Size(100, 27);
            numFrekans.TabIndex = 9;
            numFrekans.ThousandsSeparator = true;
            numFrekans.Value = new decimal(new int[] { 2400, 0, 0, 0 });
            numFrekans.ValueChanged += numFrekans_ValueChanged;
            // 
            // cmbBirim
            // 
            cmbBirim.Cursor = Cursors.Hand;
            cmbBirim.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBirim.FormattingEnabled = true;
            cmbBirim.Items.AddRange(new object[] { "MHz", "GHz" });
            cmbBirim.Location = new Point(240, 31);
            cmbBirim.Name = "cmbBirim";
            cmbBirim.Size = new Size(72, 28);
            cmbBirim.TabIndex = 10;
            cmbBirim.SelectedIndexChanged += cmbBirim_SelectedIndexChanged;
            // 
            // rtbKonsol
            // 
            rtbKonsol.BorderStyle = BorderStyle.None;
            rtbKonsol.Location = new Point(18, 705);
            rtbKonsol.Name = "rtbKonsol";
            rtbKonsol.Size = new Size(1010, 135);
            rtbKonsol.TabIndex = 11;
            rtbKonsol.Text = "";
            // 
            // numOrnekleme
            // 
            numOrnekleme.Cursor = Cursors.Hand;
            numOrnekleme.DecimalPlaces = 3;
            numOrnekleme.Location = new Point(135, 66);
            numOrnekleme.Maximum = new decimal(new int[] { 61440000, 0, 0, 0 });
            numOrnekleme.Name = "numOrnekleme";
            numOrnekleme.Size = new Size(100, 27);
            numOrnekleme.TabIndex = 15;
            numOrnekleme.ThousandsSeparator = true;
            numOrnekleme.Value = new decimal(new int[] { 24000, 0, 0, 0 });
            // 
            // numBantGenisligi
            // 
            numBantGenisligi.Cursor = Cursors.Hand;
            numBantGenisligi.DecimalPlaces = 3;
            numBantGenisligi.Location = new Point(135, 99);
            numBantGenisligi.Maximum = new decimal(new int[] { 56000000, 0, 0, 0 });
            numBantGenisligi.Name = "numBantGenisligi";
            numBantGenisligi.Size = new Size(100, 27);
            numBantGenisligi.TabIndex = 16;
            numBantGenisligi.ThousandsSeparator = true;
            numBantGenisligi.Value = new decimal(new int[] { 20000, 0, 0, 0 });
            // 
            // cmbOrneklemeBirim
            // 
            cmbOrneklemeBirim.Cursor = Cursors.Hand;
            cmbOrneklemeBirim.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOrneklemeBirim.FormattingEnabled = true;
            cmbOrneklemeBirim.Items.AddRange(new object[] { "kSps ", "MSps " });
            cmbOrneklemeBirim.Location = new Point(240, 66);
            cmbOrneklemeBirim.Name = "cmbOrneklemeBirim";
            cmbOrneklemeBirim.Size = new Size(72, 28);
            cmbOrneklemeBirim.TabIndex = 17;
            cmbOrneklemeBirim.SelectedIndexChanged += cmbOrneklemeBirim_SelectedIndexChanged;
            // 
            // cmbBantBirim
            // 
            cmbBantBirim.Cursor = Cursors.Hand;
            cmbBantBirim.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBantBirim.FormattingEnabled = true;
            cmbBantBirim.Items.AddRange(new object[] { "kHz", "MHz" });
            cmbBantBirim.Location = new Point(240, 99);
            cmbBantBirim.Name = "cmbBantBirim";
            cmbBantBirim.RightToLeft = RightToLeft.No;
            cmbBantBirim.Size = new Size(73, 28);
            cmbBantBirim.TabIndex = 18;
            cmbBantBirim.Tag = "";
            cmbBantBirim.SelectedIndexChanged += cmbBantBirim_SelectedIndexChanged;
            // 
            // grpDonanim
            // 
            grpDonanim.Controls.Add(lblTxKazanciDegeri);
            grpDonanim.Controls.Add(label10);
            grpDonanim.Controls.Add(lblRxKazanciDegeri);
            grpDonanim.Controls.Add(btnDcKalibrasyon);
            grpDonanim.Controls.Add(chkBiasTee);
            grpDonanim.Controls.Add(chkAGC);
            grpDonanim.Controls.Add(label5);
            grpDonanim.Controls.Add(label4);
            grpDonanim.Controls.Add(trbTxGain);
            grpDonanim.Controls.Add(trbRxKazanci);
            grpDonanim.Controls.Add(chkGurultuEngelle);
            grpDonanim.Controls.Add(numGurultuEsigi);
            grpDonanim.Controls.Add(numKirpmaYuzdesi);
            grpDonanim.Cursor = Cursors.Hand;
            grpDonanim.Font = new Font("Segoe UI", 9F);
            grpDonanim.ForeColor = SystemColors.ActiveCaptionText;
            grpDonanim.Location = new Point(1040, 288);
            grpDonanim.Name = "grpDonanim";
            grpDonanim.Size = new Size(329, 330);
            grpDonanim.TabIndex = 19;
            grpDonanim.TabStop = false;
            grpDonanim.Text = "Gelişmiş RF Kontrolleri";
            grpDonanim.Enter += grpDonanim_Enter;
            // 
            // lblTxKazanciDegeri
            // 
            lblTxKazanciDegeri.AutoSize = true;
            lblTxKazanciDegeri.BackColor = Color.Transparent;
            lblTxKazanciDegeri.Location = new Point(230, 109);
            lblTxKazanciDegeri.Name = "lblTxKazanciDegeri";
            lblTxKazanciDegeri.Size = new Size(39, 20);
            lblTxKazanciDegeri.TabIndex = 46;
            lblTxKazanciDegeri.Text = "0 dB";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.ForeColor = SystemColors.ActiveCaptionText;
            label10.Location = new Point(191, 257);
            label10.Name = "label10";
            label10.Size = new Size(12, 20);
            label10.TabIndex = 22;
            label10.Text = ":";
            // 
            // lblRxKazanciDegeri
            // 
            lblRxKazanciDegeri.AutoSize = true;
            lblRxKazanciDegeri.BackColor = Color.Transparent;
            lblRxKazanciDegeri.Location = new Point(230, 48);
            lblRxKazanciDegeri.Name = "lblRxKazanciDegeri";
            lblRxKazanciDegeri.Size = new Size(39, 20);
            lblRxKazanciDegeri.TabIndex = 8;
            lblRxKazanciDegeri.Text = "0 dB";
            lblRxKazanciDegeri.Click += lblRxKazanciDegeri_Click;
            // 
            // btnDcKalibrasyon
            // 
            btnDcKalibrasyon.BackColor = Color.White;
            btnDcKalibrasyon.Cursor = Cursors.Hand;
            btnDcKalibrasyon.Location = new Point(21, 217);
            btnDcKalibrasyon.Name = "btnDcKalibrasyon";
            btnDcKalibrasyon.Size = new Size(245, 30);
            btnDcKalibrasyon.TabIndex = 7;
            btnDcKalibrasyon.Text = "DC Offset Kalibrasyonu";
            btnDcKalibrasyon.UseVisualStyleBackColor = false;
            btnDcKalibrasyon.Click += btnDcKalibrasyon_Click;
            // 
            // chkBiasTee
            // 
            chkBiasTee.AutoSize = true;
            chkBiasTee.Cursor = Cursors.Hand;
            chkBiasTee.Font = new Font("Segoe UI", 9F);
            chkBiasTee.Location = new Point(21, 187);
            chkBiasTee.Name = "chkBiasTee";
            chkBiasTee.Size = new Size(196, 24);
            chkBiasTee.TabIndex = 5;
            chkBiasTee.Text = "Bias-Tee (Aktif Anten 5V)";
            chkBiasTee.UseVisualStyleBackColor = true;
            chkBiasTee.CheckedChanged += chkBiasTee_CheckedChanged;
            // 
            // chkAGC
            // 
            chkAGC.AutoSize = true;
            chkAGC.Cursor = Cursors.Hand;
            chkAGC.Font = new Font("Segoe UI", 9F);
            chkAGC.Location = new Point(21, 152);
            chkAGC.Name = "chkAGC";
            chkAGC.Size = new Size(248, 24);
            chkAGC.TabIndex = 4;
            chkAGC.Text = "AGC (Otomatik Kazanç Kontrolü)";
            chkAGC.UseVisualStyleBackColor = true;
            chkAGC.CheckedChanged += chkAGC_CheckedChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F);
            label5.Location = new Point(21, 88);
            label5.Name = "label5";
            label5.Size = new Size(213, 20);
            label5.TabIndex = 3;
            label5.Text = "TX Kazancı (Taarruz Çıkış Gücü)";
            label5.Click += label5_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F);
            label4.Location = new Point(21, 25);
            label4.Name = "label4";
            label4.Size = new Size(152, 20);
            label4.TabIndex = 2;
            label4.Text = "RX Kazancı (Dinleme)";
            label4.Click += label4_Click;
            // 
            // trbTxGain
            // 
            trbTxGain.Location = new Point(21, 109);
            trbTxGain.Maximum = 60;
            trbTxGain.Name = "trbTxGain";
            trbTxGain.Size = new Size(213, 56);
            trbTxGain.TabIndex = 1;
            trbTxGain.TickFrequency = 5;
            trbTxGain.Scroll += trbTxGain_Scroll;
            // 
            // trbRxKazanci
            // 
            trbRxKazanci.Location = new Point(21, 46);
            trbRxKazanci.Maximum = 60;
            trbRxKazanci.Name = "trbRxKazanci";
            trbRxKazanci.Size = new Size(213, 56);
            trbRxKazanci.TabIndex = 0;
            trbRxKazanci.TickFrequency = 5;
            trbRxKazanci.Scroll += trbRxKazanci_Scroll;
            trbRxKazanci.MouseUp += trbRxKazanci_MouseUp;
            // 
            // chkGurultuEngelle
            // 
            chkGurultuEngelle.AutoSize = true;
            chkGurultuEngelle.Cursor = Cursors.Hand;
            chkGurultuEngelle.Location = new Point(21, 255);
            chkGurultuEngelle.MaximumSize = new Size(186, 50);
            chkGurultuEngelle.Name = "chkGurultuEngelle";
            chkGurultuEngelle.Size = new Size(146, 24);
            chkGurultuEngelle.TabIndex = 43;
            chkGurultuEngelle.Text = "Gürültü Eşiği (dB)";
            chkGurultuEngelle.UseVisualStyleBackColor = true;
            chkGurultuEngelle.CheckedChanged += chkGurultuEngelle_CheckedChanged;
            // 
            // numGurultuEsigi
            // 
            numGurultuEsigi.Cursor = Cursors.Hand;
            numGurultuEsigi.Font = new Font("Segoe UI", 9F);
            numGurultuEsigi.Location = new Point(206, 255);
            numGurultuEsigi.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            numGurultuEsigi.Minimum = new decimal(new int[] { 60, 0, 0, int.MinValue });
            numGurultuEsigi.Name = "numGurultuEsigi";
            numGurultuEsigi.Size = new Size(60, 27);
            numGurultuEsigi.TabIndex = 41;
            numGurultuEsigi.Value = new decimal(new int[] { 30, 0, 0, int.MinValue });
            // 
            // numKirpmaYuzdesi
            // 
            numKirpmaYuzdesi.Font = new Font("Segoe UI", 9F);
            numKirpmaYuzdesi.InterceptArrowKeys = false;
            numKirpmaYuzdesi.Location = new Point(230, 255);
            numKirpmaYuzdesi.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            numKirpmaYuzdesi.Minimum = new decimal(new int[] { 30, 0, 0, 0 });
            numKirpmaYuzdesi.Name = "numKirpmaYuzdesi";
            numKirpmaYuzdesi.Size = new Size(10, 27);
            numKirpmaYuzdesi.TabIndex = 44;
            numKirpmaYuzdesi.Value = new decimal(new int[] { 30, 0, 0, 0 });
            numKirpmaYuzdesi.Visible = false;
            numKirpmaYuzdesi.ValueChanged += numKirpmaYuzdesi_ValueChanged;
            // 
            // trbYumusatma
            // 
            trbYumusatma.AutoSize = false;
            trbYumusatma.BackColor = Color.Black;
            trbYumusatma.Cursor = Cursors.Hand;
            trbYumusatma.Location = new Point(18, 564);
            trbYumusatma.Maximum = 100;
            trbYumusatma.Minimum = 1;
            trbYumusatma.Name = "trbYumusatma";
            trbYumusatma.Size = new Size(394, 30);
            trbYumusatma.TabIndex = 20;
            trbYumusatma.TickFrequency = 1000;
            trbYumusatma.Value = 1;
            trbYumusatma.Scroll += trbYumusatma_Scroll;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Black;
            label6.Font = new Font("Segoe UI", 9F);
            label6.ForeColor = SystemColors.Control;
            label6.Location = new Point(42, 588);
            label6.Name = "label6";
            label6.Size = new Size(219, 20);
            label6.TabIndex = 21;
            label6.Text = "Sinyal Yumuşatma (Video Filter)";
            label6.Click += label6_Click;
            // 
            // picGrafik
            // 
            picGrafik.BackColor = Color.Black;
            picGrafik.Location = new Point(18, 73);
            picGrafik.Name = "picGrafik";
            picGrafik.Size = new Size(1010, 494);
            picGrafik.TabIndex = 22;
            picGrafik.TabStop = false;
            picGrafik.Click += picGrafik_Click;
            picGrafik.Paint += picGrafik_Paint;
            picGrafik.MouseDown += picGrafik_MouseDown;
            picGrafik.MouseMove += picGrafik_MouseMove;
            picGrafik.MouseUp += picGrafik_MouseUp;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.Transparent;
            groupBox1.BackgroundImageLayout = ImageLayout.None;
            groupBox1.Controls.Add(rdoTestModu);
            groupBox1.Controls.Add(rdoTaarruzModu);
            groupBox1.Controls.Add(rdoIzlemeModu);
            groupBox1.Location = new Point(1040, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(329, 119);
            groupBox1.TabIndex = 23;
            groupBox1.TabStop = false;
            groupBox1.Text = "Operasyon Modu";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // rdoTestModu
            // 
            rdoTestModu.AutoSize = true;
            rdoTestModu.BackColor = Color.Transparent;
            rdoTestModu.Cursor = Cursors.Hand;
            rdoTestModu.ForeColor = SystemColors.ActiveCaptionText;
            rdoTestModu.Location = new Point(10, 86);
            rdoTestModu.Name = "rdoTestModu";
            rdoTestModu.Size = new Size(178, 24);
            rdoTestModu.TabIndex = 2;
            rdoTestModu.Text = "Dahili Test (Loopback)";
            rdoTestModu.UseVisualStyleBackColor = false;
            // 
            // rdoTaarruzModu
            // 
            rdoTaarruzModu.AutoSize = true;
            rdoTaarruzModu.BackColor = Color.Transparent;
            rdoTaarruzModu.Cursor = Cursors.Hand;
            rdoTaarruzModu.ForeColor = SystemColors.ActiveCaptionText;
            rdoTaarruzModu.Location = new Point(10, 56);
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
            rdoIzlemeModu.BackColor = Color.Transparent;
            rdoIzlemeModu.Checked = true;
            rdoIzlemeModu.Cursor = Cursors.Hand;
            rdoIzlemeModu.ForeColor = SystemColors.ActiveCaptionText;
            rdoIzlemeModu.Location = new Point(10, 26);
            rdoIzlemeModu.Name = "rdoIzlemeModu";
            rdoIzlemeModu.Size = new Size(201, 24);
            rdoIzlemeModu.TabIndex = 0;
            rdoIzlemeModu.TabStop = true;
            rdoIzlemeModu.Text = "İzleme Modu (RX Sadece)";
            rdoIzlemeModu.UseVisualStyleBackColor = false;
            // 
            // cmbHedefProfilleri
            // 
            cmbHedefProfilleri.Cursor = Cursors.Hand;
            cmbHedefProfilleri.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbHedefProfilleri.FormattingEnabled = true;
            cmbHedefProfilleri.Items.AddRange(new object[] { "DJI Drone / Wi-Fi (2.4 GHz)", "Taktik Telsiz (UHF 433 MHz)", "GSM / LTE Telefon (1.75 GHz)" });
            cmbHedefProfilleri.Location = new Point(783, 629);
            cmbHedefProfilleri.Name = "cmbHedefProfilleri";
            cmbHedefProfilleri.Size = new Size(245, 28);
            cmbHedefProfilleri.TabIndex = 25;
            cmbHedefProfilleri.SelectedIndexChanged += cmbHedefProfilleri_SelectedIndexChanged;
            // 
            // trbSquelch
            // 
            trbSquelch.AutoSize = false;
            trbSquelch.BackColor = Color.DarkGray;
            trbSquelch.Cursor = Cursors.Hand;
            trbSquelch.Location = new Point(83, 667);
            trbSquelch.Maximum = 100;
            trbSquelch.Minimum = -100;
            trbSquelch.Name = "trbSquelch";
            trbSquelch.Size = new Size(329, 30);
            trbSquelch.TabIndex = 9;
            trbSquelch.TickStyle = TickStyle.None;
            trbSquelch.Scroll += trbSquelch_Scroll;
            // 
            // lblTehditDurumu
            // 
            lblTehditDurumu.BackColor = Color.DarkGreen;
            lblTehditDurumu.BorderStyle = BorderStyle.FixedSingle;
            lblTehditDurumu.Font = new Font("Segoe UI Black", 14F, FontStyle.Bold);
            lblTehditDurumu.ForeColor = SystemColors.ButtonFace;
            lblTehditDurumu.Location = new Point(154, 12);
            lblTehditDurumu.Name = "lblTehditDurumu";
            lblTehditDurumu.Size = new Size(744, 55);
            lblTehditDurumu.TabIndex = 26;
            lblTehditDurumu.Text = "TEMİZ - DİNLENİYOR...";
            lblTehditDurumu.TextAlign = ContentAlignment.MiddleLeft;
            lblTehditDurumu.Click += lblTehditDurumu_Click;
            // 
            // chkAlarmAktif
            // 
            chkAlarmAktif.AutoSize = true;
            chkAlarmAktif.BackColor = Color.Transparent;
            chkAlarmAktif.Checked = true;
            chkAlarmAktif.CheckState = CheckState.Checked;
            chkAlarmAktif.Cursor = Cursors.Hand;
            chkAlarmAktif.Font = new Font("Segoe UI", 9F);
            chkAlarmAktif.ForeColor = SystemColors.ActiveCaptionText;
            chkAlarmAktif.Location = new Point(18, 629);
            chkAlarmAktif.Name = "chkAlarmAktif";
            chkAlarmAktif.Size = new Size(198, 24);
            chkAlarmAktif.TabIndex = 9;
            chkAlarmAktif.Text = "Sinyal Tehdit Alarmı Aktif";
            chkAlarmAktif.UseVisualStyleBackColor = false;
            chkAlarmAktif.CheckedChanged += chkAlarmAktif_CheckedChanged;
            // 
            // trbTaramaHizi
            // 
            trbTaramaHizi.AutoSize = false;
            trbTaramaHizi.BackColor = Color.Black;
            trbTaramaHizi.Cursor = Cursors.Hand;
            trbTaramaHizi.Location = new Point(418, 564);
            trbTaramaHizi.Maximum = 200;
            trbTaramaHizi.Minimum = 10;
            trbTaramaHizi.Name = "trbTaramaHizi";
            trbTaramaHizi.Size = new Size(394, 30);
            trbTaramaHizi.TabIndex = 29;
            trbTaramaHizi.TickFrequency = 1000;
            trbTaramaHizi.Value = 10;
            trbTaramaHizi.Scroll += trbTaramaHizi_Scroll;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Black;
            label7.Font = new Font("Segoe UI", 9F);
            label7.ForeColor = SystemColors.ButtonFace;
            label7.Location = new Point(442, 588);
            label7.Name = "label7";
            label7.Size = new Size(191, 20);
            label7.TabIndex = 30;
            label7.Text = "Tarama Hızı (Gecikme - ms)";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.Transparent;
            label8.Font = new Font("Segoe UI", 9F);
            label8.ForeColor = Color.Black;
            label8.Location = new Point(659, 632);
            label8.Name = "label8";
            label8.Size = new Size(49, 20);
            label8.TabIndex = 31;
            label8.Text = "Max Y";
            label8.Click += label8_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.Transparent;
            label9.Font = new Font("Segoe UI", 9F);
            label9.ForeColor = Color.Black;
            label9.Location = new Point(659, 670);
            label9.Name = "label9";
            label9.Size = new Size(46, 20);
            label9.TabIndex = 32;
            label9.Text = "Min Y";
            // 
            // lblYumusatmaDegeri
            // 
            lblYumusatmaDegeri.AutoSize = true;
            lblYumusatmaDegeri.BackColor = Color.ForestGreen;
            lblYumusatmaDegeri.Font = new Font("Segoe UI", 9F);
            lblYumusatmaDegeri.Location = new Point(357, 588);
            lblYumusatmaDegeri.Name = "lblYumusatmaDegeri";
            lblYumusatmaDegeri.RightToLeft = RightToLeft.Yes;
            lblYumusatmaDegeri.Size = new Size(29, 20);
            lblYumusatmaDegeri.TabIndex = 33;
            lblYumusatmaDegeri.Text = "%1";
            lblYumusatmaDegeri.Click += lblYumusatmaDegeri_Click;
            // 
            // lblTaramaHiziDegeri
            // 
            lblTaramaHiziDegeri.AutoSize = true;
            lblTaramaHiziDegeri.BackColor = Color.ForestGreen;
            lblTaramaHiziDegeri.Font = new Font("Segoe UI", 9F);
            lblTaramaHiziDegeri.Location = new Point(740, 588);
            lblTaramaHiziDegeri.Name = "lblTaramaHiziDegeri";
            lblTaramaHiziDegeri.Size = new Size(48, 20);
            lblTaramaHiziDegeri.TabIndex = 34;
            lblTaramaHiziDegeri.Text = "10 ms";
            lblTaramaHiziDegeri.Click += lblTaramaHiziDegeri_Click;
            // 
            // lblSqulechTehtid
            // 
            lblSqulechTehtid.BackColor = Color.ForestGreen;
            lblSqulechTehtid.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 162);
            lblSqulechTehtid.Location = new Point(18, 667);
            lblSqulechTehtid.Name = "lblSqulechTehtid";
            lblSqulechTehtid.Size = new Size(62, 30);
            lblSqulechTehtid.TabIndex = 35;
            lblSqulechTehtid.Text = "0";
            lblSqulechTehtid.TextAlign = ContentAlignment.MiddleCenter;
            lblSqulechTehtid.Click += lblSqulechTehtid_Click;
            // 
            // numYMax
            // 
            numYMax.Cursor = Cursors.Hand;
            numYMax.Font = new Font("Segoe UI", 9F);
            numYMax.Location = new Point(711, 629);
            numYMax.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numYMax.Minimum = new decimal(new int[] { 200, 0, 0, int.MinValue });
            numYMax.Name = "numYMax";
            numYMax.Size = new Size(66, 27);
            numYMax.TabIndex = 39;
            numYMax.Value = new decimal(new int[] { 200, 0, 0, 0 });
            // 
            // numYMin
            // 
            numYMin.Cursor = Cursors.Hand;
            numYMin.Font = new Font("Segoe UI", 9F);
            numYMin.Location = new Point(711, 667);
            numYMin.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numYMin.Minimum = new decimal(new int[] { 200, 0, 0, int.MinValue });
            numYMin.Name = "numYMin";
            numYMin.Size = new Size(66, 27);
            numYMin.TabIndex = 40;
            numYMin.Value = new decimal(new int[] { 200, 0, 0, int.MinValue });
            // 
            // chkMarkerAktif
            // 
            chkMarkerAktif.AutoSize = true;
            chkMarkerAktif.BackColor = Color.Black;
            chkMarkerAktif.CheckAlign = ContentAlignment.MiddleRight;
            chkMarkerAktif.Cursor = Cursors.Hand;
            chkMarkerAktif.Font = new Font("Segoe UI", 9F);
            chkMarkerAktif.ForeColor = SystemColors.ButtonFace;
            chkMarkerAktif.Location = new Point(896, 581);
            chkMarkerAktif.Name = "chkMarkerAktif";
            chkMarkerAktif.Size = new Size(112, 24);
            chkMarkerAktif.TabIndex = 46;
            chkMarkerAktif.Text = "Marker Aktif";
            chkMarkerAktif.UseVisualStyleBackColor = false;
            chkMarkerAktif.CheckedChanged += chkMarkerAktif_CheckedChanged;
            // 
            // pnlGiris
            // 
            pnlGiris.Location = new Point(0, 0);
            pnlGiris.Name = "pnlGiris";
            pnlGiris.Size = new Size(200, 100);
            pnlGiris.TabIndex = 0;
            // 
            // btnMühendis
            // 
            btnMühendis.Location = new Point(0, 0);
            btnMühendis.Name = "btnMühendis";
            btnMühendis.Size = new Size(75, 23);
            btnMühendis.TabIndex = 0;
            // 
            // btnAnalist
            // 
            btnAnalist.Location = new Point(0, 0);
            btnAnalist.Name = "btnAnalist";
            btnAnalist.Size = new Size(75, 23);
            btnAnalist.TabIndex = 0;
            // 
            // btnTaktik
            // 
            btnTaktik.Location = new Point(0, 0);
            btnTaktik.Name = "btnTaktik";
            btnTaktik.Size = new Size(75, 23);
            btnTaktik.TabIndex = 0;
            // 
            // grpGelismisAyarlar
            // 
            grpGelismisAyarlar.Controls.Add(label14);
            grpGelismisAyarlar.Controls.Add(label13);
            grpGelismisAyarlar.Controls.Add(label12);
            grpGelismisAyarlar.Controls.Add(numOrnekleme);
            grpGelismisAyarlar.Controls.Add(cmbOrneklemeBirim);
            grpGelismisAyarlar.Controls.Add(label1);
            grpGelismisAyarlar.Controls.Add(cmbBirim);
            grpGelismisAyarlar.Controls.Add(numBantGenisligi);
            grpGelismisAyarlar.Controls.Add(numFrekans);
            grpGelismisAyarlar.Controls.Add(cmbBantBirim);
            grpGelismisAyarlar.Controls.Add(label3);
            grpGelismisAyarlar.Controls.Add(label2);
            grpGelismisAyarlar.Location = new Point(1040, 137);
            grpGelismisAyarlar.Name = "grpGelismisAyarlar";
            grpGelismisAyarlar.Size = new Size(329, 145);
            grpGelismisAyarlar.TabIndex = 0;
            grpGelismisAyarlar.TabStop = false;
            grpGelismisAyarlar.Text = "RX (Dinleme) RF Parametreleri";
            grpGelismisAyarlar.Enter += grpGelismisAyarlar_Enter;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.ForeColor = SystemColors.ActiveCaptionText;
            label14.Location = new Point(120, 101);
            label14.Name = "label14";
            label14.Size = new Size(12, 20);
            label14.TabIndex = 21;
            label14.Text = ":";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.ForeColor = SystemColors.ActiveCaptionText;
            label13.Location = new Point(120, 67);
            label13.Name = "label13";
            label13.Size = new Size(12, 20);
            label13.TabIndex = 20;
            label13.Text = ":";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.ForeColor = SystemColors.ActiveCaptionText;
            label12.Location = new Point(120, 33);
            label12.Name = "label12";
            label12.Size = new Size(12, 20);
            label12.TabIndex = 19;
            label12.Text = ":";
            // 
            // button1
            // 
            button1.BackColor = Color.SteelBlue;
            button1.BackgroundImageLayout = ImageLayout.None;
            button1.Cursor = Cursors.Hand;
            button1.Font = new Font("Segoe UI", 18F);
            button1.ForeColor = SystemColors.ButtonFace;
            button1.Location = new Point(86, 12);
            button1.Name = "button1";
            button1.Size = new Size(62, 56);
            button1.TabIndex = 41;
            button1.Text = "📄";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // label11
            // 
            label11.BackColor = Color.Black;
            label11.Location = new Point(18, 564);
            label11.Name = "label11";
            label11.Size = new Size(1010, 54);
            label11.TabIndex = 48;
            label11.Text = "                             \r\n                                      \r\n                                           \r\n";
            label11.Click += label11_Click;
            // 
            // btnModMuhendis
            // 
            btnModMuhendis.BackColor = Color.White;
            btnModMuhendis.Cursor = Cursors.Hand;
            btnModMuhendis.ForeColor = SystemColors.ActiveCaptionText;
            btnModMuhendis.Location = new Point(783, 667);
            btnModMuhendis.Name = "btnModMuhendis";
            btnModMuhendis.Size = new Size(245, 27);
            btnModMuhendis.TabIndex = 51;
            btnModMuhendis.Text = "İLERİ MOD AKTİF -->";
            btnModMuhendis.UseVisualStyleBackColor = false;
            btnModMuhendis.Click += btnModMuhendis_Click_1;
            // 
            // btnSaldırı
            // 
            btnSaldırı.BackColor = Color.FromArgb(255, 128, 0);
            btnSaldırı.Cursor = Cursors.Hand;
            btnSaldırı.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 162);
            btnSaldırı.ForeColor = SystemColors.ActiveCaptionText;
            btnSaldırı.Location = new Point(904, 12);
            btnSaldırı.Name = "btnSaldırı";
            btnSaldırı.Size = new Size(124, 56);
            btnSaldırı.TabIndex = 52;
            btnSaldırı.Text = "Saldırı Başlat";
            btnSaldırı.UseVisualStyleBackColor = false;
            btnSaldırı.Click += btnSaldırı_Click_1;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label16);
            groupBox2.Controls.Add(label18);
            groupBox2.Controls.Add(label19);
            groupBox2.Controls.Add(cmbTxBirim);
            groupBox2.Controls.Add(numTxBantGenisligi);
            groupBox2.Controls.Add(numTxFrekans);
            groupBox2.Controls.Add(cmbTxBantBirim);
            groupBox2.Controls.Add(label20);
            groupBox2.Location = new Point(1040, 629);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(329, 104);
            groupBox2.TabIndex = 53;
            groupBox2.TabStop = false;
            groupBox2.Text = "TX (Saldırı) RF Parametreleri";
            groupBox2.Enter += groupBox2_Enter;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.ForeColor = SystemColors.ActiveCaptionText;
            label16.Location = new Point(120, 67);
            label16.Name = "label16";
            label16.Size = new Size(12, 20);
            label16.TabIndex = 21;
            label16.Text = ":";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.ForeColor = SystemColors.ActiveCaptionText;
            label18.Location = new Point(120, 33);
            label18.Name = "label18";
            label18.Size = new Size(12, 20);
            label18.TabIndex = 19;
            label18.Text = ":";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.ForeColor = SystemColors.ActiveCaptionText;
            label19.Location = new Point(10, 33);
            label19.Name = "label19";
            label19.Size = new Size(62, 20);
            label19.TabIndex = 1;
            label19.Text = "Frekans ";
            // 
            // cmbTxBirim
            // 
            cmbTxBirim.Cursor = Cursors.Hand;
            cmbTxBirim.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTxBirim.FormattingEnabled = true;
            cmbTxBirim.Items.AddRange(new object[] { "MHz", "GHz" });
            cmbTxBirim.Location = new Point(240, 31);
            cmbTxBirim.Name = "cmbTxBirim";
            cmbTxBirim.Size = new Size(72, 28);
            cmbTxBirim.TabIndex = 10;
            // 
            // numTxBantGenisligi
            // 
            numTxBantGenisligi.Cursor = Cursors.Hand;
            numTxBantGenisligi.DecimalPlaces = 3;
            numTxBantGenisligi.Location = new Point(135, 65);
            numTxBantGenisligi.Maximum = new decimal(new int[] { 56000000, 0, 0, 0 });
            numTxBantGenisligi.Name = "numTxBantGenisligi";
            numTxBantGenisligi.Size = new Size(100, 27);
            numTxBantGenisligi.TabIndex = 16;
            numTxBantGenisligi.ThousandsSeparator = true;
            numTxBantGenisligi.Value = new decimal(new int[] { 20000, 0, 0, 0 });
            // 
            // numTxFrekans
            // 
            numTxFrekans.Cursor = Cursors.Hand;
            numTxFrekans.DecimalPlaces = 3;
            numTxFrekans.Location = new Point(135, 31);
            numTxFrekans.Maximum = new decimal(new int[] { 1705032704, 1, 0, 0 });
            numTxFrekans.Name = "numTxFrekans";
            numTxFrekans.Size = new Size(100, 27);
            numTxFrekans.TabIndex = 9;
            numTxFrekans.ThousandsSeparator = true;
            numTxFrekans.Value = new decimal(new int[] { 2400, 0, 0, 0 });
            // 
            // cmbTxBantBirim
            // 
            cmbTxBantBirim.Cursor = Cursors.Hand;
            cmbTxBantBirim.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTxBantBirim.FormattingEnabled = true;
            cmbTxBantBirim.Items.AddRange(new object[] { "kHz", "MHz" });
            cmbTxBantBirim.Location = new Point(240, 65);
            cmbTxBantBirim.Name = "cmbTxBantBirim";
            cmbTxBantBirim.RightToLeft = RightToLeft.No;
            cmbTxBantBirim.Size = new Size(73, 28);
            cmbTxBantBirim.TabIndex = 18;
            cmbTxBantBirim.Tag = "";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.ForeColor = SystemColors.ActiveCaptionText;
            label20.Location = new Point(10, 67);
            label20.Name = "label20";
            label20.Size = new Size(100, 20);
            label20.TabIndex = 4;
            label20.Text = "Bant Genişliği";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AutoValidate = AutoValidate.EnablePreventFocusChange;
            BackColor = Color.DarkGray;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(1382, 852);
            Controls.Add(groupBox2);
            Controls.Add(button1);
            Controls.Add(btnSaldırı);
            Controls.Add(btnModMuhendis);
            Controls.Add(grpGelismisAyarlar);
            Controls.Add(chkMarkerAktif);
            Controls.Add(label7);
            Controls.Add(lblTaramaHiziDegeri);
            Controls.Add(cmbHedefProfilleri);
            Controls.Add(trbTaramaHizi);
            Controls.Add(numYMin);
            Controls.Add(numYMax);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(lblYumusatmaDegeri);
            Controls.Add(groupBox1);
            Controls.Add(chkAlarmAktif);
            Controls.Add(label6);
            Controls.Add(trbYumusatma);
            Controls.Add(trbSquelch);
            Controls.Add(btnBaglan);
            Controls.Add(lblTehditDurumu);
            Controls.Add(grpDonanim);
            Controls.Add(rtbKonsol);
            Controls.Add(label11);
            Controls.Add(lblSqulechTehtid);
            Controls.Add(picGrafik);
            ForeColor = SystemColors.ButtonHighlight;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "ATEL Akıllı Jammer Karar Destek Arayüzü";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)numFrekans).EndInit();
            ((System.ComponentModel.ISupportInitialize)numOrnekleme).EndInit();
            ((System.ComponentModel.ISupportInitialize)numBantGenisligi).EndInit();
            grpDonanim.ResumeLayout(false);
            grpDonanim.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trbTxGain).EndInit();
            ((System.ComponentModel.ISupportInitialize)trbRxKazanci).EndInit();
            ((System.ComponentModel.ISupportInitialize)numGurultuEsigi).EndInit();
            ((System.ComponentModel.ISupportInitialize)numKirpmaYuzdesi).EndInit();
            ((System.ComponentModel.ISupportInitialize)trbYumusatma).EndInit();
            ((System.ComponentModel.ISupportInitialize)picGrafik).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trbSquelch).EndInit();
            ((System.ComponentModel.ISupportInitialize)trbTaramaHizi).EndInit();
            ((System.ComponentModel.ISupportInitialize)numYMax).EndInit();
            ((System.ComponentModel.ISupportInitialize)numYMin).EndInit();
            grpGelismisAyarlar.ResumeLayout(false);
            grpGelismisAyarlar.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numTxBantGenisligi).EndInit();
            ((System.ComponentModel.ISupportInitialize)numTxFrekans).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnBaglan;
        private Button button2;
        private Label label1;
        private Label label2;
        private Label label3;
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
        private CheckBox chkMarkerAktif;
        private Panel pnlGiris;
        private Button btnMühendis;
        private Button btnAnalist;
        private Button btnTaktik;
        private GroupBox grpGelismisAyarlar;
        private Label label11;
        private Button btnModMuhendis;
        private Button btnSaldırı;
        private Button button1;
        private Label label14;
        private Label label13;
        private Label label12;
        private NumericUpDown numKirpmaYuzdesi;
        private Label label10;
        private Label lblTxKazanciDegeri;
        private GroupBox groupBox2;
        private Label label16;
        private Label label18;
        private Label label19;
        private ComboBox cmbTxBirim;
        private NumericUpDown numTxBantGenisligi;
        private NumericUpDown numTxFrekans;
        private ComboBox cmbTxBantBirim;
        private Label label20;
    }
}