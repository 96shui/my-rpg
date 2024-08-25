using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private Enemy_Skeleton enemy;
    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);

        stateTimer = enemy.stunDruation;
        
       rb.velocity=new Vector2(-enemy.facingDir*enemy.stunDirectiom.x,enemy.stunDirectiom.y);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.fx.Invoke("CannelColorChange", 0);
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
