#TECHNICAL PREVIEW

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

# Preparation
Checkout and open project with Visual Studio.

## Setup
Setup your environment with your credentials. Please insert your username, password and url here. For defuscation please follow our best practice documentation.
```c#
vxstats.Statistics statistics = new vxstats.Statistics();
statistics.setUsername( "sandbox" );
statistics.setPassword( "sandbox" );
statistics.setServerFilePath( "https://sandbox.vxstats.com" );
```

## Page
This is the global context, where you are currently on in your application. Just name it easy and with logical app structure to identify where the user stays.
```c#
statistics.page("Main");
```

## Event
When you would like to request a page with dynamic content please use this function.
```c#
statistics.action("action", "value");
```

### Ads
To capture ads - correspondingly the shown ad.
```c#
statistics.ads("$campain");
```

### Move
To capture map shifts - correspondingly the new center.
```c#
statistics.move($latitude, $longitude);
```

### Open
To capture open websites or documents including the information which page or document has been requested.
```c#
statistics.open("$urlOrName");
```

### Play
To capture played files including the information which file/action has been played.
```c#
statistics.play("$urlOrName");
```

### Search
To capture searches including the information for which has been searched.
```c#
statistics.search("$search");
```

### Shake
To capture when the device has been shaken.
```c#
statistics.shake();
```

### Touch
To capture typed/touched actions.
```c#
statistics.touch("$action");
```

# Compatiblity
## Android
## iOS
## Windows
