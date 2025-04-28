using System.Collections.Generic;
using UnityEngine;

namespace Minikit.InputAndInterface
{
    public class MK3DSelectable : MKSelectable
    {
        public Dictionary<MKInputDirection, MKSelectable> explicitNavigateSelectables = new(); // Eventually we should find a better solution for 3D selectables
        
        
        public override MKSelectable FindUI(Vector3 _direction)
        {
            return FindUI(VectorToInputDirection(_direction));
        }

        public override MKSelectable FindUI(MKInputDirection _direction)
        {
            if (explicitNavigateSelectables.ContainsKey(_direction)
                && explicitNavigateSelectables[_direction] != null)
            {
                return explicitNavigateSelectables[_direction];
            }

            return null;
        }
    }
} // Minikit.InputAndInterface namespace
