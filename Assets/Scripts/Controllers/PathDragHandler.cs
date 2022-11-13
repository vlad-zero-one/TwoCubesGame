//using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class PathDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        public class TwoVector3Event : UnityEvent<Vector3, Vector3> { }
        public class Vector3Event : UnityEvent<Vector3> { }

        public TwoVector3Event NewSegmentEvent = new TwoVector3Event();
        public Vector3Event OnBeginDragEvent = new Vector3Event();
        public Vector3Event OnEndDragEvent = new Vector3Event();
        public UnityEvent OnCancelClick = new UnityEvent();

        private Vector3 mousePosition;

        private GameSettings gameSettings;

        public void Init(GameSettings gameSettings)
        {
            this.gameSettings = gameSettings;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnCancelClick?.Invoke();
            }

            if (eventData.clickCount == 2)
            {
                OnCancelClick?.Invoke();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            var ray = Camera.main.ScreenPointToRay(eventData.position);

            if (Physics.Raycast(ray, out var hit))
            {
                mousePosition = hit.point;
                OnBeginDragEvent?.Invoke(hit.point);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            var ray = Camera.main.ScreenPointToRay(eventData.position);

            if (Physics.Raycast(ray, out var hit))
            {
                if (Vector3.Distance(mousePosition, hit.point) > gameSettings.SegmentDistance)
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
            var ray = Camera.main.ScreenPointToRay(eventData.position);

            if (Physics.Raycast(ray, out var hit))
            {
                mousePosition = hit.point;
                OnEndDragEvent?.Invoke(hit.point);
            }
            else
            {
                mousePosition = default;
                OnEndDragEvent?.Invoke(mousePosition);
            }
        }

        private void OnDestroy()
        {
            NewSegmentEvent?.RemoveAllListeners();
            OnBeginDragEvent?.RemoveAllListeners();
            OnEndDragEvent?.RemoveAllListeners();
            OnCancelClick?.RemoveAllListeners();
        }
    }
}