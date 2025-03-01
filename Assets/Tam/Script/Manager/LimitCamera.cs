using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitCamera : MonoBehaviour
{
    public Camera mainCamera;
    private Quaternion initRotation;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        initRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = initRotation;
        Vector3 position = player.position;
        position.y = transform.position.y;

        transform.position = position;
    }

}
