using Autodesk.Fbx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform groundFloor;
    public Transform secondFloor;

    public float elevatorSpeed;

    private Transform currentFloor;
    private bool isElevatorMoving = false;  

    // Start is called before the first frame update
    void Start()
    {
        currentFloor = groundFloor;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isElevatorMoving) return;
        Debug.Log(other.gameObject.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (currentFloor == groundFloor)
            {
                elevatorSpeed = Mathf.Abs(elevatorSpeed);
            }
            else
            {
                elevatorSpeed = -Mathf.Abs(elevatorSpeed);
            }

            currentFloor = currentFloor == groundFloor ? secondFloor : groundFloor;

            CharacterController characterController = other.gameObject.GetComponent<CharacterController>();
            TPlayerController tPlayerController = other.gameObject.GetComponent<TPlayerController>();
            if (tPlayerController != null)
            {
                StopAllCoroutines();  // Dừng bất kỳ coroutine nào trước đó (tránh lỗi xung đột)
                StartCoroutine(MoveElevator(currentFloor.position, tPlayerController, characterController));
            }
        }
    }

    private IEnumerator MoveElevator(Vector3 targetPosition, TPlayerController t, CharacterController c)
    {
        isElevatorMoving = true;
        t.canMove = false;
        t.transform.SetParent(transform, true);
        c.enabled = false;

        yield return new WaitForSeconds(.5f);
        while (Vector3.Distance(transform.position, targetPosition) > .1f)
        {
            Vector3 newPosition = transform.position;
            newPosition.y += Time.deltaTime * elevatorSpeed;
            transform.position = newPosition;
            Vector3 playerPosition = t.transform.position;
            playerPosition.y = transform.position.y;
            t.transform.position = playerPosition;
            yield return null; // Đợi 1 frame rồi tiếp tục (tránh treo game)
        }

        t.canMove = true;
        c.enabled = true;
        t.transform.SetParent(null);
        yield return new WaitForSeconds(1f);
        isElevatorMoving = false;
    }
}
