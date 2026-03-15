using System.Runtime.InteropServices;

namespace Mp.Drv.OpenCV
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct DeviceInfo
    {
        public int CardID;
        public int Width;
        public int Height;
        public int Rate;
        public int Error;
    }
}
