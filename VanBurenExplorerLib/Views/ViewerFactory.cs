using VanBurenExplorerLib.Models;

namespace VanBurenExplorerLib.Views
{
    public static class ViewerFactory
    {
        public static IFileViewer CreateUsing(ViewerProperties properties)
        {
            if(properties.File is CrtFile)
                return new GenericFileViewer(properties.File);

            if(properties.File is GrpFile) 
                return new GrpFileViewer(properties.File, properties.Catalog);

            if(properties.File is TextFile)
                return new TextFileViewer(properties.File);

            if (properties.File is TgaFile)
                return new TgaFileViewer(properties.File);
            
            // fallback to a generic viewer
            return new GenericFileViewer(properties.File);
        }
    }
}