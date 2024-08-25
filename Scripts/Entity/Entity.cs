using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    
    public SpriteRenderer sr { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }

    #endregion

    [Header("Collision info")]
    public Transform attackCheck;
    public float attackCheckRadius = 1.2f;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance = 1;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance = .8f;
    [SerializeField] protected LayerMask whatIsGround;

    public int konckbackDir { get; private set; }

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;



    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackPower = new Vector2(7,12);
    [SerializeField] protected float knockbackDuration = .07f;
    protected bool isKnocked;


    public System.Action onFlipped;//一个自身不用写函数，只是接受其他函数并调用他们的函数

    protected virtual void Awake()
    {
        stats = GetComponent<CharacterStats>();
    }

    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        sr = GetComponentInChildren<SpriteRenderer>();
       
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {

    }


    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)//减缓一切速度函数
    {

    }

    protected virtual void ReturnDefaultSpeed()//动画速度恢复正常函数
    {
        anim.speed = 1;
    }

                        

    public virtual void DamageImpact()
    {

        StartCoroutine("HitKnockback");
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;

        rb.velocity = new Vector2(knockbackPower.x * konckbackDir, knockbackPower.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked=false;

        SetupZeroKnockbackPower();
    }

    public virtual void SetupKnockbackDir(Transform _damageDirection)
    {
        if (_damageDirection.position.x > transform.position.x)//如果我的位置大于敌人的位置，及我在右边，使其朝左边
        {
            konckbackDir = -1;
        }
        else
        {
            konckbackDir = 1;
        }
    }

    public void SetupKnockbackPower(Vector2 _knockbackPower)//被造成大量伤害被击退函数
    {
        knockbackPower = _knockbackPower;
    }


    #region Velocity
    public virtual void SetZeroVelocity()
    {
        if (isKnocked)
        {
            return;
        }

        rb.velocity = new Vector2(0, 0);
    }


    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
        {
            return;
        }

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FilpController(_xVelocity);
    }
    #endregion

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);



    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position,attackCheckRadius);
    }

    #endregion

    #region Flip
    public virtual void Filp()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if(onFlipped!=null)
        onFlipped();
    }

    public virtual void FilpController(float _x)
    {
        if (_x > 0 && !facingRight)
        {
            Filp();
        }
        else if (_x < 0 && facingRight)
        {
            Filp();
        }
    }
    #endregion

    

    public virtual void Die()
    {

    }

    protected virtual void SetupZeroKnockbackPower()
    {

    }

    public virtual void SetupDefailtFacingDir(int _x)
    {
        facingDir = _x;
        
        if(facingDir == -1)
        {
            facingRight = false;
        }
    }
}
