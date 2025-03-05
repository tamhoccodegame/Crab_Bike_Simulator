using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float initYPosition;
    private Quaternion initRotation;


    // Start is called before the first frame update
    void Start()
    {
        initYPosition = transform.position.y;
        initRotation = transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 position = transform.position;
        position.y = initYPosition;

        transform.position = position;
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }
}
