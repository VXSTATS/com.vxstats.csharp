using System;
#if __MOBILE__
using Xamarin.Essentials;
#endif

namespace vxstats
{
    public class Device
    {
        private static string m_model;

        private static string m_vendor;

        private static string m_version;

        private static string m_osVersion = "0.0.0";

        private static bool m_darkMode = false;

        private static bool m_jailbroken = false;

        private static bool m_tabletMode = false;

        private static bool m_touchScreen = false;

        public Device()
        {
#if __MOBILE__
            m_model = DeviceInfo.Model;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    if ( m_model.Equals( ""x86_64"" ) )
                    {
                        m_model = "iOS Simulator";
                    }
                    else
                    {
/*                      var versionBegin = m_model.indexOf( "," );
                      if ( versionBegin > 1 && hwmodel.at( versionBegin - 1 ).isDigit() ) {

        --versionBegin;
      }
      if ( versionBegin > 1 && hwmodel.at( versionBegin - 1 ).isDigit() ) {

        --versionBegin;
      } */
                    }
                    break;
                case Device.Android:
                    string[] infos = m_model.Split(' ');
                    if ( infos.Length() == 2 )
                    {
                        m_model = infos[0];
                        m_version = infos[1];
                    }
                    else {

                      infos = m_model.Split('-');
                      if ( infos.Length() == 2 )
                      {
                          m_model = infos[0];
                          m_version = infos[1];
                      }
                    }
                    break;
                case Device.UWP:
                default:
                    break;
            }
            m_vendor = DeviceInfo.Manufacturer;
            m_osVersion = DeviceInfo.VersionString;
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
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    return true;
                    break;
                case Device.Android:
                    return true;
                    break;
            }
#else
            return m_tabletMode;
#endif
        }

        public bool hasTouchScreen()
        {
#if __MOBILE__
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    return true;
                    break;
                case Device.Android:
                    return true;
                    break;
            }
#else
            return m_touchScreen;
#endif
        }

        public bool isVoiceOverActive() { return false; }
    }
}
