using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] Vector3 position;
    Collider co;
    private void Start()
    {
        // getting collider on start
        co = gameObject.GetComponent<Collider>();
    }

    // sending the gameobject back to specified coordinates and changing their velocity to zero
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = position;
        try
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
        }
        catch { }
    }
}
