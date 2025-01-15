using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeController : MonoBehaviour
{
    RaycastHit hit;
    float moveInput, steerInput, rayLength, currentVelocityOffset;

    [HideInInspector] public Vector3 velocity;

    public Rigidbody sphere, bikeBody;
    public GameObject handle;
    public GameObject frontTyre;
    public GameObject backTyre;
    public float maxSpeed, acceleration, steerStrength, tiltAngle, gravity, bikeXTiltIncreament = 0.09f, zTiltAngle = 45f, 
        handleRotationValue = 30f, handleRotationSpeed = 0.15f, tyreRotationSpeed = 10000f, normalDrag = 2f, driftDrag = 0.5f;
    [Range(1,10)]
    public float brakingFactor;
    public LayerMask surface;

    // Start is called before the first frame update
    void Start()
    {
        sphere.transform.parent = null;
        bikeBody.transform.parent = null;

        rayLength = sphere.GetComponent<SphereCollider>().radius + 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal"); 

        transform.position = sphere.transform.position;

        //***************************
        //transform.rotation = sphere.transform.rotation; // Đồng bộ rotation

        bikeBody.MoveRotation(transform.rotation);

        velocity = bikeBody.transform.InverseTransformDirection(bikeBody.velocity);
        currentVelocityOffset = velocity.z / maxSpeed;
    }

    private void FixedUpdate()
    {
        Movement();

        //visual
        frontTyre.transform.Rotate(Vector3.right, Time.deltaTime * tyreRotationSpeed * currentVelocityOffset);
        backTyre.transform.Rotate(Vector3.right, Time.deltaTime * tyreRotationSpeed * currentVelocityOffset);
    }

    void Movement()
    {
        if (Grounded())
        {
            if (!Input.GetKey(KeyCode.Space))
            {
                Acceleration();
                
            }
            Rotation();
            Brake();
        }
        else
        {
            Gravity();
        }
        BikeTilt();
    }

    private void Acceleration()
    {
        ////**********************************
        //Vector3 forwardDirection = sphere.transform.forward; // Lấy hướng di chuyển của sphere
        //sphere.velocity = Vector3.Slerp(sphere.velocity, moveInput * maxSpeed * forwardDirection, Time.fixedDeltaTime * acceleration);
        sphere.velocity = Vector3.Slerp(sphere.velocity, moveInput * maxSpeed * transform.forward, Time.fixedDeltaTime * acceleration);
    }

    private void Rotation()
    {
        ////*****************************
        //sphere.transform.Rotate(0, steerInput * currentVelocityOffset * steerStrength * Time.fixedDeltaTime, 0, Space.World);
        transform.Rotate(0, steerInput * moveInput * currentVelocityOffset * steerStrength * Time.fixedDeltaTime, 0, Space.World);
        //Visual
        handle.transform.localRotation = Quaternion.Slerp(handle.transform.localRotation, Quaternion.Euler(handle.transform.localRotation.eulerAngles.x, handleRotationValue * steerInput, handle.transform.localRotation.eulerAngles.z), handleRotationSpeed);
    }

    void BikeTilt()
    {
        float xRotation = (Quaternion.FromToRotation(bikeBody.transform.up, hit.normal) * bikeBody.transform.rotation).eulerAngles.x;

        float zRotation = 0;

        if(currentVelocityOffset > 0)
        {
            zRotation = -zTiltAngle * steerInput * currentVelocityOffset;
        }

        Quaternion targetRotation = Quaternion.Slerp(bikeBody.transform.rotation, Quaternion.Euler(xRotation, transform.eulerAngles.y, zRotation), bikeXTiltIncreament);
        
        Quaternion newRotation = Quaternion.Euler(targetRotation.eulerAngles.x, transform.eulerAngles.y, targetRotation.eulerAngles.z);

        bikeBody.MoveRotation(newRotation);
    }

    private void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            sphere.velocity *= brakingFactor / 10;
            sphere.drag = driftDrag;
        }
        else
        {
            sphere.drag = normalDrag;
        }
    }

    bool Grounded()
    {
        float radious = rayLength - 0.02f;
        Vector3 origin = sphere.transform.position + radious * Vector3.up;
        if (Physics.SphereCast(origin, radious + 0.02f, - transform.up , out hit, rayLength, surface))
        {
            return true;
        }
        else { return false; }
    }

    void Gravity()
    {
        sphere.AddForce(gravity * Vector3.down, ForceMode.Acceleration);
    }
}
