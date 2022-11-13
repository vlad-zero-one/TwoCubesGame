using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider), typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Cube : MonoBehaviour
    {
        private float moveSpeed = 5f;

        private Vector3 targetPoint;
        private bool move;

        public UnityEvent OnReachTarget = new UnityEvent();
        private Coroutine colliderCoroutine;

        private GameSettings settings;

        public UnityEvent OnReachEndSphere = new UnityEvent();
        public UnityEvent OnAnotherCubeTouched = new UnityEvent();

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.CompareTag(ObjectTags.EndSphere))
            {
                GetComponent<Collider>().enabled = false;

                OnReachEndSphere?.Invoke();
            }
            
            if (collider.gameObject.CompareTag(ObjectTags.Cube))
            {
                OnAnotherCubeTouched?.Invoke();
                Stop();
            }
        }

        internal void Init()
        {
            settings = DI.Get<GameSettings>();
            moveSpeed = settings.CubeMoveSpeed;

            if (colliderCoroutine == null)
            {
                colliderCoroutine = StartCoroutine(DelayedEnableCollider());
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                move = !move;
            }

            //Debug.Log("Distance " + (Vector3.Distance(transform.position, targetPoint)));
            if (Vector3.Distance(transform.position, targetPoint) < settings.DistanceToPointForCompleteMove)
            {
                move = false;
                OnReachTarget?.Invoke();

                //Debug.Log("Reach target " + targetPoint);
            }
        }

        public void MoveTo(Vector3? point)
        {
            if (point == null)
            {
                move = false;
                GetComponent<Collider>().enabled = false;
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

        private void OnDestroy()
        {
            RemoveAllListeners();
        }

        public void RemoveAllListeners()
        {
            OnReachTarget.RemoveAllListeners();
            OnReachEndSphere.RemoveAllListeners();
            OnAnotherCubeTouched.RemoveAllListeners();
        }
    }
}
