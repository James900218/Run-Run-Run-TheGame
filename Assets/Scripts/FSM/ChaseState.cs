using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChaseState : BaseState
{
    private EnemyController m_Enemy;
    public float chaseRange = 20;
    public float attackRange = 3;

    // give state machine a reference of the GameObject
    public ChaseState(EnemyController m_Enemy)
    {
        this.m_Enemy = m_Enemy;
    }

    public override Type StateEnter()
    {
        return null;
    }

    public override Type StateExit()
    {
        return null;
    }

    public override Type StateUpdate()
    {
        // stop chasing enemy is player is far away or (10s) time has passed
        if (Vector3.Distance(m_Enemy.transform.position, m_Enemy.playerPos.transform.position) > chaseRange || Time.deltaTime > 10)
        {
            return typeof(RoamState);
        }
        else if (Vector3.Distance(m_Enemy.transform.position, m_Enemy.playerPos.transform.position) < attackRange)
        {
            return typeof(AttackState);
        }
        else
        {
            m_Enemy.Chase();
            return null;
        }
    }
}
