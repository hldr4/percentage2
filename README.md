# percentage2

![](https://raw.githubusercontent.com/kas/percentage/master/percentage.png)

See current battery percentage and charging status in the Windows system tray

## Installing

1. [Download the latest release](https://github.com/hldr4/percentage2/releases)
1. Put percentage.exe in your startup folder (press Windows+R, type "shell:startup", then press Enter to get there)

## Differences from the original
 - Updated everything from the outdated .NET Framework to .NET (currently targeting version 7.0 as 8.0 is [unable](https://github.com/dotnet/sdk/issues/37367) to be published as single file)
 - A lot of code cleanup and simplification
 - Some charging status color changes
 - Added a check that will self-close the app if running on desktop PC
 - Added icon
 - Attempt to center the text when the battery is at 100% charge a bit more evenly
 - Other small formatting changes

## Building from source

This project was compiled with Visual Studio Community 2022.

1. Open the percentage2.sln file with Visual Studio
2. Click "Build > Build Solution"
3. Output can be found in the bin folder

## Credits

 - [kas - original project](https://github.com/kas/percentage)
 - [strayge - some enhanhements](https://github.com/strayge/tray-monitor)
