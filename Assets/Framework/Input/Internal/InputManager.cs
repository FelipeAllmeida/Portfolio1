using System.Collections.Generic;
using UnityEngine;

namespace Vox.Internal
{
    public class InputManager
    {
        public InputManager()
        {
            _listMouseKeyCodes = new List<MouseKeyCode> { { MouseKeyCode.Mouse0 }, { MouseKeyCode.Mouse1 } };
        }

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

        [SerializeField] private List<MouseKeyCode> _listMouseKeyCodes;
        [SerializeField] private List<KeyCode> _listKeyboard;

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
        /// Callback called after a mouse input. It retuns a InputInfo.
        /// </summary>
        public InputEvent<InputInfo> onMouseClick;

        public void CheckInputs()
        {
            CheckMouseInputs();
        }

        #region Internal
        private void CheckMouseInputs()
        {            
            for (int i = 0; i < _listMouseKeyCodes.Count; i ++)
            {
                if (Input.GetKeyDown((KeyCode)_listMouseKeyCodes[i]))
                {
                    HandleKey((int)_listMouseKeyCodes[i], InputPhases.START);
                }
                else if (Input.GetKey((KeyCode)_listMouseKeyCodes[i]))
                {
                    HandleKey((int)_listMouseKeyCodes[i], InputPhases.UPDATE);

                }
                else if (Input.GetKeyUp((KeyCode)_listMouseKeyCodes[i]))
                {
                    HandleKey((int)_listMouseKeyCodes[i], InputPhases.FINISH);
                }
            }
        }

        private void HandleKey(int p_inputID, InputPhases p_phase)
        {
            if (p_phase == InputPhases.FINISH)
            {
                if (_dictInputs.ContainsKey(p_inputID))
                {
                    InputInfo __inputInfo = UpdateInputInfo(p_inputID, p_phase);
                    _dictInputs.Remove(p_inputID);
                    onMouseClick?.Invoke(__inputInfo);
                }
            }
            else if (p_phase == InputPhases.UPDATE)
            {
                onMouseClick?.Invoke(UpdateInputInfo(p_inputID, p_phase));
            }
            else if (p_phase == InputPhases.START)
            {
                InputInfo __inputInfo = CreateInputInfo(p_phase);
                _dictInputs.Add(p_inputID, __inputInfo);
                onMouseClick?.Invoke(__inputInfo);
            }
        }

        private InputInfo CreateInputInfo(InputPhases p_phase)
        { 
            Vector2 __mousePosition = Input.mousePosition;

            InputInfo __inputInfo = new InputInfo
            {
                Phase = p_phase,
                ScreenPoint = __mousePosition,
                RayInfo = (InputRayManager != null) ? InputRayManager.CreateRayInfo(viewCamera, __mousePosition) : null
            };

            return __inputInfo;
        }

        private InputInfo UpdateInputInfo(int p_inputID, InputPhases p_phase)
        {
            InputInfo __inputInfo = null;

            if (_dictInputs.TryGetValue(p_inputID, out __inputInfo))
            {
                Vector2 __mousePosition = Input.mousePosition;

                _dictInputs[p_inputID].Phase = p_phase;
                _dictInputs[p_inputID].ScreenPoint = __mousePosition;
                _dictInputs[p_inputID].RayInfo = (InputRayManager != null) ? InputRayManager.CreateRayInfo(viewCamera, __mousePosition) : null;
            }

            return __inputInfo;
        }
        #endregion
    }
}
