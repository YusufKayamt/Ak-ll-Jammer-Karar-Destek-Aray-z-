using System;
using System.Runtime.InteropServices;

namespace Akıllı_Jammer_Karar_Destek_Arayüzü
{
    public static class BladeRFBridge
    {
        private const string DllName = "bladeRF";

        public const int BLADERF_MODULE_RX = 0;
        public const int BLADERF_MODULE_TX = 1;

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_open(out IntPtr device, IntPtr device_identifier);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void bladerf_close(IntPtr dev);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_enable_module(IntPtr dev, int module, [MarshalAs(UnmanagedType.I1)] bool enable);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_set_frequency(IntPtr dev, int module, ulong frequency);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_set_sample_rate(IntPtr dev, int module, uint rate, out uint actual_rate);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_set_bandwidth(IntPtr dev, int module, uint bandwidth, out uint actual_bandwidth);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_set_gain(IntPtr dev, int module, int gain);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_set_gain_mode(IntPtr dev, int module, int mode);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_set_bias_tee(IntPtr dev, int module, [MarshalAs(UnmanagedType.I1)] bool enable);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_calibrate_dc(IntPtr dev, int module);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_set_loopback(IntPtr dev, int loopback_mode);

        // KOPYA VE HATALI TANIMLAMALAR SİLİNDİ! TEK VE DOĞRU OLANLAR:
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_sync_config(IntPtr dev, int module, int format, uint num_buffers, uint buffer_size, uint num_transfers, uint stream_timeout);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_sync_tx(IntPtr dev, short[] samples, uint num_samples, IntPtr metadata, uint timeout_ms);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_sync_rx(IntPtr dev, short[] samples, uint num_samples, IntPtr metadata, uint timeout_ms);
    }
}