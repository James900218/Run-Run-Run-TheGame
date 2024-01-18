using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.AI;
using System;
using System.Linq;

public class AnimationControlScript : MonoBehaviour
{
    public GameObject enemy;
    private Type fsm_State;
    public Animator animator;
    public NavMeshAgent agent;
    public float agentSpeed;
    private Vector3 lastPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        // linearly interploates between the current and last recorded position of the agent to return an accurate velocity as a float
        // this code was derived and edited from https://gamedev.stackexchange.com/questions/133380/how-do-i-find-an-accurate-current-speed-of-a-navmesh-agent

        agentSpeed = Mathf.Lerp(agentSpeed, (agent.transform.position - lastPosition).magnitude / Time.deltaTime, 0.75f);
        lastPosition = agent.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        fsm_State = enemy.GetComponent<FiniteStateMachine>().currentState.GetType();

        if (agentSpeed < 1)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
        else if (agentSpeed > 1 && agentSpeed < 5)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
        }
        else if (agentSpeed >= 5)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
        }

        if (animator.GetBool("isAttacking") == true && agentSpeed > 1)
        {
            animator.SetBool("isAttacking", false);
            ShoutAnim();
        }
    }

    public void AttackAnim()
    {
        animator.SetBool("isAttacking", true);
        enemy.GetComponent<EnemyController>().attackSFX.PlayDelayed(0.1f);
        animator.SetBool("Shout", true);
    }

    public void ShoutAnim()
    {
        animator.SetBool("Shout", true);
        enemy.GetComponent<EnemyController>().shoutSFX.PlayDelayed(0.9f);
        animator.SetBool("Shout", false);
    }

}
