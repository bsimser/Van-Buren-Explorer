namespace VanBurenExplorerLib.Viewers
{
    public static class ViewerFactory
    {
        public static IFileViewer CreateUsing(ViewerProperties properties)
        {
            // TODO check different file types in properties and return the custom viewers

            // fallback to a generic viewer
            return new GenericFileViewer(properties.File);
        }
    }
}