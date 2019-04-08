using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour, InteractiveObjectBase
{
    public enum AgentState
    {
        Idle = 0,
        Patrolling,
        Chasing,
        Dead
    }

    public AgentState state = AgentState.Chasing;

    public float distToChangeWaypoint = 1;
    private int activeWaypoint = 0;

    public Transform target;
    public GameObject[] waypoints;
    public float distToChaseTarget = 3f;
    public float distToAttackTarget = 3f;
    public float totalChasingTime = 0.1f;

    private float oldRemainingDistance = float.MaxValue;
    private float timeSinceLastSeenTarget = float.MaxValue;

    private NavMeshAgent navMeshAgent;
    private Animator animController;

    private int speedHashId;
    private int attackingHashId;
    private int dieHashId;
    private int agentHealth = 15;

    public AudioClip deathSound;
    private AudioSource audioSource;
    private bool playedSound = false;
    public Animation deadAnimation;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public string[] GetWaypoints()
    {
        string[] waypointNames = new string[waypoints.Length];
        for(int i = 0; i < waypoints.Length; i++)
        {
            waypointNames[i] = waypoints[i].name;
        }
        return waypointNames;
    }

   

    void Awake()
    {
        //Setup
        audioSource = GetComponent<AudioSource>();
        speedHashId = Animator.StringToHash("walkingSpeed");
        attackingHashId = Animator.StringToHash("attack");
        dieHashId = Animator.StringToHash("die");
        navMeshAgent = GetComponent<NavMeshAgent>();
        animController = GetComponent<Animator>();

        if (waypoints.Length < 1)
        {
            Debug.LogError("Add more waypoints to continue.");
        }
    }

    void Update()
    {
  //      Debug.Log(state);
        if (agentHealth <= 0)
        {
            state = AgentState.Dead;
            Die();
        }
        else if (state == AgentState.Idle)
            Idle();
        else if (state == AgentState.Patrolling)
            Patrol();
        else
            Chase();
    }


    private float FindRemainingDistance()
    {
        float remainingDist;
        //Path isn't ready
        if (navMeshAgent.pathPending)
        {
            remainingDist = oldRemainingDistance;
        }
        else
        {
            float distance = 0;
            Vector3[] corners = navMeshAgent.path.corners;
            //Complete distance, corner to corner of the agent's remaining path
            for (int i = 0; i < corners.Length - 1; i++)
            {
    //            Debug.Log(distance);
                distance += Vector3.Distance(corners[i], corners[i + 1]);
            }
            oldRemainingDistance = distance;
            remainingDist = distance;
        }
        return remainingDist;
    }


    private bool IsAngleLessThanThreshold(float threshold)
    {
        Vector3 targetDir = target.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        if (angle < threshold)
        {
            return true;
        }
        else
        {
            return false;
        }


    }

    void Chase()
    {
        navMeshAgent.stoppingDistance = 1.5f;
        if (agentHealth > 0)
        {
            Attack();
        }

        navMeshAgent.SetDestination(target.position);
        //Increase by time taken to complete last frame
        timeSinceLastSeenTarget += Time.deltaTime;

        //Can see target
   //     Debug.Log(FindRemainingDistance() + " < " + distToChaseTarget + " --> dist = " + (FindRemainingDistance() < distToChaseTarget));
        if (FindRemainingDistance() <= distToChaseTarget && IsAngleLessThanThreshold(110))
        {
  //          Debug.Log("can see");
            timeSinceLastSeenTarget = 0;
           
        }
        //Has lost sight of target
        else if (timeSinceLastSeenTarget > totalChasingTime)
        {
   //         Debug.Log("lost sight");
            Idle();
            Patrol();
        }
        else
        {
            navMeshAgent.isStopped = false;
            animController.SetFloat(speedHashId, 1f);
        }
    }


    void Attack()
    {
  //      Debug.Log("attack");
        if (IsAngleLessThanThreshold(50) && (FindRemainingDistance() < distToAttackTarget) && (timeSinceLastSeenTarget < totalChasingTime) && agentHealth > 0)
        {
            animController.SetTrigger(attackingHashId);
        }
    }

    void Idle()
    {
        navMeshAgent.isStopped = true;
        animController.SetFloat(speedHashId, 0.0f);
    }

    void Patrol()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.stoppingDistance = 0;
        navMeshAgent.SetDestination(waypoints[0].transform.position);
        animController.SetFloat(speedHashId, 1.0f);


        if (navMeshAgent.remainingDistance < distToChangeWaypoint)
        {
            activeWaypoint = (activeWaypoint + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[activeWaypoint].transform.position);
        }

    }


    public void OnInteraction()
    {
    //    Debug.Log("HIT");
        agentHealth = agentHealth - GameStatsController.WeaponHit;
    }

    void Die()
    {
        Debug.Log("dead");
        navMeshAgent.isStopped = true;
        animController.SetFloat(speedHashId, 0f);
    //    Debug.Log(animController.GetBool("die"));
        animController.SetTrigger(dieHashId);
        if (!playedSound)
        {
            audioSource.PlayOneShot(deathSound);
            playedSound = true;
        }
    }
}
