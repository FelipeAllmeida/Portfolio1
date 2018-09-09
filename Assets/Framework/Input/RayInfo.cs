using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vox;

namespace Vox
{
    public class RayInfo
    {
        public GameObject Hit { get; internal set; }
        public Vector3 WorldPoint { get; internal set; }
    }
}
