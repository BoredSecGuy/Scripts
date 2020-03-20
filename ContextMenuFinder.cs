using Microsoft.Win32;
using System;

namespace ContextMenuFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            // Find the CLSID of programs with context menu handlers
            Console.WriteLine("----------------------------------------------------------------------------------------------");
            RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Classes\\*\\ShellEx\\ContextMenuHandlers");
            foreach (string subKeyName in key.GetSubKeyNames())
            {
                using (RegistryKey tempKey = key.OpenSubKey(subKeyName))
                {
                    foreach (string valueName in tempKey.GetValueNames())
                    {
                        string targetDLL = tempKey.GetValue(valueName).ToString();
                        Console.WriteLine("Key: " + subKeyName);
                        Console.WriteLine("Value: " + targetDLL);

                        // Iterate through the CLSIDs to get the default DLL in Software\Classes\CLSID\<CLSID>\InprocServer32
                        string fullRegPath = "Software\\Classes\\CLSID\\" + tempKey.GetValue(valueName).ToString() + "\\InprocServer32";

                        try
                        {
                            RegistryKey cl = Registry.LocalMachine.OpenSubKey("Software\\Classes\\CLSID\\" + tempKey.GetValue(valueName).ToString() + "\\InprocServer32");
                            string dllname = cl.GetValue(null).ToString();
                            if (dllname.Contains("system32"))
                            {
                                Console.WriteLine("[!] Target DLL in C:\\Windows\\System32, pick a different target.");
                                Console.WriteLine("[!] Target DLL: " + cl.GetValue(null).ToString() + "\n");
                            }
                            else
                            {
                                Console.WriteLine("[+] Target DLL: " + cl.GetValue(null).ToString() + "\n");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("[!] No target DLL for this program.\n");
                        }

                    }
                }
                Console.WriteLine("----------------------------------------------------------------------------------------------");
            }
        }
    }
}
