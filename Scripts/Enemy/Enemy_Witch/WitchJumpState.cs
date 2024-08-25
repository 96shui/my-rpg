using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchJumpState : EnemyState
{
    private Enemy_Witch enemy;
    private float jumpDection;
    public WitchJumpState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Witch _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.rb.velocity= new Vector2(20*-enemy.facingDir, 15);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.rb.velocity.y < 0 && enemy.IsGroundDetected())
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

}
