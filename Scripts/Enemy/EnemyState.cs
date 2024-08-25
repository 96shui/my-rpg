using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState 
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    protected Rigidbody2D rb;

    private string animBoolName;

    protected bool triggerCalled;
    protected float stateTimer;

    protected EnemyState(Enemy _enemyBase,EnemyStateMachine _stateMachine,string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.enemyBase = _enemyBase;
        this.animBoolName = _animBoolName;
    }

    public virtual void Update()
    {
        stateTimer = stateTimer-Time.deltaTime;
    }

    public virtual void Enter()
    {
        enemyBase.anim.SetBool(animBoolName,true);
        rb=enemyBase.rb;
        triggerCalled = false;
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName,false);
        enemyBase.AssignLastAnimName(animBoolName);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    

}
