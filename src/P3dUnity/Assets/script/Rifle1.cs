using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Build;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Rifle1 : MonoBehaviour
{
    [Header("Rifle Things")]
    public Camera camera1;
    public float giveDamageOf=10f;
    public float fireCharge=15f;
    public float shootingRange=100f;
    public Animator animator;
    public CharacteController player;

    [Header("Rifle Ammunition and shooting")]
    private int maximumAmmunition =20;
    private int mag=15;
    private int presentAmmunition;
    public float reloadingTime = 1.3f;
    private bool setReloading = false;
    private float nextTimeToShoot =0f;

    [Header("Rifle effects")]
    public ParticleSystem muzzleSpark;
    public GameObject impactEffect;

    private void Awake()
    {
        presentAmmunition = maximumAmmunition;
    }
    //[Header("Rifle Effects")]

    //[Header("Sounds and UI")]
    // Update is called once per frame
    void Update()
    {
        if(setReloading)
        return;
        if(presentAmmunition<=0)
        {
            StartCoroutine(Reload());
            return;
        }
        if(Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
        //if(Input.GetButton("Fire1"))
        {
            
            animator.SetBool("Fire",true);
            animator.SetBool("Idle",false);
            nextTimeToShoot = Time.time + 1f/fireCharge;
            Shoot();
        }
        else if(Input.GetButton("Fire2")&&Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("Idle",false);
            animator.SetBool("IdleAim",true);
            animator.SetBool("FireWalk",true);
            animator.SetBool("Walk",true);
            animator.SetBool("Reloading",false);
        }
        else if(Input.GetButton("Fire2")&&Input.GetButton("Fire1"))
        {
            animator.SetBool("Idle",false);
            animator.SetBool("IdleAim",true);
            animator.SetBool("FireWalk",true);
            animator.SetBool("Walk",true);
            animator.SetBool("Reloading",false);
        }
        else
        {
            animator.SetBool("Fire",false);
            animator.SetBool("Idle",true);
            animator.SetBool("FireWalk",false);
            animator.SetBool("Reloading",false);
        }
    }

    void Shoot()
    {
        if(mag==0)
        {
            //show ammo out text
            return;
        }

        presentAmmunition--;
        if(presentAmmunition==0)
        {
            mag--;
        }

        muzzleSpark.Play();
        RaycastHit hitInfo;
        if(Physics.Raycast(camera1.transform.position,camera1.transform.forward,out hitInfo,shootingRange))
        {
            Objects objects = hitInfo.transform.GetComponent<Objects>();
            Debug.Log(hitInfo.transform.name+"???");
            if(objects!=null)
            {
                objects.objectHitDamage(giveDamageOf);
                GameObject impactGO = Instantiate(impactEffect,hitInfo.point,Quaternion.LookRotation(hitInfo.normal));
                Destroy(impactGO,5f);
            }
        }
    }
    
    IEnumerator Reload()
    {
        player.PlayerSpeed = 0f;
        player.playerSprint = 0f;
        setReloading = true;
        Debug.Log("reloading");
        animator.SetBool("Reloading",true);
        yield return new  WaitForSeconds(reloadingTime);
        animator.SetBool("Reloading",false);
        presentAmmunition = maximumAmmunition;
        player.PlayerSpeed = 1.9f;
        player.playerSprint = 3;
        setReloading = false;
    }
}
