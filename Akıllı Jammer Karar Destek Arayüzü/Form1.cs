using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;
using System;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

namespace Akıllı_Jammer_Karar_Destek_Arayüzü
{
    public partial class Form1 : Form
    {
        private IntPtr _devicePointer = IntPtr.Zero;
        private bool _isDeviceOpen = false;
        private bool _isStreaming = false;
        private string eskiFrekansBirim = "";
        private string eskiOrneklemeBirim = "";
        private string eskiBantBirim = "";
        private short[]? sonIqHafizasi = null;
        private Dictionary<string, double[]> ozelProfiller = new Dictionary<string, double[]>();
        private int tehditSayaci = 0;
        private const int GEREKLI_SUREKLILIK = 25;
        private const double SABIT_TEHDIT_ESIGI = -40.0;
        private double[]? yumusatilmisFFT = null;
        private double alpha = 0.2;
        private int taramaGecikmesi = 20;
        private bool dcKalibrasyonTetiklendi = false;
        private double i_offset_degeri = 0.0;
        private double q_offset_degeri = 0.0;

        public Form1()
        {
            InitializeComponent();

            if (cmbHedefProfilleri != null && cmbHedefProfilleri.Items.Count > 0)
            {
                cmbHedefProfilleri.SelectedIndex = 0;
            }

            if (cmbBirim != null && cmbBirim.Items.Count > 0) cmbBirim.SelectedIndex = 0;
            if (cmbOrneklemeBirim != null && cmbOrneklemeBirim.Items.Count > 0) cmbOrneklemeBirim.SelectedIndex = 0;
            if (cmbBantBirim != null && cmbBantBirim.Items.Count > 0) cmbBantBirim.SelectedIndex = 0;

            if (cmbBirim != null) eskiFrekansBirim = cmbBirim.Text;
            if (cmbOrneklemeBirim != null) eskiOrneklemeBirim = cmbOrneklemeBirim.Text;
            if (cmbBantBirim != null) eskiBantBirim = cmbBantBirim.Text;
        }

        private decimal BirimDonustur(decimal deger, string eskiBirim, string yeniBirim)
        {
            decimal gercekHz = deger;

            if (eskiBirim.Contains("kHz") || eskiBirim.Contains("kSps")) gercekHz = deger * 1000m;
            else if (eskiBirim.Contains("MHz") || eskiBirim.Contains("MSps")) gercekHz = deger * 1000000m;
            else if (eskiBirim.Contains("GHz")) gercekHz = deger * 1000000000m;

            if (yeniBirim.Contains("kHz") || yeniBirim.Contains("kSps")) return gercekHz / 1000m;
            else if (yeniBirim.Contains("MHz") || yeniBirim.Contains("MSps")) return gercekHz / 1000000m;
            else if (yeniBirim.Contains("GHz")) return gercekHz / 1000000000m;

            return deger;
        }

