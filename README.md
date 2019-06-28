# Total Annihilation Units and Weapons Editor

[![Build status](https://ci.appveyor.com/api/projects/status/ps1dvk06s0428avx/branch/master?svg=true)](https://ci.appveyor.com/project/MHeasell/ta-editor/branch/master)

This is a fork of the Total Annihilation Units and Weapons Editor
originally developed by MacBain on the TAUniverse forums.

The original version can be found at the following location:

https://www.tauniverse.com/forum/showthread.php?t=45855

## Info

This tool is designed to manipulate Total Annihilation data files.
Before you can read any data you need to extract the .ufo, .hpi or .gp3
files you want to manipulate. Use a HPI extracting tool, e.g. HPIView,
to do this. When you have extracted all the files, you need to specify
the folder path where you have extracted the files. This folder must
contain a 'units' folder that contains .fbi files. If you want to edit
weapon files, the folder must also contain a 'weapons' folder containing
.tdf files.

Some mods use custom weapon and unit folder names
of the format 'unitsX' or 'weaponX', where X is any letter.
These are also recognised by the tool.

**Note:** It is recommended to make a backup of all unit and weapon
files before editing.

Click on the specific button to read unit or weapon files. Weapons are
always corresponding to the shown units, so be sure to read some units
first.

Edit the values in the specific cells manually or use the calculation
functions on the selected cells. After you are finished you need to save
the edited data. This will update the unit and weapon files on disk to
reflect the changes made in the table. The last step to do is to pack
the folder back into a HPI archive and place it into your TA folder. Use
a HPI packing tool, e.g. HPIPack, to do this.

The original author of this program is Pascal Wauer. This fork contains
modifications by Michael Heasell. Use this program at your own risk.

## Why a Fork?

The original project has numerous bugs, particularly around reading and
saving files. It routinely makes unexpected edits when saving changed
data, making the tool too dangerous to use as an editor.

This fork uses a modified version of
[TAUtil](https://github.com/MHeasell/TAUtil) as a basis for reading and
saving logic. It makes precise edits to files in place, changing values
while preserving existing formatting and comments.

**WARNING**: This software comes with absolutely no warranty. It is
recommended that you back up existing data or place it under version
control before using this tool and review changes carefully.
