using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    // gameobjects
    public GameObject target;
    public NavMeshAgent agent;
    public Transform playerPos;
    public Light enemyPointLight;
    public Type currState;
    public GameObject body;
    public DirectorScript director;

    // travel
    public List<WayPointScript> wayPoints;
    public Transform closestWayPoint;
    public List<float> wpDistance;
    public float roamRange;
    public float chaseRange;
    // should the director assist the Enemy?
    public bool directorAssist;

    // animation
    public AnimationControlScript anim;
    private float radius = 3f;
    public LayerMask playerMask;
    public float attackSpeedTimer;
    public bool attacking;
    private bool tryHit = false;
    private float attackSpeedMax;

    //audio
    public AudioSource shoutSFX;
    public AudioSource attackSFX;
    public AudioSource travelSFX;
    public AudioSource chaseSFX;
    public AudioSource movementSFX;
    private Vector3 newPosition;
    private Vector3 currentPosition;

    private void Awake()
    {        
        // start state machine
        InitialiseStateMachine();

        attackSpeedMax = attackSpeedTimer;
        currentPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ChangePLColour();
        FootStepSFX();
        // time between attacks
        if (attacking == true)
        {
            attackSpeedTimer -= Time.deltaTime;
            if (attackSpeedTimer <= 0)
            {
                tryHit = true;
                attacking = false;
                attackSpeedTimer = attackSpeedMax;
            }
        }

        body.transform.rotation = agent.transform.rotation;
    }

    // create the state machine
    private void InitialiseStateMachine()
    {
        // new dictionary for this gameObject
        Dictionary<Type, BaseState> states = new Dictionary<Type, BaseState>();

        //add all states
        states.Add(typeof(RoamState), new RoamState(this));
        states.Add(typeof(ChaseState), new ChaseState(this));
        states.Add(typeof(TravelState), new TravelState(this));
        states.Add(typeof(AttackState), new AttackState(this));

        GetComponent<FiniteStateMachine>().SetState(states);
    }

    public void Chase()
    {
        chaseSFX.PlayDelayed(0.2f);
        //functionality of chase state
        target = playerPos.gameObject;
        agent.SetDestination(target.transform.position);
    }

    public void Roam()
    {
        //functionality of roam state
        // if current path is done
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;
            if (RandomRoam(this.transform.position, roamRange, out point))
            {
                agent.SetDestination(point);
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
            }
        }
    }

    public void Attack()
    {
        // attack the player
        // play animation
        anim.AttackAnim();
        // start timer for hit checking
        attacking = true;
        //stop the enemies movement
        agent.velocity = new Vector3(0, 0, 0);
        // check if the player is still in range
        bool confirmHit = Physics.CheckSphere(transform.position, radius, playerMask);
        // if the animation has finished and player is still in range
        if (confirmHit && tryHit == true  )
        {
            // confirm damage
            playerPos.gameObject.GetComponent<PlayerControl>().isHit = true;
        }

        tryHit = false;
    }

    public void TravelToNewLocation()
    {
        travelSFX.PlayDelayed(0.5f);

        // store dist for each node
        for (int i = 0; i < wayPoints.Count; i++)
        {
            // if this waypoint is the closest to the enemy, this is the new closestWayPoint
            if (wpDistance.Count < 6)
            {
                wpDistance.Add(Vector3.Distance(wayPoints[i].GetPosition(), this.transform.position));
            }
            if (wpDistance[i] < Vector3.Distance(closestWayPoint.position, this.transform.position))
            {
                closestWayPoint = wayPoints[i].transform; 
            }
        }

        if (!directorAssist)
        {


            //get waypoint timer values
            float t = 0;
            for (int i = 0; i < wayPoints.Count; i++)
            {
                if (wayPoints[i].GetWPTimer() > t)
                {
                    t = wayPoints[i].GetWPTimer();
                }
            }

            // search through waypoints to find the one with the highest priority
            for (int i = 0; i < wayPoints.Count; i++)
            {
                // if waypoint is not the closest and it has the highest timeSinceLastVisit
                if (wayPoints[i].gameObject != closestWayPoint && wayPoints[i].GetWPTimer() >= t)
                {
                    target = wayPoints[i].gameObject;
                    agent.SetDestination(target.transform.position);
                    closestWayPoint.position = wayPoints[i].GetPosition();
                    wayPoints[i].WpVisited();
                    break;
                }
            }
        }
        else
        {
            target = director.lastSeenPosTarget;
            director.DirectEnemy();
            directorAssist = false;
        }


    }

    public void ChangePLColour()
    {
        // change point light color if FSM state changes
        currState = GetComponent<FiniteStateMachine>().currentState.GetType();
        if (currState == typeof(RoamState))
        {
            enemyPointLight.color = Color.blue;
        }
        else if (currState == typeof(ChaseState))
        {
            enemyPointLight.color = Color.red;
        }
        else if (currState == typeof(TravelState))
        {
            enemyPointLight.color = Color.yellow;
        }
        else if (currState == typeof(AttackState))
        {
            enemyPointLight.color = Color.green;
        }
    }

    // random roaming function
    private bool RandomRoam(Vector3 centre, float range, out Vector3 result)
    {
        // takes random position in a sphere around the gameobject
        Vector3 randomPoint = centre + UnityEngine.Random.insideUnitSphere * range;
        // ray that holds the resulting vec3
        NavMeshHit hit;
        // takes a max range between random point and point from the navmesh
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    public void FootStepSFX()
    {
        newPosition = this.transform.position;

        if (Vector3.Distance(currentPosition, newPosition) >= 3)
        {
            movementSFX.Play();
            currentPosition = newPosition;
        }
    }
}
