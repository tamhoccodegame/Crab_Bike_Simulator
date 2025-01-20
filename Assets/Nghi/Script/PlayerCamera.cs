using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    //public float turnSpeed = 15f;
    //Camera mainCamera;

    //public Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //Get mouse input 
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        // Cập nhật góc quay
        yRotation += mouseX;
        xRotation -= mouseY;
        //Giới hạn ngửa lên ngửa xuống không quá 90 độ 
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Rotate camera and orientation
        // Cập nhật rotation cho camera (xoay lên/xuống)
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        // Cập nhật rotation cho Orientation (xoay trái/phải)
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    //private void FixedUpdate()
    //{
    //    float yawRotation = mainCamera.transform.rotation.eulerAngles.y;
    //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawRotation, 0), turnSpeed * Time.fixedDeltaTime);
    //}
}
