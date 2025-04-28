using System;
using System.Collections.Generic;

namespace Minikit.InputAndInterface
{
    public static class MKContextExtensions
    {
        public static bool GetFirst(this List<MKContext> _contexts, MKTag _tag, out MKContext _outContext)
        {
            return FindFirst(_contexts, c => c.tag == _tag, out _outContext);
        }
        
        public static bool GetFirstValid(this List<MKContext> _contexts, MKTag _tag, out MKContext _outContext)
        {
            return FindFirst(_contexts, c => c.tag == _tag && c.data != null, out _outContext);
        }

        public static bool FindFirst(this List<MKContext> _contexts, Func<MKContext, bool> _func, out MKContext _outContext)
        {
            if (_func != null)
            {
                foreach (MKContext context in _contexts)
                {
                    if (_func(context))
                    {
                        _outContext = context;
                        return true;
                    }
                }
            }

            _outContext = null;
            return false;
        }
    }
} // Minikit.InputAndInterface namespace
