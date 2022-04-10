using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fanmovement : MonoBehaviour
{
    public float speed = 100;

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        //Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        if (transform.position.y > 0.02)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), 0.1f);
            //rb.MovePosition(Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), 0.1f));
        }
        Transform ChildMeshTransform = gameObject.transform.GetChild(0);
        transform.RotateAround(ChildMeshTransform.position, Vector3.up, speed * Time.deltaTime);
    }
}
