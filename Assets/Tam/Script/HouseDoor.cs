using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseDoor : MonoBehaviour
{
    public Transform door;
    public Transform hingePosition;
    public float openAngle;
    public float rotationSpeed;

    public bool isOpen = false;
    public bool isClosed = true;
    private float currentAngle = 0f;
    private PlayerInteractor currentPlayer;

    public void ShowPrompt()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentPlayer != null && Input.GetKeyDown(KeyCode.F))
        {
            isOpen = !isOpen;
            isClosed = !isClosed;
        }

        float targetAngle = isOpen ? openAngle : 0f;
        float angleStep = rotationSpeed * Time.deltaTime;

        if(Mathf.Abs(currentAngle - targetAngle) > 0.1f)
        {
            float newAngle = Mathf.MoveTowards(currentAngle, targetAngle, angleStep);
            door.RotateAround(hingePosition.position, Vector3.up, newAngle - currentAngle);
            currentAngle = newAngle;
        }
        else
        {
            isClosed = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInteractor p = other.GetComponent<PlayerInteractor>();
        if(p != null)
        {
            currentPlayer = p;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInteractor p = other.GetComponent<PlayerInteractor>();
        if(p != null && p == currentPlayer)
        {
            currentPlayer = null;
        }
    }

}
