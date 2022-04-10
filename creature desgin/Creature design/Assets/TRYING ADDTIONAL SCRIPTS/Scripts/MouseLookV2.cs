using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookV2 : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 500;
    
    private Camera mainCamera;
    private float yRotation = 0;


    private void Awake()
    {
        Application.targetFrameRate = 60; // sets max framerate so my computer doesn't crash
        mainCamera = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // locking cursor to middle of screen and making it invisible
    }

    private void FixedUpdate()
    {
        if (GetComponentInChildren<interact>().getCameraMove())
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            transform.Rotate(Vector3.up * mouseX);

            yRotation -= mouseY;
            yRotation = Mathf.Clamp(yRotation, -90, 90);
            mainCamera.transform.localRotation = Quaternion.Euler(yRotation, 0, 0);
        }
    }
}
