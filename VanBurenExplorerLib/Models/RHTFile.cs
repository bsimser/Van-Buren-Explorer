using System.Collections.Generic;

namespace VanBurenExplorerLib.Models
{
    public class RHTFile
    {
        public F3RHTHeader Header { get; set; }
        public IList<Entry> Entries { get; set; }
    }
}