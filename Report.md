

## Introduction

A simple 3D unity game, control the doll models using weapons to defeat other troublesome toys!

### Planning

![project plan](./images/basicProjectPlan.png)

### Design

![design](./images/A-UML-Class-Diagram-for-a-video-poker-game.png)

## Script

The script below is attached to the third-person controller.

```c#
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
    public float playerSprint=3;
    
    [Header("Player Animator and Gravity")]
    public CharacterController cC;
    public float gravity = -9.81f;


    [Header("Player Jumping and velocity")]
    public float turnCalmTime=0.1f;
    float turnCalmvelority;
    Vector3 velocity;
    public float jumpRange=1f;
    public Transform surfaceCheck;
    bool onSurface;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;

    void Start()
    {
        
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
    }

    void PlayerMove()
    {
        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal_axis,0f, vertical_axis).normalized;
        if(direction.magnitude>=0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg+playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle,ref turnCalmvelority,turnCalmTime);
            transform.rotation=Quaternion.Euler(0f,angle,0f);

            Vector3 moveDirection = Quaternion.Euler(0f,targetAngle,0f)*Vector3.forward;
            cC.Move(moveDirection.normalized*PlayerSpeed*Time.deltaTime);
        }
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump")&& onSurface)
        {
            velocity.y=Mathf.Sqrt(jumpRange*-2*gravity);
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
                float targetAngle = Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg+playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle,ref turnCalmvelority,turnCalmTime);
                transform.rotation=Quaternion.Euler(0f,angle,0f);
                Vector3 moveDirection = Quaternion.Euler(0f,targetAngle,0f)*Vector3.forward;
                cC.Move(moveDirection.normalized*playerSprint*Time.deltaTime);
            }
        }
    }
}
```

## Implementation

This 3D shooting game is about: toys shooting each other and winning when the owner is not at home!

## A Research Element

Toys 3D Models
Toys 3D Action
Sound Effects
Furniture Decoration

## Summary

Critique your coursework...

## Future Work

Describe how your coursework might evolve in the future...

## Appendix A

[A link to your GitHub repository.](https://github.com/zs297zijianshao/p3dunity/tree/main)

## Appendix B - Your Scripts

_Destroyer.cs_:

```c#
```

_SpawnObjects.cs_:

```c#
```

## Appendix C - Asset and Script References

+ [Standard Assets](https://assetstore.unity.com/packages/essentials/asset-packs/standard-assets-for-unity-2018-4-32351)
+ [Free Shipping Containers](https://assetstore.unity.com/packages/3d/environments/industrial/free-shipping-containers-18315)
+ [Old USSR Lamp](https://assetstore.unity.com/packages/3d/props/electronics/old-ussr-lamp-110400)
+ [PBR LAMPS PACK](https://assetstore.unity.com/packages/3d/props/interior/free-pbr-lamps-70181)
+ [Ball Pack](https://assetstore.unity.com/packages/3d/props/ball-pack-446)

...references to any scripts by other people that you've used. Give credit where credit's due!

### Appendix D - Report References

Peter Shirley, 2020, Ray Tracing in One Weekend, Diffuse Materials. Available at https://raytracing.github.io/books/RayTracingInOneWeekend.html#diffusematerials, Accessed October, 2023.
