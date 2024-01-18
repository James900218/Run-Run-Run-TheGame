using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private EnemyController m_Enemy;

    public AttackState(EnemyController m_Enemy)
    {
        this.m_Enemy = m_Enemy;
    }
    public override Type StateEnter()
    {
        m_Enemy.Attack();
        return null;
    }

    public override Type StateExit()
    {
        return null;
    }

    public override Type StateUpdate()
    {
        if (Vector3.Distance(m_Enemy.transform.position, m_Enemy.playerPos.transform.position) < 3)
        {
            m_Enemy.Attack();
            return typeof(AttackState);
        }
        else
        {
            return typeof(ChaseState);
        }
    }

}