        private void btnBaglan_Click(object sender, EventArgs e)
        {
            ulong frekansCarpan = 1;
            if (cmbBirim.Text == "kHz") frekansCarpan = 1000;
            else if (cmbBirim.Text == "MHz") frekansCarpan = 1000000;
            else if (cmbBirim.Text == "GHz") frekansCarpan = 1000000000;

            uint orneklemeCarpan = 1;
            if (cmbOrneklemeBirim.Text.Contains("kHz") || cmbOrneklemeBirim.Text.Contains("kSps")) orneklemeCarpan = 1000;
            else if (cmbOrneklemeBirim.Text.Contains("MHz") || cmbOrneklemeBirim.Text.Contains("MSps")) orneklemeCarpan = 1000000;
            else if (cmbOrneklemeBirim.Text.Contains("GHz")) orneklemeCarpan = 1000000000;

            uint bantCarpan = 1;
            if (cmbBantBirim.Text.Contains("kHz")) bantCarpan = 1000;
            else if (cmbBantBirim.Text.Contains("MHz")) bantCarpan = 1000000;
            else if (cmbBantBirim.Text.Contains("GHz")) bantCarpan = 1000000000;
            //KIRPMA OLAYI
            decimal kullaniciIstenenBant = numBantGenisligi.Value * (decimal)bantCarpan;
            decimal kirpmaYuzdesi = 0.10m;

            try
            {
                if (numKirpmaYuzdesi != null) kirpmaYuzdesi = numKirpmaYuzdesi.Value / 100m;
            }
            catch { }

            decimal ekstraBant = kullaniciIstenenBant * kirpmaYuzdesi;

            decimal geciciBant = kullaniciIstenenBant + ekstraBant;

            decimal geciciFrekans = numFrekans.Value * (decimal)frekansCarpan;
            decimal geciciOrnekleme = numOrnekleme.Value * (decimal)orneklemeCarpan;

            if (geciciFrekans < 70000000m) geciciFrekans = 70000000m;
            if (geciciFrekans > 6000000000m) geciciFrekans = 6000000000m;

            // BladeRF Donanım Sınırı Kontrolü
            if (geciciBant > 56000000m)
            {
                geciciBant = 56000000m;
                MessageBox.Show("Seçilen Bant Genişliği ve Kırpma oranı, donanımın maksimum sınırını (56 MHz) aşıyor! Donanım güvenliği için gizli bant genişliği limitlendi.", "Donanım Sınırı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (geciciBant < 1000000m) geciciBant = 1000000m;

            decimal guvenliOrneklemeAltSiniri = geciciBant * 1.2m; // Örnekleme her zaman Banttan %20 büyük olmalı
            if (geciciOrnekleme < guvenliOrneklemeAltSiniri)
            {
                geciciOrnekleme = guvenliOrneklemeAltSiniri;
                if (geciciOrnekleme > 61440000m) // BladeRF maks örnekleme sınırı
                {
                    geciciOrnekleme = 61440000m;
                    geciciBant = geciciOrnekleme / 1.2m;
                }
            }

            numFrekans.Value = geciciFrekans / (decimal)frekansCarpan;
            numOrnekleme.Value = geciciOrnekleme / (decimal)orneklemeCarpan;

            ulong gercekFrekans = (ulong)geciciFrekans;
            uint gercekOrnekleme = (uint)geciciOrnekleme;
            uint gercekBantGenisligi = (uint)geciciBant;

            if (!_isDeviceOpen)
            {
                btnBaglan.Enabled = false;
                btnBaglan.Text = "Cihaz Aranıyor...";
                Application.DoEvents();

                int status = -1;

                try
                {
                    status = BladeRFBridge.bladerf_open(out _devicePointer, IntPtr.Zero);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bağlantı İstisnası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnBaglan.Enabled = true;
                    btnBaglan.Text = "Cihaza Bağlan";
                    return;
                }

                if (status == 0 && _devicePointer != IntPtr.Zero)
                {
                    _isDeviceOpen = true;

                    btnBaglan.BackColor = Color.Green;
                    btnBaglan.Text = "Sistem Aktif (İzleme)";
                    btnBaglan.Enabled = true;
                    btnOku.Enabled = true;
                }
                else
                {
                    btnBaglan.Text = "Bağlantı Hatası!";
                    btnBaglan.Enabled = true;
                    MessageBox.Show($"Cihaz açılamadı! Hata Kodu: {status}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (_isDeviceOpen)
            {
                if (_devicePointer == IntPtr.Zero)
                {
                    MessageBox.Show("Cihaz işaretçisi geçerli değil! Bağlantı koptu.", "Kritik Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _isDeviceOpen = false;
                    btnBaglan.Text = "Cihaza Bağlan";
                    btnBaglan.BackColor = Color.White;
                    return;
                }

                int freqStatus = BladeRFBridge.bladerf_set_frequency(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, gercekFrekans);
                if (gercekOrnekleme <= 0) gercekOrnekleme = 1000000;

                BladeRFBridge.bladerf_set_sample_rate(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, gercekOrnekleme, out uint actualSR);
                BladeRFBridge.bladerf_set_bandwidth(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, gercekBantGenisligi, out uint actualBW);

                if (freqStatus == 0)
                {
                    // --- YENİ: AÇILIŞTA KAZANÇ SENKRONİZASYONU (HARD-INIT) ---
                    // Arayüzdeki AGC kutucuğunun durumuna göre doğru başlangıç modunu seçiyoruz
                    if (chkAGC != null && chkAGC.Checked)
                    {
                        // 2 = BLADERF_GAIN_FASTRAK (Hızlı Otomatik AGC Modu)
                        BladeRFBridge.bladerf_set_gain_mode(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, 2);
                        rtbKonsol.AppendText("[DONANIM] Açılış senkronizasyonu: AGC (Otomatik Kazanç) modu aktif edildi.\n");
                    }
                    else
                    {
                        // 1 = BLADERF_GAIN_MGC (Manuel Kazanç Modu)
                        BladeRFBridge.bladerf_set_gain_mode(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, 1);

                        int baslangicKazanc = 0;
                        if (trbRxKazanci != null) baslangicKazanc = trbRxKazanci.Value;

                        BladeRFBridge.bladerf_set_gain(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, baslangicKazanc);
                        rtbKonsol.AppendText($"[DONANIM] Açılış senkronizasyonu: RX Kazancı {baslangicKazanc} dB (Manuel) olarak zorlandı.\n");
                    }
                    // --------------------------------------------------------

                    btnOku.Enabled = true;
                    MessageBox.Show($"Parametreler uygulandı!\n\nEkranda Görünen: {numBantGenisligi.Value} {cmbBantBirim.Text}\nArka Planda Çekilen Gizli Bant: {actualBW / 1000000.0:F2} MHz", "Aşırı Örnekleme Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void GrafikGuncelle()
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;
            if (sonIqHafizasi == null) return;

            int pictureBoxWidth = 0;
            int pictureBoxHeight = 0;
            bool alarmAktif = false;
            bool gurultuEngelleAktif = false;
            double dinamikTehditEsigi = -40.0;

            float max_dB = 20f;
            float min_dB = -120f;
            double gurultuEsigi = -80.0;

            double kirpmaOrani = 0.10;

            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    pictureBoxWidth = picGrafik.Width;
                    pictureBoxHeight = picGrafik.Height;

                    if (chkAlarmAktif != null) alarmAktif = chkAlarmAktif.Checked;
                    if (chkGurultuEngelle != null) gurultuEngelleAktif = chkGurultuEngelle.Checked;
                    if (trbSquelch != null) dinamikTehditEsigi = trbSquelch.Value;

                    if (numYMax != null) max_dB = (float)numYMax.Value;
                    if (numYMin != null) min_dB = (float)numYMin.Value;
                    if (numGurultuEsigi != null) gurultuEsigi = (double)numGurultuEsigi.Value;

                    if (numKirpmaYuzdesi != null) kirpmaOrani = (double)numKirpmaYuzdesi.Value / 100.0;
                });
            }
            catch { return; }

            if (pictureBoxWidth <= 0 || pictureBoxHeight <= 0) return;

            int num_samples = sonIqHafizasi.Length / 2;
            Bitmap tuval = new Bitmap(pictureBoxWidth, pictureBoxHeight);
            System.Numerics.Complex[] fftVerisi = new System.Numerics.Complex[num_samples];

            for (int i = 0; i < num_samples; i++)
            {
                if (dcKalibrasyonTetiklendi)
                {
                    double i_toplam = 0;
                    double q_toplam = 0;
                    for (int k = 0; k < num_samples; k++)
                    {
                        i_toplam += sonIqHafizasi[k * 2];
                        q_toplam += sonIqHafizasi[k * 2 + 1];
                    }
                    i_offset_degeri = i_toplam / num_samples;
                    q_offset_degeri = q_toplam / num_samples;

                    dcKalibrasyonTetiklendi = false;
                }

                double temiz_I = sonIqHafizasi[i * 2] - i_offset_degeri;
                double temiz_Q = sonIqHafizasi[i * 2 + 1] - q_offset_degeri;

                double window = 0.5 * (1 - Math.Cos(2 * Math.PI * i / (num_samples - 1)));
                fftVerisi[i] = new System.Numerics.Complex(temiz_I * window, temiz_Q * window);
            }

            MathNet.Numerics.IntegralTransforms.Fourier.Forward(fftVerisi, MathNet.Numerics.IntegralTransforms.FourierOptions.Matlab);

            /* --- AKILLI DC SPIKE YAMALAYICI ---
            // Merkezdeki sahte kuleyi sıfırlayıp dipsiz çukur açmak yerine, 
            // deliğin hemen dışındaki sağlıklı gürültüyü kopyalayıp üstünü örtüyoruz.
            int dcEtkiAlani = 5;
            System.Numerics.Complex saglikliSol = fftVerisi[num_samples - dcEtkiAlani - 1];
            System.Numerics.Complex saglikliSag = fftVerisi[dcEtkiAlani + 1];

            for (int k = 0; k <= dcEtkiAlani; k++)
            {
                fftVerisi[k] = saglikliSag;
                if (k > 0)
                {
                    fftVerisi[num_samples - k] = saglikliSol;
                }
            }
              ------------------------------------*/

            // GÜVENLİK KİLİDİ 1: NullReference Hatasını Önleme
            if (yumusatilmisFFT == null || yumusatilmisFFT.Length != pictureBoxWidth)
            {
                yumusatilmisFFT = new double[pictureBoxWidth];
                for (int i = 0; i < pictureBoxWidth; i++) yumusatilmisFFT[i] = min_dB;
            }

            double anlikMaksimumGenlik = -999;
            float dB_farki = max_dB - min_dB;
            if (dB_farki <= 0) dB_farki = 1f;

            using (Graphics g = Graphics.FromImage(tuval))
            {
                g.Clear(Color.Black);

                using (Pen gridKalem = new Pen(Color.FromArgb(40, 40, 40), 1f))
                using (Font eksenFontu = new Font("Arial", 8))
                using (Font baslikFontu = new Font("Arial", 9, FontStyle.Bold))
                using (SolidBrush yaziFircasi = new SolidBrush(Color.LightGray))
                {
                    int yatayKareSayisi = 20;
                    float sutunGenisligi = (float)tuval.Width / yatayKareSayisi;
                    for (int i = 0; i <= yatayKareSayisi; i++)
                    {
                        float x = i * sutunGenisligi;
                        g.DrawLine(gridKalem, x, 0, x, tuval.Height);
                    }

                    int dikeyKareSayisi = 10;
                    float satirYuksekligi = (float)tuval.Height / dikeyKareSayisi;
                    for (int i = 0; i <= dikeyKareSayisi; i++)
                    {
                        float y = i * satirYuksekligi;
                        g.DrawLine(gridKalem, 0, y, tuval.Width, y);
                        if (i > 0 && i < dikeyKareSayisi)
                        {
                            float oran = y / (float)tuval.Height;
                            float dbCizgi = max_dB - (oran * dB_farki);
                            g.DrawString($"{dbCizgi:F0} dB", eksenFontu, yaziFircasi, 5, y - 15);
                        }
                    }

                    int merkezX = tuval.Width / 2;
                    g.DrawLine(Pens.DarkRed, merkezX, 0, merkezX, tuval.Height);
                    g.DrawString("<- Alt Frekanslar", eksenFontu, Brushes.DarkGray, 5, tuval.Height - 15);
                    g.DrawString("[ MERKEZ FREKANS ]", baslikFontu, Brushes.Red, merkezX - 60, tuval.Height - 15);
                    g.DrawString("Üst Frekanslar ->", eksenFontu, Brushes.DarkGray, tuval.Width - 100, tuval.Height - 15);
                }

                using (Pen cizgiKalemi = new Pen(gurultuEngelleAktif ? Color.LimeGreen : Color.Cyan, 1.5f))
                {
                    int yariUzunluk = num_samples / 2;
                    PointF[] sinyalNoktalari = new PointF[tuval.Width];

                    // --- DİNAMİK KIRPMA OKUMASI (GİZLİ KORUMA BANDI) ---
                    double baslangicNoktasi = (num_samples - 1) * kirpmaOrani;
                    double gosterilecekAralik = (num_samples - 1) * (1.0 - (2 * kirpmaOrani));

                    for (int pixelX = 0; pixelX < tuval.Width; pixelX++)
                    {
                        // Kenar kesme oranlaması
                        double i_baslangic = baslangicNoktasi + (((double)pixelX / (tuval.Width - 1)) * gosterilecekAralik);
                        double i_bitis = baslangicNoktasi + (((double)(pixelX + 1) / (tuval.Width - 1)) * gosterilecekAralik);

                        int idx_baslangic = (int)Math.Floor(i_baslangic);
                        int idx_bitis = (int)Math.Ceiling(i_bitis);

                        if (idx_bitis >= num_samples) idx_bitis = num_samples - 1;
                        if (idx_baslangic > idx_bitis) idx_baslangic = idx_bitis;

                        double maxDbPixelIcin = -999;
                        for (int i = idx_baslangic; i <= idx_bitis; i++)
                        {
                            int shiftedIndex = (i + yariUzunluk) % num_samples;
                            double gercekGenlik = fftVerisi[shiftedIndex].Magnitude;
                            double db = 20 * Math.Log10((gercekGenlik / 2048.0) + 1e-10);
                            if (db > maxDbPixelIcin) maxDbPixelIcin = db;
                        }

                        double islenecekDb = maxDbPixelIcin;

                        if (gurultuEngelleAktif)
                        {
                            if (islenecekDb < gurultuEsigi)
                            {
                                islenecekDb = min_dB + 1.0;
                            }
                        }

                        yumusatilmisFFT[pixelX] = (alpha * islenecekDb) + ((1 - alpha) * yumusatilmisFFT[pixelX]);
                        double filtrelenmisDb = yumusatilmisFFT[pixelX];

                        if (filtrelenmisDb > anlikMaksimumGenlik) anlikMaksimumGenlik = filtrelenmisDb;

                        float cizilecekGuc = (float)filtrelenmisDb;
                        float oran = (max_dB - cizilecekGuc) / dB_farki;
                        float y = oran * tuval.Height;

                        if (y < 0) y = 0;
                        if (y > tuval.Height) y = tuval.Height;

                        sinyalNoktalari[pixelX] = new PointF(pixelX, y);
                    }

                    g.DrawLines(cizgiKalemi, sinyalNoktalari);
                }
            }

            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    if (picGrafik.Image != null) picGrafik.Image.Dispose();
                    picGrafik.Image = tuval;

                    if (alarmAktif)
                    {
                        if (anlikMaksimumGenlik > dinamikTehditEsigi)
                        {
                            tehditSayaci++;
                            if (tehditSayaci >= GEREKLI_SUREKLILIK)
                            {
                                lblTehditDurumu.Text = $"⚠ TEHDİT TESPİT EDİLDİ (Güç: {anlikMaksimumGenlik:F1} dB | Eşik: {dinamikTehditEsigi} dB)";
                                lblTehditDurumu.BackColor = Color.Red;
                                lblTehditDurumu.ForeColor = Color.White;
                            }
                        }
                        else
                        {
                            tehditSayaci = 0;
                            lblTehditDurumu.Text = "TEMİZ - SİNYAL BEKLENİYOR...";
                            lblTehditDurumu.BackColor = Color.DarkGreen;
                            lblTehditDurumu.ForeColor = Color.White;
                        }
                    }
                    else
                    {
                        tehditSayaci = 0;
                        lblTehditDurumu.Text = "ALARM SİSTEMİ DEVRE DIŞI";
                        lblTehditDurumu.BackColor = Color.Gray;
                        lblTehditDurumu.ForeColor = Color.White;
                    }
                });
            }
            catch { return; }
        }

        private async void btnOku_Click(object sender, EventArgs e)
        {
            if (!_isDeviceOpen)
            {
                MessageBox.Show("Önce cihazı bağlayın!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_isStreaming)
            {
                _isStreaming = false;
                btnOku.Text = "Hızlı Veri Oku";
                btnOku.BackColor = Color.White;
                rtbKonsol.AppendText("\n[SİSTEM] Spektrum izleme durduruldu.\n");
                return;
            }

            _isStreaming = true;
            btnOku.Text = "Akışı Durdur";
            btnOku.BackColor = Color.Red;

            uint num_samples = 8192;

            rtbKonsol.AppendText("\n[SİSTEM] Canlı spektrum dinleme (RX) BAŞLATILDI...\n");
            rtbKonsol.ScrollToCaret();

            BladeRFBridge.bladerf_sync_config(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, BladeRFBridge.BLADERF_FORMAT_SC16_Q11, 16, num_samples, 8, 5000);
            BladeRFBridge.bladerf_enable_module(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, true);

            await Task.Run(async () =>
            {
                while (_isStreaming)
                {
                    short[] iqData = new short[num_samples * 2];
                    int rxStatus = BladeRFBridge.bladerf_sync_rx(_devicePointer, iqData, num_samples, IntPtr.Zero, 1000);

                    if (rxStatus == 0 && _isStreaming)
                    {
                        sonIqHafizasi = iqData;
                        GrafikGuncelle();
                    }

                    await Task.Delay(taramaGecikmesi);
                }
                BladeRFBridge.bladerf_enable_module(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, false);
            });
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _isStreaming = false;
            if (_isDeviceOpen && _devicePointer != IntPtr.Zero)
            {
                BladeRFBridge.bladerf_enable_module(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, false);
                BladeRFBridge.bladerf_close(_devicePointer);
            }
            base.OnFormClosing(e);
        }

        private void cmbBirim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(eskiFrekansBirim) || eskiFrekansBirim == cmbBirim.Text) return;

            decimal yeniDeger = BirimDonustur(numFrekans.Value, eskiFrekansBirim, cmbBirim.Text);

            if (yeniDeger > numFrekans.Maximum) yeniDeger = numFrekans.Maximum;
            if (yeniDeger < numFrekans.Minimum) yeniDeger = numFrekans.Minimum;

            numFrekans.Value = yeniDeger;
            eskiFrekansBirim = cmbBirim.Text;
        }

        private void cmbOrneklemeBirim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(eskiOrneklemeBirim) || eskiOrneklemeBirim == cmbOrneklemeBirim.Text) return;

            decimal yeniDeger = BirimDonustur(numOrnekleme.Value, eskiOrneklemeBirim, cmbOrneklemeBirim.Text);

            if (yeniDeger > numOrnekleme.Maximum) yeniDeger = numOrnekleme.Maximum;
            if (yeniDeger < numOrnekleme.Minimum) yeniDeger = numOrnekleme.Minimum;

            numOrnekleme.Value = yeniDeger;
            eskiOrneklemeBirim = cmbOrneklemeBirim.Text;
        }

        private void cmbBantBirim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(eskiBantBirim) || eskiBantBirim == cmbBantBirim.Text) return;

