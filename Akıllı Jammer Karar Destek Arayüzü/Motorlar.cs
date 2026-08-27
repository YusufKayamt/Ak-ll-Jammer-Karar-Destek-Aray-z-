using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace Akıllı_Jammer_Karar_Destek_Arayüzü
{



    public static class DilMotoru
    {
        private static Dictionary<string, string> _sozluk = new Dictionary<string, string>();
        private static string _aktifDosyaYolu = "";

        public static void Yukle(string dosyaAdi)
        {
            _aktifDosyaYolu = System.IO.Path.Combine(Application.StartupPath, dosyaAdi);
            try
            {
                if (File.Exists(_aktifDosyaYolu))
                {
                    string json = File.ReadAllText(_aktifDosyaYolu);
                    _sozluk = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
                }
                else
                {
                    _sozluk = new Dictionary<string, string>();

                    _sozluk[Properties.Settings.Default.STATUS_MOD_TEST] = Properties.Settings.Default.STATUS_MOD_TEST;
                    _sozluk[Properties.Settings.Default.STATUS_MOD_TAARRUZ] = Properties.Settings.Default.STATUS_MOD_TAARRUZ;
                    _sozluk[Properties.Settings.Default.STATUS_MOD_DINLEME] = Properties.Settings.Default.STATUS_MOD_DINLEME;
                    _sozluk[Properties.Settings.Default.STATUS_ALARM_KAPALI] = Properties.Settings.Default.STATUS_ALARM_KAPALI;
                    _sozluk[Properties.Settings.Default.UI_BTN_BAGLAN] = Properties.Settings.Default.UI_BTN_BAGLAN;
                    _sozluk[Properties.Settings.Default.UI_BTN_DURDUR] = Properties.Settings.Default.UI_BTN_DURDUR;
                    _sozluk[Properties.Settings.Default.UI_BTN_DEVAM] = Properties.Settings.Default.UI_BTN_DEVAM;

                    Kaydet();
                }
            }
            catch
            {
                _sozluk = new Dictionary<string, string>();
            }
        }

        public static void Kaydet()
        {
            if (string.IsNullOrEmpty(_aktifDosyaYolu)) return;
            try
            {
                var ayarlar = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                File.WriteAllText(_aktifDosyaYolu, JsonSerializer.Serialize(_sozluk, ayarlar));
            }
            catch { }
        }

        public static string Cevir(string metin)
        {
            if (string.IsNullOrWhiteSpace(metin)) return metin;
            if (_sozluk.ContainsKey(metin)) return _sozluk[metin];

            _sozluk[metin] = metin;
            Kaydet();
            return metin;
        }
    }
    public static class TemaMotoru
    {
        private static Dictionary<string, string> _renkler = new Dictionary<string, string>();

        public static void Yukle(string dosyaAdi)
        {
            string tamYol = System.IO.Path.Combine(Application.StartupPath, dosyaAdi);
            try
            {
                if (File.Exists(tamYol))
                {
                    string json = File.ReadAllText(tamYol);
                    _renkler = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
                }
                else
                {
                    VarsayilaniYukle();
                    File.WriteAllText(tamYol, JsonSerializer.Serialize(_renkler, new JsonSerializerOptions { WriteIndented = true }));
                }
            }
            catch
            {
                VarsayilaniYukle();
            }
        }

        private static void VarsayilaniYukle()
        {
            _renkler = new Dictionary<string, string>
            {
                { "TEMA_GUVENLI", "#008080" }, { "TEMA_TAARRUZ", "#FF8C00" }, { "TEMA_TAARRUZ_AKTIF", "#800000" },
                { "TEMA_DINLEME", "#283C2D" }, { "TEMA_PASIF", "#808080" }, { "TEMA_UYARI", "#FF8C00" }
            };
        }

        private static Color Al(string anahtar)
        {
            if (_renkler.ContainsKey(anahtar)) try { return ColorTranslator.FromHtml(_renkler[anahtar]); } catch { }
            return Color.Gray;
        }

        public static Color TEMA_GUVENLI => Al("TEMA_GUVENLI");
        public static Color TEMA_TAARRUZ => Al("TEMA_TAARRUZ");
        public static Color TEMA_TAARRUZ_AKTIF => Al("TEMA_TAARRUZ_AKTIF");
        public static Color TEMA_DINLEME => Al("TEMA_DINLEME");
        public static Color TEMA_PASIF => Al("TEMA_PASIF");
        public static Color TEMA_UYARI => Al("TEMA_UYARI");
    }
}