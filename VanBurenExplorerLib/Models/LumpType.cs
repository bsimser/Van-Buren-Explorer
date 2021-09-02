namespace VanBurenExplorerLib.Models
{
    /// <summary>
    /// Each lump in a .grp file has an associated type
    /// </summary>
    public enum LumpType
    {
        G3D = 100,
        TGA = 200,
        SMA = 300,
        TRE = 400,
        SKEL = 600,
        ANIM = 700,
        INI = 800,
        WAV = 1100,
        SCR = 1200,
        MAP = 1300,
        CRITTER = 1400,
        VFX = 1500,
        GUI = 1600,
        COL = 1700,
        ITEM = 1800,
        SND_CONF = 1900,
        SND_LST = 2000,
        SND_LST_2 = 2100,
        SND_TXT = 2200,
        SND_TXT_2 = 2300,
        WEAPON = 2400,
        ARMOR = 2500,
        DOOR = 2600,
        USE = 2700,
        AMMO = 3500,
        CON = 3600,
        TXT = 3700
    }
}