using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Minikit.InputAndInterface
{
    public class MK2DButton : MK2DSelectable, ISubmitHandler
    {
        [SerializeField] public UnityEvent<MKEventData> OnPressed = new();
        

        public void OnSubmit(BaseEventData _eventData)
        {
            if (_eventData is MKEventData eventData)
            {
                OnPressed?.Invoke(eventData);
            }
        }
    }
} // Minikit.InputAndInterface namespace
