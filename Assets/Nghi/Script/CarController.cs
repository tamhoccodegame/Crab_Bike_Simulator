using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : BaseCarController
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

    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    public Transform steeringWheel;
    private float currentSteeringAngle = 0f;
    public float maxSteeringWAngle;


    public AudioSource engineSound;
    [Range(0, 1)] public float minPitch;
    [Range(1, 5)] public float maxPitch;

    private Rigidbody rb;
    private float currentVelocity;

    private void Start()
    {
        // Lấy Rigidbody và hạ trọng tâm để xe ổn định hơn
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0); // Hạ trọng tâm xuống dưới trục xe
    }

    private void FixedUpdate()
    {
        if(verticalInput > 0f)
        {
            currentVelocity += 0.2f * Time.fixedDeltaTime;
            currentVelocity = Mathf.Min(currentVelocity, maxPitch);
        }
        else if (verticalInput <= 0 || isBreaking)
        {
            currentVelocity -= 0.2f * Time.fixedDeltaTime;
            currentVelocity = Mathf.Max(minPitch, currentVelocity);
        }

            engineSound.pitch = Mathf.Lerp(minPitch, maxPitch, Mathf.Abs(currentVelocity));
    }
    private void Update()
    {
        frontLeftWheelCollider.motorTorque = 0;
        frontRightWheelCollider.motorTorque = 0;

        rearLeftWheelCollider.steerAngle = 0;
        rearRightWheelCollider.steerAngle = 0;

        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        UpdateSteeringWheels();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space) || verticalInput == 0;
    }

    private void HandleMotor()
    {
        // Chỉ áp dụng lực kéo cho bánh sau
        rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
        rearRightWheelCollider.motorTorque = verticalInput * motorForce;

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
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
        //rearLeftWheelCollider.steerAngle = currentSteerAngle;
        //rearRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateSteeringWheels()
    {
        // Lấy góc quay hiện tại của vô lăng theo trục Y
        currentSteeringAngle = steeringWheel.localEulerAngles.y;

        // Chuyển góc từ [0, 360] về [-180, 180] để dễ xử lý
        if (currentSteeringAngle > 180f)
            currentSteeringAngle -= 360f;

        // Chỉ cho phép quay nếu chưa vượt quá góc max
        if ((horizontalInput > 0 && currentSteeringAngle < maxSteeringAngle) ||
            (horizontalInput < 0 && currentSteeringAngle > -maxSteeringAngle))
        {
            steeringWheel.Rotate(Vector3.up * horizontalInput * 500f * Time.deltaTime);
        }

        // Nếu không có input, vô lăng sẽ tự động quay về vị trí trung tâm
        if (horizontalInput == 0)
        {
            float returnSpeed = 100f * Time.deltaTime; // Tốc độ trả lái
            float returnAmount = Mathf.Sign(-currentSteeringAngle) * returnSpeed;

            if (Mathf.Abs(currentSteeringAngle) > 1f) // Tránh dao động nhỏ
            {
                steeringWheel.Rotate(Vector3.up * returnAmount);
            }
        }

    }

    private void UpdateWheels()
    {
        UpdateWheelPos(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheelPos(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheelPos(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheelPos(rearRightWheelCollider, rearRightWheelTransform);
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
