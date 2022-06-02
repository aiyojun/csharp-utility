using System.Management;

namespace jlib
{
    class Platform
    {
        public static readonly string MKEY_OF_WIN_CPU = "Win32_Processor";
        public static readonly string MKEY_OF_CPU_ID  = "ProcessorId";

        public static string GetCpuSeq()
        {
            string _r = "";
            ManagementClass mc = new ManagementClass(MKEY_OF_WIN_CPU);
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                _r += ( ":" + mo.Properties[MKEY_OF_CPU_ID].Value.ToString());
            }
            _r = _r.Replace(":", "");
            return _r;
        }
    }
}