namespace VanBurenExplorerLib.Models
{
    public class Entry
    {
        public int Version { get; set; }
        // index of entry in the grp file
        public int Index { get; set; }
        // the type of lump this entry represents
        public LumpType type { get; set; }
        // raw offset into original entry names (from the binary file)
        public int RawOffsetToName { get; set; }
        // raw offset into original .grp names (from the binary file)
        public int RawOffsetToGroupName { get; set; }
        // name of the grp file this entry is for
        public string GroupName { get; set; }
        // name of the entry itself
        public string Name { get; set; }
    }
}