﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Be.Windows.Forms;
using VanBurenExplorerLib.Helpers;
using VanBurenExplorerLib.Models;
using ImageFormat = Pfim.ImageFormat;

namespace VanBurenExplorerLib.Views
{
    public class GrpFileViewer : IFileViewer
    {
        private readonly VanBurenFile _file;
        private readonly RHTFile _resourceFile;
        private IList<Lump> _lumps;
        private SplitContainer _splitter;

        public GrpFileViewer(VanBurenFile file, RHTFile resourceFile)
        {
            _file = file;
            _resourceFile = resourceFile;
        }

        public Control GetControl()
        {
            // load the lumps so we know where to look in the file for the data
            LoadLumps();

            // create a splitter for our list of lumps and a preview control for each lump
            _splitter = new SplitContainer
            {
                Orientation = Orientation.Vertical, 
                Dock = DockStyle.Fill
            };
            // listview to hold lump information
            var listview = new ListView
            {
                Dock = DockStyle.Fill,
                FullRowSelect = true,
                View = View.Details,
            };
            listview.Columns.AddRange(new[]
            {
                new ColumnHeader
                {
                    Text = "Name"
                }, 
                new ColumnHeader
                {
                    Text = "Type"
                }, 
                new ColumnHeader
                {
                    Text = "Size",
                    TextAlign = HorizontalAlignment.Right
                }
            });

            // hook up our event when someone selects an entry from the listview
            listview.SelectedIndexChanged += ListviewOnSelectedIndexChanged;

            // get the .grp filename we're looking at
            var grpFileName = Path.GetFileNameWithoutExtension(_file.FullPath);

            // find the set of resources from the catalog that match this .grp name
            var resources = _resourceFile.Entries.Where(x => x.GroupName == grpFileName);

            // load the resources into the listview
            foreach (var resource in resources)
            {
                var item = new ListViewItem(resource.Name, 0);
                var subItems = new[] {
                    new ListViewItem.ListViewSubItem(item, resource.type.ToString()),
                    new ListViewItem.ListViewSubItem(item, GetLumpSize(resource.Index))
                };
                item.SubItems.AddRange(subItems);
                item.Tag = resource;
                listview.Items.Add(item);
            }
            listview.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            // add the listview to the main panel
            _splitter.Panel1.Controls.Add(listview);

            // and return the control
            return _splitter;
        }

