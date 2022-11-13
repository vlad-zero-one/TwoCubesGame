using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Path
    {
        public List<Vector3> Points => points;

        private List<Vector3> points = new List<Vector3>();
        private PathElementsSpawner spawner;
        private StartEndSphere endSphere;

        private int index = 0;

        public Path(PathElementsSpawner spawner, StartEndSphere startPoint, StartEndSphere endSphere)
        {
            this.spawner = spawner;
            points.Add(startPoint.Position);
            this.endSphere = endSphere;
            spawner.InitPath();
        }

        public Vector3? GetNextPoint()
        {
            if (index < points.Count)
            {
                return points[index++];
            }
            return null;
        }

        public void Add(Vector3 point)
        {
            var lastPoint = points.Last();
            points.Add(point);

            spawner.SpawnPathElement(lastPoint, points.Last());
        }

        public void Clear()
        {
            spawner.DestroyPath();
            points.Clear();
        }

        public void End()
        {
            Add(endSphere.Position);
        }
    }
}