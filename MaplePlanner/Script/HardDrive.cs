using System.Linq;
using System.Management;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System;

namespace MaplePlanner.Script
{
    public class HardDrive
    {
        public static string GetHDDSerial()
        {
            ArrayList hardDriveDetails = new ArrayList();

            ManagementObjectSearcher moSearcher = new
                ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            ManagementObjectCollection moc = moSearcher.Get();

            ManagementObject mo = moc.OfType<ManagementObject>().FirstOrDefault();

            string serialnumber = mo["SerialNumber"].ToString();

            string result = MD5(MD5(serialnumber));
            return result;
        }

        public static string MD5(string input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(Encoding.Unicode.GetBytes(input));
            string result = BitConverter.ToString(bytes).Replace("-", string.Empty);

            return result.ToLower();
        }
    }
    
    
}
