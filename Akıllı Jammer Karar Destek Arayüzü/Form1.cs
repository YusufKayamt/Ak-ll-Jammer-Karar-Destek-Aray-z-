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
using System.Media;
namespace Akıllı_Jammer_Karar_Destek_Arayüzü
{
    public partial class Form1 : Form
    {
        private IntPtr _devicePointer = IntPtr.Zero;
        private bool _isDeviceOpen = false;
        private bool _isStreaming = false;
        private bool _islemeKilidi = false;
        private bool _rxIpligiAktif = false;
        private bool _ileriModAktif = false;
        private bool _otomatikDegisim = false;
        // Dinamik güncelleme ve profil seçimi için kilit mekanizmaları
        private bool _dinamikGuncellemeBekliyor = false;
        private bool _profilYukleniyor = false;
        private bool _cizimMesgul = false;

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
        private uint donanimGercekOrnekleme = 0;

        private double hedefMarkerFrekansHz = 0;
        private bool isMarkerDragging = false;
        private Size orjinalFormBoyutu;

        public Form1()
        {
            InitializeComponent();

            if (btnModMuhendis != null)
            {
                btnModMuhendis.Click -= btnModMuhendis_Click;
                btnModMuhendis.Click += btnModMuhendis_Click;
            }

            // Dinamik değer atamalarını tetikleyecek olaylar (Designer'dan bağımsız çalışır)
            if (numFrekans != null) numFrekans.ValueChanged += (s, e) => DinamikParametreUygula();
            if (numOrnekleme != null) numOrnekleme.ValueChanged += (s, e) => DinamikParametreUygula();
            if (numBantGenisligi != null) numBantGenisligi.ValueChanged += (s, e) => DinamikParametreUygula();
            if (numKirpmaYuzdesi != null) numKirpmaYuzdesi.ValueChanged += (s, e) => DinamikParametreUygula();
            if (btnSaldırı != null)
            {
                btnSaldırı.Enabled = false;
                btnSaldırı.BackColor = Color.Gray;
            }

            if (cmbBirim != null && cmbBirim.Items.Count > 0) cmbBirim.SelectedIndex = 0;
            if (cmbOrneklemeBirim != null && cmbOrneklemeBirim.Items.Count > 0) cmbOrneklemeBirim.SelectedIndex = 0;
            if (cmbBantBirim != null && cmbBantBirim.Items.Count > 0) cmbBantBirim.SelectedIndex = 0;

            if (cmbBirim != null) eskiFrekansBirim = cmbBirim.Text;
            if (cmbOrneklemeBirim != null) eskiOrneklemeBirim = cmbOrneklemeBirim.Text;
            if (cmbBantBirim != null) eskiBantBirim = cmbBantBirim.Text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "ATEL Akıllı Jammer Karar Destek Arayüzü";
            this.AutoSize = false;
            this.AutoScroll = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            orjinalFormBoyutu = this.ClientSize;

            cmbHedefProfilleri.Items.Clear();
            cmbHedefProfilleri.Items.Add("-----");
            cmbHedefProfilleri.Items.Add("➕ Yeni Özel Ayar Kaydet...");

            cmbHedefProfilleri.SelectedIndex = -1;

            if (lblSqulechTehtid != null) lblSqulechTehtid.Text = $"%{trbSquelch.Value}";
            if (lblYumusatmaDegeri != null) lblYumusatmaDegeri.Text = $"%{trbYumusatma.Value}";
            if (lblTaramaHiziDegeri != null) lblTaramaHiziDegeri.Text = $"{trbTaramaHizi.Value} ms";

            _profilYukleniyor = true;

            if (numFrekans != null) { numFrekans.Minimum = 0; numFrekans.Maximum = 9999999999m; }
            if (numOrnekleme != null) { numOrnekleme.Minimum = 0; numOrnekleme.Maximum = 9999999999m; }
            if (numBantGenisligi != null) { numBantGenisligi.Minimum = 0; numBantGenisligi.Maximum = 9999999999m; }
            if (numKirpmaYuzdesi != null) { numKirpmaYuzdesi.Minimum = 0; numKirpmaYuzdesi.Maximum = 100; }

            cmbBirim.SelectedIndex = cmbBirim.FindString("MHz");
            numFrekans.Value = 2400m;
            cmbOrneklemeBirim.SelectedIndex = cmbOrneklemeBirim.FindString("MSps") != -1 ? cmbOrneklemeBirim.FindString("MSps") : cmbOrneklemeBirim.FindString("MHz");
            numOrnekleme.Value = 22m;
            cmbBantBirim.SelectedIndex = cmbBantBirim.FindString("MHz");
            numBantGenisligi.Value = 20m;

            _profilYukleniyor = false;

            _ileriModAktif = false;
            EkranModunuAyarla(_ileriModAktif);

            lblTehditDurumu.Text = "ASKERİ (TAKTİK) MODDA SİSTEM AÇILIYOR...";
            lblTehditDurumu.BackColor = Color.DarkSlateGray;

            OtopilotuTetikle();
        }

