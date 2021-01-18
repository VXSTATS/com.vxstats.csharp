/*
 * Copyright (C) 12/10/2020 VX STATS <sales@vxstats.com>
 *
 * This document is property of VX STATS. It is strictly prohibited
 * to modify, sell or publish it in any way. In case you have access
 * to this document, you are obligated to ensure its nondisclosure.
 * Noncompliances will be prosecuted.
 *
 * Diese Datei ist Eigentum der VX STATS. Jegliche Änderung, Verkauf
 * oder andere Verbreitung und Veröffentlichung ist strikt untersagt.
 * Falls Sie Zugang zu dieser Datei haben, sind Sie verpflichtet,
 * alles in Ihrer Macht stehende für deren Geheimhaltung zu tun.
 * Zuwiderhandlungen werden strafrechtlich verfolgt.
 */

/* using */
using System;
#if __MOBILE__
using Xamarin.Essentials;
#else
using Microsoft.Win32;
#endif

/**
 * @~english
 * @brief The vxstats namespace.
 *
 * @~german
 * @brief Der vxstats Namensraum.
 */
namespace vxstats
{
    public sealed class Device
    {
        private static string model = "";

        private static string vendor = "";

        private static string version = "";

        private static string osVersion = "0.0.0";

        private static bool darkMode = false;

        private static readonly bool jailbroken = false;

        private static bool tabletMode = false;

        private static readonly bool touchScreen = false;

        private static readonly Device instance = new Device();

        static Device()
        {
        }

        private Device()
        {
#if __MOBILE__
            model = DeviceInfo.Model;
            switch (Xamarin.Forms.Device.RuntimePlatform)
            {
                case Xamarin.Forms.Device.iOS:
                    vendor = "Apple Inc.";
                    if (model.Equals("x86_64"))
                    {
                        model = "iOS Simulator";
                    }
                    else
                    {
                        var versionBegin = model.IndexOf( "," );
                        if ( versionBegin > 1 && Char.IsDigit( model, versionBegin - 1 ) ) {

                            --versionBegin;
                        }
                        if ( versionBegin > 1 && Char.IsDigit( model, versionBegin - 1 ) ) {

                            --versionBegin;
                        }
                        version = model.Substring(versionBegin, model.Length - versionBegin);
                        model = model.Substring(0, versionBegin);
                    }
                    break;
                case Xamarin.Forms.Device.Android:
                    vendor = "Google Inc.";
                    if (model.Contains("Android SDK built"))
                    {
                        model = "Android Simulator";
                    }
                    else
                    {
                        string[] androidInfos = model.Split(' ');
                        if (androidInfos.Length == 2)
                        {
                            model = androidInfos[0];
                            version = androidInfos[1];
                        }
                        else
                        {

                            androidInfos = model.Split('-');
                            if (androidInfos.Length == 2)
                            {
                                model = androidInfos[0];
                                version = androidInfos[1];
                            }
                        }
                    }
                    break;
                case Xamarin.Forms.Device.UWP:
                default:
                    string[] infos = model.Split(' ');
                    if ( infos.Length == 2 )
                    {
                        model = infos[0];
                        version = infos[1];
                    }
                    else {

                      infos = model.Split('-');
                      if ( infos.Length == 2 )
                      {
                          model = infos[0];
                          version = infos[1];
                      }
                    }
                    break;
            }
            if (!DeviceInfo.Manufacturer.Equals("unknown"))
            {
                vendor = DeviceInfo.Manufacturer;
            }
            osVersion = DeviceInfo.VersionString;
#else
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\SystemInformation"))
                {
                    if (key != null)
                    {
                        Object value = key.GetValue("SystemManufacturer");
                        if (value != null)
                        {
                            vendor = value as String;
                        }
                        value = key.GetValue("SystemProductName");
                        if (value != null)
                        {
                            model = value as String;
                            string[] infos = model.Split(' ');
                            if (infos.Length == 2)
                            {
                                model = infos[0];
                                version = infos[1];
                            }
                            else
                            {

                                infos = model.Split('-');
                                if (infos.Length == 2)
                                {
                                    model = infos[0];
                                    version = infos[1];
                                }
                            }
                        }
                    }
                    key.Close();
                }
            }
            catch
            {
                // Nothing to handle here!
            }
#endif
        }

        public static Device Instance
        {
            get
            {
                return instance;
            }
        }

        public string UniqueIdentifier()
        {
#if __MOBILE__
            string uuid = Preferences.Get("uuid", "", "group.com.vxstat.statistics");
            if (uuid == null || uuid.Equals(""))
            {
                uuid = System.Guid.NewGuid().ToString();
                Preferences.Set("uuid", uuid, "group.com.vxstat.statistics");
            }
            return uuid;
#else
            string uuid = System.Guid.NewGuid().ToString();
            try
            {
                try
                {
                    RegistryKey createKey = Registry.CurrentUser.OpenSubKey("SOFTWARE");
                    createKey.CreateSubKey("group.com.vxstat.statistics");
                    createKey.CreateSubKey("OrganizationDefaults");
                    createKey.Close();
                }
                catch
                {
                    // Do nothing, this is called, if the entry is already given.
                }

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\group.com.vxstat.statistics\\OrganizationDefaults", true))
                {
                    if (key != null)
                    {
                        Object value = key.GetValue("uuid");
                        if (value != null)
                        {
                            uuid = value as string;
                        }
                        else
                        {
                            key.SetValue("uuid", uuid);
                        }
                        key.Close();
                    }
                }
            }
            catch
            {
                // Nothing to handle here!
            }
            return uuid;
#endif
        }

        public string Model
        {
            get
            {
                return model;
            }

            set
            {
                model = value;
            }
        }

        public string Vendor
        {
            get
            {
                return vendor;
            }

            set
            {
                vendor = value;
            }
        }

        public string Version
        {
            get
            {
                return version;
            }

            set
            {
                version = value;
            }
        }

        public string OsVersion
        {
            get
            {
                return osVersion;
            }
        }

        public bool UseDarkMode()
        {
#if __MOBILE__
#else
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize"))
                {
                    if (key != null)
                    {
                        Object value = key.GetValue("AppsUseLightTheme");
                        if (value != null)
                        {
                            if (Convert.ToInt32(value) == 0)
                            {
                                darkMode = true;
                            }
                        }
                        key.Close();
                    }
                }
            }
            catch
            {
                // Nothing to handle here!
            }
#endif
            return darkMode;
        }

        public bool IsJailbroken()
        {
            return jailbroken;
        }

        public bool IsTabletMode()
        {
#if __MOBILE__
            switch (Xamarin.Forms.Device.RuntimePlatform)
            {
                case Xamarin.Forms.Device.iOS:
                    return true;
                case Xamarin.Forms.Device.Android:
                    return true;
            }
#else
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ImmersiveShell"))
                {
                    if (key != null)
                    {
                        Object value = key.GetValue("TabletMode");
                        if (value != null)
                        {
                            if (Convert.ToInt32(value) == 1)
                            {
                                tabletMode = true;
                            }
                        }
                        key.Close();
                    }
                }
            }
            catch
            {
                // Nothing to handle here!
            }
#endif
            return tabletMode;
        }

        public bool HasTouchScreen()
        {
#if __MOBILE__
            switch (Xamarin.Forms.Device.RuntimePlatform)
            {
                case Xamarin.Forms.Device.iOS:
                    return true;
                case Xamarin.Forms.Device.Android:
                    return true;
            }
#endif
            // TODO: Check for touchscreen
            return touchScreen;
        }

        // TODO: Check for voiceover
        public bool IsVoiceOverActive()
        {
            return false;
        }
    }
}
