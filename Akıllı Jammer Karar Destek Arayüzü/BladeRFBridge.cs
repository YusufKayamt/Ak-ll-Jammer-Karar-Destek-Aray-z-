using System;
using System.Runtime.InteropServices;

namespace Akıllı_Jammer_Karar_Destek_Arayüzü
{
    public static class BladeRFBridge
    {
        private const string DllName = "bladeRF";

        // Modül Tanımları (C++ Enum karşılığı)
        public const int BLADERF_MODULE_RX = 0;
        public const int BLADERF_MODULE_TX = 1;

        // Veri Formatı
        public const int BLADERF_FORMAT_SC16_Q11 = 0;

        [DllImport("bladerf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_open(out IntPtr device, IntPtr device_identifier);

        [DllImport(DllName, EntryPoint = "bladerf_close", CallingConvention = CallingConvention.Cdecl)]
        public static extern void bladerf_close(IntPtr dev);

        [DllImport(DllName, EntryPoint = "bladerf_sync_config", CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_sync_config(IntPtr dev, int module, int format, uint num_buffers, uint buffer_size, uint num_transfers, uint stream_timeout);

        [DllImport(DllName, EntryPoint = "bladerf_enable_module", CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_enable_module(IntPtr dev, int module, [MarshalAs(UnmanagedType.I1)] bool enable);

        [DllImport(DllName, EntryPoint = "bladerf_set_frequency", CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_set_frequency(IntPtr dev, int module, ulong frequency);

        [DllImport(DllName, EntryPoint = "bladerf_set_sample_rate", CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_set_sample_rate(IntPtr dev, int module, uint rate, out uint actual_rate);

        [DllImport(DllName, EntryPoint = "bladerf_set_bandwidth", CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_set_bandwidth(IntPtr dev, int module, uint bandwidth, out uint actual_bandwidth);

        [DllImport(DllName, EntryPoint = "bladerf_set_gain", CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_set_gain(IntPtr dev, int module, int gain);

        [DllImport(DllName, EntryPoint = "bladerf_set_gain_mode", CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_set_gain_mode(IntPtr dev, int module, int mode);

        [DllImport(DllName, EntryPoint = "bladerf_set_bias_tee", CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_set_bias_tee(IntPtr dev, int module, [MarshalAs(UnmanagedType.I1)] bool enable);

        [DllImport(DllName, EntryPoint = "bladerf_set_loopback", CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_set_loopback(IntPtr dev, int loopback);

        [DllImport("bladerf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_sync_tx(IntPtr device, short[] samples, uint num_samples, IntPtr metadata, uint timeout_ms);

        [DllImport("bladerf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int bladerf_sync_rx(IntPtr device, short[] samples, uint num_samples, IntPtr metadata, uint timeout_ms);
    }
}