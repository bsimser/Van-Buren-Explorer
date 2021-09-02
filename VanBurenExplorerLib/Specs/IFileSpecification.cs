using VanBurenExplorerLib.Models;

namespace VanBurenExplorerLib.Specs
{
    public interface IFileSpecification
    {
        bool IsSatisfiedBy(FileProperties properties);
    }
}