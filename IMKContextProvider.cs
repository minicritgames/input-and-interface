using System.Collections.Generic;

namespace Minikit.InputAndInterface
{
    public interface IMKContextProvider
    {
        public List<MKContext> GetAllContexts();
        public MKContext GetContextByTag(MKTag _tag);
    }
} // Minikit.InputAndInterface namespace
