using System;
#if __MOBILE__
using Xamarin.Essentials;
#endif

namespace vxstats
{
    public sealed class Device
    {
        private static string m_model = "";

        private static string m_vendor = "";

        private static string m_version = "";

        private static string m_osVersion = "0.0.0";

        private static bool m_darkMode = false;

        private static bool m_jailbroken = false;

        private static bool m_tabletMode = false;

        private static bool m_touchScreen = false;

        private static readonly Device instance = new Device();

        static Device()
        {
        }

        private Device()
        {
#if __MOBILE__
            m_model = DeviceInfo.Model;
            switch (Xamarin.Forms.Device.RuntimePlatform)
            {
                case Xamarin.Forms.Device.iOS:
                    if ( m_model.Equals( "x86_64" ) )
                    {
                        m_model = "iOS Simulator";
                    }
                    else
                    {
                        var versionBegin = m_model.IndexOf( "," );
                        if ( versionBegin > 1 && Char.IsDigit( m_model, versionBegin - 1 ) ) {

                            --versionBegin;
                        }
                        if ( versionBegin > 1 && Char.IsDigit( m_model, versionBegin - 1 ) ) {

                            --versionBegin;
                        }
                        m_version = m_model.Substring(versionBegin, m_model.Length - versionBegin);
                        m_model = m_model.Substring(0, versionBegin);
                    }
                    break;
                case Xamarin.Forms.Device.Android:
                case Xamarin.Forms.Device.UWP:
                default:
                    string[] infos = m_model.Split(' ');
                    if ( infos.Length == 2 )
                    {
                        m_model = infos[0];
                        m_version = infos[1];
                    }
                    else {

                      infos = m_model.Split('-');
                      if ( infos.Length == 2 )
                      {
                          m_model = infos[0];
                          m_version = infos[1];
                      }
                    }
                    break;
            }
            m_vendor = DeviceInfo.Manufacturer;
            m_osVersion = DeviceInfo.VersionString;
#endif
        }

        public static Device Instance
        {
            get
            {
                return instance;
            }
        }

        public string uniqueIdentifier()
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

        public bool fairUse() { return false; }
        public string model() { return m_model; }
        public void setModel(string _model) { m_model = _model; }
        public string vendor() { return m_vendor; }
        public void setVendor(string _vendor) { m_vendor = _vendor; }
        public string version() { return m_version; }
        public void setVersion(string _version) { m_version = _version; }
        public string osVersion() { return m_osVersion; }

        public bool useDarkMode()
        {
            return m_darkMode;
        }

        public bool isJailbroken()
        {
            return m_jailbroken;
        }

        public bool isTabletMode()
        {
#if __MOBILE__
            switch (Xamarin.Forms.Device.RuntimePlatform)
            {
                case Xamarin.Forms.Device.iOS:
                    return true;
                    break;
                case Xamarin.Forms.Device.Android:
                    return true;
                    break;
            }
            return m_tabletMode;
#else
            return m_tabletMode;
#endif
        }

        public bool hasTouchScreen()
        {
#if __MOBILE__
            switch (Xamarin.Forms.Device.RuntimePlatform)
            {
                case Xamarin.Forms.Device.iOS:
                    return true;
                    break;
                case Xamarin.Forms.Device.Android:
                    return true;
                    break;
            }
            return m_touchScreen;
#else
            return m_touchScreen;
#endif
        }

        public bool isVoiceOverActive() { return false; }
    }
}
