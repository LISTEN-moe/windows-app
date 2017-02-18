# ListenMoeClient
A simple lightweight [Listen.moe](https://listen.moe) client for Windows.

![](http://i.imgur.com/nQuZyh5.gif)

The window will snap to the edges of your screen, and you can mouse scroll on the window to increase/decrease the volume. You can also set it to be topmost by right clicking the window and selecting 'Always on top'. 

# Instructions

## Installation

### Windows
Download the latest release from [here](https://github.com/anonymousthing/ListenMoeClient/releases) and run it.

### Debian
1) Download and install the alpha channel of Mono (currently 4.8) by running the following commands in your favourite shell:
    ```
    echo "deb http://download.mono-project.com/repo/debian alpha main" | sudo tee /etc/apt/sources.list.d/mono-xamarin-alpha.list  
    sudo apt-get update
    sudo apt-get install mono-complete
    ```
2) Download and install libopenal with 
    ```
    sudo apt-get install libopenal1
    ```
3) Download the latest release from [here](https://github.com/anonymousthing/ListenMoeClient/releases)
4) Download [this config file](https://raw.githubusercontent.com/anonymousthing/ListenMoeClient/master/ListenMoe.exe.config) and place it in the same location where you downloaded `ListenMoe.exe`
5) Run it with `mono ListenMoe.exe`

## Updates
Auto-updates are baked into the app (updates are checked on startup only though), which will check for updates, download the latest version and restart the app automatically for you if you click OK. It backs up your older version to `ListenMoe.bak` in the same folder. You can disable update checking on startup by changing `bIgnoreUpdates` in `listenMoeSettings.ini` to `True`. 
If you have disabled updates and find that music no longer plays/song info no longer updates, you may want to check for updates; we may have changed the stream/song info API.

## Other notes
If you find yourself unable to see the window (for example if you disconnect a monitor, or change your monitor resolutions), delete `listenMoeSettings.ini` and restart the application. This will reset the remembered location.

# Todo
 - Code cleanup
 - Hide from alt-tab
 - Taskbar media player controls, global media hotkey hooks
 - Network disconnect detection (and reconnection) -- right now you can just pause and hit play again to reconnect if your network disconnects. 
