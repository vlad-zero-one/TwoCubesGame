using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class PathController : MonoBehaviour
    {
        [SerializeField] private StartEndPoint startPoint;
        [SerializeField] private StartEndPoint endPoint;
        [SerializeField] private PathElementsSpawner spawner;

        private bool isLegitStart;
        private bool isLegitEnd;

        private List<Path> paths = new List<Path>();
        private Path currentPath;

        public UnityEvent OnPathsAreReady = new UnityEvent();

        public List<Path> Paths => paths;

        private PathDragHandler pathDragHandler;
        private GameSettings gameSettings;

        public void Init(GameSettings gameSettings, PathDragHandler pathDragHandler)
        {
            this.gameSettings = gameSettings;
            this.pathDragHandler = pathDragHandler;

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
        }

        private void EndDrag(Vector3 endDragPosition)
        {
            if (Vector3.Distance(endDragPosition, endPoint.Position) <= gameSettings.SegmentDistance && isLegitStart)
            {
                isLegitEnd = true;
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
            isLegitEnd = false;

            if (Vector3.Distance(beginDragPosition, startPoint.Position) <= gameSettings.SegmentDistance)
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
                    currentPath = new Path(spawner, startPoint, endPoint);
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