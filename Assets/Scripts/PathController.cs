using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PathController : MonoBehaviour
    {
        [SerializeField] private PathDragHandler pathDragHandler;
        [SerializeField] private StartEndPoint startPoint;
        [SerializeField] private StartEndPoint endPoint;
        [SerializeField] private PathElementsSpawner spawner;

        private bool isLegitStart;
        private bool isLegitEnd;

        private List<Path> paths = new List<Path>();
        private Path currentPath;

        private void Start()
        {
            pathDragHandler.OnBeginDragEvent.AddListener(BeginDrag);
            pathDragHandler.OnEndDragEvent.AddListener(EndDrag);
            pathDragHandler.NewSegmentEvent.AddListener(AddPathPoint);
        }

        private void EndDrag(Vector3 endDragPosition)
        {
            if (Vector3.Distance(endDragPosition, endPoint.transform.position) < 1 && isLegitStart)
            {
                isLegitEnd = true;
                paths.Add(currentPath);
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

            if (Vector3.Distance(beginDragPosition, startPoint.transform.position) < 1)
            {
                isLegitStart = true;
            }
        }

        private void AddPathPoint(Vector3 from, Vector3 to)
        {
            if (isLegitStart)
            {
                if (paths.Count < 2 && currentPath == null)
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
        }
    }
}