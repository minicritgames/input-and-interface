using UnityEngine;
using UnityEngine.UI;

namespace Minikit.InputAndInterface
{
    [RequireComponent(typeof(Selectable))]
    public class MK2DSelectable : MKSelectable
    {
        public Selectable selectable { get; private set; }


        protected override void AwakeInternal()
        {
            selectable = GetComponent<Selectable>();

            // Set the navigation mode to always be explicit
            Navigation navigation = selectable.navigation;
            navigation.mode = Navigation.Mode.Explicit;
            selectable.navigation = navigation;

            base.AwakeInternal();
        }


        public override MKSelectable FindUI(Vector3 _direction)
        {
            if (selectable == null)
            {
                return null;
            }

            return FindUI(VectorToInputDirection(_direction));
        }

        public override MKSelectable FindUI(MKInputDirection _direction)
        {
            if (selectable == null)
            {
                return null;
            }

            Selectable foundSelectable = null;
            switch (_direction)
            {
                case MKInputDirection.Left:
                    foundSelectable = selectable.FindSelectableOnLeft();
                    break;
                case MKInputDirection.Right:
                    foundSelectable = selectable.FindSelectableOnRight();
                    break;
                case MKInputDirection.Up:
                    foundSelectable = selectable.FindSelectableOnUp();
                    break;
                case MKInputDirection.Down:
                    foundSelectable = selectable.FindSelectableOnDown();
                    break;
            }

            if (foundSelectable != null)
            {
                MKSelectable ui = foundSelectable.GetComponent<MKSelectable>();
                if (ui != null)
                {
                    return ui;
                }
            }

            return null;
        }

        public void LinkNavigationTo(MKInputDirection _direction, MK2DSelectable _selectable)
        {
            Navigation navigation = selectable.navigation;
            switch (_direction)
            {
                case MKInputDirection.Up:
                    navigation.selectOnUp = _selectable.selectable;
                    break;
                case MKInputDirection.Right:
                    navigation.selectOnRight = _selectable.selectable;
                    break;
                case MKInputDirection.Down:
                    navigation.selectOnDown = _selectable.selectable;
                    break;
                case MKInputDirection.Left:
                    navigation.selectOnLeft = _selectable.selectable;
                    break;
            }
            selectable.navigation = navigation;
        }
    }
} // Minikit.InputAndInterface namespace
