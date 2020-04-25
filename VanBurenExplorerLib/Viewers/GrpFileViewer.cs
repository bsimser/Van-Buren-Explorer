using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Be.Windows.Forms;
using NAudio.Wave;
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

            // create a splitter for our list of entries and a preview control
            var splitter = new SplitContainer
            {
                Orientation = Orientation.Vertical, 
                Dock = DockStyle.Fill
            };
            // list to hold entry names
            var listBox = new ListBox
            {
                Dock = DockStyle.Fill
            };
            listBox.SelectedIndexChanged += delegate(object sender, EventArgs args)
            {
                var lb = sender as ListBox;
                var entry = lb.Items[lb.SelectedIndex] as GrpEntry;

                // clear any previous control in our preview panel
                splitter.Panel2.Controls.Clear();

                switch (entry.Type)
                {
                    case GrpEntry.GrpType.Unknown:
                        splitter.Panel2.Controls.Add(LoadHexControl(entry));
                        break;
                    case GrpEntry.GrpType.BMP:
                        splitter.Panel2.Controls.Add(LoadBitmap(entry));
                        break;
                    case GrpEntry.GrpType.TGA:
                        splitter.Panel2.Controls.Add(LoadHexControl(entry));
                        break;
                    case GrpEntry.GrpType.TGA2:
                        splitter.Panel2.Controls.Add(LoadHexControl(entry));
                        break;
                    case GrpEntry.GrpType.WAV:
                        splitter.Panel2.Controls.Add(LoadAudioControl(entry));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };
            foreach (var entry in _entries)
            {
                listBox.Items.Add(entry);
            }
            splitter.Panel1.Controls.Add(listBox);
            
            return splitter;
        }

        private Control LoadAudioControl(GrpEntry entry)
        {
            var control = new UserControl1();
            using (var reader = new BinaryReader(File.OpenRead(entry.FileName)))
            {
                reader.BaseStream.Position = entry.Position;
                control.LoadAudio(reader.ReadBytes(entry.Length));
            }
            return control;
        }

        private Control LoadHexControl(GrpEntry entry)
        {
            using (var reader = new BinaryReader(File.OpenRead(entry.FileName)))
            {
                reader.BaseStream.Position = entry.Position;
                var bytes = reader.ReadBytes(entry.Length);
                var provider = new DynamicByteProvider(bytes);
                return new HexBox
                {
                    ColumnInfoVisible = true,
                    LineInfoVisible = true,
                    StringViewVisible = true,
                    UseFixedBytesPerLine = true,
                    VScrollBarVisible = true,
                    ByteProvider = provider,
                    Dock = DockStyle.Fill
                };
            }
        }

        private Control LoadBitmap(GrpEntry entry)
        {
            var pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.CenterImage
            };
            using (var reader = new BinaryReader(File.OpenRead(entry.FileName)))
            {
                reader.BaseStream.Position = entry.Position;
                var bytes = reader.ReadBytes(entry.Length);
                using (var stream = new MemoryStream(bytes))
                {
                    pictureBox.Image = Image.FromStream(stream);
                }
            }
            return pictureBox;
        }

        private void LoadFile()
        {
            _entries = new List<GrpEntry>();

            using (var reader = new BinaryReader(File.OpenRead(_file.FullPath)))
            {
                var header = reader.ReadBytes(8);
                var numEntries = reader.ReadInt32();
                for (var i = 0; i < numEntries; i++)
                {
                    var position = reader.ReadInt32();
                    var length = reader.ReadInt32();
                    _entries.Add(new GrpEntry
                    {
                        FileName = _file.FullPath,
                        Position = position, 
                        Length = length, 
                        Type = GrpEntry.GrpType.Unknown
                    });
                }
                foreach (var entry in _entries)
                {
                    reader.BaseStream.Position = entry.Position;
                    entry.Header = reader.ReadBytes(18);
                }
            }
            // process the entries to identify them
            foreach (var entry in _entries)
            {
                if (entry.Header[0] == 66 && entry.Header[1] == 77)
                {
                    entry.Type = GrpEntry.GrpType.BMP;
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
                    entry.Type = entry.Header[16] == 24 ? GrpEntry.GrpType.TGA : GrpEntry.GrpType.TGA2;
                }

                if (entry.Header[0] == 82 &&
                    entry.Header[1] == 73 &&
                    entry.Header[2] == 70 &&
                    entry.Header[3] == 70)
                {
                    entry.Type = GrpEntry.GrpType.WAV;
                }
            }
        }

        class GrpEntry
        {
            public string FileName { get; set; }

            public int Position { get; set; }
            
            public int Length { get; set; }
            
            public byte[] Header { get; set; }
            
            public GrpType Type { get; set; }

            public enum GrpType
            {
                Unknown = 0,
                BMP,
                TGA,
                TGA2,
                WAV
            }

            /// <summary>
            /// Provides description for ListBox control
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return $"type={Type} position={Position} size={Length}";
            }
        }
    }
}