using Assets.Scripts.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class Cube : MonoBehaviour
    {
        public CubeEvent OnReachEndSphere = new CubeEvent();
        public UnityEvent OnAnotherCubeTouched = new UnityEvent();
        public UnityEvent OnReachTarget = new UnityEvent();

        private float moveSpeed;

        private Vector3 targetPoint;
        private bool move;

        private Coroutine colliderCoroutine;

        private GameSettings settings;

        public void Init()
        {
            settings = DI.Get<GameSettings>();
            moveSpeed = settings.CubeMoveSpeed;

            if (colliderCoroutine == null)
            {
                colliderCoroutine = StartCoroutine(DelayedEnableCollider());
            }
        }

        public void MoveTo(Vector3? point)
        {
            if (point == null)
            {
                move = false;
                GetComponent<Collider>().enabled = false;
                OnReachEndSphere?.Invoke(this);
            }
            else
            {
                targetPoint = point.Value;
                move = true;
            }
        }

        public void Stop()
        {
            move = false;
        }

        public void RemoveAllListeners()
        {
            OnReachTarget.RemoveAllListeners();
            OnReachEndSphere.RemoveAllListeners();
            OnAnotherCubeTouched.RemoveAllListeners();
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.CompareTag(ObjectTags.Cube))
            {
                OnAnotherCubeTouched?.Invoke();
                Stop();
            }
        }

        private IEnumerator DelayedEnableCollider()
        {
            yield return new WaitForSeconds(settings.DelayForEnableColliderOnStart);
            GetComponent<Collider>().enabled = true;
        }

        private void FixedUpdate()
        {
            MovementLogic();
        }

        private void MovementLogic()
        {
            if (move)
            {
                if (Vector3.Distance(transform.position, targetPoint) >= settings.DistanceToPointForCompleteMove)
                {
                    transform.position += (-transform.position + targetPoint).normalized * moveSpeed * Time.fixedDeltaTime;
                }
            }

        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, targetPoint) < settings.DistanceToPointForCompleteMove)
            {
                move = false;
                OnReachTarget?.Invoke();
            }
        }

        private void OnDestroy()
        {
            RemoveAllListeners();
        }
    }
}
