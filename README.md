<div align="center">
	<img src="https://lolisafe.moe/DJwzPbWD.png" />
</div>
<h2 align="center">Official Windows Client<h2>
<h4 align="center">Simple lightweight <a href="https://listen.moe">LISTEN.moe</a> client for Windows.</h4> 
      
![](https://i.imgur.com/vtEKxw2.gif)

The window will snap to the edges of your screen, and you can mouse scroll on the window to increase/decrease the volume. You can also set it to be topmost by right clicking the window and selecting 'Always on top'. 

# Instructions

## Installation
Download the latest release from [here](https://github.com/anonymousthing/ListenMoeClient/releases) and run it.

## Updates
Auto-updates are baked into the app (updates are checked on startup only though), which will check for updates, download the latest version and restart the app automatically for you if you click OK. It backs up your older version to `ListenMoe.bak` in the same folder. You can disable update checking on startup by enabling "Ignore updates" in the settings screen.
If you have disabled updates and find that music no longer plays/song info no longer updates, you may want to check for updates; we may have changed the stream/song info API.

## Hide from alt-tab
If you would like to hide the program from your alt-tab menu, open `listenMoeSettings.ini` and change the line `bHideFromAltTab=False` to `bHideFromAltTab=True`. A side effect of hiding it from the alt-tab menu is that it will no longer appear in your taskbar either. As such, if this setting is enabled, the System Tray icon will be visible at all times regardless of the "Close to Tray" option. I do not currently have a UI setting for this, as an unsuspecting user may enable it and then "lose" the app underneath many other windows (and not notice the System Tray icon). 

## Other notes
If you find yourself unable to see the window (for example if you disconnect a monitor, or change your monitor resolutions), delete `listenMoeSettings.ini` and restart the application. This will reset the remembered location.

# Todo
 - Code cleanup (let's face it, this will never happen)
 - Taskbar media player controls, global media hotkey hooks
 - Network disconnect detection (and reconnection) -- right now you can just pause and hit play again to reconnect if your network disconnects. 
