using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class NPCHeadLookAt : MonoBehaviour
{
    [SerializeField] private Rig rig;
    [SerializeField] private Transform headLookAtTransform;

    private bool isLookingAtPosition;
    private float stopLookDelay = 3f; // Thời gian NPC nhìn trước khi quay lại trạng thái bình thường
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float targetWeight = isLookingAtPosition ? 1.0f : 0.0f;
        float lerpSpeed = 2f;
        rig.weight = Mathf.Lerp(rig.weight, targetWeight, Time.deltaTime * lerpSpeed);
    }

    public void LookAtPosition(Vector3 lookAtPosition)
    {
        isLookingAtPosition = true;
        headLookAtTransform.position = lookAtPosition;

        // Sau 3 giây sẽ tự động dừng nhìn
        //CancelInvoke(nameof(StopLooking)); // Đảm bảo không có lệnh dừng cũ bị chồng chéo
        //Invoke(nameof(StopLooking), stopLookDelay);
        StartCoroutine(StopLookingAfterDelay(4f));
    }

    //public void StopLooking()
    //{
    //    isLookingAtPosition = false;
    //}

    private IEnumerator StopLookingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isLookingAtPosition = false;
    }
}
