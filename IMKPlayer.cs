using UnityEngine;
using UnityEngine.EventSystems;

namespace Minikit.InputAndInterface
{
    public interface IMKPlayer
    {
        public MKSelectable selectedUI { get; set; }


        protected void OnSelectedUIChanged(MKSelectable _oldSelectable, MKSelectable _newSelectable);

        public MKInputDevice GetInputDeviceType();

        /// <summary> Have this function return the result of IMKPlayer.GetSelectedUI(this) </summary>
        public MKSelectable GetSelectedUI();

        public static MKSelectable GetSelectedUI(IMKPlayer _player)
        {
            return _player.selectedUI;
        }

        /// <summary> Have this function return the result of IMKPlayer.TrySelectUI(this, _selectable) </summary>
        public bool TrySelectUI(MKSelectable _selectable);

        public static bool TrySelectUI(IMKPlayer _player, MKSelectable _selectable)
        {
            if (!_selectable)
            {
                return false;
            }

            // We already have this selected
            if (_selectable.selectedByPlayers.Contains(_player))
            {
                return false;
            }

            if (!_selectable.CanPlayerSelect(_player))
            {
                return false;
            }

            if (_player.selectedUI
                && !_player.selectedUI.CanPlayerDeselect(_player))
            {
                return false;
            }

            // Deselect previous selection
            if (_player.selectedUI
                && _player.selectedUI.selectedByPlayers.Contains(_player))
            {
                _player.selectedUI.selectedByPlayers.RemoveAll(p => p == _player);
                MKEventSystem.instance.ExecuteUIEvent(_player, _player.selectedUI, ExecuteEvents.deselectHandler);
            }

            // Update the player's selected UI
            MKSelectable oldSelectable = _player.selectedUI;
            _player.selectedUI = _selectable;
            if (!_player.selectedUI.selectedByPlayers.Contains(_player))
            {
                _player.selectedUI.selectedByPlayers.Add(_player);
            }
            MKEventSystem.instance.ExecuteUIEvent(_player, _player.selectedUI, ExecuteEvents.selectHandler);
            _player.OnSelectedUIChanged(oldSelectable, _player.selectedUI);

            return true;
        }

        /// <summary> Have this function return the result of IMKPlayer.TryDeselectUI(this) </summary>
        public bool TryDeselectUI();

        public static bool TryDeselectUI(IMKPlayer _player)
        {
            if (!_player.selectedUI)
            {
                return true;
            }

            if (!_player.selectedUI.CanPlayerDeselect(_player))
            {
                return false;
            }

            // Deselect previous selection
            if (_player.selectedUI.selectedByPlayers.Contains(_player))
            {
                _player.selectedUI.selectedByPlayers.RemoveAll(p => p == _player);
                MKEventSystem.instance.ExecuteUIEvent(_player, _player.selectedUI, ExecuteEvents.deselectHandler);
            }

            // Update the player's selected UI
            MKSelectable oldSelectable = _player.selectedUI;
            _player.selectedUI = null;
            _player.OnSelectedUIChanged(oldSelectable, _player.selectedUI);

            return true;
        }

        /// <summary> Have this function return the result of IMKPlayer.TryNavigateUIByDirection(this, _direction) </summary>
        public bool TryNavigateUIByDirection(Vector2 _direction);

        public static bool TryNavigateUIByDirection(IMKPlayer _player, Vector2 _direction)
        {
            if (_player.selectedUI == null)
            {
                return _player.TrySelectUI(MKSelectable.GetPlayerFirstSelection(_player));
            }

            MKSelectable foundUI = _player.selectedUI.FindUI(_direction);
            if (foundUI != null)
            {
                return _player.TrySelectUI(foundUI);
            }

            return false;
        }

        /// <summary> Have this function return the result of IMKPlayer.TryNavigateUIByDirection(this, _direction) </summary>
        public bool TryNavigateUIByDirection(MKInputDirection _direction);

        public static bool TryNavigateUIByDirection(IMKPlayer _player, MKInputDirection _direction)
        {
            if (_player.selectedUI == null)
            {
                return _player.TrySelectUI(MKSelectable.GetPlayerFirstSelection(_player));
            }

            MKSelectable foundUI = _player.selectedUI.FindUI(_direction);
            if (foundUI != null)
            {
                return _player.TrySelectUI(foundUI);
            }

            return false;
        }

        /// <summary> Have this function return the result of IMKPlayer.TrySubmitUI(this, _selectable) </summary>
        public bool TrySubmitUI(MKSelectable _selectable);

        public static bool TrySubmitUI(IMKPlayer _player, MKSelectable _selectable)
        {
            if (!_player.selectedUI)
            {
                return false;
            }

            // If we're using a mouse, don't submit on anything unless we have it selected
            if (_player.GetInputDeviceType().UsesCursor()
                && !_player.selectedUI.selectedByPlayers.Contains(_player))
            {
                return false;
            }

            if (!_player.selectedUI.CanPlayerSubmit(_player))
            {
                return false;
            }

            MKEventSystem.instance.ExecuteUIEvent(_player, _player.selectedUI, ExecuteEvents.submitHandler);
            return true;
        }
    }
} // Minikit.InputAndInterface namespace
