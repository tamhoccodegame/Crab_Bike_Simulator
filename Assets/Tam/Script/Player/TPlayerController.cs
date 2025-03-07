using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TPlayerController : Controller
{
    public static TPlayerController instance;
    public float rotationSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float accelaration;
    private float currentSpeed;

    private Vector3 movement;
    public bool canMove = true;

    private CharacterController controller;
    private Camera cam;

    private Animator animator;

    public HitBox hitBox;

    public enum PlayerMode
    {
        Normal,
        Shopping,
    }

    public PlayerMode playerMode = PlayerMode.Normal;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cam = Camera.main;

        currentSpeed = walkSpeed;

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("isAttack");
        }

        if (!canMove)
        {
            movement = Vector3.zero;    
            animator.SetBool("Walking", false);
            animator.SetBool("Running", false);
            return;
        }

        ChangeSpeed();
        CalculateMove();
	}

    void ActivateHitbox()
    {
        hitBox.ActivateHitbox();
    }

    void DeactivateHitbox()
    {
        hitBox.DeactivateHitbox();
    }

    private void FixedUpdate()
    {
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

    public void ChangePlayerMode(PlayerMode newMode)
    {
        if(playerMode != newMode)
        {
            playerMode = newMode;
            switch(playerMode)
            {
                case PlayerMode.Normal:
                    canMove = true;
                    break;
                case PlayerMode.Shopping:
                    canMove = false; 
                    break;
            }
        }
    }

    public void Footstep()
    {
        int index = Random.Range(1, 9);
        AudioManager.instance.PlaySound($"Footstep_{index}");
    }

    void ChangeSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift) && movement.magnitude > 0.1f)
        {
            if(currentSpeed < runSpeed)
            {
                currentSpeed += Time.deltaTime * accelaration;
            }

            animator.SetBool("Running", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || movement.magnitude < 0.1f)
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
