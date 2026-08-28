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
using System.IO;
using System.Text.Json;
using ClosedXML.Excel;

namespace Akıllı_Jammer_Karar_Destek_Arayüzü
{
    public partial class Form1 : Form
    {
        #region 1. DEĞİŞKENLER VE DURUM YÖNETİMİ
        private IntPtr _devicePointer = IntPtr.Zero;
        private bool _isDeviceOpen = false;
        private bool _isStreaming = false;
        private bool _islemeKilidi = false;
        private bool _rxIpligiAktif = false;
        private bool _ileriModAktif = false;
        private bool _otomatikDegisim = false;
        private bool _dinamikGuncellemeBekliyor = false;
        private bool _profilYukleniyor = false;
        private bool _cizimMesgul = false;
        private bool _okumaIslemiTamamlandi = true;
        private bool _saldiriAktif = false;
        private bool dcKalibrasyonTetiklendi = false;

        private string eskiFrekansBirim = string.Empty;
        private string eskiOrneklemeBirim = string.Empty;
        private string eskiBantBirim = string.Empty;
        private string eskiTxFrekansBirim = string.Empty;
        private string eskiTxBantBirim = string.Empty;

        private short[] sonIqHafizasi = new short[4096];
        private Dictionary<string, double[]> ozelProfiller = new Dictionary<string, double[]>();
        private double[]? yumusatilmisFFT = null;

        private int tehditSayaci = 0;
        private double alpha = 0.2;
        private int taramaGecikmesi = 20;
        private double i_offset_degeri = 0.0;
        private double q_offset_degeri = 0.0;
        private uint donanimGercekOrnekleme = 0;
        private double hedefMarkerFrekansHz = 0;
        private bool isMarkerDragging = false;
        private int _mevcutLoopbackModu = -1;

        private Size orjinalFormBoyutu;
        private DateTime sonLogZamani = DateTime.MinValue;

        private string _seciliDil = string.Empty;
        private string _seciliTema = string.Empty;
        private ContextMenuStrip _ayarlarMenu;
        #endregion

