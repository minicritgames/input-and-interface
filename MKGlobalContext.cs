using System.Collections.Generic;
using System.Linq;

namespace Minikit.InputAndInterface
{
    public static class MKGlobalContext
    {
        private static Dictionary<MKTag, List<MKContext>> globalContexts = new();


        public static void SetContext(MKTag _tag, List<MKContext> _context)
        {
            globalContexts[_tag] = _context;
        }

        public static List<MKContext> GetContext(MKTag _tag)
        {
            return globalContexts.FirstOrDefault(kvp => kvp.Key == _tag).Value;
        }
    }
} // Minikit.InputAndInterface namespace
