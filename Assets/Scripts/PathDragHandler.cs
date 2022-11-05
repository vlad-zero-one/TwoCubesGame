using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PathDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 mousePosition;
        
    private bool mousePressed;

    public NewSegmentEvent newSegmentEvent;

    private void Awake()
    {
        if (newSegmentEvent == null)
        {
            newSegmentEvent = new NewSegmentEvent();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out var hit))
        {
            mousePosition = hit.point;
            mousePressed = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        var ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out var hit))
        {
            if (Vector3.Distance(mousePosition, hit.point) > 1)
            {
                newSegmentEvent?.Invoke(mousePosition, hit.point);

                mousePosition = hit.point;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        mousePressed = false;
    }

    private void OnDestroy()
    {
        newSegmentEvent.RemoveAllListeners();
    }

    public class NewSegmentEvent : UnityEvent<Vector3, Vector3> { }
}
