using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Witch : Enemy
{
    [Header("Magic attack")]
    [SerializeField] private GameObject magicPrefab;
    [SerializeField] private float magicSpeed;

    public float jumpCooldown;
    [HideInInspector] public float lastTimeJump;
    public float SafeDistance;

    [Header("Addition collision check")]
    [SerializeField] private Transform groundBechindCheck;
    [SerializeField] private Vector2 groundBechindCheckSize;

    

    public WitchIdleState idleState { get; private set; }
    public  WitchMoveState moveState { get; private set; }
    public WitchBattleState battleState { get; private set; }
    public WitchAttackState attackState { get; private set; }
    public WitchDeadState deadState { get; private set; }
    public WitchJumpState jumpState { get; private set; }
    public WitchMoveLimitState moveLimitState { get; private set; }




    protected override void Awake()
    {
        base.Awake();

        idleState = new WitchIdleState(this, StateMachine, "Idle", this);
        moveState = new WitchMoveState(this, StateMachine, "Walk", this);
        battleState = new WitchBattleState(this, StateMachine, "Idle", this);
        attackState = new WitchAttackState(this, StateMachine, "Attack", this);
        deadState = new WitchDeadState(this, StateMachine, "Idle", this);
        jumpState = new WitchJumpState(this, StateMachine, "Jump", this);
        moveLimitState = new WitchMoveLimitState(this,StateMachine,"Walk",this);
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(idleState);
        
    }

    protected override void Update()
    {
        base.Update();
        if (CheckMoveRange())
        {
            StateMachine.ChangeState(moveLimitState);
        }
    }

    public override void Die()
    {
        base.Die();
        StateMachine.ChangeState(deadState);
    }

    public override void AnimationSpecialAttackTrigger()
    {


        GameObject newMagic = Instantiate(magicPrefab, attackCheck.position, Quaternion.identity);

        newMagic.GetComponent<WitchMagic_Controller>().SetupMagic(magicSpeed * facingDir, stats);
    }

    public bool GroundBehindCheck()
    {
       return Physics2D.BoxCast(groundBechindCheck.position, groundBechindCheckSize,0,Vector2.zero,whatIsGround);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireCube(groundBechindCheck.position,groundBechindCheckSize);
    }


}
