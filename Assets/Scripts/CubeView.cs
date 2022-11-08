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
    public class CubeView : MonoBehaviour
    {
        private float moveSpeed = 5f;

        private Vector3 targetPoint = new Vector3(10, 0, 10);
        private bool move;

        public UnityEvent OnReachTarget = new UnityEvent();
        private Coroutine colliderCoroutine;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.CompareTag(ObjectTags.EndSphere))
            {
                GetComponent<Collider>().enabled = false;
            }
            
            if (collider.gameObject.CompareTag(ObjectTags.Cube))
            {
                Debug.LogError("YOU LOST!");
            }
        }

        internal void Init()
        {
            if (colliderCoroutine == null)
            {
                colliderCoroutine = StartCoroutine(DelayedEnableCollider());
            }
        }

        private IEnumerator DelayedEnableCollider()
        {
            yield return new WaitForSeconds(0.5f);
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
                if (Vector3.Distance(transform.position, targetPoint) >= 0.1f)
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

            Debug.Log("Distance " + (Vector3.Distance(transform.position, targetPoint)));
            if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
            {
                move = false;
                OnReachTarget?.Invoke();

                Debug.Log("Reach target " + targetPoint);
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
    }
}
