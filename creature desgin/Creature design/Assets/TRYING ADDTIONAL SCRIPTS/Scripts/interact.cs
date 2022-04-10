using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interact : MonoBehaviour
{
    [SerializeField] private float grabDistance = 2;
    [SerializeField] private LayerMask playerMask;
    private float breakForce = 500;

    private bool closeEnough;
    private bool grabbing = false;
    private bool inspecting = false;
    private bool lerping = false;
    private bool grabTool = false;
    private bool toolEquiped = false;
    private bool cameraMove = true;
    private bool unfreeze = false;
    private bool isHeavy = false;
    private RaycastHit hit;
    private SpringJoint joint;
    private GameObject grabbed;
    private int layer;
    private Vector3 objectStartPosition;
    private Vector3 objectStartRotation;
    private Vector3 inspectPosition = new Vector3(0, 0, 0.7f);
    private Vector3 toolPosition = new Vector3(0.4f, -0.2f, 0.6f);
    private GameObject[] tools = new GameObject[2];
    private KeyCode[] toolHotbar = { KeyCode.Alpha1, KeyCode.Alpha2 };
    private GameObject currentTool;


    private void Update()
    {
        closeEnough = Physics.Raycast(transform.position, transform.forward, out hit, grabDistance, playerMask);
        bool isPaused = GetComponentInParent<Pause>().getIsPaused();

        // grabbing objects, inspecting, and picking up tools
        if (Input.GetKeyDown(KeyCode.Mouse0) && closeEnough && !grabbing && !inspecting && !lerping && !grabTool && !toolEquiped && !isPaused)
        {
            if (hit.transform.tag.Equals("Object") || hit.transform.tag.Equals("Heavy Object"))
            {
                isHeavy = hit.transform.tag.Equals("Heavy Object");
                Grab(isHeavy);
            }
            else if (hit.transform.tag.Equals("Inspect"))
            {
                Inspect();
            }
            else if (hit.transform.tag.Equals("tool"))
            {
                GrabTool();
            }
        }
        // dropping
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (grabbing)
            {
                Drop();
            }
            else if (inspecting && !lerping)
            {
                UnInspect();
            }
        }
        // item movement when inspecting
        else if (inspecting)
        {
            InspectingMovement();
        }
        // equiping and unequiping tools
        else
        {
            for (int i = 0; i < toolHotbar.Length; i++)
            {
                if (Input.GetKeyDown(toolHotbar[i]))
                {
                    if (!toolEquiped)
                    {
                        EquipTool(tools[i]);
                    }
                    else if (currentTool != tools[i])
                    {
                        UnEquipTool(currentTool);
                        EquipTool(tools[i]);
                    }
                    else
                    {
                        UnEquipTool(tools[i]);
                    }
                }
            }
        }

        // item movement for tools
        if (toolEquiped)
        {
            ToolMovement();
        }

        if (grabbing && !isHeavy)
        {
            rotateGrabbedObject();
        }
    }



    // grabbing objects 
    private void Grab(bool isHeavy)
    {
        grabbing = true;
        grabbed = hit.transform.gameObject;
        joint = gameObject.AddComponent<SpringJoint>();
        joint.connectedBody = grabbed.GetComponent<Rigidbody>();

        // if the object is marked as heavy we do something different to it
        if (isHeavy)
        {
            joint.anchor = transform.InverseTransformPoint(hit.point);
            joint.connectedAnchor = grabbed.transform.InverseTransformPoint(hit.point);
        }
        else
        {
            joint.anchor = transform.InverseTransformPoint(hit.transform.position);
            joint.connectedAnchor = grabbed.transform.InverseTransformPoint(grabbed.transform.position);

            // configuring grabbed-object physics
            if (!joint.connectedBody.freezeRotation)
            {
                joint.connectedBody.freezeRotation = true;
                unfreeze = true;
            }
            else
            {
                unfreeze = false;
            }
        }

        // joint config
        joint.enableCollision = false;
        joint.enablePreprocessing = true;
        joint.autoConfigureConnectedAnchor = false;
        joint.maxDistance = 0;
        joint.minDistance = 0;
        joint.tolerance = 0;
        joint.spring = 150 * 3;
        joint.damper = 0;
        joint.breakForce = breakForce;
        joint.breakTorque = Mathf.Infinity;

        // physics
        joint.connectedBody.drag = 10;
        joint.connectedBody.angularDrag *= 10;
        joint.connectedBody.mass *= 3;

    }

    // dropping them
    private void Drop()
    {
        grabbing = false;
        cameraMove = true;
        joint.connectedBody.drag = 0;
        joint.connectedBody.angularDrag /= 10;
        joint.connectedBody.mass /= 3;
        if (unfreeze)
        {
            joint.connectedBody.freezeRotation = false;
        }
        grabbed = null;
        Destroy(joint);
    }

    // rotatiing grabbed objects
    private void rotateGrabbedObject()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float mouseSW = Input.GetAxis("Mouse ScrollWheel");
        cameraMove = false;

        if (Input.GetKey(KeyCode.Mouse1))
        {
            grabbed.transform.localEulerAngles = new Vector3(grabbed.transform.localEulerAngles.x, grabbed.transform.localEulerAngles.y - mouseX, grabbed.transform.localEulerAngles.z);
            grabbed.transform.Rotate(Vector3.right * mouseY);
        }
        else
        {
            cameraMove = true;
            grabbed.transform.eulerAngles = new Vector3(grabbed.transform.eulerAngles.x, grabbed.transform.eulerAngles.y + (mouseX * 100 * Time.deltaTime), grabbed.transform.eulerAngles.z);
        }
    }
    


    // inspecting items with inspect tag
    private void Inspect()
    {
        Time.timeScale = 0;
        inspecting = true;
        grabbed = hit.transform.gameObject;
        grabbed.transform.parent = transform;
        objectStartPosition = grabbed.transform.localPosition;
        objectStartRotation = grabbed.transform.eulerAngles;
        layer = grabbed.gameObject.layer;
        grabbed.gameObject.layer = LayerMask.NameToLayer("On Top");
        SetChildrenLayer(grabbed.gameObject);

        StopAllCoroutines();
        StartCoroutine(Lerp(objectStartPosition, inspectPosition, objectStartRotation, new Vector3(-90, transform.eulerAngles.y, grabbed.transform.eulerAngles.z), 0.25f));
    }

    // called after lerping
    private void Inspect2()
    {
        // restricting position of gameobject
        RestricPosition rp;
        rp = grabbed.AddComponent<RestricPosition>();
        rp.restrictX = true;
        rp.restrictY = true;
        rp.restrictZ = true;
        rp.upperLimit = new Vector3(0.25f, 0.25f, 0.1f);
        rp.lowerLimit = new Vector3(-0.25f, -0.25f, -0.2f);
        rp.point = inspectPosition;
    }

    // uninspecting
    private void UnInspect()
    {
        inspecting = false;
        grabbed.gameObject.layer = layer;
        SetChildrenLayer(grabbed.gameObject);
        Destroy(grabbed.GetComponent<RestricPosition>());

        StopAllCoroutines();
        StartCoroutine(Lerp(grabbed.transform.localPosition, objectStartPosition, grabbed.transform.eulerAngles, objectStartRotation, 0.25f));
    }

    // called after lerping
    private void UnInspect2() 
    { 
        Time.timeScale = 1;
        grabbed.transform.parent = null;
        grabbed = null;
        objectStartPosition = Vector3.zero;
    }

    // movement controls for inspecting
    private void InspectingMovement()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float mouseSW = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKey(KeyCode.Mouse1))
        {
            grabbed.transform.localPosition = new Vector3(grabbed.transform.localPosition.x + (mouseX / 100), grabbed.transform.localPosition.y + (mouseY / 100), grabbed.transform.localPosition.z);
        }
        else
        {
            grabbed.transform.localEulerAngles = new Vector3(grabbed.transform.localEulerAngles.x, grabbed.transform.localEulerAngles.y - mouseX, grabbed.transform.localEulerAngles.z);
            grabbed.transform.Rotate(Vector3.right * mouseY);
        }
        grabbed.transform.localPosition = new Vector3(grabbed.transform.localPosition.x, grabbed.transform.localPosition.y, grabbed.transform.localPosition.z + (mouseSW));
    }



    // lerping with a coroutine
    private IEnumerator Lerp(Vector3 start, Vector3 end, Vector3 startRotation, Vector3 endRotation, float lerpDuration)
    {
        float timeElapsed = 0;
        lerping = true;

        while (timeElapsed < lerpDuration)
        {
            if (grabbed == null)
            {
                lerping = false;
                yield break;
            }

            grabbed.transform.localPosition = Vector3.Lerp(start, end, timeElapsed / lerpDuration);
            grabbed.transform.rotation = Quaternion.Lerp(Quaternion.Euler(startRotation), Quaternion.Euler(endRotation), timeElapsed / lerpDuration);
            timeElapsed += Time.unscaledDeltaTime;

            yield return null;
        }
        lerping = false;

        if (inspecting)
        {
            Inspect2();
        }
        else if (grabTool)
        {
            GrabTool2();
        }
        else
        {
            UnInspect2();
        }
    }



    // grabbing tool
    private void GrabTool()
    {
        Time.timeScale = 0;
        grabTool = true;
        grabbed = hit.transform.gameObject;
        grabbed.GetComponent<Rigidbody>().isKinematic = true;
        grabbed.gameObject.layer = LayerMask.NameToLayer("On Top");
        grabbed.transform.parent = transform;
        objectStartPosition = grabbed.transform.localPosition;
        objectStartRotation = grabbed.transform.eulerAngles;

        foreach(Collider c in grabbed.GetComponents<Collider>())
        {
            c.isTrigger = true;
        }

        for (int i = 0; i < tools.Length; i++)
        {
            if (tools[i] == null)
            {
                tools[i] = grabbed;
                break;
            }
        }

        StartCoroutine(Lerp(objectStartPosition, new Vector3(0, -0.25f, 0), objectStartRotation, Vector3.zero, 0.3f));
    }

    private void GrabTool2()
    {
        grabbed.SetActive(false);
        grabTool = false;
        RestricPosition rp;

        // restricting position of gameobject
        rp = grabbed.AddComponent<RestricPosition>();
        rp.restrictX = true;
        rp.restrictY = true;
        rp.restrictZ = true;
        rp.upperLimit = new Vector3(0.5f, 0.1f, 0.1f);
        rp.lowerLimit = new Vector3(-0.5f, -0.2f, -0.1f);
        rp.point = inspectPosition;

        grabbed = null;
        Time.timeScale = 1;
        objectStartPosition = Vector3.zero;
        objectStartRotation = Vector3.zero;
    }

    // pulling out the tool
    private void EquipTool(GameObject tool)
    {
        if (tool == null)
        {
            return;
        }

        toolEquiped = true;
        currentTool = tool;
        if (joint != null)
        {
            Drop();
        }

        currentTool.SetActive(true);
        currentTool.transform.localEulerAngles = Vector3.zero;
        currentTool.transform.localPosition = toolPosition;
    }

    // putting tool away
    private void UnEquipTool(GameObject tool)
    {
        if (tool == null)
        {
            return;
        }

        toolEquiped = false;
        currentTool.SetActive(false);
        currentTool = null;
    }

    // tool movement when equiped
    private void ToolMovement()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float mouseSW = Input.GetAxis("Mouse ScrollWheel");
        cameraMove = false;

        if (Input.GetKey(KeyCode.Mouse1))
        {
            currentTool.transform.localPosition = new Vector3(currentTool.transform.localPosition.x + (mouseX / 100), currentTool.transform.localPosition.y + (mouseY / 100), currentTool.transform.localPosition.z);
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            currentTool.transform.localEulerAngles = new Vector3(currentTool.transform.localEulerAngles.x, currentTool.transform.localEulerAngles.y - mouseX, currentTool.transform.localEulerAngles.z);
            currentTool.transform.Rotate(Vector3.right * mouseY);
        }
        else
        {
            cameraMove = true;
        }
        currentTool.transform.localPosition = new Vector3(currentTool.transform.localPosition.x, currentTool.transform.localPosition.y, currentTool.transform.localPosition.z + (mouseSW));
    }


    // drops object when joint is broken
    private void OnJointBreak(float breakForce)
    {
        Drop();
    }

    // getters and setters
    public bool getGrabbing()
    {
        return grabbing;
    }
    public bool getInspecting()
    {
        return inspecting;
    }

    public bool getToolEquiped()
    {
        return toolEquiped;
    }
    public bool getCameraMove()
    {
        return cameraMove;
    }

    // sets the layer of every child object to its parent's layer. does not work of grandchildren
    private void SetChildrenLayer(GameObject obj)
    {
        foreach (Transform child in obj.transform)
        {
            child.gameObject.layer = obj.layer;
        }
    }
}