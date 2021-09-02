using System;
using System.Windows.Forms;
using NAudio.Wave;

namespace VanBurenExplorerLib.Views
{
    public partial class UserControl1 : UserControl
    {
        private RawSourceWaveStream _stream;
        private WaveOut _player;

        public UserControl1()
        {
            InitializeComponent();
            _player = new WaveOut();
        }

        public void LoadAudio(byte[] bytes)
        {
            if (_player != null)
            {
                _player.Stop();
            }
            _stream = new RawSourceWaveStream(bytes, 0, bytes.Length, new WaveFormat());
            waveViewer1.WaveStream = _stream;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(_player != null)
            {
                switch (_player.PlaybackState)
                {
                    case PlaybackState.Playing:
                        return;
                    case PlaybackState.Paused:
                        _player.Play();
                        return;
                }
            }

            _player.Init(_stream);
            _player.Play();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_player != null)
            {
                if (_player.PlaybackState == PlaybackState.Playing)
                {
                    _player.Pause();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _player?.Stop();
            _stream.Position = 0;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (disposing && _player != null)
            {
                _player.Stop();
                _player.Dispose();
            }
            if (disposing && _stream != null)
            {
                _stream.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
