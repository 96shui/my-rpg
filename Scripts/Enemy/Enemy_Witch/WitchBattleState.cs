using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchBattleState : EnemyState
{
    private int moveDir;
    private Enemy_Witch enemy;
    private Transform player;

    public WitchBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Witch _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            

            if (enemy.IsPlayerDetected().distance < enemy.SafeDistance)
            {
                
               if(CanJump())
                    stateMachine.ChangeState(enemy.jumpState);
                
            }

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.position, enemy.transform.position) > 20f)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

       
        if (player.position.x > enemy.transform.position.x)
        {
           moveDir = 1;
        }
        else if (player.position.x < enemy.transform.position.x)
       {
            moveDir = -1;
        }

        enemy.FilpController(moveDir);
        //enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    private bool CanAttack()
    {
        if (Time.time > enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }

    private bool CanJump()
    {
        if(enemy.GroundBehindCheck()==false)
            return false;

        if(Time.time>= enemy.lastTimeJump + enemy.jumpCooldown)
        {
            enemy.lastTimeJump = Time.time;
            return true;
        }
        return false;
    }
}
