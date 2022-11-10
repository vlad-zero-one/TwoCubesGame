﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

        public UnityEvent OnPathsAreReady = new UnityEvent();

        public List<Path> Paths => paths;

        private GameSettings gameSettings;

        private void Awake()
        {
            DI.Add(this);
        }

        private void Start()
        {
            gameSettings = DI.Get<GameSettings>();
        
            pathDragHandler.OnBeginDragEvent.AddListener(BeginDrag);
            pathDragHandler.OnEndDragEvent.AddListener(EndDrag);
            pathDragHandler.NewSegmentEvent.AddListener(AddPathPoint);
            pathDragHandler.OnCancelClick.AddListener(CancelPath);
        }

        private void CancelPath()
        {
            Debug.Log("CancelPath");
            if (currentPath == null)
            {
                Debug.Log("else");

                if (paths.Count > 0)
                {
                    Debug.Log("paths.Count > 0");

                    paths.RemoveAt(paths.Count - 1);
                }
                spawner.DestroyPath();
            }
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("paths.Count " + paths.Count);
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
        }
    }
}