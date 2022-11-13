using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class SpheresController : MonoBehaviour
    {
        private const float PlaneScaleFactor = 5f;
        private const float SphereScale = 0.5f;

        [SerializeField] private GameObject startSpherePrefab;
        [SerializeField] private GameObject endSpherePrefab;

        public StartEndSphere StartSphere => startSphere.GetComponent<StartEndSphere>();
        public StartEndSphere EndSphere => endSphere.GetComponent<StartEndSphere>();

        private GameObject planeGameObjest;

        private GameObject startSphere;
        private GameObject endSphere;

        public async Task Init(GameObject planeGameObjest)
        {
            this.planeGameObjest = planeGameObjest;
            var planeScale = planeGameObjest.transform.localScale;

            var startSphereposition = new Vector3();
            startSphereposition.x = GetRandomFloat(planeScale.x);
            startSphereposition.z = GetRandomFloat(planeScale.z);

            startSphere = Instantiate(startSpherePrefab, startSphereposition, Quaternion.identity);

            var endSpherePosition = new Vector3();
            endSpherePosition.x = GetRandomFloat(planeScale.x);
            endSpherePosition.z = GetRandomFloat(planeScale.z);

            while (Vector3.Distance(startSphereposition, endSpherePosition) < 2)
            {
                endSpherePosition.x = GetRandomFloat(planeScale.x);
                endSpherePosition.z = GetRandomFloat(planeScale.z);
            }

            endSphere = Instantiate(endSpherePrefab, endSpherePosition, Quaternion.identity);
        }

        public async Task Restart()
        {
            Destroy(startSphere);
            Destroy(endSphere);

            await Init(planeGameObjest);
        }

        private float GetRandomFloat(float planeScaleDimension)
        {
            return Random.Range(-planeScaleDimension * PlaneScaleFactor + SphereScale / 2,
                planeScaleDimension * PlaneScaleFactor - SphereScale / 2);
        }
    }
}