using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using VanBurenExplorerLib.Files;

namespace VanBurenExplorerLib.Viewers
{
    public class GrpFileViewer : IFileViewer
    {
        private readonly VanBurenFile _file;
        private IList<GrpEntry> _entries;

        public GrpFileViewer(VanBurenFile file)
        {
            _file = file;
        }

        public Control GetControl()
        {
            LoadFile();
            var control = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both
            };
            var sb = new StringBuilder();
            foreach (var entry in _entries)
            {
                sb.AppendLine($"{entry.Name} (size={entry.Length})");
            }
            control.Text = sb.ToString();
            return control;
        }

        private void LoadFile()
        {
            _entries = new List<GrpEntry>();

            using (var reader = new BinaryReader(File.OpenRead(_file.FullPath)))
            {
                var header = reader.ReadBytes(8);
                var numfiles = reader.ReadInt32();
                var unknown = reader.ReadBytes(8);
                // read entry headers (last entry is the end of the file)
                for (var i = 0; i < numfiles - 1; i++)
                {
                    var position = reader.ReadInt32();
                    var length = reader.ReadInt32();
                    _entries.Add(new GrpEntry {Position = position, Length = length, Name = "Unknown"});
                }
                foreach (var entry in _entries)
                {
                    reader.BaseStream.Position = entry.Position;
                    entry.Header = reader.ReadBytes(18);
                    // TODO ultimately we'll leave this and load the data when you select it in the UI
                    entry.Data = reader.ReadBytes(entry.Length);
                }
            }
            // process the entries to identify them
            foreach (var entry in _entries)
            {
                if (entry.Header[0] == 66 && entry.Header[1] == 77)
                {
                    entry.Name = "BMP file";
                }

                if (entry.Header[0] == 0 &&
                    entry.Header[1] == 0 &&
                    entry.Header[2] == 2 &&
                    entry.Header[3] == 0 &&
                    entry.Header[4] == 0 &&
                    entry.Header[5] == 0 &&
                    entry.Header[6] == 0 &&
                    entry.Header[7] == 0)
                {
                    entry.Name = entry.Header[16] == 24 ? "TGA file" : "TGA file 2";
                }
            }
        }

        class GrpEntry
        {
            public string Name { get; set; }
            public int Position { get; set; }
            public int Length { get; set; }
            public byte[] Header { get; set; }
            public byte[] Data { get; set; }
        }
    }
}