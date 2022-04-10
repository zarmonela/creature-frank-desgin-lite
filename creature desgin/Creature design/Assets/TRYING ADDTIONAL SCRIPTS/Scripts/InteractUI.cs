using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractUI : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Sprite handOpen, handClosed, inspect, tool, defaultSprite;

    private Image img;

    private void Start()
    {
        img = transform.GetComponentInChildren<Image>();
    }

    private void Update()
    {
        img.enabled = true;
        bool closeEnough = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2, playerMask);
        bool grabbing = GetComponentInParent<interact>().getGrabbing();
        bool inspecting = GetComponentInParent<interact>().getInspecting();
        bool toolEquiped = GetComponentInParent<interact>().getToolEquiped();
        bool isPaused = GetComponentInParent<Pause>().getIsPaused();


        if (grabbing)
        {
            img.sprite = handClosed;
        }
        else if (closeEnough && (hit.transform.tag.Equals("Object") || hit.transform.tag.Equals("Heavy Object")))
        {
            img.sprite = handOpen;
        }
        else if (closeEnough && hit.transform.tag.Equals("Inspect"))
        {
            img.sprite = inspect;
        }
        else if (closeEnough && hit.transform.tag.Equals("tool"))
        {
            img.sprite = tool;
        }
        else
        {
            img.sprite = defaultSprite;
        }

        if (inspecting || toolEquiped || isPaused)
        {
            img.enabled = false;
        }
    }
}
