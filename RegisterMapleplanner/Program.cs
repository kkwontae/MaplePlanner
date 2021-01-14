using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegisterMapleplanner;


namespace RegisterMapleplanner
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //Console.WriteLine("/C choice /C Y /N /D Y /T 3 & Del " +
            //    System.Reflection.Assembly.GetExecutingAssembly().Location);
            //WebBrowserVersionSetting();
            //DeleteItSelf();
        }

        private static string GetIEVersion()
        {
            string key = @"Software\Microsoft\Internet Explorer";
            RegistryKey dkey = Registry.LocalMachine.OpenSubKey(key, false);
            string data = dkey.GetValue("Version").ToString();
            return data;
        }
        private static void DeleteItSelf()
        {
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = "/C choice /C Y /N /D Y /T 3 & Del " +
                System.Reflection.Assembly.GetExecutingAssembly().Location;
            Info.WindowStyle = ProcessWindowStyle.Hidden;
            Info.CreateNoWindow = true;
            Info.FileName = "cmd.exe";
            Process.Start(Info);
        }

        private static void WebBrowserVersionSetting()
        {
            RegistryKey registryKey = null; // 레지스트리 변경에 사용 될 변수

            string version = GetIEVersion().Split('.')[1];
            int browserver = Convert.ToInt32(version);

            int ie_emulation = 0;
            var targetApplication = "MaplePlanner.exe"; // 현재 프로그램 이름

            if (browserver >= 11)
                ie_emulation = 11001;
            else if (browserver == 10)
                ie_emulation = 10001;
            else if (browserver == 9)
                ie_emulation = 9999;
            else if (browserver == 8)
                ie_emulation = 8888;
            else
                ie_emulation = 7000;
            try
            {
                registryKey = Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);

                // IE가 없으면 실행 불가능
                if (registryKey == null)
                {
                    Console.WriteLine("IE no detected");
                    return;
                }

                string FindAppkey = Convert.ToString(registryKey.GetValue(targetApplication));
                // 이미 키가 있다면 종료
                if (FindAppkey == ie_emulation.ToString())
                {
                    Console.WriteLine("이미 등록하였습니다.\n\n");
                    registryKey.Close();
                    return;
                }

                // 키가 없으므로 키 셋팅
                registryKey.SetValue(targetApplication, unchecked((int)ie_emulation), RegistryValueKind.DWord);
                Console.WriteLine("{0} 위치에 레지스트리를 등록하였습니다 : {1} = {2}\n\n", registryKey.ToString(), targetApplication, ie_emulation.ToString());

                // 다시 키를 받아와서
                FindAppkey = Convert.ToString(registryKey.GetValue(targetApplication));

                // 현재 브라우저 버전이랑 동일 한지 판단
                if (FindAppkey == ie_emulation.ToString())
                {
                    Console.WriteLine("레지스트리를 성공적으로 등록하였습니다.\n\n프로그램을 자동으로 삭제합니다.", FindAppkey, ie_emulation.ToString());
                    return;
                }
                else
                {
                    Console.WriteLine("레지스트리 등록에 실패하였습니다.\n\n", FindAppkey, ie_emulation.ToString());
                    return;
                }
            }
            catch
            {
                Console.WriteLine("관리자 권한으로 실행해주세요\n\n");
                return;
            }
            finally
            {
                // 키 메모리 해제
                if (registryKey != null)
                {
                    registryKey.Close();
                }
            }
        }
    }
}
