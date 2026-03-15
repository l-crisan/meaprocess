using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Mp.Scheme.Sdk
{
    public class Licence
    {
        [DllImport("mps-licence", EntryPoint = "mps_getLicenceHandle", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong Mps_GetLicenceHandler();

        [DllImport("mps-licence", EntryPoint = "mps_getLicencedApps", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint Mps_GetLicencedApps(ulong handle, uint app);

        [DllImport("mps-licence", EntryPoint = "mps_getLockSerial", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint Mps_GetLockSerial(ulong handle);

        [DllImport("mps-licence", EntryPoint = "mps_freeLicence", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Mps_FreeLicence(ulong licenceHandler);

        public static string VersionString
        {
            get 
            { 
                Version v = Assembly.GetExecutingAssembly().GetName().Version;
                return v.Major.ToString() +"." + v.Minor.ToString(); 
            }
        }
        
        public static int MajorVersion
        {
            get
            {
                Version v = Assembly.GetExecutingAssembly().GetName().Version;
                return v.Major;
            }
        }

        public static int MinorVersion
        {
            get
            {
                Version v = Assembly.GetExecutingAssembly().GetName().Version;
                return v.Minor;
            }
        }

        public static int IsValidLicence(uint product)
        {
            /*
            ulong handler = Mps_GetLicenceHandler();
            
            if( handler == 0)
                return 0;

            int prod = (int)Mps_GetLicencedApps(handler, product);

            Mps_FreeLicence(handler);
            return prod;
            */
            return 1;
        }

        public static bool IsRuntimeAvailable(int rt)
        {
            return IsValidLicence((uint)rt) != 0;
        }     
    }
}
