using UnityEngine;

public class PathElementsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pathElementPrefab;
    [SerializeField] private PathDragHandler pathDragHandler;

    private void Start()
    {
        pathDragHandler.newSegmentEvent.AddListener(SpawnPathElement);
    }

    private void SpawnPathElement(Vector3 start, Vector3 end)
    {
        var middlePosition = (start + end) / 2;
        middlePosition.y = 0;

        var rotation = Quaternion.LookRotation(end - start);
        rotation *= Quaternion.Euler(90, 0, 0);

        var elem = Instantiate(pathElementPrefab, middlePosition, rotation);
        var elemTransform = elem.transform;
    }

    private void OnDestroy()
    {
        pathDragHandler.newSegmentEvent.RemoveListener(SpawnPathElement);
    }
}
