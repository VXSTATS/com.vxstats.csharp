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
    /**
     * @~english
     * @brief The App class.
     * General information about the running application including validation of
     * fair use.
     *
     * @~german
     * @brief Die Klasse App.
     * Bietet allgemeine Informationen über die Anwendung und eine Überprüfung
     * der fairen Verwendung.
     */
    public sealed class App
    {
        /**
         * @~english
         * @brief The application identifier.
         *
         * @~german
         * @brief Id der Anwendung.
         */
        private static string identifier;

        /**
         * @~english
         * @brief The application version.
         *
         * @~german
         * @brief Version der Anwendung.
         */
        private static string version;

        /**
         * @~english
         * @brief The application build.
         *
         * @~german
         * @brief Build der Anwendung.
         */
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

        /**
         * @~english
         * @brief Instance of App. Singleton thread-safe.
         * @return Singleton of App.
         *
         * @~german
         * @brief Instanz von App. Singleton thread-safe.
         * @return Einzige Instanz von App.
         */
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
