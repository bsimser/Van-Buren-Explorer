using VanBurenExplorerLib.Specs;

namespace VanBurenExplorerLib.Files
{
    public static class FileFactory
    {
        public static VanBurenFile CreateUsing(FileProperties properties)
        {
            if(new CrtFileSpecification().IsSatisfiedBy(properties))
                return new CrtFile(properties.FullPath);

            if (new GrpFileSpecification().IsSatisfiedBy(properties))
                return new GrpFile(properties.FullPath);

            // fallback to a generic file
            return new GenericFile(properties.FullPath);
        }
    }
}