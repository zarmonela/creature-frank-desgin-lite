using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestricPositionRB : MonoBehaviour
{
    public Vector3 upperLimit, lowerLimit = new Vector3(0, 0, 0);
    public Vector3 point = new Vector3(0, 0, 0);
    public bool restrictX, restrictY, restrictZ = true;

    private Rigidbody rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        point = transform.TransformPoint(point);
        upperLimit += point;
        lowerLimit += point;
    }

    void LateUpdate()
    {
        if (restrictX)
        {
            rb.position = new Vector3(Mathf.Clamp(rb.position.x, lowerLimit.x, upperLimit.x), rb.position.y, rb.position.z);
        }
        if (restrictY)
        {
            rb.position = new Vector3(rb.position.x, Mathf.Clamp(rb.position.y, lowerLimit.y, upperLimit.y), rb.position.z);
        }
        if (restrictZ)
        {
            rb.position = new Vector3(rb.position.x, rb.position.y, Mathf.Clamp(rb.position.z, lowerLimit.z, upperLimit.z));
        }
    }
}
