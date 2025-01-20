using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGame : MonoBehaviour
{
    public float timeDayNight = 1f; //Mot ngay trong Game bang bao nhieu tgian? 1ngay = 1phut
    public Light lighting;
    public Gradient gradient;
    public float rotationSpeed;//Toc do xoay: 360 = 1 vong MatTroi = 1 ngay
    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = 360f / (timeDayNight * 60f);

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);//Xoay theo truc X+ cua Light
        if (lighting != null & gradient != null)
        {
            float time = Mathf.PingPong(Time.time / (timeDayNight * 30f), 1f);
            lighting.color = gradient.Evaluate(time);
        }
    }

}
