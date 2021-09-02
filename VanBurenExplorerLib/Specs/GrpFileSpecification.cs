using VanBurenExplorerLib.Models;

namespace VanBurenExplorerLib.Specs
{
    public class GrpFileSpecification : IFileSpecification
    {
        public bool IsSatisfiedBy(FileProperties properties)
        {
            if (properties.FullPath.EndsWith(".grp"))
                return true;
            return false;
        }
    }
}