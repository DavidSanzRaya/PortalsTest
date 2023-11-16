using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerController : MonoBehaviour
{
    private Camera playerCamera;
    [SerializeField] private float sensitivity = 2.0f;
    [SerializeField] private float rotLimit = 45.0f;
    private float rotationX = 0.0f;
    Rigidbody rb;

    void Start()
    {
        playerCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //mouse movement per frame
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // X-axis rotation (vertical) with limits
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -rotLimit, rotLimit);

        //camera rotation
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        //transform.Rotate(Vector3.up * mouseX);
        Quaternion rotation = Quaternion.Euler(0, mouseX * sensitivity, 0);
        rb.MoveRotation(rb.rotation * rotation);
    }
}
