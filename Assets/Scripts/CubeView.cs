using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider), typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CubeView : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private Vector3 startPoint;
    private float moveSpeed = 0.5f;

    private Vector3 targetPoint = new Vector3(10, 0, 10);
    private bool move;

    public UnityEvent OnReachTarget = new UnityEvent();

    public bool ReachesTarget;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        startPoint = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            move = !move;
        }

        Debug.Log("Distance " + (Vector3.Distance(transform.position - startPoint, targetPoint)));
        if (Vector3.Distance(transform.position - startPoint, targetPoint) < 1.3f)
        {
            move = false;
            ReachesTarget = true;
            OnReachTarget?.Invoke();

            Debug.Log("Reach target " + targetPoint);
        }
    }

    private void FixedUpdate()
    {
        if (move)
        {
            ReachesTarget = false;
            //rigidbody.velocity = new Vector3(targetPoint.x, targetPoint.y, targetPoint.z) * moveSpeed;

            if (Vector3.Distance(transform.position - startPoint, targetPoint) < 1.3f)
            {
                transform.position = transform.position + targetPoint * Time.deltaTime * moveSpeed;
            }
        }
        else
        {
            //rigidbody.velocity = Vector3.zero;
        }
    }

    public void MoveTo(Vector3 point)
    {
        Debug.Log(point);
        //if (rigidbody != null && rigidbody.velocity != null) rigidbody.velocity = Vector3.zero;
        targetPoint = point;
        move = true;
    }

    public void Stop()
    {
        move = false;
    }
}
