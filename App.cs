using System;
#if __MOBILE__
using Xamarin.Essentials;
#endif

namespace vxstats
{
    public sealed class App
    {
        private static string m_identifier;

        private static string m_version;

        private static string m_build;

        private static readonly App instance = new App();

        static App()
        {
        }

        private App()
        {
#if __MOBILE__
            m_identifier = AppInfo.PackageName;
            m_version = AppInfo.VersionString;
            m_build = AppInfo.BuildString;
#endif
        }

        public static App Instance
        {
            get
            {
                return instance;
            }
        }

        public bool fairUse() { return false; }
        public string identifier() { return m_identifier; }
        public void setIdentifier(string _identifier) { m_identifier = _identifier; }
        public string version() { return m_version; }
        public void setVersion(string _version) { m_version = _version; }
        public string build() { return m_build; }
        public void setBuild(string _build) { m_build = _build; }
    }
}
