using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            var pos = startPoint.transform.position;
            pos.y = 0.5f;
            var cube = Instantiate(cubePrefab, pos, Quaternion.identity, gameObject.transform).GetComponent<CubeView>();

            cubes.Add(cube);
            cube.MoveTo(pathController.Paths[0].GetNextPoint().Value - startPoint.transform.position);
            cube.OnReachTarget.AddListener(() => cube.MoveTo(pathController.Paths[0].GetNextPoint().Value - startPoint.transform.position));
            //StartCoroutine(MoveCube(cube));
        }

        private IEnumerator MoveCube(CubeView cube)
        {
            var pointToMove = pathController.Paths[0].GetNextPoint();
            while (pointToMove != null)
            {
                Debug.Log("!");

                cube.MoveTo((Vector3)pointToMove - startPoint.transform.position);

                yield return new WaitUntil(() => cube.ReachesTarget);

                pointToMove = pathController.Paths[0].GetNextPoint();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log(pathController.Paths[0].GetNextPoint().Value);
            }
        }
    }
}