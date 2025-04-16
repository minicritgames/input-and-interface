using UnityEngine.EventSystems;

namespace Minikit.InputAndInterface
{
    public class MKEventData : BaseEventData
    {
        public IMKPlayer pressingPlayer;
        public MKSelectable pressedUISelectable;
        public string specialInputKey;


        public MKEventData(IMKPlayer _pressingPlayer, MKSelectable _pressedUISelectable, EventSystem _eventSystem, string _specialInputKey = null) : base(_eventSystem)
        {
            pressingPlayer = _pressingPlayer;
            pressedUISelectable = _pressedUISelectable;
            specialInputKey = _specialInputKey;
        }
    }
} // Minikit.InputAndInterface namespace
