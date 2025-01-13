using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Làm mượt di chuyển CameraHolder
        transform.position = Vector3.Lerp(transform.position, cameraPosition.position, Time.deltaTime * 10f);
        //transform.position = cameraPosition.position;
    }
}
