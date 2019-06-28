# Total Annihilation Units and Weapons Editor

This is a fork of the Total Annihilation Units and Weapons Editor
originally developed by MacBain on the TAUniverse forums.

The original version can be found at the following location:

https://www.tauniverse.com/forum/showthread.php?t=45855

## Why a Fork?

The original project has numerous bugs, particularly
around reading and saving files.
It routinely makes unexpected edits when saving changed data,
making the tool too dangerous to use as an editor.

This fork uses a modified version of [TAUtil](https://github.com/MHeasell/TAUtil)
as a basis for reading and saving logic.
It makes precise edits to files in place,
changing values while preserving existing formatting and comments.

**WARNING**: This software comes with absolutely no warranty.
It is recommended that you back up existing data
or place it under version control before using this tool
and review changes carefully.
