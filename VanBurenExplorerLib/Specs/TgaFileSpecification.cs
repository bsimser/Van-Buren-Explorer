using VanBurenExplorerLib.Models;

namespace VanBurenExplorerLib.Specs
{
    public class TgaFileSpecification : IFileSpecification
    {
        public bool IsSatisfiedBy(FileProperties properties)
        {
            return properties.FullPath.EndsWith(".tga");
        }
    }
}