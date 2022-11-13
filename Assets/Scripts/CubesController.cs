using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class CubesController : MonoBehaviour
    {
        [SerializeField] private GameObject cubePrefab;
        [SerializeField] private StartEndPoint startPoint;

        private PathController pathController;

        private List<Cube> cubes = new List<Cube>();

        public UnityEvent OnCubesTouched = new UnityEvent();
        public UnityEvent OnAllCubesReachedEndSphere = new UnityEvent();

        private int cubesInEndSphere = 0;

        public void Init(PathController pathController)
        {
            this.pathController = pathController;

            pathController.OnPathsAreReady.AddListener(SpawnCubes);
        }

        private void SpawnCubes()
        {
            foreach(var path in pathController.Paths)
            {
                var cube = Instantiate(cubePrefab, startPoint.Position, Quaternion.identity, gameObject.transform)
                    .GetComponent<Cube>();

                cubes.Add(cube);

                cube.Init();
                cube.OnReachTarget.AddListener(() => MoveCube(cube, path));
                cube.OnAnotherCubeTouched.AddListener(CubesTouched);
                cube.OnReachEndSphere.AddListener(CubeReachedEndSphere);

                MoveCube(cube, path);
            }
        }

        internal void Restart()
        {
            cubesInEndSphere = 0;

            foreach(var cube in cubes)
            {
                Destroy(cube.gameObject);
            }
            cubes.Clear();
        }

        private void CubeReachedEndSphere()
        {
            cubesInEndSphere++;
            if(cubes.Count == cubesInEndSphere)
            {
                OnAllCubesReachedEndSphere?.Invoke();
            }
        }

        private void CubesTouched()
        {
            OnCubesTouched?.Invoke();
        }

        private void MoveCube(Cube cube, Path path)
        {
            cube.MoveTo(path.GetNextPoint());
        }

        private void OnDestroy()
        {
            pathController.OnPathsAreReady.RemoveListener(SpawnCubes);
            foreach(var cube in cubes)
            {
                cube.RemoveAllListeners();
            }
            OnCubesTouched.RemoveAllListeners();
            OnAllCubesReachedEndSphere.RemoveAllListeners();
        }
    }
}