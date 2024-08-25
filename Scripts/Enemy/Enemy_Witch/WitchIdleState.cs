using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchIdleState : WitchGroundstate
{
    public WitchIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Witch _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
        enemy = _enemy;
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
        if (stateTimer < 0f)
        {
            stateMachine.ChangeState(enemy.moveState);

        }


    }
}
