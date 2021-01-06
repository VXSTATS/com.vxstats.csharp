﻿using System;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Linq;
using System.Diagnostics;
#if __MOBILE__
using Xamarin.Essentials;
#else
using System.Drawing;
using System.Windows.Forms;
#endif

namespace vxstats
{
    public sealed class Statistics
    {
        private int baseLength = 255;

        private static string m_serverFilePath = "";

        private static string m_username = "";

        private static string m_password = "";

        private static string m_lastPage = "";

        private static readonly Statistics instance = new Statistics();

        static Statistics()
        {
        }

        private Statistics()
        {
        }

        public static Statistics Instance
        {
            get
            {
                return instance;
            }
        }

        public void setServerFilePath(string _serverFilePath)
        {
            if (m_serverFilePath.Equals(_serverFilePath))
            {
                return;
            }
            m_serverFilePath = _serverFilePath;
        }

        public void setUsername(string _username)
        {
            if (m_username.Equals(_username))
            {
                return;
            }
            m_username = _username;
        }

        public void setPassword(string _password)
        {
            if (m_password.Equals(_password))
            {
                return;
            }
            m_password = _password;
        }

        public void page(string _page)
        {
            string page = _page;
            if (page.Equals(""))
            {
                Console.WriteLine("Bad implementation - page with empty 'page'");
                return;
            }

            if (page.Length > baseLength)
            {
                Console.WriteLine("Bad implementation - 'page': {0} is larger than {1} signs", page, baseLength);
                page = page.Substring(0, baseLength);
            }
            m_lastPage = page;
            action("");
        }

        public void action( string _action, string _value = "" )
        {
            if (m_lastPage.Equals(""))
            {
                Console.WriteLine("Bad implementation - 'action': {0} with empty 'page'", _action);
            }

            string action = _action;
            if (action.Length > baseLength)
            {
                Console.WriteLine("Bad implementation - 'action': {0} is larger than {1} signs", action, baseLength);
                action = action.Substring(0, baseLength);
            }

            string value = _value;
            if (value.Length > baseLength)
            {
                Console.WriteLine("Bad implementation - 'value': {0} is larger than {1} signs", value, baseLength);
                value = value.Substring(0, baseLength);
            }

            NameValueCollection message = coreMessage();
            if (!_action.Equals(""))
            {
                message["action"] = _action;
            }
            if (!_value.Equals(""))
            {
                message["value"] = _value;
            }
            sendMessage( message );
        }

        public void ads(string _campaign)
        {
            string campaign = _campaign;
            if (campaign.Equals(""))
            {
                Console.WriteLine("Bad implementation - 'ads' with empty 'campaign' name, 'page': {0}", m_lastPage);
            }
            else if (campaign.Length > baseLength)
            {
                Console.WriteLine("Bad implementation - 'campaign': {0} is larger than {1} signs", campaign, baseLength);
                campaign = campaign.Substring(0, baseLength);
            }
            action("ads", _campaign);
        }

        public void move(double _latitude, double _longitude)
        {
            if (_latitude == 0.0 || _longitude == 0.0)
            {
                Console.WriteLine("Bad implementation - 'move' with empty 'latitude' or 'longitude'");
            }
            action("move", String.Format("{0},{1}", _latitude, _longitude));
        }

        public void open(string _urlOrName)
        {
            string urlOrName = _urlOrName;
            if (urlOrName.Equals(""))
            {
                Console.WriteLine("Bad implementation - 'open' with empty 'urlOrName' name, 'page': {0}", m_lastPage);
            }
            else if (urlOrName.Length > baseLength)
            {
                Console.WriteLine("Bad implementation - 'urlOrName': {0} is larger than {1} signs", urlOrName, baseLength);
                urlOrName = urlOrName.Substring(0, baseLength);
            }
            action("open", _urlOrName);
        }

        public void play( string _urlOrName )
        {
            string urlOrName = _urlOrName;
            if (urlOrName.Equals(""))
            {
                Console.WriteLine("Bad implementation - 'play' with empty 'urlOrName' name, 'page': {0}", m_lastPage);
            }
            else if (urlOrName.Length > baseLength)
            {
                Console.WriteLine("Bad implementation - 'urlOrName': {0} is larger than {1} signs", urlOrName, baseLength);
                urlOrName = urlOrName.Substring(0, baseLength);
            }
            action("play", _urlOrName);
        }

        public void search(string _text)
        {
            string text = _text;
            if (text.Equals(""))
            {
                Console.WriteLine("Bad implementation - 'search' with empty 'text' name, 'page': {0}", m_lastPage);
            }
            else if (text.Length > baseLength)
            {
                Console.WriteLine("Bad implementation - 'text': {0} is larger than {1} signs", text, baseLength);
                text = text.Substring(0, baseLength);
            }
            action("search", text);
        }

        public void shake()
        {
            action("shake");
        }

        public void touch( string _action )
        {
            string action_ = _action;
            if (action_.Equals(""))
            {
                Console.WriteLine("Bad implementation - 'touch' with empty 'text' name, 'page': {0}", m_lastPage);
            }
            else if (action_.Length > baseLength)
            {
                Console.WriteLine("Bad implementation - 'action': {0} is larger than {1} signs", action_, baseLength);
                action_ = action_.Substring(0, baseLength);
            }
            action( "touch", action_);
        }

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117,

            // http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
        }

#if __MOBILE__
#else
        private float getScalingFactor()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

            float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;

