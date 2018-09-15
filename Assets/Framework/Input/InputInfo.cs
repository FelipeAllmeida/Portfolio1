using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vox
{
    public enum InputPhases
    {
        START,
        UPDATE,
        FINISH
    }

    public class InputInfo
    {
        public InputInfo(KeyCode p_keyCode, InputPhases p_phase, Vector2 p_screenPoint, RayInfo p_rayInfo)
        {
            KeyCode = p_keyCode;
            Phase = p_phase;
            ScreenPoint = p_screenPoint;
            RayInfo = p_rayInfo;
        }

        public KeyCode KeyCode { get; private set; }
        public InputPhases Phase { get; private set; }
        public Vector2 ScreenPoint { get; private set; }
        public RayInfo RayInfo { get; private set; }

        public void SetPhase(InputPhases p_phase)
        {
            Phase = p_phase;
        }

        public void SetScreenPoint(Vector2 p_screenPoint)
        {
            ScreenPoint = p_screenPoint;
        }

        public void SetRayInfo(RayInfo p_rayInfo)
        {
            RayInfo = p_rayInfo;
        }
    }
}
