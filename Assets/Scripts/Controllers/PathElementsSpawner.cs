using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PathElementsSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject pathElementPrefab;
        [SerializeField] private GameObject pathPrefab;

        private Stack<GameObject> pathsGameObjects = new Stack<GameObject>();

        private GameObject lastPath;

        public GameObject SpawnPathElement(Vector3 from, Vector3 to)
        {
            var middlePosition = (from + to) / 2;
            middlePosition.y = 0;

            var rotation = Quaternion.LookRotation(to - from);
            rotation *= Quaternion.Euler(90, 0, 0);

            var elem = Instantiate(pathElementPrefab, middlePosition, rotation, lastPath.transform);
            var elemTransform = elem.transform;

            return elem;
        }

        public void InitPath()
        {
            lastPath = Instantiate(pathPrefab);
            pathsGameObjects.Push(lastPath);
        }

        public void DestroyPath()
        {
            if (pathsGameObjects.Count > 0)
            {
                Destroy(pathsGameObjects.Pop());

                if (pathsGameObjects.Count > 0)
                {
                    lastPath = pathsGameObjects.Peek();
                }
            }
        }
    }
}