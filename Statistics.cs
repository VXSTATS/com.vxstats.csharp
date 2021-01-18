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
using System.Net;
using System.Collections.Specialized;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
#if __MOBILE__
using Xamarin.Essentials;
#else
using System.Drawing;
using System.Windows.Forms;
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
    public sealed class Statistics
    {
        private static readonly int baseLength = 255;

        private static string serverFilePath = "";

        private static string username = "";

        private static string password = "";

        private static string _realm;
        private static string _nonce;
        private static string _qop;
        private static string _cnonce;
        private static DateTime _cnonceDate;
        private static int _nc;

        private static string lastPage = "";

        private static readonly Statistics instance = new Statistics();

        static Statistics()
        {
        }

        private Statistics()
        {
            // TODO: Reachability check

            // Remove insecure protocols
            ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
            ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls;
            ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls11;

            // Add secure tls1.2+
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
//            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls13;
        }

        public static Statistics Instance
        {
            get
            {
                return instance;
            }
        }

        public string ServerFilePath
        {
            set
            {
                serverFilePath = value;
            }
        }

        public string Username
        {
            set
            {
                username = value;
            }
        }

        public string Password
        {
            set
            {
                password = value;
            }
        }

        public void Page(string _page)
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
            lastPage = page;
            Action("");
        }

        public void Action(string _action, string _value = "")
        {
            if (lastPage.Equals(""))
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

            NameValueCollection message = CoreMessage();
            if (!action.Equals(""))
            {
                message["action"] = action;
            }
            if (!value.Equals(""))
            {
                message["value"] = value;
            }
            SendMessage( message );
        }

        public void Ads(string _campaign)
        {
            string campaign = _campaign;
            if (campaign.Equals(""))
            {
                Console.WriteLine("Bad implementation - 'ads' with empty 'campaign' name, 'page': {0}", lastPage);
            }
            else if (campaign.Length > baseLength)
            {
                Console.WriteLine("Bad implementation - 'campaign': {0} is larger than {1} signs", campaign, baseLength);
                campaign = campaign.Substring(0, baseLength);
            }
            Action("ads", campaign);
        }

        public void Move(double _latitude, double _longitude)
        {
            if (_latitude == 0.0 || _longitude == 0.0)
            {
                Console.WriteLine("Bad implementation - 'move' with empty 'latitude' or 'longitude'");
            }
            Action("move", String.Format("{0},{1}", _latitude, _longitude));
        }

        public void Open(string _urlOrName)
        {
            string urlOrName = _urlOrName;
            if (urlOrName.Equals(""))
            {
                Console.WriteLine("Bad implementation - 'open' with empty 'urlOrName' name, 'page': {0}", lastPage);
            }
            else if (urlOrName.Length > baseLength)
            {
                Console.WriteLine("Bad implementation - 'urlOrName': {0} is larger than {1} signs", urlOrName, baseLength);
                urlOrName = urlOrName.Substring(0, baseLength);
            }
            Action("open", urlOrName);
        }

        public void Play(string _urlOrName)
        {
            string urlOrName = _urlOrName;
            if (urlOrName.Equals(""))
            {
                Console.WriteLine("Bad implementation - 'play' with empty 'urlOrName' name, 'page': {0}", lastPage);
            }
            else if (urlOrName.Length > baseLength)
            {
                Console.WriteLine("Bad implementation - 'urlOrName': {0} is larger than {1} signs", urlOrName, baseLength);
                urlOrName = urlOrName.Substring(0, baseLength);
            }
            Action("play", urlOrName);
        }

        public void Search(string _text)
        {
            string text = _text;
            if (text.Equals(""))
            {
                Console.WriteLine("Bad implementation - 'search' with empty 'text' name, 'page': {0}", lastPage);
            }
            else if (text.Length > baseLength)
            {
                Console.WriteLine("Bad implementation - 'text': {0} is larger than {1} signs", text, baseLength);
                text = text.Substring(0, baseLength);
            }
            Action("search", text);
        }

        public void Shake()
        {
            Action("shake");
        }

        public void Touch(string _action)
        {
            string action = _action;
            if (action.Equals(""))
            {
                Console.WriteLine("Bad implementation - 'touch' with empty 'text' name, 'page': {0}", lastPage);
            }
            else if (action.Length > baseLength)
            {
                Console.WriteLine("Bad implementation - 'action': {0} is larger than {1} signs", action, baseLength);
                action = action.Substring(0, baseLength);
            }
            Action("touch", action);
        }

#if __MOBILE__
#else
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117,

            // http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
        }
        private float GetScalingFactor()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

            float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;

            return ScreenScalingFactor; // 1.25 = 125%
        }
