using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vox
{
    public class InputEvent<T> : UnityEvent<T> where T : InputInfo
    {

    }

    public enum InputPhases
    {
        START,
        UPDATE,
        FINISH
    }

    public class InputInfo
    {
        public InputPhases Phase { get; internal set; }
        public Vector2 ScreenPoint { get; internal set; }
        public RayInfo RayInfo { get; internal set; }
    }
}
