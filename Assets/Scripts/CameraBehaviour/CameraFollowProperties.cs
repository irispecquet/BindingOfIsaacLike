using System;
using UnityEngine;

namespace CameraBehaviour
{
    [Serializable]
    public class CameraFollowProperties
    {
        [field:SerializeField] public bool FollowOnX { get; private set; }
        [field:SerializeField] public bool FollowOnY { get; private set; }
        [field:SerializeField, Tooltip("x being the min value and y being the max")] public Vector2 ClampX { get; private set; }
        [field:SerializeField, Tooltip("x being the min value and y being the max")] public Vector2 ClampY { get; private set; }
    }
}