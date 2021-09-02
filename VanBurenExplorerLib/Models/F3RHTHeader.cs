namespace VanBurenExplorerLib.Models
{
    public class F3RHTHeader
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int NumEntries { get; set; }
        public int OffsetToGroupNames { get; set; }
        public int OffsetToResourceNames { get; set; }
    }
}