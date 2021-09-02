# Van Buren Explorer

A Windows File-like Explorer tool to view and export the contents of files from the lost Fallout 3 game, code named "Van Buren", created by Black Isle Studios.

## Table of Contents

- [Overview](#overview)
- [Installation](#installation)
- [License](#license)
- [Links](#links)

## Overview

Van Buren was the project name Black Isle Studios assigned to their version of Fallout 3. The game was going to use an engine that Black Isle had made for Baldur's Gate 3, commonly referred to as the Jefferson Engine. In 2003, the game was canceled due to financial difficulties.

![](Vbtitle.png)

A tech demo of Van Buren was created during the game's development. While the tech demo was not the finished game, it demonstrated the basic interface, gameplay, and interactions the final game might have had. The tech demo did however come with a set of files including textures, meshes, sounds, and text used to display in the demo.

Van Buren Explore is a labour of love and was created to decrypt and view information stored in these files. 

The tech demo can be downloaded from the [Mod DB](https://www.moddb.com/) site [here](https://www.moddb.com/games/van-buren/downloads/van-buren-tech-demo).

## Features

* View contents of .GRP files from the Van Buren tech demo
* Export (most) file formats (see below for the current list of supported ones)
* Full original entity names for viewing and exporting

## File Formats

This is a list of the file formats Van Buren Explorer supports for exporting and viewing:

* TXT
* INI
* WAV
* TGA (24/32 bit, compressed and uncompressed)
* BMP

The following formats are unknown and/or currently unsupported (but can still be viewed in hex mode):

* G3D
* SMA
* TRE
* SKEL
* ANIM
* SCR
* MAP
* CRITTER
* VFX
* GUI
* COL
* ITEM
* WEAPON
* ARMOR
* DOOR
* USE
* AMMO
* CON

(note as viewers are built for these formats they will be moved into the supported section above)

## Requirements

* Visual Studio 2019

## Current Version

The current version is 1.0 and was released 9/2/2021

## Installation

* TODO

## Usage

1. Launch the applicaton from wherever you have downloaded or built it
2. When the application launches, locate the folder where you have your copy of Van Buren installed to
3. Enjoy!

NOTE: If you want to change folders just click on the "..." button on the toolbar to switch to a different directory

## Repository

This repository uses the Gitflow workflow model. All production releases are done on the master branch. The develop branch is the latest and greatest version. New PRs must be done in new branches against the develop branch.

## NuGet packages

This projects uses the following NuGet packages

* [Be.Windows.Form.HexBox](https://www.nuget.org/packages/Be.Windows.Forms.HexBox/) - Viewing uknown formats
* [NAudio](https://www.nuget.org/packages/NAudio/) - WAV player
* [Pfim](https://www.nuget.org/packages/Pfim/) - TGA viewer

## How to Contribute

1. Clone the develop branch of this repository and create a new branch: `$ git checkout https://github.com/bsimser/Van-Buren-Explorer -b name_for_new_branch`
2. Make changes and test
3. Submit a Pull Request with a description of your changes

## Support

* Twitter at [@bsimser](https://twitter.com/bsimser)

## Thanks

Many thanks to a lot of great people far smarter than me. I just compiled a lot of their hard work together into this package and extended on it. Without their great tools this one would not exist.

* RedneckHax0r for his awesome tool dump of Van Buren tools that pointed me to a lot of file info
* Emersont1 for his vb_unpacker that pointed me to some file format info

If you do spot anything that is wrong or you think I should include your name here please let me know!

## License

[![License](http://img.shields.io/:license-mit-blue.svg?style=flat-square)](http://badges.mit-license.org)

Van Buren Explorer is licensed under the [MIT license](http://opensource.org/licenses/mit-license.php) and is available for free.

## Links

* [Issue Tracker](https://github.com/bsimser/Van-Buren-Explorer/issues)
* [Source code](https://github.com/bsimser/Van-Buren-Explorer)

