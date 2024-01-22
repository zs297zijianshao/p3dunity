using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CharacteController : MonoBehaviour
{

    //[Header("Health things")]
    // Start is called before the first frame update
    [Header("Player Script Cameras")]
    public Transform playerCamera;

    [Header("Player Movement")]
    public float PlayerSpeed=1.9f;
    public float playerSprint=5;
    
    [Header("Player Animator and Gravity")]
    public CharacterController cC;
    public float gravity = -9.81f;
    public Animator animator;


    [Header("Player Jumping and velocity")]
    public float turnCalmTime=0.01f;
    float turnCalmvelority;
    Vector3 velocity;
    public float jumpRange=2f;
    public Transform surfaceCheck;
    bool onSurface;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;

    void Start()
    {
        Cursor.lockState=CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        onSurface=Physics.CheckSphere(surfaceCheck.position,surfaceDistance,surfaceMask);
        if(onSurface&&velocity.y<0)
        {
            velocity.y=-2f;
        }
        //gravity
        velocity.y +=gravity*Time.deltaTime;
        cC.Move(velocity*Time.deltaTime);
        PlayerMove();
        Jump();
        Sprint();
        //addd to fixedupdate
    }

    /*void FixedUpdate()
    {
        onSurface=Physics.CheckSphere(surfaceCheck.position,surfaceDistance,surfaceMask);
        if(onSurface&&velocity.y<0)
        {
            velocity.y=-2f;
        }
        //gravity
        velocity.y +=gravity*Time.deltaTime;
        cC.Move(velocity*Time.deltaTime);
        PlayerMove();
        Jump();
        Sprint();
    }*/

    void PlayerMove()
    {
        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal_axis,0f, vertical_axis).normalized;
        if(direction.magnitude>=0.01f)
        {
            animator.SetBool("Walk",true);
            animator.SetBool("Running",false);
            animator.SetBool("Idle",false);
            animator.SetTrigger("Jump");
            animator.SetBool("AimWalk",false);
            animator.SetBool("IdleAim",false);
            float targetAngle = Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg+playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle,ref turnCalmvelority,turnCalmTime);
            transform.rotation=Quaternion.Euler(0f,angle,0f);

            Vector3 moveDirection = Quaternion.Euler(0f,targetAngle,0f)*Vector3.forward;
            cC.Move(moveDirection.normalized*PlayerSpeed*Time.deltaTime);
        }
        else
        {
            animator.SetBool("Idle",true);
            animator.SetTrigger("Jump");
            animator.SetBool("Walk",false);
            animator.SetBool("Running",false);
            animator.SetBool("AimWalk",false);
        }

    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump")&& onSurface)
        {
            animator.SetBool("Walk",false);
            animator.SetTrigger("Jump");
            velocity.y=Mathf.Sqrt(jumpRange*-2*gravity);
        }
        else
        {
            animator.ResetTrigger("Jump");
        }
    }

        void Sprint()
    {
        if(Input.GetButton("Sprint")&& Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow)&&onSurface)
        {
            float horizontal_axis = Input.GetAxisRaw("Horizontal");
            float vertical_axis = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal_axis,0f, vertical_axis).normalized;
            if(direction.magnitude>=0.1f)
            {
                animator.SetBool("Running",true);
                animator.SetBool("Idle",false);
                animator.SetBool("Walk",false);
                animator.SetBool("IdleAim",false);
                float targetAngle = Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg+playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle,ref turnCalmvelority,turnCalmTime);
                transform.rotation=Quaternion.Euler(0f,angle,0f);
                Vector3 moveDirection = Quaternion.Euler(0f,targetAngle,0f)*Vector3.forward;
                cC.Move(moveDirection.normalized*playerSprint*Time.deltaTime);
            }
            else
            {
                animator.SetBool("Idle",false);
                animator.SetBool("Walk",false);
            }
        }
    }
}
