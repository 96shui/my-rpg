using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : EnemyState
{
    private Boss enemy;
    public BossIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Boss enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();

        AudioManage.instance.PlaySFX(38, enemy.transform);
    }

    public override void Update()
    {
        base.Update();
        if(Vector2.Distance(PlayerManager.instance.player.transform.position, enemy.transform.position) < 7)
        {
            enemy.bossFightBegun = true;
        }

        if(stateTimer < 0 && enemy.bossFightBegun)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
