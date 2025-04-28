using System.Collections.Generic;

namespace Minikit.InputAndInterface
{
    public abstract class MKActionObject
    {
        protected List<MKActionRule> actionRules { get; set; } = new(); 
        public MKActionObject instance { get; private set; }


        protected MKActionObject()
        {
            instance = this;
        }

        public virtual bool CanExecute()
        {
            // If any of our action rules fail, fail the execution check
            foreach (MKActionRule actionRule in actionRules)
            {
                if (actionRule != null)
                {
                    if (!actionRule.Check())
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }
        
        public abstract void Execute();
    }
} // Minikit.InputAndInterface namespace
