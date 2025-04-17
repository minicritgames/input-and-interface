using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Minikit.InputAndInterface
{
    public enum MKInputDirection
    {
        Up,
        Right,
        Down,
        Left
    }

    [RequireComponent(typeof(EventTrigger))]
    public abstract class MKSelectable : MonoBehaviour
    {
        [SerializeField] protected bool allowAsFirstSelection = false; // Not intended to be changed during runtime

        [HideInInspector] public List<IMKPlayer> selectedByPlayers = new();
        [HideInInspector] public MKRuleset<IMKPlayer> canInteractRuleset = new();
        public bool hovered { get; private set; } // Hovered is only used for the KeyboardAndMouse InputDeviceType
        public bool selected { get => selectedByPlayers.Count > 0; }

        protected EventTrigger eventTrigger { get; private set; }

        private static List<MKSelectable> firstSelections = new();


        private void Awake()
        {
            AwakeInternal();
        }

        protected virtual void AwakeInternal()
        {
            eventTrigger = GetComponent<EventTrigger>();

            // Hook up to the hover events (this works for 2D and 3D selectables, so long as the Camera used has a PhysicsRaycaster component)
            EventTrigger.Entry pointerEnterEventTrigger = eventTrigger.triggers.FirstOrDefault(e => e.eventID == EventTriggerType.PointerEnter);
            if (pointerEnterEventTrigger == null)
            {
                pointerEnterEventTrigger = new EventTrigger.Entry() { eventID = EventTriggerType.PointerEnter };
                eventTrigger.triggers.Add(pointerEnterEventTrigger);
            }
            pointerEnterEventTrigger.callback.AddListener(OnHovered);

            EventTrigger.Entry pointerExitEventTrigger = eventTrigger.triggers.FirstOrDefault(e => e.eventID == EventTriggerType.PointerExit);
            if (pointerExitEventTrigger == null)
            {
                pointerExitEventTrigger = new EventTrigger.Entry() { eventID = EventTriggerType.PointerExit };
                eventTrigger.triggers.Add(pointerExitEventTrigger);
            }
            pointerExitEventTrigger.callback.AddListener(OnUnhovered);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            OnValidateInternal();
        }

        protected virtual void OnValidateInternal()
        {
            // Clear all OnClick listeners, we do not want to use that event when we're using this UI system
            Button button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
            }

            // Add the EventTrigger if it doesn't exist yet
            eventTrigger = GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                eventTrigger = gameObject.AddComponent<EventTrigger>();
            }

            // Make an empty "Submit" trigger if there isn't one already
            if (eventTrigger.triggers.Count == 0
                || eventTrigger.triggers.FirstOrDefault(e => e.eventID == EventTriggerType.Submit) == null)
            {
                eventTrigger.triggers.Add(new EventTrigger.Entry() { eventID = EventTriggerType.Submit });
            }
        }
