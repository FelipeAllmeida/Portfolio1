using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vox
{
    public enum MouseKeyCode
    {
        Mouse0 = 323,
        Mouse1 = 324,
        Mouse2 = 325,
        Mouse3 = 326,
        Mouse4 = 327,
        Mouse5 = 328,
        Mouse6 = 329,
    }

    public static class InputDefaults
    {
        public readonly static List<MouseKeyCode> ListDefaultMouseKeyCodes = new List<MouseKeyCode> { { MouseKeyCode.Mouse0 }, { MouseKeyCode.Mouse1 } };
    }
    public class InputManager
    {
        public InputManager()
        {
            ListListenedMouseKeyCodes = InputDefaults.ListDefaultMouseKeyCodes;
        }

        public InputManager(List<MouseKeyCode> p_listenedMouseKeyCodes)
        {
            ListListenedMouseKeyCodes = p_listenedMouseKeyCodes;
        }

        public List<MouseKeyCode> ListListenedMouseKeyCodes;

        public bool interactable = true;
        public bool raycastMouseEnabled = true;

        /// <summary>
        /// The input default camera. If null, Camera.main will be used instead.
        /// </summary>
        private Camera _viewCamera;        
        public Camera viewCamera
        {
            get
            {
                if (_viewCamera == null) return Camera.main;
                else return _viewCamera;
            }
            set
            {
                _viewCamera = value;
            }
        }

        /// <summary>
        /// It handles the creation of a RayInfo for the input.
        /// </summary>
        public InputRayManager InputRayManager;

        /// <summary>
        /// Collection containing all current inputs.
        /// </summary>
        private Dictionary<int, InputInfo> _dictInputs = new Dictionary<int, InputInfo>();

        /// <summary>
        /// Callback called after a input. It retuns a InputInfo.
        /// </summary>
        public event Action<InputInfo> onInput;

        public void CheckInputs()
        {
            if (interactable == false) return;

            CheckMouseInputs();
        }

        #region Internal
        private void CheckMouseInputs()
        {            
            if (ListListenedMouseKeyCodes != null)
            {
                for (int i = 0; i < ListListenedMouseKeyCodes.Count; i ++)
                {
                    if (Input.GetKeyDown((KeyCode)ListListenedMouseKeyCodes[i]))
                    {
                        HandleKey((int)ListListenedMouseKeyCodes[i], InputPhases.START, raycastMouseEnabled);
                    }
                    else if (Input.GetKey((KeyCode)ListListenedMouseKeyCodes[i]))
                    {
                        HandleKey((int)ListListenedMouseKeyCodes[i], InputPhases.UPDATE, raycastMouseEnabled);

                    }
                    else if (Input.GetKeyUp((KeyCode)ListListenedMouseKeyCodes[i]))
                    {
                        HandleKey((int)ListListenedMouseKeyCodes[i], InputPhases.FINISH, raycastMouseEnabled);
                    }
                }
            }
        }

        private void HandleKey(int p_inputID, InputPhases p_phase, bool p_useRaycast)
        {
            if (p_phase == InputPhases.FINISH)
            {
                if (_dictInputs.ContainsKey(p_inputID))
                {
                    InputInfo __inputInfo = UpdateInputInfo(p_inputID, p_phase, p_useRaycast);
                    _dictInputs.Remove(p_inputID);
                    onInput?.Invoke(__inputInfo);
                }
            }
            else if (p_phase == InputPhases.UPDATE)
            {
                onInput?.Invoke(UpdateInputInfo(p_inputID, p_phase, p_useRaycast));
            }
            else if (p_phase == InputPhases.START)
            {
                InputInfo __inputInfo = CreateInputInfo((KeyCode)p_inputID, p_phase, p_useRaycast);
                _dictInputs.Add(p_inputID, __inputInfo);
                onInput?.Invoke(__inputInfo);
            }
        }

        private InputInfo CreateInputInfo(KeyCode p_keyCode, InputPhases p_phase, bool p_useRaycast)
        { 
            Vector2 __mousePosition = Input.mousePosition;

            InputInfo __inputInfo = new InputInfo(
                p_keyCode,
                p_phase,
                __mousePosition,
                (InputRayManager != null && p_useRaycast) ? InputRayManager.CreateRayInfo(viewCamera, __mousePosition) : null);

            return __inputInfo;
        }

        private InputInfo UpdateInputInfo(int p_inputID, InputPhases p_phase, bool p_useRaycast)
        {
            InputInfo __inputInfo = null;

            if (_dictInputs.TryGetValue(p_inputID, out __inputInfo))
            {
                Vector2 __mousePosition = Input.mousePosition;

                _dictInputs[p_inputID].SetPhase(p_phase);
                _dictInputs[p_inputID].SetScreenPoint(__mousePosition);
                _dictInputs[p_inputID].SetRayInfo((InputRayManager != null && p_useRaycast) ? InputRayManager.CreateRayInfo(viewCamera, __mousePosition) : null);
            }

            return __inputInfo;
        }
        #endregion
    }
}