            decimal yeniDeger = BirimDonustur(numBantGenisligi.Value, eskiBantBirim, cmbBantBirim.Text);

            if (yeniDeger > numBantGenisligi.Maximum) yeniDeger = numBantGenisligi.Maximum;
            if (yeniDeger < numBantGenisligi.Minimum) yeniDeger = numBantGenisligi.Minimum;

            numBantGenisligi.Value = yeniDeger;
            eskiBantBirim = cmbBantBirim.Text;
        }

        private void trbYumusatma_Scroll(object sender, EventArgs e)
        {
            alpha = trbYumusatma.Value / 100.0;

            if (lblYumusatmaDegeri != null)
            {
                lblYumusatmaDegeri.Text = $"%{trbYumusatma.Value}";
            }
        }

        private void rdoIzlemeModu_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoIzlemeModu.Checked)
            {
                rtbKonsol.AppendText("\n[SİSTEM] İzleme Modu Aktif. Sadece dinleme (RX) yapılıyor.\n");
                rtbKonsol.ScrollToCaret();
            }
        }

        private void btnDcKalibrasyon_Click(object sender, EventArgs e)
        {
            dcKalibrasyonTetiklendi = true;
            MessageBox.Show("DC Offset Kalibrasyonu alındı. Merkezdeki donanım gürültüsü sıfırlandı!", "Kalibrasyon Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string TehditSiniflandir()
        {
            decimal frekansMhz = numFrekans.Value;
            if (cmbBirim.Text == "GHz") frekansMhz *= 1000m;
            else if (cmbBirim.Text == "kHz") frekansMhz /= 1000m;

            if (frekansMhz >= 2400m && frekansMhz <= 2500m)
                return "DRONE / FPV / Wİ-Fİ SİNYALİ";

            if (frekansMhz >= 430m && frekansMhz <= 440m)
                return "UHF TAKTİK TELSİZ";

            if (frekansMhz >= 800m && frekansMhz <= 1900m)
                return "GSM / LTE HÜCRESEL AĞ (TELEFON)";

            return "BİLİNMEYEN RF AKTİVİTESİ";
        }

        private void rdoTaarruzModu_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTaarruzModu.Checked)
            {
                rtbKonsol.SelectionColor = Color.Red;
                rtbKonsol.AppendText("\n[DİKKAT] Taarruz modu harici terminal üzerinden yönetilecektir.\n");
                rtbKonsol.SelectionColor = rtbKonsol.ForeColor;
                rtbKonsol.ScrollToCaret();
            }
        }

        private void rdoTestModu_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTestModu.Checked)
            {
                rtbKonsol.AppendText("\n[SİSTEM] Dahili Test (Loopback) Modu Aktif.\n");
                if (_isDeviceOpen && _devicePointer != IntPtr.Zero)
                {
                    BladeRFBridge.bladerf_set_loopback(_devicePointer, 2);
                    rtbKonsol.AppendText("[DONANIM] Donanımsal Loopback şalteri kapatıldı.\n");
                }
                rtbKonsol.ScrollToCaret();
            }
            else
            {
                if (_isDeviceOpen && _devicePointer != IntPtr.Zero)
                {
                    BladeRFBridge.bladerf_set_loopback(_devicePointer, 0);
                    rtbKonsol.AppendText("[DONANIM] Loopback devreden çıkarıldı.\n");
                }
            }
        }

        private void trbRxKazanci_Scroll(object sender, EventArgs e)
        {
            lblRxKazanciDegeri.Text = trbRxKazanci.Value.ToString() + " dB";
        }

        private void chkAGC_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isDeviceOpen || _devicePointer == IntPtr.Zero) return;

            if (chkAGC.Checked)
            {
                // 2 = BLADERF_GAIN_FASTRAK (Hızlı Otomatik AGC Modu)
                int status = BladeRFBridge.bladerf_set_gain_mode(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, 2);

                if (status == 0)
                {
                    rtbKonsol.AppendText("[DONANIM] AGC (Otomatik Kazanç) AKTİF.\n");
                    trbRxKazanci.Enabled = false;
                }
                else
                {
                    rtbKonsol.AppendText($"[HATA] AGC açılamadı! (Kod: {status})\n");
                    chkAGC.Checked = false;
                }
            }
            else
            {
                // 1 = BLADERF_GAIN_MGC (Manuel Kazanç Modu)
                BladeRFBridge.bladerf_set_gain_mode(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, 1);
                rtbKonsol.AppendText("[DONANIM] AGC KAPATILDI. Manuel moda dönüldü.\n");

                trbRxKazanci.Enabled = true;

                // AGC kapanınca cihaz havada kalmasın diye mevcut sürgü değerini zorla uygula
                int guncelKazanc = trbRxKazanci.Value;
                BladeRFBridge.bladerf_set_gain(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, guncelKazanc);
                rtbKonsol.AppendText($"[DONANIM] Kazanç senkronize edildi: {guncelKazanc} dB.\n");
            }

            rtbKonsol.ScrollToCaret();
        }

        private void chkBiasTee_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isDeviceOpen || _devicePointer == IntPtr.Zero) return;
            bool aktifMi = chkBiasTee.Checked;
            BladeRFBridge.bladerf_set_bias_tee(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, aktifMi);
            if (aktifMi)
            {
                rtbKonsol.SelectionColor = Color.Orange;
                rtbKonsol.AppendText("[DİKKAT] Bias-Tee AKTİF! 5V güç basılıyor.\n");
                rtbKonsol.SelectionColor = rtbKonsol.ForeColor;
            }
            else
            {
                rtbKonsol.AppendText("[SİSTEM] Bias-Tee kapatıldı.\n");
            }
            rtbKonsol.ScrollToCaret();
        }

        private void cmbHedefProfilleri_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbHedefProfilleri.SelectedItem == null) return;

            string secilenProfil = cmbHedefProfilleri.Text;
            string secilenItem = cmbHedefProfilleri.SelectedItem.ToString() ?? "";

            if (string.IsNullOrEmpty(secilenItem)) return;

            if (secilenItem == "➕ Yeni Özel Ayar Kaydet...")
            {
                string yeniProfilAdi = "Özel Profil " + (ozelProfiller.Count + 1).ToString();

                double guncelFrekans = Convert.ToDouble(numFrekans.Value);
                double guncelOrnekleme = Convert.ToDouble(numOrnekleme.Value);
                double guncelBant = Convert.ToDouble(numBantGenisligi.Value);

                ozelProfiller.Add(yeniProfilAdi, new double[] { guncelFrekans, guncelOrnekleme, guncelBant });

                int sonSira = cmbHedefProfilleri.Items.Count - 1;
                cmbHedefProfilleri.Items.Insert(sonSira, yeniProfilAdi);
                cmbHedefProfilleri.SelectedItem = yeniProfilAdi;

                MessageBox.Show($"{yeniProfilAdi} başarıyla kaydedildi!\nFrekans: {guncelFrekans} GHz\nÖrnekleme: {guncelOrnekleme} MSps", "Profil Kaydedildi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (ozelProfiller.ContainsKey(secilenItem))
            {
                double[] kayitliAyarlar = ozelProfiller[secilenItem];
                numFrekans.Value = Convert.ToDecimal(kayitliAyarlar[0]);
                numOrnekleme.Value = Convert.ToDecimal(kayitliAyarlar[1]);
                numBantGenisligi.Value = Convert.ToDecimal(kayitliAyarlar[2]);
            }
            else if (secilenProfil.Contains("Drone"))
            {
                eskiFrekansBirim = "GHz";
                cmbBirim.SelectedIndex = cmbBirim.FindString("GHz");
                numFrekans.Value = 2.4m;

                eskiOrneklemeBirim = "MSps";
                int orneklemeIndex = cmbOrneklemeBirim.FindString("MSps");
                if (orneklemeIndex == -1) orneklemeIndex = cmbOrneklemeBirim.FindString("MHz");
                cmbOrneklemeBirim.SelectedIndex = orneklemeIndex;
                numOrnekleme.Value = 22m;

                eskiBantBirim = "MHz";
                cmbBantBirim.SelectedIndex = cmbBantBirim.FindString("MHz");
                numBantGenisligi.Value = 20m;

                rtbKonsol.AppendText("\n[SİSTEM] Taktik Profil Yüklendi: DRONE / FPV (2.4 GHz Ağı)\n");
            }
            else if (secilenProfil.Contains("Telsiz"))
            {
                eskiFrekansBirim = "MHz";
                cmbBirim.SelectedIndex = cmbBirim.FindString("MHz");
                numFrekans.Value = 433m;

                eskiOrneklemeBirim = "MSps";
                int orneklemeIndex = cmbOrneklemeBirim.FindString("MSps");
                if (orneklemeIndex == -1) orneklemeIndex = cmbOrneklemeBirim.FindString("MHz");
                cmbOrneklemeBirim.SelectedIndex = orneklemeIndex;
                numOrnekleme.Value = 2.5m;

                eskiBantBirim = "MHz";
                cmbBantBirim.SelectedIndex = cmbBantBirim.FindString("MHz");
                numBantGenisligi.Value = 2m;

                rtbKonsol.AppendText("\n[SİSTEM] Taktik Profil Yüklendi: TAKTİK TELSİZ (433 MHz)\n");
            }
            else if (secilenProfil.Contains("Telefon"))
            {
                eskiFrekansBirim = "MHz";
                cmbBirim.SelectedIndex = cmbBirim.FindString("MHz");
                numFrekans.Value = 1750m;

                eskiOrneklemeBirim = "MSps";
                int orneklemeIndex = cmbOrneklemeBirim.FindString("MSps");
                if (orneklemeIndex == -1) orneklemeIndex = cmbOrneklemeBirim.FindString("MHz");
                cmbOrneklemeBirim.SelectedIndex = orneklemeIndex;
                numOrnekleme.Value = 2m;

                eskiBantBirim = "MHz";
                cmbBantBirim.SelectedIndex = cmbBantBirim.FindString("MHz");
                numBantGenisligi.Value = 1.5m;

                rtbKonsol.AppendText("\n[SİSTEM] Taktik Profil Yüklendi: GSM / LTE TELEFON SİNYALİ\n");
            }

            rtbKonsol.ScrollToCaret();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "ATEL Akıllı Jammer Karar Destek Arayüzü";

            cmbHedefProfilleri.Items.Add("-----");
            cmbHedefProfilleri.Items.Add("➕ Yeni Özel Ayar Kaydet...");

            if (lblSqulechTehtid != null) lblSqulechTehtid.Text = $"%{trbSquelch.Value}";
            if (lblYumusatmaDegeri != null) lblYumusatmaDegeri.Text = $"%{trbYumusatma.Value}";
            if (lblTaramaHiziDegeri != null) lblTaramaHiziDegeri.Text = $"{trbTaramaHizi.Value} ms";
        }

        private void grpDonanim_Enter(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }

        private void trbSquelch_Scroll(object sender, EventArgs e)
        {
            GrafikGuncelle();

            if (lblSqulechTehtid != null)
            {
                lblSqulechTehtid.Text = $"%{trbSquelch.Value}";
            }
        }

        private void picGrafik_Click(object sender, EventArgs e) { }
        private void lblTehditDurumu_Click(object sender, EventArgs e) { }
        private void numFrekans_ValueChanged(object sender, EventArgs e) { }

        private void numYMax_ValueChanged(object sender, EventArgs e)
        {
            if (numYMin == null || numYMax == null) return;

            if (numYMax.Value <= numYMin.Value)
            {
                decimal guvenliDeger = numYMax.Value - 10;

                if (guvenliDeger < numYMin.Minimum) guvenliDeger = numYMin.Minimum;
                if (guvenliDeger > numYMin.Maximum) guvenliDeger = numYMin.Maximum;

                numYMin.Value = guvenliDeger;
            }
        }

        private void numYMin_ValueChanged(object sender, EventArgs e)
        {
            if (numYMin == null || numYMax == null) return;

            if (numYMin.Value >= numYMax.Value)
            {
                decimal guvenliDeger = numYMin.Value + 10;

                if (guvenliDeger > numYMax.Maximum) guvenliDeger = numYMax.Maximum;
                if (guvenliDeger < numYMax.Minimum) guvenliDeger = numYMax.Minimum;

                numYMax.Value = guvenliDeger;
            }
        }

        private void trbTaramaHizi_Scroll(object sender, EventArgs e)
        {
            taramaGecikmesi = trbTaramaHizi.Value;

            if (lblTaramaHiziDegeri != null)
            {
                lblTaramaHiziDegeri.Text = $"{trbTaramaHizi.Value} ms";
            }
        }

        private void lblOlcekDegeri_Click(object sender, EventArgs e) { }
        private void lblYumusatmaDegeri_Click(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }

        private void lblRxKazanciDegeri_Click(object sender, EventArgs e)
        {

        }

        private void trbRxKazanci_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_isDeviceOpen || _devicePointer == IntPtr.Zero || chkAGC.Checked) return;

            int yeniKazanc = trbRxKazanci.Value;
            int status = BladeRFBridge.bladerf_set_gain(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, yeniKazanc);

            if (status == 0)
                rtbKonsol.AppendText($"[DONANIM] RX Kazancı {yeniKazanc} dB olarak ayarlandı.\n");
            else
                rtbKonsol.AppendText($"[HATA] Kazanç uygulanamadı! Kodu: {status}\n");
        }

        private void numKirpmaYuzdesi_ValueChanged(object sender, EventArgs e)
        {
            lblKırpılmayanAlan.Text = "%" + (100 - numKirpmaYuzdesi.Value).ToString() ;

        }

        private void lblKırpılmayanAlan_Click(object sender, EventArgs e)
        {

        }
    }
}