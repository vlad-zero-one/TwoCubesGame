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
        [SerializeField] private PathController pathController;
        [SerializeField] private StartEndPoint startPoint;

        private List<CubeView> cubes = new List<CubeView>();

        void Start()
        {
            pathController.OnPathsAreReady.AddListener(SpawnCubes);
        }

        private void SpawnCubes()
        {
            foreach(var path in pathController.Paths)
            {
                var cube = Instantiate(cubePrefab, startPoint.Position, Quaternion.identity, gameObject.transform).GetComponent<CubeView>();

                cubes.Add(cube);

                cube.Init();
                cube.OnReachTarget.AddListener(() => MoveCube(cube, path));
                MoveCube(cube, path);
            }
        }

        private void MoveCube(CubeView cube, Path path)
        {
            cube.MoveTo(path.GetNextPoint());
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log(pathController.Paths[0].GetNextPoint().Value);
            }
        }

        private void OnDestroy()
        {
            foreach(var cube in cubes)
            {
                cube.OnReachTarget.RemoveAllListeners();
            }
        }
    }
}