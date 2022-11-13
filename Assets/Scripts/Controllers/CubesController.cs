using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class CubesController : MonoBehaviour
    {
        [SerializeField] private GameObject cubePrefab;

        public UnityEvent OnCubesTouched = new UnityEvent();
        public UnityEvent OnAllCubesReachedEndSphere = new UnityEvent();

        private StartEndSphere startSphere;
        private PathController pathController;

        private List<Cube> cubes = new List<Cube>();

        private int cubesInEndSphere = 0;

        public void Init(PathController pathController, StartEndSphere startSphere)
        {
            this.pathController = pathController;
            this.startSphere = startSphere;
            pathController.OnPathsAreReady.AddListener(SpawnCubes);
        }

        public void Restart()
        {
            cubesInEndSphere = 0;

            foreach (var cube in cubes)
            {
                Destroy(cube.gameObject);
            }
            cubes.Clear();

            startSphere = DI.Get<SpheresController>().StartSphere;
        }

        private void SpawnCubes()
        {
            foreach(var path in pathController.Paths)
            {
                var cube = Instantiate(cubePrefab, startSphere.Position, Quaternion.identity, gameObject.transform)
                    .GetComponent<Cube>();

                cubes.Add(cube);

                cube.Init();
                cube.OnReachTarget.AddListener(() => MoveCube(cube, path));
                cube.OnAnotherCubeTouched.AddListener(CubesTouched);
                cube.OnReachEndSphere.AddListener(CubeReachedEndSphere);

                MoveCube(cube, path);
            }
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