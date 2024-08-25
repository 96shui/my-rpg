using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpellCastState : EnemyState
{
    private Boss enemy;

    private int amountOfSpell;
    private float spellTime;

    public BossSpellCastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Boss _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        amountOfSpell = enemy.amountOfSpell;
        spellTime =  .5f;

    }

    public override void Exit()
    {
        base.Exit();


        enemy.lastTimeCast = Time.time;
    }

    public override void Update()
    {
        base.Update();

        spellTime -= Time.deltaTime;

        if (CanCast())
        {
            enemy.CastSpell();
        }


        if (amountOfSpell <= 0)
        {
            stateMachine.ChangeState(enemy.TpState);
        }
        
    }


    private bool CanCast()
    {
        if(amountOfSpell > 0 && spellTime <0)
        {
            amountOfSpell--;
            spellTime = enemy.spellCooldown;
            return true;
        }

        return false;
    }
}
