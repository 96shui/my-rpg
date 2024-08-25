using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveLimitState : EnemyState
{
    private Enemy_Skeleton enemy;
    private bool canMoveNoLimit;
    public SkeletonMoveLimitState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        CanMoveNoLimit();
    }

    private void CanMoveNoLimit()
    {
        if (enemy.transform.position.x - enemy.startTransfrom.position.x < .2f)
        {
            canMoveNoLimit = true;
        }

        if (canMoveNoLimit)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
