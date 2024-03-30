using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Minikit.InputAndInterface
{
    public static class MKInputExtensions
    {


        public static MKInputDevice GetInputDeviceType(this InputAction.CallbackContext _context)
        {
            return _context.control.device.GetInputDeviceType();
        }

        public static MKInputDevice GetInputDeviceType(this InputDevice _inputDevice)
        {
            switch (_inputDevice.name)
            {
                case "Mouse":
                    return MKInputDevice.Mouse;
                case "Keyboard":
                    return MKInputDevice.Keyboard;
                case "Touchscreen":
                    return MKInputDevice.Touch;
                case "Pen":
                    goto case null;
                case "Gamepad":
                    goto case default;
                case null:
                    Debug.LogWarning($"Unknown input device displayName: {_inputDevice.name}");
                    return MKInputDevice.NONE;
                // We can't possibly know all of the names of the various gamepads, so we will treat the default non-null device as a Gamepad to be safe
                default:
                    return MKInputDevice.Gamepad;
            }
        }

        public static bool UsesCursor(this MKInputDevice _inputDeviceType)
        {
            return _inputDeviceType.HasFlag(MKInputDevice.Mouse) // This check is usually enough, but the other two are for safety
                || _inputDeviceType == MKInputDevice.Mouse
                || _inputDeviceType == MKInputDevice.MouseAndKeyboard;
        }
    }
} // Minicrit.MUI namespace
