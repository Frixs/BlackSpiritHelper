# Black Spirit Helper

<p align="center">
    <img src="Resources/logo_red_text_512.png" alt="Logo Black Spirit Helper" width="256" style="text-align:center;">
</p>

**Are you tired of forgetting important tasks in a game such as... daily rewards, world boss spawns, buff activation at proper time and so on?**  
Or... are you finding hard to track all your tasks you want to do in-game?  
  
Black Spirit Helper can **solve your problem!** You can also set the application to start with your operating system to do not miss any task you want to do, accidentally.  
**Black Spirit Helper offers** several features that can manage these things with multiple types of adjustable timers.  
Oh... I almost forgot to tell you, the application has **own overlay!** So, you can easily track your timers in-game without needs to switch to your desktop.
### Is it legal to use?
Of course! Black Spirit Helper does **not** affect any game files. It does **not** track any screen events. It is just an advanced timer. I ask you... is Windows OS build-in timer illegal? It is in the same category as other software like [Discord](https://discordapp.com/), [TeamSpeak](https://www.teamspeak.com), etc.

## Table of Contents
- **[Features](#features)**
- **[Installation (Download)](#installation)**
- **[Credits](#credits)**
- **[License](#license)**
- **[TODO List](#todo-list)**
- **[Release History](#release-history)**

## Features
### Schedule Section
This feature allows you to build your own schedule. It can be very useful in any kind of game where you need to track down special events at particular time. The application can start with your operating system, so you will never miss any event again. This section has already some predefined templates, so you do not need to create your own from scratch. For example, there are already predefined templates for [Black Desert Online](https://www.blackdesertonline.com) world boss spawns.  
The application allows to user to create his own templates with own items. This allows you to update your schedule according to your own needs.
### Daily Check Section
**Work in progress.** Set your custom message and Black Spirit Helper will nottify you each time period you set. You have only 2 options... say OK or notify me later. Elegant and simple. ...like a normal alarm clock.  
In [Black Desert Online](https://www.blackdesertonline.com), you can find it useful for daily boss scrolls pickup or guild daily reward, for example.
### Timer Section
It offers to you to create multiple groups with multiple timers in each. You can control each timer independently, but you can also control multiple timers thanks to group. You can find it useful in all kinds of games whre you need to track self-buffs/food-buffs/potion-buffs at proper time.
### Watchdog Section
**Work in progress.** This feature checks your internet connection, physical device input access and it also checks your game's process to determine, if you game is running or sitting in login screen (stuck in black/disconnection screen).  
Everything just to avoid game disconnections over night!  
Based on what happends you can set events like send a message to your favorite platform to inform you about the situation or just simply shutdown/restart your computer.

## Installation
#### Requirements
- **Windows**: .NET 4.7.2+

#### Installation process
1. Download installer -> **[setup.exe](Release/setup.exe?raw=true)**.
2. Turn off your antivirus. It may cause issues while installation due to you do not have installed my publisher certificate on your computer. You will see me under the name "Unknown Publisher". If you would like to install certificate to prevent this, you can follow the installation [here](https://github.com/Frixs/BlackSpiritHelper/wiki/CertificateInstallation) (It is not required).
3. Run the installer (setup.exe) and follow the installation process.
4. Your application is installed!
    - Now, you can check, **[Getting Started](https://github.com/Frixs/BlackSpiritHelper/wiki/GettingStarted)** page!
    
#### Updating your application
The application automatically tracks a new updates and download them if needed before each start of the application. Use application shortcut created in the start menu or on your desktop to run the application with update check. If you run the application exe file directly, the update check will not run.

## Credits
#### Author
[@Tomáš Frixs](https://github.com/Frixs)  
If you want, you can **[Donate Me!](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=QE2V3BNQJVG5W&source=url)**

## License
[GNU General Public License v3.0](https://github.com/Frixs/BlackSpiritHelper/blob/master/LICENSE)

## TODO List
- [ ] Daily Check Section.
- [ ] Voice Alerts.
- [x] Watchdog Section.
- [ ] HW Optimalization.
- [x] Loading info screen.

## Release History
```
[26/10/2019] [1.2.2.0, BETA] - Added Schedule Custom Templates + user settings revamp.
[15/08/2019] [1.2.1.1, BETA] - Added Schedule section (beta).
[29/07/2019] [1.1.2.4, BETA] - Added custom settings provider due to issues with locating settings generated by MS while you are in Administrator mode or not. Timer code optimalized/recode. Added Setup methods into ApplicationDataContent, we are loading everything at the same time from the settings, so we do not want relations to IoC.DataContent inside during creation. Instead of that, we use setup methods which runs after creation to do the rest of work.
[26/07/2019] [1.0.2.0, BETA] - Overlay bug fixes. Added keypress to activate interaction with the overlay. Added group control buttons into overlay.
[25/07/2019] [1.0.1.0, BETA] - The application launch!
```
