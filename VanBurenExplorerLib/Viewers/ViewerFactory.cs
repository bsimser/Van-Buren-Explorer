using VanBurenExplorerLib.Files;

namespace VanBurenExplorerLib.Viewers
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

            // fallback to a generic viewer
            return new GenericFileViewer(properties.File);
        }
    }
}