using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu()]
    public class GameSettings : ScriptableObject
    {
        [Space, Header("Cube")]
        public float CubeMoveSpeed;
        public float DelayForEnableColliderOnStart;
        [Range(0.1f, 1f)]
        public float DistanceToPointForCompleteMove = 0.1f;

        [Space, Header("Path")]
        public float SegmentDistance;
        public int PathsCount;
    }
}