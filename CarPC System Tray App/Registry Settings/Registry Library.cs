using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Win32;

namespace SteveO.RegistryLibrary
{
    public static class RegistryLibrary
    {
        public static string GetStringValue(RegistryLocation location, string ValueName, string DefaultValue)
        {
            return Convert.ToString(GetValue(location, ValueName, DefaultValue));
        }
        public static DateTime GetDateTimeValue(RegistryLocation location, string ValueName, DateTime DefaultValue)
        {
            return DateTime.Parse(Convert.ToString(GetValue(location, ValueName, DefaultValue)));
        }
        public static int GetInt32(RegistryLocation location, string ValueName, int DefaultValue)
        {
            return Convert.ToInt32(GetValue(location, ValueName, DefaultValue));
        }
        public static uint GetUInt32(RegistryLocation location, string ValueName, uint DefaultValue)
        {
            return Convert.ToUInt32(GetValue(location, ValueName, DefaultValue));
        }
        public static bool GetBooleanValue(RegistryLocation location, string ValueName, bool DefaultValue)
        {
            return Convert.ToBoolean(GetValue(location, ValueName, DefaultValue));
        }
        private static object GetValue(RegistryLocation location, string ValueName, object DefaultValue)
        {
            RegistryKey key = GetRegKey(location);
            object ReturnValue = key.GetValue(ValueName, DefaultValue);
            key.SetValue(ValueName, ReturnValue, RegistryValueKind.String);

            return ReturnValue;
        }
        public static void SetValue(RegistryLocation location, string ValueName, object Value)
        {
            RegistryKey key = GetRegKey(location);
            key.SetValue(ValueName, Value);
        }
        
        // Registry Subkeys
        public static RegistryLocation CreateSubKey(RegistryLocation location, string NewKeyName)
        {
            RegistryKey NewKey = GetRegKey(location).CreateSubKey(NewKeyName);

            return new RegistryLocation(location.Hive, location.RegistryPath + "\\" + NewKeyName);
        }
        public static void DeleteSubKeys(RegistryLocation location)
        {
            RegistryKey key = GetRegKey(location);
            string[] SubKeyNames = key.GetSubKeyNames();
            foreach (string SubKey in SubKeyNames)
            {
                key.DeleteSubKey(SubKey);
            }
        }
        public static string[] GetSubKeyNames(RegistryLocation location)
        {
            return GetRegKey(location).GetSubKeyNames();
        }
        private static RegistryKey GetRegKey(RegistryLocation location)
        {
            switch (location.Hive)
            {
                case RegistryHive.LocalMachine:
                    return Registry.LocalMachine.CreateSubKey(location.RegistryPath);
                case RegistryHive.CurrentUser:
                    return Registry.CurrentUser.CreateSubKey(location.RegistryPath);
            }
            throw new Exception();
        }
        private static RegistryKey CreateOrOpenKey(RegistryKey key)
        {
            return null;
        }
    }
    public enum RegistryHive
    {
        LocalMachine = 1,
        CurrentUser = 2
    }
    public class RegistryLocation
    {
        private RegistryHive _Hive;
        private string _RegistryKeyPath;

        public RegistryHive Hive
        {
            get
            {
                return this._Hive;
            }
        }
        public string RegistryPath
        {
            get
            {
                return this._RegistryKeyPath;
            }
        }
        public RegistryLocation(RegistryHive hive, string RegistryKeyPath)
        {
            _Hive = hive;
            _RegistryKeyPath = RegistryKeyPath;
        }
    }
}