        private void btnModMuhendis_Click(object? sender, EventArgs e)
        {
            _ileriModAktif = !_ileriModAktif;
            EkranModunuAyarla(_ileriModAktif);
        }

        private void EkranModunuAyarla(bool ileriModAcik)
        {
            if (ileriModAcik)
            {
                this.AutoScroll = true;
                this.ClientSize = orjinalFormBoyutu;

                foreach (System.Windows.Forms.Control c in this.Controls)
                {
                    if (c.Left >= picGrafik.Right && (c is GroupBox || c is Button))
                    {
                        c.Visible = true;
                    }
                }

                btnModMuhendis.Text = "TAKTİK MODA DÖN";
                this.Text = "ATEL Karar Destek - [İLERİ MÜHENDİS MODU]";
            }
            else
            {
                foreach (System.Windows.Forms.Control c in this.Controls)
                {
                    if (c.Left >= picGrafik.Right && (c is GroupBox || c is Button))
                    {
                        if (c.Name != "btnModMuhendis" && c.Name != "btnSaldırı")
                        {
                            c.Visible = false;
                        }
                    }
                }

                btnModMuhendis.Text = "İLERİ MOD AKTİF";
                this.Text = "ATEL Karar Destek - [TAKTİK (ASKERİ) MOD]";
                this.AutoScroll = false;

                int yeniGenislik = picGrafik.Right + 30;
                if (btnSaldırı != null && btnSaldırı.Right > yeniGenislik) yeniGenislik = btnSaldırı.Right + 30;

                int yeniYukseklik = this.ClientSize.Height;
                if (rtbKonsol != null && rtbKonsol.Visible)
                    yeniYukseklik = rtbKonsol.Bottom + 10;
                else
                    yeniYukseklik = btnModMuhendis.Bottom + 10;

                this.ClientSize = new Size(yeniGenislik, yeniYukseklik);
            }

            this.CenterToScreen();
        }

        private async void OtopilotuTetikle()
        {
            await Task.Delay(1000);

            if (!_isDeviceOpen)
            {
                rtbKonsol.AppendText("\n[OTOPİLOT] Cihaza otomatik bağlanılıyor...\n");
                btnBaglan_Click(this, EventArgs.Empty);
                await Task.Delay(1000);
            }

            if (_isDeviceOpen && !_isStreaming)
            {
                rtbKonsol.AppendText("[OTOPİLOT] Veri akışı başlatılıyor...\n");
                btnOku_Click(this, EventArgs.Empty);
                rtbKonsol.ScrollToCaret();
            }
        }

