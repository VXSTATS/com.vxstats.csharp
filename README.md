# TECHNICAL PREVIEW

* [Preparation](#preparation)
* [Implementation](#implementation)
   * [Pre-Setup](#pre-setup)
   * [Setup](#setup)
   * [Page](#page)
   * [Event](#event)
      * [Ads](#ads)
      * [Move](#move)
      * [Open](#open)
      * [Play](#play)
      * [Search](#search)
      * [Shake](#shake)
      * [Touch](#touch)
* [Compatiblity](#compatiblity)
   * [Android](#android)
   * [iOS](#ios)
   * [Windows](#windows)
* [Known Issues](#known-issues)
   * [Windows](#windows-1)

# Preparation
Checkout and open project with Visual Studio.

## Setup
Setup your environment with your credentials. Please insert your username, password and url here. For defuscation please follow our best practice documentation.
```c#
Statistics.Instance.Username = "sandbox";
Statistics.Instance.Password = "sandbox";
Statistics.Instance.ServerFilePath = "https://sandbox.vxstats.com";
```

## Page
This is the global context, where you are currently on in your application. Just name it easy and with logical app structure to identify where the user stays.
```c#
Statistics.Instance.Page("Main");
```

## Event
When you would like to request a page with dynamic content please use this function.
```c#
Statistics.Instance.Action("action", "value");
```

### Ads
To capture ads - correspondingly the shown ad.
```c#
Statistics.Instance.Ads("$campain");
```

### Move
To capture map shifts - correspondingly the new center.
```c#
Statistics.Instance.Move($latitude, $longitude);
```

### Open
To capture open websites or documents including the information which page or document has been requested.
```c#
Statistics.Instance.Open("$urlOrName");
```

### Play
To capture played files including the information which file/action has been played.
```c#
Statistics.Instance.Play("$urlOrName");
```

### Search
To capture searches including the information for which has been searched.
```c#
Statistics.Instance.Search("$search");
```

### Shake
To capture when the device has been shaken.
```c#
Statistics.Instance.Shake();
```

### Touch
To capture typed/touched actions.
```c#
Statistics.Instance.Touch("$action");
```

# Compatiblity
## Android
## iOS
## Windows

# Known Issues
## Windows
1. For applications that have been manifested for Windows 8.1 or Windows 10. Applications not manifested for Windows 8.1 or Windows 10 will return the Windows 8 OS version value (6.2). To manifest your applications for Windows 8.1 or Windows 10, refer to Targeting your application for Windows. - https://docs.microsoft.com/de-de/windows/win32/api/winnt/ns-winnt-osversioninfoexa#remarks