#endif

        private NameValueCollection CoreMessage() {

            NameValueCollection result = new NameValueCollection
            {
                ["uuid"] = vxstats.Device.Instance.UniqueIdentifier()
            };

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

            if (!vxstats.Device.Instance.Model.Equals(""))
            {
                result["model"] = vxstats.Device.Instance.Model;
            }

            if (!vxstats.Device.Instance.Version.Equals(""))
            {
                result["modelversion"] = vxstats.Device.Instance.Version;
            }

            if (!vxstats.Device.Instance.Vendor.Equals(""))
            {
                result["vendor"] = vxstats.Device.Instance.Vendor;
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
            result["connection"] = "Wifi";
#endif

            result["appid"] = vxstats.App.Instance.Identifier;
            result["appversion"] = vxstats.App.Instance.Version;
            string build = vxstats.App.Instance.Build;
            if (!build.Equals(""))
            {
                result["appbuild"] = build;
            }

            if (vxstats.Device.Instance.UseDarkMode())
            {
                result["dark"] = "1";
            }

            if (vxstats.App.Instance.FairUse())
            {
                result["fair"] = "1";
            }

            if (vxstats.Device.Instance.IsJailbroken())
            {
                result["free"] = "1";
            }

            if (vxstats.Device.Instance.IsTabletMode())
            {
                result["tabletmode"] = "1";
            }

            if (vxstats.Device.Instance.HasTouchScreen())
            {
                result["touch"] = "1";
            }

            if (vxstats.Device.Instance.IsVoiceOverActive())
            {
                result["voiceover"] = "1";
            }

#if __MOBILE__
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
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
            result["dpr"] = GetScalingFactor().ToString();
#endif

            long epochTicks = new DateTime(1970, 1, 1).Ticks;
            long unixTime = ((DateTime.UtcNow.Ticks - epochTicks) / TimeSpan.TicksPerSecond);
            result["created"] = unixTime.ToString();
            result["page"] = lastPage;

            return result;
        }

        private string CalculateMd5Hash(string input)
        {
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = MD5.Create().ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var b in hash)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        private string GrabHeaderVar(string varName, string header)
        {
            var regHeader = new Regex(string.Format(@"{0}=""([^""]*)""", varName));
            var matchHeader = regHeader.Match(header);
            if (matchHeader.Success)
            {
                return matchHeader.Groups[1].Value;
            }
            throw new ApplicationException(string.Format("Header {0} not found", varName));
        }

        private string GetDigestHeader(string dir)
        {
            _nc = _nc + 1;

            var ha1 = CalculateMd5Hash(string.Format("{0}:{1}:{2}", username, _realm, password));
            var ha2 = CalculateMd5Hash(string.Format("{0}:{1}", "POST", dir));
            var digestResponse = CalculateMd5Hash(string.Format("{0}:{1}:{2:00000000}:{3}:{4}:{5}", ha1, _nonce, _nc, _cnonce, _qop, ha2));

            return string.Format("Digest username=\"{0}\", realm=\"{1}\", nonce=\"{2}\", uri=\"{3}\", algorithm=MD5, response=\"{4}\", qop={5}, nc={6:00000000}, cnonce=\"{7}\"", username, _realm, _nonce, dir, digestResponse, _qop, _nc, _cnonce);
        }

        private void SendMessage(NameValueCollection _message) {

            using (var wb = new WebClient())
            {
                NetworkCredential networkCredential = new NetworkCredential(username, password);

                wb.Credentials = networkCredential;

                // If we've got a recent Auth header, re-use it!
                if (!string.IsNullOrEmpty(_cnonce) && DateTime.Now.Subtract(_cnonceDate).TotalHours < 1.0)
                {
                    wb.Headers.Add("Authorization", GetDigestHeader("/"));
                }

                try
                {
#if DEBUG
                    foreach (string s in _message)
                        foreach (string v in _message.GetValues(s))
                        {
                            Debug.WriteLine("{0} {1}", s, v);
                        }
#endif
                    var response = wb.UploadValues(serverFilePath, "POST", _message);
                    string responseInString = Encoding.UTF8.GetString(response);
                }
                catch (WebException ex)
                {
                    // Try to fix a 401 exception by adding a Authorization header
                    if (ex.Response == null || ((HttpWebResponse)ex.Response).StatusCode != HttpStatusCode.Unauthorized)
                    {
                        // TODO: Spooling
                        return;
                    }

                    var wwwAuthenticateHeader = ex.Response.Headers["WWW-Authenticate"];
                    _realm = GrabHeaderVar("realm", wwwAuthenticateHeader);
                    _nonce = GrabHeaderVar("nonce", wwwAuthenticateHeader);
                    _qop = GrabHeaderVar("qop", wwwAuthenticateHeader);

                    _nc = 0;
                    _cnonce = new Random().Next(123400, 9999999).ToString();
                    _cnonceDate = DateTime.Now;

                    try
                    {
                        wb.Headers.Add("Authorization", GetDigestHeader("/"));
                        var response = wb.UploadValues(serverFilePath, "POST", _message);
                        string responseInString = Encoding.UTF8.GetString(response);
                    }
                    catch
                    {
                        // TODO: Spooling

                    }
                }
                catch
                {
                    // TODO: Spooling
                }
            }
        }
    }
}
