using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;

namespace Minikit.InputAndInterface
{
    [System.Flags]
    [System.Serializable]
    public enum MKInputDevice
    {
        NONE = 0,
        Gamepad = 1,
        Touch = 2,
        Mouse = 4,
        Keyboard = 8,
        MouseAndKeyboard = Mouse | Keyboard
    }

    /// <summary> This is a singleton that lives on the EventSystem in your scene </summary>
    public class MKEventSystem : MonoBehaviour
    {
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private InputSystemUIInputModule inputModule;


        public static MKEventSystem instance { get; private set; }

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (eventSystem.currentSelectedGameObject != null)
            {
                eventSystem.SetSelectedGameObject(null);
            }

            // If any of our Mouse players are no longer hovering their currently selected UI, we unselect it
            if (IMKPlayerManager.instance != null)
            {
                foreach (IMKPlayer player in IMKPlayerManager.instance.GetPlayers())
                {
                    if (player.GetInputDeviceType().UsesCursor()
                        && player.GetSelectedUI() != null
                        && !player.GetSelectedUI().hovered)
                    {
                        player.TryDeselectUI();
                    }
                }
            }
        }


        public EventSystem GetEventSystem()
        {
            return eventSystem;
        }

        public InputSystemUIInputModule GetUIInputModule()
        {
            return inputModule;
        }

        public void ExecuteUIEvent<T>(IMKPlayer _player, MKSelectable _selectable, ExecuteEvents.EventFunction<T> _eventFunction, string _specialInputKey = null) where T : IEventSystemHandler
        {
            if (_player == null
                || _selectable == null)
            {
                Debug.LogError("Failed to execute event because some of the supplied data was invalid");
                return;
            }

            MKEventData uiEventData = new MKEventData(_player, _selectable, GetEventSystem(), _specialInputKey);
            ExecuteEvents.Execute(_selectable.gameObject, uiEventData, _eventFunction);
        }
    }
} // Minikit.InputAndInterface namespace
