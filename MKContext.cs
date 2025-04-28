using UnityEngine;

namespace Minikit.InputAndInterface
{
    public class MKContext
    {
        public MKTag tag { get; set; }
        public object data { get; set; }


        public MKContext(MKTag _tag, object _data)
        {
            tag = _tag;
            data = _data;
        }
    }
} // Minikit.InputAndInterface namespace
