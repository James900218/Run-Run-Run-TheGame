using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class DirectorScript : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public GameObject lastSeenPosTarget;

    private Type enemyCurrState;
    private NavMeshAgent enAgent;

    public EnemyController en_Control;
    public PlayerControl pl_Control;
    public FiniteStateMachine en_FSM;

    private float absenceTimer;
    private Vector3 lastSeenPos;


    public void Start()
    {
        lastSeenPos = player.transform.position;

        enAgent = en_Control.agent;
        pl_Control = player.GetComponent<PlayerControl>();
    }

    public void Update()
    {
        // get current state
        enemyCurrState = en_FSM.currentState.GetType();

        // setting gameobject to be used as a target for agent
        lastSeenPosTarget.transform.position = lastSeenPos;

        if (enemyCurrState != typeof(ChaseState))
        {
            absenceTimer += Time.deltaTime;
        }
        else if (enemyCurrState == typeof(ChaseState) || enAgent.destination == lastSeenPos && enAgent.remainingDistance < enAgent.stoppingDistance)
        {
            absenceTimer = 0.0f;
        }

        if (absenceTimer >= 150)
        {
            enemy.GetComponent<EnemyController>().directorAssist = true;
        }
    }

    public void DirectEnemy()
    {
        lastSeenPos = player.transform.position;
        enAgent.SetDestination(lastSeenPos);
        Debug.Log("REDIRECT ENEMY TO PLAYER");
        absenceTimer = 0.0f;
    }

    public void BuffEnemy()
    {
        if (pl_Control.numKeys >= 1)
        {
            Debug.Log("BUFF ENEMY");
            float multiplier = 1 + (pl_Control.numKeys / 10);

            enAgent.speed *= multiplier; // if player has 2 keys, multiply by 1.2
            enAgent.acceleration *= multiplier;

            en_Control.roamRange *= multiplier;
            en_Control.chaseRange *= multiplier;
        }
    }
}