        private async void DinamikParametreUygula()
        {
            if (!_isDeviceOpen || _islemeKilidi || _profilYukleniyor || _otomatikDegisim) return;
            if (_dinamikGuncellemeBekliyor) return;

            _dinamikGuncellemeBekliyor = true;
            await Task.Delay(400); // Kullanıcı sayılarla oynamayı bitirene kadar bekle
            _dinamikGuncellemeBekliyor = false;

            btnBaglan_Click(this, EventArgs.Empty);
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

        private async void btnBaglan_Click(object sender, EventArgs e)
        {
            if (_islemeKilidi) return;
            _islemeKilidi = true;

            try
            {
                ulong frekansCarpan = 1;
                if (cmbBirim.Text == "kHz") frekansCarpan = 1000;
                else if (cmbBirim.Text == "MHz") frekansCarpan = 1000000;
                else if (cmbBirim.Text == "GHz") frekansCarpan = 1000000000;

                uint orneklemeCarpan = 1;
                if (cmbOrneklemeBirim.Text.Contains("k")) orneklemeCarpan = 1000;
                else if (cmbOrneklemeBirim.Text.Contains("M")) orneklemeCarpan = 1000000;
                else if (cmbOrneklemeBirim.Text.Contains("G")) orneklemeCarpan = 1000000000;

                uint bantCarpan = 1;
                if (cmbBantBirim.Text.Contains("k")) bantCarpan = 1000;
                else if (cmbBantBirim.Text.Contains("M")) bantCarpan = 1000000;
                else if (cmbBantBirim.Text.Contains("G")) bantCarpan = 1000000000;

                decimal kullaniciFrekans = numFrekans.Value * (decimal)frekansCarpan;
                decimal kullaniciOrnekleme = numOrnekleme.Value * (decimal)orneklemeCarpan;
                decimal kullaniciBant = numBantGenisligi.Value * (decimal)bantCarpan;

                decimal kirpmaYuzdesi = 0.30m;
                try { if (numKirpmaYuzdesi != null) kirpmaYuzdesi = numKirpmaYuzdesi.Value / 100m; } catch { }

                decimal donanimBant = kullaniciBant + (kullaniciBant * kirpmaYuzdesi);

                decimal guvenliOrneklemeAltSiniri = donanimBant * 1.2m;

                if (kullaniciOrnekleme < guvenliOrneklemeAltSiniri)
                {
                    kullaniciOrnekleme = guvenliOrneklemeAltSiniri;
                }

                if (kullaniciFrekans < 70000000m) kullaniciFrekans = 70000000m;
                if (kullaniciFrekans > 6000000000m) kullaniciFrekans = 6000000000m;

                if (donanimBant > 56000000m)
                {
                    donanimBant = 56000000m;

                    kullaniciBant = donanimBant / (1m + kirpmaYuzdesi);
                }

                if (donanimBant < 1000000m)
                {
                    donanimBant = 1000000m;
                    kullaniciBant = donanimBant / (1m + kirpmaYuzdesi);
                }

                if (kullaniciOrnekleme > 61440000m) kullaniciOrnekleme = 61440000m;

                _otomatikDegisim = true;
                numFrekans.Value = kullaniciFrekans / (decimal)frekansCarpan;
                numBantGenisligi.Value = kullaniciBant / (decimal)bantCarpan; 
                numOrnekleme.Value = kullaniciOrnekleme / (decimal)orneklemeCarpan;
                _otomatikDegisim = false;

                ulong gercekFrekans = (ulong)kullaniciFrekans;
                uint gercekOrnekleme = (uint)kullaniciOrnekleme;
                uint gercekBantGenisligi = (uint)donanimBant;


                if (!_isDeviceOpen)
                {
                    int status = -1;
                    try
                    {
                        status = BladeRFBridge.bladerf_open(out _devicePointer, IntPtr.Zero);
                    }
                    catch
                    {
                        return;
                    }

                    if (status == 0 && _devicePointer != IntPtr.Zero) _isDeviceOpen = true;
                    else return;
                }

                if (_isDeviceOpen && _devicePointer != IntPtr.Zero)
                {
                    bool arkaPlandaAkiyordu = _isStreaming;

                    if (arkaPlandaAkiyordu)
                    {
                        _isStreaming = false;
                        while (_rxIpligiAktif) await Task.Delay(50);
                        await Task.Delay(300);
                    }

                    try
                    {
                        int freqStatus = BladeRFBridge.bladerf_set_frequency(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, gercekFrekans);
                        BladeRFBridge.bladerf_set_sample_rate(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, gercekOrnekleme, out uint actualSR);
                        BladeRFBridge.bladerf_set_bandwidth(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, gercekBantGenisligi, out uint actualBW);

                        donanimGercekOrnekleme = actualSR;

                        if (freqStatus == 0)
                        {
                            rtbKonsol.AppendText($"\n[DONANIM] Parametreler uygulandı! Yeni SR: {actualSR} Hz\n");
                            rtbKonsol.ScrollToCaret();
                        }
                    }
                    catch (DivideByZeroException)
                    {
                        rtbKonsol.AppendText("\n[UYARI] Cihaz FPGA saati toparlanıyor...\n");
                    }
                    catch (Exception ex)
                    {
                        rtbKonsol.AppendText($"\n[DONANIM HATASI] {ex.Message}\n");
                    }

                    if (arkaPlandaAkiyordu)
                    {
                        await Task.Delay(100);
                        btnOku_Click(this, EventArgs.Empty);
                    }
                }
            }
            finally
            {
                _islemeKilidi = false;
            }
        }

        private async void btnOku_Click(object sender, EventArgs e)
        {
            if (!_isDeviceOpen) return;

            if (_isStreaming)
            {
                _isStreaming = false;
                rtbKonsol.AppendText("\n[SİSTEM] Spektrum izleme durduruldu...\n");
                return;
            }

            _isStreaming = true;
            uint num_samples = 8192;
            rtbKonsol.AppendText("\n[SİSTEM] Canlı dinleme BAŞLATILDI...\n");
            rtbKonsol.ScrollToCaret();

            BladeRFBridge.bladerf_sync_config(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, BladeRFBridge.BLADERF_FORMAT_SC16_Q11, 16, num_samples, 8, 5000);
            BladeRFBridge.bladerf_enable_module(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, true);

            _rxIpligiAktif = true;

            await Task.Run(async () =>
            {
                try
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
                }
                finally
                {
                    BladeRFBridge.bladerf_enable_module(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, false);
                    _rxIpligiAktif = false;
                }
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

        private void GrafikGuncelle()
        {
            if (this.IsDisposed || !this.IsHandleCreated || sonIqHafizasi == null) return;

            // === 1. ZIRH: KARE ATLAMA (FRAME DROPPING) ===
            // Eğer arayüz hala bir önceki veriyi çizmekle boğuşuyorsa, bu yeni veriyi atla! (Sistemin donmasını %100 engeller)
            if (_cizimMesgul) return;
            _cizimMesgul = true;

            int pictureBoxWidth = 0;
            int pictureBoxHeight = 0;
            bool alarmAktif = false;
            bool gurultuEngelleAktif = false;
            double dinamikTehditEsigi = -40.0;
            float max_dB = 20f;
            float min_dB = -120f;
            double gurultuEsigi = -80.0;
            double gosterilecekBantHz = 20000000.0;

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

                    double bCarpan = 1;
                    if (cmbBantBirim.Text.Contains("k")) bCarpan = 1000;
                    else if (cmbBantBirim.Text.Contains("M")) bCarpan = 1000000;
                    else if (cmbBantBirim.Text.Contains("G")) bCarpan = 1000000000;
                    gosterilecekBantHz = (double)numBantGenisligi.Value * bCarpan;
                });
            }
            catch
            {
                _cizimMesgul = false;
                return;
            }

            if (pictureBoxWidth <= 0 || pictureBoxHeight <= 0)
            {
                _cizimMesgul = false;
                return;
            }

            int num_samples = sonIqHafizasi.Length / 2;
            Bitmap tuval = new Bitmap(pictureBoxWidth, pictureBoxHeight);
            System.Numerics.Complex[] fftVerisi = new System.Numerics.Complex[num_samples];

            for (int i = 0; i < num_samples; i++)
            {
                if (dcKalibrasyonTetiklendi)
                {
                    double i_toplam = 0, q_toplam = 0;
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
                            float dbCizgi = max_dB - ((y / tuval.Height) * dB_farki);
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

                    double gercekSR = donanimGercekOrnekleme > 0 ? donanimGercekOrnekleme : 31200000.0;
                    if (gosterilecekBantHz > gercekSR) gosterilecekBantHz = gercekSR;

                    double soldanKirpilacakOran = ((gercekSR - gosterilecekBantHz) / 2.0) / gercekSR;
                    double gosterilecekOran = gosterilecekBantHz / gercekSR;

                    double baslangicNoktasi = (num_samples - 1) * soldanKirpilacakOran;
                    double gosterilecekAralik = (num_samples - 1) * gosterilecekOran;

                    for (int pixelX = 0; pixelX < tuval.Width; pixelX++)
                    {
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
                            double db = 20 * Math.Log10((fftVerisi[shiftedIndex].Magnitude / 2048.0) + 1e-10);
                            if (db > maxDbPixelIcin) maxDbPixelIcin = db;
                        }

                        double islenecekDb = maxDbPixelIcin;
                        if (gurultuEngelleAktif && islenecekDb < gurultuEsigi) islenecekDb = min_dB + 1.0;

                        yumusatilmisFFT[pixelX] = (alpha * islenecekDb) + ((1 - alpha) * yumusatilmisFFT[pixelX]);
                        double filtrelenmisDb = yumusatilmisFFT[pixelX];

                        if (filtrelenmisDb > anlikMaksimumGenlik) anlikMaksimumGenlik = filtrelenmisDb;

                        float y = ((max_dB - (float)filtrelenmisDb) / dB_farki) * tuval.Height;
                        if (y < 0) y = 0;
                        if (y > tuval.Height) y = tuval.Height;

                        sinyalNoktalari[pixelX] = new PointF(pixelX, y);
                    }
                    g.DrawLines(cizgiKalemi, sinyalNoktalari);
                }
            }

