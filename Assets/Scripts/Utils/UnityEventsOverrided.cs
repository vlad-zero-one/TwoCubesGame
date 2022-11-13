using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Utils
{
    public class TwoVector3Event : UnityEvent<Vector3, Vector3> { }
    public class Vector3Event : UnityEvent<Vector3> { }
    public class CubeEvent : UnityEvent<Cube> { }
}