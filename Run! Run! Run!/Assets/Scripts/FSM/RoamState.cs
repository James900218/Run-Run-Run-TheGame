using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamState : BaseState
{

    private EnemyController m_Enemy;
    public float detectTargetRange;
    private float m_Timer = 0f;
    public float searchDuration = 15f;
    
    // give state machine a reference of the GameObject
    public RoamState(EnemyController m_Enemy)
    {
        this.m_Enemy = m_Enemy;
    }

    public override Type StateEnter()
    {
        // return search timer back to 0 upon entering roam state
        m_Timer = 0f;
        detectTargetRange = m_Enemy.chaseRange;
        return null;
    }

    public override Type StateExit()
    {
        return null;
    }

    public override Type StateUpdate()
    {
        // start timer
        m_Timer += Time.deltaTime;

        // if a player is seen, go to chase state / if roaming for > searchDuration, go to travel state, else keep roaming
        if (Vector3.Distance(m_Enemy.transform.position, m_Enemy.playerPos.transform.position) < detectTargetRange)
        {
            return typeof(ChaseState);
        }
        else if (m_Timer >= searchDuration)
        {
            return typeof(TravelState);
        }
        else
        {
            m_Enemy.Roam();
            return null;
        }
    }
}