        #region 2. BAŞLATMA VE YAŞAM DÖNGÜSÜ
        public Form1()
        {
            InitializeComponent();
            _seciliDil = Properties.Settings.Default.VARSAYILAN_DIL;
            _seciliTema = Properties.Settings.Default.VARSAYILAN_TEMA;

            if (btnModMuhendis != null) btnModMuhendis.Click += btnModMuhendis_Click;
            if (trbTxGain != null) trbTxGain.MouseUp += trbTxGain_MouseUp;
            if (numRxFrekans != null) numRxFrekans.ValueChanged += (s, e) => DinamikParametreUygula();
            if (numRxOrnekleme != null) numRxOrnekleme.ValueChanged += (s, e) => DinamikParametreUygula();
            if (numRxBantGenisligi != null) numRxBantGenisligi.ValueChanged += (s, e) => DinamikParametreUygula();
            if (numKirpmaYuzdesi != null) numKirpmaYuzdesi.ValueChanged += (s, e) => DinamikParametreUygula();

            if (numTxFrekans != null) numTxFrekans.ValueChanged += TxParametresiDegisti;
            if (numTxBantGenisligi != null) numTxBantGenisligi.ValueChanged += TxParametresiDegisti;
            if (cmbTxBirim != null) cmbTxBirim.SelectedIndexChanged += cmbTxBirim_SelectedIndexChanged;
            if (cmbTxBantBirim != null) cmbTxBantBirim.SelectedIndexChanged += cmbTxBantBirim_SelectedIndexChanged;

            if (rdoIzlemeModu != null) rdoIzlemeModu.CheckedChanged += OperasyonModu_Degisimi;
            if (rdoTaarruzModu != null) rdoTaarruzModu.CheckedChanged += OperasyonModu_Degisimi;
            if (rdoTestModu != null) rdoTestModu.CheckedChanged += OperasyonModu_Degisimi;

            if (btnSaldırı != null)
            {
                btnSaldırı.Enabled = false;
                btnSaldırı.BackColor = TemaMotoru.TEMA_PASIF;
            }

            if (cmbRxBirim != null && cmbRxBirim.Items.Count > 0) cmbRxBirim.SelectedIndex = 0;
            if (cmbRxOrneklemeBirim != null && cmbRxOrneklemeBirim.Items.Count > 0) cmbRxOrneklemeBirim.SelectedIndex = 0;
            if (cmbRxBantBirim != null && cmbRxBantBirim.Items.Count > 0) cmbRxBantBirim.SelectedIndex = 0;
            if (cmbTxBirim != null && cmbTxBirim.Items.Count > 0) cmbTxBirim.SelectedIndex = 0;
            if (cmbTxBantBirim != null && cmbTxBantBirim.Items.Count > 0) cmbTxBantBirim.SelectedIndex = 0;

            if (cmbRxBirim != null) eskiFrekansBirim = cmbRxBirim.Text;
            if (cmbRxOrneklemeBirim != null) eskiOrneklemeBirim = cmbRxOrneklemeBirim.Text;
            if (cmbRxBantBirim != null) eskiBantBirim = cmbRxBantBirim.Text;
            if (cmbTxBirim != null) eskiTxFrekansBirim = cmbTxBirim.Text;
            if (cmbTxBantBirim != null) eskiTxBantBirim = cmbTxBantBirim.Text;

            _ileriModAktif = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = DilMotoru.Cevir(Properties.Settings.Default.UI_MOD_TAKTIK_BASLIK);
            this.AutoSize = false;
            this.AutoScroll = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            orjinalFormBoyutu = this.ClientSize;
            cmbHedefProfilleri.Items.Clear();

            cmbHedefProfilleri.Items.Add(Properties.Settings.Default.UI_PROFIL_DRONE);
            cmbHedefProfilleri.Items.Add(Properties.Settings.Default.UI_PROFIL_TELSIZ);
            cmbHedefProfilleri.Items.Add(Properties.Settings.Default.UI_PROFIL_TELEFON);

            DilVeTemaMenusuOlustur();
            AyarlariYukle();
            UI_LimitleriUygula();

            cmbHedefProfilleri.Items.Add(DilMotoru.Cevir(Properties.Settings.Default.UI_PROFIL_YENI_KAYDET));
            cmbHedefProfilleri.SelectedIndex = 0;

            ContextMenuStrip profilMenu = new ContextMenuStrip();
            profilMenu.BackColor = Color.FromArgb(40, 40, 40);
            profilMenu.ForeColor = Color.White;
            profilMenu.Items.Add(DilMotoru.Cevir(Properties.Settings.Default.MENU_PROFIL_SIL), null, (s, ev) =>
            {
                if (!_ileriModAktif)
                {
                    MessageBox.Show(DilMotoru.Cevir(Properties.Settings.Default.MSG_YETKI_METIN), DilMotoru.Cevir(Properties.Settings.Default.MSG_YETKI_BASLIK), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string seciliItem = cmbHedefProfilleri.SelectedItem?.ToString() ?? string.Empty;
                if (!string.IsNullOrEmpty(seciliItem) && ozelProfiller.ContainsKey(seciliItem))
                {
                    string mesaj = string.Format(DilMotoru.Cevir(Properties.Settings.Default.MSG_PROFIL_SIL_ONAY), seciliItem);
                    DialogResult cevap = MessageBox.Show(mesaj, DilMotoru.Cevir(Properties.Settings.Default.MSG_PROFIL_SIL_BASLIK), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (cevap == DialogResult.Yes)
                    {
                        ozelProfiller.Remove(seciliItem);
                        cmbHedefProfilleri.Items.Remove(seciliItem);
                        cmbHedefProfilleri.SelectedIndex = 0;
                        AyarlariKaydet();
                        KonsolaYaz(string.Format(DilMotoru.Cevir(Properties.Settings.Default.LOG_PROFIL_SILINDI), seciliItem));
                    }
                }
            });
            cmbHedefProfilleri.ContextMenuStrip = profilMenu;

            _profilYukleniyor = true;
            EkranModunuAyarla(_ileriModAktif);
            OtopilotuTetikle();
            GrafikGuncelle();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            AyarlariKaydet();
            _isStreaming = false;
            _saldiriAktif = false;

            Task.Run(() =>
            {
                int zamanAsimi = 0;
                while (!_okumaIslemiTamamlandi && zamanAsimi < Properties.Settings.Default.KapanisZamanAsimiIterasyon)
                {
                    System.Threading.Thread.Sleep(Properties.Settings.Default.KapanisBeklemeMs);
                    zamanAsimi++;
                }

                if (_isDeviceOpen && _devicePointer != IntPtr.Zero)
                {
                    try
                    {
                        BladeRFBridge.bladerf_enable_module(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, false);
                        BladeRFBridge.bladerf_close(_devicePointer);
                    }
                    catch { }
                }
            });
            base.OnFormClosing(e);
        }
        #endregion

        #region 3. ARAYÜZ (UI), KORUMALI LİMİTLER VE LOG MOTORU

        // ZIRH 1: "ArgumentOutOfRangeException" çökmesini engelleyen güvenli atama metodu
        private void GuvenliAta(NumericUpDown num, decimal deger)
        {
            if (num == null) return;
            try
            {
                decimal yuvarlanmisDeger = Math.Round(deger, num.DecimalPlaces);
                if (yuvarlanmisDeger < num.Minimum) yuvarlanmisDeger = num.Minimum;
                if (yuvarlanmisDeger > num.Maximum) yuvarlanmisDeger = num.Maximum;
                num.Value = yuvarlanmisDeger;
            }
            catch { }
        }

        private void UI_LimitleriUygula()
        {
            SetNumericLimits(numRxFrekans, cmbRxBirim?.Text, (decimal)Properties.Settings.Default.MinFrekansHz, (decimal)Properties.Settings.Default.MaxFrekansHz);
            SetNumericLimits(numRxOrnekleme, cmbRxOrneklemeBirim?.Text, 1000000m, (decimal)Properties.Settings.Default.MaxOrneklemeHz);
            SetNumericLimits(numRxBantGenisligi, cmbRxBantBirim?.Text, (decimal)Properties.Settings.Default.MinBantGenisligi, (decimal)Properties.Settings.Default.MaxBantGenisligi);

            SetNumericLimits(numTxFrekans, cmbTxBirim?.Text, (decimal)Properties.Settings.Default.MinFrekansHz, (decimal)Properties.Settings.Default.MaxFrekansHz);
            SetNumericLimits(numTxBantGenisligi, cmbTxBantBirim?.Text, (decimal)Properties.Settings.Default.MinBantGenisligi, (decimal)Properties.Settings.Default.MaxBantGenisligi);
        }

        private void SetNumericLimits(NumericUpDown num, string birim, decimal gercekMinHz, decimal gercekMaxHz)
        {
            if (num == null || string.IsNullOrEmpty(birim)) return;

            decimal carpan = 1;
            if (birim.Contains("G")) carpan = 1000000000m;
            else if (birim.Contains("M")) carpan = 1000000m;
            else if (birim.Contains("k") || birim.Contains("K")) carpan = 1000m;

            decimal maxVal = gercekMaxHz / carpan;
            decimal minVal = gercekMinHz / carpan;

            // ZIRH 2: Kilitleri serbest bırak, hassasiyeti sabitle ve güvenli sınırla.
            num.Minimum = decimal.MinValue;
            num.Maximum = decimal.MaxValue;
            num.DecimalPlaces = 4;

            if (num.Value > maxVal) num.Value = maxVal;
            if (num.Value < minVal) num.Value = minVal;

            num.Minimum = minVal;
            num.Maximum = maxVal;
        }

        private decimal BirimDonustur(decimal deger, string eskiBirim, string yeniBirim)
        {
            decimal gercekHz = deger;

            if (eskiBirim.Contains("G")) gercekHz = deger * 1000000000m;
            else if (eskiBirim.Contains("M")) gercekHz = deger * 1000000m;
            else if (eskiBirim.Contains("k") || eskiBirim.Contains("K")) gercekHz = deger * 1000m;

            if (yeniBirim.Contains("G")) return gercekHz / 1000000000m;
            else if (yeniBirim.Contains("M")) return gercekHz / 1000000m;
            else if (yeniBirim.Contains("k") || yeniBirim.Contains("K")) return gercekHz / 1000m;

            return deger;
        }

        private void KonsolaYaz(string metin)
        {
            if (string.IsNullOrEmpty(metin)) return;
            string temizMetin = metin.Replace("\\n", "").Replace("\n", "") + Environment.NewLine;

            if (rtbKonsol.InvokeRequired)
            {
                rtbKonsol.Invoke(new Action(() => { rtbKonsol.AppendText(temizMetin); rtbKonsol.ScrollToCaret(); }));
            }
            else
            {
                rtbKonsol.AppendText(temizMetin);
                rtbKonsol.ScrollToCaret();
            }
        }

        private void DilVeTemaMenusuOlustur()
        {
            _ayarlarMenu = new ContextMenuStrip();
            _ayarlarMenu.BackColor = Color.FromArgb(40, 40, 40);
            _ayarlarMenu.ForeColor = Color.White;

            ToolStripMenuItem dilMenu = new ToolStripMenuItem("🌐 " + DilMotoru.Cevir(Properties.Settings.Default.MENU_DIL_SECIMI));
            ToolStripMenuItem temaMenu = new ToolStripMenuItem("🎨 " + DilMotoru.Cevir(Properties.Settings.Default.MENU_TEMA_SECIMI));

            string aramaDizini = AppDomain.CurrentDomain.BaseDirectory;
            string[] tumDosyalar = Directory.GetFiles(aramaDizini);

            foreach (string dosya in tumDosyalar)
            {
                string dosyaAdi = Path.GetFileName(dosya);
                string kucukAd = dosyaAdi.ToLower();

                if (kucukAd.StartsWith("dil_") && (kucukAd.EndsWith(".json") || kucukAd.EndsWith(".txt")))
                {
                    string temizAd = dosyaAdi.Replace("Dil_", "").Replace("dil_", "").Replace(".json", "").Replace(".txt", "").Replace(".JSON", "").Replace(".TXT", "");
                    ToolStripMenuItem item = new ToolStripMenuItem(temizAd);
                    item.Click += (s, e) => { _seciliDil = dosyaAdi; DilMotoru.Yukle(dosyaAdi); TemaVeDiliEkranaBas(); AyarlariKaydet(); };
                    dilMenu.DropDownItems.Add(item);
                }
                else if (kucukAd.StartsWith("tema_") && (kucukAd.EndsWith(".json") || kucukAd.EndsWith(".txt")))
                {
                    string temizAd = dosyaAdi.Replace("Tema_", "").Replace("tema_", "").Replace(".json", "").Replace(".txt", "").Replace(".JSON", "").Replace(".TXT", "");
                    ToolStripMenuItem item = new ToolStripMenuItem(temizAd);
                    item.Click += (s, e) => { _seciliTema = dosyaAdi; TemaMotoru.Yukle(dosyaAdi); TemaVeDiliEkranaBas(); AyarlariKaydet(); };
                    temaMenu.DropDownItems.Add(item);
                }
            }

            if (dilMenu.DropDownItems.Count == 0) dilMenu.DropDownItems.Add(new ToolStripMenuItem(DilMotoru.Cevir(Properties.Settings.Default.MENU_DOSYA_YOK)));
            if (temaMenu.DropDownItems.Count == 0) temaMenu.DropDownItems.Add(new ToolStripMenuItem(DilMotoru.Cevir(Properties.Settings.Default.MENU_DOSYA_YOK)));

            _ayarlarMenu.Items.Add(dilMenu);
            _ayarlarMenu.Items.Add(temaMenu);
            _ayarlarMenu.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem klasorAcMenu = new ToolStripMenuItem("📁 " + DilMotoru.Cevir(Properties.Settings.Default.MENU_KLASOR_AC));
            klasorAcMenu.Click += (s, e) => { System.Diagnostics.Process.Start("explorer.exe", aramaDizini); };
            _ayarlarMenu.Items.Add(klasorAcMenu);

            this.ContextMenuStrip = _ayarlarMenu;
        }

        private void ButunArayuzuCevir(System.Windows.Forms.Control.ControlCollection kontroller)
        {
            foreach (System.Windows.Forms.Control c in kontroller)
            {
                if (_ayarlarMenu != null && !(c is TextBox) && !(c is NumericUpDown) && !(c is RichTextBox)) c.ContextMenuStrip = _ayarlarMenu;
                if (c.Tag == null && !string.IsNullOrWhiteSpace(c.Text) && !(c is TextBox) && !(c is NumericUpDown) && !(c is RichTextBox)) c.Tag = c.Text.Trim();
                if (c.Tag != null) c.Text = DilMotoru.Cevir(c.Tag.ToString());
                if (c.HasChildren) ButunArayuzuCevir(c.Controls);
            }
        }

        private void TemaVeDiliEkranaBas()
        {
            ButunArayuzuCevir(this.Controls);
            this.BackColor = TemaMotoru.TEMA_PASIF;
            Color dinamikYaziRengi = TemaMotoru.TEMA_PASIF.GetBrightness() < 0.5f ? Color.White : Color.Black;
            this.ForeColor = dinamikYaziRengi;

            YaziRenkleriniDerinlemesineUygula(this.Controls, dinamikYaziRengi);

            System.Windows.Forms.Button btnDc = this.Controls.Find("btnDcKalibrasyon", true).FirstOrDefault() as System.Windows.Forms.Button;
            if (btnDc != null)
            {
                btnDc.BackColor = Color.White;
                btnDc.ForeColor = Color.Black;
                btnDc.FlatStyle = FlatStyle.Standard;
                btnDc.Text = DilMotoru.Cevir("DC Offset Kalibrasyonu");
            }

            if (rdoTestModu != null && rdoTestModu.Checked)
            {
                lblTehditDurumu.Text = DilMotoru.Cevir(Properties.Settings.Default.STATUS_MOD_TEST);
                lblTehditDurumu.BackColor = TemaMotoru.TEMA_GUVENLI;
                lblTehditDurumu.ForeColor = Color.White;
            }
            else if (rdoTaarruzModu != null && rdoTaarruzModu.Checked)
            {
                lblTehditDurumu.Text = DilMotoru.Cevir(Properties.Settings.Default.STATUS_MOD_TAARRUZ);
                lblTehditDurumu.BackColor = TemaMotoru.TEMA_UYARI;
                lblTehditDurumu.ForeColor = Color.White;
            }
            else
            {
                lblTehditDurumu.Text = DilMotoru.Cevir(Properties.Settings.Default.STATUS_MOD_DINLEME);
                lblTehditDurumu.BackColor = TemaMotoru.TEMA_DINLEME;
                lblTehditDurumu.ForeColor = Color.White;
            }

            if (btnSaldırı != null && btnSaldırı.Enabled == false)
            {
                btnSaldırı.Text = DilMotoru.Cevir(Properties.Settings.Default.UI_BTN_SALDIRI_BASLAT);
                btnSaldırı.BackColor = Color.Gray;
            }

            if (btnBaglan != null) btnBaglan.Text = _isStreaming ? DilMotoru.Cevir(Properties.Settings.Default.UI_BTN_DURDUR) : DilMotoru.Cevir(Properties.Settings.Default.UI_BTN_BAGLAN);

            EkranModunuAyarla(_ileriModAktif);
            this.Refresh();
        }

        private void YaziRenkleriniDerinlemesineUygula(System.Windows.Forms.Control.ControlCollection kontroller, Color yaziRengi)
        {
            foreach (System.Windows.Forms.Control c in kontroller)
            {
                if (c is GroupBox || c is Panel)
                {
                    c.BackColor = TemaMotoru.TEMA_PASIF;
                    c.ForeColor = yaziRengi;
                }
                else if (c is NumericUpDown || c is TextBox || c is ComboBox || c is RichTextBox)
                {
                    c.BackColor = Color.White;
                    c.ForeColor = Color.Black;
                    if (c is ComboBox cmb) cmb.FlatStyle = FlatStyle.Standard;
                }
                else if (c.ForeColor != Color.LimeGreen && c.ForeColor != Color.Lime && c.Name != "lblTehditDurumu")
                {
                    if (c is RadioButton || c is CheckBox || c is Label) c.ForeColor = yaziRengi;
                }

                if (c.HasChildren) YaziRenkleriniDerinlemesineUygula(c.Controls, yaziRengi);
            }
        }

        private void EkranModunuAyarla(bool ileriModAcik)
        {
            this.SuspendLayout();

            if (ileriModAcik)
            {
                foreach (System.Windows.Forms.Control c in this.Controls)
                {
                    if (c.Left >= picGrafik.Right && (c is GroupBox || c is Button || c is Panel)) c.Visible = true;
                }
                btnModMuhendis.Text = DilMotoru.Cevir(Properties.Settings.Default.UI_BTN_TAKTIK_MODA_DON);
                this.Text = DilMotoru.Cevir(Properties.Settings.Default.UI_MOD_MUHENDIS_BASLIK);
                this.ClientSize = new Size(grpGelismisAyarlar.Right + 20, orjinalFormBoyutu.Height);
            }
            else
            {
                foreach (System.Windows.Forms.Control c in this.Controls)
                {
                    if (c.Left >= picGrafik.Right && (c is GroupBox || c is Button || c is Panel))
                    {
                        if (c.Name != "btnModMuhendis" && c.Name != "btnSaldırı") c.Visible = false;
                    }
                }
                btnModMuhendis.Text = DilMotoru.Cevir(Properties.Settings.Default.UI_BTN_ILERI_MOD_AKTIF);
                this.Text = DilMotoru.Cevir(Properties.Settings.Default.UI_MOD_TAKTIK_BASLIK);

                int yeniGenislik = picGrafik.Right + 18;
                if (btnModMuhendis.Right > yeniGenislik) yeniGenislik = btnModMuhendis.Right + 30;
                this.ClientSize = new Size(yeniGenislik, orjinalFormBoyutu.Height);
            }

            this.ResumeLayout();
            this.CenterToScreen();
        }
        #endregion

        #region 4. DONANIM KONTROLÜ (BLADERF API) VE VERİ AKIŞI
        private async void OtopilotuTetikle()
        {
            await Task.Delay(Properties.Settings.Default.OtopilotBeklemeMs);

            if (!_isDeviceOpen)
            {
                KonsolaYaz(DilMotoru.Cevir(Properties.Settings.Default.LOG_OTOPILOT_BAGLANILIYOR));
                btnBaglan_Click(this, EventArgs.Empty);
                await Task.Delay(Properties.Settings.Default.OtopilotBeklemeMs);
            }

            if (_isDeviceOpen && !_isStreaming)
            {
                KonsolaYaz(DilMotoru.Cevir(Properties.Settings.Default.LOG_OTOPILOT_AKIS_BASLIYOR));
                VeriAkisiniBaslat();
            }
        }

        private async void DinamikParametreUygula()
        {
            if (!_isDeviceOpen || _islemeKilidi || _profilYukleniyor || _otomatikDegisim) return;
            if (_dinamikGuncellemeBekliyor) return;

            _dinamikGuncellemeBekliyor = true;
            await Task.Delay(Properties.Settings.Default.DinamikAyarGecikmesiMs);
            _dinamikGuncellemeBekliyor = false;

            await CihazParametreleriniUygula();
        }

        private async void btnBaglan_Click(object sender, EventArgs e)
        {
            if (_islemeKilidi) return;
            _islemeKilidi = true;

            try
            {
                if (!_isDeviceOpen)
                {
                    KonsolaYaz(DilMotoru.Cevir(Properties.Settings.Default.LOG_BAGLANIYOR));
                    await CihazParametreleriniUygula();

                    if (_isDeviceOpen)
                    {
                        btnBaglan.Text = DilMotoru.Cevir(Properties.Settings.Default.UI_BTN_DURDUR);
                        VeriAkisiniBaslat();
                    }
                    else KonsolaYaz(DilMotoru.Cevir(Properties.Settings.Default.ERR_CIHAZ_BULUNAMADI));
                }
                else
                {
                    if (_isStreaming)
                    {
                        _isStreaming = false;
                        btnBaglan.Text = DilMotoru.Cevir(Properties.Settings.Default.UI_BTN_DEVAM);
                        KonsolaYaz(DilMotoru.Cevir(Properties.Settings.Default.LOG_IZLEME_DURDURULDU));
                    }
                    else
                    {
                        btnBaglan.Text = DilMotoru.Cevir(Properties.Settings.Default.UI_BTN_DURDUR);
                        VeriAkisiniBaslat();
                    }
                }
            }
            catch (Exception ex)
            {
                KonsolaYaz(string.Format(DilMotoru.Cevir(Properties.Settings.Default.LOG_ISLEM_BASARISIZ), ex.Message));
            }
            finally
            {
                _islemeKilidi = false;
            }
        }

        private async Task CihazParametreleriniUygula()
        {
            decimal frekansCarpan = 1, orneklemeCarpan = 1, bantCarpan = 1;

            if (cmbRxBirim.Text.Contains("G")) frekansCarpan = 1000000000m;
            else if (cmbRxBirim.Text.Contains("M")) frekansCarpan = 1000000m;
            else if (cmbRxBirim.Text.Contains("k") || cmbRxBirim.Text.Contains("K")) frekansCarpan = 1000m;

            if (cmbRxOrneklemeBirim.Text.Contains("G")) orneklemeCarpan = 1000000000m;
            else if (cmbRxOrneklemeBirim.Text.Contains("M")) orneklemeCarpan = 1000000m;
            else if (cmbRxOrneklemeBirim.Text.Contains("k") || cmbRxOrneklemeBirim.Text.Contains("K")) orneklemeCarpan = 1000m;

            if (cmbRxBantBirim.Text.Contains("G")) bantCarpan = 1000000000m;
            else if (cmbRxBantBirim.Text.Contains("M")) bantCarpan = 1000000m;
            else if (cmbRxBantBirim.Text.Contains("k") || cmbRxBantBirim.Text.Contains("K")) bantCarpan = 1000m;

            decimal kullaniciFrekans = numRxFrekans.Value * frekansCarpan;
            decimal kullaniciOrnekleme = numRxOrnekleme.Value * orneklemeCarpan;
            decimal kullaniciBant = numRxBantGenisligi.Value * bantCarpan;

            decimal kirpmaYuzdesi = Properties.Settings.Default.BantKirpmaYuzdesi;
            decimal donanimBant = kullaniciBant + (kullaniciBant * kirpmaYuzdesi);

            decimal guvenliOrneklemeAltSiniri = donanimBant * (decimal)Properties.Settings.Default.BolenYari;

            if (kullaniciOrnekleme < guvenliOrneklemeAltSiniri) kullaniciOrnekleme = guvenliOrneklemeAltSiniri;
            if (kullaniciFrekans < (decimal)Properties.Settings.Default.MinFrekansHz) kullaniciFrekans = (decimal)Properties.Settings.Default.MinFrekansHz;
            if (kullaniciFrekans > (decimal)Properties.Settings.Default.MaxFrekansHz) kullaniciFrekans = (decimal)Properties.Settings.Default.MaxFrekansHz;
            if (donanimBant > (decimal)Properties.Settings.Default.MaxBantGenisligi) { donanimBant = (decimal)Properties.Settings.Default.MaxBantGenisligi; kullaniciBant = donanimBant / (1m + kirpmaYuzdesi); }
            if (donanimBant < (decimal)Properties.Settings.Default.MinBantGenisligi) { donanimBant = (decimal)Properties.Settings.Default.MinBantGenisligi; kullaniciBant = donanimBant / (1m + kirpmaYuzdesi); }
            if (kullaniciOrnekleme > (decimal)Properties.Settings.Default.MaxOrneklemeHz) kullaniciOrnekleme = (decimal)Properties.Settings.Default.MaxOrneklemeHz;

            _otomatikDegisim = true;
            UI_LimitleriUygula();

            GuvenliAta(numRxFrekans, kullaniciFrekans / frekansCarpan);
            GuvenliAta(numRxBantGenisligi, kullaniciBant / bantCarpan);
            GuvenliAta(numRxOrnekleme, kullaniciOrnekleme / orneklemeCarpan);

            _otomatikDegisim = false;

            if (!_isDeviceOpen)
            {
                try
                {
                    if (BladeRFBridge.bladerf_open(out _devicePointer, IntPtr.Zero) == 0 && _devicePointer != IntPtr.Zero) _isDeviceOpen = true;
                    else return;
                }
                catch { return; }
            }

            bool akisVardi = _isStreaming;
            if (akisVardi)
            {
                _isStreaming = false;
                while (_rxIpligiAktif) await Task.Delay(Properties.Settings.Default.KapanisBeklemeMs);
            }

            try
            {
                BladeRFBridge.bladerf_set_frequency(_devicePointer, 0, (ulong)kullaniciFrekans);
                BladeRFBridge.bladerf_set_sample_rate(_devicePointer, 0, (uint)kullaniciOrnekleme, out uint actualSR);
                BladeRFBridge.bladerf_set_bandwidth(_devicePointer, 0, (uint)donanimBant, out uint actualBW);

                donanimGercekOrnekleme = actualSR;
                KonsolaYaz(DilMotoru.Cevir(Properties.Settings.Default.LOG_PARAMETRE_UYGULANDI) + $" {actualSR} Hz");
            }
            catch (Exception ex)
            {
                KonsolaYaz(DilMotoru.Cevir(Properties.Settings.Default.ERR_DONANIM_HATASI) + $" {ex.Message}");
            }

            if (akisVardi) VeriAkisiniBaslat();
        }

        private void VeriAkisiniBaslat()
        {
            if (!_isDeviceOpen || _isStreaming) return;

            _isStreaming = true;
            KonsolaYaz(DilMotoru.Cevir(Properties.Settings.Default.LOG_CANLI_DINLEME));

            uint num_samples = (uint)Properties.Settings.Default.RxBufferBoyutu;

            BladeRFBridge.bladerf_sync_config(_devicePointer, 0, 0, (uint)Properties.Settings.Default.RxNumBuffers, num_samples, (uint)Properties.Settings.Default.RxNumTransfers, (uint)Properties.Settings.Default.RxZamanAsimiMs);
            BladeRFBridge.bladerf_enable_module(_devicePointer, 0, true);

            _rxIpligiAktif = true;

            Task.Run(async () =>
            {
                _okumaIslemiTamamlandi = false;
                short[] iqData = new short[num_samples * 2];

                if (sonIqHafizasi == null || sonIqHafizasi.Length != iqData.Length)
                    sonIqHafizasi = new short[iqData.Length];

                try
                {
                    while (_isStreaming)
                    {
                        int rxStatus = BladeRFBridge.bladerf_sync_rx(_devicePointer, iqData, num_samples, IntPtr.Zero, (uint)Properties.Settings.Default.RxZamanAsimiMs);

                        if (rxStatus == 0 && _isStreaming)
                        {
                            if (!_cizimMesgul)
                            {
                                Buffer.BlockCopy(iqData, 0, sonIqHafizasi, 0, iqData.Length * 2);
                                GrafikGuncelle();
                            }
                        }
                        else if (rxStatus != 0 && _isStreaming)
                        {
                            _isStreaming = false;

                            this.Invoke((MethodInvoker)delegate {
                                btnBaglan.Text = DilMotoru.Cevir(Properties.Settings.Default.UI_BTN_BAGLAN);
                            });

                            KonsolaYaz($"[RX HATASI] Okuma Durdu. Kod: {rxStatus}");
                            break;
                        }

                        await Task.Delay(1);
                    }
                }
                finally
                {
                    try { BladeRFBridge.bladerf_enable_module(_devicePointer, 0, false); } catch { }
                    _rxIpligiAktif = false;
                    _okumaIslemiTamamlandi = true;
                }
            });
        }
        private void OperasyonModu_Degisimi(object sender, EventArgs e)
        {
            RadioButton secilenButon = sender as RadioButton;
            if (secilenButon == null || !secilenButon.Checked) return;

            _saldiriAktif = false;
            int hedefLoopback = 0;

            if (secilenButon.Name == "rdoTestModu")
            {
                hedefLoopback = 2;
                lblTehditDurumu.Text = DilMotoru.Cevir(Properties.Settings.Default.STATUS_MOD_TEST);
                lblTehditDurumu.BackColor = TemaMotoru.TEMA_GUVENLI;
                if (btnSaldırı != null)
                {
                    btnSaldırı.Enabled = true;
                    btnSaldırı.BackColor = Color.Orange;
                    btnSaldırı.ForeColor = Color.Black;
                    btnSaldırı.Text = DilMotoru.Cevir(Properties.Settings.Default.UI_BTN_TEST_GONDER);
                }
            }
            else if (secilenButon.Name == "rdoTaarruzModu")
            {
                hedefLoopback = 0;
                lblTehditDurumu.Text = DilMotoru.Cevir(Properties.Settings.Default.STATUS_MOD_TAARRUZ);
                lblTehditDurumu.BackColor = TemaMotoru.TEMA_UYARI;
                if (btnSaldırı != null)
                {
                    btnSaldırı.Enabled = true;
                    btnSaldırı.BackColor = TemaMotoru.TEMA_TAARRUZ_AKTIF;
                    btnSaldırı.ForeColor = Color.White;
                    btnSaldırı.Text = DilMotoru.Cevir(Properties.Settings.Default.UI_BTN_SALDIRI_BASLAT);
                }
            }
            else
            {
                hedefLoopback = 0;
                lblTehditDurumu.Text = DilMotoru.Cevir(Properties.Settings.Default.STATUS_MOD_DINLEME);
                lblTehditDurumu.BackColor = TemaMotoru.TEMA_DINLEME;
                if (btnSaldırı != null)
                {
                    btnSaldırı.Enabled = false;
                    btnSaldırı.BackColor = Color.Gray;
                    btnSaldırı.ForeColor = Color.White;
                    btnSaldırı.Text = DilMotoru.Cevir(Properties.Settings.Default.UI_BTN_SALDIRI_BASLAT);
                }
            }

            lblTehditDurumu.ForeColor = Color.White;
            lblTehditDurumu.Update();
            if (btnSaldırı != null) btnSaldırı.Update();

            if (_mevcutLoopbackModu != hedefLoopback && _isDeviceOpen && _devicePointer != IntPtr.Zero)
            {
                bool arkaPlandaAkiyordu = _isStreaming;
                Task.Run(async () =>
                {
                    try
                    {
                        if (arkaPlandaAkiyordu)
                        {
                            _isStreaming = false;
                            int bekleme = 0;
                            while (!_okumaIslemiTamamlandi && bekleme < Properties.Settings.Default.ModGecisZamanAsimiIterasyon)
                            {
                                await Task.Delay(Properties.Settings.Default.ModGecisBeklemeMs);
                                bekleme++;
                            }
                        }

                        BladeRFBridge.bladerf_set_loopback(_devicePointer, hedefLoopback);
                        _mevcutLoopbackModu = hedefLoopback;

                        if (arkaPlandaAkiyordu)
                        {
                            await Task.Delay(Properties.Settings.Default.TxRxGecisSuresiMs);
                            this.Invoke((MethodInvoker)delegate { if (!_isStreaming) VeriAkisiniBaslat(); });
                        }
                    }
                    catch (Exception ex) { KonsolaYaz(Properties.Settings.Default.ERR_GECIS_HATASI + ex.Message); }
                });
            }
        }

        private void btnSaldırı_Click_1(object sender, EventArgs e)
        {
            if (!_isDeviceOpen || _devicePointer == IntPtr.Zero) return;

            if (!_saldiriAktif)
            {
                ulong txFrekans = 0;
                uint txBant = 0;
                uint rxBant = 0;
                int txKazanc = trbTxGain != null ? trbTxGain.Value : 40;

                try
                {
                    decimal txFrekansCarpani = cmbTxBirim.Text.Contains("G") ? 1000000000m : (cmbTxBirim.Text.Contains("M") ? 1000000m : 1000m);
                    txFrekans = (ulong)(numTxFrekans.Value * txFrekansCarpani);

                    decimal txBantCarpani = cmbTxBantBirim.Text.Contains("G") ? 1000000000m : (cmbTxBantBirim.Text.Contains("M") ? 1000000m : 1000m);
                    txBant = (uint)(numTxBantGenisligi.Value * txBantCarpani);

                    decimal rxBantCarpani = cmbRxBantBirim.Text.Contains("G") ? 1000000000m : (cmbRxBantBirim.Text.Contains("M") ? 1000000m : 1000m);
                    rxBant = (uint)(numRxBantGenisligi.Value * rxBantCarpani);
                }
                catch { return; }

                _saldiriAktif = true;
                btnSaldırı.Text = "SİNYALİ KES";
                btnSaldırı.BackColor = TemaMotoru.TEMA_TAARRUZ_AKTIF;
                btnSaldırı.ForeColor = Color.White;
                lblTehditDurumu.Text = "TAARRUZ AKTİF!";
                lblTehditDurumu.BackColor = TemaMotoru.TEMA_TAARRUZ_AKTIF;
                KonsolaYaz($"[SİSTEM] ANA SİLAH: Orijinal 24 Ağustos Kare Dalga (Tarak) Başlatıldı!");

                Task.Run(async () =>
                {
                    try
                    {
                        _isStreaming = false;
                        await Task.Delay(200);
                        BladeRFBridge.bladerf_enable_module(_devicePointer, 0, false);
                        BladeRFBridge.bladerf_enable_module(_devicePointer, 1, false);

                        BladeRFBridge.bladerf_set_frequency(_devicePointer, 1, txFrekans);
                        BladeRFBridge.bladerf_set_frequency(_devicePointer, 0, txFrekans);

                        uint hedefHiz = donanimGercekOrnekleme > 0 ? donanimGercekOrnekleme : 16440000u;
                        BladeRFBridge.bladerf_set_sample_rate(_devicePointer, 0, hedefHiz, out uint gercekRx);
                        BladeRFBridge.bladerf_set_sample_rate(_devicePointer, 1, hedefHiz, out uint gercekTx);
                        donanimGercekOrnekleme = gercekRx;

                        BladeRFBridge.bladerf_set_bandwidth(_devicePointer, 0, (rxBant > 0 ? rxBant : hedefHiz), out uint _);
                        BladeRFBridge.bladerf_set_bandwidth(_devicePointer, 1, txBant, out uint _);

                        BladeRFBridge.bladerf_set_gain(_devicePointer, 1, txKazanc);

                        // 🚀 24 AĞUSTOS'TAKİ KUSURSUZ AYARLARIN
                        uint donanim_buffer = 8192u;
                        uint timeout_ms = 1000u;

                        BladeRFBridge.bladerf_sync_config(_devicePointer, 0, 0, 16u, donanim_buffer, 8u, timeout_ms);
                        BladeRFBridge.bladerf_sync_config(_devicePointer, 1, 0, 16u, donanim_buffer, 8u, timeout_ms);

                        BladeRFBridge.bladerf_enable_module(_devicePointer, 0, true);
                        BladeRFBridge.bladerf_enable_module(_devicePointer, 1, true);

                        _isStreaming = true;
                        _rxIpligiAktif = true;
                        _ = Task.Run(() =>
                        {
                            _okumaIslemiTamamlandi = false;
                            short[] iqData = new short[donanim_buffer * 2];
                            if (sonIqHafizasi == null || sonIqHafizasi.Length != iqData.Length) sonIqHafizasi = new short[iqData.Length];
                            try
                            {
                                while (_isStreaming)
                                {
                                    int rxStatus = BladeRFBridge.bladerf_sync_rx(_devicePointer, iqData, donanim_buffer, IntPtr.Zero, timeout_ms);
                                    if (rxStatus == 0 && _isStreaming && !_cizimMesgul)
                                    {
                                        Buffer.BlockCopy(iqData, 0, sonIqHafizasi, 0, iqData.Length * 2);
                                        GrafikGuncelle();
                                    }
                                }
                            }
                            finally
                            {
                                _rxIpligiAktif = false;
                                _okumaIslemiTamamlandi = true;
                            }
                        });

                        // ====================================================================
                        // 🚀 24 AĞUSTOS'UN GERÇEK SIRRI: İLKEL VE VAHŞİ KARE DALGA
                        // ====================================================================
                        short[] testSinyali = new short[donanim_buffer * 2];

                        // O sihirli değerleri doğrudan kullanıyoruz!
                        short genlik = 30000;
                        int periyot = 64;

                        // Not: İstersen yukarıdaki iki satırı şu şekilde kendi Settings dosyana bağlayabilirsin:
                        // short genlik = (short)Properties.Settings.Default.SinyalGenligi;
                        // int periyot = Properties.Settings.Default.KareDalgaPeriyodu;

                        for (int i = 0; i < donanim_buffer; i++)
                        {
                            // İntegral yok, faz hesaplama yok! Sadece periyodun yarısı +30000, yarısı -30000.
                            // Bu ilkel çarpma, DAC'den çıkarken senin çizdiğin o 15 dişi kusursuzca yaratır.
                            short val = (short)((i % periyot) < (periyot / 2) ? genlik : -genlik);

                            testSinyali[i * 2] = val;       // I Kanalı
                            testSinyali[i * 2 + 1] = val;   // Q Kanalı
                        }

                        // ATEŞLE!
                        while (_saldiriAktif)
                        {
                            int txStatus = BladeRFBridge.bladerf_sync_tx(_devicePointer, testSinyali, donanim_buffer, IntPtr.Zero, timeout_ms);
                            if (txStatus != 0 && txStatus != -7)
                            {
                                KonsolaYaz($"[TX HATASI] Kod: {txStatus}");
                                break;
                            }
                        }
                    }
                    catch (Exception ex) { KonsolaYaz($"[SİSTEM KRİZİ] {ex.Message}"); }
                    finally
                    {
                        _saldiriAktif = false;
                        try { BladeRFBridge.bladerf_enable_module(_devicePointer, 1, false); } catch { }
                    }
                });
            }
            else
            {
                _saldiriAktif = false;
                _isStreaming = false;
                btnSaldırı.Text = "SALDIRI BAŞLAT";
                btnSaldırı.BackColor = TemaMotoru.TEMA_TAARRUZ_AKTIF;
                KonsolaYaz("[TX KAPATILDI] Gönderim kesildi. Beklemede.");
            }
        }
        private async void btnDcKalibrasyon_Click(object sender, EventArgs e)
        {
            if (!_isDeviceOpen || _devicePointer == IntPtr.Zero)
            {
                MessageBox.Show(DilMotoru.Cevir(Properties.Settings.Default.MSG_CIHAZ_BAGLI_DEGIL), DilMotoru.Cevir(Properties.Settings.Default.MSG_BAGLANTI_HATASI_BASLIK), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Button kalibrasyonButonu = sender as Button;
            string eskiYazi = kalibrasyonButonu.Text;
            Color eskiRenk = kalibrasyonButonu.BackColor;

            kalibrasyonButonu.Enabled = false;
            kalibrasyonButonu.Text = DilMotoru.Cevir(Properties.Settings.Default.UI_BTN_KALIBRE_EDILIYOR);
            kalibrasyonButonu.BackColor = Color.Orange;
            kalibrasyonButonu.ForeColor = Color.Black;

            KonsolaYaz(DilMotoru.Cevir(Properties.Settings.Default.LOG_KALIBRASYON_BASLADI));

            await Task.Run(() =>
            {
                try
                {
                    int sonuc = BladeRFBridge.bladerf_calibrate_dc(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX);
                    System.Threading.Thread.Sleep(Properties.Settings.Default.KalibrasyonBeklemeMs);
                }
                catch (Exception ex) { KonsolaYaz(Properties.Settings.Default.ERR_KALIBRASYON_HATASI + ex.Message); }
            });

            kalibrasyonButonu.Enabled = true;
            kalibrasyonButonu.Text = eskiYazi;
            kalibrasyonButonu.BackColor = eskiRenk;
            kalibrasyonButonu.ForeColor = Color.Black;
            KonsolaYaz(DilMotoru.Cevir(Properties.Settings.Default.LOG_KALIBRASYON_BITTI));
        }

        private void GrafikGuncelle()
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;
            if (_cizimMesgul) return;
            _cizimMesgul = true;

            try
            {
                int pictureBoxWidth = 100;
                int pictureBoxHeight = 100;
                bool alarmAktif = false;
                bool gurultuEngelleAktif = false;

                double dinamikTehditEsigi = Properties.Settings.Default.VarsayilanTehditEsigiDb;
                float max_dB = (float)Properties.Settings.Default.GrafikVarsayilanMaxDb;
                float min_dB = (float)Properties.Settings.Default.GrafikVarsayilanMinDb;
                double gurultuEsigi = Properties.Settings.Default.VarsayilanGurultuEsigiDb;
                double gosterilecekBantHz = Properties.Settings.Default.VarsayilanBantHz;

                double merkezFrekansHz = (double)Properties.Settings.Default.MinFrekansHz;

                this.Invoke((MethodInvoker)delegate
                {
                    if (picGrafik.Width > 0 && picGrafik.Height > 0)
                    {
                        pictureBoxWidth = picGrafik.Width;
                        pictureBoxHeight = picGrafik.Height;
                    }

                    if (chkAlarmAktif != null) alarmAktif = chkAlarmAktif.Checked;
                    if (chkGurultuEngelle != null) gurultuEngelleAktif = chkGurultuEngelle.Checked;
                    if (trbSquelch != null) dinamikTehditEsigi = trbSquelch.Value;
                    if (numYMax != null) max_dB = (float)numYMax.Value;
                    if (numYMin != null) min_dB = (float)numYMin.Value;
                    if (numGurultuEsigi != null) gurultuEsigi = (double)numGurultuEsigi.Value;

                    double bCarpan = 1;
                    if (cmbRxBantBirim.Text.Contains("G")) bCarpan = 1000000000d;
                    else if (cmbRxBantBirim.Text.Contains("M")) bCarpan = 1000000d;
                    else if (cmbRxBantBirim.Text.Contains("k") || cmbRxBantBirim.Text.Contains("K")) bCarpan = 1000d;
                    gosterilecekBantHz = (double)numRxBantGenisligi.Value * bCarpan;

                    double fCarpan = 1;
                    if (cmbRxBirim.Text.Contains("G")) fCarpan = 1000000000d;
                    else if (cmbRxBirim.Text.Contains("M")) fCarpan = 1000000d;
                    else if (cmbRxBirim.Text.Contains("k") || cmbRxBirim.Text.Contains("K")) fCarpan = 1000d;
                    merkezFrekansHz = (double)numRxFrekans.Value * fCarpan;
                });

                double gercekSR = donanimGercekOrnekleme > 0 ? donanimGercekOrnekleme : Properties.Settings.Default.VarsayilanOrneklemeHz;
                if (gosterilecekBantHz > gercekSR) gosterilecekBantHz = gercekSR;
                double bolenYari = Properties.Settings.Default.BolenYari > 0 ? Properties.Settings.Default.BolenYari : 2;
                double solKenarFrekans = merkezFrekansHz - (gosterilecekBantHz / bolenYari);

                short[] anlikIq = sonIqHafizasi;
                int num_samples = 0;

                if (anlikIq != null) num_samples = anlikIq.Length / 2;

                Bitmap tuval = new Bitmap(pictureBoxWidth, pictureBoxHeight);
                float dB_farki = max_dB - min_dB;
                if (dB_farki <= 0) dB_farki = 1f;

                using (Graphics g = Graphics.FromImage(tuval))
                {
                    g.Clear(Color.Black);

                    using (Pen gridKalem = new Pen(Color.FromArgb(40, 40, 40), 1f))
                    using (Font eksenFontu = new Font("Arial", 8))
                    using (SolidBrush yaziFircasi = new SolidBrush(Color.LightGray))
                    {
                        int yatayKareSayisi = Properties.Settings.Default.GridYatayKareSayisi > 0 ? Properties.Settings.Default.GridYatayKareSayisi : 10;
                        float sutunGenisligi = (float)tuval.Width / yatayKareSayisi;
                        for (int i = 0; i <= yatayKareSayisi; i++)
                        {
                            float x = i * sutunGenisligi;
                            g.DrawLine(gridKalem, x, 0, x, tuval.Height);
                        }

                        int dikeyKareSayisi = Properties.Settings.Default.GridDikeyKareSayisi > 0 ? Properties.Settings.Default.GridDikeyKareSayisi : 10;
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
                        g.DrawString(DilMotoru.Cevir(Properties.Settings.Default.UI_GRAFIK_ALT_FREKANSLAR), eksenFontu, Brushes.DarkGray, 15, tuval.Height - 40);
                        g.DrawString(DilMotoru.Cevir(Properties.Settings.Default.UI_GRAFIK_UST_FREKANSLAR), eksenFontu, Brushes.DarkGray, tuval.Width - 115, tuval.Height - 40);
                    }

                    if (num_samples > 1)
                    {
                        System.Numerics.Complex[] fftVerisi = new System.Numerics.Complex[num_samples];

                        for (int i = 0; i < num_samples; i++)
                        {
                            if (dcKalibrasyonTetiklendi)
                            {
                                double i_toplam = 0, q_toplam = 0;
                                for (int k = 0; k < num_samples; k++)
                                {
                                    i_toplam += anlikIq[k * 2];
                                    q_toplam += anlikIq[k * 2 + 1];
                                }
                                i_offset_degeri = i_toplam / num_samples;
                                q_offset_degeri = q_toplam / num_samples;
                                dcKalibrasyonTetiklendi = false;
                            }

                            double temiz_I = anlikIq[i * 2] - i_offset_degeri;
                            double temiz_Q = anlikIq[i * 2 + 1] - q_offset_degeri;
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

                        using (Pen cizgiKalemi = new Pen(gurultuEngelleAktif ? Color.LimeGreen : Color.Cyan, 1.5f))
                        {
                            int yariUzunluk = num_samples / 2;
                            PointF[] sinyalNoktalari = new PointF[tuval.Width];

                            double soldanKirpilacakOran = ((gercekSR - gosterilecekBantHz) / bolenYari) / gercekSR;
                            double gosterilecekOran = gosterilecekBantHz / gercekSR;
                            double baslangicNoktasi = (num_samples - 1) * soldanKirpilacakOran;
                            double gosterilecekAralik = (num_samples - 1) * gosterilecekOran;

                            for (int pixelX = 0; pixelX < tuval.Width; pixelX++)
                            {
                                double i_baslangic = baslangicNoktasi + (((double)pixelX / Math.Max(1, tuval.Width - 1)) * gosterilecekAralik);
                                double i_bitis = baslangicNoktasi + (((double)(pixelX + 1) / Math.Max(1, tuval.Width - 1)) * gosterilecekAralik);

                                int idx_baslangic = (int)Math.Floor(i_baslangic);
                                int idx_bitis = (int)Math.Ceiling(i_bitis);

                                if (idx_bitis >= num_samples) idx_bitis = num_samples - 1;
                                if (idx_baslangic > idx_bitis) idx_baslangic = idx_bitis;

                                double maxDbPixelIcin = -999;
                                for (int i = idx_baslangic; i <= idx_bitis; i++)
                                {
                                    int shiftedIndex = (i + yariUzunluk) % num_samples;
                                    double mag = (fftVerisi[shiftedIndex].Magnitude / Properties.Settings.Default.FftOlcekBoleni) + Properties.Settings.Default.MathLogSifirKorumasi;
                                    if (mag < 0.000001) mag = 0.000001;
                                    double db = Properties.Settings.Default.DbHesapCarpani * Math.Log10(mag);
                                    if (db > maxDbPixelIcin) maxDbPixelIcin = db;
                                }

                                double islenecekDb = maxDbPixelIcin;

                                if (gurultuEngelleAktif && islenecekDb < gurultuEsigi) islenecekDb = min_dB + 1.0;

                                // 🚀 MÜHENDİSLİK ÇÖZÜMÜ: Peak-Hold (Hızlı Saldırı, Yavaş Sönümleme)
                                // Eğer gelen yeni sinyal (Jammer) ekrandaki çizimden daha güçlüyse, 
                                // filtreyi tamamen by-pass edip mızrağı ANINDA ekrana saplıyoruz!
                                if (islenecekDb > yumusatilmisFFT[pixelX])
                                {
                                    yumusatilmisFFT[pixelX] = islenecekDb;
                                }
                                else
                                {
                                    // Sinyal kesildiğinde veya düştüğünde yavaşça sönümle (Yumuşatma burada çalışsın)
                                    yumusatilmisFFT[pixelX] = (alpha * islenecekDb) + ((1 - alpha) * yumusatilmisFFT[pixelX]);
                                }

                                double filtrelenmisDb = yumusatilmisFFT[pixelX];
                                if (filtrelenmisDb > anlikMaksimumGenlik) anlikMaksimumGenlik = filtrelenmisDb;

                                float y = ((max_dB - (float)filtrelenmisDb) / dB_farki) * tuval.Height;
                                if (float.IsNaN(y) || float.IsInfinity(y)) y = tuval.Height;
                                else if (y < 0) y = 0;
                                else if (y > tuval.Height) y = tuval.Height;

                                sinyalNoktalari[pixelX] = new PointF(pixelX, y);
                            }

                            g.DrawLines(cizgiKalemi, sinyalNoktalari);
                        }

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            if (alarmAktif)
                            {
                                if (anlikMaksimumGenlik > dinamikTehditEsigi)
                                {
                                    tehditSayaci++;
                                    if (tehditSayaci >= Properties.Settings.Default.GerekliSureklilik)
                                    {
                                        lblTehditDurumu.Text = string.Format(DilMotoru.Cevir(Properties.Settings.Default.STATUS_SINYAL_BASILIYOR), anlikMaksimumGenlik, dinamikTehditEsigi);
                                        lblTehditDurumu.BackColor = TemaMotoru.TEMA_TAARRUZ_AKTIF;
                                        lblTehditDurumu.ForeColor = Color.White;
                                        if (btnSaldırı != null) { btnSaldırı.Enabled = true; btnSaldırı.BackColor = TemaMotoru.TEMA_TAARRUZ_AKTIF; }
                                    }
                                }
                                else
                                {
                                    if (rdoTaarruzModu != null && rdoTaarruzModu.Checked) return;
                                    if (rdoTestModu != null && rdoTestModu.Checked) return;
                                    tehditSayaci = 0;
                                    lblTehditDurumu.Text = DilMotoru.Cevir(Properties.Settings.Default.STATUS_MOD_DINLEME);
                                    lblTehditDurumu.BackColor = TemaMotoru.TEMA_DINLEME;
                                    lblTehditDurumu.ForeColor = Color.White;
                                    if (btnSaldırı != null) { btnSaldırı.Enabled = false; btnSaldırı.BackColor = TemaMotoru.TEMA_PASIF; }
                                }
                            }
                            else
                            {
                                if (rdoTaarruzModu != null && rdoTaarruzModu.Checked) return;
                                if (rdoTestModu != null && rdoTestModu.Checked) return;
                                tehditSayaci = 0;
                                lblTehditDurumu.Text = DilMotoru.Cevir(Properties.Settings.Default.STATUS_ALARM_KAPALI);
                                lblTehditDurumu.BackColor = TemaMotoru.TEMA_PASIF;
                                lblTehditDurumu.ForeColor = Color.White;
                                if (btnSaldırı != null) { btnSaldırı.Enabled = false; btnSaldırı.BackColor = TemaMotoru.TEMA_PASIF; }
                            }
                        });
                    }
                }

                this.BeginInvoke((MethodInvoker)delegate
                {
                    try
                    {
                        Image eskiResim = picGrafik.Image;
                        picGrafik.Image = tuval;
                        if (eskiResim != null) eskiResim.Dispose();
                        picGrafik.Invalidate();
                    }
                    catch { }
                });
            }
            catch
            {
            }
            finally
            {
                _cizimMesgul = false;
            }
        }

        private double FareX_To_Frekans(int fareX, int width)
        {
            double fCarpan = 1;
            if (cmbRxBirim.Text.Contains("G")) fCarpan = 1000000000d;
            else if (cmbRxBirim.Text.Contains("M")) fCarpan = 1000000d;
            else if (cmbRxBirim.Text.Contains("k") || cmbRxBirim.Text.Contains("K")) fCarpan = 1000d;
            double merkezFrekansHz = (double)numRxFrekans.Value * fCarpan;

            double bCarpan = 1;
            if (cmbRxBantBirim.Text.Contains("G")) bCarpan = 1000000000d;
            else if (cmbRxBantBirim.Text.Contains("M")) bCarpan = 1000000d;
            else if (cmbRxBantBirim.Text.Contains("k") || cmbRxBantBirim.Text.Contains("K")) bCarpan = 1000d;
            double gosterilecekBantHz = (double)numRxBantGenisligi.Value * bCarpan;

            double gercekSR = donanimGercekOrnekleme > 0 ? (double)donanimGercekOrnekleme : Properties.Settings.Default.VarsayilanOrneklemeHz;
            if (gosterilecekBantHz > gercekSR) gosterilecekBantHz = gercekSR;

            double solKenarFrekans = merkezFrekansHz - (gosterilecekBantHz / Properties.Settings.Default.BolenYari);
            double ekranOrani = (double)fareX / width;

            return solKenarFrekans + (ekranOrani * gosterilecekBantHz);
        }

        private void picGrafik_Paint(object sender, PaintEventArgs e)
        {
            if (chkMarkerAktif.Checked && hedefMarkerFrekansHz > 0)
            {
                double fCarpan = 1;
                if (cmbRxBirim.Text.Contains("G")) fCarpan = 1000000000d;
                else if (cmbRxBirim.Text.Contains("M")) fCarpan = 1000000d;
                else if (cmbRxBirim.Text.Contains("k") || cmbRxBirim.Text.Contains("K")) fCarpan = 1000d;
                double merkezFrekansHz = (double)numRxFrekans.Value * fCarpan;

                double bCarpan = 1;
                if (cmbRxBantBirim.Text.Contains("G")) bCarpan = 1000000000d;
                else if (cmbRxBantBirim.Text.Contains("M")) bCarpan = 1000000d;
                else if (cmbRxBantBirim.Text.Contains("k") || cmbRxBantBirim.Text.Contains("K")) bCarpan = 1000d;
                double gosterilecekBantHz = (double)numRxBantGenisligi.Value * bCarpan;

                double gercekSR = donanimGercekOrnekleme > 0 ? donanimGercekOrnekleme : Properties.Settings.Default.VarsayilanOrneklemeHz;
                if (gosterilecekBantHz > gercekSR) gosterilecekBantHz = gercekSR;

                double solKenarFrekans = merkezFrekansHz - (gosterilecekBantHz / Properties.Settings.Default.BolenYari);
                double sagKenarFrekans = merkezFrekansHz + (gosterilecekBantHz / Properties.Settings.Default.BolenYari);

                if (hedefMarkerFrekansHz >= solKenarFrekans && hedefMarkerFrekansHz <= sagKenarFrekans)
                {
                    float oran = (float)((hedefMarkerFrekansHz - solKenarFrekans) / gosterilecekBantHz);
                    float markerPixelX = oran * picGrafik.Width;

                    using (Pen markerKalemi = new Pen(Color.Yellow, 2f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                    using (Font markerFontu = new Font("Consolas", 10, FontStyle.Bold))
                    {
                        e.Graphics.DrawLine(markerKalemi, markerPixelX, 0, markerPixelX, picGrafik.Height);
                        string markerYazi = string.Format(DilMotoru.Cevir(Properties.Settings.Default.UI_MARKER_METIN), (hedefMarkerFrekansHz / Properties.Settings.Default.CarpanMega));

                        SizeF textSize = e.Graphics.MeasureString(markerYazi, markerFontu);
                        float yaziX = markerPixelX + 5;

                        if (markerPixelX + textSize.Width + 10 > picGrafik.Width)
                        {
                            yaziX = markerPixelX - textSize.Width - 5;
                        }

                        if (yaziX < 5) yaziX = 5;

                        e.Graphics.DrawString(markerYazi, markerFontu, Brushes.Yellow, yaziX, 20);
                    }
                }
            }
        }

        private void AyarlariYukle()
        {
            try
            {
                if (File.Exists(Properties.Settings.Default.DOSYA_AYARLAR))
                {
                    string jsonMetni = File.ReadAllText(Properties.Settings.Default.DOSYA_AYARLAR);
                    AyarModeli ayarlar = JsonSerializer.Deserialize<AyarModeli>(jsonMetni);

                    if (ayarlar != null)
                    {
                        _seciliDil = ayarlar.SeciliDil ?? Properties.Settings.Default.VARSAYILAN_DIL;
                        _seciliTema = ayarlar.SeciliTema ?? Properties.Settings.Default.VARSAYILAN_TEMA;

                        TemaMotoru.Yukle(_seciliTema);
                        DilMotoru.Yukle(_seciliDil);
                        TemaVeDiliEkranaBas();

                        ozelProfiller = ayarlar.OzelProfiller ?? new Dictionary<string, double[]>();

                        int sq = ayarlar.Squelch;
                        if (sq < trbSquelch.Minimum) sq = trbSquelch.Minimum;
                        if (sq > trbSquelch.Maximum) sq = trbSquelch.Maximum;
                        trbSquelch.Value = sq;
                        if (lblSqulechTehtid != null) lblSqulechTehtid.Text = string.Format(Properties.Settings.Default.FORMAT_YUZDE, trbSquelch.Value);

                        int ym = ayarlar.Yumusatma;
                        if (ym < trbYumusatma.Minimum) ym = trbYumusatma.Minimum;
                        if (ym > trbYumusatma.Maximum) ym = trbYumusatma.Maximum;
                        trbYumusatma.Value = ym;
                        alpha = ym / Properties.Settings.Default.YuzdeBolen;
                        if (lblYumusatmaDegeri != null) lblYumusatmaDegeri.Text = string.Format(Properties.Settings.Default.FORMAT_YUZDE, trbYumusatma.Value);

                        int th = ayarlar.TaramaHizi;
                        if (th < trbTaramaHizi.Minimum) th = trbTaramaHizi.Minimum;
                        if (th > trbTaramaHizi.Maximum) th = trbTaramaHizi.Maximum;
                        trbTaramaHizi.Value = th;
                        taramaGecikmesi = th;
                        if (lblTaramaHiziDegeri != null) lblTaramaHiziDegeri.Text = string.Format(Properties.Settings.Default.FORMAT_MS, trbTaramaHizi.Value);

                        foreach (var profil in ozelProfiller.Keys)
                        {
                            if (!cmbHedefProfilleri.Items.Contains(profil)) cmbHedefProfilleri.Items.Add(profil);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KonsolaYaz(string.Format(DilMotoru.Cevir(Properties.Settings.Default.LOG_ISLEM_BASARISIZ), ex.Message));
            }
        }

        private void AyarlariKaydet()
        {
            try
            {
                AyarModeli ayarlar = new AyarModeli
                {
                    OzelProfiller = this.ozelProfiller,
                    Squelch = trbSquelch.Value,
                    Yumusatma = trbYumusatma.Value,
                    TaramaHizi = trbTaramaHizi.Value,
                    SeciliDil = _seciliDil,
                    SeciliTema = _seciliTema
                };

                JsonSerializerOptions secenekler = new JsonSerializerOptions { WriteIndented = true };
                string jsonMetni = JsonSerializer.Serialize(ayarlar, secenekler);
                File.WriteAllText(Properties.Settings.Default.DOSYA_AYARLAR, jsonMetni);
            }
            catch (Exception ex)
            {
                MessageBox.Show(DilMotoru.Cevir(Properties.Settings.Default.ERR_AYAR_KAYDEDILEMEDI) + ex.Message, DilMotoru.Cevir(Properties.Settings.Default.ERR_KAYIT_BASLIK), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string PromptGoster(string metin, string baslik)
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 160,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = baslik,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                MaximizeBox = false,
                MinimizeBox = false
            };
            Label lbl = new Label() { Left = 20, Top = 20, Text = metin, AutoSize = true };
            TextBox box = new TextBox() { Left = 20, Top = 50, Width = 340, BackColor = Color.FromArgb(50, 50, 50), ForeColor = Color.LimeGreen, Font = new Font("Consolas", 10) };
            Button btnOnay = new Button() { Text = DilMotoru.Cevir(Properties.Settings.Default.UI_BTN_KAYDET), Left = 260, Width = 100, Top = 85, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(45, 48, 54) };

            btnOnay.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(lbl); prompt.Controls.Add(box); prompt.Controls.Add(btnOnay);
            prompt.AcceptButton = btnOnay;

            prompt.ShowDialog();
            return box.Text.Trim();
        }

        private void TehditRaporlaCSV(double merkezFrekans, double sinyalGucu, double tehditEsigi, double bantGenisligi, double ornekleme)
        {
            try
            {
                bool dosyaIlkKezMiOlusturuluyor = !File.Exists(Properties.Settings.Default.DOSYA_LOG_CSV);
                using (StreamWriter sw = new StreamWriter(Properties.Settings.Default.DOSYA_LOG_CSV, true, System.Text.Encoding.UTF8))
                {
                    if (dosyaIlkKezMiOlusturuluyor) sw.WriteLine(Properties.Settings.Default.CSV_BASLIK_SATIRI);
                    string satir = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss};{merkezFrekans:F3};{sinyalGucu:F1};{tehditEsigi:F1};{bantGenisligi:F3};{ornekleme:F3}";
                    sw.WriteLine(satir);
                }
            }
            catch (Exception ex) { KonsolaYaz(Properties.Settings.Default.ERR_LOG_YAZILAMADI + ex.Message); }
        }

        private void ExcelRaporuOlustur()
        {
            if (!File.Exists(Properties.Settings.Default.DOSYA_LOG_CSV))
            {
                MessageBox.Show(DilMotoru.Cevir(Properties.Settings.Default.MSG_RAPOR_BOS), DilMotoru.Cevir(Properties.Settings.Default.MSG_BILGI_BASLIK), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add(DilMotoru.Cevir(Properties.Settings.Default.EXCEL_SAYFA_ADI));
                    string[] satirlar = File.ReadAllLines(Properties.Settings.Default.DOSYA_LOG_CSV, System.Text.Encoding.UTF8);

                    if (satirlar == null || satirlar.Length == 0) { Cursor.Current = Cursors.Default; return; }

                    int sutunSayisi = satirlar[0].Split(';').Length;
                    for (int i = 0; i < satirlar.Length; i++)
                    {
                        string oankiSatir = satirlar[i] ?? "";
                        if (string.IsNullOrWhiteSpace(oankiSatir)) continue;

                        string[] hucreler = oankiSatir.Split(';');
                        for (int j = 0; j < hucreler.Length; j++) worksheet.Cell(i + 1, j + 1).Value = hucreler[j] ?? "";
                    }

                    var baslikSatiri = worksheet.Row(1);
                    baslikSatiri.Style.Font.Bold = true;
                    baslikSatiri.Style.Font.FontColor = XLColor.White;
                    baslikSatiri.Style.Fill.BackgroundColor = XLColor.FromArgb(50, 100, 70);
                    baslikSatiri.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Columns().AdjustToContents();

                    var tumTablo = worksheet.Range(1, 1, satirlar.Length, sutunSayisi);
                    tumTablo.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    tumTablo.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    string masaustuYolu = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) ?? "";
                    string raporAdi = string.Format(Properties.Settings.Default.EXCEL_DOSYA_SABLONU, DateTime.Now.ToString("yyyyMMdd_HHmmss"));
                    string tamYol = Path.Combine(masaustuYolu, raporAdi);

                    workbook.SaveAs(tamYol);
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(string.Format(DilMotoru.Cevir(Properties.Settings.Default.MSG_RAPOR_BASARILI), raporAdi), DilMotoru.Cevir(Properties.Settings.Default.MSG_RAPOR_HAZIR_BASLIK), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(string.Format(DilMotoru.Cevir(Properties.Settings.Default.MSG_RAPOR_HATA_METIN), ex.Message), DilMotoru.Cevir(Properties.Settings.Default.MSG_RAPOR_HATA_BASLIK), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 7. ARAYÜZ (FORM) OLAY TETİKLEYİCİLERİ EKSİKSİZ LİSTE
        private void btnModMuhendis_Click(object? sender, EventArgs e)
        {
            _ileriModAktif = !_ileriModAktif;
            EkranModunuAyarla(_ileriModAktif);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExcelRaporuOlustur();
        }

        private void cmbHedefProfilleri_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbHedefProfilleri.SelectedItem == null || cmbHedefProfilleri.SelectedIndex == -1) return;
            string secilenItem = cmbHedefProfilleri.SelectedItem.ToString() ?? string.Empty;

            _profilYukleniyor = true;

            if (secilenItem == DilMotoru.Cevir(Properties.Settings.Default.UI_PROFIL_YENI_KAYDET))
            {
                if (!_ileriModAktif)
                {
                    MessageBox.Show(DilMotoru.Cevir(Properties.Settings.Default.MSG_YETKI_METIN), DilMotoru.Cevir(Properties.Settings.Default.MSG_YETKI_BASLIK), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbHedefProfilleri.SelectedIndex = 0;
                    _profilYukleniyor = false;
                    return;
                }

                string yeniProfilAdi = PromptGoster(DilMotoru.Cevir(Properties.Settings.Default.MSG_PROFIL_ADI_GIRIN), DilMotoru.Cevir(Properties.Settings.Default.MSG_PROFIL_KAYDET_BASLIK));

                if (string.IsNullOrWhiteSpace(yeniProfilAdi))
                {
                    cmbHedefProfilleri.SelectedIndex = 0;
                    _profilYukleniyor = false;
                    return;
                }

                if (ozelProfiller.ContainsKey(yeniProfilAdi) || cmbHedefProfilleri.Items.Contains(yeniProfilAdi))
                {
                    MessageBox.Show(DilMotoru.Cevir(Properties.Settings.Default.ERR_PROFIL_ZATEN_VAR), DilMotoru.Cevir(Properties.Settings.Default.MSG_HATA_BASLIK), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cmbHedefProfilleri.SelectedIndex = 0;
                    _profilYukleniyor = false;
                    return;
                }

                double guncelFrekans = Convert.ToDouble(numRxFrekans.Value);
                double guncelOrnekleme = Convert.ToDouble(numRxOrnekleme.Value);
                double guncelBant = Convert.ToDouble(numRxBantGenisligi.Value);
                double guncelSquelch = trbSquelch.Value;
                double guncelYumusatma = trbYumusatma.Value;
                double guncelTaramaHizi = trbTaramaHizi.Value;

                ozelProfiller.Add(yeniProfilAdi, new double[] { guncelFrekans, guncelOrnekleme, guncelBant, guncelSquelch, guncelYumusatma, guncelTaramaHizi });

                int yeniSira = cmbHedefProfilleri.Items.Count - 1;
                cmbHedefProfilleri.Items.Insert(yeniSira, yeniProfilAdi);
                cmbHedefProfilleri.SelectedItem = yeniProfilAdi;

                AyarlariKaydet();
                MessageBox.Show(string.Format("'{0}' {1}", yeniProfilAdi, DilMotoru.Cevir(Properties.Settings.Default.MSG_BASARIYLA_KAYDEDILDI)), DilMotoru.Cevir(Properties.Settings.Default.MSG_BILGI_BASLIK), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _otomatikDegisim = true;

                if (ozelProfiller.ContainsKey(secilenItem))
                {
                    double[] kayitliAyarlar = ozelProfiller[secilenItem];
                    GuvenliAta(numRxFrekans, Convert.ToDecimal(kayitliAyarlar[0]));
                    GuvenliAta(numRxOrnekleme, Convert.ToDecimal(kayitliAyarlar[1]));
                    GuvenliAta(numRxBantGenisligi, Convert.ToDecimal(kayitliAyarlar[2]));

                    if (kayitliAyarlar.Length > 3)
                    {
                        trbSquelch.Value = (int)kayitliAyarlar[3];
                        if (lblSqulechTehtid != null) lblSqulechTehtid.Text = string.Format(Properties.Settings.Default.FORMAT_YUZDE, trbSquelch.Value);
                    }

                    if (kayitliAyarlar.Length > 5)
                    {
                        trbYumusatma.Value = (int)kayitliAyarlar[4];
                        alpha = trbYumusatma.Value / Properties.Settings.Default.YuzdeBolen;
                        if (lblYumusatmaDegeri != null) lblYumusatmaDegeri.Text = string.Format(Properties.Settings.Default.FORMAT_YUZDE, trbYumusatma.Value);

                        trbTaramaHizi.Value = (int)kayitliAyarlar[5];
                        taramaGecikmesi = trbTaramaHizi.Value;
                        if (lblTaramaHiziDegeri != null) lblTaramaHiziDegeri.Text = string.Format(Properties.Settings.Default.FORMAT_MS, trbTaramaHizi.Value);
                    }

                    KonsolaYaz($"[SİSTEM] {secilenItem} {DilMotoru.Cevir(Properties.Settings.Default.LOG_PROFIL_YUKLENDI)}");
                }
                else if (secilenItem == Properties.Settings.Default.UI_PROFIL_DRONE)
                {
                    cmbRxBirim.SelectedIndex = cmbRxBirim.FindString(Properties.Settings.Default.BIRIM_GHZ);
                    GuvenliAta(numRxFrekans, (decimal)Properties.Settings.Default.DroneFrekans);

                    cmbRxOrneklemeBirim.SelectedIndex = cmbRxOrneklemeBirim.FindString(Properties.Settings.Default.BIRIM_MSPS) != -1 ? cmbRxOrneklemeBirim.FindString(Properties.Settings.Default.BIRIM_MSPS) : cmbRxOrneklemeBirim.FindString(Properties.Settings.Default.BIRIM_MHZ);
                    GuvenliAta(numRxOrnekleme, (decimal)Properties.Settings.Default.DroneOrnekleme);

                    cmbRxBantBirim.SelectedIndex = cmbRxBantBirim.FindString(Properties.Settings.Default.BIRIM_MHZ);
                    GuvenliAta(numRxBantGenisligi, (decimal)Properties.Settings.Default.DroneBant);

                    KonsolaYaz(DilMotoru.Cevir(Properties.Settings.Default.LOG_PROFIL_DRONE));
                }
                else if (secilenItem == Properties.Settings.Default.UI_PROFIL_TELSIZ)
                {
                    cmbRxBirim.SelectedIndex = cmbRxBirim.FindString(Properties.Settings.Default.BIRIM_MHZ);
                    GuvenliAta(numRxFrekans, (decimal)Properties.Settings.Default.TelsizFrekans);

                    cmbRxOrneklemeBirim.SelectedIndex = cmbRxOrneklemeBirim.FindString(Properties.Settings.Default.BIRIM_MSPS) != -1 ? cmbRxOrneklemeBirim.FindString(Properties.Settings.Default.BIRIM_MSPS) : cmbRxOrneklemeBirim.FindString(Properties.Settings.Default.BIRIM_MHZ);
                    GuvenliAta(numRxOrnekleme, (decimal)Properties.Settings.Default.TelsizOrnekleme);

                    cmbRxBantBirim.SelectedIndex = cmbRxBantBirim.FindString(Properties.Settings.Default.BIRIM_MHZ);
                    GuvenliAta(numRxBantGenisligi, (decimal)Properties.Settings.Default.TelsizBant);

                    KonsolaYaz(DilMotoru.Cevir(Properties.Settings.Default.LOG_PROFIL_TELSIZ));
                }
                else if (secilenItem == Properties.Settings.Default.UI_PROFIL_TELEFON)
                {
                    cmbRxBirim.SelectedIndex = cmbRxBirim.FindString(Properties.Settings.Default.BIRIM_MHZ);
                    GuvenliAta(numRxFrekans, (decimal)Properties.Settings.Default.TelefonFrekans);

                    cmbRxOrneklemeBirim.SelectedIndex = cmbRxOrneklemeBirim.FindString(Properties.Settings.Default.BIRIM_MSPS) != -1 ? cmbRxOrneklemeBirim.FindString(Properties.Settings.Default.BIRIM_MSPS) : cmbRxOrneklemeBirim.FindString(Properties.Settings.Default.BIRIM_MHZ);
                    GuvenliAta(numRxOrnekleme, (decimal)Properties.Settings.Default.TelefonOrnekleme);

                    cmbRxBantBirim.SelectedIndex = cmbRxBantBirim.FindString(Properties.Settings.Default.BIRIM_MHZ);
                    GuvenliAta(numRxBantGenisligi, (decimal)Properties.Settings.Default.TelefonBant);

                    KonsolaYaz(DilMotoru.Cevir(Properties.Settings.Default.LOG_PROFIL_TELEFON));
                }

                UI_LimitleriUygula();
                _otomatikDegisim = false;
            }

            _profilYukleniyor = false;

            if (_isDeviceOpen)
            {
                _ = CihazParametreleriniUygula();
            }
        }

        private void cmbBirim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_otomatikDegisim) return;
            if (string.IsNullOrEmpty(eskiFrekansBirim) || eskiFrekansBirim == cmbRxBirim.Text) return;
            _otomatikDegisim = true;
            decimal yeniDeger = BirimDonustur(numRxFrekans.Value, eskiFrekansBirim, cmbRxBirim.Text);
            numRxFrekans.Maximum = decimal.MaxValue;
            numRxFrekans.Minimum = decimal.MinValue;
            numRxFrekans.Value = yeniDeger;
            UI_LimitleriUygula();
            eskiFrekansBirim = cmbRxBirim.Text;
            _otomatikDegisim = false;
            DinamikParametreUygula();
        }

        private void cmbOrneklemeBirim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_otomatikDegisim) return;
            if (string.IsNullOrEmpty(eskiOrneklemeBirim) || eskiOrneklemeBirim == cmbRxOrneklemeBirim.Text) return;
            _otomatikDegisim = true;
            decimal yeniDeger = BirimDonustur(numRxOrnekleme.Value, eskiOrneklemeBirim, cmbRxOrneklemeBirim.Text);
            numRxOrnekleme.Maximum = decimal.MaxValue;
            numRxOrnekleme.Minimum = decimal.MinValue;
            numRxOrnekleme.Value = yeniDeger;
            UI_LimitleriUygula();
            eskiOrneklemeBirim = cmbRxOrneklemeBirim.Text;
            _otomatikDegisim = false;
            DinamikParametreUygula();
        }

        private void cmbBantBirim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_otomatikDegisim) return;
            if (string.IsNullOrEmpty(eskiBantBirim) || eskiBantBirim == cmbRxBantBirim.Text) return;
            _otomatikDegisim = true;
            decimal yeniDeger = BirimDonustur(numRxBantGenisligi.Value, eskiBantBirim, cmbRxBantBirim.Text);
            numRxBantGenisligi.Maximum = decimal.MaxValue;
            numRxBantGenisligi.Minimum = decimal.MinValue;
            numRxBantGenisligi.Value = yeniDeger;
            UI_LimitleriUygula();
            eskiBantBirim = cmbRxBantBirim.Text;
            _otomatikDegisim = false;
            DinamikParametreUygula();
        }

        private void cmbTxBirim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_otomatikDegisim) return;
            if (string.IsNullOrEmpty(eskiTxFrekansBirim) || eskiTxFrekansBirim == cmbTxBirim.Text) return;
            _otomatikDegisim = true;
            decimal yeniDeger = BirimDonustur(numTxFrekans.Value, eskiTxFrekansBirim, cmbTxBirim.Text);
            numTxFrekans.Maximum = decimal.MaxValue;
            numTxFrekans.Minimum = decimal.MinValue;
            numTxFrekans.Value = yeniDeger;
            UI_LimitleriUygula();
            eskiTxFrekansBirim = cmbTxBirim.Text;
            _otomatikDegisim = false;
        }

        private void cmbTxBantBirim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_otomatikDegisim) return;
            if (string.IsNullOrEmpty(eskiTxBantBirim) || eskiTxBantBirim == cmbTxBantBirim.Text) return;
            _otomatikDegisim = true;
            decimal yeniDeger = BirimDonustur(numTxBantGenisligi.Value, eskiTxBantBirim, cmbTxBantBirim.Text);
            numTxBantGenisligi.Maximum = decimal.MaxValue;
            numTxBantGenisligi.Minimum = decimal.MinValue;
            numTxBantGenisligi.Value = yeniDeger;
            UI_LimitleriUygula();
            eskiTxBantBirim = cmbTxBantBirim.Text;
            _otomatikDegisim = false;
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

        private void chkBiasTee_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isDeviceOpen || _devicePointer == IntPtr.Zero) return;

            // Hem dinleme (RX) hem de saldırı (TX) portlarındaki aktif amfilere 5V gücü bas!
            BladeRFBridge.bladerf_set_bias_tee(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, chkBiasTee.Checked);
            BladeRFBridge.bladerf_set_bias_tee(_devicePointer, BladeRFBridge.BLADERF_MODULE_TX, chkBiasTee.Checked);
        }

        private void trbRxKazanci_Scroll(object sender, EventArgs e)
        {
            if (lblRxKazanciDegeri != null) lblRxKazanciDegeri.Text = trbRxKazanci.Value.ToString() + " dB";
        }

        private void trbRxKazanci_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_isDeviceOpen || _devicePointer == IntPtr.Zero || chkAGC.Checked) return;
            BladeRFBridge.bladerf_set_gain(_devicePointer, BladeRFBridge.BLADERF_MODULE_RX, trbRxKazanci.Value);
        }

