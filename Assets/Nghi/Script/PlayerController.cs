﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        //Idle,
        Move,
        Attack,
    }


    public Animator animator;
    public PlayerState currentState;

    //private float velocity = 0.0f;
    private float acceleration = 0.2f;
    private float deceleration = 0.6f;


    private int velocityHash;

    public float speed = 5f;
    public CharacterController characterController;
    //private float rotationSpeed = 5f;

    private float velocityZ = 0.0f;
    private float velocityX = 0.0f;

    //private float maxWalkVelocity = 1f;
    //private float maxRunVelocity = 2f;

    private float maxWalkVelocity = 0.5f;
    private float maxRunVelocity = 1.0f;

    private Vector3 moveDirection; // Hướng di chuyển


    public Transform orientation; // Gắn Empty Object giữ hướng Player

    //Attack
    public int attackAnimationCount; // Số lượng animations có trong Animator
    public Transform attackPoint;
    public float attackRange;
    public LayerMask npcLayer;
    public float attackDamage;

    public HitBox hitBox;
    public float attackCooldown = 2f;
    public float attackDuration = 0.2f;
    private float lastAttackTime;

    private bool canAttack; 

    private void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        velocityHash = Animator.StringToHash("Velocity");
    }

    private void Update()
    {
        UpdateAnimator();
        Move();
        RotatePlayerToCamera(); // Gọi hàm xoay Player

        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime >= attackCooldown)
        {
            ChangeState(PlayerState.Attack);
        }

        //if (Input.GetMouseButtonDown(0) && canAttack)
        //{
        //    ChangeState(PlayerState.Attack);
        //}
    }

    public void ChangeState(PlayerState newState)
    {
        switch (currentState)
        {
            //case PlayerState.Idle:
            //    break;
            case PlayerState.Move:
                break;
            case PlayerState.Attack:
                break;
        }

        switch (newState)
        {
            //case PlayerState.Idle:
            //    break;
            case PlayerState.Move:
                Move();
                break;
            case PlayerState.Attack:
                Attack();
                //StartCoroutine(AttackCoroutine());
                break;
        }

        currentState = newState;
    }

    public void Move()
    {
        bool forwardPressed = Input.GetKey("w");
        bool backwardPressed = Input.GetKey("s");
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");

        bool runPressed = Input.GetKey("left shift");

        // Tính vận tốc tối đa hiện tại (chạy hoặc đi bộ)
        float currentMaxVelocity = runPressed ? maxRunVelocity : maxWalkVelocity;

        // Tăng tốc (Acceleration)
        if (forwardPressed && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }
        if (backwardPressed && velocityZ > -currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }
        if (leftPressed && velocityX > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
        }
        if (rightPressed && velocityX < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        // Giảm tốc (Deceleration)
        if (!forwardPressed && velocityZ > 0)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }
        else if (!backwardPressed && velocityZ < 0)
        {
            velocityZ += Time.deltaTime * deceleration;
        }
        if (!leftPressed && velocityX < 0)
        {
            velocityX += Time.deltaTime * deceleration;
        }
        else if (!rightPressed && velocityX > 0)
        {
            velocityX -= Time.deltaTime * deceleration;
        }

        // Đặt lại vận tốc khi giá trị nhỏ
        if (!forwardPressed && !backwardPressed && Mathf.Abs(velocityZ) < 0.01f)
        {
            velocityZ = 0;
        }
        if (!leftPressed && !rightPressed && Mathf.Abs(velocityX) < 0.01f)
        {
            velocityX = 0;
        }

        // Khóa vận tốc khi chạy (Lock)
        if (forwardPressed && runPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        else if (forwardPressed && !runPressed && velocityZ > maxWalkVelocity)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        // Tương tự cho bên trái (Lock)
        if (leftPressed && runPressed && velocityX < -currentMaxVelocity)
        {
            velocityX = -currentMaxVelocity;
        }
        else if (leftPressed && !runPressed && velocityX < -maxWalkVelocity)
        {
            velocityX += Time.deltaTime * deceleration;
        }

        // Tương tự cho bên phải (Lock)
        if (rightPressed && runPressed && velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }
        else if (rightPressed && !runPressed && velocityX > maxWalkVelocity)
        {
            velocityX -= Time.deltaTime * deceleration;
        }


        // Tính toán hướng di chuyển
        moveDirection = new Vector3(velocityX, 0, velocityZ);
        Vector3 movement = orientation.TransformDirection(moveDirection) * speed * Time.deltaTime;

        // Di chuyển Player
        characterController.Move(movement);
        //Vector3 movement = moveDirection * speed * Time.deltaTime;

        //characterController.Move(transform.TransformDirection(movement));
    }


    void UpdateAnimator()
    {
        animator.SetFloat("VelocityZ", velocityZ);
        animator.SetFloat("VelocityX", velocityX);
    }

    // Hàm xoay hướng Player theo Camera
    void RotatePlayerToCamera()
    {
        // Lấy hướng quay từ orientation (hướng Camera)
        Vector3 targetDirection = orientation.forward;
        targetDirection.y = 0; // Khóa trục Y để không xoay theo chiều dọc

        // Nếu có hướng quay hợp lệ
        if (targetDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection); // Tính góc quay
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Quay mượt
        }
    }

    public void Attack()
    {
        // Kiểm tra nếu chưa hết cooldown thì không cho đánh
        if (Time.time - lastAttackTime < attackCooldown)
        {
            Debug.Log("Chưa thể tấn công, hãy chờ cooldown!");
            return;
        }

        int randomAttackAnimation = Random.Range(0, attackAnimationCount);

        animator.SetInteger("Index", randomAttackAnimation);
        animator.SetTrigger("isAttack");

        //Check va cham
        Collider[] hitNPC = Physics.OverlapSphere(attackPoint.position, attackRange, npcLayer);

        foreach (Collider npc in hitNPC)
        {
            //Tru mau
            if (npc != null)//Bi danh trung
            {
                //Invoke(nameof(ActivateHitbox), 0.2f); // Bật hitbox khi tay chạm vào NPC
                lastAttackTime = Time.time;
                //*******************
                //NPC_Health npc_Health = npc.GetComponent<NPC_Health>();
                //npc_Health.TakeDamage(attackDamage);
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        canAttack = false;

        int randomAttackAnimation = Random.Range(0, attackAnimationCount);
        animator.SetInteger("Index", randomAttackAnimation);
        animator.SetTrigger("isAttack");

        // Chờ cooldown trước khi có thể tấn công lần tiếp theo
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;

        //canAttack = false; // Không thể tấn công liên tục

        //int randomAttackAnimation = Random.Range(0, attackAnimationCount);
        //animator.SetInteger("Index", randomAttackAnimation);
        //animator.SetTrigger("isAttack");
        ////Check va cham
        //Collider[] hitNPCArray = Physics.OverlapSphere(transform.position, attackRange, npcLayer);
        //foreach(Collider npc in hitNPCArray)
        //{
        //    if (npc != null)
        //    {
        //        //Cho thoi gian hien thi, ton tai HitBox  
        //        yield return new WaitForSeconds(attackDuration);
        //    }
        //}
        ////Cho thoi gian CoolDown
        //yield return new WaitForSeconds(attackCooldown-attackDuration);
        //canAttack = true; // Cho phép tấn công lại
    }

    void ActivateHitbox()
    {
        hitBox.ActivateHitbox();
    }


    void DeactivateHitbox()
    {
        hitBox.DeactivateHitbox();
    }

    //Hien thi attackRange trong Scene
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
