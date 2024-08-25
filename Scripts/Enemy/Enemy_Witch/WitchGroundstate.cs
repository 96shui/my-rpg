using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WitchGroundstate : EnemyState
{
    protected Enemy_Witch enemy;
    protected Transform player;
    protected WitchGroundstate(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Witch _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();


        

        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < 15f)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

}