        private void trbTxGain_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_isDeviceOpen || _devicePointer == IntPtr.Zero) return;
            BladeRFBridge.bladerf_set_gain(_devicePointer, BladeRFBridge.BLADERF_MODULE_TX, trbTxGain.Value);
        }

        private void trbYumusatma_Scroll(object sender, EventArgs e)
        {
            alpha = trbYumusatma.Value / Properties.Settings.Default.YuzdeBolen;
            if (lblYumusatmaDegeri != null) lblYumusatmaDegeri.Text = string.Format(Properties.Settings.Default.FORMAT_YUZDE, trbYumusatma.Value);
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

        private void trbSquelch_Scroll(object sender, EventArgs e)
        {
            if (lblSqulechTehtid != null) lblSqulechTehtid.Text = $"{trbSquelch.Value}dB";
        }

        private void trbTaramaHizi_Scroll(object sender, EventArgs e)
        {
            taramaGecikmesi = trbTaramaHizi.Value;
            if (lblTaramaHiziDegeri != null) lblTaramaHiziDegeri.Text = string.Format(Properties.Settings.Default.FORMAT_MS, trbTaramaHizi.Value);
        }

        private void chkMarkerAktif_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMarkerAktif.Checked)
            {
                double fCarpan = 1;
                if (cmbRxBirim.Text == "kHz") fCarpan = Properties.Settings.Default.CarpanKilo;
                else if (cmbRxBirim.Text == "MHz") fCarpan = Properties.Settings.Default.CarpanMega;
                else if (cmbRxBirim.Text == "GHz") fCarpan = Properties.Settings.Default.CarpanGiga;
                hedefMarkerFrekansHz = (double)numRxFrekans.Value * fCarpan;
            }
            picGrafik.Invalidate();
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

        private void chkAlarmAktif_CheckedChanged(object sender, EventArgs e)
        {
            if (trbSquelch != null) trbSquelch.Enabled = chkAlarmAktif.Checked;
        }

        private void trbTxGain_Scroll(object sender, EventArgs e)
        {
            if (lblTxKazanciDegeri != null) lblTxKazanciDegeri.Text = trbTxGain.Value.ToString() + " dB";
        }

        private void TxParametresiDegisti(object sender, EventArgs e)
        {
            if (!_isDeviceOpen || _devicePointer == IntPtr.Zero || !_saldiriAktif) return;

            try
            {
                decimal txFrekansCarpani = cmbTxBirim.Text.Contains("G") ? 1000000000m : (cmbTxBirim.Text.Contains("M") ? 1000000m : 1000m);
                ulong yeniTxFrekans = (ulong)(numTxFrekans.Value * txFrekansCarpani);

                decimal txBantCarpani = cmbTxBantBirim.Text.Contains("G") ? 1000000000m : (cmbTxBantBirim.Text.Contains("M") ? 1000000m : 1000m);
                uint yeniTxBant = (uint)(numTxBantGenisligi.Value * txBantCarpani);

                BladeRFBridge.bladerf_set_frequency(_devicePointer, 1, yeniTxFrekans);
                BladeRFBridge.bladerf_set_bandwidth(_devicePointer, 1, yeniTxBant, out uint _);
            }
            catch { }
        }
        private void btnModMuhendis_Click_1(object sender, EventArgs e) { }
        private void groupBox2_Enter(object sender, EventArgs e) { }
        private void chkTXgör_CheckedChanged(object sender, EventArgs e) { }
        private void TxDegerDegisti(object sender, EventArgs e) { }
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
        private void chkGurultuEngelle_CheckedChanged(object sender, EventArgs e) { }
        private void rdoTaarruzModu_CheckedChanged(object sender, EventArgs e) { }
        private void label10_Click(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        #endregion
    }//SAĞLAM SIN 

    #region 9. SİSTEM AYAR MODELLERİ
    public class AyarModeli
    {
        public Dictionary<string, double[]> OzelProfiller { get; set; } = new Dictionary<string, double[]>();
        public int Squelch { get; set; } = -40;
        public int Yumusatma { get; set; } = 1;
        public int TaramaHizi { get; set; } = 10;
        public string SeciliDil { get; set; } = Properties.Settings.Default.VARSAYILAN_DIL;
        public string SeciliTema { get; set; } = Properties.Settings.Default.VARSAYILAN_TEMA;
    }
    #endregion
}