using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LINECON : MonoBehaviour
{
    public LineRenderer line;
    public Transform pos1;
    public Transform pos2;
    public Transform pos3;
    public Transform pos4;
    public Transform pos5;
    public Transform pos6;

    // Start is called before the first frame update
    void Start()
    {
        line.positionCount = 6;
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, pos1.position);
        line.SetPosition(1, pos2.position);
        line.SetPosition(2, pos3.position);
        line.SetPosition(3, pos4.position);
        line.SetPosition(4, pos5.position);
        line.SetPosition(5, pos6.position);

    }
}