            return ScreenScalingFactor; // 1.25 = 125%
        }
#endif

        private NameValueCollection coreMessage() {

            NameValueCollection result = new NameValueCollection();

            result["uuid"] = vxstats.Device.Instance.uniqueIdentifier();

            var os = Environment.OSVersion;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                result["os"] = "Windows";
                if (os.Version.Major == 10 && os.Version.Minor == 0)
                {
                    result["osversion"] = "10";
                }
                else if (os.Version.Major == 6 && os.Version.Minor == 3)
                {
                    result["osversion"] = "8.1";
                }
                else if (os.Version.Major == 6 && os.Version.Minor == 2)
                {
                    result["osversion"] = "8";
                }
                else if (os.Version.Major == 6 && os.Version.Minor == 1)
                {
                    result["osversion"] = "7";
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                result["os"] = "Linux";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                result["os"] = "macOS";
            }
            else
            {
                result["os"] = "Unknown";
            }

#if __MOBILE__
            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
            {
                result["osversion"] = os.VersionString;
                result["os"] = "iOS";
            }
            else if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
            {
                result["osversion"] = os.VersionString;
                result["os"] = "Android";
            }
#endif

            if (!vxstats.Device.Instance.model().Equals(""))
            {
                result["model"] = vxstats.Device.Instance.model();
            }

            if (!vxstats.Device.Instance.version().Equals(""))
            {
                result["modelversion"] = vxstats.Device.Instance.version();
            }

            if (!vxstats.Device.Instance.vendor().Equals(""))
            {
                result["vendor"] = vxstats.Device.Instance.vendor();
            }

            CultureInfo cultureInfo = CultureInfo.InstalledUICulture;
            string[] locale = cultureInfo.Name.Split('-');
            result["language"] = locale[0];
            result["country"] = locale[1];

            /* connection - 'Bluetooth','Ethernet','Offline','Unknown','Wifi','WWAN' */
#if __MOBILE__
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                var profiles = Connectivity.ConnectionProfiles;
                if (profiles.Contains(ConnectionProfile.Bluetooth))
                {
                    result["connection"] = "Bluetooth";
                }
                else if (profiles.Contains(ConnectionProfile.Cellular))
                {
                    result["connection"] = "WWAN";
                }
                else if (profiles.Contains(ConnectionProfile.Ethernet))
                {
                    result["connection"] = "Ethernet";
                }
                else if (profiles.Contains(ConnectionProfile.Unknown))
                {
                    result["connection"] = "Unknown";
                }
                else if (profiles.Contains(ConnectionProfile.WiFi))
                {
                    result["connection"] = "Wifi";
                }
            }
            else
            {
                result["connection"] = "Offline";
            }
#else
//            ConnectionProfile profile = System.Net.NetworkInformation.GetInternetConnectionProfile();
//            if (profile.IsWwanConnectionProfile)
//            {
                result["connection"] = "Wifi";
#endif

            result["appid"] = vxstats.App.Instance.identifier();
            result["appversion"] = vxstats.App.Instance.version();
            string build = vxstats.App.Instance.build();
            if (!build.Equals(""))
            {
                result["appbuild"] = build;
            }

            if (vxstats.Device.Instance.useDarkMode())
            {
                result["dark"] = "1";
            }

            if (vxstats.App.Instance.fairUse())
            {
                result["fair"] = "1";
            }

            if (vxstats.Device.Instance.isJailbroken())
            {
                result["free"] = "1";
            }

            if (vxstats.Device.Instance.isTabletMode())
            {
                result["tabletmode"] = "1";
            }

            if (vxstats.Device.Instance.hasTouchScreen())
            {
                result["touch"] = "1";
            }

            if (vxstats.Device.Instance.isVoiceOverActive())
            {
                result["voiceover"] = "1";
            }

#if __MOBILE__
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var orientation = mainDisplayInfo.Orientation;
            var rotation = mainDisplayInfo.Rotation;

            var width = mainDisplayInfo.Width;
            var height = mainDisplayInfo.Height;
            var density = mainDisplayInfo.Density;

            result["width"] = width.ToString();
            result["height"] = height.ToString();
            result["dpr"] = density.ToString();
#else
            string screenWidth = Screen.PrimaryScreen.Bounds.Width.ToString();
            string screenHeight = Screen.PrimaryScreen.Bounds.Height.ToString();
            result["width"] = screenWidth;
            result["height"] = screenHeight;
            result["dpr"] = getScalingFactor().ToString();
#endif

            long epochTicks = new DateTime(1970, 1, 1).Ticks;
            long unixTime = ((DateTime.UtcNow.Ticks - epochTicks) / TimeSpan.TicksPerSecond);
            result["created"] = unixTime.ToString();
            result["page"] = m_lastPage;

            return result;
        }

        private void sendMessage(NameValueCollection _message ) {

            using (var wb = new WebClient())
            {
                wb.Credentials = new NetworkCredential(m_username, m_password);
                try
                {
#if DEBUG
                    foreach (string s in _message)
                        foreach (string v in _message.GetValues(s))
                        {
                            Debug.WriteLine("{0} {1}", s, v);
                            Console.WriteLine("{0} {1}", s, v);
                        }
#endif
                    var response = wb.UploadValues(m_serverFilePath, "POST", _message);
                    string responseInString = Encoding.UTF8.GetString(response);
                }
                catch
                {
                    // TODO: Spooling
                }
            }
        }
    }
}