#endif // UNITY_EDITOR

        private void OnEnable()
        {
            OnEnableInternal();
        }

        protected virtual void OnEnableInternal()
        {
            if (allowAsFirstSelection)
            {
                firstSelections.Add(this);
            }
        }

        private void OnDisable()
        {
            OnDisableInternal();
        }

        protected virtual void OnDisableInternal()
        {
            hovered = false;

            if (allowAsFirstSelection
                && firstSelections.Contains(this))
            {
                firstSelections.Remove(this);
            }
        }

        private void OnDestroy()
        {
            OnDestroyInternal();
        }

        protected virtual void OnDestroyInternal()
        {

        }


        public abstract MKSelectable FindUI(Vector3 _direction);

        public abstract MKSelectable FindUI(MKInputDirection _direction);

        public virtual bool CanPlayerSelect(IMKPlayer _player)
        {
            return canInteractRuleset.Check(_player);
        }

        public virtual bool CanPlayerDeselect(IMKPlayer _player)
        {
            return true;
        }

        public virtual bool CanPlayerSubmit(IMKPlayer _player)
        {
            return canInteractRuleset.Check(_player);
        }

        private void OnHovered(BaseEventData _baseEventData)
        {
            hovered = true;

            // Since we can't tell which player sent the PointerEnter event, we have to just set all players selectUI to this as long as they have a mouse device
            if (IMKPlayerManager.instance != null)
            {
                foreach (IMKPlayer _player in IMKPlayerManager.instance.GetPlayers())
                {
                    if (_player.GetInputDeviceType().UsesCursor())
                    {
                        _player.TrySelectUI(this);
                    }
                }
            }
        }

        private void OnUnhovered(BaseEventData _baseEventData)
        {
            hovered = false;

            // Setting pressingPlayer.selectedUI to null is handled in the UIEventSystem instead
        }

        protected virtual void OnSelected(BaseEventData _baseEventData)
        {

        }

        protected virtual void OnDeselected(BaseEventData _baseEventData)
        {

        }

        public void AddSelectedListener(UnityAction<BaseEventData> _func)
        {
            // NOTE: If you get a null reference on eventTrigger here, it may be because you are calling this function inside of an Awake event
            // which could possibly be firing before this component's Awake event, which is what assigns eventTrigger

            EventTrigger.Entry selectedEventTrigger = eventTrigger.triggers.FirstOrDefault(e => e.eventID == EventTriggerType.Select);
            if (selectedEventTrigger == null)
            {
                selectedEventTrigger = new EventTrigger.Entry() { eventID = EventTriggerType.Select };
                eventTrigger.triggers.Add(selectedEventTrigger);
            }
            selectedEventTrigger.callback.AddListener(_func);
        }

        public void RemoveSelectedListener(UnityAction<BaseEventData> _func)
        {
            EventTrigger.Entry selectedEventTrigger = eventTrigger.triggers.FirstOrDefault(e => e.eventID == EventTriggerType.Select);
            if (selectedEventTrigger != null)
            {
                selectedEventTrigger.callback.RemoveListener(_func);
            }
        }

        public void RemoveAllSelectedListeners()
        {
            EventTrigger.Entry selectedEventTrigger = eventTrigger.triggers.FirstOrDefault(e => e.eventID == EventTriggerType.Select);
            if (selectedEventTrigger != null)
            {
                selectedEventTrigger.callback.RemoveAllListeners();
            }
        }

        public void AddDeselectedListener(UnityAction<BaseEventData> _func)
        {
            // NOTE: If you get a null reference on eventTrigger here, it may be because you are calling this function inside of an Awake event
            // which could possibly be firing before this component's Awake event, which is what assigns eventTrigger

            EventTrigger.Entry deselectedEventTrigger = eventTrigger.triggers.FirstOrDefault(e => e.eventID == EventTriggerType.Deselect);
            if (deselectedEventTrigger == null)
            {
                deselectedEventTrigger = new EventTrigger.Entry() { eventID = EventTriggerType.Deselect };
                eventTrigger.triggers.Add(deselectedEventTrigger);
            }
            deselectedEventTrigger.callback.AddListener(_func);
        }

        public void RemoveDeselectedListener(UnityAction<BaseEventData> _func)
        {
            EventTrigger.Entry deselectedEventTrigger = eventTrigger.triggers.FirstOrDefault(e => e.eventID == EventTriggerType.Deselect);
            if (deselectedEventTrigger != null)
            {
                deselectedEventTrigger.callback.RemoveListener(_func);
            }
        }

        public void RemoveAllDeselectedListeners()
        {
            EventTrigger.Entry deselectedEventTrigger = eventTrigger.triggers.FirstOrDefault(e => e.eventID == EventTriggerType.Deselect);
            if (deselectedEventTrigger != null)
            {
                deselectedEventTrigger.callback.RemoveAllListeners();
            }
        }

        public void RemovePlayerFromSelected(IMKPlayer _player)
        {
            if (selectedByPlayers.RemoveAll((p) => p == _player) > 0)
            { 
                MKEventSystem.instance.ExecuteUIEvent(_player, this, ExecuteEvents.deselectHandler);
            }
        }

        public void AddSubmitListener(UnityAction<BaseEventData> _func)
        {
            // NOTE: If you get a null reference on eventTrigger here, it may be because you are calling this function inside an Awake event
            // which could possibly be firing before this component's Awake event, which is what assigns eventTrigger

            EventTrigger.Entry submitEventTrigger = eventTrigger.triggers.FirstOrDefault(e => e.eventID == EventTriggerType.Submit);
            if (submitEventTrigger == null)
            {
                submitEventTrigger = new EventTrigger.Entry() { eventID = EventTriggerType.Submit };
                eventTrigger.triggers.Add(submitEventTrigger);
            }
            submitEventTrigger.callback.AddListener(_func);
        }

        public void RemoveSubmitListener(UnityAction<BaseEventData> _func)
        {
            EventTrigger.Entry submitEventTrigger = eventTrigger.triggers.FirstOrDefault(e => e.eventID == EventTriggerType.Submit);
            if (submitEventTrigger != null)
            {
                submitEventTrigger.callback.RemoveListener(_func);
            }
        }

        public void RemoveAllSubmitListeners()
        {
            EventTrigger.Entry submitEventTrigger = eventTrigger.triggers.FirstOrDefault(e => e.eventID == EventTriggerType.Submit);
            if (submitEventTrigger != null)
            {
                submitEventTrigger.callback.RemoveAllListeners();
            }
        }

        protected MKInputDirection VectorToInputDirection(Vector2 _direction)
        {
            // The native Selectable.FindSelectable(Vector3 dir) will ignore explicit navigation rules,
            // so we need to convert our input Vector2 direction into our enum so they don't navigate
            // to places they shouldn't

            float directionAngle = Vector2.SignedAngle(Vector2.right, _direction);

            MKInputDirection direction = MKInputDirection.Right;
            if (directionAngle < 45.0f && directionAngle >= -45.0f)
            {
                direction = MKInputDirection.Right;
            }
            else if (directionAngle < 135.0f && directionAngle >= 45.0f)
            {
                direction = MKInputDirection.Up;
            }
            else if (directionAngle < -135.0f || directionAngle >= 135.0f)
            {
                direction = MKInputDirection.Left;
            }
            else if (directionAngle < -45.0f && directionAngle >= -135.0f)
            {
                direction = MKInputDirection.Down;
            }

            return direction;
        }

        public static MKSelectable GetPlayerFirstSelection(IMKPlayer _player)
        {
            foreach (MKSelectable firstUISelection in firstSelections)
            {
                if (firstUISelection.CanPlayerSelect(_player))
                {
                    return firstUISelection;
                }
            }

            return null;
        }
    }
} // Minikit.InputAndInterface namespace
