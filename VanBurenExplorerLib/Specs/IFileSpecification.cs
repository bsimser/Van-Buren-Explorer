using VanBurenExplorerLib.Files;

namespace VanBurenExplorerLib.Specs
{
    public interface IFileSpecification
    {
        bool IsSatisfiedBy(FileProperties properties);
    }
}