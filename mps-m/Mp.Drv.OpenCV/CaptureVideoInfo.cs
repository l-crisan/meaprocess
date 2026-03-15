using System.Runtime.InteropServices;

namespace Mp.Drv.OpenCV
{
    public class VideoCapture
    {
        [DllImport("mps-drv-opencv", EntryPoint = "mpsopencv_open", CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong Open(int card);

        [DllImport("mps-drv-opencv", EntryPoint = "mpsopencv_readFrame", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadFrame(ulong handle, byte[] buffer);

        [DllImport("mps-drv-opencv", EntryPoint = "mpsopencv_close", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Close(ulong handle);

        [DllImport("mps-drv-opencv", EntryPoint = "mpsopencv_detect", CallingConvention = CallingConvention.Cdecl)]
        public static extern DeviceInfo Detect(int card);
    }
}