            try
            {
                // === 2. ZIRH: ASENKRON ÇİZİM (BEGININVOKE) ===
                // Invoke, çizim bitene kadar arka planı kilitlerdi. BeginInvoke sistemi kilitlemez, "Bunu müsait olunca çiz" der.
                this.BeginInvoke((MethodInvoker)delegate
                {
                    try
                    {
                        if (picGrafik.Image != null) picGrafik.Image.Dispose();
                        picGrafik.Image = tuval;
                        picGrafik.Invalidate();

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

                                    if (btnSaldırı != null)
                                    {
                                        btnSaldırı.Enabled = true;
                                        btnSaldırı.BackColor = Color.Red;
                                        btnSaldırı.ForeColor = Color.White;
                                    }
                                }
                            }
                            else
                            {
                                tehditSayaci = 0;
                                lblTehditDurumu.Text = "TEMİZ - DİNLENİYOR...";
                                lblTehditDurumu.BackColor = Color.DarkGreen;
                                lblTehditDurumu.ForeColor = Color.White;

                                if (btnSaldırı != null)
                                {
                                    btnSaldırı.Enabled = false;
                                    btnSaldırı.BackColor = Color.Gray;
                                }
                            }
                        }
                        else
                        {
                            tehditSayaci = 0;
                            lblTehditDurumu.Text = "ALARM SİSTEMİ DEVRE DIŞI";
                            lblTehditDurumu.BackColor = Color.Gray;
                            lblTehditDurumu.ForeColor = Color.White;

                            if (btnSaldırı != null)
                            {
                                btnSaldırı.Enabled = false;
                                btnSaldırı.BackColor = Color.Gray;
                            }
                        }
                    }
                    finally
                    {
                        _cizimMesgul = false;
                    }
                });
            }
            catch
            {
                _cizimMesgul = false;
            }
        }

        private double FareX_To_Frekans(int fareX, int width)
        {
            double fCarpan = 1;
            if (cmbBirim.Text == "kHz") fCarpan = 1000;
            else if (cmbBirim.Text == "MHz") fCarpan = 1000000;
            else if (cmbBirim.Text == "GHz") fCarpan = 1000000000;
            double merkezFrekansHz = (double)numFrekans.Value * fCarpan;

            double bCarpan = 1;
            if (cmbBantBirim.Text.Contains("k")) bCarpan = 1000;
            else if (cmbBantBirim.Text.Contains("M")) bCarpan = 1000000;
            else if (cmbBantBirim.Text.Contains("G")) bCarpan = 1000000000;
            double gosterilecekBantHz = (double)numBantGenisligi.Value * bCarpan;

            double gercekSR = donanimGercekOrnekleme > 0 ? (double)donanimGercekOrnekleme : 31200000.0;
            if (gosterilecekBantHz > gercekSR) gosterilecekBantHz = gercekSR;

            double solKenarFrekans = merkezFrekansHz - (gosterilecekBantHz / 2.0);
            double ekranOrani = (double)fareX / width;

            return solKenarFrekans + (ekranOrani * gosterilecekBantHz);
        }

        private void chkMarkerAktif_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMarkerAktif.Checked)
            {
                double fCarpan = 1;
                if (cmbBirim.Text == "kHz") fCarpan = 1000;
                else if (cmbBirim.Text == "MHz") fCarpan = 1000000;
                else if (cmbBirim.Text == "GHz") fCarpan = 1000000000;
                hedefMarkerFrekansHz = (double)numFrekans.Value * fCarpan;
            }
            picGrafik.Invalidate();
        }

        private void picGrafik_MouseDown(object sender, MouseEventArgs e)
        {
            if (chkMarkerAktif.Checked && e.Button == MouseButtons.Left)
            {
                isMarkerDragging = true;
                hedefMarkerFrekansHz = FareX_To_Frekans(e.X, picGrafik.Width);
                picGrafik.Invalidate();
            }
        }

        private void picGrafik_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMarkerDragging && chkMarkerAktif.Checked)
            {
                hedefMarkerFrekansHz = FareX_To_Frekans(e.X, picGrafik.Width);
                picGrafik.Invalidate();
            }
        }

        private void picGrafik_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) isMarkerDragging = false;
        }

        private void picGrafik_Paint(object sender, PaintEventArgs e)
        {
            if (chkMarkerAktif.Checked && hedefMarkerFrekansHz > 0)
            {
                double fCarpan = 1;
                if (cmbBirim.Text == "kHz") fCarpan = 1000;
                else if (cmbBirim.Text == "MHz") fCarpan = 1000000;
                else if (cmbBirim.Text == "GHz") fCarpan = 1000000000;
                double merkezFrekansHz = (double)numFrekans.Value * fCarpan;

                double bCarpan = 1;
                if (cmbBantBirim.Text.Contains("k")) bCarpan = 1000;
                else if (cmbBantBirim.Text.Contains("M")) bCarpan = 1000000;
                else if (cmbBantBirim.Text.Contains("G")) bCarpan = 1000000000;
                double gosterilecekBantHz = (double)numBantGenisligi.Value * bCarpan;

                double gercekSR = donanimGercekOrnekleme > 0 ? donanimGercekOrnekleme : 31200000.0;
                if (gosterilecekBantHz > gercekSR) gosterilecekBantHz = gercekSR;

                double solKenarFrekans = merkezFrekansHz - (gosterilecekBantHz / 2.0);
                double sagKenarFrekans = merkezFrekansHz + (gosterilecekBantHz / 2.0);

                if (hedefMarkerFrekansHz >= solKenarFrekans && hedefMarkerFrekansHz <= sagKenarFrekans)
                {
                    float oran = (float)((hedefMarkerFrekansHz - solKenarFrekans) / gosterilecekBantHz);
                    float markerPixelX = oran * picGrafik.Width;

                    using (Pen markerKalemi = new Pen(Color.Yellow, 2f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                    using (Font markerFontu = new Font("Consolas", 10, FontStyle.Bold))
                    {
                        e.Graphics.DrawLine(markerKalemi, markerPixelX, 0, markerPixelX, picGrafik.Height);
                        string markerYazi = $"▼ M1: {hedefMarkerFrekansHz / 1000000.0:F4} MHz";
                        float yaziX = markerPixelX > picGrafik.Width - 120 ? markerPixelX - 130 : markerPixelX + 5;
                        e.Graphics.DrawString(markerYazi, markerFontu, Brushes.Yellow, yaziX, 20);
                    }
                }
            }
        }

        private void cmbHedefProfilleri_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbHedefProfilleri.SelectedItem == null || cmbHedefProfilleri.SelectedIndex == -1) return;
            string secilenItem = cmbHedefProfilleri.SelectedItem.ToString() ?? "";
            if (string.IsNullOrEmpty(secilenItem) || secilenItem == "-----") return;

            _profilYukleniyor = true;

            if (secilenItem == "➕ Yeni Özel Ayar Kaydet...")
            {
                string yeniProfilAdi = "Özel Profil " + (ozelProfiller.Count + 1).ToString();
                double guncelFrekans = Convert.ToDouble(numFrekans.Value);
                double guncelOrnekleme = Convert.ToDouble(numOrnekleme.Value);
                double guncelBant = Convert.ToDouble(numBantGenisligi.Value);
                double guncelSquelch = trbSquelch.Value;

                ozelProfiller.Add(yeniProfilAdi, new double[] { guncelFrekans, guncelOrnekleme, guncelBant, guncelSquelch });
                cmbHedefProfilleri.Items.Insert(cmbHedefProfilleri.Items.Count - 1, yeniProfilAdi);
                cmbHedefProfilleri.SelectedItem = yeniProfilAdi;
                MessageBox.Show($"{yeniProfilAdi} Kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (ozelProfiller.ContainsKey(secilenItem))
            {
                double[] kayitliAyarlar = ozelProfiller[secilenItem];
                numFrekans.Value = Convert.ToDecimal(kayitliAyarlar[0]);
                numOrnekleme.Value = Convert.ToDecimal(kayitliAyarlar[1]);
                numBantGenisligi.Value = Convert.ToDecimal(kayitliAyarlar[2]);
                if (kayitliAyarlar.Length > 3) trbSquelch.Value = (int)kayitliAyarlar[3];
                rtbKonsol.AppendText($"\n[SİSTEM] {secilenItem} yüklendi.\n");
            }
            else if (secilenItem.Contains("Drone"))
            {
                cmbBirim.SelectedIndex = cmbBirim.FindString("GHz");
                numFrekans.Value = 2.4m;
                cmbOrneklemeBirim.SelectedIndex = cmbOrneklemeBirim.FindString("MSps") != -1 ? cmbOrneklemeBirim.FindString("MSps") : cmbOrneklemeBirim.FindString("MHz");
                numOrnekleme.Value = 22m;
                cmbBantBirim.SelectedIndex = cmbBantBirim.FindString("MHz");
                numBantGenisligi.Value = 20m;
                rtbKonsol.AppendText("\n[SİSTEM] Taktik Profil: DRONE (2.4 GHz)\n");
            }
            else if (secilenItem.Contains("Telsiz"))
            {
                cmbBirim.SelectedIndex = cmbBirim.FindString("MHz");
                numFrekans.Value = 433m;
                cmbOrneklemeBirim.SelectedIndex = cmbOrneklemeBirim.FindString("MSps") != -1 ? cmbOrneklemeBirim.FindString("MSps") : cmbOrneklemeBirim.FindString("MHz");
                numOrnekleme.Value = 2.5m;
                cmbBantBirim.SelectedIndex = cmbBantBirim.FindString("MHz");
                numBantGenisligi.Value = 2m;
                rtbKonsol.AppendText("\n[SİSTEM] Taktik Profil: TELSİZ (433 MHz)\n");
            }
            else if (secilenItem.Contains("Telefon"))
            {
                cmbBirim.SelectedIndex = cmbBirim.FindString("MHz");
                numFrekans.Value = 1750m;
                cmbOrneklemeBirim.SelectedIndex = cmbOrneklemeBirim.FindString("MSps") != -1 ? cmbOrneklemeBirim.FindString("MSps") : cmbOrneklemeBirim.FindString("MHz");
                numOrnekleme.Value = 2m;
                cmbBantBirim.SelectedIndex = cmbBantBirim.FindString("MHz");
                numBantGenisligi.Value = 1.5m;
                rtbKonsol.AppendText("\n[SİSTEM] Taktik Profil: GSM / LTE TELEFON\n");
            }

            rtbKonsol.ScrollToCaret();
            _profilYukleniyor = false;

            if (_isDeviceOpen) btnBaglan_Click(this, EventArgs.Empty);
        }

        private void btnSaldırı_Click(object sender, EventArgs e)
        {
            rtbKonsol.AppendText("\n[DİKKAT] TAARRUZ SİNYALİ BAŞLATILDI!\n");
            rtbKonsol.ScrollToCaret();
        }

        private void cmbBirim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(eskiFrekansBirim) || eskiFrekansBirim == cmbBirim.Text) return;
            decimal yeniDeger = BirimDonustur(numFrekans.Value, eskiFrekansBirim, cmbBirim.Text);
            if (yeniDeger > numFrekans.Maximum) yeniDeger = numFrekans.Maximum;
            if (yeniDeger < numFrekans.Minimum) yeniDeger = numFrekans.Minimum;
            numFrekans.Value = yeniDeger;
            eskiFrekansBirim = cmbBirim.Text;
            DinamikParametreUygula();
        }

        private void cmbOrneklemeBirim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(eskiOrneklemeBirim) || eskiOrneklemeBirim == cmbOrneklemeBirim.Text) return;
            decimal yeniDeger = BirimDonustur(numOrnekleme.Value, eskiOrneklemeBirim, cmbOrneklemeBirim.Text);
            if (yeniDeger > numOrnekleme.Maximum) yeniDeger = numOrnekleme.Maximum;
            if (yeniDeger < numOrnekleme.Minimum) yeniDeger = numOrnekleme.Minimum;
            numOrnekleme.Value = yeniDeger;
            eskiOrneklemeBirim = cmbOrneklemeBirim.Text;
            DinamikParametreUygula();
        }

        private void cmbBantBirim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(eskiBantBirim) || eskiBantBirim == cmbBantBirim.Text) return;
            decimal yeniDeger = BirimDonustur(numBantGenisligi.Value, eskiBantBirim, cmbBantBirim.Text);
            if (yeniDeger > numBantGenisligi.Maximum) yeniDeger = numBantGenisligi.Maximum;
            if (yeniDeger < numBantGenisligi.Minimum) yeniDeger = numBantGenisligi.Minimum;
            numBantGenisligi.Value = yeniDeger;
            eskiBantBirim = cmbBantBirim.Text;
            DinamikParametreUygula();
        }

        private void trbYumusatma_Scroll(object sender, EventArgs e)
        {
            alpha = trbYumusatma.Value / 100.0;
            if (lblYumusatmaDegeri != null) lblYumusatmaDegeri.Text = $"%{trbYumusatma.Value}";
        }

        private void trbTaramaHizi_Scroll(object sender, EventArgs e)
        {
            taramaGecikmesi = trbTaramaHizi.Value;
            if (lblTaramaHiziDegeri != null) lblTaramaHiziDegeri.Text = $"{trbTaramaHizi.Value} ms";
        }

        private void trbSquelch_Scroll(object sender, EventArgs e)
        {
            if (lblSqulechTehtid != null) lblSqulechTehtid.Text = $"%{trbSquelch.Value}";
        }

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

        private void chkAGC_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isDeviceOpen || _devicePointer == IntPtr.Zero) return;
            if (chkAGC.Checked)
            {
                BladeRFBridge.bladerf_set_gain_mode(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, 2);
                trbRxKazanci.Enabled = false;
            }
            else
            {
                BladeRFBridge.bladerf_set_gain_mode(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, 1);
                trbRxKazanci.Enabled = true;
                BladeRFBridge.bladerf_set_gain(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, trbRxKazanci.Value);
            }
        }

        private void trbRxKazanci_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_isDeviceOpen || _devicePointer == IntPtr.Zero || chkAGC.Checked) return;
            BladeRFBridge.bladerf_set_gain(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, trbRxKazanci.Value);
        }

        private void trbRxKazanci_Scroll(object sender, EventArgs e)
        {
            if (lblRxKazanciDegeri != null) lblRxKazanciDegeri.Text = trbRxKazanci.Value.ToString() + " dB";
        }

        private void chkBiasTee_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isDeviceOpen || _devicePointer == IntPtr.Zero) return;
            BladeRFBridge.bladerf_set_bias_tee(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, chkBiasTee.Checked);
        }

        private void rdoIzlemeModu_CheckedChanged(object sender, EventArgs e) { }
        private void rdoTaarruzModu_CheckedChanged(object sender, EventArgs e) { }

        private void rdoTestModu_CheckedChanged(object sender, EventArgs e)
        {
            if (_isDeviceOpen && _devicePointer != IntPtr.Zero)
            {
                BladeRFBridge.bladerf_set_loopback(_devicePointer, rdoTestModu.Checked ? 2 : 0);
            }
        }

        private void btnDcKalibrasyon_Click(object sender, EventArgs e)
        {
            dcKalibrasyonTetiklendi = true;
        }

        // Hata almamak için eski tasarım bağlantıları korunmuştur
        private void numFrekans_ValueChanged(object sender, EventArgs e) { }
        private void grpDonanim_Enter(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void picGrafik_Click(object sender, EventArgs e) { }
        private void lblTehditDurumu_Click(object sender, EventArgs e) { }
        private void lblOlcekDegeri_Click(object sender, EventArgs e) { }
        private void lblYumusatmaDegeri_Click(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
        private void lblRxKazanciDegeri_Click(object sender, EventArgs e) { }
        private void numKirpmaYuzdesi_ValueChanged(object sender, EventArgs e) { }
        private void lblKırpılmayanAlan_Click(object sender, EventArgs e) { }
        private void lblSqulechTehtid_Click(object sender, EventArgs e) { }
        private void pnlAnaArayuz_Paint(object sender, PaintEventArgs e) { }
        private void label11_Click(object sender, EventArgs e) { }
        private void lblTaramaHiziDegeri_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void grpGelismisAyarlar_Enter(object sender, EventArgs e) { }

        private void btnSaldırı_Click_1(object sender, EventArgs e)
        {/*rtbKonsol.AppendText("\n[DİKKAT] TAARRUZ SİNYALİ BAŞLATILDI!\n");
            rtbKonsol.ScrollToCaret();

            try
            {
                string muzikYolu = @"C:\Users\kayay\OneDrive\Masaüstü\ATEL SAVUNMA STAJ\saldırıbaslat.wav";

                System.Media.SoundPlayer oynatici = new System.Media.SoundPlayer(muzikYolu);
                oynatici.PlayLooping();
            }
            catch (Exception ex)
            {
                rtbKonsol.AppendText($"\n[HATA] Ses dosyası oynatılamadı: {ex.Message}\n");
                rtbKonsol.ScrollToCaret();
            }*/
        }
    }
}