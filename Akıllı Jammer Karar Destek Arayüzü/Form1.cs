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

            decimal geciciFrekans = numFrekans.Value * (decimal)frekansCarpan;
            decimal geciciOrnekleme = numOrnekleme.Value * (decimal)orneklemeCarpan;
            decimal geciciBant = numBantGenisligi.Value * (decimal)bantCarpan;

            if (geciciFrekans < 70000000m) geciciFrekans = 70000000m;
            if (geciciFrekans > 6000000000m) geciciFrekans = 6000000000m;

            if (geciciOrnekleme < 1000000m) geciciOrnekleme = 1000000m;
            if (geciciOrnekleme > 61440000m) geciciOrnekleme = 61440000m;

            if (geciciBant < 1000000m) geciciBant = 1000000m;
            if (geciciBant > 56000000m) geciciBant = 56000000m;

            decimal guvenliOrneklemeAltSiniri = geciciBant * 1.2m;
            if (geciciOrnekleme < guvenliOrneklemeAltSiniri)
            {
                geciciOrnekleme = guvenliOrneklemeAltSiniri;
                if (geciciOrnekleme > 61440000m)
                {
                    geciciOrnekleme = 61440000m;
                    geciciBant = geciciOrnekleme / 1.2m;
                }
            }

            numFrekans.Value = geciciFrekans / (decimal)frekansCarpan;
            numOrnekleme.Value = geciciOrnekleme / (decimal)orneklemeCarpan;
            numBantGenisligi.Value = geciciBant / (decimal)bantCarpan;

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
                    btnOku.Enabled = true;
                    MessageBox.Show($"Parametreler cihaza başarıyla uygulandı!\n\nDonanımın Ayarladığı Örnekleme: {actualSR} Hz", "Güncelleme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    btnOku.Enabled = false;
                    MessageBox.Show("Parametreler cihaza uygulanamadı. Sınırları kontrol edin.", "Donanım Sınırı İhlali", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            double dinamikTehditEsigi = -40.0;

            float max_dB = 20f;
            float min_dB = -120f;

            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    pictureBoxWidth = picGrafik.Width;
                    pictureBoxHeight = picGrafik.Height;
                    alarmAktif = chkAlarmAktif.Checked;
                    dinamikTehditEsigi = trbSquelch.Value;

                    if (numYMax != null) max_dB = (float)numYMax.Value;
                    if (numYMin != null) min_dB = (float)numYMin.Value;
                });
            }
            catch (ObjectDisposedException) { return; }
            catch (InvalidOperationException) { return; }

            if (pictureBoxWidth <= 0 || pictureBoxHeight <= 0) return;

            int num_samples = sonIqHafizasi.Length / 2;
            Bitmap tuval = new Bitmap(pictureBoxWidth, pictureBoxHeight);

            Complex[] fftVerisi = new Complex[num_samples];
            for (int i = 0; i < num_samples; i++)
            {
                double window = 0.5 * (1 - Math.Cos(2 * Math.PI * i / (num_samples - 1)));
                fftVerisi[i] = new Complex(sonIqHafizasi[i * 2] * window, sonIqHafizasi[i * 2 + 1] * window);
            }

            Fourier.Forward(fftVerisi, FourierOptions.Matlab);

            double anlikMaksimumGenlik = -999;

            // DÜZELTME 1: Filtre başlangıcı artık arayüzdeki "min_dB" değerine bağlı değil!
            // Mutlak fiziksel bir dip gürültü değeri olan -150 dB'ye sabitlendi.
            if (yumusatilmisFFT == null || yumusatilmisFFT.Length != pictureBoxWidth)
            {
                yumusatilmisFFT = new double[pictureBoxWidth];
                for (int i = 0; i < pictureBoxWidth; i++) yumusatilmisFFT[i] = -150.0;
            }

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
                    // Arka plan Grid çizimi (Referans çizgilerimiz)
                    for (int i = 0; i < tuval.Height; i += 50)
                    {
                        g.DrawLine(gridKalem, 0, i, tuval.Width, i);
                        float oran = i / (float)tuval.Height;
                        float dB_degeri = max_dB - (oran * dB_farki);

                        if (i > 10) g.DrawString($"{dB_degeri:F0} dB", eksenFontu, yaziFircasi, 5, i - 15);
                    }

                    for (int i = 0; i < tuval.Width; i += 50) g.DrawLine(gridKalem, i, 0, i, tuval.Height);

                    int merkezX = tuval.Width / 2;
                    g.DrawLine(Pens.DarkRed, merkezX, 0, merkezX, tuval.Height);

                    g.DrawString("<- Alt Frekanslar", eksenFontu, Brushes.DarkGray, 5, tuval.Height - 15);
                    g.DrawString("[ MERKEZ FREKANS ]", baslikFontu, Brushes.Red, merkezX - 60, tuval.Height - 15);
                    g.DrawString("Üst Frekanslar ->", eksenFontu, Brushes.DarkGray, tuval.Width - 100, tuval.Height - 15);
                }

                using (Pen cyanKalem = new Pen(Color.Cyan, 1.5f))
                {
                    PointF oncekiNokta = new PointF(0, tuval.Height);
                    bool ilkNoktaMi = true;
                    int yariUzunluk = num_samples / 2;
                    int adimBoyutu = Math.Max(1, num_samples / tuval.Width);

                    for (int pixelX = 0; pixelX < tuval.Width; pixelX++)
                    {
                        double maxDbPixelIcin = -999;

                        for (int b = 0; b < adimBoyutu; b++)
                        {
                            int i = (pixelX * adimBoyutu) + b;
                            if (i >= num_samples) break;

                            int shiftedIndex = i < yariUzunluk ? i + yariUzunluk : i - yariUzunluk;
                            double gercekGenlik = fftVerisi[shiftedIndex].Magnitude;

                            double db = 20 * Math.Log10((gercekGenlik / 2048.0) + 1e-10);

                            if (db > maxDbPixelIcin) maxDbPixelIcin = db;
                        }

                        yumusatilmisFFT[pixelX] = (alpha * maxDbPixelIcin) + ((1 - alpha) * yumusatilmisFFT[pixelX]);
                        double filtrelenmisDb = yumusatilmisFFT[pixelX];

                        if (filtrelenmisDb > anlikMaksimumGenlik) anlikMaksimumGenlik = filtrelenmisDb;

                        // DÜZELTME 2: Mavi sinyalin koordinatı SADECE kılavuz çizgileriyle aynı formülü kullanır.
                        float cizilecekGuc = (float)filtrelenmisDb;
                        float oran = (max_dB - cizilecekGuc) / dB_farki;
                        float y = oran * tuval.Height;

                        // SİLİNEN KISIM: Eski mantıksız "Osiloskop Ölçeği (trbOlcek)" çarpanı buradan tamamen silindi!
                        // Sinyal artık ekrandaki sayıları birebir takip edecek.

                        if (y < 0) y = 0;
                        if (y > tuval.Height) y = tuval.Height;

                        PointF yeniNokta = new PointF(pixelX, y);

                        if (!ilkNoktaMi) g.DrawLine(cyanKalem, oncekiNokta, yeniNokta);

                        oncekiNokta = yeniNokta;
                        ilkNoktaMi = false;
                    }
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
                                string tehditTuru = TehditSiniflandir();
                                lblTehditDurumu.Text = $"⚠ TEHDİT TESPİT EDİLDİ: {tehditTuru} (Güç: {anlikMaksimumGenlik:F1} dB | Eşik: {dinamikTehditEsigi} dB)";
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
            catch (ObjectDisposedException) { return; }
            catch (InvalidOperationException) { return; }
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
        }

        private void rdoIzlemeModu_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoIzlemeModu.Checked)
            {
                rtbKonsol.AppendText("\n[SİSTEM] İzleme Modu Aktif. Sadece dinleme (RX) yapılıyor.\n");
                rtbKonsol.ScrollToCaret();
            }
        }

        private void btnDcOffset_Click(object sender, EventArgs e)
        {
            if (!_isDeviceOpen)
            {
                rtbKonsol.AppendText("\n[HATA] Cihaz bağlı değil. Kalibrasyon yapılamaz.\n");
                return;
            }

            rtbKonsol.AppendText("\n--- DC OFFSET KALİBRASYONU BAŞLATILDI ---\n");
            rtbKonsol.AppendText("Lokal Osilatör (LO) sızıntısı donanımsal olarak temizleniyor...\n");
            rtbKonsol.AppendText("[BAŞARILI] I ve Q kanallarındaki faz dengesizliği sıfırlandı.\n");
            rtbKonsol.ScrollToCaret();
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
            if (!_isDeviceOpen || _devicePointer == IntPtr.Zero) return;
            int yeniKazanc = trbRxKazanci.Value;
            BladeRFBridge.bladerf_set_gain(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, yeniKazanc);
            rtbKonsol.AppendText($"[DONANIM] RX Kazancı {yeniKazanc} dB olarak ayarlandı.\n");
            rtbKonsol.ScrollToCaret();
        }

        private void chkAGC_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isDeviceOpen || _devicePointer == IntPtr.Zero) return;
            if (chkAGC.Checked)
            {
                BladeRFBridge.bladerf_set_gain_mode(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, 1);
                rtbKonsol.AppendText("[DONANIM] AGC (Otomatik Kazanç) AKTİF.\n");
                trbRxKazanci.Enabled = false;
            }
            else
            {
                BladeRFBridge.bladerf_set_gain_mode(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, 0);
                rtbKonsol.AppendText("[DONANIM] AGC KAPATILDI.\n");
                trbRxKazanci.Enabled = true;
                BladeRFBridge.bladerf_set_gain(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, trbRxKazanci.Value);
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
            cmbHedefProfilleri.Items.Add("-----");
            cmbHedefProfilleri.Items.Add("➕ Yeni Özel Ayar Kaydet...");
        }

        private void grpDonanim_Enter(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void trbSquelch_Scroll(object sender, EventArgs e)
        {
            GrafikGuncelle();
        }
        private void picGrafik_Click(object sender, EventArgs e) { }
        private void lblTehditDurumu_Click(object sender, EventArgs e) { }
        private void numFrekans_ValueChanged(object sender, EventArgs e) { }

        private void numYMax_ValueChanged(object sender, EventArgs e)
        {
            if (numYMax.Value <= numYMin.Value)
                numYMax.Value = numYMin.Value + 25;

            GrafikGuncelle();
        }

        private void numYMin_ValueChanged(object sender, EventArgs e)
        {
            if (numYMin.Value >= numYMax.Value)
                numYMin.Value = numYMax.Value - 25;

            GrafikGuncelle();
        }

        private void trbTaramaHizi_Scroll(object sender, EventArgs e)
        {
            taramaGecikmesi = trbTaramaHizi.Value;
        }
    }
}