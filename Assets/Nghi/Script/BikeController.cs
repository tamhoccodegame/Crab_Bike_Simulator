using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BikeController : BaseCarController
{
    RaycastHit hit;
    float moveInput, steerInput, rayLength;
    public float currentVelocityOffset;
    public Vector3 velocity;

    public float maxSpeed, accelaration, steerStrength, gravity, bikeXTiltIncrement, xTiltAngle = 45f, tyreRotSpeed = 1000f;
    public GameObject handle;
    public GameObject frontTyre;
    public GameObject rearTyre;
    public GameObject carSmoke;

    public float handleRotVal = 30f, handleRotSpeed = 0.15f;
    [Range(1f, 10f)]
    public float brakingFactor;
    public LayerMask derivableSurface;

    public Rigidbody sphereRb, bikeBody;

    public AudioSource engineSound;
    [Range(0, 1)] public float minPitch;
    [Range(1, 5)] public float maxPitch;

    public GameObject headlight;

    private void Start()
    {
        LightingManager light = FindObjectOfType<LightingManager>();
        if(light != null)
        {
            if (light.TimeOfDay >= 7.5f && light.TimeOfDay < 20)
            {
                headlight.SetActive(false);
            }
            else
            {
                headlight.SetActive(true);
            }
        }

        sphereRb.transform.parent = null;
        bikeBody.transform.parent = null;
        this.enabled = false;
        
        rayLength = sphereRb.GetComponent<SphereCollider>().radius + 0.5f;
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        steerInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.H))
        {
            headlight.SetActive(!headlight.activeSelf);
        }
    }

    private void FixedUpdate()
    {
        Movement();
        EngineSound();
        carSmoke.SetActive(velocity.magnitude > 0.1f);
        transform.position = sphereRb.transform.position;

        velocity = bikeBody.transform.InverseTransformDirection(bikeBody.velocity);
        currentVelocityOffset = bikeBody.velocity.magnitude / maxSpeed;

        frontTyre.transform.Rotate(Vector3.forward * velocity.magnitude * 200 * Time.fixedDeltaTime);
        rearTyre.transform.Rotate(Vector3.forward * velocity.magnitude * 200 * Time.fixedDeltaTime);

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
        sphereRb.velocity = Vector3.Lerp(sphereRb.velocity, maxSpeed * moveInput * -transform.right, Time.deltaTime * accelaration);
    }

    void Rotation()
    {
        transform.Rotate(0, steerInput * moveInput * steerStrength  * Time.deltaTime, 0, Space.World);

        handle.transform.localRotation = Quaternion.Slerp(handle.transform.localRotation, Quaternion.Euler(handle.transform.localRotation.eulerAngles.x, handleRotVal * steerInput, handle.transform.localRotation.eulerAngles.z), handleRotSpeed);
    }

    void BikeTilt()
    {
        float xRot = 0;
        float zRot = (Quaternion.FromToRotation(bikeBody.transform.up, hit.normal) * bikeBody.transform.rotation).eulerAngles.z;
        
        if (currentVelocityOffset > 0)
        {
            xRot = xTiltAngle * steerInput * currentVelocityOffset;
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

    private void OnDestroy()
    {
        Destroy(bikeBody.gameObject);
        Destroy(sphereRb.gameObject);
    }
}
