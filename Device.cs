using System;
#if __MOBILE__
using Xamarin.Essentials;
#else
using Microsoft.Win32;
#endif

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
                    if ( model.Equals( "x86_64" ) )
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
            vendor = DeviceInfo.Manufacturer;
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
                // TODO: Nothing to handle here?
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

        // TODO: Persistence uuid for non mobile
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
            // TODO: Fix configuration/preferences
            string uuid = ""; // d = Properties.Settings.Default["uuid"];
            if (uuid == null || uuid.Equals(""))
            {
                uuid = System.Guid.NewGuid().ToString();
//                Properties.Settings.Default["uuid"] = uuid;
//                Properties.Settings.Default.Save();
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
                    }
                    key.Close();
                }
            }
            catch
            {
                // TODO: Nothing to handle here?
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
                    }
                    key.Close();
                }
            }
            catch
            {
                // TODO: Nothing to handle here?
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
