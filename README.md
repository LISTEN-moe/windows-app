# ListenMoeClient
A simple lightweight [Listen.moe](https://listen.moe) client for Windows.

![](https://my.mixtape.moe/wkrwbo.png)

# Instructions
Download the latest release from [here](https://github.com/anonymousthing/ListenMoeClient/releases).  

The window will snap to any screen edges, and you can mouse scroll on the window to increase/decrease the volume. You can also set it to be topmost by right clicking the window and selecting 'Always on top'. 

## Updates
Auto-updates are baked into the app (updates are checked on startup only though), which will check for updates, download the latest version and restart the app automatically for you if you click OK. It backs up your older version to `ListenMoe.bak` in the same folder. You can disable update checking on startup by changing `bIgnoreUpdates` in `listenMoeSettings.ini` to `True`. I recommend against this though, since we're still changing the Listen.moe API quite frequently, so compatibility breaks happen pretty often. If you have disabled updates and find that music no longer plays/song info no longer updates, you may want to check for updates. 

## Other notes
If you find yourself unable to see the window (for example if you disconnect a monitor, or change your monitor resolutions), delete `listenMoeSettings.ini` and restart the application. This will reset the remembered location.

# Todo
 - Code cleanup
 - Hide from alt-tab
 - Taskbar media player controls, global media hotkey hooks
 - Network disconnect detection (and reconnection) -- right now you can just pause and hit play again to reconnect if your network disconnects. 
