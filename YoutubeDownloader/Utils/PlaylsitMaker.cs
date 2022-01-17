using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using YoutubeExplode.Videos;

namespace YoutubeDownloader.Utils
{
    internal class PlaylsitMaker
    {
        private string _dirPath;
        private string _title;
        private List<IVideo>? _videos;

        public PlaylsitMaker ( string title , string dirPath )
        {
            if ( String.IsNullOrWhiteSpace ( dirPath ) )
                throw new ArgumentNullException ( nameof ( dirPath ) );

            if ( String.IsNullOrWhiteSpace ( title ) )
                throw new ArgumentNullException ( nameof ( title ) );

            if ( !Directory.Exists ( dirPath ) )
                throw new DirectoryNotFoundException ( nameof ( dirPath ) );

            this._dirPath = dirPath;
            this._title = title;
        }

        internal void Add ( IVideo video )
        {
            var videos = this.Vdieos;
            Debug.Assert ( null != video );
            videos.Add ( video );
        }

        private List<IVideo> Vdieos
        {
            get
            {
                if ( null == _videos )
                    _videos = new List<IVideo> ();
                return _videos;
            }
        }

        public void Save ()
        {
            string fileFullName = String.Empty;
            int count = 0;
            do
            {
                string sufix = count > 0 ? $"_{count}" : string.Empty;
                fileFullName = $"{_dirPath}\\{_title}{sufix}.csv";
                count++;
            }
            while ( File.Exists ( fileFullName ) );

            using ( FileStream fileStream = new FileStream ( fileFullName , FileMode.OpenOrCreate , FileAccess.Write ) )
            {
                Debug.Assert ( null != _videos );
                using ( StreamWriter streamWriter = new StreamWriter ( fileStream ) )
                {
                    const char Delimiter = '|';

                    foreach ( var v in _videos )
                    {
                        string line = $"{v.Title} {Delimiter} {v.Author} {Delimiter} {v.Duration}";
                        streamWriter.WriteLine ( line );
                    }
                    streamWriter.Close ();
                }
            }
        }
    }
}
