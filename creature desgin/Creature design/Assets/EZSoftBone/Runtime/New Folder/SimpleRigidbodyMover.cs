using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleRigidbodyMover : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 initialPosition;
    private Vector3 prevPosition;
    private Vector3 targetPosition;
    private float prevMovementChangeTime = 0f;

    public Vector3 movementRange = Vector3.one;
    public float movementIntervalSeconds = 2f;
    public float movementLerpRate = 0.1f;
    public float movementSlerpRate = 0.1f;

    private void OnEnable()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        initialPosition = transform.position;
        PickNewTarget();
    }

    private void FixedUpdate()
    {
        if (Time.timeSinceLevelLoad > prevMovementChangeTime + movementIntervalSeconds)
        {
            prevMovementChangeTime = Time.timeSinceLevelLoad;
            PickNewTarget();
        }
        
        rb.MovePosition(Vector3.Lerp(transform.position, targetPosition, movementLerpRate));

        rb.MoveRotation(Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(prevPosition - transform.position),
            movementSlerpRate)
        );

        prevPosition = transform.position;     
    }

    private void PickNewTarget()
    {
        targetPosition = Random.insideUnitSphere;
        targetPosition.Scale(movementRange);
        targetPosition += initialPosition;
    }

}
