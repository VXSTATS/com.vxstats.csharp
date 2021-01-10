﻿using System;
#if __MOBILE__
using Xamarin.Essentials;
#endif

namespace vxstats
{
    public sealed class Device
    {
        private static string model = "";

        private static string vendor = "";

        private static string version = "";

        private static string osVersion = "0.0.0";

        private static readonly bool darkMode = false;

        private static readonly bool jailbroken = false;

        private static readonly bool tabletMode = false;

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
#endif
            // TODO: Check for TabletMode on Windows
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
        public bool IsVoiceOverActive() { return false; }
    }
}
