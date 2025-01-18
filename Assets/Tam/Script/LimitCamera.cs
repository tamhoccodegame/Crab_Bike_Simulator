using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitCamera : MonoBehaviour
{
    public Camera mainCamera;
    public float minX, maxX, minZ, maxZ;
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


        // Lấy chiều rộng và chiều cao của camera trong không gian thế giới
        float cameraHeight = 2f * mainCamera.orthographicSize; // Chỉ áp dụng cho camera orthographic
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Tính toán vị trí giới hạn để cạnh của camera không ra ngoài map
        float clampedX = Mathf.Clamp(transform.position.x, minX + cameraWidth / 2f, maxX - cameraWidth / 2f);
        float clampedZ = Mathf.Clamp(transform.position.z, minZ + cameraHeight / 2f, maxZ - cameraHeight / 2f);

        // Cập nhật vị trí camera
        transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
    }
}
