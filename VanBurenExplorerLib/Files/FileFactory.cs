namespace VanBurenExplorerLib.Files
{
    public static class FileFactory
    {
        public static VanBurenFile CreateUsing(FileProperties properties)
        {
            // TODO add specifications for different file types

            // fallback to a generic file
            return new GenericFile(properties.FullPath);
        }
    }
}