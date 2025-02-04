using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform groundFloor;
    public Transform secondFloor;

    public float elevatorSpeed;

    private Transform currentFloor;

    // Start is called before the first frame update
    void Start()
    {
        currentFloor = groundFloor;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            currentFloor = currentFloor == groundFloor ? secondFloor : groundFloor;
            CharacterController playerController = collision.gameObject.GetComponent<CharacterController>();
            if(playerController != null)
            {
                StopAllCoroutines();  // Dừng bất kỳ coroutine nào trước đó (tránh lỗi xung đột)
                StartCoroutine(MoveElevator(currentFloor.position, playerController));
            }
        }
    }

    private IEnumerator MoveElevator(Vector3 targetPosition, CharacterController playerController)
    {
        playerController.enabled = false;
        playerController.transform.SetParent(transform, true);
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            Vector3 newPosition = transform.position;
            newPosition.y += Time.deltaTime * elevatorSpeed;
            transform.position = newPosition;
            yield return null; // Đợi 1 frame rồi tiếp tục (tránh treo game)
        }
        playerController.enabled = true;
        playerController.transform.SetParent(null);
    }
}
