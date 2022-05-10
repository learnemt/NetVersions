// Decompiled with JetBrains decompiler
// Type: DotNetVersions.Program
// Assembly: DotNetVersions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 844D0A80-A0FA-43FD-BE13-E8A428CB26C8
// Assembly location: F:\DotNetVersions\DotNetVersions.exe

using Microsoft.Win32;
using System;

namespace DotNetVersions
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      bool flag = false;
      if (args.Length != 0 && (args[0] == "/help" || args[0] == "-help" || args[0] == "--help" || args[0] == "/?" || args[0] == "-?" || args[0] == "--?"))
      {
        Console.Write("Writes all the currently installed versions of \"classic\" .NET platform in the system.\r\nUse --b, -b or /b to use in a batch, showing only the installed versions, without any extra informational lines.");
      }
      else
      {
        if (args.Length != 0 && (args[0] == "/b" || args[0] == "-b" || args[0] == "--b"))
          flag = true;
        if (!flag)
          Console.WriteLine("Currently installed \"classic\" .NET Versions in the system:");
        Program.Get1To45VersionFromRegistry();
        Program.Get45PlusFromRegistry();
      }
      if (flag)
        return;
      Console.ReadKey();
    }

    private static void WriteVersion(string version, string spLevel = "")
    {
      version = version.Trim();
      if (string.IsNullOrEmpty(version))
        return;
      string str = "";
      if (!string.IsNullOrEmpty(spLevel))
        str = " Service Pack " + spLevel;
      Console.WriteLine(version + str);
    }

    private static void Get1To45VersionFromRegistry()
    {
      using (RegistryKey registryKey1 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\"))
      {
        foreach (string subKeyName1 in registryKey1.GetSubKeyNames())
        {
          if (!(subKeyName1 == "v4") && subKeyName1.StartsWith("v"))
          {
            RegistryKey registryKey2 = registryKey1.OpenSubKey(subKeyName1);
            string version1 = (string) registryKey2.GetValue("Version", (object) "");
            string spLevel = registryKey2.GetValue("SP", (object) "").ToString();
            string str1 = registryKey2.GetValue("Install", (object) "").ToString();
            if (string.IsNullOrEmpty(str1))
              Program.WriteVersion(version1);
            else if (!string.IsNullOrEmpty(spLevel) && str1 == "1")
              Program.WriteVersion(version1, spLevel);
            if (string.IsNullOrEmpty(version1))
            {
              foreach (string subKeyName2 in registryKey2.GetSubKeyNames())
              {
                RegistryKey registryKey3 = registryKey2.OpenSubKey(subKeyName2);
                string version2 = (string) registryKey3.GetValue("Version", (object) "");
                if (!string.IsNullOrEmpty(version2))
                  spLevel = registryKey3.GetValue("SP", (object) "").ToString();
                string str2 = registryKey3.GetValue("Install", (object) "").ToString();
                if (string.IsNullOrEmpty(str2))
                  Program.WriteVersion(version2);
                else if (!string.IsNullOrEmpty(spLevel) && str2 == "1")
                  Program.WriteVersion(version2, spLevel);
                else if (str2 == "1")
                  Program.WriteVersion(version2);
              }
            }
          }
        }
      }
    }

    private static void Get45PlusFromRegistry()
    {
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
      {
        if (registryKey == null)
          return;
        if (registryKey.GetValue("Version") != null)
        {
          Program.WriteVersion(registryKey.GetValue("Version").ToString());
        }
        else
        {
          if (registryKey == null || registryKey.GetValue("Release") == null)
            return;
          Program.WriteVersion(CheckFor45PlusVersion((int) registryKey.GetValue("Release")));
        }
      }

      static string CheckFor45PlusVersion(int releaseKey)
      {
        if (releaseKey >= 528040)
          return "4.8";
        if (releaseKey >= 461808)
          return "4.7.2";
        if (releaseKey >= 461308)
          return "4.7.1";
        if (releaseKey >= 460798)
          return "4.7";
        if (releaseKey >= 394802)
          return "4.6.2";
        if (releaseKey >= 394254)
          return "4.6.1";
        if (releaseKey >= 393295)
          return "4.6";
        if (releaseKey >= 379893)
          return "4.5.2";
        if (releaseKey >= 378675)
          return "4.5.1";
        return releaseKey >= 378389 ? "4.5" : "";
      }
    }
  }
}
