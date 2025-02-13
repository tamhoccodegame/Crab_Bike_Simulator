using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCullingLayer : MonoBehaviour
{
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        float[] layerCulling = new float[32];
        int cityPropLayer = LayerMask.NameToLayer("CityProp");
        layerCulling[cityPropLayer] = 500f;
        cam.layerCullDistances = layerCulling;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
