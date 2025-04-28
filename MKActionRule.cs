using UnityEngine;

namespace Minikit.InputAndInterface
{
    public abstract class MKActionRule
    {
        public MKActionRule instance { get; private set; }


        protected MKActionRule()
        {
            instance = this;
        }
        
        public abstract bool Check();
    }
} // Minikit.InputAndInterface namespace
