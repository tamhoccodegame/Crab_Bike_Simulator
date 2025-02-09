using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeControllerW : MonoBehaviour
{
    //private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;

    private float horizontalInput;
    private float verticalInput;
    private bool isBreaking;
    private float steerAngle;

    public float motorForce = 1000f;
    public float brakeForce = 3000f;
    public float maxSteeringAngle = 30f;

    public WheelCollider frontWheelCollider;
    public WheelCollider rearWheelCollider;


    private Rigidbody rb;

    private void Start()
    {
        // Lấy Rigidbody và hạ trọng tâm để xe ổn định hơn
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0); // Hạ trọng tâm xuống dưới trục xe
    }

    private void Update()
    {
        frontWheelCollider.motorTorque = 0;

        rearWheelCollider.steerAngle = 0;

        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        // Chỉ áp dụng lực kéo cho bánh sau
        rearWheelCollider.motorTorque = verticalInput * motorForce;

        // Xử lý phanh
        currentbreakForce = isBreaking ? brakeForce : 0f;
        ApplyBreaking();

        //frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        //frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        //currentbreakForce = isBreaking ? brakeForce : 0f;
        //ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontWheelCollider.brakeTorque = currentbreakForce;
        rearWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        // Tính góc lái mục tiêu dựa trên input của người chơi
        steerAngle = maxSteeringAngle * horizontalInput;
        // Cập nhật góc lái cho bánh trước
        frontWheelCollider.steerAngle = steerAngle;
    }

    private void UpdateWheels()
    {
    }

    private void UpdateWheelPos(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }




    //private float horizontalInput, verticalInput;
    //private float currentSteerAngle, currentbreakForce;
    //private bool isBreaking;

    //// Settings
    //[SerializeField] private float motorForce, breakForce, maxSteerAngle;

    //// Wheel Colliders
    //[SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    //[SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    //// Wheels
    //[SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    //[SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    //private void FixedUpdate()
    //{
    //    GetInput();
    //    HandleMotor();
    //    HandleSteering();
    //    UpdateWheels();
    //}

    //private void GetInput()
    //{
    //    // Steering Input
    //    horizontalInput = Input.GetAxis("Horizontal");

    //    // Acceleration Input
    //    verticalInput = Input.GetAxis("Vertical");

    //    // Breaking Input
    //    isBreaking = Input.GetKey(KeyCode.Space);
    //}

    //private void HandleMotor()
    //{
    //    frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
    //    frontRightWheelCollider.motorTorque = verticalInput * motorForce;
    //    currentbreakForce = isBreaking ? breakForce : 0f;
    //    ApplyBreaking();
    //}

    //private void HandleSteering()
    //{
    //    currentSteerAngle = maxSteerAngle * horizontalInput;
    //    frontLeftWheelCollider.steerAngle = currentSteerAngle;
    //    frontRightWheelCollider.steerAngle = currentSteerAngle;
    //}

    //private void UpdateWheels()
    //{
    //    UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
    //    UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
    //    UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    //    UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    //}

    //private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    //{
    //    Vector3 pos;
    //    Quaternion rot;
    //    wheelCollider.GetWorldPose(out pos, out rot);
    //    wheelTransform.rotation = rot;
    //    wheelTransform.position = pos;
    //}
}
