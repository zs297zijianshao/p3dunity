using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //[Header("Enemy Health and Damage")]
    
    [Header("Enemy Things")]
    public NavMeshAgent enemyAgent;
    public Transform playerBody;
    public LayerMask playerLayer;

    [Header("Enemy Guarding Var")]
    public GameObject[] walkPoints;
    int currentEnemyPosition = 0;
    public float enemySpeed;
    float walkingpointRadius =10;


    //[Header("Sounds and UI")]

    //[Header("Enemy Animation and Spark effect")]

    [Header("Enemy mood/situation")]
    public float visionRadius;
    public float shootingRadius;
    public bool playerInvisionRadius;
    public bool playerInshootingRadius;

    private void Awake()
    {
        playerBody = GameObject.Find("Player").transform;
        enemyAgent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position,visionRadius,playerLayer);
        playerInshootingRadius = Physics.CheckSphere(transform.position,shootingRadius, playerLayer);
        if(!playerInvisionRadius && !playerInshootingRadius) 
        {
        Guard();
        //Debug.Log(walkPoints.Length+"aaaaaa");
        }
        if(playerInvisionRadius && !playerInshootingRadius) 
        {
            Pursueplayer();
            //Debug.Log(walkPoints.Length+"bbbbbb");
        }
    }
    private void Guard()
    {
        
      if(Vector3.Distance(walkPoints[currentEnemyPosition].transform.position,transform.position)<walkingpointRadius)  
        {
            currentEnemyPosition = Random.Range(0,walkPoints.Length);
            //Debug.Log("start guarding length"+walkPoints.Length+"position"+currentEnemyPosition);
            if(currentEnemyPosition>=walkPoints.Length)
            {
                currentEnemyPosition=0;
                
            }
            //Debug.Log("start guarding2");
            transform.position = Vector3.MoveTowards(transform.position,walkPoints[currentEnemyPosition].transform.position,Time.deltaTime*enemySpeed);
            //Debug.Log("start guarding3");
            transform.LookAt(walkPoints[currentEnemyPosition].transform.position);
        }
        //Debug.Log("????");
    }

    private void Pursueplayer()
    {
        enemyAgent.SetDestination(playerBody.position);
    }

}
