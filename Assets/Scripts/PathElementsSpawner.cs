using UnityEngine;

public class PathElementsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pathElementPrefab;

    public GameObject SpawnPathElement(Vector3 from, Vector3 to)
    {
        var middlePosition = (from + to) / 2;
        middlePosition.y = 0;

        var rotation = Quaternion.LookRotation(to - from);
        rotation *= Quaternion.Euler(90, 0, 0);

        var elem = Instantiate(pathElementPrefab, middlePosition, rotation);
        var elemTransform = elem.transform;

        return elem;
    }

    public void DestroyPathElement(GameObject pathElement)
    {
        Destroy(pathElement);
    }
}
