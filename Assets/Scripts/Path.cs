using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Path
    {
        public List<GameObject> PathElements = new List<GameObject>();

        private List<Vector3> points = new List<Vector3>();
        private PathElementsSpawner spawner;
        private StartEndPoint endPoint;

        private int index = 0;

        public Path(PathElementsSpawner spawner, StartEndPoint startPoint, StartEndPoint endPoint)
        {
            this.spawner = spawner;
            points.Add(startPoint.Position);
            this.endPoint = endPoint;
            spawner.InitPath();
        }

        public Vector3? GetNextPoint()
        {
            if (index < points.Count)
            {
                //Debug.Log("!!! " + points[index]);
                return points[index++];
            }
            return null;
        }

        public void Add(Vector3 point)
        {
            var lastPoint = points.Last();
            points.Add(point);

            var elem = spawner.SpawnPathElement(lastPoint, points.Last());
            PathElements.Add(elem);
        }

        public void Clear()
        {
            Debug.Log("Clear");

            spawner.DestroyPath();
            PathElements.Clear();
            points.Clear();
        }

        internal void End()
        {
            Add(endPoint.Position);
        }
    }
}