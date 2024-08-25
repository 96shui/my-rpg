using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public LayerMask whatIsPlayer;

    [Header("Stunned info")]
    public float stunDruation = 1;
    public Vector2 stunDirectiom = new Vector2(10,12);
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;


    [Header("Move info")]
    public float moveSpeed = 1.5f;
    public float idleTime = 1;
    public float battleTime = 7;
    private float defautltMoveSpeed;

    [Header("Attack info")]
    public float attackDistance=2;
    public float battleDistance = 5;
    public float attackCooldown ;
    public float minAttackCooldown;
    public float maxAttackCooldown;
    [HideInInspector] public float lastTimeAttacked;

    [Header("MoveRange")]
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;

    [Header("Start transform")]
    public Transform startTransfrom;

    public EnemyStateMachine StateMachine {  get; private set; }
    public EntityFX fx { get; private set; }
    public string lastAnimBoolName {  get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine();

        defautltMoveSpeed = moveSpeed;
    }

    protected override void Start()
    {
        base.Start();
        fx = GetComponent<EntityFX>();
    }

    protected override void Update()
    {
        base.Update();

        

        StateMachine.currentState.Update();
    }

    public virtual void AssignLastAnimName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1-_slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defautltMoveSpeed;
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;

        }
        else
        {
            moveSpeed = defautltMoveSpeed;
            anim.speed = 1;
        }
    }

    public virtual void FreezeTimeFor(float druation) => StartCoroutine(FreezeTimeCoroutine(druation));

    protected virtual IEnumerator FreezeTimeCoroutine(float _seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(_seconds);

        FreezeTime(false);
    }

    #region Counter Attack Window
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned=true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }



    public virtual void AnimationFinishTrigger()
    {
        StateMachine.currentState.AnimationFinishTrigger();
        
    }

    public virtual void AnimationSpecialAttackTrigger()
    {

    }

    public virtual RaycastHit2D IsPlayerDetected()=>Physics2D.Raycast(wallCheck.position,Vector2.right*facingDir,battleDistance,whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,new Vector2(transform.position.x+attackDistance*facingDir,transform.position.y));
    }

    protected virtual bool CheckMoveRange()
    {
        if(GetComponent<Transform>().position.x < leftLimit.position.x && GetComponent<Rigidbody2D>().velocity.x<0)
        {
            Filp();
            return true;
        }
        else if( GetComponent<Transform>().position.x > rightLimit.position.x && GetComponent<Rigidbody2D>().velocity.x>0)
        {
            Filp();

            return true;
        }
        return false;
    }
    
}
