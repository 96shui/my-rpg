using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    #region States
   
    public BossBattleState battleState { get; private set; }
    public BossAttackState AttackState { get; private set; }
    public BossDeathState DeathState { get; private set; }
    public BossIdleState idleState { get; private set; }
    public BossSpellCastState spellCastState { get; private set; }
    public BossTpState TpState { get; private set; }
    #endregion

    public bool bossFightBegun;

    [Header("Spell cast details")]
    [SerializeField] private GameObject spellPrefab;
    public int amountOfSpell;
    public float spellCooldown;
    public float lastTimeCast;
    [SerializeField] private float spellStateCooldown;

    [Header("Tp details")]
    [SerializeField] private BoxCollider2D arema;
    [SerializeField] private Vector2 surroundingCheck;
    public float chanceToTp = 20f;
    public float defaultChanceToTp = 20f;
 
    protected override void Awake()
    {
        base.Awake();

        SetupDefailtFacingDir(-1);

        idleState = new BossIdleState(this,StateMachine,"Idle",this);
        battleState = new BossBattleState(this, StateMachine, "Move", this);
        TpState = new BossTpState(this, StateMachine, "Teleport", this);
        AttackState = new BossAttackState(this, StateMachine, "Attack", this);
        DeathState = new BossDeathState(this, StateMachine, "Idle", this);
        spellCastState = new BossSpellCastState(this, StateMachine, "SpellCast", this);
    }

    protected override void Start()
    {
        base.Start();

        StateMachine.Initialize(idleState);
    }

    public override void Die()
    {
        base.Die();
        StateMachine.ChangeState(DeathState);
    }

    public void CastSpell()
    {
        Player player = PlayerManager.instance.player;

        Vector3 spellPosition = new Vector3(player.transform.position.x + player.facingDir * 2, player.transform.position.y + 2f);

        if (player.rb.velocity.x == 0)
            spellPosition = new Vector3(player.transform.position.x, player.transform.position.y + 2f);

        GameObject newSpell = Instantiate(spellPrefab, spellPosition, Quaternion.identity);
        newSpell.GetComponent<BossSpell_Controller>().SetupSpell(stats);
    }

    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
    private bool SomethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheck, 0, Vector2.zero,0, whatIsGround);
    public void FindPosition()
    {
        float x =Random.Range(arema.bounds.min.x+3, arema.bounds.max.x-3);
        float y = Random.Range(arema.bounds.min.y+3, arema.bounds.max.y-3);

        transform.position = new Vector3(x, y);
        transform.position = new Vector3(transform.position.x, transform.position.y-GroundBelow().distance+(cd.size.y/2));

        if(!GroundBelow() || SomethingIsAround())
        {
            FindPosition();
        }
    }

   

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x,transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position,surroundingCheck);
    }

    public bool CanToTp()
    {
        if(Random.Range(0,100)<= chanceToTp)
        {
            chanceToTp = defaultChanceToTp;
            return true;

        }

        return false;
    }

    public bool CanDoSpellCast()
    {
        if (Time.time > lastTimeCast + spellStateCooldown)
        {

            return true;
        }
        return false;
    }
}
