using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PathDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 mousePosition;
        
    private bool mousePressed;

    public TwoVector3Event NewSegmentEvent;

    public Vector3Event OnBeginDragEvent;
    public Vector3Event OnEndDragEvent;

    private void Awake()
    {
        NewSegmentEvent = new TwoVector3Event();
        OnBeginDragEvent = new Vector3Event();
        OnEndDragEvent = new Vector3Event();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        var ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out var hit))
        {
            mousePosition = hit.point;
            mousePressed = true;

            OnBeginDragEvent?.Invoke(hit.point);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");

        var ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out var hit))
        {
            if (Vector3.Distance(mousePosition, hit.point) > 1)
            {
                NewSegmentEvent?.Invoke(mousePosition, hit.point);

                mousePosition = hit.point;
            }
        }
        else
        {
            OnEndDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        var ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out var hit))
        {
            mousePosition = hit.point;
            mousePressed = false;

            OnEndDragEvent?.Invoke(hit.point);
        }
        else
        {
            mousePosition = default;
            mousePressed = false;

            OnEndDragEvent?.Invoke(mousePosition);
        }
    }

    private void OnDestroy()
    {
        NewSegmentEvent?.RemoveAllListeners();
        OnBeginDragEvent?.RemoveAllListeners();
        OnEndDragEvent?.RemoveAllListeners();
    }

    public class TwoVector3Event : UnityEvent<Vector3, Vector3> { }
    public class Vector3Event : UnityEvent<Vector3> { }
}
