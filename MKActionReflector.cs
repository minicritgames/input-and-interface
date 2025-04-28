using System;
using System.Collections.Generic;
using System.Reflection;

namespace Minikit.InputAndInterface
{
    public static class MKActionReflector
    {
        private static List<MKActionObject> actionObjects = new();
        private static List<MKActionRule> actionRules = new();
        
        
        static MKActionReflector()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(MKActionObject))
                        && !type.IsAbstract) // Ignore abstract classes since we don't want to register them
                    {
                        MKActionObject actionObject = (MKActionObject)Activator.CreateInstance(type);
                        if (actionObject != null)
                        {
                            actionObjects.Add(actionObject);
                        }

                        continue;
                    }

                    if (type.IsSubclassOf(typeof(MKActionRule))
                        && !type.IsAbstract)
                    {
                        MKActionRule actionRule = (MKActionRule)Activator.CreateInstance(type);
                        if (actionRule != null)
                        {
                            actionRules.Add(actionRule);
                        }

                        continue;
                    }
                }
            }
        }
    }
} // Minikit.InputAndInterface namespace
