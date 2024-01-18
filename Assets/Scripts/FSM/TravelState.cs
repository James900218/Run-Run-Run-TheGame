using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelState : BaseState
{
    private EnemyController m_Enemy;
    private float detectTargetRange;

    public TravelState(EnemyController m_Enemy)
    {
        this.m_Enemy = m_Enemy;
    }
    public override Type StateEnter()
    {
        detectTargetRange = m_Enemy.chaseRange;
        m_Enemy.TravelToNewLocation();
        Debug.Log("Travel");
        return null;
    }

    public override Type StateExit()
    {
        return null;
    }

    public override Type StateUpdate()
    {
        // if waypoint met, change to roam state / if player seen, change to chase state
        if (Vector3.Distance(m_Enemy.transform.position, m_Enemy.target.transform.position) <= 5)
        {
            return typeof(RoamState);
        }
        else if (Vector3.Distance(m_Enemy.transform.position, m_Enemy.playerPos.transform.position) < detectTargetRange)
        {
            return typeof(ChaseState);
        }
        else
        {
            return null;
        }

    }

}
