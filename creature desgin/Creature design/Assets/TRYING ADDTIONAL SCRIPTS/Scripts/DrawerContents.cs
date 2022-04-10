using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerContents : MonoBehaviour
{
    public Collider co;
    private RestricPosition rp;

    private void OnTriggerStay(Collider other)
    {
        if (co.bounds.Contains(other.bounds.min) && co.bounds.Contains(other.bounds.max)) 
        {
            if (other.gameObject.GetComponent<RestricPosition>() == null)
            {
                rp = other.gameObject.AddComponent<RestricPosition>();
                rp.restrictX = true;
                rp.restrictY = true;
                rp.restrictZ = true;
                rp.upperLimit = new Vector3(0.1032f, 0.6947f, 0.3115f);
                rp.lowerLimit = new Vector3(-0.0178f, 0.69f, -0.3852f);
            }
            other.gameObject.GetComponent<RestricPosition>().point = transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Destroy(rp);
    }
}
