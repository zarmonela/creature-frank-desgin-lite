using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestricPosition : MonoBehaviour
{
    public Vector3 upperLimit, lowerLimit = new Vector3(0, 0, 0);
    public Vector3 point = new Vector3(0, 0, 0);
    public bool restrictX, restrictY, restrictZ = true;

    private void Start()
    {
        upperLimit += point;
        lowerLimit += point;
    }

    private void LateUpdate()
    {
        if (restrictX)
        {
            transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, lowerLimit.x, upperLimit.x), transform.localPosition.y, transform.localPosition.z);
        }
        if (restrictY)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Clamp(transform.localPosition.y, lowerLimit.y, upperLimit.y), transform.localPosition.z);
        }
        if (restrictZ)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Clamp(transform.localPosition.z, lowerLimit.z, upperLimit.z));
        }
    }
}