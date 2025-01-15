using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TPlayerController : MonoBehaviour
{
    public float rotationSpeed;
    public float walkSpeed;
    public float runSpeed;
    private float currentSpeed;

    private Vector3 movement;
    public bool canMove = true;

    private CharacterController controller;
    private Camera cam;

    private Animator animator;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cam = Camera.main;



        gameManager.onGameStateChange += OnGameStateChange;
        currentSpeed = walkSpeed;

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;

        ChangeSpeed();
        CalculateMove();

		if (movement.magnitude > Mathf.Epsilon)
		{
			Quaternion targetRotation = Quaternion.LookRotation(movement);
			animator.SetBool("Walking", true);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
		}
		else
		{
			animator.SetBool("Walking", false);
		}

		controller.Move(movement * currentSpeed * Time.deltaTime);

	}

    void OnGameStateChange(GameManager.GameState gameState)
    {
        if(gameState == GameManager.GameState.Menu)
        {
            canMove = false;
            cam.GetComponent<CinemachineBrain>().enabled = false;
        }
        else if(gameState == GameManager.GameState.Playing)
        {
            canMove = true;
			cam.GetComponent<CinemachineBrain>().enabled = true;
		}
	}

    void ChangeSpeed()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
            animator.SetBool("Running", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSpeed = walkSpeed;
            animator.SetBool("Running", false);
        }
    }

	void CalculateMove()
    {
		float horizontalMove = Input.GetAxisRaw("Horizontal");
		float verticalMove = Input.GetAxisRaw("Vertical");

		Vector3 camForward = cam.transform.forward;
		Vector3 camRight = cam.transform.right;
		camForward.y = 0;
		camRight.y = 0;

		movement = camForward * verticalMove + camRight * horizontalMove;
	}
}
