using System.Collections.Generic;

namespace Minikit.InputAndInterface
{
    public interface IMKContextReceiver
    {
        public void ReceiveContext(List<MKContext> _contexts);
        public void ClearAllContexts();
        public void ClearContextByTag(MKTag _tag);
    }
} // Minikit.InputAndInterface namespace
