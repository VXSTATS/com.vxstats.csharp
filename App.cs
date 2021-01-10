using System;
#if __MOBILE__
using Xamarin.Essentials;
#endif

namespace vxstats
{
    public sealed class App
    {
        private static string identifier;

        private static string version;

        private static string build;

        private static readonly App instance = new App();

        static App()
        {
        }

        private App()
        {
#if __MOBILE__
            identifier = AppInfo.PackageName;
            version = AppInfo.VersionString;
            build = AppInfo.BuildString;
#endif
        }

        public static App Instance
        {
            get
            {
                return instance;
            }
        }

        // TODO: Correct signed on Android or iOS
        public bool FairUse() { return false; }

        public string Identifier
        {
            get
            {
                return identifier;
            }

            set
            {
                identifier = value;
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

        public string Build
        {
            get
            {
                return build;
            }

            set
            {
                build = value;
            }
        }
    }
}
