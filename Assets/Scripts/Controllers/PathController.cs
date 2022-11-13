using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class PathController : MonoBehaviour
    {
        [SerializeField] private PathElementsSpawner spawner;

        public UnityEvent OnPathsAreReady = new UnityEvent();

        public List<Path> Paths => paths;

        private bool isLegitStart;

        private List<Path> paths = new List<Path>();
        private Path currentPath;

        private PathDragHandler pathDragHandler;
        private GameSettings gameSettings;
        private StartEndSphere startSphere;
        private StartEndSphere endSphere;

        public void Init(GameSettings gameSettings, 
            PathDragHandler pathDragHandler, 
            StartEndSphere startSphere, 
            StartEndSphere endSphere)
        {
            this.gameSettings = gameSettings;
            this.pathDragHandler = pathDragHandler;
            this.startSphere = startSphere;
            this.endSphere = endSphere;

            pathDragHandler.OnBeginDragEvent.AddListener(BeginDrag);
            pathDragHandler.OnEndDragEvent.AddListener(EndDrag);
            pathDragHandler.NewSegmentEvent.AddListener(AddPathPoint);
            pathDragHandler.OnCancelClick.AddListener(CancelPath);
        }

        private void CancelPath()
        {
            if (currentPath == null)
            {
                if (paths.Count > 0)
                {
                    paths.RemoveAt(paths.Count - 1);
                }
                spawner.DestroyPath();
            }
        }

        internal void Restart()
        {
            while (paths.Count > 0)
            {
                CancelPath();
            }

            startSphere = DI.Get<SpheresController>().StartSphere;
            endSphere = DI.Get<SpheresController>().EndSphere;
        }

        private void EndDrag(Vector3 endDragPosition)
        {
            if (Vector3.Distance(endDragPosition, endSphere.Position) <= gameSettings.SegmentDistance && isLegitStart)
            {
                if (paths.Count < gameSettings.PathsCount)
                {
                    currentPath.End();
                    paths.Add(currentPath);

                    if (paths.Count == gameSettings.PathsCount)
                    {
                        OnPathsAreReady?.Invoke();
                    }
                }
            }
            else
            {
                if (currentPath != null)
                {
                    currentPath.Clear();
                }
            }
            currentPath = null;
            isLegitStart = false;
        }

        private void BeginDrag(Vector3 beginDragPosition)
        {
            if (Vector3.Distance(beginDragPosition, startSphere.Position) <= gameSettings.SegmentDistance)
            {
                isLegitStart = true;
            }
        }

        private void AddPathPoint(Vector3 from, Vector3 to)
        {
            if (isLegitStart)
            {
                if (paths.Count < gameSettings.PathsCount && currentPath == null)
                {
                    currentPath = new Path(spawner, startSphere, endSphere);
                }
                
                if (currentPath != null)
                {
                    currentPath.Add(to);
                }
            }
        }

        private void OnDestroy()
        {
            pathDragHandler.OnBeginDragEvent.RemoveListener(BeginDrag);
            pathDragHandler.OnEndDragEvent.RemoveListener(EndDrag);
            pathDragHandler.NewSegmentEvent.RemoveListener(AddPathPoint);
            pathDragHandler.OnCancelClick.RemoveListener(CancelPath);

            OnPathsAreReady.RemoveAllListeners();
        }
    }
}