        private void ListviewOnSelectedIndexChanged(object sender, EventArgs e)
        {
            var view = sender as ListView;
            if (view.SelectedItems.Count != 1) return;
            var item = view.SelectedItems[0];
            if (item == null) return;
            var entry = item.Tag as Entry;
            if (entry == null) return;
            _splitter.Panel2.Controls.Clear();
            switch (entry.type)
            {
                // TODO add specific viewers for each known type
                case LumpType.SMA:
                case LumpType.G3D:
                case LumpType.TRE:
                case LumpType.SKEL:
                case LumpType.ANIM:
                case LumpType.SCR:
                case LumpType.MAP:
                case LumpType.CRITTER:
                case LumpType.VFX:
                case LumpType.GUI:
                case LumpType.COL:
                case LumpType.ITEM:
                case LumpType.WEAPON:
                case LumpType.ARMOR:
                case LumpType.DOOR:
                case LumpType.USE:
                case LumpType.AMMO:
                case LumpType.CON:
                    _splitter.Panel2.Controls.Add(LoadHexControl(entry));
                    break;
                case LumpType.TGA:
                    _splitter.Panel2.Controls.Add(LoadImageControl(entry));
                    break;
                case LumpType.WAV:
                    _splitter.Panel2.Controls.Add(LoadAudioControl(entry));
                    break;
                case LumpType.INI:
                case LumpType.SND_CONF:
                case LumpType.SND_LST:
                case LumpType.SND_LST_2:
                case LumpType.SND_TXT:
                case LumpType.SND_TXT_2:
                case LumpType.TXT:
                    _splitter.Panel2.Controls.Add(LoadTextControl(entry));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _splitter.Panel2.Controls.Add(new Label {Text = entry.Name});
        }

        private string GetLumpSize(int index)
        {
            var lump = _lumps[index];
            return FileHelper.GetFormattedSize(lump.length);
        }

        private Control LoadAudioControl(Entry entry)
        {
            var control = new UserControl1();
            using (var reader = new BinaryReader(File.OpenRead(_file.FullPath)))
            {
                var lump = _lumps[entry.Index];
                reader.BaseStream.Position = lump.offset;
                control.LoadAudio(reader.ReadBytes(lump.length));
            }
            return control;
        }

        private Control LoadImageControl(Entry entry)
        {
            var control = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.CenterImage
            };
            using (var reader = new BinaryReader(File.OpenRead(_file.FullPath)))
            {
                var lump = _lumps[entry.Index];
                reader.BaseStream.Position = lump.offset;
                var data = reader.ReadBytes(lump.length);

                if (data[0] == 'B' && data[1] == 'M')
                {
                    // BMP images can just be read directly
                    using (var stream = new MemoryStream(data))
                    {
                        control.Image = Image.FromStream(stream);
                    }
                }
                else
                {
                    // TGA files need to go through Pfim
                    using (var stream = new MemoryStream(data))
                    {
                        var image = Pfim.Pfim.FromStream(stream);
                        PixelFormat format;
                        switch (image.Format)
                        {
                            case ImageFormat.Rgb8:
                                format = PixelFormat.Format8bppIndexed;
                                break;
                            case ImageFormat.R5g5b5:
                                format = PixelFormat.Format16bppRgb555;
                                break;
                            case ImageFormat.R5g6b5:
                                format = PixelFormat.Format16bppRgb565;
                                break;
                            case ImageFormat.R5g5b5a1:
                                format = PixelFormat.Format16bppArgb1555;
                                break;
                            case ImageFormat.Rgba16:
                                // TODO this is probably wrong
                                format = PixelFormat.Format16bppGrayScale;
                                break;
                            case ImageFormat.Rgb24:
                                format = PixelFormat.Format24bppRgb;
                                break;
                            case ImageFormat.Rgba32:
                                format = PixelFormat.Format32bppArgb;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        // pin image data as the picture box can outlive the pfim image object
                        // which, unless pinned, will garbage collect the data array causing image corruption
                        var handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
                        var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(image.Data, 0);
                        var bitmap = new Bitmap(image.Width, image.Height, image.Stride, format, ptr);
                        // TODO might need to do something with the bitmap palette here
                        control.Image = bitmap;
                    }
                }
            }

            return control;
        }

        private Control LoadTextControl(Entry entry)
        {
            using (var reader = new BinaryReader(File.OpenRead(_file.FullPath)))
            {
                var lump = _lumps[entry.Index];
                reader.BaseStream.Position = lump.offset;
                return new TextBox
                {
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    ReadOnly = true,
                    ScrollBars = ScrollBars.Both,
                    Text = Encoding.UTF8.GetString(reader.ReadBytes(lump.length), 0, lump.length)
                };
            }
        }

        private Control LoadHexControl(Entry entry)
        {
            var control = new HexBox();
            using (var reader = new BinaryReader(File.OpenRead(_file.FullPath)))
            {
                var lump = _lumps[entry.Index];
                reader.BaseStream.Position = lump.offset;
                var bytes = reader.ReadBytes(lump.length);
                var provider = new DynamicByteProvider(bytes);
                control = new HexBox
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
            return control;
        }

        /// <summary>
        /// Given a .grp file, load the lumps from the header which will give us the offsets to the data
        /// </summary>
        private void LoadLumps()
        {
            _lumps = new List<Lump>();

            using (var reader = new BinaryReader(File.OpenRead(_file.FullPath)))
            {
                var header = new F3GRPHeader
                {
                    vMajor = reader.ReadInt32(),
                    vMinor = reader.ReadInt32(),
                    nFiles = reader.ReadInt32()
                };

                for (var i = 0; i < header.nFiles; i++)
                {
                    _lumps.Add(new Lump
                    {
                        offset = reader.ReadInt32(), 
                        length = reader.ReadInt32()
                    });
                }
            }
        }
    }
}