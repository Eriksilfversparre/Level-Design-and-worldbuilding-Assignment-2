using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Transactions;
using UnityEngine.SceneManagement;

public class Enemies : MonoBehaviour
{
    [Header("General Variables")]
    [SerializeField] private GameObject player;


    [Header("Movement Settings")]
    [SerializeField] private float defaultSpeed = 5f;
    [SerializeField] private float howCloseToPatrollingPoint = 0.1f;
    [SerializeField] private float accelerationWhenChasingPlayer = 40;
    [SerializeField] private float speedWhenChasingPlayer = 5;
    [SerializeField] private Transform[] patrollingPoints;
    private int patrollingCounter = 1;
    private int numberOfPointsToPatrol;
    public NavMeshAgent myAgent;
    public bool reachedDestination;
    private float defaultAcceleration = 40;

    [Header("Player in range Settings")]
    [SerializeField] private float distanceToKeepFromPlayer = 3.0f;
    private EnemyVision myFieldOfView;

    [Header("Health system Settings")]
    [SerializeField] public int health = 100;

    [Header("Attack Settings")]
    [SerializeField] private int damage = 20;


    
    private void Start()
    {
        myAgent = this.GetComponent<NavMeshAgent>();
        myAgent.speed = defaultSpeed;
        numberOfPointsToPatrol = patrollingPoints.Length;
        myFieldOfView = this.GetComponent<EnemyVision>();
        player = GameObject.FindGameObjectWithTag("Player");


        GoToNextPoint();

    }

    private void FixedUpdate()
    {
        if (!myAgent.enabled) return;

        if (myFieldOfView.canSeePlayer)
        {
            ChasePlayer();
        }
        else
        {
            Patrolling();
        }

    }

    private void ChasePlayer()
    {
        //set the player as destination and as thing to look
        this.transform.LookAt(player.transform.position);
        myAgent.SetDestination(player.transform.position);

        //set variables for the chasing player case
        myAgent.autoBraking = true;
        myAgent.speed = speedWhenChasingPlayer;
        myAgent.acceleration = accelerationWhenChasingPlayer;

        if (myAgent.remainingDistance < distanceToKeepFromPlayer)
        {
            myAgent.isStopped = true;
            //Time.timeScale = 0;
            player.GetComponent<Movement>().enabled = false;
            //StartCoroutine(ExecuteAfterTime(2f));
        }
        else
        {
            myAgent.isStopped = false;
        }
    }

    private void GoToNextPoint()
    {
        if (numberOfPointsToPatrol == 0)
            return;

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        myAgent.autoBraking = false;

        myAgent.SetDestination(patrollingPoints[patrollingCounter].position);

        patrollingCounter = (patrollingCounter + 1) % numberOfPointsToPatrol;

    }

    private void Patrolling()
    {

        myAgent.speed = defaultSpeed;
        myAgent.acceleration = defaultAcceleration;

        if (!myAgent.pathPending && myAgent.remainingDistance < howCloseToPatrollingPoint)
        {
            GoToNextPoint();
        }
    }

   /* IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(2);
    }*/
} 

