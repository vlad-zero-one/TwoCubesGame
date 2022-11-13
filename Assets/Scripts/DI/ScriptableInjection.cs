using UnityEngine;

namespace Assets.Scripts
{
    public class ScriptableInjection : MonoBehaviour
    {
        [SerializeField] private ScriptableObject[] scriptableObjects;
        
        private void Awake()
        {
            foreach(var obj in scriptableObjects)
            {
                var type = obj.GetType();
                DI.Add(type, obj);
            }
        }
    }
}