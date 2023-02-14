# WebTV Partition Editor

Mirror of the source code for the WebTV Partition Editor tool written by WebTV hacker Eric MacDonald (eMac). The tool allows users to mount and edit the proprietary hard drive partitions used by some [WebTV](https://en.wikipedia.org/wiki/MSN_TV) (or MSN TV) internet set-top boxes onto their PC, either from a image file or a physical hard drive. WebTV Partition Editor supports hard drive partitions used by WebTV Plus, Echostar DishPlayer, UltimateTV (DirecTV), and Japanese boxes. **This tool will not work with MSN TV 2, as that uses a normal FAT16 CompactFlash card to store data.**

A compiled version of this program already exists at https://turdinc.kicks-ass.net/Msntv/eMac/index.html, but this source code is being uploaded with permission for posterity.

WebTV Partition Editor is confirmed to work on Windows 7 and higher, and will likely work on Windows Vista as well. It is also confirmed to run on Windows XP, but is known to crash either when mounting physical drives or images. This tool requires the [ImDisk Virtual Disk Driver](http://www.ltr-data.se/opencode.html/#ImDisk) to be installed before it can function properly.

This version of the WebTV Partition Editor code dates back to January 9, 2021 and has not received any public updates since. This code is being provided **AS-IS**. Use at your own risk.

## Supported Features
- Mount WebTV/MSN TV hard drive partitions from an image file or a physical drive.
- View proprietary WebTV/MSN TV partitions - FAT16, FAT16 DVR (DishPlayer), boot partitions (WebTV/MSN TV .o binaries)
- Add, remove, and delete proprietary WebTV/MSN TV partitions
- Explore and edit WebTV/MSN TV partitions (FAT16 and boot partitions only)
- Mounting multiple drives
- Adding and modifying boot partitions
- Viewing build information from installed WebTV/MSN TV builds
