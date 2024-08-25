using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : EnemyState
{
    private Boss enemy
        ;
    public BossAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Boss boss) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = boss;
    }
    public override void Enter()
    {
        base.Enter();

        enemy.chanceToTp += 15;

    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if (triggerCalled)
        {
            if(enemy.CanToTp())
            stateMachine.ChangeState(enemy.TpState);
            else
            {
                stateMachine.ChangeState(enemy.battleState);
            }
        }
    }
}
