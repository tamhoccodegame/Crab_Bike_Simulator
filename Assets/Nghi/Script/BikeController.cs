using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeController : MonoBehaviour
{
    RaycastHit hit;
    float moveInput, steerInput, rayLength, currentVelocityOffset;
    public Vector3 velocity;

    public float maxSpeed, accelaration, steerStrength, gravity, bikeXTiltIncrement, zTiltAngle = 45f;
    public GameObject handle;
    public float handleRotVal = 30f, handleRotSpeed = 0.15f;
    [Range(1f, 10f)]
    public float brakingFactor;
    public LayerMask derivableSurface;

    public Rigidbody sphereRb, bikeBody;

    public AudioSource engineSound;
    [Range(0, 1)] public float minPitch;
    [Range(1, 5)] public float maxPitch;

    private void Start()
    {
        sphereRb.transform.parent = null;
        bikeBody.transform.parent = null;
        
        rayLength = sphereRb.GetComponent<SphereCollider>().radius + 0.2f;
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        steerInput = Input.GetAxisRaw("Horizontal");
        Movement();

        transform.position = sphereRb.transform.position;
        bikeBody.MoveRotation(transform.rotation);

        velocity = bikeBody.transform.InverseTransformDirection(bikeBody.velocity);
        currentVelocityOffset = bikeBody.velocity.magnitude / maxSpeed;
    }

    private void FixedUpdate()
    {
        EngineSound();
    }

    void Movement()
    {
        if(Grounded())
        {
            if(!Input.GetKey(KeyCode.Space))
            {
                Accelaration();
                Rotation();
            }
            Brake();
        }
        else
        {
            Gravity();  
        }
        BikeTilt();
    }

    void Accelaration()
    {
        sphereRb.velocity = Vector3.Lerp(sphereRb.velocity, maxSpeed * moveInput * -transform.right, Time.fixedDeltaTime * accelaration);
    }

    void Rotation()
    {
        transform.Rotate(0, steerInput * moveInput * steerStrength  * Time.fixedDeltaTime, 0, Space.World);

        handle.transform.localRotation = Quaternion.Slerp(handle.transform.localRotation, Quaternion.Euler(handle.transform.localRotation.eulerAngles.x, handleRotVal * steerInput, handle.transform.localRotation.eulerAngles.z), handleRotSpeed);
    }

    void BikeTilt()
    {
        float xRot = (Quaternion.FromToRotation(bikeBody.transform.up, hit.normal) * bikeBody.transform.rotation).eulerAngles.x;
        float zRot = 0;

        if (currentVelocityOffset > 0)
        {
            zRot = -zTiltAngle * steerInput * currentVelocityOffset;
        }

        Quaternion targetRot = Quaternion.Slerp(bikeBody.transform.rotation, Quaternion.Euler(xRot, transform.eulerAngles.y, zRot), bikeXTiltIncrement);

        Quaternion newRotation = Quaternion.Euler(targetRot.eulerAngles.x, transform.eulerAngles.y, targetRot.eulerAngles.z);

        bikeBody.MoveRotation(newRotation);
    }

    void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            sphereRb.velocity *= brakingFactor / 10f;
        }
    }

    bool Grounded()
    {
        if (Physics.Raycast(sphereRb.position, Vector3.down, out hit, rayLength, derivableSurface))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Gravity()
    {
        sphereRb.AddForce(gravity * Vector3.down, ForceMode.Acceleration);
    }

    void EngineSound()
    {
        engineSound.pitch = Mathf.Lerp(minPitch, maxPitch, Mathf.Abs(currentVelocityOffset));
    }
}
